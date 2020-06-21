using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    private float playerSpeed = 2.5f;
    private Rigidbody2D rbPlayer;
    private Animator animPlayer;

    private string facingDirection = "Front";

    private bool freezePlayer = false;

    private int health = 100;
    private int baseDamage = 60;

    private GameManager gameManager;

    private BoxCollider2D collider;

    public int amountOfKeys = 0;

    private Vector2 center { get { return collider.bounds.center; } }

    public Text playerHealthText;

    private bool died = false;

    // Use this for initialization
    void Start () {
        collider = GetComponent<BoxCollider2D>();
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        rbPlayer = this.GetComponent<Rigidbody2D>();
        animPlayer = this.GetComponent<Animator>();
	}

    void Update()
    {
        if(died)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rbPlayer.velocity = new Vector2(0, 0);
            animPlayer.SetFloat("horizontalSpeed", 0);
            animPlayer.SetFloat("verticalSpeed", 0);

            int directionID = 0;

            switch (facingDirection)
            {
                case "Front":
                    animPlayer.Play("front_slashing");
                    directionID = 3;
                    break;
                case "Back":
                    animPlayer.Play("back_slashing");
                    directionID = 1;
                    break;
                case "Left":
                    animPlayer.Play("left_slashing");
                    directionID = 4;
                    break;
                case "Right":
                    animPlayer.Play("right_slashing");
                    directionID = 2;
                    break;
            }

            StartCoroutine(PlayerAttackFreeze(1, directionID));
        }
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        if (!freezePlayer && !died)
        {
            MovePlayer();
        }
	}

    private bool hitSomething(out RaycastHit2D hit, int direction)
    {
        Vector2 start = transform.position;

        Vector2 end = start + new Vector2(0, 0);

        float raySize = 1.2f;

        switch (direction)
        {
            case 1:
                end = start + new Vector2(0, raySize);
                break;
            case 2:
                end = start + new Vector2(raySize, 0);
                break;
            case 3:
                end = start + new Vector2(0, -raySize);
                break;
            case 4:
                end = start + new Vector2(-raySize, 0);
                break;
        }

        collider.enabled = false;
        hit = Physics2D.Linecast(center, end);
        collider.enabled = true;

        if (hit.transform == null)
        {
            return true;
        }

        if (hit.transform.tag == "Enemy" || hit.transform.tag == "Boss")
        {
            hit.transform.GetComponent<EnemyController>().gotHitByPlayer(gameManager.calculateDamageTaken(baseDamage, false));
        }

        return false;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "BossAreaEnter")
        {
            gameManager.SwitchBossActivationState(true);
        }
        else if (collision.tag == "Enemy")
        {
            gameManager.SwitchEnemyActivationState(true, collision.GetComponent<EnemyController>());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "BossAreaEnter")
        {
            gameManager.SwitchBossActivationState(false);
        }
        else if (collision.tag == "Enemy")
        {
            gameManager.SwitchEnemyActivationState(false, collision.GetComponent<EnemyController>());
        }
    }

    private IEnumerator PlayerAttackFreeze(int time, int directionID)
    {
        freezePlayer = true;
        yield return new WaitForSeconds(0.2f);
        RaycastHit2D hit;
        hitSomething(out hit, directionID);
        yield return new WaitForSeconds((time - 0.2f));
        freezePlayer = false;
    }

    public void gotHitByEnemy(int damage)
    {
        int actualDamage = gameManager.calculateDamageTaken(damage, true);
        health -= actualDamage;
        playerHealthText.text = "Health: " + health;

        if(health <= 0)
        {
            died = true;
            health = 0;
            playerHealthText.text = "Health: " + health;
            playerDied();
        }
    }

    void playerDied()
    {
        animPlayer.SetFloat("horizontalSpeed", 0);
        animPlayer.SetFloat("verticalSpeed", 0);
        animPlayer.SetInteger("Direction", 0);
        StartCoroutine(deadPlayer());
    }

    IEnumerator deadPlayer()
    {
        animPlayer.Play("dying");
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("MainMenu");
    }

    void MovePlayer()
    {
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");

        rbPlayer.velocity = new Vector2(hor * playerSpeed, ver * playerSpeed);

        animPlayer.SetFloat("horizontalSpeed", hor);
        animPlayer.SetFloat("verticalSpeed", ver);

        if(hor == 0 && ver == 0)
        {
            return;
        }

        if (Mathf.Abs(hor) > Mathf.Abs(ver))
        {
            if (hor < 0)
            {
                facingDirection = "Left";
                animPlayer.SetInteger("Direction", 4);
            }
            else
            {
                facingDirection = "Right";
                animPlayer.SetInteger("Direction", 2);
            }
        }
        else
        {
            if (ver < 0)
            {
                facingDirection = "Front";
                animPlayer.SetInteger("Direction", 1);
            }
            else
            {
                facingDirection = "Back";
                animPlayer.SetInteger("Direction", 3);
            }
        }
    }

}
