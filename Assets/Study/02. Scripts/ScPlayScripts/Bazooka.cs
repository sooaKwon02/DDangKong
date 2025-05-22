using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bazooka : MonoBehaviour
{
    private Animator anim;
    private PlayerCtrl playerCtrl;

    public Rigidbody2D rocket;
    public float rocketSpeed = 20.0f;

    public bool shooting;

    bool isButtonDown = false;

    private void Awake()
    {
        anim = transform.root.gameObject.GetComponent<Animator>();
        playerCtrl = transform.root.GetComponent<PlayerCtrl>();
    }

    void Update()
    {
        if (isButtonDown)
        {
            shooting = false;
            anim.SetTrigger("Shoot");
            GetComponent<AudioSource>().Play();

            if (playerCtrl.dirRight)
            {
                Rigidbody2D bulletInstance = Instantiate(rocket, transform.position, Quaternion.Euler(new Vector3(0, 0, 0))) as Rigidbody2D;
                bulletInstance.velocity = new Vector2(rocketSpeed, 0);
            }
            else
            {
                Rigidbody2D bulletInstance = Instantiate(rocket, transform.position, Quaternion.Euler(new Vector3(0, 0, 180f))) as Rigidbody2D;
                bulletInstance.velocity = new Vector2(-rocketSpeed, 0);
            }
            isButtonDown = false;
        }
    }

    public void AttackButtonClick()
    {
        isButtonDown = true;
    }
}