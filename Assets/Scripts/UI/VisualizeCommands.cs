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
	Image _waves;

	float _delayIntegral = 0;

	private struct Sonar
	{
		public GameObject sonar;
		public float startTime;
		public float delay;
	}

	void Start()
	{
		Player.OnCommand += OnSend;

		_commandBoxArrow = UIManager.inst.commandBox.Find("CommandBoxArrow");
		_commandsParent = new GameObject("Commands parent").transform;
		_commandsParent.SetParent(transform.parent);
		_commandsParent.localScale = _commandBoxArrow.parent.localScale;

		_start = _commandBoxArrow.Find("Start");
		_end = UIManager.inst.receiver.Find("End");
        
		_distance = transform.Find("Distance").GetComponent<Text>();
		_waves = transform.Find("Waves").GetComponent<Image>();
		_waves.material.hideFlags = HideFlags.DontSave;
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
                _commandBoxArrow.localScale = new Vector3(-1, 1, 1);
                commandImage = Instantiate(leftArrow);
				commandImage.transform.SetParent(_commandsParent);
				commandImage.transform.localScale = new Vector3(-1, 1, 1);
				break;

			case Player.Command.Right:
			default:
				_commandBoxArrow.localScale = new Vector3(1, 1, 1);
                commandImage = Instantiate(rightArrow);
				commandImage.transform.SetParent(_commandsParent);
				commandImage.transform.localScale = new Vector3(1, 1, 1);
				break;
        }

		StartCoroutine(moveSonarCo(delay, commandImage.transform));
    }

	const float MIN_DELAY = 0.1f;
	IEnumerator moveSonarCo(float delay, Transform sonar) {
		delay = Mathf.Max(MIN_DELAY, delay);
		float timer = 0;

		Vector3 startPos = _start.transform.position;
		Vector3 endPos = _end.transform.position;

		sonar.gameObject.SetActive(true);
		sonar.transform.position = _start.position;

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

		_delayIntegral += Time.deltaTime / Mathf.Max(MIN_DELAY, Player.instance.delay);
		_waves.material.SetFloat("_Delay", Player.instance.delay);
		_waves.material.SetFloat("_DelayIntegral", _delayIntegral);
	}

	void OnDisable() {
		StopAllCoroutines();
		if (_commandsParent != null) {
			for (int i = _commandsParent.childCount - 1; i >= 0; i--) {
				Destroy(_commandsParent.GetChild(i).gameObject);
			}
		}
	}
}
