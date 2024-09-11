using JetBrains.Annotations;
using System;
using TMPro.Examples;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterControl : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;

    public Animator animator;
    public Rigidbody2D rb2D;

    public Transform groundCheckPosition;
    public Transform respawnPoint;
    public float groundCheckRadius;
    public LayerMask groundCheckLayer;
    public bool grounded;
    public bool doublejump;

    public bool nearBonfire;
    public float healthRegen = 5f;

    public GameObject projectile;
    public float launchVelocity = 700f;


    public Image filler; // This is the image. We'll adjust fillamount value

    
    public float counter; // this runs from 0 -> maxCounter in seconds and starts again from 0.
    public float maxCounter; // this tells us how long it takes to animate the filler

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        grounded = Physics2D.OverlapCircle(groundCheckPosition.position, groundCheckRadius, groundCheckLayer);
        
        transform.Translate(Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime, 0, 0);

        if(Input.GetAxisRaw("Horizontal") != 0)
        {
            // This means you either have a or d pressed down
            transform.localScale = new Vector3(Input.GetAxisRaw("Horizontal"), 1, 1);
            animator.SetBool("Walk", true);
        }
        else
        {
            // nothing happens
            animator.SetBool("Walk", false);
        }

        if (grounded)
        {
            doublejump = true;
        }

        if(Input.GetKeyDown(KeyCode.F))
        {
            animator.SetTrigger("AxeThrow");
        }

        if(Input.GetButtonDown("Jump") && grounded | doublejump)
        {
            if (doublejump && !grounded)
            {
                rb2D.linearVelocity = new Vector2(0, 5);
                doublejump = !doublejump;
            }
            if (grounded)
            {
                rb2D.linearVelocity = new Vector2(0, jumpForce);
            }
            animator.SetTrigger("Jump");
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            GameObject Axe = Instantiate(projectile, transform.position, transform.rotation);

            float direction = transform.localScale.x > 0 ? 1 : -1;

            Axe.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector3(launchVelocity * direction, 0, 0));

            if (direction == -1)
            {
                Vector3 axeScale = Axe.transform.localScale;
                axeScale.x *= -1;
                Axe.transform.localScale = axeScale;
            }

            Destroy(Axe, 1);
        }



        if (transform.position.y < -15)
        {
            transform.position = respawnPoint.position;
        }

        // health bar stuff

        if(counter > maxCounter)
        {
            GameManager.manager.previousHealth = GameManager.manager.health;
            counter = 0;
        }
        else
        {
            counter += Time.deltaTime;
        }

        filler.fillAmount = Mathf.Lerp(GameManager.manager.previousHealth / GameManager.manager.maxHealth, GameManager.manager.health / GameManager.manager.maxHealth, counter / maxCounter);

        // regenerating health while near bonfire and holding E
        if (nearBonfire && Input.GetKey(KeyCode.E))
        {
            RegenerateHealth();
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("LevelEnd"))
        {
            SceneManager.LoadScene("Map");
        }

        if (collision.CompareTag("HealthPickup"))
        {
            GameManager.manager.health += 20;
            if (GameManager.manager.health >= GameManager.manager.maxHealth)
            {
                GameManager.manager.health = GameManager.manager.maxHealth;
            }
            Destroy(collision.gameObject);
        }

        if(collision.CompareTag("MaxHealthUp"))
        {
            GameManager.manager.maxHealth += 20;
            Destroy(collision.gameObject);
        }

        if(collision.CompareTag("Bonfire"))
        {
            nearBonfire = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
       if (collision.CompareTag("Bonfire"))
        {
            nearBonfire = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Trap"))
        {
            TakeDamage(15);
        }
    }

    public void TakeDamage(float damage)
    {
        GameManager.manager.previousHealth = filler.fillAmount * GameManager.manager.maxHealth;
        counter = 0;
        GameManager.manager.health -= damage;
    }

    private void RegenerateHealth()
    {
        GameManager.manager.health += healthRegen * Time.deltaTime;
        if (GameManager.manager.health > GameManager.manager.maxHealth)
        {
            GameManager.manager.health = GameManager.manager.maxHealth;
        }
    }
}
