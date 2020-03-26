using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGen : MonoBehaviour
{

    public int mapSize;
    public float[][] mapArray;

    public GameObject floor;
    GameObject[][] floorTiles;

    public float scale, xOffset, yOffset;
    float lastScale, xLastOffset, yLastOffset;  
    
    void Start()
    {
        //Used later to check for change
        lastScale = scale;
        xLastOffset = xOffset;
        yLastOffset = yOffset;
        mapArray = new float[mapSize][];
        floorTiles = new GameObject[mapSize][];

        for (int i = 0; i < mapSize; ++i)
        {
            mapArray[i] = new float[mapSize];
            floorTiles[i] = new GameObject[mapSize];
        }



        //I'm thinking of layering multiple noise maps on top of each other. Right now I got these two layers generated. 
        mapArray = generateNoiseMap(mapArray, scale);
        //resourceArray = generateNoiseMap(resourceArray, scale); //Unused for now.

        createTileMap();
        displayMap();
    }


    float[][] generateNoiseMap(float[][] array, float scale)
    {
        for (int i = 0; i < mapSize; ++i)
        {
            for (int j = 0; j < mapSize; ++j)

            {
                float x = (float)i;
                float y = (float)j;

                float iPerlin = (x / mapSize * scale)+xOffset;
                float jPerlin = (y / mapSize * scale)+yOffset;
                float noiseValue = Mathf.PerlinNoise(iPerlin, jPerlin);
                
                array[i][j] = noiseValue;
            }
        }
        return array;
    }
    void createTileMap()
    {
        for (int i = 0; i < mapSize; ++i)
        {
            for (int j = 0; j < mapSize; ++j)
            {
                float x = (float)i;
                float z = (float)j;

                //Offset values depends on the size of the gameobject tile
                float xOffset = (x * 10f);
                float zOffset = (z * 10f);

                Vector3 position = new Vector3(xOffset, 0, zOffset);
                //Creates multiple gameobjects. Set mapSize to small initial values for performance. Probably can do this a better way but this is for testing.
                floorTiles[i][j] = Instantiate(floor, position, Quaternion.AngleAxis(0, Vector3.left)) as GameObject;
                floorTiles[i][j].transform.parent = this.transform;
                floorTiles[i][j].name = "maptile" + i + j;
            }
        }
    }


    void displayMap()
    {
        for (int i = 0; i < mapSize; ++i)
        {
            for(int j =0; j <mapSize; ++j)
            {
                //Adds the color to each game tile. 
                float noiseValue = mapArray[i][j] * 0.5f;
                Color newColor = new Color(noiseValue, noiseValue, noiseValue);
                Material material = floorTiles[i][j].GetComponent<Renderer>().material;
                material.SetColor("_Color", newColor);
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (xLastOffset != xOffset || yLastOffset != yOffset || lastScale != scale)
        {
            mapArray = generateNoiseMap(mapArray, scale);
            displayMap();
            xLastOffset = xOffset;
            yLastOffset = yOffset;
            lastScale = scale;
        }
    }
}
