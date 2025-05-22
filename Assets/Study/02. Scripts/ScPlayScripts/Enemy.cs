using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 2.0f;
    public int hp = 2;
    public Sprite deadEnemy;
    public Sprite damagedEnemy;
    public AudioClip[] deathClips;
    public GameObject hunderedPointsUI;
    public float deathSpinMin = -100.0f;
    public float deathSpinMax = 100.0f;

    private SpriteRenderer ren;
    private Rigidbody2D rb;
    private Transform frontCheck;
    private bool dead = false;
    private Score score;

    private void Awake()
    {
        ren = transform.Find("body").GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();   
        frontCheck = transform.Find("frontCheck").transform;
        score = GameObject.Find("Score").GetComponent<Score>();
    }

    private void FixedUpdate()
    {
        Collider2D[] frontHits = Physics2D.OverlapPointAll(frontCheck.position, 1);

        foreach(Collider2D c in frontHits)
        {
            if(c.tag == "Obstacle")
            {
                Flip();
                break;
            }
        }

        rb.velocity = new Vector2(transform.localScale.x * moveSpeed, rb.velocity.y);
        if(hp == 1 && damagedEnemy != null)
        {
            ren.sprite = damagedEnemy;
        }

        if(hp <= 0 && !dead)
        {
            Death();
        }
    }

    public void Hurt()
    {
        hp--;
    }

    void Death()
    {
        SpriteRenderer[] otherRenderers = GetComponentsInChildren<SpriteRenderer>();

        foreach(SpriteRenderer s in otherRenderers)
        {
            s.enabled = false;
        }

        ren.enabled = true;
        ren.sprite = deadEnemy;

        score.score += 100;
        dead = true;

        rb.fixedAngle = false;
        rb.AddTorque(Random.Range(deathSpinMin, deathSpinMax));

        Collider2D[] cols = GetComponents<Collider2D>();
        foreach(Collider2D c in cols)
        {
            c.isTrigger = true;
        }

        int i = Random.Range(0, deathClips.Length);
        AudioSource.PlayClipAtPoint(deathClips[i], transform.position);

        Vector3 scorePos;
        scorePos = transform.position;
        scorePos.y += 1.5f;

        Instantiate(hunderedPointsUI, scorePos, Quaternion.identity);
    }

    public void Flip()
    {
        Vector3 enemyScale = transform.localScale;
        enemyScale.x *= -1;
        transform.localScale = enemyScale;
    }

}
