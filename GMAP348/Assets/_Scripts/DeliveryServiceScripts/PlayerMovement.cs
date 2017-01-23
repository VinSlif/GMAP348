using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public float speed = 15.0f;

	private Rigidbody rb_player;
	private Transform transform_player;

	private float verticalMove;
	private float horizontalMove;

	void Start () {
		rb_player = GetComponent<Rigidbody>();
		transform_player = this.gameObject.transform;
	}
	
	void Update () {

		verticalMove = 0f;
		horizontalMove = 0f;

		verticalMove = Input.GetAxis ("Vertical");
		horizontalMove = Input.GetAxis ("Horizontal");

		rb_player.velocity = new Vector3 (horizontalMove * speed, 0, verticalMove * speed);
		if (horizontalMove > 0) {
			if (verticalMove > 0) {
				transform_player.rotation = Quaternion.Euler(0, 45f, 0);
			} else if (verticalMove < 0) {
				transform_player.rotation = Quaternion.Euler(0, 135f, 0);
			} else {
				transform_player.rotation = Quaternion.Euler(0, 90f, 0);
			}
		} else if (horizontalMove < 0) {
			if (verticalMove > 0) {
				transform_player.rotation = Quaternion.Euler(0, -45f, 0);
			} else if (verticalMove < 0) {
				transform_player.rotation = Quaternion.Euler(0, -135f, 0);
			} else {
				transform_player.rotation = Quaternion.Euler(0, -90f, 0);
			}
		} else if (verticalMove > 0) {
			transform_player.rotation = Quaternion.Euler(0, 0, 0);
		} else if (verticalMove < 0) {
			transform_player.rotation = Quaternion.Euler(0, 180, 0);
		}
	}
}
