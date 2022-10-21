using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    bool isJump = true;
    bool isDead = false;
    int idMove = 0;
    Animator anim;

    [SerializeField] private float jumpForce = 10f;
    public GameObject projectile;
    public Vector2 projectileVelocity;
    public Vector2 projectileOffset;
    public float cooldown = 0.5f;
    bool isCanShoot = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        EnemyController.enemyKilled = 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveLeft();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveRight();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            Idle();
        }

        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            Idle();
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Fire();
        }
        Move();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Peluru") && !collision.gameObject.GetComponent<Bullet>().isShooting)
        {
            isCanShoot = true;
            Destroy(collision.gameObject);
            Debug.Log("Can Shoot : " + isCanShoot.ToString());
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Contact");
            isDead = true;
            SceneManager.LoadScene("Game Over");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Coin"))
        {
            Data.score += 15;
            Destroy(collision.gameObject);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (isJump)
        {
            anim.ResetTrigger("jump");
            if (idMove == 0) anim.SetTrigger("idle");
            isJump = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        anim.SetTrigger("jump");
        anim.ResetTrigger("idle");
        anim.ResetTrigger("run");
        isJump = true;
    }

    public void MoveRight()
    {
        idMove = 1;
    }

    public void MoveLeft()
    {
        idMove = 2;
    }

    public void Move()
    {
        if (idMove == 1 && !isDead)
        {
            if (!isJump) anim.SetTrigger("run");
            transform.Translate(-1 * Time.deltaTime * 5f, 0,0);
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        if (idMove == 2 && !isDead)
        {
            if (!isJump) anim.SetTrigger("run");
            transform.Translate(1 * Time.deltaTime * 5f, 0, 0);
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }

    public void Idle()
    {
        if (!isJump)
        {
            anim.ResetTrigger("run");
            anim.ResetTrigger("jump");
            anim.SetTrigger("idle");
        }
        idMove = 0;
    }

    public void Jump()
    {
        if (!isJump)
        {
            GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpForce,ForceMode2D.Impulse);
        }
    }

    void Fire()
    {
        if (isCanShoot)
        {
            GameObject bullet = Instantiate(projectile,(Vector2) transform.position - projectileOffset * transform.localScale.x,Quaternion.identity);
            bullet.GetComponent<Bullet>().isShooting = true;
            bullet.transform.localScale = transform.localScale;
            Vector2 velocity = new Vector2(projectileVelocity.x * transform.localScale.x,projectileVelocity.y);
            bullet.GetComponent<Rigidbody2D>().velocity = velocity;
            StartCoroutine(CanShoot());
            anim.SetTrigger("shoot");
        }
    }

    IEnumerator CanShoot()
    {
        anim.SetTrigger("shoot");
        isCanShoot = false;
        yield return new WaitForSeconds(cooldown);
        isCanShoot = true;
    }
}
