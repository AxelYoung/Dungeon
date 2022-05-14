using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public float speed;

    public Sprite[] laserPointSprites;
    public Sprite[] laserSprites;

    private SpriteRenderer sender;
    private SpriteRenderer receiver;
    private Transform laser;

    public AudioClip laserOn;
    public AudioClip[] loadUp;

    private AudioSource au;

    public float volume;

    private Transform player;

    private bool playerNear;

    private float trueVolume;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        au = GetComponent<AudioSource>();
        sender = GetComponent<SpriteRenderer>();
        receiver = transform.GetChild(0).GetComponent<SpriteRenderer>();
        laser = transform.GetChild(1);
        laser.gameObject.SetActive(false);
        StartCoroutine(LaserControl());
        StartCoroutine(LaserAnim());
    }

    void Update()
    {
        if (Vector2.Distance(player.position, laser.transform.position) <= 10)
        {
            trueVolume = volume / (Vector2.Distance(player.position, transform.position));
        }
        else
        {
            trueVolume = 0;
        }
    }
    public IEnumerator LaserControl()
    {
        sender.sprite = laserPointSprites[0];
        receiver.sprite = laserPointSprites[0];
        yield return new WaitForSeconds(speed / 4);
        sender.sprite = laserPointSprites[1];
        receiver.sprite = laserPointSprites[1];
        au.PlayOneShot(loadUp[0], trueVolume);
        yield return new WaitForSeconds(speed / 4);
        sender.sprite = laserPointSprites[2];
        receiver.sprite = laserPointSprites[2];
        au.PlayOneShot(loadUp[1], trueVolume);
        yield return new WaitForSeconds(speed / 4);
        sender.sprite = laserPointSprites[3];
        receiver.sprite = laserPointSprites[3];
        au.PlayOneShot(loadUp[2], trueVolume);
        yield return new WaitForSeconds(speed / 4);
        laser.gameObject.SetActive(true);
        au.PlayOneShot(laserOn, trueVolume);
        yield return new WaitForSeconds(speed);
        laser.gameObject.SetActive(false);
        StartCoroutine(LaserControl());
    }

    public IEnumerator LaserAnim()
    {
        for (int i = 0; laser.childCount > i; i++)
        {
            laser.GetChild(i).GetComponent<SpriteRenderer>().color = GetComponent<SpriteRenderer>().color;
            laser.GetChild(i).GetComponent<SpriteRenderer>().sprite = laserSprites[0];
        }
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; laser.childCount > i; i++)
        {
            laser.GetChild(i).GetComponent<SpriteRenderer>().sprite = laserSprites[1];
        }
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(LaserAnim());
    }
}
