using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

// this script does things
public class Weaponscript : MonoBehaviour
{
    public Gun WeaponGrab;
    public Light weaponlight;
    private GameObject player;
    public void Awake()
    {
        var RandomInput = Random.Range(1, 5);
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
        if(RandomInput == 4)
        {
            WeaponGrab = Gun.Shotgun;
        }
        StartCoroutine(Timer());
    }
    private bool targetflash = true;
    private int timervalue = 40;
    public IEnumerator Timer()
    {
        StartCoroutine(Flash());
        while (timervalue > 0)
        {
            timervalue--;
            yield return new WaitForSeconds(1);
            if(timervalue == 0)
            {
                Destroy(gameObject);
            }
        }
    }
    public IEnumerator Flash()
    {
        yield return new WaitUntil(Timerreached);
        while (timervalue < 10)
        {
        if (targetflash == true)
        {
            targetflash = false;
            weaponlight.enabled = true;
            yield return new WaitForSeconds(Mathf.Lerp(0.1f, 1, timervalue / 2));
            weaponlight.enabled = false;
            yield return new WaitForSeconds(Mathf.Lerp(0.1f, 1, timervalue / 2));
            targetflash = true;
        }
        }

    }
    public bool Timerreached()
    {
        if (timervalue < 10)
        {
            return true;
        }
        else
        {
            return false;
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
            player.GetComponent<Player>().Pickedupweapon(Rounds.lazar,0,10,0,10,Gun.LazerBeam);
            player.GetComponent<Player>().Ammo = 300;
        }
        if(WeaponGrab == Gun.MiniGun)
        {
            player.GetComponent<Player>().Pickedupweapon(Rounds.standard, 2400, 10, 30, 10,Gun.MiniGun);
            player.GetComponent<Player>().Ammo = 400;
        }
        if (WeaponGrab == Gun.Grenade_Launcher)
        {
            player.GetComponent<Player>().Pickedupweapon(Rounds.Explosive, 60, 50, 12, 15,Gun.Grenade_Launcher);
            player.GetComponent<Player>().Ammo = 8;
        }
        if(WeaponGrab == Gun.Shotgun)
        {
            player.GetComponent<Player>().Pickedupweapon(Rounds.standard, 60, 20, 25, 2, Gun.Shotgun);
            player.GetComponent<Player>().Ammo = 100;
        }
    }

}
public enum Gun {
    None,
    LazerBeam,
    MiniGun,
    Grenade_Launcher,
    Shotgun
}

