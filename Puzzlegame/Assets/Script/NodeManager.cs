using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour {

    private Swipe m_Swipe;
    public GameObject testobj;
    private Vector3 desiredPosition;

	// Use this for initialization
	void Start () {
        m_Swipe = GetComponent<Swipe>();
    }
	
	// Update is called once per frame
	void Update () {
        if (m_Swipe.SwipeLeft)
            desiredPosition += Vector3.left;
        if (m_Swipe.SwipeRight)
            desiredPosition += Vector3.right;
        if (m_Swipe.SwipeUp)
            desiredPosition += Vector3.up;
        if (m_Swipe.SwipeDown)
            desiredPosition += Vector3.down;

        testobj.transform.position = Vector3.MoveTowards(testobj.transform.position, desiredPosition, 3f * Time.deltaTime);

        if (m_Swipe.Tap)
            Debug.Log("Tapp!");
    }
}
