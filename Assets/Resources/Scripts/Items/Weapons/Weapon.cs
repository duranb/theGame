using UnityEngine;
using System.Collections;

public enum WeaponType
{
	Melee,
	Gun
}

public class Weapon : MonoBehaviour {

	public string _weaponName;

	protected WeaponType _weaponType;

	protected float _baseDamage;
	protected float _baseRate;
	
	public virtual void Attack(float attackDamageModifier, float attackRateModifier) {
	}

	public virtual void Start() {
	}

	public virtual void Update() {
	}
}
