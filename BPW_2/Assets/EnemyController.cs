using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private bool isBoss = false;

    private float minXWalkArea = 2.56f;
    private float maxXWalkArea = 16.64f;

    private float minYWalkArea = -16.64f;
    private float maxYWalkArea = -1.28f;

    float moveSpeed = 2.5f;

    private Rigidbody2D rBody;

    private Vector3 enemyRoomPosition;

    private GameObject player;

    private Vector2 endLocation = new Vector2(50,-40);

    private int minWanderCooldown = 3;
    private int maxWanderCooldown = 12;

    private bool chasePlayer = false;

    private Animator anim;

    private bool wandering = true;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
        if(this.tag == "Boss")
        {
            isBoss = true;

            //double the walking area
            maxXWalkArea = (maxXWalkArea * 2);
            maxYWalkArea = (maxYWalkArea * 2);
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

    // Update is called once per frame
    void Update()
    {
        if (chasePlayer)
        {
            if (Vector3.Distance(player.transform.position, this.transform.position) < 100)
            {
                Vector3 direction = player.transform.position - this.transform.position;

                direction.Normalize();
                endLocation = direction;
            }
        }
    }

    private void FixedUpdate()
    {
        if (wandering)
        {
            moveEnemy();
        }
        
        if(transform.position.x >= 72.81f || transform.position.y <= -61.1f || transform.position.x < 41.19f || transform.position.y >= -33.7f)
        {
            endLocation = new Vector2(0, 0);
            anim.SetFloat("horizontalSpeed", 0);
            anim.SetFloat("verticalSpeed", 0);
        }
    }

    private IEnumerator destinationPickerCooldown(int cooldown)
    {
        yield return new WaitForSeconds(cooldown);
        pickNewEndDestination();
    }

    private IEnumerator startIdle(int idleTime)
    {
        anim.SetFloat("horizontalSpeed", 0);
        anim.SetFloat("verticalSpeed", 0);
        wandering = false;
        yield return new WaitForSeconds(idleTime);
        wandering = true;
        pickNewEndDestination();
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

        if (Mathf.Abs(endLocation.x) > Mathf.Abs(endLocation.y))
        {
            if (endLocation.x < 0)
            {
                anim.SetInteger("Direction", 4);
            }
            else
            {
                anim.SetInteger("Direction", 2);
            }
        }
        else
        {
            if (endLocation.y < 0)
            {
                anim.SetInteger("Direction", 1);
            }
            else
            {
                anim.SetInteger("Direction", 3);
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
