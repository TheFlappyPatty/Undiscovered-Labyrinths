using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BossScript : MonoBehaviour
{
    [Header("Boss Health")]
    public GameObject Healthbar;
    public Slider HealthBarBoss;
    public int Stage2Start = 5000;
    public float BossHealth = 10000;

    [Header("Boss Attacks")]
    public GameObject EnemyBullet;
    public GameObject enemymissle;

    [Space]
    [Header("Enemies system")]
    public GameObject[] Enemies;
    public GameObject[] Spawners;


    private GameObject playerc;
    private bool fight;

    [Space]
    [Header("Fight Controls")]
    public int stage;
    public int Spawnenemyamount;
    public int enemyamountleft;
    public GameObject currentpillar;

    [Space]
    [Header("Boss Systems")]
    public static float bossHealth;
    public GameObject[] Pillars;
    public bool BossMoves = true;
    public List<DestructionPillars> Funky;

    // Start is called before the first frame update
    void Start()
    {
        bossHealth = BossHealth;
        HealthBarBoss.maxValue = bossHealth;
    }
    // Update is called once per frame
    void Update()
    {
        HealthBarBoss.value = bossHealth;



        if (bossHealth <= 0)
        {
            SceneManager.LoadScene("MainMenu");
        }
        if(fight == true)
        {
            if (Funky.Count < 0)
            {
                foreach (DestructionPillars f in Funky)
                {
                    if (f.HealthToDestroy >= BossHealth)
                    {
                        Instantiate(f.DestroyedPillar, f.PillarToDestroy.transform.position, Quaternion.identity, gameObject.transform);
                        Destroy(f.PillarToDestroy);
                        Funky.Remove(f);
                    }
                }
            }

            if (stage == 1)
            {
                StartCoroutine(Stage1());

            }
            if (stage == 2)
            {
                StartCoroutine(Stage2());
            }
        }
    }











    private bool shooting = true;
    private bool stalltime = true;
    public IEnumerator Stage1()
    {
        //The bosses Shooting attack
        if (shooting == true)
        {
            if (stalltime == true)
            {
                stalltime = false;
                var bullet = Instantiate(EnemyBullet, gameObject.transform.position, Quaternion.identity, null);
                bullet.GetComponent<EnemyBulletScript>().Player = playerc;
                yield return new WaitForSeconds(2);
                stalltime = true;
            }
        }

        //Enemies the boss spawns
        if (Spawnenemyamount >= enemyamountleft)
        {
            while (Spawnenemyamount >= enemyamountleft)
            {
                var enemie = Instantiate(Enemies[Random.Range(0, Enemies.Length)], Spawners[Random.Range(0, Spawners.Length)].transform.position, Quaternion.identity, null);
                enemie.GetComponent<EnemyAi>().Encounter = gameObject;
                enemyamountleft++;
                yield return new WaitForSeconds(0.05f);
            }
        }
        //When to go to the next stage
        if (bossHealth <= Stage2Start)
        {
            stage = 2;
        }

    }
    private bool wait = true;
    private bool hold = true;
    public IEnumerator Stage2()
    {
        //Shooting
        if (shooting == true)
        {
            if (stalltime == true)
            {
                stalltime = false;
                var bullet = Instantiate(EnemyBullet, gameObject.transform.position, Quaternion.identity, null);
                bullet.GetComponent<EnemyBulletScript>().Player = playerc;
                yield return new WaitForSeconds(1);
                stalltime = true;
            }
        }
        //spawning
        if (Spawnenemyamount >= enemyamountleft)
        {
            while (Spawnenemyamount >= enemyamountleft)
            {
                var enemie = Instantiate(Enemies[Random.Range(0, Enemies.Length)], Spawners[Random.Range(0, Spawners.Length)].transform.position, Quaternion.identity, null);
                enemie.GetComponent<EnemyAi>().Encounter = gameObject;
                enemyamountleft++;
            }
        }
        //When the boss moves
        if (wait == true)
        {
            if(BossMoves == true)
            {
                 move(); 
            }

            wait = false;
            yield return new WaitForSeconds(20);
            wait = true;
        }


        //For when the boss is dead
        if (bossHealth >= 0)
        {

            if (hold == true)
            {
                hold = false;
                var pos = new Vector3(playerc.transform.position.x, playerc.transform.position.y + 10, playerc.transform.position.z);
                Instantiate(enemymissle, pos, Quaternion.identity, null);
                yield return new WaitForSeconds(1);
                hold = true;
            }

        }
    }
    //Handles everything related to the boss moving
    public void move()
    {
        currentpillar.GetComponentInChildren<PillarScript>().active = false;
        currentpillar.GetComponentInChildren<PillarScript>().boss = null;
        currentpillar = Pillars[Random.Range(0, Pillars.Length)];
        gameObject.transform.position = currentpillar.transform.position;
        currentpillar.GetComponentInChildren<PillarScript>().active = true;
        currentpillar.GetComponentInChildren<PillarScript>().boss = gameObject;
    }
    //when the player Starts the fight
    public void StartFight(GameObject player)
    {
        playerc = player;
        stage = 1;
        fight = true;
        Healthbar.SetActive(true);
    }
    [System.Serializable]
    public class DestructionPillars
    {
        public GameObject PillarToDestroy;
        public GameObject DestroyedPillar;
        public int HealthToDestroy;
    }
}