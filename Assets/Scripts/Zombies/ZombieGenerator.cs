using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieGenerator : MonoBehaviour
{
    public GameObject zombiePrefab;
    public int initialNumbers;
    StaticObstacle[] allObstacles;
    float areaWidth;
    float areaHeight;


    void Awake()
    {
        //wyciągnięcie wymiarów z skryptu do tworzenia ramki
        WallsCreator wallsCreator = FindObjectsOfType<WallsCreator>()[0].GetComponent<WallsCreator>();
        areaWidth = wallsCreator.width;
        areaHeight = wallsCreator.height;
        allObstacles = (StaticObstacle[])FindObjectsOfType(typeof(StaticObstacle));

        //spawnowanie danej liczby zombie
        int zombieCounter = 0;
        int unsuccesfulAtempts = 0;
        while (zombieCounter < initialNumbers)
        {   
            bool created = CreateZombie();
            if (created) 
            {
                zombieCounter++;
                unsuccesfulAtempts = 0;
            }
            else unsuccesfulAtempts++;

            if (unsuccesfulAtempts >= 10)
            {
                Debug.Log("Too many usuccesful tries to create a zombie!");
                break;
            }
        }
    }

    bool CreateZombie()
    {
        //Losowanie pozycji:
        Vector2 randomPosition = new Vector2(Random.Range(-areaWidth/2 + 1, areaWidth/2 -1), Random.Range(-areaHeight/2 + 1, areaHeight/2 -1));
            GameObject zombie = Instantiate(zombiePrefab, randomPosition, Quaternion.identity, transform);
            bool check = IsThisSpaceFree(zombie.GetComponent<CircleCollider>());
            int tries = 1;
            while (!check || tries < 10)
            {
                randomPosition = new Vector2(Random.Range(-areaWidth/2 + 1, areaWidth/2 -1), Random.Range(-areaHeight/2 + 1, areaHeight/2 -1));
                zombie.transform.position = randomPosition;
                check = IsThisSpaceFree(zombie.GetComponent<CircleCollider>());
                tries ++;
            }


            zombie.GetComponent<ZombieController>().placeholderDirection = new Vector2(Random.Range(-1, 1), Random.Range(-1, 1));
            zombie.GetComponent<EnemyBody>().previous_position = randomPosition;
            //nadaj losową szybkość
            return true;
        }
        

    private bool IsThisSpaceFree(CircleCollider collider)
    {
        foreach (MyPhysicsBody obstacle in allObstacles)
        {
            if(obstacle.c_collider.Overlaps(collider))
            {
                return false;
                //Debug.Log("Somebody " + body +" collided with " + obstacle);
            }          
        }
        
        return true;
    }
}
