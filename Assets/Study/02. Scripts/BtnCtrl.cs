using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnCtrl : MonoBehaviour
{
    public GameObject spObj;
    public Text txt;
    private bool isPlay;

    public Sprite[] spImgs;  //heap
    public Image spAnim;
    int spImgCount;  //stack
    float animTime;

    public GameObject scroll;
    public GameObject[] btnCard;
    public Transform bombPrefab;
    bool scrollPlay;



    void Start()
    {
        //spImgs = new Sprite[5];
        spAnim.sprite = spImgs[0];
    }
    //private void Awake()
    //{
    //    //reference����
    //    //Debug.Assert(go); ->������ �߿�
    //}
    void Update()
    {
        if (isPlay)  //isPlay = true�϶�
        {
            if(Time.time > animTime) //animTime���� Ŭ��
            {
                spImgCount += 1; //1 �ø���
                if(spImgCount > 4) //4���� ũ��
                {
                    spImgCount = 0; 
                }
                spAnim.sprite = spImgs[spImgCount];
                animTime = Time.time + 0.3f;
            }
        }
    }
    public void Play()
    {
        //    Destroy(gameObject);
        //    Destroy(this.gameObject);
        if (!isPlay)
        {
            scroll.SetActive(false);
            btnCard[0].SetActive(false);
            btnCard[1].SetActive(false);
            isPlay = true;
            spObj.SetActive(true);
            txt.text = "STOP";
        }
        else
        {
            btnCard[0].SetActive(true);
            btnCard[1].SetActive(true);
            isPlay = false;
            spObj.SetActive(false);
            txt.text = "PLAY";
        }
    }
    public void Btn_Card()
    {

        if (!scrollPlay)
        {
            scrollPlay = true;
            scroll.SetActive(true);
        }
        else
        {
            scrollPlay = false;
            scroll.SetActive(false);
        }
    }

    public void Btn_CardUp()
    {
        Transform bomb = Instantiate(bombPrefab, new Vector3(0, 0, 0), Quaternion.identity) as Transform;
        bomb.parent = scroll.transform;
    }
}

