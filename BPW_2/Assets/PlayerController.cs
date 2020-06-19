using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private float playerSpeed = 2.5f;
    private Rigidbody2D rbPlayer;
    private Animator animPlayer;

    private string facingDirection = "Front";

    private bool freezePlayer = false;

	// Use this for initialization
	void Start () {
        rbPlayer = this.GetComponent<Rigidbody2D>();
        animPlayer = this.GetComponent<Animator>();
	}
	
    void getDirection()
    {
        Debug.Log(facingDirection);
    }

	// Update is called once per frame
	void FixedUpdate () {
        if (!freezePlayer)
        {
            MovePlayer();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rbPlayer.velocity = new Vector2(0,0);
            animPlayer.SetFloat("horizontalSpeed", 0);
            animPlayer.SetFloat("verticalSpeed", 0);

            switch (facingDirection)
            {
                case "Front":
                    animPlayer.Play("front_slashing");
                    break;
                case "Back":
                    animPlayer.Play("back_slashing");
                    break;
                case "Left":
                    animPlayer.Play("left_slashing");
                    break;
                case "Right":
                    animPlayer.Play("right_slashing");
                    break;
            }
            StartCoroutine(freezePlayerMovement(1));
        }

	}

    private IEnumerator freezePlayerMovement(int time)
    {
        freezePlayer = true;
        yield return new WaitForSeconds(time);
        freezePlayer = false;
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
