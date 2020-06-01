using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private float playerSpeed = 2f;
    private Rigidbody2D rbPlayer;
    private Animator animPlayer;

	// Use this for initialization
	void Start () {
        rbPlayer = this.GetComponent<Rigidbody2D>();
        animPlayer = this.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        MovePlayer();     
	}

    void MovePlayer()
    {
        var hor = Input.GetAxis("Horizontal");
        var ver = Input.GetAxis("Vertical");

        /*Vector3 inputVector = new Vector3(hor, 0, ver);
        inputVector.Normalize();*/

        rbPlayer.velocity = new Vector2(hor * playerSpeed, ver * playerSpeed);

        animPlayer.SetFloat("horizontalSpeed", hor);
        animPlayer.SetFloat("verticalSpeed", ver);
    }

}
