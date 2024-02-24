using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerControler : MonoBehaviour
{

    public GameObject Bullet;
    public  float Health;
    public bool IsFireGem;
    public int Damage = 25;
    private GameObject player;
    [Header("Tower stats")]
    public GameObject Gem;
    public Color UnCharedColor;
    public Color ChargedColor;
    public float rechargeTime;
    public float ChargeUpTime;
    private bool Charged = true;
    private Material ThisGem;

    public void Start()
    {
        player = GameObject.Find("Player");
        ThisGem = Instantiate<Material>(Gem.transform.GetChild(1).GetComponent<Renderer>().material);
        Gem.transform.GetChild(0).GetComponent<Renderer>().material = ThisGem;
        Gem.transform.GetChild(1).GetComponent<Renderer>().material = ThisGem;
    }
    public IEnumerator Targeting()
    {
        if(Health > 0)
        {
            if (Charged == true)
            {
                Charged = false;
                if(IsFireGem == true)
                {
                  var fire = Instantiate(Bullet, Gem.transform.position, Quaternion.identity, null).GetComponent<EnemyBulletScript>();
                    fire.Player = player;
                    fire.FireBullet = true;
                    fire.damage = Damage;
                    fire.speed = 30;
                }
                else
                {
                    var fire = Instantiate(Bullet, Gem.transform.position, Quaternion.identity, null).GetComponent<EnemyBulletScript>();
                    fire.Player = player;
                    fire.damage = Damage;
                    fire.speed = 30;
                }

                yield return new WaitForSeconds(Random.Range(rechargeTime - 0.5f,rechargeTime + 0.5f));
                ThisGem.color = Color.black;
                StartCoroutine(ChargeUp());
            }
        }
        else
        {
            ThisGem.color = Color.blue;
        }
    }
    public IEnumerator ChargeUp()
    {
        var Charge = 0f;
        var Charging = true;
        if(Charge < 100)
        {
            while (Charging)
            {
                Charge++;
                ThisGem.color = Color.Lerp(UnCharedColor, ChargedColor, Charge/100);
                yield return new WaitForSeconds(ChargeUpTime);
                if (Charge >= 100)
                {
                    Charged = true;
                    Charging = false;
                    break;
                }
            }
        }
        StartCoroutine(Targeting());
       yield return null;
    }
}
