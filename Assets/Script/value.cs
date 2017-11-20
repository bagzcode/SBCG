using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Data; 
using System;
using Mono.Data.SqliteClient; 
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.EventSystems;


public class value : MonoBehaviour {

	public int no_card;
	
	dbAccess db;

	public IDataReader infonumber; 
	public IDataReader problemnumber;

	private int maxNumbers;
	private List<int> uniqueNumber;
	private List <int> finishedList;

	public void InfoCardOnClick()
	{
	//	Debug.Log (EventSystem.current.currentSelectedGameObject.GetComponent<value>().no_card);
		db = GameObject.Find("database").GetComponent<dbAccess>();
		db.OpenDB("mydb.sdb");

		infonumber = db.BasicQuery ("SELECT COUNT(id) FROM tbl_Info_Card;");
		infonumber.Read ();
		maxNumbers = infonumber.GetInt32 (0);

		maxNumbers = checkmaxnumber(maxNumbers, 12);
		hidelistcard (maxNumbers, "btninfocard");

		int selectednumber = EventSystem.current.currentSelectedGameObject.GetComponent<value>().no_card;
		//Debug.Log (selectednumber);
		ArrayList result = db.SingleSelectWhere ("tbl_Info_Card", "*", "id", "=", selectednumber.ToString());
		string desc = ((string[])result[0])[1];
		string reward = ((string[])result[0])[2];
		GameObject.Find ("Content").GetComponent<Text> ().text = desc;
		GameObject.Find ("Reward").GetComponent<Text> ().text = reward;
		showcard ();
		// insert new data to history
		db.CloseDB();
	}

	public void ProblemCardOnClick()
	{
		//Debug.Log (EventSystem.current.currentSelectedGameObject.GetComponent<value>().no_card);
		db = GameObject.Find("database").GetComponent<dbAccess>();
		db.OpenDB("mydb.sdb");

		problemnumber = db.BasicQuery ("SELECT COUNT(id) FROM tbl_Problem_Card;");
		problemnumber.Read ();
		maxNumbers = problemnumber.GetInt32 (0);

		maxNumbers = checkmaxnumber(maxNumbers, 28);
		hidelistcard(maxNumbers, "btnProblemcard");

		int selectednumber = EventSystem.current.currentSelectedGameObject.GetComponent<value>().no_card;
		//Debug.Log (selectednumber);
		ArrayList result = db.SingleSelectWhere("tbl_Problem_Card", "*", "id", "=", selectednumber.ToString());
		string desc = ((string[])result[0])[1];
		string reward = ((string[])result[0])[2];
		GameObject.Find ("Content").GetComponent<Text> ().text = desc;
		GameObject.Find ("Reward").GetComponent<Text> ().text = reward;
		showcard ();
	}

	public void AcceptOnClick()
	{
		//GameObject.Find ("EventSystem").SetActive (false);
		SceneManager.LoadScene("Main", LoadSceneMode.Additive);
	}

	void showcard()
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

	void hidecard()
	{
		GameObject.Find ("Card Background").GetComponent<Image>().enabled = false;
		GameObject.Find ("Title").GetComponent<Text>().enabled = false;
		GameObject.Find ("Line").GetComponent<RawImage>().enabled = false;
		GameObject.Find ("Content").GetComponent<Text>().enabled = false;
		GameObject.Find ("Reward").GetComponent<Text>().enabled = false;
		GameObject.Find ("btnAccept").GetComponent<Image>().enabled = false;
		GameObject.Find ("btnAccept").GetComponentInChildren<Text> ().enabled = false; 
	}
		

	void hidelistcard(int number, string btn)
	{
		for (int i = 1; i <= number; i++) {
			GameObject.Find (btn + i).GetComponent<Image> ().enabled = false;
			GameObject.Find (btn + i).GetComponentInChildren<Text> ().enabled = false;
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
