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
    public GameObject Encounter;
    private bool Cooldown = false;
    public GameObject audionode;
    //drops
    public GameObject WeaponDrop;
    public GameObject HealthDrop;
    public void Awake()
    {
        Controlpoint = GetComponent<NavMeshAgent>();
        Controlpoint.speed = Speed;
        player = GameObject.Find("Player");
    }
    public void Update()
    {
        if(Vector3.Distance(player.transform.position,transform.position) < 20 || Encounter != null)
        {
        Controlpoint.destination = player.transform.position;
        }
        if (Vector3.Distance(player.transform.position, transform.position) < 2)
        {
            StartCoroutine(attack());
        }
        if (Health <= 0)
        {
            var random = Random.Range(0,100);
            if(random > 90)
            {
                var randombuff = Random.Range(0, 100);
                if (randombuff > 60)
                {
                    Instantiate(WeaponDrop, transform.position, Quaternion.identity, null);
                }
                else
                {
                    Instantiate(HealthDrop, transform.position, Quaternion.identity, null);
                }
            }
            Instantiate(audionode);
            if(Encounter == null)
            {
                Destroy(gameObject);
            } else
            {
                if(Encounter.GetComponent<EncounterToggle>() != null) Encounter.GetComponent<EncounterToggle>().EnemiesAlive--;
                if(Encounter.GetComponent<BossScript>() != null) Encounter.GetComponent<BossScript>().enemyamountleft--;
                Destroy(gameObject);
            }
        }
    }
    public IEnumerator attack()
    {
        if (Cooldown == false)
        {
            Cooldown = true;
            player.GetComponent<Player>().Health -= Damage;
            yield return new WaitForSeconds(0.5f);
            Cooldown = false;
        }
    }
}
