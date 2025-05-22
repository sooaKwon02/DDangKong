using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public int score = 0;

    private PlayerCtrl playerCtrl;
    private int previousScore = 0;
    public Text[] spScore;
    
    void Awake()
    {
        playerCtrl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCtrl>();
    }

    void Update()
    {
        spScore[0].text = "Score : " + score;
        spScore[1].text = "Score : " + score;

        if(previousScore != score)
        {
            playerCtrl.StartCoroutine(playerCtrl.Taunt());
        }
        previousScore = score;
    }
}
