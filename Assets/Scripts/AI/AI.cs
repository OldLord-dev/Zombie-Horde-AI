using System.Data;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    [SerializeField] private Transform player;
    AIState currentState;
    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        currentState = new Wander(this.gameObject, player);
    }
    void Update()
    {
        currentState = currentState.Process();
    }
}
