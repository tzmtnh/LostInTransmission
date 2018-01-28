using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Player : MonoBehaviour {

    public static Player instance;

    public enum Command
    {
        Left,
        Right
    }
    public delegate void IssueCommand(Command cmd, float delay);
    public static event IssueCommand OnCommand;

    private static readonly float[] LANE_POSITIONS = new float[] { -1, 0, 1 };
    private int laneIndex = Array.IndexOf(LANE_POSITIONS, 0);

	private float _speed;
	private float _targetSpeed;
	private float _speedVelocity; // used by SmoothDamp
	public float speed { get { return _speed; } }
	public float normalizedSpeed { get { return _speed / defaultSpeed; } }
	public float lightSpeedParam { get { return Mathf.InverseLerp(defaultSpeed, jumpSpeed, _speed); } }

    public float distance = 0;

    public float delay = 0.0f;

    [SerializeField]
    private float laneChangeTime = 1.0f;

    [SerializeField]
    private float laneChangeMaxYaw = 35;

    [SerializeField]
    private float laneChangeYawIntensity = 10;

    Vector3 velocity = Vector3.zero;
    private Vector3 targetPosition;

    public int maxHealth = 3;
    public int currentHealth = 3;

    public Texture fullHealthTexture;
    public Texture damageTexture1;
    public Texture damageTexture2;

    public ParticleSystem damageParticles;
    public GameObject deathExplosion;
    private SpriteRenderer _deathExplosionRenderer;
    public ParticleSystem deathParticles;

    private GameObject shipContainer;

    [SerializeField]
    private float defaultSpeed = 10.0f;

    [SerializeField]
    private float jumpSpeed = 15.0f;

    [SerializeField]
    private float jumpSpeedDuration = 3.0f;

    [SerializeField]
    private float amplifyAmount = .1f;

    public bool isDamageable = true;

	Material _shipMaterial;
	int _GlowID;
	float _glow = 0;
	float _glowTarget = 0;
	float _glowVelocity = 0;

	void Awake()
    {
        instance = this;
    }

    void Start () {
		_speed = defaultSpeed;
		_targetSpeed = defaultSpeed;

        shipContainer = GameObject.Find("Spaceship_container");

        Transform ship = shipContainer.transform.Find("Spaceship");
		Assert.IsNotNull(ship);
		_shipMaterial = ship.GetComponent<Renderer>().material;
		_GlowID = Shader.PropertyToID("_Glow");

        _deathExplosionRenderer = deathExplosion.GetComponent<SpriteRenderer>();
        _deathExplosionRenderer.enabled = false;

        ResetToMiddleLane();
        Hitable.onHitableHit += OnHit;
    }

	void Update ()
    {
        if (this.currentHealth <= 0)
        {
            return;
        }
        
        this.delay += Time.deltaTime * .01f;
        this.distance += Time.deltaTime * speed;

		const float SPEED_SMOOTH_TIME = 1;
		_speed = Mathf.SmoothDamp(_speed, _targetSpeed, ref _speedVelocity, SPEED_SMOOTH_TIME);

		if (Input.GetKeyDown("left"))
        {
			AudioManager.inst.playSound("Click");
			IssueLeftCommand();
        }
        else if (Input.GetKeyDown("right"))
        {
			AudioManager.inst.playSound("Click");
			IssueRightCommand();
        }

        this.gameObject.transform.position = Vector3.SmoothDamp(this.gameObject.transform.position, this.targetPosition, ref velocity, this.laneChangeTime);
        this.gameObject.transform.rotation = Quaternion.AngleAxis(Mathf.Clamp(-velocity.x * laneChangeYawIntensity, -laneChangeMaxYaw, laneChangeMaxYaw), Vector3.forward);

		_glowTarget = isDamageable ? 0 : 1;
		_glow = Mathf.SmoothDamp(_glow, _glowTarget, ref _glowVelocity, 0.2f);
		_shipMaterial.SetFloat(_GlowID, _glow);
	}

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(targetPosition, .1f);
    }

    void IssueLeftCommand()
    {
        if (OnCommand != null)
        {
            OnCommand(Command.Left, this.delay);
        }
        Invoke("MoveLeft", this.delay);
    }

    void IssueRightCommand()
    {
        if (OnCommand != null)
        {
            OnCommand(Command.Right, this.delay);
        }
        Invoke("MoveRight", this.delay);
    }

    void MoveLeft()
    {
        if (this.laneIndex > 0)
        {
            SetTargetLane(--this.laneIndex);
        }
    }

    void MoveRight()
    {
        if (this.laneIndex < LANE_POSITIONS.Length - 1)
        {
            SetTargetLane(++this.laneIndex);
        }
    }

    void ResetToMiddleLane()
    {
        this.laneIndex = Array.IndexOf(LANE_POSITIONS, 0);
        var pos = this.gameObject.transform.position;
        gameObject.transform.position = new Vector3(LANE_POSITIONS[this.laneIndex], pos.y, pos.z);
        SetTargetLane(this.laneIndex);
    }

    void SetTargetLane(int laneIndex)
    {
        var pos = this.gameObject.transform.position;
        this.targetPosition = new Vector3(LANE_POSITIONS[laneIndex], pos.y, pos.z);
    }

	void canTakeDamageAgain() {
		isDamageable = true;
	}

    void OnHit(Hitable.HitableType hitType)
    {
        switch (hitType)
        {
            case Hitable.HitableType.Astroid:
                if (this.currentHealth > 0)
                {
                    SetTexture(--this.currentHealth);

                    if (this.currentHealth > 0)
                    {
                        damageParticles.Play();
                        isDamageable = false;

						const float SHIELD_TIME = 2;
                        Invoke("canTakeDamageAgain", SHIELD_TIME);

						const float SHAKE_TIME = 0.3f;
						CameraControl.inst.shake(SHAKE_TIME);
                    }
                    else
                    {
                        this.isDamageable = false;
                        Explode();
                    }
                    
                }
                break;
            case Hitable.HitableType.Repair:
                if (this.currentHealth < this.maxHealth)
                {
                    SetTexture(++this.currentHealth);
                }
                break;
            case Hitable.HitableType.Amplify:
                this.delay = Mathf.Max(0, this.delay - this.amplifyAmount);
                break;
            case Hitable.HitableType.Jump:
                _targetSpeed = this.jumpSpeed;
                isDamageable = false;
                StartCoroutine(RevertToDefaultSpeed(jumpSpeedDuration));
                break;
        }
    }

    IEnumerator RevertToDefaultSpeed(float duration)
    {
        yield return new WaitForSeconds(duration);
        _targetSpeed = this.defaultSpeed;
        isDamageable = true;
    }

    private void SetTexture(int health) {
        switch (this.currentHealth)
        {
            case 3:
				_shipMaterial.mainTexture = fullHealthTexture;
                break;
            case 2:
				_shipMaterial.mainTexture = damageTexture1;
                break;
            case 1:
				_shipMaterial.mainTexture = damageTexture2;
                break;
        }
    }

    void Explode()
    {
        _deathExplosionRenderer.enabled = true;
        this.shipContainer.SetActive(false);
        deathParticles.Play();
        Invoke("EndExplosion", .4f);

		const float SHAKE_TIME = 1f;
		CameraControl.inst.shake(SHAKE_TIME);
	}

    void EndExplosion()
    {
        _deathExplosionRenderer.enabled = false;
    }

    public void Reset()
    {
        this.distance = 0;
        this.delay = 0;
        this.currentHealth = maxHealth;
        this.isDamageable = true;
        this.shipContainer.SetActive(true);
        this.ResetToMiddleLane();
        this.SetTexture(this.currentHealth);
    }
}
