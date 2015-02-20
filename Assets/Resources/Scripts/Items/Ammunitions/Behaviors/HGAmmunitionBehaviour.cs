using UnityEngine;
using System.Collections;

public class HGAmmunitionBehaviour : MonoBehaviour {
	protected AmmunitionType _ammunitionType;

	public float _lifeSpan;
	public float _baseVelocity;

	private float _damage;
	private float _velocity;

	private Vector3 _direction;
	private TrajectoryType _trajectoryType;

	public float damage {
		get { return _damage; }
	}

	/*
	 * damage - the amount of damage to deal to whatever it hits
	 * direction - the general direction to travel in
	 * trajectoryType - the type of trajectory to follow
	 * velocityModifier - the modifier to affect the velocity
	 */
	public virtual void Fire(float damage, Vector3 direction, TrajectoryType trajectoryType, float velocityModifier) {
		_damage = damage;
		_direction = direction;
		_velocity = _baseVelocity * velocityModifier;
		_trajectoryType = trajectoryType;

		StartCoroutine(Life());
	}

	private IEnumerator Life() {
		yield return new WaitForSeconds(_lifeSpan);
		Expire();
	}

	private void Expire() {
		Destroy(this.gameObject);
	}

	private void FixedUpdate() {
		// DO TRAJECTORY MODIFYING HERE
		switch(_trajectoryType) {
			case TrajectoryType.Arch:
				break;
			case TrajectoryType.Straight:
				this.transform.LookAt(this.transform.position + _direction);

				Rigidbody2D rigidBody = this.GetComponent<Rigidbody2D>();
				rigidBody.velocity = this.transform.forward * _velocity;
				break;
			case TrajectoryType.Wave:
				break;
		}
	}

	private void OnTriggerEnter2D(Collider2D collider) {
		Expire();
	}
}
