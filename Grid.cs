﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuestryGameGeneral.PathFinding;
using System;

public class Grid : MonoBehaviour
{

    [SerializeField]
    private GridRelations gridRelations; //you have to pass in an array of 8, even if some values are null.

    private static Grid startGrid;

    private static DijkstraMap<Grid> gridMap;

    private bool positionSet = false;

    #region Unity and Instantiation

    // Start is called before the first frame update
    void Start()
    {
        List<DijkstraMap<Grid>.NodeLink> nodeLinks = new List<DijkstraMap<Grid>.NodeLink>();
        foreach(Grid g in gridRelations.GetGrids())
        {
            if (g == null)
                continue;
            nodeLinks.Add(new DijkstraMap<Grid>.NodeLink() { distance = 1, endObject = g }); //you may modify the distances for some interesting effects. Here, however, a more conventional definition was used.
        }
        gridMap.AddNode(this, nodeLinks);
        if(startGrid == null)
        {
            startGrid = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        if(startGrid == this)
        {
            startGrid = null;
        }
    }

    #endregion

    #region Getters and Setters

    #region Getters

    public GridRelations GetGridRelations()
    {
        return gridRelations;
    }

    #endregion

    #endregion

    #region Static Functions

    #region Grid Map Functions

    public static void InitializeGridMap(GameManager manager)
    {
        if (manager == null)
            return;
        gridMap = new DijkstraMap<Grid>();
    }

    #endregion

    #region Visualization

    public static void VisualizeGrids()
    {
        startGrid.ExecuteVisualizationProcedure(startGrid.transform.position, GameProperties.GetGridDimensions());
    }

    private void ExecuteVisualizationProcedure(Vector3 pos, Vector3 scale)
    {
        transform.position = pos;
        transform.localScale = scale;
        positionSet = true;
        gridRelations.ExecuteOnNonNullGrids(delegate (Grid g, Vector2 v)
        {
            if (g.positionSet)
                return;
            g.ExecuteVisualizationProcedure(new Vector3(pos.x + (scale.x / 2) * v.x, pos.y, pos.z + (scale.z / 2) * v.y), scale); //if you change this you can create a non-symmetrical Grid system with this code. Like some grids can be larger than others etc. Of course you'd also have to change the relations and the distances in the pathfinding system. Nonetheless some interesting behaviour may be created.
        });
    }

    #endregion

    #endregion
}

[Serializable]
public struct GridRelations
{
    public Grid[] relatedGrids; //clockwise, starting from 0 = upperGrid

    public GridRelations(bool created = true)
    {
        relatedGrids = new Grid[8]; 
    }


    public Grid GetGrid(byte directionID)
    {
        try
        {
            return relatedGrids[directionID];
        }
        catch
        {
            throw new Exception("the Direction ID provided does not match a direction regarding Grid queries.");
        }
    }

    public Grid[] GetGrids()
    {
        return relatedGrids;
    }

    public Vector2 GetIDDirection(byte directionID)
    {
        try
        {
            switch (directionID)
            {
                case 0:
                    return new Vector2(0, 1);
                case 1:
                    return new Vector2(1, 1).normalized;
                case 2:
                    return new Vector2(1, 0);
                case 3:
                    return  new Vector2(1, -1).normalized;
                case 4:
                    return  new Vector2(0, -1);
                case 5:
                    return new Vector2(-1, -1).normalized;
                case 6:
                    return  new Vector2(-1, 0);
                case 7:
                    return  new Vector2(-1, 1).normalized;
            }
        }
        catch
        {
            throw new Exception("the Direction ID provided does not match a direction regarding Grid queries.");
        }
        return Vector2.negativeInfinity;
    }

    public GridRelation GetGridRelation(byte directionID)
    {
        try
        {
             return new GridRelation(relatedGrids[directionID], GetIDDirection(directionID));
        }
        catch
        {
            throw new Exception("the Direction ID provided does not match a direction regarding Grid queries.");
        }
    }

    public GridRelation[] GetGridRelations()
    {
        GridRelation[] relations = new GridRelation[8];
        for(byte i = 0; i < 8; i++)
        {
            relations[i] = GetGridRelation(i);
        }
        return relations;
    }

    public void ExecuteOnNonNullGrids(Action<Grid, Vector2> action)
    {
        GridRelation[] gridRelations = GetGridRelations();
        foreach(GridRelation g in gridRelations)
        {
            if (g.grid == null)
                continue;

            action(g.grid, g.direction);
        }
    }

}

public struct GridRelation
{
    public Vector2 direction;
    public Grid grid;

    public GridRelation(Grid g, Vector2 d)
    {
        direction = d;
        grid = g;
    }
}
