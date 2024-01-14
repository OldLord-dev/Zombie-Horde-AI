using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    public GameObject deathScreen;
    void Awake()
    {
    }

    public void Death()
    {
        deathScreen.SetActive(true);
    }
    public void Restart()
    {

        SceneManager.LoadScene("MainScene");
    }
}
