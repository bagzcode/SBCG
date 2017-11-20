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

	// Use this for initialization
	void Start () {
		SceneManager.UnloadScene("Main");

		hideproblemcard ();
		db = GameObject.Find("database").GetComponent<dbAccess>();
		db.OpenDB("mydb.sdb");

		problemnumber = db.BasicQuery ("SELECT COUNT(id) FROM tbl_Problem_Card;");
		problemnumber.Read ();
		maxNumbers = problemnumber.GetInt32 (0);


		uniqueNumber = new List<int>();
		finishedList = new List<int>();

		putProblemCard (maxNumbers);
		AssignedValue ();
		db.CloseDB();
	}


	void showproblemcard()
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

	void hideproblemcard()
	{
		GameObject.Find ("Card Background").GetComponent<Image>().enabled = false;
		GameObject.Find ("Title").GetComponent<Text>().enabled = false;
		GameObject.Find ("Line").GetComponent<RawImage>().enabled = false;
		GameObject.Find ("Content").GetComponent<Text>().enabled = false;
		GameObject.Find ("Reward").GetComponent<Text>().enabled = false;
		GameObject.Find ("btnAccept").GetComponent<Image>().enabled = false;
		GameObject.Find ("btnAccept").GetComponentInChildren<Text> ().enabled = false; 
	}


	void hidelistproblemcard(int number)
	{
		for (int i = 1; i <= number; i++) {
			GameObject.Find ("btnProblemcard" + i).GetComponent<Image> ().enabled = false;
			GameObject.Find ("btnProblemcard" + i).GetComponentInChildren<Text> ().enabled = false;
			Debug.Log ("hide");
		}
	}

	void showlistproblemcard(int number)
	{
		for (int i = 1; i <= number; i++) {
			GameObject.Find ("btnProblemcard" + i).GetComponent<Image> ().enabled = true;
			GameObject.Find ("btnProblemcard" + i).GetComponentInChildren<Text> ().enabled = true;
			Button b = GameObject.Find ("btnProblemcard" + i).GetComponent<Button>(); 
			ColorBlock cb = b.colors;
			cb.normalColor = Color.red;
			b.colors = cb;
		}
	}

	void AssignedValue ()
	{
		for(int i = 0; i < maxNumbers; i++){
			uniqueNumber.Add(i+1);
		}
		int number;
		maxNumbers = checkmaxnumber (maxNumbers,28); 
		for(int i = 0; i < maxNumbers; i ++){
			number = i + 1;
			int ranNum = uniqueNumber[UnityEngine.Random.Range(0,uniqueNumber.Count)];
			//Debug.Log ("r "+ranNum);
			finishedList.Add(ranNum);
			uniqueNumber.Remove (ranNum);
			GameObject.Find ("btnProblemcard" + number).GetComponent<value> ().no_card = ranNum;
		} 


	}

	/*
	 * x................................|.....y
	-540  -360  -180  0  180  360  540  |  175
	-540  -360  -180  0  180  360  540  |  40
	-540  -360  -180  0  180  360  540  |  -95
	-540  -360  -180  0  180  360  540  |  -230
	*/
	int[] x,y;
	void putProblemCard(int maxval)//28
	{
		maxval = checkmaxnumber (maxval,28);
		x = new int[maxval];
		y = new int[maxval];
		int a = 0;
		for (int i = 1; i <=maxval; i++)
		{
			a = i - 1;
			if ((i == 1) || (i == 4) || (i == 7) || (i == 10))
				x [a] = -180;
			else if ((i == 2) || (i == 5) || (i == 8) || (i == 11))
				x [a] = 180;
			else if ((i == 3) || (i == 6) || (i == 9) || (i == 12))
				x [a] = 0;	
			else if ((i == 13) || (i == 15) || (i == 17) || (i == 19))
				x [a] = -360;
			else if ((i == 14) || (i == 16) || (i == 18) || (i == 20))
				x [a] = 360;
			else if ((i == 21) || (i == 23) || (i == 25) || (i == 27))
				x [a] = -540;	
			else if ((i == 22) || (i == 24) || (i == 26) || (i == 28))
				x [a] = 540;

			if (((i >= 1) && (i <= 3)) || ((i >= 13) && (i <= 14)) || ((i >= 21) && (i <= 22)))
				y [a] = 40;
			else if (((i >= 4) && (i <= 6)) || ((i >= 15) && (i <= 16)) || ((i >= 23) && (i <= 24)))
				y [a] = -95;
			else if (((i >= 7) && (i <= 9)) || ((i >= 17) && (i <= 18)) || ((i >= 25) && (i <= 26)))
				y [a] = 175;
			else if (((i >= 10) && (i <= 12)) || ((i >= 19) && (i <= 20)) || ((i >= 27) && (i <= 28)))
				y [a] = -230;
		
			GameObject btnProblemCard = Instantiate(Resources.Load("btnProblemCard"), new Vector3(x [a]+640, y [a]+400, 0), Quaternion.identity, GameObject.Find("ProblemScreen").transform) as GameObject;
			btnProblemCard.name = "btnProblemcard"+i;


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
