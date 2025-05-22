using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject explosion;

    void Start()
    {
        Destroy(gameObject, 2);    
    }

    void OnExplode()
    {
        Quaternion randomRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));

        Instantiate(explosion, transform.position, randomRotation);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Enemy")
        {
            col.gameObject.GetComponent<Enemy>().Hurt();
            OnExplode();
            Destroy(gameObject);
        }
        else if(col.tag == "BombPickup")
        {
            col.gameObject.GetComponent<Bomb>().Explode();
            Destroy(col.transform.root.gameObject);
            Destroy(gameObject);
        }
        else if (col.gameObject.tag != "Player")
        {
            OnExplode();
            Destroy(gameObject);
        }
    }
}
