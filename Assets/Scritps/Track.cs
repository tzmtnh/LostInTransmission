using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Track : MonoBehaviour {

	public static Track inst;

	public float speed = 1;
	public float enemySpawnRate = 5;
	public float laneWidth = 1;

	public Hitable astroidPrefab;

	float _traveledDistance = 0;
	float _traveledDistanceDelta = 0;

	Transform _lane1;
	Transform _lane2;
	float _laneLength;

	List<Hitable> _enemies = new List<Hitable>();
	float _lastEnemySpawnTime = 0;

	void initLanes() {
		_lane1 = transform.Find("Lanes");
		Assert.IsNotNull(_lane1);
		_lane1.name = "Lanes 1";
		_laneLength = _lane1.localScale.y;

		_lane2 = Instantiate(_lane1, transform);
		_lane2.name = "Lanes 2";
		_lane2.localPosition += new Vector3(0, 0, _laneLength);
	}

	void updateLanes() {
		Vector3 pos1 = _lane1.localPosition;
		pos1.z = -(_traveledDistance % _laneLength);
		if (-pos1.z > _laneLength) {
			//swap lanes
			Transform temp = _lane1;
			_lane1 = _lane2;
			_lane2 = temp;
			pos1.z += _laneLength;
		}

		_lane1.localPosition = pos1;

		Vector3 pos2 = pos1;
		pos2.z += _laneLength;
		_lane2.localPosition = pos2;
	}

	void updateEnemies() {
		float t = Time.time;
		float timeSinceLastEnemySpawn = t - _lastEnemySpawnTime;
		if (timeSinceLastEnemySpawn >= enemySpawnRate) {
			_lastEnemySpawnTime = t;
			Assert.IsNotNull(astroidPrefab);
			int lane = Random.Range(-1, 2);
			Hitable enemy = Instantiate(astroidPrefab);
			enemy.transform.SetParent(transform);
			enemy.transform.localPosition = new Vector3(lane * laneWidth, 0.5f, _laneLength / 2f);
			_enemies.Add(enemy);
		}

		Vector3 playerPos = Player.instance.transform.localPosition;
		Vector3 offset = new Vector3(0, 0, _traveledDistanceDelta);
		for (int i = _enemies.Count - 1; i >= 0; i--) {
			Hitable enemy = _enemies[i];
			enemy.transform.localPosition -= offset;
			Vector3 enemyPos = enemy.transform.localPosition;

			if (enemy.destroyed == false && enemyPos.z <= playerPos.z) {
				float horizontalDist = Mathf.Abs(enemyPos.x - playerPos.x);
				if (horizontalDist < 0.5f) {
					enemy.destroy();
				}
			}

			if (enemyPos.z < -_laneLength / 2f) {
				_enemies.RemoveAt(i);
				Destroy(enemy.transform.gameObject);
			}
		}
	}

	void Awake() {
		Assert.IsNull(inst, "Only one instance allowed!");
		inst = this;
		initLanes();
	}

	void Update() {
		float dt = Time.deltaTime;
		float lastTraveledDistance = _traveledDistance;
		_traveledDistance += speed * dt;
		_traveledDistanceDelta = _traveledDistance - lastTraveledDistance;

		updateLanes();
		updateEnemies();
	}

}
