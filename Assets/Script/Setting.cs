using UnityEngine;
using System.Collections;

public class Setting : MonoBehaviour {

	private bool menuon = false;// Use this for initialization
	private float speed = 20f;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void settingbtnOnclick()
	{
		if (menuon == false)
		{
			menuon = true;
			GameObject.Find("Panel").GetComponent<Menu>().updatespeed(-speed);
		}
		else
		{
			menuon = false;
			GameObject.Find("Panel").GetComponent<Menu>().updatespeed(speed);
		}
	}
}
