using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float speed;
    public float jumpForce;
    public GameObject bulletObj;

    private Rigidbody2D rb;
    private int moveDir = 1;

    private bool grounded;

    private bool canShoot = true;
    private bool canShootX;
    private bool canShootXN;
    private bool canShootY;

    private Animator anim;

    public float rechargeSpeed;
    public Sprite[] rechargeAnim;

    private SpriteRenderer recharge;

    private bool canMove = true;

    public GameObject deathPart;

    private RaycastHit2D floorRay;

    public AudioClip jump;
    public AudioClip die;
    public AudioClip shoot;
    public AudioClip[] rechargeSound;

    public AudioSource au;

    public float volume;

    private Door dr;

    public bool hasKey;
    public GameObject key;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        dr = GameObject.FindObjectOfType<Door>();
        anim = GetComponent<Animator>();
        recharge = transform.GetChild(0).GetComponent<SpriteRenderer>();
        canMove = false;
        StartCoroutine(Spawn());
    }

    void Update()
    {
        if (!dr.permaUnlock)
        {
            if (hasKey)
            {
                dr.Unlock();
            }
            else
            {
                dr.Lock();
            }
        }
        RaycastHit2D wallRay = Physics2D.Raycast(new Vector3(transform.position.x + (moveDir * 0.6f), transform.position.y), moveDir * transform.right, 0.1f);
        if (wallRay.collider != null)
        {
            if (wallRay.collider.tag == "Tile")
            {
                Flip(-moveDir);
            }
        }
        floorRay = Physics2D.Raycast(new Vector3(transform.position.x, transform.position.y - 0.6f), Vector2.down, 0.1f);
        if (floorRay.collider != null)
        {
            if (floorRay.collider.transform.tag == "Tile" || floorRay.collider.transform.tag == "Ceiling")
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
        RaycastHit2D gunRayX = Physics2D.Raycast(new Vector3(transform.position.x + 0.6f, transform.position.y), transform.right, 1.5f);
        if (gunRayX.collider != null)
        {
            if (gunRayX.collider.tag == "Tile")
            {
                canShootX = false;
            }
            else
            {
                canShootX = true;
            }
        }
        else
        {
            canShootX = true;
        }
        RaycastHit2D gunRayXN = Physics2D.Raycast(new Vector3(transform.position.x - 0.6f, transform.position.y), -transform.right, 1.5f);
        if (gunRayXN.collider != null)
        {
            if (gunRayXN.collider.tag == "Tile")
            {
                canShootXN = false;
            }
            else
            {
                canShootXN = true;
            }
        }
        else
        {
            canShootXN = true;
        }
        RaycastHit2D gunRayY = Physics2D.Raycast(new Vector3(transform.position.x, transform.position.y + 0.6f), transform.up, 1.5f);
        if (gunRayY.collider != null)
        {
            if (gunRayY.collider.tag == "Tile")
            {
                canShootY = false;
            }
            else
            {
                canShootY = true;
            }
        }
        else
        {
            canShootY = true;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            Up();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            Left();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Down();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Right();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Tap();
        }
        anim.SetBool("Grounded", grounded);
    }

    public void Tap()
    {
        if (grounded && canMove)
        {
            Jump();
        }
    }

    public void Up()
    {
        if (!canShoot && grounded && canMove)
        {
            StartCoroutine(Recharge());
        }
        if (canShoot && canShootY)
        {
            SpawnBullet(new Vector2(0, 1), 90);
        }
    }

    public void Down()
    {
        floorRay = Physics2D.Raycast(new Vector3(transform.position.x, transform.position.y - 0.6f), Vector2.down, 0.1f);
        if (floorRay.collider != null)
        {
            if (floorRay.collider.usedByEffector == true)
            {
                StartCoroutine(FallThroughFloor(floorRay.transform.GetComponent<Effector2D>()));
            }
        }
    }

    public void Left()
    {
        if (!canShoot && grounded && canMove)
        {
            StartCoroutine(Recharge());
        }
        if (canShoot && canShootXN)
        {
            SpawnBullet(new Vector2(-1, 0), 0);
            Flip(-1);
        }
    }

    public void Right()
    {
        if (!canShoot && grounded && canMove)
        {
            StartCoroutine(Recharge());
        }
        if (canShoot && canShootX)
        {
            SpawnBullet(new Vector2(1, 0), 0);
            Flip(1);
        }
    }

    void FixedUpdate()
    {
        if (canMove)
        {
            rb.velocity = new Vector2(moveDir * speed, rb.velocity.y);
        }
    }

    public void Flip(int dir)
    {
        moveDir = dir;
        transform.localScale = new Vector2(moveDir, 1);
    }

    public void Jump()
    {
        au.PlayOneShot(jump, volume / 3);
        Vibration.Vibrate(50);
        rb.AddForce(Vector2.up * jumpForce * 100);
    }

    public void SpawnBullet(Vector3 dir, int angle)
    {
        GameObject bullet = Instantiate(bulletObj);
        bullet.transform.position = transform.position + (dir * 0.5f);
        bullet.transform.localScale = new Vector2(dir.x + dir.y, 1);
        bullet.transform.rotation = Quaternion.Euler(0, 0, angle);
        au.PlayOneShot(shoot, volume);
        Vibration.Vibrate(100);
        canShoot = false;
        recharge.sprite = null;
    }

    public IEnumerator FallThroughFloor(Effector2D eff)
    {
        eff.useColliderMask = true;
        yield return new WaitForSeconds(0.5f);
        eff.useColliderMask = false;
    }

    public IEnumerator Recharge()
    {
        canMove = false;
        rb.velocity = Vector2.zero;
        anim.SetBool("Reload", true);
        yield return new WaitForSeconds(rechargeSpeed / 4);
        recharge.sprite = rechargeAnim[0];
        Vibration.Vibrate(10);
        au.PlayOneShot(rechargeSound[0], volume / 2);
        yield return new WaitForSeconds(rechargeSpeed / 4);
        recharge.sprite = rechargeAnim[1];
        Vibration.Vibrate(10);
        au.PlayOneShot(rechargeSound[1], volume / 2);
        yield return new WaitForSeconds(rechargeSpeed / 4);
        recharge.sprite = rechargeAnim[2];
        Vibration.Vibrate(10);
        au.PlayOneShot(rechargeSound[2], volume / 2);
        yield return new WaitForSeconds(rechargeSpeed / 4);
        recharge.sprite = rechargeAnim[3];
        Vibration.Vibrate(10);
        au.PlayOneShot(rechargeSound[3], volume / 2);
        canShoot = true;
        canMove = true;
        anim.SetBool("Reload", false);
    }

    public void Die()
    {
        rb.velocity = Vector2.zero;
        Instantiate(deathPart, transform.position, Quaternion.identity);
        if (hasKey)
        {
            key.transform.position = transform.position;
            hasKey = false;
            key.SetActive(true);
        }
        transform.position = Vector2.zero;
        canMove = false;
        anim.Rebind();
        au.PlayOneShot(die, volume);
        Vibration.Vibrate(1000);
        StartCoroutine(Spawn());
    }

    public IEnumerator Spawn()
    {
        yield return new WaitForSeconds(0.5f);
        canMove = true;
    }
}
