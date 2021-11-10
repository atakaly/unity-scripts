using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMove : MonoBehaviour
{

    GameObject kamera;
    public GameObject panel;

    Animator animator;

    public bool move, stop, climb, crawl, attack, push, pull, grappling, snowboard, jumptosnow;


    bool touched;

    public bool attackmove;


    bool ladderCreate = false;

    public float speed;

    public Transform ladder;
    public GameObject bagpack;

    public GameObject[] dominoes;
    public GameObject hortum;
    public GameObject bagLad;
    public GameObject pickaxe;
    public GameObject pickaxebag;

    public GameObject grappleGun;
    public GameObject grappleGunInBag;
    public GameObject grapple_bas;
    public GameObject grapple_bit;


    public GameObject leftConfetti;
    public GameObject rightConfetti;


    /// <summary>
    /// UIII
    /// </summary>
    /// 

    public GameObject failedPanel;
    public GameObject levelCompletedPanel;


    void Start()
    {
        animator = GetComponent<Animator>();
        kamera = GameObject.FindGameObjectWithTag("MainCamera");
        touched = false;
    }

    void FixedUpdate()
    {
		if (stop)
		{
            move = false;
            animator.SetBool("move", false);
            animator.SetBool("stop", true);
            speed = 0f;
		} else if(move)
		{
            stop = false;
            animator.SetBool("stop", false);
            animator.SetBool("move", true);

            //Hareketler
            speed = 3f;
            transform.position += Vector3.forward * speed * Time.deltaTime;
        } else
		{
            animator.SetBool("stop", false);
            animator.SetBool("move", false);
        }

		/*if (move)
		{
            //Animasyonlar
            

        */

		if (climb)
		{
            stop = false;
            move = false;
            StartCoroutine(Climb());
		}

		if (crawl)
		{
            Crawl();
        } else if (!crawl)
		{
            animator.SetBool("crawl", false);
		}
        if (attack)
		{
            StartCoroutine(Attack());
        }

		if (jumptosnow)
		{
            StartCoroutine(JumpOnSnow());
		}

		if (snowboard)
		{
            StartCoroutine(SnowBoard());
		}

		if (grappling)
		{
            StartCoroutine(Grappling());
		}

       

        if (Input.GetMouseButtonDown(0) && !touched)
		{
            move = true;
            stop = false;
            touched = true;
		}

		if (Input.GetKeyDown(KeyCode.E))
		{
           // StartCoroutine(PullDominoes());

        }
        if (push)
		{
            StartCoroutine(Push());
        }

        if (pull)
        {
            StartCoroutine(PullDominoes());
        }


    }

    void Crawl()
	{
        animator.SetBool("crawl", true);
        transform.position += Vector3.forward * 1f * Time.deltaTime;
    }

    IEnumerator Climb()
	{
        climb = false;
        panel.SetActive(false);
        bagLad.SetActive(false);
        if (ladderCreate == false)
		{
            CreateLadder();
            ladderCreate = true;
        }
        yield return new WaitForSecondsRealtime(2f);
        move = true;
        yield return new WaitForSeconds(1.5f);
        animator.SetBool("climb", true);
        ChangeCameraPos(new Vector3(2.67f, 3.67f, -7.87f), 0.05f);
        gameObject.GetComponent<Rigidbody>().useGravity = false;
        iTween.MoveTo(gameObject, iTween.Hash("path", iTweenPath.GetPath("climbLadder"), "time", 1.25f, "easetype", iTween.EaseType.linear));
        yield return new WaitForSeconds(2.2f);
        iTween.MoveTo(gameObject, iTween.Hash("path", iTweenPath.GetPath("climbend"), "time", 1.15f, "easetype", iTween.EaseType.linear));
        yield return new WaitForSeconds(3f);

        animator.SetBool("climb", false);
        gameObject.GetComponent<Rigidbody>().useGravity = true;
        move = true;
        stop = false;
        yield return new WaitForSeconds(2.5f);
        ChangeCameraPos(new Vector3(2.3f, 0.95f, -4.78f), 0.125f);



        StopCoroutine(Climb());

    }

    IEnumerator Attack()
	{
        attack = false;
        stop = false;
        panel.SetActive(false);
        //yield return new WaitForSecondsRealtime(2f);
        attackmove = true;
        move = true;
        yield return new WaitForSeconds(1.5f);

        pickaxe.SetActive(true);
        pickaxebag.SetActive(false);
        animator.SetBool("attack", true);
        //animator.SetBool("stop", false);
        yield return new WaitForSecondsRealtime(3f);
        Failed();


        

	}

    void CreateLadder()
	{
        Transform ladderGo = Instantiate(ladder, bagpack.transform.position, Quaternion.identity);
        iTween.ScaleTo(ladderGo.gameObject, iTween.Hash("x", 4.86, "y", 4.86, "z", 4.86, "time", 2f, "easetype", iTween.EaseType.easeOutQuint));
        iTween.MoveTo(ladderGo.gameObject, iTween.Hash("path", iTweenPath.GetPath("ladPath"), "time", 2f));
        iTween.RotateAdd(ladderGo.gameObject, iTween.Hash("x", 275f, "time",2f, "easetype", iTween.EaseType.easeOutQuint));

    }

    void ChangeCameraPos(Vector3 vectors, float smooth)
	{
        kamera.GetComponent<CameraController>().smoothSpeed = smooth;
        kamera.GetComponent<CameraController>().offset = vectors;
    }

    IEnumerator Push()
	{
        panel.SetActive(false);

        animator.SetBool("push",true);
        ChangeCameraPos(new Vector3(2.67f, 3.67f, -7.87f), 0.05f);
        yield return new WaitForSeconds(5f);
        move = true;
        stop = false;
        ChangeCameraPos(new Vector3(2.3f, 0.95f, -4.78f), 0.05f);
        push = false;
        animator.SetBool("push", false);
        StopCoroutine(Push());
	}

    IEnumerator PullDominoes()
	{
        panel.SetActive(false);

        ChangeCameraPos(new Vector3(2.67f, 3.67f, -7.87f), 0.05f);
        yield return new WaitForSecondsRealtime(0.5f);
        pull = false;

        hortum.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, 100f);
        hortum.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(1, 0);
        for (int i = 0; i < dominoes.Length; i++)
		{
            dominoes[i].GetComponent<Rigidbody>().useGravity = false;
            iTween.MoveTo(dominoes[i].gameObject, iTween.Hash("position", hortum.transform.position, "time", .35f, "easetype", iTween.EaseType.linear));
            iTween.ScaleTo(dominoes[i].gameObject, iTween.Hash("x", 0f, "y", 0f, "z", 0f, "time", .75f, "easetype", iTween.EaseType.linear));
            iTween.RotateTo(dominoes[i].gameObject, iTween.Hash("z", -90f, "time", .75f, "easetype", iTween.EaseType.linear));
            yield return new WaitForSeconds(0.75f);
            iTween.PunchScale(bagpack.gameObject, iTween.Hash("amount", new Vector3(0.5f,0.5f,0.5f), "time", 0.1f, "easetype", iTween.EaseType.linear));
		}
        yield return new WaitForSecondsRealtime(0.25f);
        hortum.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(1, 100f);
        hortum.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, 0f);
        ChangeCameraPos(new Vector3(2.3f, 0.95f, -4.78f), 0.125f);
        StopCoroutine(PullDominoes());
        move = true;
        stop = false;
        yield return new WaitForSeconds(1.5f);
        Failed();

    }

    IEnumerator Grappling()
	{
        panel.SetActive(false);
        move = true;
        stop = false;
        yield return new WaitForSeconds(1.5f);
        grappling = false;
        ChangeCameraPos(new Vector3(1.5f, -0.1f, -1.93f), 0.05f);
        animator.SetBool("grappling", true);
        transform.position = animator.rootPosition;
        yield return new WaitForSeconds(1.2f);

        grapple_bas.SetActive(true);
        grapple_bit.SetActive(true);
        grappleGun.transform.GetChild(1).gameObject.SetActive(false);
        iTween.MoveTo(grapple_bit, iTween.Hash("path", iTweenPath.GetPath("grapplingrope"), "time", 2f));
        yield return new WaitForSeconds(1.8f);
        ChangeCameraPos(new Vector3(2.3f, 0.95f, -4.78f), 0.125f);
        yield return new WaitForSeconds(2.4f);
      //  grapple_bas.transform.SetParent(null);
        animator.SetBool("grappling", false);
        transform.position = new Vector3(transform.position.x, transform.position.y, 7.5f);
        GetComponent<Rigidbody>().useGravity = false;
        iTween.MoveTo(gameObject, iTween.Hash("path", iTweenPath.GetPath("ropeclimb"), "time", 4.5f, "easetype", iTween.EaseType.linear));
        yield return new WaitForSecondsRealtime(4.5f);

        grapple_bas.SetActive(false);
        grapple_bit.SetActive(false);

        animator.SetBool("endofclimb", true);

        GetComponent<Rigidbody>().useGravity = true;

        move = true;
        stop = false;
        //transform.position += Vector3.up * Time.deltaTime * 5f;
        //move = true;
        //stop = false;
    }

    IEnumerator JumpOnSnow()
	{
        jumptosnow = false;
        panel.SetActive(false);

        yield return null;
        GetComponent<Rigidbody>().useGravity = false;
        animator.SetBool("jumptosnow", true);
        iTween.MoveTo(gameObject, iTween.Hash("path", iTweenPath.GetPath("jumptosnow"), "time", 2f, "easetype", iTween.EaseType.linear));
        yield return new WaitForSeconds(2f);
        GetComponent<Rigidbody>().useGravity = true;
    }

    IEnumerator SnowBoard()
	{
        snowboard = false;
        panel.SetActive(false);
        yield return null;
        animator.SetBool("snowboard", true);
        //iTween.MoveAdd(gameObject, iTween.Hash("z", 5f, "time", 1.5f));
    }

    public void Finish()
    {
        move = false;
        stop = true;
        animator.SetBool("finish", true);
        leftConfetti.SetActive(true);
        rightConfetti.SetActive(true);
        leftConfetti.GetComponent<ParticleSystem>().Play();
        rightConfetti.GetComponent<ParticleSystem>().Play();
        levelCompletedPanel.SetActive(true);
    }

    public void Failed()
	{
        failedPanel.SetActive(true);
	}

    public void Retry()
	{
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

    public void NextLevel()
	{
		if (SceneManager.GetActiveScene().buildIndex==1)
		{
            SceneManager.LoadScene(0);
        } else
		{
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

}
