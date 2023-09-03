using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blacksmith : MonoBehaviour
{
    [SerializeField] private ParticleSystem _sparks;

    public void HitHammer()
    {
        _sparks.Play();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.root.tag == "Player")
        {
            MenuManager.Singleon.OpenShop();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.root.tag == "Player")
        {
            MenuManager.Singleon.CloseShop();
        }
    }
}
