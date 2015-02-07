using UnityEngine;
using System.Collections;

public enum WeaponType
{
	Melee = 0,
	Gun = 1
}

public class Weapon : MonoBehaviour {
	#region events, properties and fields

	public string _weaponName;

	protected float _baseDamage;

	protected float _baseRate;

	protected WeaponType _weaponType;

	protected float _currentDamage;

	protected float _currentRate;

	#endregion

	#region public
	
	public virtual void Attack(float attackSpeedModifier) {
	}

	#endregion

	#region Monobehaviour

	public virtual void Start() {
		_currentDamage = _baseDamage;
		_currentRate = _baseRate;
	}

	public virtual void Update() {

	}

	#endregion
}
