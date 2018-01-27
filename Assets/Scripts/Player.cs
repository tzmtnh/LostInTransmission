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

    private static readonly float[] LANE_POSITIONS = new float[] { -1.3f, 0, 1.3f };
    private int laneIndex = Array.IndexOf(LANE_POSITIONS, 0);

    public float delay = 0.0f;

    void Awake()
    {
        instance = this;
    }

    void Start () {
        SetLane(this.laneIndex);
	}

	void Update ()
    {
		if (Input.GetKeyDown("left"))
        {
            IssueLeftCommand();
        }
        else if (Input.GetKeyDown("right"))
        {
            IssueRightCommand();
        }
    }

    void IssueLeftCommand()
    {
        if (OnCommand != null)
        {
            OnCommand(Command.Left, this.delay);
        }
        StartCoroutine(MoveLeft(this.delay));
    }

    void IssueRightCommand()
    {
        if (OnCommand != null)
        {
            OnCommand(Command.Right, this.delay);
        }
        StartCoroutine(MoveRight(this.delay));
    }

    IEnumerator MoveLeft(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (laneIndex > 0)
        {
            this.laneIndex--;
            SetLane(this.laneIndex);
        }
    }

    IEnumerator MoveRight(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (laneIndex < LANE_POSITIONS.Length - 1)
        {
            this.laneIndex++;
            SetLane(this.laneIndex);
        }
    }

    void SetLane(int laneIndex)
    {
        var pos = gameObject.transform.position;
        gameObject.transform.position = new Vector3(LANE_POSITIONS[laneIndex], pos.y, pos.z);
    }
}
