using UnityEngine;
using System.Collections;

public enum AmmunitionType
{
	Revolver,
	Rifle
}

public class Ammunition : MonoBehaviour {
	protected AmmunitionType _ammunitionType;

	public float _lifeSpan;
	public float _velocity;

	public void Fire(float damage) {
		Rigidbody rigidBody = this.GetComponent<Rigidbody>();
		rigidBody.velocity = this.transform.right * _velocity;

		StartCoroutine(Life());
	}

	private void Expire() {
		Destroy(this.gameObject);
	}

	private IEnumerator Life() {
		yield return new WaitForSeconds(_lifeSpan);
		Expire();
	}

	private void OnCollisionEnter2D(Collision2D collision) {
		Expire();
	}
}
