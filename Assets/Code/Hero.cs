using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Hero : MonoBehaviour
{
    public Rigidbody2D rigidbody2;
    public Animator animator;
    private int score = 0;
    private int lives = 3;

    private Bullet bullet;
    private int reShoot = 100;
    private int com = 0;

    public Transform GroundCheck;
    [SerializeField]
    public LayerMask whatIsGround;
    private bool isGrounded = false;

    void Start()
    {
        rigidbody2 = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        bullet = Resources.Load<Bullet>("fireball");
    }

    void Update()
    {
        CheckGround();

        if (isGrounded) animator.SetInteger("AnimNum", 0);
        if (isGrounded && Input.GetButtonDown("Fire2"))
        {
            rigidbody2.velocity = Vector3.zero;
            Jump();
        }

        if (Input.GetAxis("Horizontal") == 0 && isGrounded) animator.SetInteger("AnimNum", 0);
        else if (Input.GetAxis("Horizontal")!=0 && isGrounded)
        {
            Flip();
            animator.SetInteger("AnimNum", 1);
        }

        if (com < 200) com++;
        else com = 198;
        if (Input.GetButtonDown("Fire1") && (com >= reShoot))
        {
            Shoot();
            com = 0;
        }
    }

    private void FixedUpdate()
    {
        rigidbody2.velocity = new Vector2(Input.GetAxis("Horizontal") * 6.0f, rigidbody2.velocity.y);
    }


    private void Jump()
    {
        animator.SetInteger("AnimNum", 2);
        rigidbody2.AddForce(transform.up * 10.0f, ForceMode2D.Impulse);
    }

    private void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(GroundCheck.position, 0.1F, whatIsGround);
        if (!isGrounded) animator.SetInteger("AnimNum", 2);
        if (Input.GetAxis("Horizontal") > 0) transform.localRotation = Quaternion.Euler(0, 0, 0);
        if (Input.GetAxis("Horizontal") < 0) transform.localRotation = Quaternion.Euler(0, 180, 0);
    }

    private void Flip()
    {
        if (Input.GetAxis("Horizontal") > 0) transform.localRotation = Quaternion.Euler(0, 0, 0);
        if (Input.GetAxis("Horizontal") < 0) transform.localRotation = Quaternion.Euler(0, 180, 0);
    }


    private void Shoot()
    {
        animator.SetInteger("AnimNum", 3);
        Vector3 position = transform.position;
        position.y -=0.4F; 
        Bullet newBullet = Instantiate(bullet, position, bullet.transform.rotation) as Bullet;
        newBullet.Direction = transform.right;
        if (transform.right.x>0)
            newBullet.transform.localRotation = Quaternion.Euler(0, 0, 0);
        else newBullet.transform.localRotation = Quaternion.Euler(0, 180, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Money")
        {
            ++score;
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.tag == "Enemy")
        {
            GetDamage();
        }
        else if (collision.gameObject.tag == "Poison")
        {
            Destroy(collision.gameObject);
            lives -= 2;
            if (lives < 1)
            {
                Invoke("ReloadLevel", 4);
            }
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.tag == "Bronze")
            SceneManager.LoadScene("2nd");
        else if (collision.gameObject.tag == "Silver")
            SceneManager.LoadScene("3d");
        else if (collision.gameObject.tag == "Fall")
        { 
            ReloadLevel();
        }
    }

    public void GetDamage()
    {
        if (lives > 0)
        {
            lives--;
            rigidbody2.AddForce(transform.up * 8f, ForceMode2D.Impulse);
        }
        else
        {
            Invoke ("ReloadLevel",4);
        }
    }

    private void OnGUI()
    {
        GUI.Box(new Rect(0, 0, 100, 30), "Lives= " + lives);
        GUI.Box(new Rect(100, 0, 100, 30), "Score= " + score);
    }

    void ReloadLevel()
    {
        Application.LoadLevel(Application.loadedLevel);
    }
}
