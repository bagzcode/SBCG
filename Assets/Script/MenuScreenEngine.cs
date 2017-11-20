using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScreenEngine : MonoBehaviour {

	dbAccess db;

	void Start()
	{
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
		//GameObject.Find ("EventSystem").SetActive (false);
		SceneManager.LoadScene("Credit", LoadSceneMode.Additive);
	}
}
