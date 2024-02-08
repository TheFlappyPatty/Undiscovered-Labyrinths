using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletScript : MonoBehaviour
{
    public int damage;
    public int speed;
    public bool FireBullet;
    public GameObject Player;
    public void Start()
    {
        transform.LookAt(Player.transform);
        StartCoroutine(lifetime());
    }
    public void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if (FireBullet == true)
            {
                Player.GetComponent<Player>().Health -= damage;
                if (Player.GetComponent<FirePlayerEffect>() == null)
                {
                    Player.AddComponent<FirePlayerEffect>().Timer = 10;
                }
                else
                {
                    Player.GetComponent<FirePlayerEffect>().Timer += 5;
                }
            }
            else
            {
            Player.GetComponent<Player>().Health -= damage;
            Destroy(gameObject);
            }

        }
        Destroy(gameObject);
    }

    public IEnumerator lifetime()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
}
