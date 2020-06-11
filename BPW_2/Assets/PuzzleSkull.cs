using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSkull : MonoBehaviour
{
    private bool attachToPlayer = false;
    private Transform player;
    private Rigidbody2D playerRB;
    private Rigidbody2D body;
    private float speed = 10f;
    private float minDistance = 1.5f;

    private bool canFollowPlayer = false;

    private bool canPush = false;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerRB = player.GetComponent<Rigidbody2D>();
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(canPush && Input.GetKeyDown(KeyCode.E))
        {
            pushSkull();
        }
        /*if (canFollowPlayer && Input.GetKeyDown(KeyCode.E))
        {
                if (attachToPlayer)
                {
                    attachToPlayer = false;
                }
                else
                {
                    attachToPlayer = true;
                }
                Debug.Log(attachToPlayer);
        }

        if (attachToPlayer)
        {
            if(Vector3.Distance(body.position, playerRB.position) > minDistance)
            {
                Vector2 targetDirection = (playerRB.position - body.position).normalized;
                body.velocity = targetDirection * speed;
            }
        }*/
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            canPush = true;
            //canFollowPlayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            canPush = false;
            //canFollowPlayer = false;
            //attachToPlayer = false;
        }
    }

    private void pushSkull()
    {
        float horizontalHit = (transform.position.x - player.position.x);
        float verticalHit = (transform.position.y - player.position.y);

        Vector2 fixedPos = new Vector2(horizontalHit, verticalHit);

        //Debug.Log(Mathf.Abs(fixedPos.x) + "___" + Mathf.Abs(fixedPos.y));

        if(Mathf.Abs(fixedPos.x) > Mathf.Abs(fixedPos.y)) 
        {
            if (horizontalHit < 0)
            {
                body.AddForce(new Vector2(-40000, 0));
                Debug.Log("hit right");
                
            }
            else if (horizontalHit > 0)
            {
                body.AddForce(new Vector2(40000, 0));
                Debug.Log("hit left");
            }
        }
        else
        { 
            if (verticalHit < 0)
            {
                body.AddForce(new Vector2(0, -40000));
                Debug.Log("hit above");
            }
            else if (verticalHit > 0)
            {
                body.AddForce(new Vector2(0, 40000));
                Debug.Log("hit Bototm");
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        /*if(collision.gameObject.tag == "Player")
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (attachToPlayer)
                {
                    attachToPlayer = false;
                }
                else
                {
                    attachToPlayer = true;
                }
                Debug.Log(attachToPlayer);
            }
        }*/
    }
}
