using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterToggle : MonoBehaviour
{
    public GameObject EntranceWall;
    public GameObject Exitwall;
    public int[] Wave_count;
    public int CurrentWave;
    public int EnemiesAlive;
    public GameObject[] SpawnPoints;
    public GameObject[] enemy;
    public bool Active;
    public Transform EncounterCam;
    private GameObject player;
    public void Update()
    {

    }

    public IEnumerator Wave()
    {
        while (CurrentWave <= Wave_count.Length)
        {
            while (Wave_count[CurrentWave] >= EnemiesAlive)
            {
                Instantiate(enemy[Random.Range(0,enemy.Length)], SpawnPoints[Random.Range(0, SpawnPoints.Length)].gameObject.transform.position, Quaternion.identity, null).GetComponent<EnemyAi>().Encounter = gameObject;
                EnemiesAlive++;
                yield return new WaitForSeconds(0.3f);
            }
            yield return new WaitUntil(Enemieskilled);
            CurrentWave++;
            if(CurrentWave >= Wave_count.Length)
            {
                Active = false;
                EncounterOver();
            }
        }

    }
    public bool Enemieskilled()
    {
        if(EnemiesAlive == 0)
        {
            return true;
        }
        return false;
    }
    public void EncounterOver()
    {
        player.GetComponent<Player>().InEncounter = false;
        Active = false;
        Exitwall.SetActive(false);
    }
    public void OnTriggerEnter(Collider other)
    {
        gameObject.GetComponent<BoxCollider>().enabled = false;
        if(other.gameObject.tag == "Player")
        {
            EntranceWall.SetActive(true);
            StartCoroutine(Wave());
            player = other.gameObject;
         GameObject.Find("Playercam").transform.position = EncounterCam.position;
         GameObject.Find("Playercam").transform.eulerAngles = EncounterCam.eulerAngles;
         other.gameObject.GetComponent<Player>().InEncounter = true;
         Active = true;
        }
    }
}
