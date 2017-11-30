using UnityEngine;
using System;
using System.Collections;
using System.Data;
using System.Text;
using Mono.Data.SqliteClient;
using System.IO;

public class dbAccess : MonoBehaviour {
	private string connection;
	private IDbConnection dbcon;
	private IDbCommand dbcmd;
	private IDataReader reader;
	private StringBuilder builder;
	public string err;
	
	public void OpenDB(string p)
	{
		//Debug.Log("Call to OpenDB:" + p);
		// check if file exists in Application.persistentDataPath
		string filepath = Application.persistentDataPath + "/" + p;
		if (filepath != "") {
			err = "Database found at " + filepath;
			if(!File.Exists(filepath))
			{
				Debug.LogWarning("File \"" + filepath + "\" does not exist. Attempting to create from \"" +
					Application.dataPath + "!/assets/" + p);
				// if it doesn't ->
				// open StreamingAssets directory and load the db -> 
				WWW loadDB = new WWW("jar:file://" + Application.dataPath + "!/assets/" + p);//path for Android
				//WWW loadDB = new WWW("jar:file://" + Application.dataPath + "!/assets/" + p);//path for Android
				while(!loadDB.isDone) {}
				// then save to Application.persistentDataPath
				File.WriteAllBytes(filepath, loadDB.bytes);
			}

			//open db connection
			connection = "URI=file:" + filepath;
			//Debug.Log("Stablishing connection to: " + connection);
			dbcon = new SqliteConnection(connection);
			dbcon.Open();
			
		} else {
			err = "Database File Not Found"; 
		}
	}
	
	public void CloseDB(){
		reader.Close(); // clean everything up
  	 	reader = null;
   		dbcmd.Dispose();
   		dbcmd = null;
   		dbcon.Close();
   		dbcon = null;
	}
	
	public IDataReader BasicQuery(string query){ // run a basic Sqlite query
		//Debug.Log (query);
		dbcmd = dbcon.CreateCommand(); // create empty command
		dbcmd.CommandText = query; // fill the command
		reader = dbcmd.ExecuteReader(); // execute command which returns a reader
		return reader; // return the reader
	
	}
	
	
	public bool CreateTable(string name,string[] col, string[] colType){ // Create a table, name, column array, column type array
		string query;
		query  = "CREATE TABLE " + name + "(" + col[0] + " " + colType[0];
		for(var i=1; i< col.Length; i++){
			query += ", " + col[i] + " " + colType[i];
		}
		query += ")";
		try{
			dbcmd = dbcon.CreateCommand(); // create empty command
			dbcmd.CommandText = query; // fill the command
			reader = dbcmd.ExecuteReader(); // execute command which returns a reader
		}
		catch(Exception e){
			
			Debug.Log(e);
			return false;
		}
		return true;
	}
	
	public int InsertIntoSingle(string tableName, string colName , string value ){ // single insert
		string query;
		query = "INSERT INTO " + tableName + "(" + colName + ") " + "VALUES (" + value + ")";
		try
		{
			dbcmd = dbcon.CreateCommand(); // create empty command
			dbcmd.CommandText = query; // fill the command
			reader = dbcmd.ExecuteReader(); // execute command which returns a reader
		}
		catch(Exception e){
			
			Debug.Log(e);
			return 0;
		}
		return 1;
	}
	
	public int InsertIntoSpecific(string tableName, string[] col, string[] values){ // Specific insert with col and values
		string query;
		query = "INSERT INTO " + tableName + "(" + col[0];
		for(int i=1; i< col.Length; i++){
			query += ", " + col[i];
		}
		query += ") VALUES (" + values[0];
		for(int i=1; i< col.Length; i++){
			query += ", " + values[i];
		}
		query += ")";
		//Debug.Log(query);
		try
		{
			dbcmd = dbcon.CreateCommand();
			dbcmd.CommandText = query;
			reader = dbcmd.ExecuteReader();
		}
		catch(Exception e){
			
			Debug.Log(e);
			return 0;
		}
		return 1;
	}
	
