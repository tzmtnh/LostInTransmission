using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisualizeCommands : MonoBehaviour {

    private GameObject commandBox;
    private GameObject commandBoxArrow;

    public GameObject leftArrow;
    public GameObject rightArrow;

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

        commandBox = GameObject.Find("CommandBox");
        commandBoxArrow = GameObject.Find("CommandBoxArrow");
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
        GameObject commandImage;
        switch (cmd)
        {
            case Player.Command.Left:
                commandBoxArrow.transform.localScale = new Vector3(-.7f, .7f, 1);
                commandImage = Instantiate<GameObject>(leftArrow);
                break;
            case Player.Command.Right:
                commandBoxArrow.transform.localScale = new Vector3(.7f, .7f, 1);
                commandImage = Instantiate<GameObject>(rightArrow);
                break;
            default:
                commandBoxArrow.transform.localScale = new Vector3(-.7f, .7f, 1);
                commandImage = Instantiate<GameObject>(leftArrow);
                break;
        }

        commandImage.transform.position = commandBox.transform.position;
        commandImage.transform.SetParent(commandBox.transform);

		if (spawned_prefab == null) {
			spawned_prefab = new List<sonars> ();
			var struct_sonar = new sonars();
			struct_sonar.startTime = Time.time;
			struct_sonar.delay = delay;
			struct_sonar.sonar = commandImage;
			spawned_prefab.Add(struct_sonar);
		} else 
		{
			var sonar_prefab = new sonars();
			sonar_prefab.startTime = Time.time;
			sonar_prefab.delay = delay;
			sonar_prefab.sonar = commandImage;
			spawned_prefab.Add(sonar_prefab);
		}
    }

	void Update()
	{
        GameObject.Find("Delay").GetComponent<UnityEngine.UI.Text>().text = "Delay: " + Player.instance.delay.ToString();
        GameObject.Find("Distance").GetComponent<UnityEngine.UI.Text>().text = "Distance: " + (int)Player.instance.distance;

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
