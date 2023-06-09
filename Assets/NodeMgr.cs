using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NodeMgr : MonoBehaviour
{
    public static NodeMgr inst;

    void Awake()
    {
        inst = this;
    }

    public Node nodePrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool CreateNode(Vector3 position, int i, int j)
    {
        Node node = Instantiate(nodePrefab, position, Quaternion.Euler(0f, 0f, 0f), transform);
        if (node != null)
        {
            node.walkable = Random.Range(0f,1f) > .3f;
            node.gridPositionX = i;
            node.gridPositionY = j;
            GridMgr.inst.grid[i, j] = node;
            return true;
        }
        return false;
    }

    public bool DeleteNode(Node node)
    {
        if (GridMgr.inst.grid[node.gridPositionX, node.gridPositionY] != node)
        {
            return false;
        }
        GridMgr.inst.grid[node.gridPositionX, node.gridPositionY] = null;
        Destroy(node.gameObject);
        Destroy(node);
        return true;
    }

    public bool DestoryAllNodes()
    {
        foreach (Node node in GridMgr.inst.grid)
        {
            if (!DeleteNode(node))
            {
                return false;
            }
        }
        return true;
    }
}
