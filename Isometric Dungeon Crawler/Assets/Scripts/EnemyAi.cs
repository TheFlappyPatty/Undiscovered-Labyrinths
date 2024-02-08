using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    //Enemy Stats
    public int Speed;
    public int Health;
    public int Damage;
    public bool FireUnit = false;
    public bool RangedUnit = false;
    public int DetectionRange = 20;

    //Enemy FunctionControlls
    private NavMeshAgent Controlpoint;
    private GameObject player;
    public GameObject Encounter;
    public GameObject Bullet;
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
        //Detection System
        if(Vector3.Distance(player.transform.position,transform.position) < DetectionRange || Encounter != null)
        {
        Controlpoint.destination = player.transform.position;
        }

        //Attack And Attack type
        if (RangedUnit == true)
        {
            if (Vector3.Distance(player.transform.position, transform.position) < 10)
            {
                StartCoroutine(Shoot());
            }
        }
        else if (Vector3.Distance(player.transform.position, transform.position) < 2)
        {
            StartCoroutine(attack());
        }




        //Loot Drop System and death
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
            if(FireUnit == true)
            {
                player.GetComponent<Player>().Health -= Damage;
                if(player.GetComponent<FirePlayerEffect>() == null)
                {
                    player.AddComponent<FirePlayerEffect>().Timer = 10;
                }
                else
                {
                    player.GetComponent<FirePlayerEffect>().Timer += 5;
                }
            }
            else
            {
                player.GetComponent<Player>().Health -= Damage;
            }
            yield return new WaitForSeconds(2f);
            Cooldown = false;
        }
    }
    public IEnumerator Shoot()
    {

        if (Cooldown == false)
        {
            Cooldown = true;
            if (FireUnit == true)
            {
                var bullet = Instantiate(Bullet, transform.position, Quaternion.identity, null);
                bullet.GetComponent<EnemyBulletScript>().FireBullet = true;
                bullet.GetComponent<EnemyBulletScript>().Player = player;
                bullet.GetComponent<EnemyBulletScript>().damage = Damage;
                bullet.GetComponent<EnemyBulletScript>().speed = 10;
            }
            else
            {
                var bullet = Instantiate(Bullet, transform.position, Quaternion.identity, null);
                bullet.GetComponent<EnemyBulletScript>().Player = player;
                bullet.GetComponent<EnemyBulletScript>().damage = Damage;
                bullet.GetComponent<EnemyBulletScript>().speed = 20;
            }
            yield return new WaitForSeconds(2f);
            Cooldown = false;
        }


    }
}
