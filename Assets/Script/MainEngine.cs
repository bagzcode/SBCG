﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Data;
using Mono.Data.SqliteClient;
using UnityEngine.SceneManagement;

public class MainEngine : MonoBehaviour {

	string session_id;
	private string[] registerdices = {"diceone","dicetwo","dicethree","dicefour","dicefive","dicesix"};
	private int diceNumber;
	private int number = 0;
	string active_number = "1";
	int nextactive = 1;
	private IDataReader playernumber; 
	public ArrayList playernum;
	private string activesessionid;
	public int[] avataractive;

	//dbAccess db;
	dbAccess db;
	//StartGame defvalueavatar;

	// Use this for initialization to connect database
	void Start () {

		SceneManager.UnloadScene("New");
		SceneManager.UnloadScene("Info");
		SceneManager.UnloadScene("Problem");

		activesessionid = GameObject.Find("EventSystem").GetComponent<StartGame>().active_session_id;
		avataractive = GameObject.Find("EventSystem").GetComponent<StartGame>().avataractive;


		//db = GetComponent<dbAccess>();
		db = GameObject.Find("database").GetComponent<dbAccess>();
		db.OpenDB("mydb.sdb");
		ArrayList result = db.SelectLastRecord ("id", "tbl_Sessions");
	
		session_id = ((string[])result[0])[0];
		GameObject.Find("EventSystem").GetComponent<StartGame>().active_session_id = session_id;
		db.InsertHistory("Game Starts", session_id);
		//Debug.Log (session_id);
		playernumber = db.BasicQuery ("SELECT id FROM tbl_players WHERE tbl_Sessions_id=" + session_id);
		playernum = new ArrayList();
     	while(playernumber.Read ()){
     		for (var i = 0; i < playernumber.FieldCount; i++)
     		playernum.Add(playernumber.GetValue(i)); // This reads the entries in a row
     	}

		PlayerOnClick (1);
		GameObject.Find("BtnPlayer"+nextactive).GetComponent<Image>().color = Color.blue;
	}

	void OnApplicationQuit(){
		//db.CloseDB();
		Debug.Log ("Application Close");
	}

	//private functions
	// Use this for initialization
	public void btnRollDiceOnClick () 
	{
		InvokeRepeating("GetDiceValue", 0.0f, 0.1f);
		int nextactive = int.Parse(active_number) + 1;
		if (nextactive == 5)
			nextactive = 1;
		//Debug.Log ("BtnPlayer" + nextactive);
		GameObject.Find ("BtnPlayer" + active_number).GetComponent<Image>().color = Color.black;
		GameObject.Find("BtnPlayer"+nextactive).GetComponent<Image>().color = Color.blue;
		active_number = nextactive.ToString();
	}

	public void btnInfoOnClick () 
	{
		db.CloseDB();

		//GameObject.Find ("EventSystem").SetActive (false);
		SceneManager.LoadScene("info", LoadSceneMode.Additive);
		//SceneManager.LoadScene("Untitiled", LoadSceneMode.Additive);
	}

	public void btnProblemOnClick () 
	{
		db.CloseDB();

		//GameObject.Find ("EventSystem").SetActive (false);
		SceneManager.LoadScene("Problem", LoadSceneMode.Additive);
		//SceneManager.LoadScene("Untitiled", LoadSceneMode.Additive);
	}

	void GetDiceValue()
	{
		number += 1;
		diceNumber = UnityEngine.Random.Range (1, 7);
		if (diceNumber == 1) 
			GetDiceActive(registerdices[0]);
		else if (diceNumber == 2)
			GetDiceActive(registerdices[1]);
		else if (diceNumber == 3)
			GetDiceActive(registerdices[2]);
		else if (diceNumber == 4)
			GetDiceActive(registerdices[3]);
		else if (diceNumber == 5)
			GetDiceActive(registerdices[4]);
		else
			GetDiceActive(registerdices[5]);
		
		if (number == 15) 
		{
			CancelInvoke ();
			number = 0;
		}
	}

	void GetDiceActive(string dice)
	{
		DiceDisable ();
		GameObject.Find (dice).GetComponent<RawImage> ().enabled = true;
	}

	void DiceDisable()
	{
		for (int i = 0; i <= 5; i++)
			GameObject.Find (registerdices[i]).GetComponent<RawImage> ().enabled = false;
	}

