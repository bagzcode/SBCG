using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Data;
using Mono.Data.SqliteClient;
using UnityEngine.SceneManagement;

public class Tab : MonoBehaviour {


	public GameObject PC;
	public GameObject F;
	public GameObject RM;
	public GameObject P;

	void Start()
	{
		PC = GameObject.Find("PC");
		F = GameObject.Find("F");
		RM = GameObject.Find("RM");
		P = GameObject.Find("P");
	}

	public void tabPC ()
	{
		
		PC.SetActive(true);
		F.SetActive(false);
		RM.SetActive(false);
		P.SetActive(false);
	}

	public void tabF ()
	{
		
		PC.SetActive(false);
		F.SetActive(true);
		RM.SetActive(false);
		P.SetActive(false);
	}

	public void tabRM ()
	{
		
		PC.SetActive(false);
		F.SetActive(false);
		RM.SetActive(true);
		P.SetActive(false);
	}

	public void tabP ()
	{
		
		PC.SetActive(false);
		F.SetActive(false);
		RM.SetActive(false);
		P.SetActive(true);
	}

	

	
}
