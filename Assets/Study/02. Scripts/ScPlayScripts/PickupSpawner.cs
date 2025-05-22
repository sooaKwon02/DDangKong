using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    public GameObject[] pickups;
    public float pickipDeliveryTime = 5f;
    public float dropRangeLeft;
    public float dropRangeRight;
    public float highLifeThreshold = 75f;
    public float lowHealthThreshold = 25f;

    private PlayerLife playerLife;

    private void Awake()
    {
        playerLife = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerLife>();
    }

    private void Start()
    {
        StartCoroutine(DeliverPickup());
    }

    public IEnumerator DeliverPickup()
    {
        yield return new WaitForSeconds(pickipDeliveryTime);

        float dropPosX = Random.Range(dropRangeLeft, dropRangeRight);

        Vector3 dropPos = new Vector3(dropPosX, 15f, 1f);

        if(playerLife.life >= highLifeThreshold)
        {
            Instantiate(pickups[0], dropPos, Quaternion.identity);
        }
        else if(playerLife.life <= lowHealthThreshold)
        {
            Instantiate(pickups[1], dropPos, Quaternion.identity);
        }
        else
        {
            int pickupIndex = Random.Range(0, pickups.Length);
            Instantiate(pickups[pickupIndex], dropPos, Quaternion.identity);
        }
    }
}
