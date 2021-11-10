using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Obstacle : MonoBehaviour
{
    randomCreate randomCreate;
    CameraFollow cameraFollow;
    SkinnedMeshRenderer skinnedMesh;

    public GameObject gem;
    public GameObject ghost;
    public Text failText;


    public GameObject confettiLeft;
    public GameObject confettiRight;

    bool i = false;
    bool o = false;

    bool oneTime = false;

    public List<GameObject> fdObject = new List<GameObject>();

    public UnityEngine.UI.Button nextLevel;
    public GameObject LevelCompletePanel;
    //public Image levelCompleteImage;


    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        skinnedMesh = gameObject.GetComponent<SkinnedMeshRenderer>();
        randomCreate = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<randomCreate>();
        cameraFollow = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();
        gem = GameObject.FindGameObjectWithTag("Gem");
		for (int i = 0; i < randomCreate.obstaclesInScene.Count; i++)
		{
            Transform fdobj = Instantiate(randomCreate.obstaclesInScene[i].transform, randomCreate.obstaclesInScene[i].transform.position, Quaternion.identity, randomCreate.obstaclesInScene[i].transform);
            fdobj.GetComponent<ObstacleColor>().enabled = false;
            fdobj.GetComponent<MeshRenderer>().material = randomCreate.obstaclesInScene[i].transform.GetComponent<MeshRenderer>().material;
            fdobj.gameObject.SetActive(false);
            fdObject.Add(fdobj.gameObject);
        }
        ghost.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
		if (skinnedMesh.GetBlendShapeWeight(0) <= 0)
		{
            skinnedMesh.SetBlendShapeWeight(0, 0f);
		}

        float x = skinnedMesh.GetBlendShapeWeight(0);
         
        Vector3 innerPos = transform.position + new Vector3(transform.position.x + x * 0.01f + 0.1f, 0, 0);

        Vector3 outerPos = transform.position + new Vector3(transform.position.x + x * 0.01f + 0.3f, 0, 0);



        RaycastHit hit;

        if (Physics.Raycast(innerPos, transform.TransformDirection(Vector3.down), out hit, .05f))
        {
			if (hit.transform.tag=="Obstacle")
			{
                Debug.DrawRay(innerPos, transform.TransformDirection(Vector3.down) * hit.distance, Color.yellow);
                i = true;
            }
		}
		else
		{
            i = false;
		}

        if (Physics.Raycast(outerPos, transform.TransformDirection(Vector3.down), out hit, .05f))
        {
            if (hit.transform.tag == "Obstacle")
            {
                Debug.DrawRay(outerPos, transform.TransformDirection(Vector3.down) * hit.distance, Color.yellow);
                o = true;
            }

        }
		else
		{
            o = false;
		}

		if (i || o)
		{
            StartCoroutine(PlayAgain());
		}

        //Gölgenin renk değişimi
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 5f))
        {
            bool a = false;
            bool b = false;
            ghost.SetActive(true);
            ghost.transform.position = hit.point;
            //ghost.GetComponent<MeshRenderer>().material.color = Color.red;
            if (Physics.Raycast(innerPos, transform.TransformDirection(Vector3.down), out hit, 5f))
            {
                Debug.DrawRay(innerPos, transform.TransformDirection(Vector3.down) * hit.distance, Color.yellow);
				if (hit.transform.tag == "Obstacle")
				{
                    a = true;
                }
                if (hit.transform.tag == "FinalTrigger")
                {
                    ghost.SetActive(false);
                }
            }
            else
            {
                a = false;
            }

            if (Physics.Raycast(outerPos, transform.TransformDirection(Vector3.down), out hit, 5f))
            {
                Debug.DrawRay(outerPos, transform.TransformDirection(Vector3.down) * hit.distance, Color.yellow);
                if (hit.transform.tag == "Obstacle")
                {
                    b = true;
                }
                if(hit.transform.tag == "FinalTrigger")
				{
                    ghost.SetActive(false);
				}
            }
            else
            {
                b = false;
            }

            if(a || b)
			{
                ghost.GetComponent<SkinnedMeshRenderer>().material.color = new Color(200, 0, 0, 0.15f);
            } else
			{
                ghost.GetComponent<SkinnedMeshRenderer>().material.color = new Color(0, 128, 0, 0.15f);
            }

        } 

        //Yavaslama
        if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), out hit, 5f))
		{
			if (hit.transform.tag == "Obstacle")
			{
                if (hit.transform.gameObject == randomCreate.obstaclesInScene[randomCreate.obstaclesInScene.Count - 1])
                {
                    Debug.Log("Sonuncuya Çarptı");
                    StartCoroutine(LevelComplete());
                    ghost.SetActive(false);
                }
                GameObject fadeObj = hit.transform.GetChild(0).gameObject;
                fadeObj.SetActive(true);
                fadeObj.GetComponent<MeshRenderer>().material = hit.transform.GetComponent<MeshRenderer>().material;
                iTween.MoveTo(fadeObj, iTween.Hash("y", fadeObj.transform.position.y+0.1f, "time", .35f));
                iTween.ScaleTo(fadeObj, iTween.Hash("x", 1.1f, "y", 1.1f, "z", 1.1f, "time", .35f));
                iTween.FadeTo(fadeObj, iTween.Hash("alpha", 0f, "time", .35f));
                StartCoroutine(CreateFading(fadeObj));
            }

        }


        //Final Trigger
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.left), out hit, 25f))
        {
			if (hit.transform.gameObject.tag == "FinalTrigger")
			{
                Debug.Log("Final Trigger");
                ghost.SetActive(false);
                confettiLeft.SetActive(true);
                confettiLeft.GetComponent<ParticleSystem>().Play();
                confettiRight.SetActive(true);
                confettiRight.GetComponent<ParticleSystem>().Play();
                //StartCoroutine(LevelComplete());
                cameraFollow.enabled = false;
            }
        }

        Vector3 leftpos = transform.position + new Vector3(transform.position.x + x * 0.01f + 0.1f, 0, 0);
        Vector3 rightpos = transform.position + new Vector3(transform.position.x - x * 0.01f + 0.1f, 0, 0);
        Vector3 backpos = transform.position + new Vector3(0, 0, transform.position.z + x * 0.01f + 0.1f);
        Vector3 fwdpos = transform.position + new Vector3(0, 0, transform.position.z - x * 0.01f + 0.1f);
        Vector3 lbpos = transform.position + new Vector3(transform.position.x + x * 0.01f + 0.1f, 0, transform.position.z + x * 0.01f - 0.1f);
        Vector3 lfpos = transform.position + new Vector3(transform.position.x + x * 0.01f - 0.1f, 0, transform.position.z - x * 0.01f + 0.1f);
        Vector3 rbpos = transform.position + new Vector3(transform.position.x - x * 0.01f - 0.1f, 0, transform.position.z + x * 0.01f + 0.1f);
        Vector3 rfpos = transform.position + new Vector3(transform.position.x - x * 0.01f + 0.1f, 0, transform.position.z - x * 0.01f - 0.1f);

        float dis0 = Vector3.Distance(leftpos,gem.transform.position);
        float dis1 = Vector3.Distance(rightpos, gem.transform.position);
        float dis2 = Vector3.Distance(backpos, gem.transform.position);
        float dis3 = Vector3.Distance(fwdpos, gem.transform.position);
        float dis4 = Vector3.Distance(lbpos, gem.transform.position);
        float dis5 = Vector3.Distance(lfpos, gem.transform.position);
        float dis6 = Vector3.Distance(rbpos, gem.transform.position);
        float dis7 = Vector3.Distance(rfpos, gem.transform.position);

		if (dis0<0.2f ||dis1<0.2f ||dis2<0.2f ||dis3<0.2f ||dis4<0.2f ||dis5<0.2f ||dis6<0.2f ||dis7<0.2f)
		{
            StartCoroutine(GemAlmak());
		}

    }

	public IEnumerator PlayAgain()
	{
        /*failText.gameObject.SetActive(true);
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(1f);
        Time.timeScale = 1f;
        //SceneManager.LoadScene(0);*/
        Debug.Log("Play again");
        transform.GetComponent<blendshape>().downSpeed = 0f;
        
        iTween.MoveTo(gameObject, iTween.Hash("y", transform.position.y + 1f, "time", 1f));
        yield return new WaitForSecondsRealtime(1f);
        transform.GetComponent<blendshape>().downSpeed = 2f;
    }

    public IEnumerator LevelComplete()
	{
        float t = 1f;
        t += 1f * Time.deltaTime;
        transform.GetComponent<blendshape>().downSpeed += Mathf.Lerp(2f, 7.5f, t);

        LevelCompletePanel.SetActive(true);
        yield return null;
    }

    public IEnumerator yavaslat()
	{
        transform.GetComponent<blendshape>().downSpeed = 1.25f;
        yield return new WaitForSecondsRealtime(.35f);
        transform.GetComponent<blendshape>().downSpeed = 2f;
    }

    public void NextLevel()
	{
        int sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        if (sceneName==4)
		{
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
		} else
		{
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName + 1);
        }

    }

    public IEnumerator CreateFading(GameObject gameObject)
	{
        yield return new WaitForSecondsRealtime(.35f);
        Destroy(gameObject);
    }

    public IEnumerator GemAlmak()
	{
        iTween.ScaleTo(gem, iTween.Hash("x",0,"y",0,"z",0,"time",0.5f));
        gem.transform.GetChild(1).gameObject.SetActive(true);
        Debug.Log("Gem Alindi");
        yield return new WaitForSecondsRealtime(1f);
        Destroy(gem.gameObject);
    }

}
