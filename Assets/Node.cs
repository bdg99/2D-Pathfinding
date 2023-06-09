using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Cost
{
    public float hCost;
    public float gCost;
    public float fCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    public Cost()
    {

    }
}

public class Node : MonoBehaviour
{
    public bool walkable = true;
    public Vector3 worldPosition = new Vector2(0f, 0f);

    public int gridPositionX;
    public int gridPositionY;

    public Cost cost;
    public Node parent;

    // Start is called before the first frame update
    void Start()
    {
        worldPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        walkable = false;
        worldPosition = Vector2.zero;
        cost = null;
    }
}
