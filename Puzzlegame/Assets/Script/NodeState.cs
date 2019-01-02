using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeState : MonoBehaviour {

    private Vector2 nodePosition;
    private Vector2 targetPosition;
    private GameObject transNode;
    private bool isMoveEnd;
    private int nodeNumber;

    #region GETSET
    public Vector2 NodePosition
    {
        get
        {
            return nodePosition;
        }

        set
        {
            nodePosition = value;
        }
    }

    public Vector2 TargetPosition
    {
        get
        {
            return targetPosition;
        }

        set
        {
            targetPosition = value;
        }
    }

    public GameObject TransNode
    {
        get
        {
            return transNode;
        }

        set
        {
            transNode = value;
        }
    }

    public bool IsMoveEnd
    {
        get
        {
            return isMoveEnd;
        }

        set
        {
            isMoveEnd = value;
        }
    }

    public int NodeNumber
    {
        get
        {
            return nodeNumber;
        }

        set
        {
            nodeNumber = value;
        }
    }


    #endregion

    // Use this for initialization
    void Start () {
        transNode = null;
        isMoveEnd = false;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
