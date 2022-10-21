using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyController : MonoBehaviour
{
    bool isGrounded = false;
    bool isFacingRight = false;

    [SerializeField] private Transform batasKiri;
    [SerializeField] private Transform batasKanan;

    Rigidbody2D rigid;
    Animator animator;
    [SerializeField] private int hp = 1;
    private bool isDie = false;
    public static int enemyKilled = 0;
    [SerializeField] float speed = 2;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isGrounded && !isDie)
        {
            if (isFacingRight)
            {
                MoveRight();
            }
            else
            {
                MoveLeft();
            }

            if (transform.position.x >= batasKanan.position.x && isFacingRight)
            {
                Flip();
            }else if (transform.position.x <= batasKiri.position.x && !isFacingRight)
            {
                Flip();
            }
        }
    }

    public void MoveRight()
    {
        Vector3 pos = transform.position;
        pos.x += speed * Time.deltaTime;
        transform.position = pos;
        if (!isFacingRight)
        {
            Flip();
        }
    }

    public void MoveLeft()
    {
        Vector3 pos = transform.position;
        pos.x -= speed * Time.deltaTime;
        transform.position = pos;
        if (isFacingRight)
        {
            Flip();
        }
    }

    public void Flip()
    {
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
        isFacingRight = !isFacingRight;
    }

    void TakeDamage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            isDie = true;
            rigid.velocity = Vector2.zero;
            animator.SetBool("isDie",true);
            Destroy(gameObject, 0.8f);
            Data.score += 20;
            enemyKilled++;

            if (enemyKilled == 4)
            {
                SceneManager.LoadScene("Game Over");
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground")) {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }

    }
}
