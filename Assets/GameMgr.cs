using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr : MonoBehaviour
{
    public static GameMgr inst;

    void Awake()
    {
        inst = this;
    }

    public bool gameStarted = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameStarted == false && GridMgr.inst.isInitialized)
        {
            PathMgr.inst.FindPath();
            gameStarted = true;
        }
    }

    public void ResetGame()
    {
        PathMgr.inst.Initialize();
        GridMgr.inst.Initialize();
        gameStarted = false;
    }
}
