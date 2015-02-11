using UnityEngine;
using System.Collections;
using System.Timers;

public class HGWeapon : HGItem {
	protected WeaponType _weaponType;

	protected WeaponState _weaponState = WeaponState.Unequipped;

	protected float _baseDamage;
	protected float _baseRate;

	protected float _baseEquipTime;

	public WeaponType weaponType {
		get { return _weaponType; }
	}

	public WeaponState weaponState {
		get { return _weaponState; }
	}
	
	public HGWeapon(string weaponName, WeaponType weaponType, float baseDamage, float baseEquipTime, float baseRate) : base(weaponName, ItemType.Weapon) {
		_weaponType = weaponType;

		_baseDamage = baseDamage;
		_baseRate = baseRate;

		_baseEquipTime = baseEquipTime;
	}

	public virtual float Attack(Vector3 position, Quaternion direction, float attackDamageModifier, float attackRateModifier) {
		return _baseRate * attackRateModifier;
	}

	public virtual void AttackDone() {

	}

	/*
	 * weaponState - the new state of the weapon
	 */
	protected void SetState(WeaponState weaponState) {
		Debug.Log("New state: " + weaponState);
		_weaponState = weaponState;
	}


	public virtual float Equip() {
		SetState(WeaponState.Equipping);

		return _baseEquipTime;
	}

	public void EquipDone() {
		SetState(WeaponState.Ready);
	}

	public virtual void Unequip() {
		SetState(WeaponState.Unequipped);
	}
}
