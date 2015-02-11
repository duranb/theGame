using UnityEngine;
using System.Collections;
using System.Timers;

public class HGGun : HGWeapon {
	protected GameObject _ammunitionPrefab;

	protected GunType _gunType;
	protected AmmunitionType _ammunitionType;

	protected float _baseReloadTime;
	protected int _baseClipSize;

	protected int _currentClipAmmoCount;

	// protected Timer _reloadTimer;

	// private bool _isFiring = false;
	// private bool _isReloading = false;
	public GunType gunType {
		get { return _gunType; }
	}

	public AmmunitionType ammunitionType {
		get { return _ammunitionType; }
	}

	// Callback for when reloading is done
	public delegate void OnReloadDelegate(AmmunitionType type, int ammoLeft);
	public OnReloadDelegate OnReloadDone;

	public HGGun(string weaponName, GunType gunType, AmmunitionType ammunitionType, GameObject ammunitionPrefab, float baseDamage, float baseEquipTime, float baseRate, float baseReloadTime, int baseClipSize) : base(weaponName, WeaponType.Gun, baseDamage, baseEquipTime, baseRate) {
		_gunType = gunType;
		_ammunitionType = ammunitionType;

		_ammunitionPrefab = ammunitionPrefab;

		_baseReloadTime = baseReloadTime;
		_baseClipSize = baseClipSize;
	}

	/*
	 * position - the position to fire the bullet from
	 * direction - the direction to fire the bullet at
	 * attackDamageModifier - the attribute from the character to affect damage
	 * attackRateModifier - the attribute from the character to affect rate of fire
	 */
	public override WeaponState Attack(Vector3 position, Quaternion direction, float attackDamageModifier, float attackRateModifier) {
		if(_currentClipAmmoCount > 0 && _weaponState == WeaponState.Ready/*!_isFiring && !_isReloading*/) {
			// Fire projectile
			GameObject ammoObject = (GameObject)MonoBehaviour.Instantiate(_ammunitionPrefab, position, direction);
			HGAmmunitionBehaviour ammo = ammoObject.GetComponent<HGAmmunitionBehaviour>();

			ammo.Fire(this._baseDamage * attackDamageModifier);

			_currentClipAmmoCount--;

			// _isFiring = true;
			SetState(WeaponState.Attacking);

	        _attackTimer = new Timer(this._baseRate * attackRateModifier);
	        _attackTimer.Enabled = true;
    		_attackTimer.AutoReset = false; //Stops it from repeating
	        // Hook up the Elapsed event for the timer. 
	        _attackTimer.Elapsed += delegate { AttackDone(); };
		} else if(_currentClipAmmoCount == 0 && _weaponState == WeaponState.Ready) {
			// OnEmptyReload();
			SetState(WeaponState.Empty);
		}

		return _weaponState;
	}

	/*
	 * Used to throttle the firing rate
	 */ 
	public override void AttackDone() {
		base.AttackDone();
		if(_weaponState == WeaponState.Attacking) {
			// _isFiring = false;
			if(_currentClipAmmoCount == 0) {
				SetState(WeaponState.Empty);
			} else {
				SetState(WeaponState.Ready);
			}
		}
	}

	/*
	 * currentAmmoCount - the count of ammo of the type used by the gun
	 * reloadTimeModifier - the attribute from the character to affect reload time
	 */
	public virtual float Reload(float reloadTimeModifier) {
		if(_currentClipAmmoCount == _baseClipSize) {
			return 0f;
		}

		if(_weaponState == WeaponState.Ready || _weaponState == WeaponState.Empty) {
			SetState(WeaponState.Reloading);


	        return _baseReloadTime * reloadTimeModifier;
	    }

	    return 0f;
	}

	/*
	 * Used to impose reload time
	 *
	 * currentAmmoCount - the count of ammo of the type used by the gun
	 */
	public int ReloadDone(int currentAmmoCount) {
		if(_weaponState == WeaponState.Reloading) {
			int clipDeficit = _baseClipSize - _currentClipAmmoCount;

			int ammoLeft = currentAmmoCount - clipDeficit;
			_currentClipAmmoCount += (ammoLeft > 0) ? clipDeficit : currentAmmoCount;
			currentAmmoCount = (ammoLeft > 0) ? ammoLeft : 0;
			SetState(WeaponState.Ready);
		}

		return currentAmmoCount;
	}

	public override void Unequip() {
		base.Unequip();

		// if(_reloadTimer != null) {
		// 	_reloadTimer.Dispose();
		// }
	}
}
