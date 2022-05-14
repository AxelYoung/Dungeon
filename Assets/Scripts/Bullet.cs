using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Bullet : MonoBehaviour
{
    private float lifeSpan;

    public float speed;

    private AudioSource au;

    public AudioClip contact;

    public float volume;

    void Start()
    {
        GetComponent<SpriteRenderer>().color = GameObject.FindObjectOfType<Tilemap>().color;
        au = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioSource>();
        StartCoroutine(DestroyBullet());
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            au.PlayOneShot(contact, volume);
            collision.GetComponent<Slime>().Die();
        }
    }

    void Update()
    {
        if(transform.parent.localScale.x != -1)
        {
            if(transform.position.x <= 5f)
            {
                transform.position += transform.right * speed;
            }
        }
        else
        {
            if(transform.position.x >= -5f)
            {
                transform.position -= transform.right * speed;
            }
        }

    }

    public IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(1f/3f);
        Destroy(transform.parent.gameObject);
    }
}
