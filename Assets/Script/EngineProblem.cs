using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Mono.Data.Sqlite; 
using System.Data; 
using System;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class EngineProblem : MonoBehaviour {

	dbAccess db;

	private IDataReader problemnumber; 

	private int maxNumbers;
	private List <int> uniqueNumber;
	private List <int> finishedList;

	public string selectedcard;

	// Use this for initialization
	public void LoadProblemScene () {
		hideproblemcard ();
		maxNumbers = 10;
		uniqueNumber = new List<int>();
		finishedList = new List<int>();
		AssignedValue ();
	}

	void hideproblemcard()
	{
		GameObject.Find ("CardBackgroundP").GetComponent<Image>().enabled = false;
		GameObject.Find ("CardBackgroundi").GetComponent<Image>().enabled = false;
	}

	void AssignedValue ()
	{
		for(int i = 0; i < maxNumbers; i++){
			uniqueNumber.Add(i+1);
		}
		int number;
		maxNumbers = checkmaxnumber (maxNumbers,3); 
		for(int i = 0; i < maxNumbers; i ++){
			number = i + 1;
			int ranNum = uniqueNumber[UnityEngine.Random.Range(0,uniqueNumber.Count)];
			//Debug.Log ("r "+ranNum);
			finishedList.Add(ranNum);
			uniqueNumber.Remove (ranNum);
			selectedcard = "p"+ranNum;
			GameObject.Find("CardBackgroundP").GetComponent<Image>().sprite = Resources.Load<Sprite>(selectedcard);
		} 
	}

	int checkmaxnumber(int number, int limit)
	{
		if (number > limit)
			return limit;
		else 
			return number;
	}


}
