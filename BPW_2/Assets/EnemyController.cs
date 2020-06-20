using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private bool isBoss = false;

    private LayerMask blockingLayer;

    private float minXWalkArea = 2.56f;
    private float maxXWalkArea = 15.36f;

    private float minYWalkArea = -15.36f;
    private float maxYWalkArea = -1.28f;

    float moveSpeed = 1.5f;

    private Rigidbody2D rBody;

    private Vector3 enemyRoomPosition;
    public int roomID;

    private GameObject player;

    private CapsuleCollider2D collider;

    private Vector2 endLocation = new Vector2(50,-40);

    private int minWanderCooldown = 3;
    private int maxWanderCooldown = 12;

    private bool chasePlayer = false;

    private Animator anim;

    private bool wandering = true;

    private Vector2 center { get { return collider.bounds.center; } }

    private string direction = "Front";

    private bool freezeEnemy = false;

    private SpriteRenderer spriteRenderer;

    public bool hasKey = false;

    private int enemyHealth = 100;
    private int attackDamage = 10;

    private bool dead = false;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<CapsuleCollider2D>();
        blockingLayer = LayerMask.GetMask("RaycastLayer");
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
        if(this.tag == "Boss")
        {
            isBoss = true;

            //double the walking area
            maxXWalkArea = (maxXWalkArea * 2);
            maxYWalkArea = (maxYWalkArea * 2);
            moveSpeed = 2.3f;

            enemyHealth = 1000;
            attackDamage = 100;
        }

        rBody = GetComponent<Rigidbody2D>();

        if (isBoss)
        {
            setBossWalkLimits();
        }
        else
        {
            setWalkAreaLimits();
        }

        pickNewEndDestination();
    }

    public void setRoomPosition(Vector3 roomPosition)
    {
        enemyRoomPosition = roomPosition;
    }

    public void activateChase(bool isChasing)
    {
        chasePlayer = isChasing;
        wandering = !isChasing;

        if (!isChasing)
        {
            pickNewEndDestination();
        }
        else
        {
            endLocation = new Vector2(0, 0);
            anim.SetFloat("horizontalSpeed", 0);
            anim.SetFloat("verticalSpeed", 0);
        }
    }

    private bool hitSomething(out RaycastHit2D hit, int direction, bool checkingForImpact)
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
        hit = Physics2D.Linecast(center, end, blockingLayer);
        collider.enabled = true;

        if (hit.transform == null)
        {
            return true;
        }

        if(hit.transform.name == "Player")
        {
            if (checkingForImpact)
            {
                player.GetComponent<PlayerController>().gotHitByEnemy(attackDamage);
            }
        }
        else if (!checkingForImpact)
        {
            pickNewEndDestination();
        }
        return false;
    }

    private void FixedUpdate()
    {
        if (freezeEnemy || dead)
        {
            return;
        }
        if (wandering)
        {
            moveEnemy();
        }

        if (chasePlayer)
        {
            if (Vector3.Distance(player.transform.position, this.transform.position) > 1.6f)
            {
                Vector3 direction = player.transform.position - this.transform.position;

                direction.Normalize();
                endLocation = direction;

                moveEnemy();
            }
            else
            {
                attack();
            }
        }
    }

    private void attack()
    {
        endLocation = new Vector2(0, 0);
        anim.SetFloat("horizontalSpeed", 0);
        anim.SetFloat("verticalSpeed", 0);

        int directionID = 0;

        switch (direction)
        {
            case "Back":
                anim.Play("back_slashing");
                directionID = 1;
                break;
            case "Right":
                anim.Play("right_slashing");
                directionID = 2;
                break;
            case "Front":
                anim.Play("front_slashing");
                directionID = 3;
                break;
            case "Left":
                anim.Play("left_slashing");
                directionID = 4;
                break;
        }
        StartCoroutine(AttackAnimationFreeze(1, directionID));
    }

    private IEnumerator AttackAnimationFreeze(int time, int directionID)
    {
        freezeEnemy = true;
        yield return new WaitForSeconds(0.2f);
        RaycastHit2D hit;
        hitSomething(out hit, directionID, true);
        yield return new WaitForSeconds((time - 0.2f));
        freezeEnemy = false;
    }

    public void gotHitByPlayer(int damage)
    {
        if (!dead)
        {
            enemyHealth -= damage;
            if (enemyHealth <= 0)
            {
                enemyDied();
            }
        }
    }

    private void enemyDied()
    {
        dead = true;
        collider.enabled = false;
        if (hasKey)
        {
            player.GetComponent<PlayerController>().amountOfKeys++;
        }
        anim.Play("Dying");
        SpriteRenderer spriteRen = gameObject.GetComponent<SpriteRenderer>();
        StartCoroutine(FadeTo(0.0f, 2, spriteRen));
        StartCoroutine(destroyTheEnemy());
    }

    IEnumerator FadeTo(float aValue, float aTime, SpriteRenderer spriteRend)
    {
        float alpha = spriteRend.material.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, aValue, t));
            spriteRend.material.color = newColor;
            yield return null;
        }
    }

    private IEnumerator destroyTheEnemy()
    {
        yield return new WaitForSeconds(4);
        Destroy(this.gameObject);
    }

    private IEnumerator destinationPickerCooldown(int cooldown)
    {
        yield return new WaitForSeconds(cooldown);
        if (!chasePlayer)
        {
            pickNewEndDestination();
        }
    }

    private IEnumerator startIdle(int idleTime)
    {
        anim.SetFloat("horizontalSpeed", 0);
        anim.SetFloat("verticalSpeed", 0);
        wandering = false;
        yield return new WaitForSeconds(idleTime);
        if (!chasePlayer)
        {
            wandering = true;
            pickNewEndDestination();
        }
    }

    void setBossWalkLimits()
    {
        minXWalkArea = 42f;
        maxXWalkArea = 72f;
        minYWalkArea = -60f;
        maxYWalkArea = -35f;
    }

    void setWalkAreaLimits()
    {
        minXWalkArea = (enemyRoomPosition.x + minXWalkArea);
        maxXWalkArea = (enemyRoomPosition.x + maxXWalkArea);

        minYWalkArea = (enemyRoomPosition.y + minYWalkArea);
        maxYWalkArea = (enemyRoomPosition.y + maxYWalkArea);
    }

    void moveEnemy()
    {
        rBody.MovePosition((Vector2)transform.position + (endLocation * moveSpeed * Time.deltaTime));
        anim.SetFloat("horizontalSpeed", endLocation.x);
        anim.SetFloat("verticalSpeed", endLocation.y);

        RaycastHit2D hit;

        if (Mathf.Abs(endLocation.x) > Mathf.Abs(endLocation.y))
        {
            if (endLocation.x < 0)
            {
                if (isBoss)
                {
                    spriteRenderer.sortingOrder = 0;
                }
                direction = "Left";
                anim.SetInteger("Direction", 4);
                hitSomething(out hit, 4, false);
            }
            else
            {
                if (isBoss)
                {
                    spriteRenderer.sortingOrder = 0;
                }
                direction = "Right";
                anim.SetInteger("Direction", 2);
                hitSomething(out hit, 2, false);
            }
        }
        else
        {
            if (endLocation.y < 0)
            {
                if (isBoss)
                {
                    spriteRenderer.sortingOrder = 0;
                }
                direction = "Front";
                anim.SetInteger("Direction", 3);
                hitSomething(out hit, 3, false);
            }
            else
            {
                if (isBoss)
                {
                    spriteRenderer.sortingOrder = 2;
                }
                direction = "Back";
                anim.SetInteger("Direction", 1);
                hitSomething(out hit, 1, false);
            }
        }

    }

    void pickNewEndDestination()
    {
        Vector2 endpos = new Vector2(Random.Range(minXWalkArea, maxXWalkArea), Random.Range(minYWalkArea, maxYWalkArea));
        endLocation = endpos - (Vector2)transform.position;
        endLocation.Normalize();

        int idleChance = Random.Range(1, 5);
        if(idleChance == 1)
        {
            StartCoroutine(startIdle(Random.Range(10, 20)));
        }
        StartCoroutine(destinationPickerCooldown(Random.Range(minWanderCooldown, maxWanderCooldown)));
    }
}
