using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallsCreator : MonoBehaviour
{
    public float width;
    public float height;
    public float wallWidth;

    public GameObject wallPrefab;
    void Start()
    {
        //top Wall
        CreateWall(new Vector3(width+wallWidth, wallWidth, 1.0f), new Vector3(0.0f, height/2, 0.0f));

        //below Wall
        CreateWall(new Vector3(width+wallWidth, wallWidth, 1.0f), new Vector3(0.0f, -height/2, 0.0f));

        //left Wall
        CreateWall(new Vector3(wallWidth, height, 1.0f), new Vector3(-width/2, 0.0f, 0.0f));

        //right Wall
        CreateWall(new Vector3(wallWidth, height, 1.0f), new Vector3(width/2, 0.0f, 0.0f));
    }

    void CreateWall(Vector3 newScale, Vector3 position)
    {
        GameObject wall = Instantiate(wallPrefab, position, Quaternion.identity, transform);
        wall.transform.localScale = newScale;
        wall.transform.GetComponent<RectangleCollider>().Start(); 
    }
}
