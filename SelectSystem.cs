using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelectSystem : MonoBehaviour
{
	public int choiceCount;

	public List<Button> choiceButtons = new List<Button>();
	public List<string> trigTypes = new List<string>();

	public string currentTrigType;

	public void Start()
	{
		choiceCount = 2;
	}

	public void click()
	{
		if (EventSystem.current.currentSelectedGameObject.GetComponent<ButtonIndex>().trigTypeName == currentTrigType)
		{
			//Doğru Seçim
			Debug.Log("doğru seçim");
			chooseStyle();
		}
		else
		{
			//Yanlış Seçim
			Debug.Log("yanlış seçim");
			chooseStyle();
		}
	}

	public void chooseStyle()
	{
		switch (EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text>().text)
		{
			case "climb":
				GetComponent<PlayerMove>().climb = true;
				break;
			case "crawl":
				GetComponent<PlayerMove>().crawl = true;
				break;
			case "attack":
				GetComponent<PlayerMove>().attack = true;
				break;
			case "push":
				GetComponent<PlayerMove>().push = true;
				break;
			case "pull":
				GetComponent<PlayerMove>().pull = true;
				break;
			case "grappling":
				GetComponent<PlayerMove>().grappling = true;
				break;
			case "snowboard":
				GetComponent<PlayerMove>().snowboard = true;
				break;
			case "jumptosnow":
				GetComponent<PlayerMove>().jumptosnow = true;
				break;
			default:
				break;
		}
	}
}
