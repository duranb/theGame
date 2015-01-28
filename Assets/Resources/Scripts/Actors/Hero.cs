using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Hero : MonoBehaviour{
	public float speed = 6.0F;
    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;

	private Vector3 moveDirection = Vector3.zero;

    void Update() {
       CharacterController controller = GetComponent<CharacterController>();

        if (controller.isGrounded) {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;
            if (Input.GetButton("Jump"))
                moveDirection.y = jumpSpeed;
            
        }

        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
        Debug.Log(moveDirection);
    }

	// Use this for initialization

	// protected override void Start() {
	// 	base.Start();
	// }

	// protected override void Update() {
	// 	base.Update();

	// 	Jump(Input.GetKeyDown("space"));
	// 	Attack();
	// }

	// // Update is called once per frame
	// protected override void FixedUpdate () {
	// 	Run(Input.GetAxis("Horizontal"));
	// }

	// protected override void OnCollisionEnter(Collision collision) {
	// 	base.OnCollisionEnter(collision);
	// }

	// protected override void OnTriggerEnter(Collider collider) {
	// 	base.OnTriggerEnter(collider);

	// 	this.Pickup(collider.gameObject);
	// }
}