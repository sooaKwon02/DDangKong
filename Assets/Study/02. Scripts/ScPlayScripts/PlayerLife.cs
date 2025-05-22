using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    private Animator anim;
    private PlayerCtrl playerCtrl;
    private SpriteRenderer lifeBar;

    public AudioClip[] ouchClips;

    public float life = 100.0f;
    public float damageAmount = 10.0f;
    public float hurtForce = 10.0f;
    public float repeatDamagePeriod = 2.0f;

    private Vector3 lifeScale;
    private float lastHitTime;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerCtrl = GetComponent<PlayerCtrl>();
        lifeBar = GameObject.Find("LifeBar").GetComponent<SpriteRenderer>();

        lifeScale = lifeBar.transform.localScale;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Enemy")
        {
            if(Time.time > lastHitTime + repeatDamagePeriod)
            {
                if ((life > 0f))
                {
                    TakeDamage(col.transform);
                    lastHitTime = Time.time;
                }
                else
                {
                    SpriteRenderer[] spr = GetComponentsInChildren<SpriteRenderer>();
                    foreach (SpriteRenderer s in spr)
                    {
                        s.sortingLayerName = "UI";
                    }
                    Collider2D[] cols = GetComponents<Collider2D>();
                    foreach (Collider2D c in cols)
                    {
                        c.isTrigger = true;
                    }

                    GetComponent<PlayerCtrl>().enabled = false;
                    GetComponentInChildren<Bazooka>().enabled = false;
                    anim.SetTrigger("Die");
                }
            }
        }
    }

    void TakeDamage(Transform enemy)
    {
        playerCtrl.jump = false;
        Vector3 hurtVector = transform.position - enemy.position + Vector3.up * 5f;
        GetComponent<Rigidbody2D>().AddForce(hurtVector * hurtForce);
        life -= damageAmount;
        UpdateLifeBar();
        int i = Random.Range(0, ouchClips.Length);
        AudioSource.PlayClipAtPoint(ouchClips[i], transform.position);
    }

    public void UpdateLifeBar()
    {
        lifeBar.material.color = Color.Lerp(Color.green, Color.red, 1 - life * 0.01f);
        lifeBar.transform.localScale = new Vector3(lifeScale.x * life * 0.01f, 3, 3);
    }
}
