using System.Collections;
using System.Collections.Generic;
using UnityEngine;


enum NODE_KIND
{
    EMPTY,
    NUMBER
}
public class NodeArray
{
    private NODE_KIND node_Kind;
    public GameObject obj;

    #region GetSet
    internal NODE_KIND Node_Kind
    {
        get
        {
            return node_Kind;
        }

        set
        {
            node_Kind = value;
        }
    }
    #endregion
}
public class NodeManager : MonoBehaviour {

    private Swipe m_Swipe;
    private Vector3 desiredPosition;
    public GameObject testobj;
    private List<List<NodeArray>> nodeArr = new List<List<NodeArray>>();

	// Use this for initialization
	void Start () {
        m_Swipe = GetComponent<Swipe>();
        initNodeArr(5);

        nodeArr[1][1].obj = Instantiate(testobj, transform);

    }

    void initNodeArr(int size)
    {
        for (int i = 0; i < size; i++)
        {
            nodeArr.Add(new List<NodeArray>());
            for (int j = 0; j < size; j++)
            {
                NodeArray temp = new NodeArray();
                temp.Node_Kind = NODE_KIND.EMPTY;
                temp.obj = null;
                nodeArr[i].Add(temp);
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        SwipeObject();
    }

    bool IsSwipe()
    {
        if (m_Swipe.SwipeLeft)
        {
            desiredPosition = Vector3.left;
            return true;
        }
        if (m_Swipe.SwipeRight)
        {
            desiredPosition = Vector3.right;
            return true;
        }
        if (m_Swipe.SwipeUp)
        {
            desiredPosition = Vector3.up;
            return true;
        }
        if (m_Swipe.SwipeDown)
        {
            desiredPosition = Vector3.down;
            return true;
        }
        desiredPosition = Vector3.zero;
        return false;
    }

    void SwipeObject()
    {
        if(IsSwipe())
        {
            nodeArr[1][1].obj.transform.position = Vector3.MoveTowards(nodeArr[1][1].obj.transform.position, desiredPosition, 3f * Time.deltaTime);
        }

        //testobj.transform.position = Vector3.MoveTowards(testobj.transform.position, desiredPosition, 3f * Time.deltaTime);

        if (m_Swipe.Tap)
            Debug.Log("Tapp!");
    }
}
