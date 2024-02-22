using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    public List<GameObject> Towers;
}

[System.Serializable]
public class Tower
{
    public TowerControler Gem;
    public bool isactive;
}