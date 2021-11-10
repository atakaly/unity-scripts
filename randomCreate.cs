using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomCreate : MonoBehaviour
{
    public Transform[] obs;
	public Transform finalTrigger;
	public Transform gem;
	public int obsCount=10;

	public List<GameObject> obstaclesInScene = new List<GameObject>();

	public void Start()
	{
		Create();
	}

	public void Create()
	{

		Vector3 lastPos = new Vector3(0, 0, 0);
		Vector3 posDifference = new Vector3(0, -5, 0);

		int rand = UnityEngine.Random.Range(0,5);

		for (int i = 0; i < obsCount; i++)
		{
			int random = UnityEngine.Random.Range(0, obs.Length);

			Transform obstacle = Instantiate(obs[random], lastPos, Quaternion.identity);

			lastPos = lastPos + posDifference;

			obstaclesInScene.Add(obstacle.gameObject);


		}

		for (int i = 0; i < 1; i++)
		{
			Transform finalTrig = Instantiate(finalTrigger, lastPos + new Vector3(0,-1f,0), Quaternion.Euler(-90,90,0));

			//obstaclesInScene.Add(finalTrig.gameObject);

		}

		float randomXZ = UnityEngine.Random.Range(-.5f,.5f);
		float[] yAxis = new float[] { -2.5f, -7.5f, -12.5f, -17.5f, -22.5f, -27.5f, -32.5f };


		for (int i = 0; i < 1; i++)
		{
			Transform gemTransform = Instantiate(gem, new Vector3(randomXZ,yAxis[rand],randomXZ), Quaternion.Euler(-90,0,0));
		}
	}
}
