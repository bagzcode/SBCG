using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Data;
using Mono.Data.SqliteClient;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainEngine : MonoBehaviour {

	string session_id;
	private string[] registerdices = {"diceone","dicetwo","dicethree","dicefour","dicefive","dicesix"};
	private int diceNumber;
	private int number = 0;
	string active_number = "1", player_active_number = "1", tempcurrentactive;
	int nextactive = 1;
	private IDataReader playernumber, avatarnumber, totplayeractive; 
	public ArrayList playernum, avatarnum; 
	public int totnumplayeractive, diceNumberResult;
	private string activesessionid, hismsg;
	public int[] avataractive;
	public int[] playercurrentposition = {0,0,0,0};
	public int[] posxactive = {0,-141, -73, -5, 63}, posyactive = {0,-122,-193,-264,-335 };

	public string PlayerNameActive;

	public string pressbutton = "";

	public int nextplayerposition;

	dbAccess db;

	string[] numavt = {"","","","",""};

	// Use this for initialization to connect database
	void Start () {

		//Load required scenes for the games 
		SceneManager.UnloadScene("New");

		//get values of active session and number of active players
		activesessionid = GameObject.Find("EventSystem").GetComponent<StartGame>().active_session_id;
		avataractive = GameObject.Find("EventSystem").GetComponent<StartGame>().avataractive;


		//db = GetComponent<dbAccess>();
		db = GameObject.Find("database").GetComponent<dbAccess>();
		db.OpenDB("mydb.sdb");
		ArrayList result = db.SelectLastRecord ("id", "tbl_Sessions");
	
		session_id = ((string[])result[0])[0];
		
		//Set active_session_id on EventSystem to be used laterduring the game
		GameObject.Find("EventSystem").GetComponent<StartGame>().active_session_id = session_id;
		
		//this function is to insert history into table
		hismsg = "Game Starts";
		db.InsertHistory(hismsg, session_id);
		HistPrint("-");HistPrint("-"); 
		HistPrint(hismsg); 

		totplayeractive = db.BasicQuery ("SELECT COUNT(id) FROM tbl_players WHERE tbl_Sessions_id=" + session_id);
		totplayeractive.Read ();
		totnumplayeractive = totplayeractive.GetInt32 (0);
        

		playernumber = db.BasicQuery ("SELECT id FROM tbl_players WHERE tbl_Sessions_id=" + session_id);
		playernum = new ArrayList();
     	while(playernumber.Read ()){
     		for (var i = 0; i < playernumber.FieldCount; i++)
     		playernum.Add(playernumber.GetValue(i)); // This reads the entries in a row
     	}

     	avatarnumber = db.BasicQuery ("SELECT avatar_number FROM tbl_players WHERE tbl_Sessions_id=" + session_id);
		avatarnum = new ArrayList();
     	while(avatarnumber.Read ()){
     		for (var k = 0; k < avatarnumber.FieldCount; k++)
     		avatarnum.Add(avatarnumber.GetValue(k)); // This reads the entries in a row
     	}
     	disableandenabletab();disableandenabletab2();

		PlayerOnClick (1);
		GameObject.Find("BtnPlayer"+nextactive).GetComponent<Image>().color = Color.blue;
		PlayerNameActive = GameObject.Find ("numPlayerName").GetComponent<Text>().text;


	}

	void OnApplicationQuit(){
		//db.CloseDB();
		Debug.Log ("Application Close");
	}

	public void btnRollDiceOnClick () 
	{
		InvokeRepeating("GetDiceValue", 0.0f, 0.1f);
		
		//activate next player
		nextplayeractive();
		PlayerOnClick(nextactive);

		
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
		
		if (number == 3) 
		{
			
			CancelInvoke ();
			
			int pan = int.Parse(tempcurrentactive);
			number = 0;
			diceNumberResult = diceNumber;
			
			//handling rotation more than date 31
			nextplayerposition = playercurrentposition[pan-1] + diceNumberResult;
			if (nextplayerposition > 31)
			{
				nextplayerposition -= 31; 
			}

			//save the history to database and print it to interface
			hismsg = "Player "+  pan + "(" + PlayerNameActive + ")" + " Dice Result is "+ diceNumberResult +", Player "+  pan + "(" + PlayerNameActive + ")" + " walks from " + playercurrentposition[pan-1] + " to " + nextplayerposition;
			db.InsertHistory(hismsg, session_id);
			HistPrint(hismsg);

			//Update current position to array list
			playercurrentposition[pan-1] = nextplayerposition;

			//automatically go to info or problem scene for special dates
			checkinfoorproblem ();
			
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
		pressbutton = EventSystem.current.currentSelectedGameObject.name;
	}

	public void Player2onClick () 
	{
		PlayerOnClick (2);
		pressbutton = EventSystem.current.currentSelectedGameObject.name;
	}

	public void Player3onClick () 
	{
		PlayerOnClick (3);
		pressbutton = EventSystem.current.currentSelectedGameObject.name;
	}

	public void Player4onClick () 
	{
		PlayerOnClick (4);
		pressbutton = EventSystem.current.currentSelectedGameObject.name;
	}

	int selectednumber;
	void PlayerOnClick (int number)
	{
		selectednumber = number - 1;
		ArrayList resultstart = db.SelectJoinTable ("tbl_Users.Name, tbl_Players.*", "tbl_Players INNER JOIN tbl_Users", "tbl_Users.id=tbl_Players.tbl_Users_id AND tbl_Players.tbl_Sessions_id=" + session_id + " AND tbl_Players.id=" + playernum[selectednumber]);
		SetValueNum (resultstart);
		disableAvatar ();
		active_number = ((string[])resultstart [0]) [2];
		string avatar = "Avatar" + active_number.ToString ();
		GameObject.Find (avatar).GetComponent<RawImage> ().enabled = true;
		for(int j = 1; j <= totnumplayeractive ; j++)
		{
			//Debug.Log(j+" == "+int.Parse(numavt[j]));
			if (numavt[j] == "1")
			{
				GameObject.Find("BtnPlayer"+j).GetComponent<Image>().color = new Color32(101,191,208,100);//"#65BFD0FF";
				GameObject.Find("BtnPlayer"+j+"a").GetComponent<Image>().color = new Color32(101,191,208,100);//"#65BFD0FF";
				ColorBlock bt1 = GameObject.Find("BtnPlayer"+j).GetComponent<Button>().colors;
        		bt1.normalColor = new Color32(101,191,208,100);
        		bt1.highlightedColor = new Color32(101,191,208,100);
        		GameObject.Find("BtnPlayer"+j).GetComponent<Button>().colors = bt1;
        		ColorBlock bt2 = GameObject.Find("BtnPlayer"+j+"a").GetComponent<Button>().colors;
        		bt2.normalColor = new Color32(101,191,208,100);
        		bt2.highlightedColor = new Color32(101,191,208,100);
        		GameObject.Find("BtnPlayer"+j+"a").GetComponent<Button>().colors = bt2;
			}
			else if (numavt[j] == "2")
			{
				GameObject.Find("BtnPlayer"+j).GetComponent<Image>().color = new Color32(233,92,92,100);//(243,158,150,100);//"#F39E96FF";
				GameObject.Find("BtnPlayer"+j+"a").GetComponent<Image>().color = new Color32(233,92,92,100);//"#F39E96FF";
				ColorBlock bt1 = GameObject.Find("BtnPlayer"+j).GetComponent<Button>().colors;
				bt1.normalColor = new Color32(233,92,92,100);
				bt1.highlightedColor = new Color32(233,92,92,100);
        		GameObject.Find("BtnPlayer"+j).GetComponent<Button>().colors = bt1;
        		ColorBlock bt2 = GameObject.Find("BtnPlayer"+j+"a").GetComponent<Button>().colors;
				bt2.normalColor = new Color32(233,92,92,100);
				bt2.highlightedColor = new Color32(233,92,92,100);
        		GameObject.Find("BtnPlayer"+j+"a").GetComponent<Button>().colors = bt2;
			}
			else if (numavt[j] == "3")
			{
				GameObject.Find("BtnPlayer"+j).GetComponent<Image>().color = new Color32(157,157,157,100);//"#9D9D9DFF";
				GameObject.Find("BtnPlayer"+j+"a").GetComponent<Image>().color = new Color32(157,157,157,100);//"#9D9D9DFF";
				ColorBlock bt1 = GameObject.Find("BtnPlayer"+j).GetComponent<Button>().colors;
        		bt1.normalColor = new Color32(157,157,157,100);
        		bt1.highlightedColor = new Color32(157,157,157,100);
        		GameObject.Find("BtnPlayer"+j).GetComponent<Button>().colors = bt1;
        		ColorBlock bt2 = GameObject.Find("BtnPlayer"+j+"a").GetComponent<Button>().colors;
        		bt2.normalColor = new Color32(157,157,157,100);
        		bt2.highlightedColor = new Color32(157,157,157,100);
        		GameObject.Find("BtnPlayer"+j+"a").GetComponent<Button>().colors = bt2;
			}
			else if (numavt[j] == "4")
			{
				GameObject.Find("BtnPlayer"+j).GetComponent<Image>().color = new Color32(255,200,2,100);//"#FFC802FF";
				GameObject.Find("BtnPlayer"+j+"a").GetComponent<Image>().color = new Color32(255,200,2,100);//"#FFC802FF";
				ColorBlock bt1 = GameObject.Find("BtnPlayer"+j).GetComponent<Button>().colors;
        		bt1.normalColor = new Color32(255,200,2,100);
        		bt1.highlightedColor = new Color32(255,200,2,100);
        		GameObject.Find("BtnPlayer"+j).GetComponent<Button>().colors = bt1;
        		ColorBlock bt2 = GameObject.Find("BtnPlayer"+j+"a").GetComponent<Button>().colors;
        		bt2.normalColor = new Color32(255,200,2,100);
        		bt2.highlightedColor = new Color32(255,200,2,100);
        		GameObject.Find("BtnPlayer"+j+"a").GetComponent<Button>().colors = bt2;
			}
		}
		GameObject.Find("BtnPlayer"+number).GetComponent<Image>().color = Color.blue;

		
		if (pressbutton == "BtnPlayer1a" || pressbutton == "BtnPlayer2a" || pressbutton == "BtnPlayer3a" || pressbutton == "BtnPlayer4a")
		setassetscene();
		
	}

	void SetValueNum(ArrayList result)
	{
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

	//this function to control active tab depend on the active players
	void disableandenabletab()

	{
		for(int i = 1; i <= 4; i++)
		{
			GameObject.Find("BtnPlayer"+i).GetComponent<Image>().enabled = false;
			GameObject.Find("BtnPlayer"+i+"av").GetComponent<Image>().enabled = false;
		}

		for(int j = 1; j <= totnumplayeractive ; j++)
		{
			GameObject.Find("BtnPlayer"+j).GetComponent<Image>().enabled = true;
			GameObject.Find("BtnPlayer"+j+"av").GetComponent<Image>().enabled = true;
			numavt[j] = avatarnum[j-1].ToString();
			//Debug.Log(numavt[j]);
			GameObject.Find("BtnPlayer"+j+"av").GetComponent<Image>().sprite = Resources.Load<Sprite>("Avatar"+numavt[j]);
			if (numavt[j] == "3")GameObject.Find("BtnPlayer"+j+"av").GetComponent<RectTransform>().localPosition = new Vector3(0f, -6.7f, 0f);
			else GameObject.Find("BtnPlayer"+j+"av").GetComponent<RectTransform>().localPosition = new Vector3(0f, 0f, 0f);
			
		}
	}

	void disableandenabletab2()

	{
		for(int i = 1; i <= 4; i++)
		{
			GameObject.Find("BtnPlayer"+i+"a").GetComponent<Image>().enabled = false;
			GameObject.Find("BtnPlayer"+i+"ava").GetComponent<Image>().enabled = false;
		}

		for(int j = 1; j <= totnumplayeractive ; j++)
		{
			GameObject.Find("BtnPlayer"+j+"a").GetComponent<Image>().enabled = true;
			GameObject.Find("BtnPlayer"+j+"ava").GetComponent<Image>().enabled = true;
			numavt[j] = avatarnum[j-1].ToString();
			GameObject.Find("BtnPlayer"+j+"ava").GetComponent<Image>().sprite = Resources.Load<Sprite>("Avatar"+numavt[j]);
			if (numavt[j] == "3")GameObject.Find("BtnPlayer"+j+"ava").GetComponent<RectTransform>().localPosition = new Vector3(0f, -6.7f, 0f);
			else GameObject.Find("BtnPlayer"+j+"ava").GetComponent<RectTransform>().localPosition = new Vector3(0f, 0f, 0f);
			
		}
	}

	void nextplayeractive()
	{
		tempcurrentactive = player_active_number;
		PlayerNameActive = GameObject.Find ("numPlayerName").GetComponent<Text>().text;
		
		nextactive = int.Parse(player_active_number) + 1;
		if (nextactive > totnumplayeractive)
			nextactive = 1;
		
		GameObject.Find("LabelActive Player").GetComponent<RectTransform>().localPosition = new Vector3(posxactive[nextactive], -353, 0);
		GameObject.Find("LabelActive Playera").GetComponent<RectTransform>().localPosition = new Vector3(-588, posyactive[nextactive], 0);
		player_active_number = nextactive.ToString();
	}

	void setassetscene()
	{
		GameObject.Find("MainBoardgame").GetComponent<Canvas>().sortingOrder = 2;
		GameObject.Find("PlayerAssetScreen").GetComponent<Canvas>().sortingOrder = 3;
		GameObject.Find("Info").GetComponent<Canvas>().sortingOrder = 1;
		GameObject.Find("Problem").GetComponent<Canvas>().sortingOrder = 0;
	}

	void checkinfoorproblem ()
	{
		if (nextplayerposition == 2 || nextplayerposition == 10 || nextplayerposition == 13 || nextplayerposition == 18 || nextplayerposition == 29 || nextplayerposition == 30)
		{
			//this function is to insert history into table
			hismsg = "You stand in INFO area no "+ nextplayerposition;
			db.InsertHistory(hismsg, session_id);
			HistPrint(hismsg); 
			GameObject.Find("TabCode").GetComponent<Tab>().Infogame();
		}
		
		else if (nextplayerposition == 5 || nextplayerposition == 8 || nextplayerposition == 15 || nextplayerposition == 19 || nextplayerposition == 23 || nextplayerposition == 26)
		{
			hismsg = "You stand in PROBLEM area no "+ nextplayerposition;
			db.InsertHistory(hismsg, session_id);
			HistPrint(hismsg); 
			GameObject.Find("TabCode").GetComponent<Tab>().Problemgame();
		}
	}

	//function to print history
	public void HistPrint(string msg)
	{

		GameObject.Find ("numHistory3").GetComponent<Text>().text = GameObject.Find ("numHistory2").GetComponent<Text>().text;
		GameObject.Find ("numHistory2").GetComponent<Text>().text = GameObject.Find ("numHistory1").GetComponent<Text>().text;
		GameObject.Find ("numHistory1").GetComponent<Text>().text = msg;
	}

	public void updatedataplayer(string fieldb)
	{
		int vfield = int.Parse(GameObject.Find(fieldb).GetComponent<Text>().text) + 1;
		db.UpdateData("tbl_Players", fieldb, vfield.ToString(), playernum[selectednumber].ToString());
	}


}
