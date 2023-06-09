using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathMgr : MonoBehaviour
{
    public static PathMgr inst;

    void Awake()
    {
        inst = this;
    }

    public List<Node> path;

    public List<Node> openSet;
    public List<Node> closedSet;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize()
    {
        path = null;
    }

    public void FindPath()
    {
        openSet = new List<Node>();
        closedSet = new List<Node>();

        openSet.Add(GridMgr.inst.sourceNode);

        while (openSet.Count > 0)
        {
            openSet.Sort(SortByFCost);
            Node currentNode = openSet[0];
            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == GridMgr.inst.sinkNode)
            {
                TracePath();
                ColorPath();
                return;
            }

            foreach (Node neighbor in GridMgr.inst.GetNeighbors(currentNode))
            {
                if (!neighbor.walkable || closedSet.Contains(neighbor))
                {
                    continue;
                }

                float costToNeighbor = currentNode.cost.gCost + GetDisnatce(currentNode, neighbor);
                if(costToNeighbor < neighbor.cost.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.cost.gCost = costToNeighbor;
                    neighbor.cost.hCost = GetDisnatce(neighbor, GridMgr.inst.sinkNode);
                    neighbor.parent = currentNode;

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                        if (neighbor != GridMgr.inst.sinkNode)
                        {
                            neighbor.GetComponentInChildren<SpriteRenderer>().color = Color.red;
                        }
                    }
                }
            }
        }
    }

    void TracePath()
    {
        path = new List<Node>();
        Node currentNode = GridMgr.inst.sinkNode;
        while (currentNode != GridMgr.inst.sourceNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();
    }

    void ColorPath()
    {
        foreach (Node node in path)
        {
            if (node == GridMgr.inst.sourceNode || node == GridMgr.inst.sinkNode)
            {
                continue;
            }

            node.GetComponentInChildren<SpriteRenderer>().color = Color.blue;
        }
    }

    float GetDisnatce(Node n1, Node n2)
    {
        float distX = (float) Mathf.Abs(n1.gridPositionX - n2.gridPositionX);
        float distY = (float) Mathf.Abs(n1.gridPositionY - n2.gridPositionY);

        if (distX > distY)
            return Mathf.Sqrt(2) * distY + (distX - distY);
        return Mathf.Sqrt(2) * distX + (distY - distX);
    }

    static int SortByFCost(Node n1, Node n2)
    {
        int comparisonValue = int.MaxValue;
        if (n1.cost.fCost == n2.cost.fCost)
        {
            comparisonValue = n1.cost.hCost.CompareTo(n2.cost.hCost);
        }
        else if (n1.cost.fCost < n2.cost.fCost)
        {
            comparisonValue = n1.cost.fCost.CompareTo(n2.cost.fCost);
        }
        return comparisonValue;
    }
}
