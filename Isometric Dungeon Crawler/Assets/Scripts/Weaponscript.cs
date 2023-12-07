using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weaponscript : MonoBehaviour
{
    public Gun WeaponGrab;
    private GameObject player;
    public void Awake()
    {
        var RandomInput = Random.Range(1, 4);
        if (RandomInput == 1)
        {
            WeaponGrab = Gun.LazerBeam;
        }
        if(RandomInput == 2)
        {
            WeaponGrab = Gun.Grenade_Launcher;
        }
        if (RandomInput == 3)
        {
            WeaponGrab = Gun.MiniGun;
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Player")
        {
            player = other.gameObject;
            GiveGun();
            Destroy(gameObject);
        }
    }
    public void GiveGun()
    {
        if(WeaponGrab == Gun.LazerBeam)
        {
            player.GetComponent<Player>().Pickedupweapon(Rounds.lazar,0,4,0,10,Gun.LazerBeam);
            player.GetComponent<Player>().Ammo = 300;
        }
        if(WeaponGrab == Gun.MiniGun)
        {
            player.GetComponent<Player>().Pickedupweapon(Rounds.standard, 2400, 10, 30, 15,Gun.MiniGun);
            player.GetComponent<Player>().Ammo = 400;
        }
        if (WeaponGrab == Gun.Grenade_Launcher)
        {
            player.GetComponent<Player>().Pickedupweapon(Rounds.Explosive, 40, 50, 8, 30,Gun.Grenade_Launcher);
            player.GetComponent<Player>().Ammo = 6;
        }
    }

}
public enum Gun {
    None,
    LazerBeam,
    MiniGun,
    Grenade_Launcher
}
