using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisualizeCommands : MonoBehaviour {

	public GameObject leftArrow;
    public GameObject rightArrow;

	Transform _commandBoxArrow;
	Transform _start;
	Transform _end;
	Transform _commandsParent;

	Text _distance;

	private struct Sonar
	{
		public GameObject sonar;
		public float startTime;
		public float delay;
	}

	void Awake()
	{
        _commandBoxArrow = transform.parent.Find("CommandBoxArrow");
		_commandsParent = new GameObject("Commands parent").transform;
		_commandsParent.SetParent(transform.parent);

		_start = _commandBoxArrow.Find("Start");
		Transform receiver = transform.parent.Find("Receiver");
		_end = receiver.Find("End");
        
		_distance = transform.parent.Find("Distance").GetComponent<Text>();
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
                _commandBoxArrow.localScale = new Vector3(-.7f, .7f, .7f);
                commandImage = Instantiate(leftArrow);
				commandImage.transform.localScale = new Vector3(-1, 1, 1);
				break;

			case Player.Command.Right:
			default:
				_commandBoxArrow.localScale = new Vector3(.7f, .7f, .7f);
                commandImage = Instantiate(rightArrow);
				commandImage.transform.localScale = new Vector3(1, 1, 1);
				break;
        }

		commandImage.SetActive(true);
		commandImage.transform.SetParent(_commandsParent);
		commandImage.transform.position = _start.position;

		StartCoroutine(moveSonarCo(delay, commandImage.transform));
    }

	IEnumerator moveSonarCo(float delay, Transform sonar) {
		const float MIN_DELAY = 0.05f;
		delay = Mathf.Max(MIN_DELAY, delay);
		float timer = 0;

		Vector3 startPos = _start.transform.position;
		Vector3 endPos = _end.transform.position;

		while (timer < delay) {
			sonar.position = Vector3.Lerp(startPos, endPos, timer / delay);
			yield return null;
			timer += Time.deltaTime;
		}

		sonar.position = endPos;
		yield return new WaitForSeconds(0.2f);
		Destroy(sonar.gameObject);
	}

	void Update()
	{
		int distance = (int)Player.instance.distance;
		_distance.text = distance.ToString();
	}

	void OnDisable() {
		StopAllCoroutines();
		for (int i = _commandsParent.childCount - 1; i >= 0; i--) {
			Destroy(_commandsParent.GetChild(i).gameObject);
		}
	}

}
