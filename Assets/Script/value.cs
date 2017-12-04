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

	public string session_id, hismsg, correctanswer, reward, desc, fielddb;

	MainEngine msg;


	public void InfoCardOnClick()
	{
		
		db = GameObject.Find("database").GetComponent<dbAccess>();
		msg = GameObject.Find("EngineMain").GetComponent<MainEngine>();
		session_id = GameObject.Find("EventSystem").GetComponent<StartGame>().active_session_id;

		infonumber = db.BasicQuery ("SELECT COUNT(id) FROM tbl_Info_Card;");
		infonumber.Read ();
		maxNumbers = infonumber.GetInt32 (0);

		maxNumbers = checkmaxnumber(maxNumbers, 12);

		int selectednumber = EventSystem.current.currentSelectedGameObject.GetComponent<value>().no_card;
		string selectedbutton = EventSystem.current.currentSelectedGameObject.name;
		GameObject.Find("CardBackgroundi").GetComponent<RectTransform>().localPosition = new Vector3(posopencard(selectedbutton), -80, 0);
		
		ArrayList result = db.SingleSelectWhere ("tbl_Info_Card", "*", "id", "=", selectednumber.ToString());
		desc = ((string[])result[0])[1];
		reward = "$2"; 
		correctanswer = ((string[])result[0])[2];
		GameObject.Find ("Content").GetComponent<Text> ().text = desc;
		GameObject.Find ("Reward").GetComponent<Text> ().text = reward;
		showcard ();
		
		
		
	}

	public void ProblemCardOnClick()
	{
		db = GameObject.Find("database").GetComponent<dbAccess>();
		msg = GameObject.Find("EngineMain").GetComponent<MainEngine>();
		session_id = GameObject.Find("EventSystem").GetComponent<StartGame>().active_session_id;

		string selectedbutton = EventSystem.current.currentSelectedGameObject.name;
		GameObject.Find("CardBackgroundP").GetComponent<RectTransform>().localPosition = new Vector3(posopencard(selectedbutton), -80, 0);
		GameObject.Find ("CardBackgroundP").GetComponent<Image>().enabled = true;
		
	}

	void showcard()
	{
		GameObject.Find ("CardBackgroundi").GetComponent<Image>().enabled = true;
		GameObject.Find ("Content").GetComponent<Text>().enabled = true;
		GameObject.Find ("Reward").GetComponent<Text>().enabled = true;
		GameObject.Find ("btnTrue").GetComponent<Image>().enabled = true;
		GameObject.Find ("btnFalse").GetComponent<Image>().enabled = true;
	}

	int checkmaxnumber(int number, int limit)
	{
		if (number > limit)
			return limit;
		else 
			return number;
	}

	int posx;
	int posopencard (string btn)
	{
		
		if (btn == "btninfocard1" || btn == "btnproblemcard1") {posx = -380;} 
		else if (btn == "btninfocard2" || btn == "btnproblemcard2") {posx = 0;}
		else if (btn == "btninfocard3" || btn == "btnproblemcard3") {posx = 380;}

		return posx;
	}

	public void checkanswer ()
	{
		string choice = EventSystem.current.currentSelectedGameObject.name;
		string playername = msg.PlayerNameActive;

		if (choice == "btnTrue")choice = "True";
		else choice = "False";

		if (choice ==  correctanswer)
		{
			// insert new data to history
			hismsg = playername+", Your answer is CORRECT";
			db.InsertHistory(hismsg, session_id);
			msg.HistPrint(hismsg);
			hismsg = desc+" is TRUE statement";
			db.InsertHistory(hismsg, session_id);
			msg.HistPrint(hismsg);
			hismsg = "you receive "+reward+" for your reward";
			db.InsertHistory(hismsg, session_id);
			msg.HistPrint(hismsg);
		}
		else
		{
			// insert new data to history
			hismsg = playername+", Your answer is WRONG";
			db.InsertHistory(hismsg, session_id);
			msg.HistPrint(hismsg);
			hismsg = desc+" is TRUE statement";
			db.InsertHistory(hismsg, session_id);
			msg.HistPrint(hismsg);
			hismsg = "you loss "+reward+" reward";
			db.InsertHistory(hismsg, session_id);
			msg.HistPrint(hismsg);
		}
		GameObject.Find("TabCode").GetComponent<Tab>().Boardgame();
	}

	bool status;
	public void getproblemcard ()
	{
		string choice = GameObject.Find("EngineProblem").GetComponent<EngineProblem>().selectedcard;
		string playername = msg.PlayerNameActive;
		

		if (choice == "p1")
		{
			Debug.Log(choice);
			status = false;
		}
		else if (choice == "p2")
		{
			Debug.Log(choice);
			status = false;
		}
		else if (choice == "p3")
		{
			Debug.Log(choice);
			status = false;
		}
		else if (choice == "p4")
		{
			Debug.Log(choice);
			status = false;
		}
		else if (choice == "p5")
		{
			Debug.Log(choice);
			status = false;
		}
		else if (choice == "p6")
		{
			Debug.Log(choice);//numPCpoverty
			fielddb = "numPCpoverty";
			status = true;
		}
		else if (choice == "p7")
		{
			Debug.Log(choice);//numPCtechnology
			fielddb = "numPCtechnology";
			status = true;
		}
		else if (choice == "p8")
		{
			Debug.Log(choice);//numPCeducation
			fielddb = "numPCeducation";
			status = true;
		}
		else if (choice == "p9")
		{
			Debug.Log(choice);//numPCenvironment
			fielddb = "numPCenvironment";
			status = true;
		}
		else if (choice == "p10")
		{
			Debug.Log(choice);//numPChealth
			fielddb = "numPChealth";
			status = true;
		}

		
		if (status)msg.updatedataplayer(fielddb);

		//get number player active
		string activeplayer = GameObject.Find("EngineMain").GetComponent<MainEngine>().tempcurrentactive;
		if (activeplayer == "1")GameObject.Find("EngineMain").GetComponent<MainEngine>().Player1onClick();
		else if (activeplayer == "2")GameObject.Find("EngineMain").GetComponent<MainEngine>().Player2onClick();
		else if (activeplayer == "3")GameObject.Find("EngineMain").GetComponent<MainEngine>().Player3onClick();
		else if (activeplayer == "4")GameObject.Find("EngineMain").GetComponent<MainEngine>().Player4onClick();

		GameObject.Find("TabCode").GetComponent<Tab>().Assetsgame();
	}

	
}
