using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombPickup : MonoBehaviour
{
    public AudioClip pickupClip;

    Animator anim;
    bool landed = false;

    private void Awake()
    {
        anim = transform.root.GetComponent<Animator>(); 
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            AudioSource.PlayClipAtPoint(pickupClip, transform.position);
            other.GetComponent<LayBombs>().bombCount++;
            Destroy(transform.root.gameObject);
        }
        else if(other.tag == "Ground" && !landed)
        {
            anim.SetTrigger("Land");
            transform.parent = null;
            gameObject.AddComponent<Rigidbody2D>();
            landed = true;
        }
    }
}
