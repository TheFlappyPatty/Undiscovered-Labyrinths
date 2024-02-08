using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossroomScript : MonoBehaviour
{
    public GameObject Boss;
    public GameObject Door;
    public bool Closed;
    public bool closing;
    public Vector3 openpos;
    public Vector3 Closepos;

    public void Awake()
    {
        openpos = Door.transform.localPosition;
    }
    public void Open()
    {
        Door.transform.localPosition = openpos;
        Closed = false;
    }
    public void Close()
    {
        Door.transform.localPosition = Closepos;
        Closed = true;
    }
    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Close();
            Boss.GetComponent<BossScript>().StartFight(other.gameObject);
        }
    }
}
