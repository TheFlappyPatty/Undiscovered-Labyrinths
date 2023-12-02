using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    public int Speed;
    public int Health;
    public int Damage;
    public NavMeshAgent Controlpoint;
    public GameObject player;
    public void Start()
    {
        Controlpoint = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player");
    }
    public void Update()
    {
        Controlpoint.destination = player.transform.position;
        if (Health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
