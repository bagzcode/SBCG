using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour {

	public string active_session_id;
	public int[] avataractive;
	// Use this for initialization
	void Start () {
		avataractive = new int[4];
		SceneManager.LoadScene("Menu", LoadSceneMode.Additive);

	}
	
}
