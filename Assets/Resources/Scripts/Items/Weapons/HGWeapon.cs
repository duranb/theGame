using UnityEngine;
using System.Collections;
using System.Timers;

public enum WeaponType
{
	Melee,
	Gun
}

public enum WeaponState {
	Equipping,
	Attacking,
	Reloading,
	Ready,
	Empty,
	Unequipped,
	Broken
}

public class HGWeapon : HGItem {
	protected WeaponType _weaponType;

	protected WeaponState _weaponState = WeaponState.Unequipped;

	protected float _baseDamage;
	protected float _baseRate;

	protected float _baseEquipTime;

	protected Timer _attackTimer;
	protected Timer _equipTimer;

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

	public virtual WeaponState Attack(Vector3 position, Quaternion direction, float attackDamageModifier, float attackRateModifier) {
		return WeaponState.Ready;
	}

	/*
	 * weaponState - the new state of the weapon
	 */
	protected void SetState(WeaponState weaponState) {
		Debug.Log("New state: " + weaponState);
		_weaponState = weaponState;
	}


	public virtual void Equip() {
		SetState(WeaponState.Equipping);

        _equipTimer = new Timer(this._baseEquipTime);
        _equipTimer.Enabled = true;
		_equipTimer.AutoReset = false; //Stops it from repeating
        // Hook up the Elapsed event for the timer. 
        _equipTimer.Elapsed += delegate { SetState(WeaponState.Ready); };
	}

	public virtual void Unequip() {
		SetState(WeaponState.Unequipped);

		if(_attackTimer != null) {
			_attackTimer.Dispose();
		}
		if(_equipTimer != null) {
			_equipTimer.Dispose();
		}
	}
}
