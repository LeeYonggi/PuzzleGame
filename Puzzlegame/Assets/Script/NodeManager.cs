using System.Collections;
using System.Collections.Generic;
using UnityEngine;


enum NODE_KIND
{
    EMPTY,
    NUMBER,
    STOP
}

public class NodeArray
{
    private NODE_KIND node_Kind;
    public Vector2 position;
    public bool isNodechange;
    public int nodeNumber;

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
    private float nodeDistance = 1.0f;
    private int NodeSize = 5;

    private Swipe m_Swipe;
    private Vector3 desiredPosition;
    public GameObject testobj;
    private List<List<NodeArray>> nodeArr = new List<List<NodeArray>>();
    private List<List<GameObject>> nodeObjList = new List<List<GameObject>>();

    enum TURN_STATE
    {
        STAY,
        SWIPE,
        MOVE
    }
    private TURN_STATE turn;

    // Use this for initialization
    void Start () {
        m_Swipe = GetComponent<Swipe>();
        InitNodeArr(NodeSize);


        InitNode(1, 0, 3);
        InitNode(3, 0, 1);
        InitNode(2, 2, 3);
        InitNode(4, 1, 2);
        InitStopNode(1, 1);
        InitStopNode(1, 3);
        InitStopNode(2, 1);
        InitStopNode(2, 3);
        InitStopNode(3, 1);
        InitStopNode(3, 3);
        InitStopNode(3, 2);
        InitStopNode(4, 0);
        turn = TURN_STATE.STAY;
    }

    //노트 배열 초기화
    void InitNodeArr(int size)
    {
        for (int i = 0; i < size; i++)
        {
            nodeArr.Add(new List<NodeArray>());
            nodeObjList.Add(new List<GameObject>());
            for (int j = 0; j < size; j++)
            {
                NodeArray temp = new NodeArray();
                temp.Node_Kind = NODE_KIND.EMPTY;
                temp.position = transform.position + new Vector3(-2.2f, 0) + new Vector3(j * nodeDistance, -i * nodeDistance);
                temp.isNodechange = false;
                temp.nodeNumber = 0;
                nodeArr[i].Add(temp);
                nodeObjList[i].Add(null);
            }
        }
    }

    void InitNode(int y, int x, int number)
    {
        nodeObjList[y][x] = Instantiate(testobj, transform);
        nodeObjList[y][x].transform.position = nodeArr[y][x].position;
        nodeObjList[y][x].GetComponent<NodeState>().TargetPosition = nodeArr[y][x].position;
        nodeObjList[y][x].GetComponent<NodeState>().NodeNumber = number;
        nodeArr[y][x].Node_Kind = NODE_KIND.NUMBER;
        nodeArr[y][x].nodeNumber = number;
    }
    void InitStopNode(int y, int x)
    {
        nodeObjList[y][x] = Instantiate(Resources.Load<GameObject>("Prefeb/Nodes/StopNode"), transform);
        nodeObjList[y][x].transform.position = nodeArr[y][x].position;
        nodeArr[y][x].Node_Kind = NODE_KIND.STOP;
        nodeArr[y][x].nodeNumber = 0;
    }
	
	// Update is called once per frame
	void Update () {
        TurnState();
    }

    void TurnState()
    {
        switch (turn)
        {
            case TURN_STATE.STAY:
                if (m_Swipe.IsSwipeing)
                {
                    turn = TURN_STATE.SWIPE;
                    TurnState();
                }
                break;
            case TURN_STATE.SWIPE:
                SwipeNodes();
                turn = TURN_STATE.MOVE;
                break;
            case TURN_STATE.MOVE:
                int number = 0;
                for(int i = 0; i < NodeSize; i++)
                {
                    for (int j = 0; j < NodeSize; j++)
                    {
                        if (MoveNodes(i, j) == false)
                            ++number;
                    }
                }
                if(number == NodeSize * NodeSize)
                {
                    turn = TURN_STATE.STAY;

                    for (int i = 0; i < NodeSize; i++)
                    {
                        for (int j = 0; j < NodeSize; j++)
                        {
                            SetNodeMoveEnd(i, j);
                            nodeArr[i][j].isNodechange = false;
                        }
                    }
                }
                break;
        }
    }

