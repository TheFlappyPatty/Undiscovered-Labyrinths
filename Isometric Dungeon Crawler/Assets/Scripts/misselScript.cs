using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class misselScript : MonoBehaviour
{
    public void Update()
    {
        transform.Translate(new Vector3(0, -10, 0) * Time.deltaTime);
    }


    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().Health -= 30;
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }
}
