using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Player.Checkpoint = gameObject.transform.position;
        }
    }
}
