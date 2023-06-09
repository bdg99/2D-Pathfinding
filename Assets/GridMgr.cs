using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GridDimensions
{
    public int width;
    public int widthUpperBound;
    public int widthLowerBound;

    public int height;
    public int heightUpperBound;
    public int heightLowerBound;

    public GridDimensions(int width, int height)
    {
        this.width = width;
        this.widthLowerBound = -width / 2;
        this.widthUpperBound = width / 2;

        this.height = height;
        this.heightLowerBound = -height / 2;
        this.heightUpperBound = height / 2;

        if (width % 2f > .5f)
            this.widthUpperBound += 1;
        if (height % 2f > .5f)
            this.heightUpperBound += 1;
    }
}

public class GridMgr : MonoBehaviour
{
    public static GridMgr inst;

    void Awake()
    {
        inst = this;
    }

    public int width =  20;
    public int height = 20;

    public Node[,] grid;

    public Node sourceNode;
    public Node sinkNode;

    public bool isInitialized = false;

    public GridDimensions gridDimensions = null;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isInitialized == false)
            Initialize();
    }

    public void Initialize()
    {
        BuildGrid();
        isInitialized = true;
    }

    public void BuildGrid()
    {
        if (GameMgr.inst.gameStarted == true)
        {
            DeleteGrid();
        }
        if (CreateGrid())
        {
            SetSourceAndSink();
            ColorGrid();
        }
    }

    bool CreateGrid()
    {
        gridDimensions = new GridDimensions(width, height);
        grid = new Node[gridDimensions.width, gridDimensions.height];
        for (int i = gridDimensions.widthLowerBound; i < gridDimensions.widthUpperBound; i++)
        {
            for (int j = gridDimensions.heightLowerBound; j < gridDimensions.heightUpperBound; j++)
            {
                if (!NodeMgr.inst.CreateNode(new Vector2(i, j), i - gridDimensions.widthLowerBound, j - gridDimensions.heightLowerBound))
                    return false;
            }
        }
        return true;
    }

    bool DeleteGrid()
    {
        bool nodesDestroyed = NodeMgr.inst.DestoryAllNodes();
        gridDimensions = null;
        grid = null;
        isInitialized = false;
        return nodesDestroyed;
    }

    void SetSourceAndSink()
    {
        sourceNode = grid[Random.Range(0, gridDimensions.width - 1), Random.Range(0, gridDimensions.height - 1)];
        while (!sourceNode.walkable)
        {
            sourceNode = grid[Random.Range(0, gridDimensions.width - 1), Random.Range(0, gridDimensions.height - 1)];
        }

        sinkNode = grid[Random.Range(0, gridDimensions.width - 1), Random.Range(0, gridDimensions.height - 1)];
        while (!sinkNode.walkable || sinkNode == sourceNode)
        {
            sinkNode = grid[Random.Range(0, gridDimensions.width - 1), Random.Range(0, gridDimensions.height - 1)];
        }
    }

    public void ColorGrid()
    {
        for (int i = 0; i < gridDimensions.width; i++)
        {
            for (int j = 0; j < gridDimensions.height; j++)
            {
                if (!grid[i, j].walkable)
                {
                    grid[i, j].GetComponentInChildren<SpriteRenderer>().color = Color.black;
                }
            }
        }
        sourceNode.GetComponentInChildren<SpriteRenderer>().color = Color.green;
        sinkNode.GetComponentInChildren<SpriteRenderer>().color = Color.green;
    }

    public List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();

        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (i == 0 && j == 0)
                    continue;
                if (CheckInsideBounds(node.gridPositionX + i, node.gridPositionY + j))
                    neighbors.Add(grid[node.gridPositionX + i, node.gridPositionY + j]);

            }
        }

        return neighbors;
    }

    bool CheckInsideBounds(int i, int j)
    {
        bool isOutOfBounds = i < 0 || j < 0 || i > gridDimensions.width - 1 || j > gridDimensions.height - 1;
        return !isOutOfBounds;
    }
}
