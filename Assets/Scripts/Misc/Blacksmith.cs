using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blacksmith : MonoBehaviour
{
    [SerializeField] private ParticleSystem _sparks;
    [SerializeField] private AudioSource _blink;

    public void HitHammer()
    {
        _sparks.Play();
        _blink.Play();
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
