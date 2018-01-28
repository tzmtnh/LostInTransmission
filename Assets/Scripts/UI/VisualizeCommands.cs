using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualizeCommands : MonoBehaviour {

	public GameObject sonar_prefab;
	private static VisualizeCommands _instance;
	public static VisualizeCommands singleton{ get{ return _instance; } }

	void Awake()
	{
		if( _instance == null )//Singleton implemention
			_instance = this;
		else
		{
			Destroy( this.gameObject );
			return;
		}
		DontDestroyOnLoad (this.gameObject);
	}
		
	public void OnStart()
	{
		Player.OnCommand += OnSend;
	}

	public void OnDestroy()
	{
		Player.OnCommand -= OnSend;
	}
		
	public void OnSend(Player.Command cmd, float delay)
	{
		
	}

	public void Update()
	{
		if (Player.OnCommand != null)
		{
			GameObject sonar = Instantiate<GameObject>( sonar_prefab );
		}
	}

}
