using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{

    public bool locked;
    public Sprite[] doorSprites;

    public bool permaUnlock;

    void Start()
    {
        if (!locked)
        {
            permaUnlock = true;
        }
    }
    public void Unlock()
    {
        GetComponent<SpriteRenderer>().sprite = doorSprites[1];
        locked = false;
    }

    public void Lock()
    {
        GetComponent<SpriteRenderer>().sprite = doorSprites[0];
        locked = true;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !locked)
        {
            if(SceneManager.GetActiveScene().buildIndex != SceneManager.sceneCountInBuildSettings - 1)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            else
            {
                SceneManager.LoadScene(0);
            }

        }
    }
}
