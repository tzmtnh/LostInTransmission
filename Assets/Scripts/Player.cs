using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public static Player instance;

    public enum Command
    {
        Left,
        Right
    }
    public delegate void IssueCommand(Command cmd, float delay);
    public static event IssueCommand OnCommand;

    private Queue<Command> commandQueue = new Queue<Command>();

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
    private AnimationCurve laneChangePositionCurve;

    [SerializeField]
    private AnimationCurve laneChangeRotationCurve;

    [SerializeField]
    private float laneChangeRotationAngle = -35;

    private bool isChangingLane = false;
    private float laneChangeStartTime;
    private float laneChangeEndTime;
    private Vector3 startPosition;
    private Vector3 targetPosition;

    public int maxHealth = 3;
    public int currentHealth = 3;

    [SerializeField]
    private float defaultSpeed = 10.0f;

    [SerializeField]
    private float jumpSpeed = 15.0f;

    [SerializeField]
    private float jumpSpeedDuration = 3.0f;

    [SerializeField]
    private float amplifyAmount = .1f;

    void Awake()
    {
        instance = this;
    }

    void Start () {
        _targetSpeed = this.defaultSpeed;
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

        if (this.isChangingLane)
        {
            var t = (Time.time - laneChangeStartTime) / (laneChangeEndTime - laneChangeStartTime);
            var movementVector = targetPosition - startPosition;
            var newPosition = startPosition + movementVector * laneChangePositionCurve.Evaluate(t);
            var newRotation = Quaternion.AngleAxis(laneChangeRotationCurve.Evaluate(t) * laneChangeRotationAngle * (movementVector.x / Math.Abs(movementVector.x)), Vector3.forward);
            this.gameObject.transform.position = newPosition;
            this.gameObject.transform.rotation = newRotation;
            
            if (Time.time > laneChangeEndTime)
            {
                this.isChangingLane = false;
            }
        }
        else
        {
            ReadCommandQueue();
        }
    }

    void ReadCommandQueue()
    {
        if (commandQueue.Count > 0)
        {
            var cmd = commandQueue.Dequeue();
            switch (cmd)
            {
                case Command.Left:
                    MoveLeft();
                    break;
                case Command.Right:
                    MoveRight();
                    break;
            }
        }
    }

    void IssueLeftCommand()
    {
        if (OnCommand != null)
        {
            OnCommand(Command.Left, this.delay);
        }
        StartCoroutine(QueueCommand(Command.Left, this.delay));
    }

    void IssueRightCommand()
    {
        if (OnCommand != null)
        {
            OnCommand(Command.Right, this.delay);
        }
        StartCoroutine(QueueCommand(Command.Right, this.delay));
    }

    IEnumerator QueueCommand(Command cmd, float delay)
    {
        yield return new WaitForSeconds(delay);
        this.commandQueue.Enqueue(cmd);
    }

    void MoveLeft()
    {
        if (this.laneIndex > 0 && !this.isChangingLane)
        {
            this.laneIndex--;
            SetTargetLane(this.laneIndex);
        }
    }

    void MoveRight()
    {
        if (this.laneIndex < LANE_POSITIONS.Length - 1 && !this.isChangingLane)
        {
            this.laneIndex++;
            SetTargetLane(this.laneIndex);
        }
    }

    void ResetToMiddleLane()
    {
        var pos = this.gameObject.transform.position;
        gameObject.transform.position = new Vector3(LANE_POSITIONS[Array.IndexOf(LANE_POSITIONS, 0)], pos.y, pos.z);
    }

    void SetTargetLane(int laneIndex)
    {
        var pos = this.gameObject.transform.position;
        this.laneChangeStartTime = Time.time;
        this.laneChangeEndTime = this.laneChangeStartTime + this.laneChangeTime;
        this.startPosition = pos;
        this.targetPosition = new Vector3(LANE_POSITIONS[laneIndex], pos.y, pos.z);
        this.isChangingLane = true;
    }

    void OnHit(Hitable.HitableType hitType)
    {
        switch (hitType)
        {
            case Hitable.HitableType.Astroid:
                if (this.currentHealth > 0)
                {
                    this.currentHealth--;
                }
                break;
            case Hitable.HitableType.Repair:
                if (this.currentHealth < this.maxHealth)
                {
                    this.currentHealth++;
                }
                break;
            case Hitable.HitableType.Amplify:
                this.delay = Mathf.Max(0, this.delay - this.amplifyAmount);
                break;
            case Hitable.HitableType.Jump:
                _targetSpeed = this.jumpSpeed;
                StartCoroutine(RevertToDefaultSpeed(jumpSpeedDuration));
                break;
        }
    }

    IEnumerator RevertToDefaultSpeed(float duration)
    {
        yield return new WaitForSeconds(duration);
        _targetSpeed = this.defaultSpeed;
    }

    public void Reset()
    {
        this.distance = 0;
        this.delay = 0;
        this.currentHealth = maxHealth;
        this.gameObject.SetActive(true);
        this.ResetToMiddleLane();
    }
}
