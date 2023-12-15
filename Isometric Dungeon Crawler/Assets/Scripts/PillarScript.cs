using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarScript : MonoBehaviour
{
    public bool active;
    public GameObject boss;
    public Material material;

    public void Start()
    {

    }
    public void Update()
    {
        if (active)
        {
            material.color = Color.red;
        } else {
            material.color = Color.white;
        }
    }
}
