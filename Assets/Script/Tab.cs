using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Data;
using Mono.Data.SqliteClient;
using UnityEngine.SceneManagement;

public class Tab : MonoBehaviour {


	private RectTransform PC;
	private RectTransform F;
	private RectTransform RM;
	private RectTransform P;
	private GameObject BG;
	private GameObject AS;
	private GameObject INFO;
	private GameObject PROBLEM;
	private GameObject SP;


	void Start()
	{
		PC = GameObject.Find("BackgroundPC").GetComponent<RectTransform>();
		F = GameObject.Find("BackgroundF").GetComponent<RectTransform>();
		RM = GameObject.Find("BackgroundRM").GetComponent<RectTransform>();
		P = GameObject.Find("BackgroundP").GetComponent<RectTransform>();
		BG = GameObject.Find("MainBoardgame");
		AS = GameObject.Find("PlayerAssetScreen");
		INFO = GameObject.Find("Info");
		PROBLEM = GameObject.Find("Problem");
		SP = GameObject.Find("SettingPanel");

	}

	public void tabPC ()
	{
		
		P.SetAsFirstSibling();
		RM.SetAsFirstSibling();
		F.SetAsFirstSibling();
	}

	public void tabF ()
	{
		
		P.SetAsFirstSibling();
		RM.SetAsFirstSibling();
		PC.SetAsFirstSibling();
	}

	public void tabRM ()
	{
		
		
		P.SetAsFirstSibling();
		PC.SetAsFirstSibling();
		F.SetAsFirstSibling();
		
	}

	public void tabP ()
	{
		
		
		PC.SetAsFirstSibling();
		RM.SetAsFirstSibling();
		F.SetAsFirstSibling();
	}

	public void Boardgame ()
	{
		SP.GetComponent<Canvas>().sortingOrder = 4;
		BG.GetComponent<Canvas>().sortingOrder = 3;
		AS.GetComponent<Canvas>().sortingOrder = 2;
		INFO.GetComponent<Canvas>().sortingOrder = 1;
		PROBLEM.GetComponent<Canvas>().sortingOrder = 0;
	}

	public void Assetsgame ()
	{
		SP.GetComponent<Canvas>().sortingOrder = 4;
		AS.GetComponent<Canvas>().sortingOrder = 3;
		INFO.GetComponent<Canvas>().sortingOrder = 2;
		PROBLEM.GetComponent<Canvas>().sortingOrder = 1;
		BG.GetComponent<Canvas>().sortingOrder = 0;
	}

	public void Infogame ()
	{
		SP.GetComponent<Canvas>().sortingOrder = 4;
		INFO.GetComponent<Canvas>().sortingOrder = 3;
		PROBLEM.GetComponent<Canvas>().sortingOrder = 2;
		BG.GetComponent<Canvas>().sortingOrder = 1;
		AS.GetComponent<Canvas>().sortingOrder = 0;
		
		GameObject.Find("EngineInfo").GetComponent<EngineInfo>().LoadInfoScene();
	}

	public void Problemgame ()
	{
		SP.GetComponent<Canvas>().sortingOrder = 4;
		PROBLEM.GetComponent<Canvas>().sortingOrder = 3;
		BG.GetComponent<Canvas>().sortingOrder = 2;
		AS.GetComponent<Canvas>().sortingOrder = 1;
		INFO.GetComponent<Canvas>().sortingOrder = 0;
		
		GameObject.Find("EngineProblem").GetComponent<EngineProblem>().LoadProblemScene();
	}

	

	
}
