using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScreenEngine : MonoBehaviour {

	dbAccess db;

	private bool menuon = false;// Use this for initialization
	private float speed = 20f;
	

	void Start()
	{
		GameObject.Find("MenuScreen").GetComponent<Canvas>().sortingOrder = 0;
		GameObject.Find("SettingPanel").GetComponent<Canvas>().sortingOrder = 1;
		db = GameObject.Find("database").GetComponent<dbAccess>();
		db.OpenDB("mydb.sdb");
		GameObject.Find ("err").GetComponent<Text> ().text = db.err;
	}
	public void btnNewGameonClick()
	{
		//GameObject.Find ("EventSystem").SetActive (false);
		SceneManager.LoadScene("New", LoadSceneMode.Additive);
	}

	public void btnLoadGameonClick()
	{
		//GameObject.Find ("EventSystem").SetActive (false);
		SceneManager.LoadScene("Load", LoadSceneMode.Additive);
	}

	public void btnCreditonClick()
	{
		if (menuon == false)
		{
			menuon = true;
			GameObject.Find("PanelMenu").GetComponent<Menu>().updatespeed(-speed);
		}
		else
		{
			menuon = false;
			GameObject.Find("PanelMenu").GetComponent<Menu>().updatespeed(speed);
		}
	}

	
}
