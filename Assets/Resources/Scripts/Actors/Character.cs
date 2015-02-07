using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Character : MonoBehaviour {
	public float maxHealth = 3;
	public float baseSpeed = 10;
	public float baseJumpHeight = 5;

	protected CharacterInventory _inventory = new CharacterInventory(1);

	protected Rigidbody _body;

	protected float _currentHealth;

	protected float _speed;
	protected float _jumpHeight;

	[HideInInspector]public bool _grounded;

	// Use this for initialization
	protected virtual void Start () {
		this.gameObject.tag = "Character";

		_body = this.GetComponent<Rigidbody>();

		_currentHealth = maxHealth;

		_speed = baseSpeed;
		_jumpHeight = baseJumpHeight;
	}

	protected virtual void Update() {
		_speed = baseSpeed;
		_jumpHeight = baseJumpHeight;

		List<AttributeModifier> modifiers = _inventory.modifiers;

		_inventory.BurnDownModifiers(Time.deltaTime);

		// Apply modifiers
		foreach(AttributeModifier modifier in modifiers) {
			switch(modifier._modifierType) {
				case AttributeModifierTypes.Speed:
					_speed = modifier.Apply(baseSpeed);
					break;
				case AttributeModifierTypes.Jump:
					_jumpHeight = modifier.Apply(baseJumpHeight);
					break;
			}
		}
	}

	protected virtual void FixedUpdate() {

	}

	protected virtual void OnCollisionEnter(Collision collision) {
		if(collision.gameObject.tag == "Platform") {
			_grounded = true;
		}
	}

	protected virtual void OnTriggerEnter(Collider collider) {

	}

	public virtual void Run(float move) {
		Vector2 newVelocity = new Vector3(move * _speed, _body.velocity.y, _body.velocity.z);
		
	    _body.velocity = newVelocity;
	}

	public virtual void Jump(bool jump) {
		if(jump &&  _grounded) {
			Vector2 newVelocity = new Vector3(_body.velocity.x, _jumpHeight, _body.velocity.z);
			_body.velocity = newVelocity;

			_grounded = false;
		}
	}

	public virtual void Attack() {

	}

	public virtual void Pickup(GameObject itemGameObject) {
		if(itemGameObject.tag == "Item") {
			Item item = itemGameObject.GetComponent<Item>();
			if(_inventory.Add(item)) {
				Destroy(itemGameObject);
			}
		}
	}

	public virtual void TakeDamage(float damage) {
		_currentHealth -= damage;
	}
}
