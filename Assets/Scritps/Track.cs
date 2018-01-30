using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Track : MonoBehaviour {

	class ParticleCacher {
		public ParticleSystem system;
		public ParticleSystemRenderer renderer;
		public float defaultVelocityScale;
	}

	public static Track inst;

    public bool spawnEnabled;

	public float spawnChance = 2;
	public float spawnMinGap = 0.1f;
	public float spawnMaxGap = 3;
	public int spawnPUEvry = 20;
	public float laneWidth = 1;

	public Hitable astroidPrefab;
	public Hitable jumpPrefab;
	public Hitable amplifyPrefab;
	public Hitable repairPrefab;
	public GameObject asteroidExplosionPrefab;

	float _traveledDistance = 0;
	float _traveledDistanceDelta = 0;
	float _lastSpawnTime = 0;

	Transform _lane1;
	Transform _lane2;
	float _laneLength;

	List<Hitable> _hitables = new List<Hitable>(8);

	//AudioSource _glitchSource;
	AudioSource _staticSource;

	ParticleCacher[] _particles;

	int _lastLane = 0;
	int _numAstroidsSinceLastPU = 0;
	float _timeAtLastJump = 0;
	List<Hitable> _availablePUs = new List<Hitable>(4);

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

	void spawnNext() {
		_lastSpawnTime = Time.time;
		Hitable prefab = astroidPrefab;

		_numAstroidsSinceLastPU++;
		if (_numAstroidsSinceLastPU >= spawnPUEvry) {
			const float RANDOM_CHANCE = 0.2f;  // random chance of getting a powerup for no reason
			const float MIN_DELAY = 0.5f;      // minimum delay before we can get Amplify
			const float MIN_JUMP_GAP = 15;     // minimum time sice last Jump before we can get it again

			_availablePUs.Clear();
			if (Player.instance.currentHealth < Player.instance.maxHealth || Random.value < RANDOM_CHANCE)
				_availablePUs.Add(repairPrefab);
			if (Player.instance.delay > MIN_DELAY || Random.value < RANDOM_CHANCE)
				_availablePUs.Add(amplifyPrefab);
			if (Time.time - _timeAtLastJump > MIN_JUMP_GAP || Random.value < RANDOM_CHANCE)
				_availablePUs.Add(jumpPrefab);

			int numPowerups = _availablePUs.Count;
			if (numPowerups > 0) {
				_numAstroidsSinceLastPU = 0;
				int index = (int)(Random.value * numPowerups);
				prefab = _availablePUs[index];
				if (prefab.type == Hitable.HitableType.Jump)
					_timeAtLastJump = Time.time;
			}
		}

		int lane = _lastLane;
		while (lane == _lastLane) {
			lane = Random.Range(-1, 2);
		}
		_lastLane = lane;

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
				needToSpawn = Random.value < spawnChance * Time.deltaTime;
			}
		}

		if (needToSpawn && spawnEnabled) {
			spawnNext();
		}

		Vector3 playerPos = Player.instance.transform.localPosition;
		Vector3 offset = new Vector3(0, 0, _traveledDistanceDelta);
		for (int i = _hitables.Count - 1; i >= 0; i--) {
			Hitable hitable = _hitables[i];
			hitable.transform.localPosition -= offset;
			Vector3 hitablePos = hitable.transform.localPosition;

			if (hitable.destroyed == false && (hitable.canDamage == false || Player.instance.canTakeDamage)) {
				float distX = Mathf.Abs(hitablePos.x - playerPos.x);
				float distZ = Mathf.Abs(hitablePos.z - playerPos.z);

				const float THRESH_X = 0.5f;
				const float THRESH_Z = 0.5f;
				if (distX < THRESH_X && distZ < THRESH_Z) {
					hitable.destroy();
				}
			}

			if (hitablePos.z < -_laneLength / 2f) {
				_hitables.RemoveAt(i);
				Destroy(hitable.transform.gameObject);
			}
		}
	}

	void updateGlitch() {
		float delay = Player.instance.delay;
		//_glitchSource.volume = delay * 0.1f;
		//_glitchSource.pitch = 1f / Mathf.Max(0.01f, delay);

		_staticSource.volume = delay * 0.2f;
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

	public void reset() {
		_traveledDistance = 0;
		_traveledDistanceDelta = 0;
		_lastSpawnTime = Time.time;
		_lastLane = 0;
		_numAstroidsSinceLastPU = 0;
		_timeAtLastJump = _lastSpawnTime;

		foreach (Hitable hitable in _hitables) {
			Destroy(hitable.transform.gameObject);
		}
		_hitables.Clear();

		//if (_glitchSource != null) {
		//	AudioManager.inst.stopSound(_glitchSource);
		//}
		//_glitchSource = AudioManager.inst.playSound("Glitch", volume:0, loop: true);

		if (_staticSource != null) {
			AudioManager.inst.stopSound(_staticSource);
		}
		_staticSource = AudioManager.inst.playSound("Static", volume: 0, loop: true);
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

		initLanes();
	}

	void Start() {
		reset();
	}

	void Update() {
		if (Player.instance.isAlive == false)
			return;

		float dt = Time.deltaTime;
		float lastTraveledDistance = _traveledDistance;
		_traveledDistance += Player.instance.speed * dt;
		_traveledDistanceDelta = _traveledDistance - lastTraveledDistance;

		updateGlitch();
		updateParticles();
		updateLanes();
		updateEnemies();
	}

}
