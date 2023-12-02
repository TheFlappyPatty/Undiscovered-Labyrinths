using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets : MonoBehaviour
{
    public int bulletspeed;
    public int Damage;
    public float lifetime = 10;
    public Rounds Type;
    public void Awake()
    {
        StartCoroutine(Bulletlife());
    }
    public void Update()
    {
        transform.Translate(Vector3.forward * bulletspeed * Time.deltaTime);
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Enemy")
        {
            if (Type == Rounds.standard)
            {
                collision.gameObject.GetComponent<EnemyAi>().Health -= Damage;
            }
            if (Type == Rounds.Explosive)
            {

                foreach (GameObject G in GameObject.FindGameObjectsWithTag("Enemy"))
                {
                    if (Vector3.Distance(G.transform.position, transform.position) < 5)
                    {
                        G.GetComponent<EnemyAi>().Health -= Damage;
                    }
                }
                Destroy(gameObject);
            }
            Destroy(gameObject);
        }
    }
    public IEnumerator Bulletlife()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}
