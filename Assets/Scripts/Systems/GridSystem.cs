using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridSystem : MonoBehaviour
{
    [Range(1,25)][Header("Amound of Tiles")]
    [SerializeField] private int tileSize = 3;
    [Header("Amount of Space Between Tiles ")]
    [Range(1,1.5f)][SerializeField] private float spacing = 1;

    private const string resourcesTileName = "Tile";


    private float aspectRatio = 1;

    private GameObject resourcesTile = null;
    private Camera cam = null;

    private Dictionary<GameObject, bool> tileDictionary = null;

    private int num = 1;
    private int clickedNum = -1;
    private void Awake()
    {
        //LOADING TILE'S RESOURCE
        resourcesTile = Tile();
        cam = FindObjectOfType<Camera>();
    }

    private void Start()
    {
        aspectRatio = (float)Screen.height / (float)Screen.width;

        CreateGrid();

        EventManager.StartListening(EventManager.tileClickedEvent, TileCilcked);
    }

    #region Initialization Grid System

    /// <summary>
    /// CREATING TILES IN THIS FUNCTION
    /// </summary>
    private void CreateGrid()
    {
        tileDictionary = new Dictionary<GameObject, bool>();

        for (int x = 0; x < tileSize; x++)
        {
            for (int y = 0; y < tileSize; y++)
            {
                GameObject tile = Instantiate(resourcesTile, transform);

                float posX = x * spacing * aspectRatio;
                float posY = y * -spacing * aspectRatio;

                //SETTING TILE'S POSITIONS 
                tile.transform.position = new Vector2(posX, posY);

                //SETTING SCALE OF TILES DUE RESPECT SCREEN'S ASPECT RATIO
                tile.transform.localScale = Vector3.one * aspectRatio;

                tile.name = num.ToString();


                //ADDING TILES TO OUR DICTIONARY
                tileDictionary.Add(tile, false);

                num++;
            }
        }

        resourcesTile = null;

        SetParentPosition();
        SetCameraZoom();
    }

    /// <summary>
    /// SET PARENT TILES POSITION UP TO SPACING RATE AND SCREEN'S ASPECT RATIO
    /// </summary>
    private void SetParentPosition()
    {
        float grid = tileSize * spacing * aspectRatio;

        transform.position = new Vector2(-grid / 2 + aspectRatio * spacing / 2, grid / 2 - aspectRatio * spacing / 2);
    }

    /// <summary>
    /// CALCULATING CAMERA'S ZOOM'S RATIO
    /// </summary>
    private void SetCameraZoom()
    {
        cam.orthographicSize = tileSize * spacing * Screen.height / Screen.width * 0.5f;
    }

    #endregion


    private void TileCilcked(GameObject go)
    {  
    }

    #region Resources Loader

    private GameObject Tile()
    {
        return Resources.Load(resourcesTileName) as GameObject;
    }

    #endregion
}
