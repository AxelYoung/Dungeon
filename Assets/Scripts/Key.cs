using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public GameObject particle;

    public AudioClip clip;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Instantiate(particle, new Vector2(transform.position.x, transform.position.y + 2.2f), Quaternion.identity);
            collision.GetComponent<PlayerController>().hasKey = true;
            collision.GetComponent<PlayerController>().key = gameObject;
            GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioSource>().PlayOneShot(clip, 0.75f);
            gameObject.SetActive(false);
        }
    }
}
