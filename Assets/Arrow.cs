using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Arrow : MonoBehaviour
{
    public AudioClip impact;
    public float speed;

    public GameObject impactParticle;

    private AudioSource au;

    public float volume;

    private float trueVolume;

    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        GetComponent<SpriteRenderer>().color = GameObject.FindObjectOfType<Tilemap>().color;
        au = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Vector2.Distance(player.position, transform.position) <= 10)
        {
            trueVolume = volume / (Vector2.Distance(player.position, transform.position));
        }
        else
        {
            trueVolume = 0;
        }
    }

    void FixedUpdate()
    {
        transform.position += transform.right * speed;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Impact();
            collision.GetComponent<PlayerController>().Die();
        }
        if(collision.tag == "Tile")
        {
            Impact();
        }
    }

    public void Impact()
    {
        Instantiate(impactParticle, transform.position, transform.rotation);
        au.PlayOneShot(impact, trueVolume);
        Destroy(gameObject);
    }
}
