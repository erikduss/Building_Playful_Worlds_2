using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private float playerSpeed = 2.5f;
    private Rigidbody2D rbPlayer;
    private Animator animPlayer;

    private string facingDirection = "Front";

	// Use this for initialization
	void Start () {
        rbPlayer = this.GetComponent<Rigidbody2D>();
        animPlayer = this.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        MovePlayer();

        if (Input.GetKeyDown(KeyCode.Space))
        {

        }

	}

    void MovePlayer()
    {
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");

        if(Mathf.Abs(hor) > Mathf.Abs(ver))
        {
            if(hor < 0)
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



        rbPlayer.velocity = new Vector2(hor * playerSpeed, ver * playerSpeed);

        animPlayer.SetFloat("horizontalSpeed", hor);
        animPlayer.SetFloat("verticalSpeed", ver);
    }

}
