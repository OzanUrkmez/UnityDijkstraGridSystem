using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuestryGameGeneral.PathFinding;
using System;

public class Grid : MonoBehaviour
{

    [SerializeField]
    private GridRelations gridRelations;

    private static Grid startGrid;

    private static DijkstraMap<Grid> gridMap;

    #region Unity and Instantiation

    // Start is called before the first frame update
    void Start()
    {
        List<DijkstraMap<Grid>.NodeLink> nodeLinks = new List<DijkstraMap<Grid>.NodeLink>();
        foreach(Grid g in gridRelations.GetGrids())
        {
            if (g == null)
                continue;
            nodeLinks.Add(new DijkstraMap<Grid>.NodeLink() { distance = 1, endObject = g });
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

    }

    private void ExecuteVisualizationProcedure(Vector3 pos, Vector3 scale)
    {

    }

    #endregion

    #endregion
}

[Serializable]
public struct GridRelations
{
    public Grid upperGrid;
    public Grid upperRightGrid;
    public Grid rightGrid;
    public Grid bottomRightGrid;
    public Grid bottomGrid;
    public Grid bottomLeftGrid;
    public Grid leftGrid;
    public Grid upperLeftGrid;

    public Grid GetGrid(byte directionID)
    {
        switch (directionID)
        {
            case 0:
                return upperGrid;
            case 1:
                return upperRightGrid;
            case 2:
                return rightGrid;
            case 3:
                return bottomRightGrid;
            case 4:
                return bottomGrid;
            case 5:
                return bottomLeftGrid;
            case 6:
                return leftGrid;
            case 7:
                return upperLeftGrid;
        }

        throw new Exception("the Direction ID provided does not match a direction regarding Grid queries.");
    }

    public Grid[] GetGrids()
    {
        return new Grid[] { upperGrid, upperRightGrid, rightGrid, bottomRightGrid, bottomGrid, bottomLeftGrid, leftGrid, upperLeftGrid };
    }


}

