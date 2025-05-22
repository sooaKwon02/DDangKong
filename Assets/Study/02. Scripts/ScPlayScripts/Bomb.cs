using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float bombRadius = 10.0f;
    public float bombForce = 100.0f;
    public AudioClip boom;
    public AudioClip fuse;
    public float fuseTime = 1.5f;
    public GameObject explosion;

    LayBombs layBombs;
    PickupSpawner pickupSpawner;
    ParticleSystem explosionFX;

    private void Awake()
    {
        explosionFX = GameObject.FindGameObjectWithTag("ExplosionFX").GetComponent<ParticleSystem>();
        pickupSpawner = GameObject.Find("PickupSpawner").GetComponent<PickupSpawner>();

        if (GameObject.FindGameObjectWithTag("Player"))
        {
            layBombs = GameObject.FindGameObjectWithTag("Player").GetComponent<LayBombs>();
        }
    }

    private void Start()
    {
        if(transform.root == transform)
        {
            StartCoroutine(BombDetonation());
        }
    }

    IEnumerator BombDetonation()
    {
        AudioSource.PlayClipAtPoint(fuse, transform.position);

        yield return new WaitForSeconds(fuseTime);

        Explode();
    }

    public void Explode()
    {
        layBombs.bombLaid = false;
        pickupSpawner.StartCoroutine(pickupSpawner.DeliverPickup());
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, bombRadius, 1 << LayerMask.NameToLayer("Enemies"));

        foreach(Collider2D en in enemies)
        {
            Rigidbody2D rb = en.GetComponent<Rigidbody2D>();
            if(rb != null && rb.tag == "Enemy")
            {
                rb.gameObject.GetComponent<Enemy>().hp = 0;
                Vector3 deltaPos = rb.transform.position - transform.position;

                Vector3 force = deltaPos.normalized * bombForce;
                rb.AddForce(force);
            }
        }

        explosionFX.transform.position = transform.position;
        explosionFX.Play();

        Instantiate(explosion, transform.position, Quaternion.identity);
        AudioSource.PlayClipAtPoint(boom, transform.position);
        Destroy(gameObject);    
    }
}
