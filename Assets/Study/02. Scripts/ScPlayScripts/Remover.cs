using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Remover : MonoBehaviour
{
    public GameObject splash;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<FollowCamera>().enabled = false;

            if (GameObject.FindGameObjectWithTag("LifeBar").activeSelf)
            {
                GameObject.FindGameObjectWithTag("LifeBar").SetActive(false);
            }

            Instantiate(splash, col.transform.position, transform.rotation);
            Destroy(col.gameObject);
            StartCoroutine("ReloadGame");
        }
        else
        {
            Instantiate(splash, col.transform.position, transform.rotation);
            Destroy(col.gameObject);
        }
    }

    IEnumerator ReloadGame()
    {
        yield return new WaitForSeconds(2);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
