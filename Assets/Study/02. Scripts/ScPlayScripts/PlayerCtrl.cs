using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;

    [HideInInspector]
    public bool dirRight = true;

    [HideInInspector]
    public bool jump = false;
    public float jumpForce = 1000f;
    public AudioClip[] jumpClips;

    private bool grounded = false;
    private Transform groundCheck;

    public float moveForce = 365f;
    public float maxSpeed = 5.0f;

    public float tauntprobability = 50.0f;
    public AudioClip[] taunts;
    private int tauntIndex;
    public float tauntDelay = 1.0f;

    bool isButtonDown = false;

    private void Awake()
    {
        groundCheck = transform.Find("GroundCheck");
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));

        if (isButtonDown && grounded)
        {
            jump = true;
            isButtonDown = false;
        }
    }

    private void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");
        h = UltimateJoystick.GetHorizontalAxis("JoyStick");
        anim.SetFloat("Speed", Mathf.Abs(h));

        if (h * rb.velocity.x < maxSpeed)
        {
            rb.AddForce(Vector2.right * h * moveForce);
        }

        if (Mathf.Abs(rb.velocity.x) > maxSpeed)
        {
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);
        }

        if (h > 0 && !dirRight)
        {
            Flip();
        }
        else if (h < 0 && dirRight)
        {
            Flip();
        }

        if (jump)
        {
            anim.SetTrigger("Jump");

            int i = Random.Range(0, jumpClips.Length);
            AudioSource.PlayClipAtPoint(jumpClips[i], transform.position);

            rb.AddForce(new Vector2(0f, jumpForce));

            jump = false;
        }
    }

    void Flip()
    {
        dirRight = !dirRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public IEnumerator Taunt()
    {
        float tauntChance = Random.Range(0f, 100f);
        if (tauntChance > tauntprobability)
        {
            yield return new WaitForSeconds(tauntDelay);

            if (!GetComponent<AudioSource>().isPlaying)
            {
                tauntIndex = TauntRandom();

                GetComponent<AudioSource>().clip = taunts[tauntIndex];
                GetComponent<AudioSource>().Play();
            }
        }
    }

    int TauntRandom()
    {
        int i = Random.Range(0, taunts.Length);

        if (i == tauntIndex)
        {
            return TauntRandom();
        }
        else
        {
            return i;
        }
    }

    public void JumpButtonClick()
    {
        isButtonDown = true;
    }
}
