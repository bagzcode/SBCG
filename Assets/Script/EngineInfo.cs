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
	void Start () {
		SceneManager.UnloadScene("Main");

		hideinfocard ();
		db = GameObject.Find("database").GetComponent<dbAccess>();
		db.OpenDB("mydb.sdb");

		infonumber = db.BasicQuery ("SELECT COUNT(id) FROM tbl_Info_Card;");
		infonumber.Read ();
		maxNumbers = infonumber.GetInt32 (0);
		Debug.Log (maxNumbers);

		uniqueNumber = new List<int>();
		finishedList = new List<int>();

		putInfoCard (maxNumbers);
		AssignedValue ();
		db.CloseDB();
	}


	void showinfocard()
	{
		GameObject.Find ("Card Background").GetComponent<Image>().enabled = true;
		GameObject.Find ("Title").GetComponent<Text>().enabled = true;
		GameObject.Find ("Line").GetComponent<RawImage>().enabled = true;
		GameObject.Find ("Content").GetComponent<Text>().enabled = true;
		GameObject.Find ("Reward").GetComponent<Text>().enabled = true;
		GameObject.Find ("btnAccept").GetComponent<Image>().enabled = true;
		GameObject.Find ("btnAccept").GetComponent<Image>().color = Color.red;
		GameObject.Find ("btnAccept").GetComponentInChildren<Text> ().enabled = true; 
	}

	void hideinfocard()
	{
		GameObject.Find ("Card Background").GetComponent<Image>().enabled = false;
		GameObject.Find ("Title").GetComponent<Text>().enabled = false;
		GameObject.Find ("Line").GetComponent<RawImage>().enabled = false;
		GameObject.Find ("Content").GetComponent<Text>().enabled = false;
		GameObject.Find ("Reward").GetComponent<Text>().enabled = false;
		GameObject.Find ("btnAccept").GetComponent<Image>().enabled = false;
		GameObject.Find ("btnAccept").GetComponentInChildren<Text> ().enabled = false; 
	}
		

	void hidelistinfocard(int number)
	{
		for (int i = 1; i <= number; i++) {
			GameObject.Find ("btninfocard" + i).GetComponent<Image> ().enabled = false;
			GameObject.Find ("btninfocard" + i).GetComponentInChildren<Text> ().enabled = false;
		}
	}

	void showlistinfocard(int number)
	{
		for (int i = 1; i <= number; i++) {
			GameObject.Find ("btninfocard" + i).GetComponent<Image> ().enabled = true;
			GameObject.Find ("btninfocard" + i).GetComponentInChildren<Text> ().enabled = true;
			Button b = GameObject.Find ("btninfocard" + i).GetComponent<Button>(); 
			ColorBlock cb = b.colors;
			cb.normalColor = Color.blue;
			b.colors = cb;
		}
	}

	void AssignedValue ()
	{
		for(int i = 0; i < maxNumbers; i++){
			uniqueNumber.Add(i+1);
		}
		int number;
		for(int i = 0; i < maxNumbers; i ++){
			number = i + 1;
			int ranNum = uniqueNumber[UnityEngine.Random.Range(0,uniqueNumber.Count)];
			//Debug.Log ("r "+ranNum);
			finishedList.Add(ranNum);
			uniqueNumber.Remove (ranNum);
			GameObject.Find ("btninfocard" + number).GetComponent<value> ().no_card = ranNum;
		} 
	}

	/*
	 * x................................|.....y
	-550  -300  -100  0  100  300  500  |  120
	-540  -360  -180  0  100  300  500  |  -120
	*/
	int[] x,y;
	void putInfoCard(int maxval)//12
	{
		x = new int[maxval];
		y = new int[maxval];
		int a = 0;
		for (int i = 1; i <=maxval; i++)
		{
			a = i - 1;
			if ((i >= 1) && (i <= 4))
			{
				if (i % 2 == 0)
					x [a] = 100;
				else
					x [a] = -100;
			}
			else if ((i >= 5) && (i <= 8))
			{
				if (i % 2 == 0)
					x [a] = 300;
				else
					x [a] = -300;
			}
			else 
			{
				if (i % 2 == 0)
					x [a] = 500;
				else
					x [a] = -500;
			}

			if ((i == 1) || (i == 2) || (i == 5) || (i == 6) || (i == 9) || (i == 10))
				y [a] = 120;
			else 
				y [a] = -120;
			//Debug.Log (x[a]+", "+y[a]);
			
			GameObject btnInfoCard = Instantiate(Resources.Load("btnInfoCard", typeof(GameObject)), new Vector3(x [a]+640, y [a]+400, 0), Quaternion.identity, GameObject.Find("InfoScreen").transform) as GameObject; 
			btnInfoCard.name = "btninfocard"+i;
		}
	}

}
