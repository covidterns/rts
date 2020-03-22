using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGen : MonoBehaviour
{

    public int mapSize;
    public float[][] mapArray, resourceArray;
    public GameObject floor;

    void Start()
    {
        //I'm thinking of layering multiple noise maps on top of each other. Right now I got these two layers generated. 

        mapArray = generateNoiseMap(mapArray, 10f);
        resourceArray = generateNoiseMap(resourceArray, 10f); //Unused for now.
        displayMap();
    }


    float[][] generateNoiseMap(float[][] array, float scale)
    {
        array = new float[mapSize][];

        for(int i =0; i < mapSize; ++i)
        {
            array[i] = new float[mapSize];
        }

        for (int i = 0; i < mapSize; ++i)
        {
            for (int j = 0; j < mapSize; ++j)

            {
                float x = (float)i;
                float y = (float)j;

                float iPerlin = x / mapSize * scale;
                float jPerlin = y / mapSize * scale;
                float noiseValue = Mathf.PerlinNoise(iPerlin, jPerlin);
                
                array[i][j] = noiseValue;
            }
        }
        return array;
    }


    void displayMap()
    {
        for (int i = 0; i < mapSize; ++i)
        {
            for(int j =0; j <mapSize; ++j)
            {
                float x = (float)i;
                float z = (float)j;
                
                //Offset values depends on the size of the gameobject tile
                float xOffset = (x * 10f);
                float zOffset = (z * 10f);

                Vector3 position = new Vector3(xOffset, 0, zOffset);
                //Creates multiple gameobjects. Set mapSize to small initial values for performance. Probably can do this a better way but this is for testing.
                GameObject mapTile = Instantiate(floor, position, Quaternion.AngleAxis(0, Vector3.left)) as GameObject;
                mapTile.transform.parent = this.transform;
                mapTile.name = "maptile" + i + j;

                //Adds the color to each game tile. 
                float noiseValue = mapArray[i][j] * 0.5f;
                Color newColor = new Color(noiseValue, noiseValue, noiseValue);
                Material material = mapTile.GetComponent<Renderer>().material;
                material.SetColor("_Color", newColor);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
