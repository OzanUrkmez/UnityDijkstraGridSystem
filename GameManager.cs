using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    #region Unity and Instantiation

    private static GameManager singleton;

    void Start()
    {
        if(singleton != null)
        {
            Destroy(gameObject);
            return;
        }
        singleton = this;
        InitializeGame();
    }

    private void OnDestroy()
    {
        if(singleton == this)
        {
            singleton = null;
        }
    }

    #endregion

    private void InitializeGame()
    {
        PrepareGame();
    }

    private void PrepareGame()
    {
        Grid.InitializeGridMap(this);
    }

    public static void InitializeLateInitialization()
    {
        singleton.LaterGameInitialization();
    }

    private void LaterGameInitialization()
    {
        Grid.VisualizeGrids();
    }
}
