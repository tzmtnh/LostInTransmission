using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualizeCommands : MonoBehaviour {

	public GameObject sonar_prefab;
	private static VisualizeCommands _instance;
	public static VisualizeCommands singleton{ get{ return _instance; } }

	private static float CommandDelay = 0.0f;
	private List<sonars> spawned_prefab;
	private float timeTracker = 0.0f;
	private float movementRate = 0.5f;

	private struct sonars
	{
		public GameObject sonar;
		public float startTime;
		public float delay;
	}

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

		if (spawned_prefab == null) {
			spawned_prefab = new List<sonars> ();
			var struct_sonar = new sonars();
			struct_sonar.startTime = Time.time;
			struct_sonar.delay = delay;
			struct_sonar.sonar = sonar;
			spawned_prefab.Add(struct_sonar);
		} else 
		{
			var sonar_prefab = new sonars();
			sonar_prefab.startTime = Time.time;
			sonar_prefab.delay = delay;
			sonar_prefab.sonar = sonar;
			spawned_prefab.Add(sonar_prefab);
		}
    }

	void Update()
	{
        GameObject.Find("Delay").GetComponent<UnityEngine.UI.Text>().text = "Delay: " + Player.instance.delay.ToString();
        GameObject.Find("Distance").GetComponent<UnityEngine.UI.Text>().text = "Distance: " + Player.instance.distance.ToString();

        if (spawned_prefab != null) 
		{
			float startPosition = 50.0f;
			float endPosition = 1000.0f;

			foreach (var x in spawned_prefab) {
				if (x.sonar != null) {
					float timey = Mathf.InverseLerp(x.startTime, x.delay, Time.time);
					float pos = Mathf.Lerp(startPosition, endPosition, timey);

					var horizontal = x.sonar.transform.localPosition;
					horizontal.x += pos; 
					x.sonar.transform.localPosition = horizontal;
					if (horizontal.x >= 1000) {
						Destroy (x.sonar);
					}
				}
			}

		}

	}

}
