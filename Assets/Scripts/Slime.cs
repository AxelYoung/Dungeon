using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    public float jumpForce;

    private Animator anim;
    private Rigidbody2D rb;

    private int moveDir = -1;

    private bool jumped;
    private bool grounded;

    public GameObject deathPart;

    public AudioClip death;
    public AudioClip jump;

    public float audioVolume;

    private AudioSource au;

    private Transform player;

    private float trueVolume;
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        au = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioSource>();
        int rand = Random.Range(0, 2);
        if(rand == 0)
        {
            moveDir = -1;
        }
        else
        {
            moveDir = 1;
        }
        StartCoroutine(Attack());
    }

    void Update()
    {
        if(Vector2.Distance(player.position, transform.position) <= 8)
        {
            trueVolume = audioVolume / (Vector2.Distance(player.position, transform.position));
        }
        else
        {
            trueVolume = 0;
        }
        RaycastHit2D groundCheck = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 0.6f), Vector2.down, 0.1f);
        if(groundCheck.collider != null)
        {
            if(groundCheck.collider.tag == "Tile" || groundCheck.collider.tag == "Ceiling")
            {
                grounded = true;
            }
            else
            {
                grounded = false;
            }
        }
        else
        {
            grounded = false;
        }
        if (jumped && grounded)
        {
            anim.SetInteger("JumpState", 0);
        }
        RaycastHit2D wallCheck = Physics2D.Raycast(new Vector2(transform.position.x + (moveDir * 0.6f), transform.position.y), transform.right * moveDir, 2);
        if (wallCheck.collider != null)
        {
            if (wallCheck.collider.tag == "Tile" || wallCheck.collider.tag == "Enemy")
            {
                moveDir = -moveDir;
                transform.localScale = new Vector2(moveDir, 1);
            }
        }
    }

    public IEnumerator Attack()
    {
        yield return new WaitForSeconds(Random.Range(1f, 2f));
        jumped = false;
        anim.SetInteger("JumpState", 1);
        yield return new WaitForSeconds(0.25f);
        rb.AddForce(new Vector2(moveDir, 1.5f) * jumpForce);
        au.PlayOneShot(jump, trueVolume);
        yield return new WaitForSeconds(0.1f);
        jumped = true;
        StartCoroutine(Attack());
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "Player")
        {
            collision.transform.GetComponent<PlayerController>().Die();
        }
    }

    public void Die()
    {
        au.PlayOneShot(death, trueVolume);
        Instantiate(deathPart, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
