using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

	private static UIManager _instance;
	public static UIManager singleton{ get{ return _instance; } }

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

}
