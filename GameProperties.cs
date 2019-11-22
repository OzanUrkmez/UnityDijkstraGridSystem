using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameProperties : MonoBehaviour
{

    #region Player-Set Properties

    #endregion

    #region Development Properties

    [SerializeField]
    private Vector3 gridDimensions;

    #endregion

    #region Initialization and Unity

    private static GameProperties singleton;

    void Start()
    {
        if(singleton != null)
        {
            Destroy(gameObject);
            return;
        }
        singleton = this;
    }

    #endregion

    #region Property Getters

    public static Vector3 GetGridDimensions()
    {
        return singleton.gridDimensions;
    }

    #endregion
}
