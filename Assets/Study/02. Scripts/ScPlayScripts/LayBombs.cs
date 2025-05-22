using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LayBombs : MonoBehaviour
{
    [HideInInspector]
    public bool bombLaid = false;
    public int bombCount = 0;
    public AudioClip bombsAway;
    public GameObject bomb;

    private Image bombHUD;
    bool isButtonDown = false;

    private void Awake()
    {
        bombHUD = GameObject.Find("Ui_bombHUD").GetComponent<Image>();
    }

    private void Update()
    {
        if (isButtonDown && !bombLaid && bombCount > 0)
        {
            bombCount--;
            bombLaid = true;

            AudioSource.PlayClipAtPoint(bombsAway, transform.position);

            Instantiate(bomb, transform.position, transform.rotation);
            isButtonDown = false;
        }

        bombHUD.enabled = bombCount > 0;
    }

    public void ItemButtonClick()
    {
        isButtonDown = true;
    }
}
