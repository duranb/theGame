using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Hero : Character {
	// Use this for initialization

	protected override void Start() {
		base.Start();
	}

	protected override void Update() {
		base.Update();

		Jump(Input.GetKeyDown("space"));
		Attack();
	}

	// Update is called once per frame
	protected override void FixedUpdate () {
		Move(Input.GetAxis("Horizontal"));
	}

	protected override void OnCollisionEnter(Collision collision) {
		base.OnCollisionEnter(collision);
	}

	protected override void OnTriggerEnter(Collider collider) {
		base.OnTriggerEnter(collider);

		this.Pickup(collider.gameObject);
	}
}