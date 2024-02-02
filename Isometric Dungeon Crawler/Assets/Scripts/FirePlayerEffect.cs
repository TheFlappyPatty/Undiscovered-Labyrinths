using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePlayerEffect : MonoBehaviour
{
    public int Timer;
    private Player player;
    private bool Damage = false;

    public void Awake()
    {
        player = gameObject.GetComponent<Player>();

    }
    public void Update()
    {
        Timer = Mathf.Clamp(Timer,0,20);
        StartCoroutine(Burn());
    }
    public IEnumerator Burn()
    {
        player.FireEffect.SetActive(true);
        if (Damage == false)
        {
            Damage = true;
            player.Health -= 1;
            yield return new WaitForSeconds(1);
            Timer -= 1;
            Damage = false;
        }
        if (Timer == 0)
        {
            player.FireEffect.SetActive(false);
            Destroy(gameObject.GetComponent<FirePlayerEffect>());
        }

    }
}
