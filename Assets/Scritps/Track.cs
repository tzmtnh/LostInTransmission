using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Track : MonoBehaviour {

	[System.Serializable]
	public struct Spawnable {
		[HideInInspector] public string name;
		public Hitable hitable;
		public float probability;
	}

	class ParticleCacher {
		public ParticleSystem system;
		public ParticleSystemRenderer renderer;
		public float defaultVelocityScale;
	}

	public static Track inst;

	public float spawnChange = 0.1f;
	public float spawnMinGap = 0.1f;
	public float spawnMaxGap = 3;
	public float laneWidth = 1;

	public Spawnable[] spawnablePrefabs;

	float _traveledDistance = 0;
	float _traveledDistanceDelta = 0;
	float _lastSpawnTime = 0;

	Transform _lane1;
	Transform _lane2;
	float _laneLength;

	List<Hitable> _hitables = new List<Hitable>(8);
	float _totalProbability = 0;

	AudioSource _glitchSource;

	ParticleCacher[] _particles;


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

	void validate() {
		_totalProbability = 0;
		for (int i = 0; i < spawnablePrefabs.Length; i++) {
			if (spawnablePrefabs[i].hitable != null)
				spawnablePrefabs[i].name = spawnablePrefabs[i].hitable.name;
			_totalProbability += spawnablePrefabs[i].probability;
		}
	}

	void spawnRandomHitalbe() {
		_lastSpawnTime = Time.time;
		float rand = Random.value * _totalProbability;
		float current = 0;
		Hitable prefab = null;
		for (int i = 0; i < spawnablePrefabs.Length; i++) {
			current += spawnablePrefabs[i].probability;
			if (current >= rand) {
				prefab = spawnablePrefabs[i].hitable;
				break;
			}
		}

		Assert.IsNotNull(prefab);
		int lane = Random.Range(-1, 2);
		Hitable hitable = Instantiate(prefab);
		hitable.transform.SetParent(transform);
		hitable.transform.localPosition = new Vector3(lane * laneWidth, 0.5f, _laneLength / 2f);
		_hitables.Add(hitable);
	}

	void updateEnemies() {
		float timeSinceLastSpawn = Time.time - _lastSpawnTime;
		// make sure we don't spawn too much
		bool needToSpawn = timeSinceLastSpawn >= spawnMinGap;
		if (needToSpawn) {
			// make sure we spawn too sparsly
			needToSpawn = timeSinceLastSpawn >= spawnMaxGap;
			// randomize spawning
			if (needToSpawn == false) {
				needToSpawn = Random.value < spawnChange * Time.deltaTime;
			}
		}

		if (needToSpawn) {
			spawnRandomHitalbe();
		}

		Vector3 playerPos = Player.instance.transform.localPosition;
		Vector3 offset = new Vector3(0, 0, _traveledDistanceDelta);
		for (int i = _hitables.Count - 1; i >= 0; i--) {
			Hitable hitable = _hitables[i];
			hitable.transform.localPosition -= offset;
			Vector3 hitablePos = hitable.transform.localPosition;

			float distX = Mathf.Abs(hitablePos.x - playerPos.x);
			float distZ = Mathf.Abs(hitablePos.z - playerPos.z);

			const float THRESH_X = 0.5f;
			const float THRESH_Z = 0.5f;
			if (hitable.destroyed == false && distX < THRESH_X && distZ < THRESH_Z) {
				hitable.destroy();
			}

			if (hitablePos.z < -_laneLength / 2f) {
				_hitables.RemoveAt(i);
				Destroy(hitable.transform.gameObject);
			}
		}
	}

	void updateGlitch() {
		float delay = Player.instance.delay;
		_glitchSource.volume = delay * 0.1f;
		_glitchSource.pitch = 1f / Mathf.Max(0.01f, delay);
	}

	void updateParticles() {
		float speedMultiplier = Player.instance.normalizedSpeed;
		for (int i = 0; i < _particles.Length; i++) {
			ParticleCacher pc = _particles[i];
			var main = pc.system.main;
			main.simulationSpeed = speedMultiplier;
			float baseSpeed = pc.defaultVelocityScale;
			pc.renderer.velocityScale = baseSpeed * (1f + (speedMultiplier - 1f) / 2f);
		}
	}

	void Awake() {
		Assert.IsNull(inst, "Only one instance allowed!");
		inst = this;

		ParticleSystem[] particles = transform.parent.GetComponentsInChildren<ParticleSystem>();
		_particles = new ParticleCacher[particles.Length];
		for (int i = 0; i < particles.Length; i++) {
			ParticleCacher pc = new ParticleCacher();
			pc.system = particles[i];
			pc.renderer = particles[i].GetComponent<ParticleSystemRenderer>();
			pc.defaultVelocityScale = pc.renderer.velocityScale;
			_particles[i] = pc;
		}

		validate();
		initLanes();
	}

	void Start() {
		_glitchSource = AudioManager.inst.playSound("Glitch", loop:true);
	}

	void Update() {
		float dt = Time.deltaTime;
		float lastTraveledDistance = _traveledDistance;
		_traveledDistance += Player.instance.speed * dt;
		_traveledDistanceDelta = _traveledDistance - lastTraveledDistance;

		updateLanes();
		updateEnemies();
		updateGlitch();
		updateParticles();
	}

	void OnValidate() {
		validate();
	}
}