	public int InsertInto(string tableName , string[] values ){ // basic Insert with just values
		string query;
		query = "INSERT INTO " + tableName + " VALUES (" + values[0];
		for(int i=1; i< values.Length; i++){
			query += ", " + values[i];
		}
		query += ")";
		//Debug.Log (query);
		try
		{
			dbcmd = dbcon.CreateCommand();
			dbcmd.CommandText = query;
			reader = dbcmd.ExecuteReader();
		}
		catch(Exception e){
			
			Debug.Log(e);
			return 0;
		}
		return 1;
	}
	
	public ArrayList SingleSelectWhere(string tableName, string itemToSelect, string wCol, string wPar, string wValue){ // Selects a single Item
		string query;
		query = "SELECT " + itemToSelect + " FROM " + tableName + " WHERE " + wCol + wPar + wValue;	
		//Debug.Log (query);
		dbcmd = dbcon.CreateCommand();
		dbcmd.CommandText = query;
		reader = dbcmd.ExecuteReader();
		string[] row = new string[reader.FieldCount];
		ArrayList readArray = new ArrayList();
		while(reader.Read()){
			int j=0;
			while(j < reader.FieldCount)
			{
				row[j] = reader.GetString(j);
				j++;
			}
			readArray.Add(row);
		}
		return readArray; // return matches
	}

	public ArrayList SelectLastRecord(string itemToSelect, string tableName){ // Selects a single Item last record
		string query;
		query = "SELECT " + itemToSelect + " FROM " + tableName + " ORDER BY id DESC LIMIT 1";	
		dbcmd = dbcon.CreateCommand();
		//Debug.Log (query);
		dbcmd.CommandText = query;
		reader = dbcmd.ExecuteReader();
		//string[,] readArray = new string[reader, reader.FieldCount];
		string[] row = new string[reader.FieldCount];
		ArrayList readArray = new ArrayList();
		while(reader.Read()){
			int j=0;
			while(j < reader.FieldCount)
			{
				row[j] = reader.GetString(j);
				j++;
			}
			readArray.Add(row);
		}
		return readArray; // return matches
	}

	public ArrayList SelectJoinTable(string itemToSelect, string tableName, string conditions){ // Selects a single Item last record
		string query;
		query = "SELECT " + itemToSelect + " FROM " + tableName + " ON " + conditions;
		//Debug.Log (query);
		dbcmd = dbcon.CreateCommand();
		dbcmd.CommandText = query;
		reader = dbcmd.ExecuteReader();
		//string[,] readArray = new string[reader, reader.FieldCount];
		string[] row = new string[reader.FieldCount];
		ArrayList readArray = new ArrayList();
		while(reader.Read()){
			int j=0;
			while(j < reader.FieldCount)
			{
				row[j] = reader.GetString(j);
				j++;
			}
			readArray.Add(row);
		}
		return readArray; // return matches
	}

	//Query for History
	public int InsertHistory(string desc, string session_id ){
		string query;
		query = "INSERT INTO tbl_Histories (Descriptions, tbl_Sessions_id) VALUES ('" + desc + "', " + session_id + ")" ;
		//Debug.Log(query);
		try
		{

			dbcmd = dbcon.CreateCommand();
			dbcmd.CommandText = query;
			reader = dbcmd.ExecuteReader();
		}
		catch(Exception e){
			
			Debug.Log(e);
			return 0;
		}
		return 1;
	}

	//UPDATE 'tbl_Players' SET 'numMoney'=? WHERE '_rowid_'='490';
	public int UpdateData (string tbl, string field, string vfield, string userid)
	{
		string query;
		query = "UPDATE '"+tbl+"' SET '"+field+"'="+vfield+" WHERE '_rowid_'='"+userid+"'" ;
		Debug.Log(query);
		try
		{

			dbcmd = dbcon.CreateCommand();
			dbcmd.CommandText = query;
			reader = dbcmd.ExecuteReader();
		}
		catch(Exception e){
			
			Debug.Log(e);
			return 0;
		}
		return 1;
	}

	




}