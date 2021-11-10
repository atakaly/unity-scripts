using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class blendshape : MonoBehaviour
{

    #region oldcode 
    float speed = 50f;
    public float downSpeed = 2f;

	public bool left;
	public bool right;

	SkinnedMeshRenderer skinnedMesh;

    private Vector2 fingerDown;
    private Vector2 fingerUp;
    public bool detectSwipeOnlyAfterRelease = false;

    public float SWIPE_THRESHOLD = 1f;


    private void Awake()
	{
		Application.targetFrameRate = 60;
	}

	void Start()
    {
        skinnedMesh = gameObject.GetComponent<SkinnedMeshRenderer>();
    }

	private void FixedUpdate()
	{
		transform.position += new Vector3(0, -downSpeed * Time.deltaTime, 0);

        if (speed < 100f)
        {
            if (right == true)
            {
                speed += 2f;
                skinnedMesh.SetBlendShapeWeight(0, speed);
            }
        }
        if (speed > 0f)
        {
            if (left == true)
            {
                speed -= 2f;
                skinnedMesh.SetBlendShapeWeight(0, speed);
            }
        }


    }

    void Update()
	{
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                fingerUp = touch.position;
                fingerDown = touch.position;
            }

            if (touch.phase == TouchPhase.Moved)
            {
                if (!detectSwipeOnlyAfterRelease)
                {
                    fingerDown = touch.position;
                    checkSwipe();
                }
            }

            if (touch.phase == TouchPhase.Ended)
            {
                fingerDown = touch.position;
                checkSwipe();
                right = false;
                left = false;
            }

			if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Canceled)
			{
                fingerDown = touch.position;
                checkSwipe();
                right = false;
                left = false;
            }
        }

		if (skinnedMesh.GetBlendShapeWeight(0) >= 100f)
		{
			skinnedMesh.SetBlendShapeWeight(0, 100f);
		}
		if (skinnedMesh.GetBlendShapeWeight(0) <= 0f)
		{
			skinnedMesh.SetBlendShapeWeight(0, 0);
		}


        void checkSwipe()
        {

            //Check if Horizontal swipe
            if (horizontalValMove() > SWIPE_THRESHOLD)
            {
                //Debug.Log("Horizontal");
                if (fingerDown.x - fingerUp.x > 0)//Right swipe
                {
                    OnSwipeRight();
                }
                else if (fingerDown.x - fingerUp.x < 0)//Left swipe
                {
                    OnSwipeLeft();
                }
                fingerUp = fingerDown;
            }

            //No Movement at-all
            else
            {
                //Debug.Log("No Swipe!");
            }
        }

        float horizontalValMove()
        {
            return Mathf.Abs(fingerDown.x - fingerUp.x);
        }



        void OnSwipeLeft()
        {
            right = false;
            left = true;
        }

        void OnSwipeRight()
        {
            right = true;
            left = false;
        }
    }
    #endregion

}