	public void Player1onClick () 
	{
		PlayerOnClick (1);
	}

	public void Player2onClick () 
	{
		PlayerOnClick (2);
	}

	public void Player3onClick () 
	{
		PlayerOnClick (3);
	}

	public void Player4onClick () 
	{
		PlayerOnClick (4);
	}

	void PlayerOnClick (int number)
	{
		//Debug.Log (playernum+":"+number);
		//if (playernum >= int.Parse(number)) {
			ArrayList resultstart = db.SelectJoinTable ("tbl_Users.Name, tbl_Players.*", "tbl_Players INNER JOIN tbl_Users", "tbl_Users.id=tbl_Players.tbl_Users_id AND tbl_Players.tbl_Sessions_id=" + session_id + " AND tbl_Players.id=" + playernum[number-1]);
			//Debug.Log (((string[])resultstart [0]) [2]);
			SetValueNum (resultstart);
			disableAvatar ();
			active_number = ((string[])resultstart [0]) [2];
			string avatar = "Avatar" + active_number.ToString ();
			GameObject.Find (avatar).GetComponent<RawImage> ().enabled = true;
		//}
	}

	void SetValueNum(ArrayList result)
	{
		//Debug.Log (reader.GetInt32 (3).ToString());
		GameObject.Find ("numPlayerName").GetComponent<Text>().text = ((string[])result[0])[0];
		GameObject.Find ("numMoney").GetComponent<Text>().text = ((string[])result[0])[3];
		GameObject.Find ("numLevel").GetComponent<Text>().text = ((string[])result[0])[4];
		GameObject.Find ("numReward").GetComponent<Text>().text = ((string[])result[0])[5];
		GameObject.Find ("numPCpoverty").GetComponent<Text>().text = ((string[])result[0])[6];
		GameObject.Find ("numPChealth").GetComponent<Text>().text = ((string[])result[0])[7];
		GameObject.Find ("numPCeducation").GetComponent<Text>().text = ((string[])result[0])[8];
		GameObject.Find ("numPCtechnology").GetComponent<Text>().text = ((string[])result[0])[9];
		GameObject.Find ("numPCenvironment").GetComponent<Text>().text = ((string[])result[0])[10];
		GameObject.Find ("numFFood").GetComponent<Text>().text = ((string[])result[0])[11];
		GameObject.Find ("numFVitamin").GetComponent<Text>().text = ((string[])result[0])[12];
		GameObject.Find ("numFCard").GetComponent<Text>().text = ((string[])result[0])[13];
		GameObject.Find ("numFBags").GetComponent<Text>().text = ((string[])result[0])[14];
		GameObject.Find ("numFCloth").GetComponent<Text>().text = ((string[])result[0])[15];
		GameObject.Find ("numLFood").GetComponent<Text>().text = ((string[])result[0])[16];
		GameObject.Find ("numLVitamin").GetComponent<Text>().text = ((string[])result[0])[17];
		GameObject.Find ("numLCard").GetComponent<Text>().text = ((string[])result[0])[18];
		GameObject.Find ("numLBags").GetComponent<Text>().text = ((string[])result[0])[19];
		GameObject.Find ("numLCloth").GetComponent<Text>().text = ((string[])result[0])[20];
		GameObject.Find ("numA").GetComponent<Text>().text = ((string[])result[0])[21];
		GameObject.Find ("numB").GetComponent<Text>().text = ((string[])result[0])[22];
		GameObject.Find ("numC").GetComponent<Text>().text = ((string[])result[0])[23];
		GameObject.Find ("numPFood").GetComponent<Text>().text = ((string[])result[0])[24];
		GameObject.Find ("numPVitamin").GetComponent<Text>().text = ((string[])result[0])[25];
		GameObject.Find ("numPCard").GetComponent<Text>().text = ((string[])result[0])[26];
		GameObject.Find ("numPBags").GetComponent<Text>().text = ((string[])result[0])[27];
		GameObject.Find ("numPCloth").GetComponent<Text>().text = ((string[])result[0])[28];
	}

	void disableAvatar()

	{
		for(int i = 1; i <= 4; i++)
		{
			string avatar = "Avatar"+i;
			GameObject.Find(avatar).GetComponent<RawImage>().enabled = false;
		}
	}
}