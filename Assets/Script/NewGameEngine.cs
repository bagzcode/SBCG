using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Mono.Data.Sqlite; 
using System.Data; 
using System;
using UnityEngine.SceneManagement;

public class NewGameEngine : MonoBehaviour 
{
	public int numSelectors, numplayers, numplayersactive;
	public GameObject[] player;
	public string[] playerusername, playerusernameactive;

	int n; 

	dbAccess db;
	StartGame defvalueavatar;

	void Start ()
	{
		SceneManager.UnloadScene("Menu");

		numSelectors = 4;
		player = new GameObject[numSelectors];
		playerusername = new string[numSelectors];
		playerusernameactive = new string[numSelectors];
		disableAvatar ();
		//player [0].SetActive (true);
		numplayers = 4;
		numplayersactive = 0;

		db = GameObject.Find("database").GetComponent<dbAccess>();
		db.OpenDB("mydb.sdb");
		defvalueavatar = GameObject.Find("EventSystem").GetComponent<StartGame>();
	}

	public void SubmitonClick()
	{
		numSelectors = numplayers;
		disableAvatar ();
		numplayers = int.Parse(GameObject.Find("InputFieldPlayerNumber").GetComponent<InputField>().text);
		if (numplayers < 1)
			numplayers = 1;
		if (numplayers > 5)
			numplayers = 4;
		for (int i = 0; i < numplayers; i++) {
			player [i].SetActive (true);
		}
	}
		

	public void disableAvatar()
	{
		for (int i = 0; i < numSelectors; i++) 
		{
			n = i + 1;
			player [i] = GameObject.Find ("Avatar" + n);
			//player[i].SetActive(false);

		}
	}

	public void StartNewGame()
	{
		//makesure all input fieds are not empty
		bool endstatus = true;
		for (int i = 0; i < numplayers; i++)
		{
			int number = i + 1;
			playerusername[i] = GameObject.Find("InputFieldPlayer"+number+"name").GetComponent<InputField>().text;
			if (playerusername [i] != "")
			{
				
				playerusernameactive[numplayersactive] = playerusername[i];
				defvalueavatar.avataractive[numplayersactive] = number;
				numplayersactive += 1;
				
			}

			if (number == numplayers) 
				endstatus = true;
			else
				endstatus = false;
				
		}
		//Debug.Log (avataractive[0]);
		//endstatus = false;
		if (endstatus) 
		{
			//create a session
			string datetimenow = System.DateTime.Now.ToString ("yyyy/MM/dd HH:mm:ss");
			string[] valuedata = { "'"+datetimenow+"'", "'Active'" };
			//int insertresult = 
			db.InsertInto("tbl_Sessions (Session_Name, Status)", valuedata);
			//Debug.Log(insertresult);

			//select the new created sessions ID.
			ArrayList result = db.SelectLastRecord ("id", "tbl_Sessions");
			string session_id = ((string[])result[0])[0];
			//Debug.Log ("Session created ok" + session_id);

			for (int i = 0; i < numplayersactive; i++) {
				//insert the login and the user and also create the player
				string[] valuedata2 = {"'"+playerusernameactive [i]+"'" , "'"+playerusernameactive [i]+"'"};
				//int insertresult2 = 
				db.InsertInto("tbl_Login (username, password)", valuedata2);
				//Debug.Log(insertresult2);

				//select the new login ID.
				ArrayList result2 = db.SelectLastRecord ("id", "tbl_Login");
				string login_id = ((string[])result2[0])[0];
				//Debug.Log ("Login created ok" + login_id);

				//insert the user
				string[] valuedata3 = {"'"+playerusernameactive [i]+"'", "'"+login_id+"'"};
				//int insertresult3 = 
				db.InsertInto("tbl_Users (name, tbl_Login_id)", valuedata3);
				//Debug.Log(insertresult3);

				//select the new User ID.
				ArrayList result3 = db.SelectLastRecord ("id", "tbl_Users");
				string user_id = ((string[])result3[0])[0];
				//Debug.Log ("user_id created ok" + user_id);

				string statusdice;
				if (i == 0)
					statusdice = "Active";
				else
					statusdice = "Off";

				//insert the players
				int number = defvalueavatar.avataractive[i];
				//Debug.Log(number);
				string[] valuedata4 = { "'"+number+"'" , "'10'" , "'1'", "'none'", "'"+statusdice+"'", "'"+user_id+"'", "'"+session_id+"'"};
				//int insertresult4 = 
				db.InsertInto("tbl_Players (avatar_number, numMoney, numLevel, numReward, status_dice_roll, tbl_Users_id, tbl_Sessions_id)", valuedata4);
				//Debug.Log ("status for ok " + insertresult4);
			}


			db.CloseDB();

			//GameObject.Find ("EventSystem").SetActive (false);
			SceneManager.LoadScene("Main", LoadSceneMode.Additive);
		}

	}
}
