using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medscript : MonoBehaviour
{
    public int HealthGain;

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            other.GetComponent<Player>().Health += HealthGain;
            Destroy(gameObject);
        }
    }
}
