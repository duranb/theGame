using UnityEngine;
using System.Collections;

public class HGWeapon : HGItem {
	protected WeaponType _weaponType;

	protected WeaponState _weaponState = WeaponState.Unequipped;

	protected float _baseDamage;
	protected float _baseAttackRate;

	protected float _baseEquipTime;

	protected float _baseAccuracy;

	public WeaponType weaponType {
		get { return _weaponType; }
	}

	public WeaponState weaponState {
		get { return _weaponState; }
	}

	/*
	 * weaponName - the name of the weapon
	 * weaponType - the type of the weapon
	 * baseDamage - the base damage of the weapon
	 * baseEquipTime - the base equipping time of the weapon
	 * baseAttackRate - the base attack rate of the weapon
	 * baseAccuracy(optional) - the base accuracy of the weapon
	 */
	public HGWeapon(string weaponName, WeaponType weaponType, float baseDamage, float baseEquipTime, float baseAttackRate, float baseAccuracy = 1.0f) 
	: base(weaponName, ItemType.Weapon) {
		_weaponType = weaponType;

		_baseDamage = baseDamage;
		_baseAttackRate = baseAttackRate;

		_baseEquipTime = baseEquipTime;

		_baseAccuracy = baseAccuracy;
	}

	/*
	 * position - the position to fire the bullet from
	 * direction - the direction to point the weapon at
	 * attackDamageModifier - the attribute from the character to affect damage
	 * attackRateModifier - the attribute from the character to affect rate of attack
	 * attackAccuracyModifier - the attribute from the character to affect the weapon's accuracy
	 */
	public virtual float Attack(Vector3 position, Vector3 direction, float attackDamageModifier, float attackRateModifier, float attackAccuracyModifer = 1.0f) {
		return _baseAttackRate * attackRateModifier;
	}

	/*
	 * Callback for when the weapon has finished it's current attack
	 */
	public virtual void AttackDone() {}

	/*
	 * weaponState - the new state of the weapon
	 */
	protected void SetState(WeaponState weaponState) {
		// Debug.Log("New state: " + weaponState);
		_weaponState = weaponState;
	}


	/*
	 * Equip the weapon
	 */
	public virtual float Equip() {
		SetState(WeaponState.Equipping);

		return _baseEquipTime;
	}

	/*
	 * Callback for when the weapon has been equipped
	 * Sets the weapon's state to ready
	 */
	public void EquipDone() {
		SetState(WeaponState.Ready);
	}

	/*
	 * Unequip the weapon
	 */
	public virtual void Unequip() {
		SetState(WeaponState.Unequipped);
	}
}
