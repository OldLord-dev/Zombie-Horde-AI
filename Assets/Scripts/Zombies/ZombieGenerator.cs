using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieGenerator : MonoBehaviour
{
    public GameObject zombiePrefab;
    public int initialNumbers;

    float areaWidth;
    float areaHeight;


    void Awake()
    {
        //wyciągnięcie wymiarów z skryptu do tworzenia ramki
        WallsCreator wallsCreator = FindObjectsOfType<WallsCreator>()[0].GetComponent<WallsCreator>();
        areaWidth = wallsCreator.width;
        areaHeight = wallsCreator.height;

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
        if (IsThisSpaceFree(randomPosition))
        {
            GameObject zombie = Instantiate(zombiePrefab, randomPosition, Quaternion.identity, transform);
            zombie.GetComponent<ZombieController>().placeholderDirection = new Vector2(Random.Range(-1, 1), Random.Range(-1, 1));
            zombie.GetComponent<EnemyBody>().previous_position = randomPosition;
            //nadaj losową szybkość
            return true;
        }
        
        else return false;
    }

    private bool IsThisSpaceFree(Vector2 position)
    {
        return true;
    }
}
