using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Data; 
using System;
using Mono.Data.SqliteClient; 
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class EngineInfo : MonoBehaviour {

	dbAccess db;

	private IDataReader infonumber; 

	private int maxNumbers;
	private List<int> uniqueNumber;
	private List <int> finishedList;
	
	// Use this for initialization
	public void LoadInfoScene () {
		
		hideinfocard ();
		db = GameObject.Find("database").GetComponent<dbAccess>();

		infonumber = db.BasicQuery ("SELECT COUNT(id) FROM tbl_Info_Card;");
		infonumber.Read ();
		maxNumbers = infonumber.GetInt32 (0);
		//Debug.Log (maxNumbers);
		
		uniqueNumber = new List<int>();
		finishedList = new List<int>();
		AssignedValue ();
	}


	void showinfocard()
	{
		GameObject.Find ("CardBackgroundi").GetComponent<Image>().enabled = true;
		GameObject.Find ("Content").GetComponent<Text>().enabled = true;
		GameObject.Find ("Reward").GetComponent<Text>().enabled = true;
		GameObject.Find ("btnTrue").GetComponent<Image>().enabled = true;
		GameObject.Find ("btnFalse").GetComponent<Image>().enabled = true;
	}

	void hideinfocard()
	{
		GameObject.Find ("CardBackgroundi").GetComponent<Image>().enabled = false;
		GameObject.Find ("CardBackgroundP").GetComponent<Image>().enabled = false;
		GameObject.Find ("Content").GetComponent<Text>().enabled = false;
		GameObject.Find ("Reward").GetComponent<Text>().enabled = false;
		GameObject.Find ("btnTrue").GetComponent<Image>().enabled = false;
		GameObject.Find ("btnFalse").GetComponent<Image>().enabled = false;
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
			GameObject.Find ("btninfocard" + number).GetComponent<value> ().no_card = ranNum;
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
