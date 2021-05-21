using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private int width = 5;
    [SerializeField] private int height = 5;

    [SerializeField] private int borderSize = 0;

    private Tile[,] tiles;

    private GameObject tilePrefab = null;
    private GameObject crossPrefab = null;

    private const string resourcesTileName = "Tile";
    private const string resourcesCrossName = "Cross";

    #region Initialization

    private void Start()
    {
        //Loading our resources and giving our empty gameobjects
        tilePrefab = Tile();
        crossPrefab = Cross();

        tiles = new Tile[width, height];


        SetupTiles();
        SetupCamera();

        //When any tile clicked, this listening will work
        EventManager.StartListening(EventManager.tileClickedEvent, TileClicked);
    }

    #endregion

    /// <summary>
    /// Creating our tiles due respect to our width and height
    /// </summary>
    private void SetupTiles()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                GameObject tile = Instantiate(tilePrefab, new Vector3(i, j, 0), Quaternion.identity);

                tile.name = "Tile (" + i + "," + j + ")";

                tiles[i, j] = tile.GetComponent<Tile>();

                //Initialization of our tiles due respect to their column and row numbers
                tiles[i, j].Init(i, j, this);

                //Setting this gameobject to parents of tiles
                tile.transform.parent = transform;
            }
        }

        tilePrefab = null;
    }

    private void SetupCamera()
    {
        //Set camera to center of tile's group
        Camera.main.transform.position = new Vector3((float)(width - 1) / 2f,(float) (height - 1) / 2f, -10f);

        //We are calculating aspect ratio due to our screen's resolution
        float aspectRatio = (float) Screen.width / (float)Screen.height;


        ///Calculating vertical and horizontal size and comparing for  which one is bigger and then simply fit orthopgraphicSize to bigger one

        float verticalSize = (float)height / 2f + (float)borderSize;

        float horizontalSize = ((float)width / 2f + (float)borderSize) / aspectRatio;

        Camera.main.orthographicSize = (verticalSize > horizontalSize) ? verticalSize : horizontalSize;
    }

    private void TileClicked(GameObject go)
    {
        //Gameobject is which tile clicked in our touch system
        Tile clickedTile = go.GetComponent<Tile>();

        GameObject cross = Instantiate(crossPrefab, new Vector2(clickedTile.xIndex,clickedTile.yIndex), Quaternion.identity);

        cross.name = "Cross (" + clickedTile.xIndex + "," + clickedTile.yIndex + ")";

        cross.transform.parent = go.transform;

        Cross createdCross = cross.GetComponent<Cross>();

        createdCross.SetCoord(clickedTile.xIndex, clickedTile.yIndex);

        FindMatches(createdCross);
    }

    private void FindMatches(Cross selectedCross,int minLength = 3)
    {
        Cross[] allCrosses = FindObjectsOfType<Cross>();

        List<Cross> matches = new List<Cross>();


        //FIRST MATCH
        matches.Add(selectedCross);
        
        //LOOKING SELECTED CROSS'S NEIGHBOUR
        foreach (Cross cross in allCrosses)
        {
            if (cross != selectedCross)
            {
                //CHECK VERTICAL
                if (cross.xIndex == selectedCross.xIndex)
                {
                    if (cross.yIndex == selectedCross.yIndex + 1)
                    {
                        if (!matches.Contains(cross))
                            matches.Add(cross);
                    }
                    else if (cross.yIndex == selectedCross.yIndex - 1)
                    {
                        if (!matches.Contains(cross))
                            matches.Add(cross);
                    }
                }
                //CHECK HORIZONTAL
                else if (cross.yIndex == selectedCross.yIndex)
                {
                    if (cross.xIndex == selectedCross.xIndex + 1)
                    {
                        if (!matches.Contains(cross))
                            matches.Add(cross);
                    }
                    else if (cross.xIndex == selectedCross.xIndex - 1)
                    {
                        if (!matches.Contains(cross))
                            matches.Add(cross);
                    }
                }
            }
        }

        //LOOKING NEIGHBOUR OF MATCHES
        foreach (Cross match in matches.ToArray())
        {
            foreach (Cross cross in allCrosses)
            {
                if (cross != match)
                {
                    //CHECK VERTICAL
                    if (cross.xIndex == match.xIndex)
                    {
                        if (cross.yIndex == match.yIndex + 1)
                        {
                            if (!matches.Contains(cross))
                                matches.Add(cross);
                        }
                        else if (cross.yIndex == match.yIndex - 1)
                        {
                            if (!matches.Contains(cross))
                                matches.Add(cross);
                        }
                    }
                    //CHECK HORIZONTAL
                    else if (cross.yIndex == match.yIndex)
                    {
                        if (cross.xIndex == match.xIndex + 1)
                        {
                            if (!matches.Contains(cross))
                                matches.Add(cross);
                        }
                        else if (cross.xIndex == match.xIndex - 1)
                        {
                            if (!matches.Contains(cross))
                                matches.Add(cross);
                        }
                    }
                }
            }
        }


        if (matches.Count >= minLength)
        {
            EventManager.TriggerEvent(EventManager.successSoundEvent);

            foreach (Cross item in matches)
            {
                //MAKE PARENT CLICKABLE AGAIN
                item.GetComponentInParent<Tile>().clicked = false;
                //DESTROY SELECTED CROSSES
                Destroy(item.gameObject);
            }
        }
        else
        {
            EventManager.TriggerEvent(EventManager.clickSoundEvent);
        }
    }

    #region Resources Loader
    private GameObject Tile()
    {
        return Resources.Load(resourcesTileName) as GameObject;
    }

    private GameObject Cross()
    {
        return Resources.Load(resourcesCrossName) as GameObject;
    }

    #endregion
}
