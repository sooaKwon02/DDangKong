using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifePickup : MonoBehaviour
{
    public float lifeBonus;
    public AudioClip collect;

    PickupSpawner pickupSpawner;
    Animator anim;
    bool landed = false;

    private void Awake()
    {
        pickupSpawner = GameObject.Find("PickupSpawner").GetComponent<PickupSpawner>();
        anim = transform.root.GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            PlayerLife playerLife = other.GetComponent<PlayerLife>();

            playerLife.life += lifeBonus;
            playerLife.life = Mathf.Clamp(playerLife.life, 0f, 100f);
            
            playerLife.UpdateLifeBar();

            pickupSpawner.StartCoroutine(pickupSpawner.DeliverPickup());
            AudioSource.PlayClipAtPoint(collect, transform.position);
            Destroy(transform.root.gameObject);
        }
        else if (other.tag == "Ground" && !landed)
        {
            anim.SetTrigger("Land");
            transform.parent = null;
            gameObject.AddComponent<Rigidbody2D>();
            landed = true;
        }
    }
}
