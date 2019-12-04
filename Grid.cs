using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using QuestryGameGeneral.PathFinding;
public class Grid : MonoBehaviour
{

    [SerializeField]
    private GridRelations gridRelations; //you have to pass in an array of 8, even if some values are null.

    [SerializeField]
    private bool isStartGrid = false; //this is primarily so that the semi-automatize function will work.

    private static Grid startGrid;

    private static DijkstraMap<Grid> gridMap = new DijkstraMap<Grid>();

    private bool positionSet = false;

    #region Unity and Instantiation

    // Start is called before the first frame update
    void Start()
    {
        if (isStartGrid)
        {
            startGrid = this;
        }
        List<DijkstraMap<Grid>.NodeLink> nodeLinks = new List<DijkstraMap<Grid>.NodeLink>();
        foreach(Grid g in gridRelations.GetGrids())
        {
            if (g == null)
                continue;
            nodeLinks.Add(new DijkstraMap<Grid>.NodeLink() { distance = 1, endObject = g }); //you may modify the distances for some interesting effects. Here, however, a more conventional definition was used.
        }

        gridMap.AddNode(this, nodeLinks);
   
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

    public static Stack<Grid> FindPath(Grid source, Grid goal)
    {
        return gridMap.FindPath(source, goal);
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
            SemiAutomatizeRelations(g, v);
            g.ExecuteVisualizationProcedure(new Vector3(pos.x + (scale.x) * v.x, pos.y,
                pos.z + (scale.z) * v.y), scale); //if you change this you can create a non-symmetrical Grid system with this code. Like some grids can be larger than others etc. Of course you'd also have to change the relations and the distances in the pathfinding system. Nonetheless some interesting behaviour may be created.
        });
    }

    private void SemiAutomatizeRelations(Grid g, Vector2 v) //delete or do not call this if you would like even higher flexibility.
    {
        g.gridRelations.relatedGrids[GridRelations.DirectionToID(-v)] = this;
        for (byte i = 0; i < 8; i++)
        {
            byte id = GridRelations.DirectionToID(GridRelations.GetIDDirection(i) - v);
            if (id == 255)
                continue;
            g.gridRelations.relatedGrids[id] = gridRelations.GetGrid(i);
        }
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

    #region Static Queries

    public static byte NormalizedDirectionToID(Vector2 v)
    {
        if (v.x == 0)
        {
            if (v.y == 1)
            {
                return 0;
            }
            else if (v.y == -1)
            {
                return 4;
            }
        }
        else if (v.x == 1 || v.x == Mathf.Sqrt(0.5f))
        {
            if (v.y == -Mathf.Sqrt(0.5f))
            {
                return 3;
            }
            else if (v.y == 0)
            {
                return 2;
            }
            else if (v.y == Mathf.Sqrt(0.5f))
            {
                return 1;
            }
        }
        else if (v.x == -1 || v.x == -Mathf.Sqrt(0.5f))
        {
            if (v.y == -Mathf.Sqrt(0.5f))
            {
                return 5;
            }
            else if (v.y == 0)
            {
                return 6;
            }
            else if (v.y == Mathf.Sqrt(0.5f))
            {
                return 7;
            }
        }

        return 255;
    }

    public static Vector2 GetNormalizedIDDirection(byte directionID)
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
                    return new Vector2(1, -1).normalized;
                case 4:
                    return new Vector2(0, -1);
                case 5:
                    return new Vector2(-1, -1).normalized;
                case 6:
                    return new Vector2(-1, 0);
                case 7:
                    return new Vector2(-1, 1).normalized;
            }
        }
        catch
        {
            throw new Exception("the Direction ID provided does not match a direction regarding Grid queries.");
        }
        return Vector2.negativeInfinity;
    }

    public static Vector2 GetIDDirection(byte directionID)
    {
        try
        {
            switch (directionID)
            {
                case 0:
                    return new Vector2(0, 1);
                case 1:
                    return new Vector2(1, 1);
                case 2:
                    return new Vector2(1, 0);
                case 3:
                    return new Vector2(1, -1);
                case 4:
                    return new Vector2(0, -1);
                case 5:
                    return new Vector2(-1, -1);
                case 6:
                    return new Vector2(-1, 0);
                case 7:
                    return new Vector2(-1, 1);
            }
        }
        catch
        {
            throw new Exception("the Direction ID provided does not match a direction regarding Grid queries.");
        }
        return Vector2.negativeInfinity;
    }

    public static byte DirectionToID(Vector2 v, float acceptedMaximumLinearMagnitude = 1f)
    {
        acceptedMaximumLinearMagnitude = Mathf.Abs(acceptedMaximumLinearMagnitude);
        float acceptedMaximumVectorMagnitude = new Vector2(acceptedMaximumLinearMagnitude, acceptedMaximumLinearMagnitude).magnitude;
        if(v.x == 0)
        {
            if(v.y > 0 && v.y <= acceptedMaximumLinearMagnitude)
            {
                return 0;
            }else if(v.y < 0 && v.y >= -acceptedMaximumLinearMagnitude)
            {
                return 4;
            }
        }
        else if (v.y == 0)
        {
            if (v.x > 0 && v.x <= acceptedMaximumLinearMagnitude)
            {
                return 2;
            }
            else if (v.x < 0 && v.x >= -acceptedMaximumLinearMagnitude)
            {
                return 6;
            }
        }
        else if(v.x == v.y && v.magnitude <= acceptedMaximumVectorMagnitude)
        {
            if(v.x < 0)
            {
                return 5;
            }
            else
            {
                return 1;
            }
        }else if(v.x == -v.y && v.magnitude <= acceptedMaximumVectorMagnitude)
        {
            if(v.x > 0)
            {
                return 3;
            }
            else
            {
                return 7;
            }
        }

        return 255;
    }


    #endregion

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
