using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStartTrigger : MonoBehaviour
{
    public GameObject Towermanager;
    public GameObject ECamera;
    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Towermanager.GetComponent<TowerManager>().enabled = true;
            Towermanager.GetComponent<TowerManager>().ActivateTowers();
            other.GetComponent<Player>().InEncounter = true;
            other.GetComponent<Player>().playerCam.transform.position = ECamera.transform. position;
            other.GetComponent<Player>().playerCam.transform.rotation = ECamera.transform.rotation;
            Destroy(gameObject);
        }
    }
}