    private void SwipeNodes()
    {
        if (m_Swipe.SwipeLeft)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 1; j < 5; j++)
                {
                    CompareNode(i, j, 0, -1);
                }
            }
        }
        else if (m_Swipe.SwipeRight)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 3; j > -1; j--)
                {
                    CompareNode(i, j, 0, +1);
                }
            }
        }
        else if (m_Swipe.SwipeUp)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 1; j < 5; j++)
                {
                    CompareNode(j, i, -1, 0);
                }
            }
        }
        else if (m_Swipe.SwipeDown)
        {
            for(int i = 0; i < 5; i++)
            {
                for(int j = 3; j > -1; j--)
                {
                    CompareNode(j, i, +1, 0);
                }
            }
        }
    }

    private void CompareNode(int y, int x, int dy, int dx)
    {
        if (x + dx < 0 || x + dx > NodeSize - 1) return;
        if (y + dy < 0 || y + dy > NodeSize - 1) return;
        if (nodeArr[y][x].Node_Kind == NODE_KIND.NUMBER)
        {
            //다음 이동이 비었다면
            if (nodeArr[y + dy][x + dx].Node_Kind == NODE_KIND.EMPTY)
            {
                //다음노드로 현재노드 정보이동
                nodeArr[y + dy][x + dx].Node_Kind = NODE_KIND.NUMBER;
                nodeArr[y + dy][x + dx].nodeNumber = nodeArr[y][x].nodeNumber;
                //현재노드 삭제
                nodeArr[y][x].Node_Kind = NODE_KIND.EMPTY;
                nodeArr[y][x].nodeNumber = 0;
                //실제 노드 오브젝트 타겟 포지션 변경
                nodeObjList[y][x].GetComponent<NodeState>().TargetPosition = nodeArr[y + dy][x + dx].position;
                nodeObjList[y + dy][x + dx] = nodeObjList[y][x];
                nodeObjList[y][x] = null;
                //재귀함수
                CompareNode(y + dy, x + dx, dy, dx);
            }
            else if (nodeArr[y + dy][x + dx].Node_Kind == NODE_KIND.NUMBER && nodeArr[y + dy][x + dx].isNodechange == false)
            {
                if (nodeArr[y + dy][x + dx].nodeNumber == nodeArr[y][x].nodeNumber)
                {
                    //합체된 노드 접근방지
                    nodeArr[y + dy][x + dx].isNodechange = true;
                    nodeArr[y + dy][x + dx].nodeNumber -= 1;
                    //현재 노드 정보를 다음 노드로 이동
                    nodeObjList[y][x].GetComponent<NodeState>().TargetPosition = nodeArr[y + dy][x + dx].position;
                    nodeObjList[y + dy][x + dx].GetComponent<NodeState>().TransNode = nodeObjList[y][x];
                    nodeObjList[y][x] = null;
                    //현재노드 삭제
                    nodeArr[y][x].nodeNumber = 0;
                    nodeArr[y][x].Node_Kind = NODE_KIND.EMPTY;
                }
                return;
            }
        }
    }

    private bool MoveNodes(int y, int x)
    {
        if (nodeObjList[y][x] == null) return false;
        if (nodeObjList[y][x].GetComponent<NodeController>() == null) return false;
        nodeObjList[y][x].GetComponent<NodeController>().MoveNode();
        if (nodeObjList[y][x].GetComponent<NodeState>().IsMoveEnd)
            return false;
        return true;
    }

    private void SetNodeMoveEnd(int y, int x)
    {
        if (nodeObjList[y][x] == null) return;
        if (nodeObjList[y][x].GetComponent<NodeState>() == null) return;
        nodeObjList[y][x].GetComponent<NodeState>().IsMoveEnd = false;
    }

    void SwipeObject()
    {
        //if(IsSwipe())
        {
            //nodeArr[1][1].obj.transform.position = Vector3.MoveTowards(nodeArr[1][1].obj.transform.position, desiredPosition, 3f * Time.deltaTime);
        }

        //testobj.transform.position = Vector3.MoveTowards(testobj.transform.position, desiredPosition, 3f * Time.deltaTime);

        if (m_Swipe.Tap)
            Debug.Log("Tapp!");
    }
}
