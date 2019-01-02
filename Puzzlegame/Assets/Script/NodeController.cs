using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeController : MonoBehaviour {

    const float MoveSpeed = 10.0f;
    private NodeState mNodeState;
    private SpriteRenderer mSpRenderer;


	// Use this for initialization
	void Start () {
        mNodeState = GetComponent<NodeState>();
        mSpRenderer = GetComponent<SpriteRenderer>();
        SpriteChange(mNodeState.NodeNumber);
    }
	
	// Update is called once per frame
	void Update () {
        //mSpRenderer.sprite = Resources.Load<Sprite>("AnotherNode/StopNode");
    }

    public void SpriteChange(int number)
    {
        string path = "NumberNode/Node" + number.ToString();
        mSpRenderer.sprite = Resources.Load<Sprite>(path);
    }
    
    public void MoveNode()
    {
        //노드 이동
        transform.position = Vector3.MoveTowards(transform.position, mNodeState.TargetPosition, MoveSpeed * Time.deltaTime);

        //만약 합체되는 노드가 있다면
        if(mNodeState.TransNode != null)
        {
            //합체되는 노드 이동
            GameObject tempNode = mNodeState.TransNode;
            tempNode.transform.position = Vector3.MoveTowards(tempNode.transform.position,
                tempNode.GetComponent<NodeState>().TargetPosition, MoveSpeed * Time.deltaTime);
            //합체되는 노드, 현재노드 이동확인
            if(tempNode.transform.position == (Vector3)tempNode.GetComponent<NodeState>().TargetPosition
                && transform.position == (Vector3)(mNodeState.TargetPosition))
            {
                mNodeState.IsMoveEnd = true;

                SpriteChange(--mNodeState.NodeNumber);

                //합체되는 노드 삭제
                Destroy(mNodeState.TransNode);
                mNodeState.TransNode = null;
            }
        }
        else
        {
            //노드 이동 확인
            if(transform.position == (Vector3)(mNodeState.TargetPosition))
            {
                mNodeState.IsMoveEnd = true;
            }
        }
    }
}
