using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dispenser : MonoBehaviour
{
    private SpriteRenderer sr;

    public Sprite[] dispenserLoading;

    public float speed;

    public float volume;
    private float trueVolume;

    private Transform player;

    private AudioSource au;

    public AudioClip shoot;

    public GameObject arrow;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        au = GetComponent<AudioSource>();
        sr = GetComponent<SpriteRenderer>();
        StartCoroutine(DispenserControl());
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

    public IEnumerator DispenserControl()
    {
        sr.sprite = dispenserLoading[0];
        yield return new WaitForSeconds(speed / 5);
        sr.sprite = dispenserLoading[1];
        yield return new WaitForSeconds(speed / 5);
        sr.sprite = dispenserLoading[2];
        yield return new WaitForSeconds(speed / 5);
        sr.sprite = dispenserLoading[3];
        yield return new WaitForSeconds(speed / 5);
        sr.sprite = dispenserLoading[4];
        Instantiate(arrow, transform.position + transform.right, transform.rotation);
        au.PlayOneShot(shoot, trueVolume);
        yield return new WaitForSeconds(speed / 5);
        StartCoroutine(DispenserControl());
    }
}
