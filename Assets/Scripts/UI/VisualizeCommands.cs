using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualizeCommands : MonoBehaviour {

	public GameObject sonar_prefab;
	private static VisualizeCommands _instance;
	public static VisualizeCommands singleton{ get{ return _instance; } }

	private static float CommandDelay = 0.0f;
	private List<GameObject> spawned_prefab;

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
		
	void Start()
	{
		Player.OnCommand += OnSend;
	}

	void OnDestroy()
	{
		Player.OnCommand -= OnSend;
	}
		
	public void OnSend(Player.Command cmd, float delay)
	{
        GameObject sonar = Instantiate<GameObject>(sonar_prefab);
		sonar.transform.position = GameObject.Find ("Remote").transform.position;
		sonar.transform.SetParent(GameObject.Find ("Remote").transform);
		CommandDelay = delay;

		if (spawned_prefab == null) {
			spawned_prefab = new List<GameObject> ();
			spawned_prefab.Add(sonar);
		} else 
		{
			spawned_prefab.Add(sonar);
		}
    }

	void Update()
	{
		if (spawned_prefab != null) 
		{
			foreach (var x in spawned_prefab) {
				var horizontal = x.transform.localPosition;
				horizontal.x++; 
				x.transform.localPosition = horizontal;
			}

		}

	}

}
