using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    public List<TowerControler> Towers;
    public GameObject TowerPile;

    public int Spawnenemyamount;
    public int enemyamountleft;
    public GameObject[] Enemies;
    public GameObject[] Spawners;
    public void Awake()
    {
        TowerPile = GameObject.Find("TowerPile");
        FindChildren();
    }
    public void Update()
    {
        foreach (TowerControler f in Towers)
        {
            if (f.Health <= 0)
            {
                Towers.Remove(f);
            }
        }
        if (Towers.Count == 0)
        {
            Debug.Log("Player Wins");
        }
        //if (Spawnenemyamount >= enemyamountleft)
        //{
        //    while (Spawnenemyamount >= enemyamountleft)
        //    {
        //        var enemie = Instantiate(Enemies[Random.Range(0, Enemies.Length)], Spawners[Random.Range(0, Spawners.Length)].transform.position, Quaternion.identity, null);
        //        enemie.GetComponent<EnemyAi>().Encounter = gameObject;
        //        enemyamountleft++;
        //    }
        //}
    }
    public void ActivateTowers()
    {
        foreach (TowerControler f in Towers)
        {
            StartCoroutine(f.Targeting());
        }
    }
    public void FindChildren()
    {
        var Count = TowerPile.transform.childCount -1;
        while(Count > -1)
        {
            Towers.Add(TowerPile.transform.GetChild(Count).GetComponent<TowerControler>());
            Count--;
        }
    }
}

