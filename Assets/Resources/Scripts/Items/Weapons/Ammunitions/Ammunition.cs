using UnityEngine;
using System.Collections;

public enum AmmunitionType
{
	Revolver = 0,
	Rifle = 1
}

public class Ammunition : MonoBehaviour {
	public float _lifeSpan;

	public void Fire() {
		Rigidbody rigidBody = this.GetComponent<Rigidbody>();
		rigidBody.velocity = this.transform.right * 100;

		StartCoroutine(Life());
	}

	private IEnumerator Life() {
		yield return new WaitForSeconds(_lifeSpan);
		Destroy(this.gameObject);
	}	
}
