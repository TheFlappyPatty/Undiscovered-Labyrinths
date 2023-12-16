using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarScript : MonoBehaviour
{
    public bool active;
    public GameObject boss;
    public Material material;
    public GameObject[] pillarchildren;

    public void Update()
    {
        if (active)
        {
            material.color = Color.cyan;
            gameObject.tag = "ActivePillar";
            foreach (GameObject n in pillarchildren)
            {
                n.tag = "ActivePillar";
            }
        }
        else
        {
            material.color = Color.white;
            gameObject.tag = "Pillar";
            foreach (GameObject n in pillarchildren)
            {
                n.tag = "Pillar";
            }
        }
    }
}
