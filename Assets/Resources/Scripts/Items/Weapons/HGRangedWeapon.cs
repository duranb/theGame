using UnityEngine;
using System.Collections;

public class HGRangedWeapon : HGWeapon {
	protected GameObject _ammunitionPrefab;

	protected RangedWeaponType _rangedWeaponType;
	protected AmmunitionType _ammunitionType;

	protected float _baseReloadTime;
	protected int _baseClipSize;

	protected Vector3 _ammunitionScale;
	protected float _ammunitionVelocityModifier;
	protected TrajectoryType _ammunitionTrajectoryType;

	protected int _currentClipAmmunitionCount;

	public RangedWeaponType rangedWeaponType {
		get { return _rangedWeaponType; }
	}

	public AmmunitionType ammunitionType {
		get { return _ammunitionType; }
	}

	// Callback for when reloading is done
	public delegate void OnReloadDelegate(AmmunitionType type, int ammunitionLeft);
	public OnReloadDelegate OnReloadDone;

	/*
	 * weaponName - the name of the weapon
	 * rangedWeaponType - the type of the ranged weapon
	 * ammunitionType - the type of the ammunition
	 * ammunitionPrefab - the loaded prefab of the ammunition
	 * ammunitionScale - the scale of the ammunition model
	 * ammunitionTrajectoryType - the path type that the ammunition will follow
	 * baseDamage - the base damage of the weapon
	 * baseEquipTime - the base equipping time of the weapon
	 * baseAttackRate - the base attack rate of the weapon
	 * baseReloadTime - the base reload time of the ranged weapon
	 * baseClipSize - the base clip size of the ranged weapon
	 * baseAccuracy(optional) - the base accuracy of the weapon
	 * currentClipAmmunitionCount(optional) - the current ammunition count in the ranged weapon's clip
	 * ammunitionVelocityModifier(optional) - the modifier for the ammunitions velocity
	 *
	 * NOTE: a currentClipAmmunitionCount of -1 indicates infinite ammunition
	 */
	public HGRangedWeapon(string weaponName, RangedWeaponType rangedWeaponType, AmmunitionType ammunitionType, GameObject ammunitionPrefab, Vector3 ammunitionScale, TrajectoryType ammunitionTrajectoryType, float baseDamage, float baseEquipTime, float baseAttackRate, float baseReloadTime, int baseClipSize, float baseAccuracy = 1.0f, int currentClipAmmunitionCount = 0, float ammunitionVelocityModifier = 1.0f) 
	: base(weaponName, WeaponType.Ranged, baseDamage, baseEquipTime, baseAttackRate, baseAccuracy) {
		_rangedWeaponType = rangedWeaponType;
		_ammunitionType = ammunitionType;

		_ammunitionPrefab = ammunitionPrefab;
		_ammunitionScale = ammunitionScale;
		_ammunitionTrajectoryType = ammunitionTrajectoryType;

		_baseReloadTime = baseReloadTime;
		_baseClipSize = baseClipSize;

		_ammunitionVelocityModifier = ammunitionVelocityModifier;

		_currentClipAmmunitionCount = currentClipAmmunitionCount;
	}

	/*
	 * position - the position to fire the ammunition from
	 * direction - the direction to point the weapon at
	 * attackDamageModifier - the attribute from the character to affect damage
	 * attackRateModifier - the attribute from the character to affect rate of attack
	 * velocityModifier - the attribute from the character to affect the ammunition's velocity
	 * attackAccuracyModifier - the attribute from the character to affect the weapon's accuracy
	 */
	public virtual float Shoot(Vector3 position, Vector3 direction, float attackDamageModifier, float attackRateModifier, float velocityModifier = 1.0f, float attackAccuracyModifier = 1.0f) {
		float attackTime = 0;
		if((_currentClipAmmunitionCount > 0 || _currentClipAmmunitionCount == -1) && this._weaponState == WeaponState.Ready) {
			// Fire projectile
			GameObject ammunitionObject = (GameObject)MonoBehaviour.Instantiate(_ammunitionPrefab, position, Quaternion.identity);
			
			HGAmmunitionBehaviour ammunition = ammunitionObject.GetComponent<HGAmmunitionBehaviour>();

			float accuracy = this._baseAccuracy * attackAccuracyModifier;
			direction.y += Random.Range(-(1f - (1f * accuracy)), (1f - (1f * accuracy)));
			
			ammunitionObject.transform.localScale = Vector3.Scale(ammunitionObject.transform.localScale, _ammunitionScale);
			ammunition.Fire(this._baseDamage * attackDamageModifier, direction, _ammunitionTrajectoryType, _ammunitionVelocityModifier * velocityModifier);

			if(_currentClipAmmunitionCount > 0) {
				_currentClipAmmunitionCount--;
			}

			this.SetState(WeaponState.Attacking);
			attackTime = this._baseAttackRate * attackRateModifier;
		} else if(_currentClipAmmunitionCount == 0 && this._weaponState == WeaponState.Ready) {
			this.SetState(WeaponState.Empty);
		}

		return attackTime;
	}

	/*
	 * Used to throttle the firing rate
	 */ 
	public override void AttackDone() {
		base.AttackDone();
		if(this._weaponState == WeaponState.Attacking) {
			if(_currentClipAmmunitionCount == 0) {
				this.SetState(WeaponState.Empty);
			} else {
				this.SetState(WeaponState.Ready);
			}
		}
	}

	/*
	 * reloadTimeModifier - the attribute from the character to affect reload time
	 */
	public virtual float Reload(float reloadTimeModifier) {
		if(_currentClipAmmunitionCount == _baseClipSize) {
			return 0f;
		}

		if(this._weaponState == WeaponState.Ready || this._weaponState == WeaponState.Empty) {
			this.SetState(WeaponState.Reloading);


	        return _baseReloadTime * reloadTimeModifier;
	    }

	    return 0f;
	}

	/*
	 * Used to impose reload time
	 *
	 * currentAmmunitionCount - the count of ammunition of the type used by the ranged weapon
	 */
	public int ReloadDone(int currentAmmunitionCount) {
		if(this._weaponState == WeaponState.Reloading) {
			int clipDeficit = _baseClipSize - _currentClipAmmunitionCount;

			int ammunitionLeft = currentAmmunitionCount - clipDeficit;
			_currentClipAmmunitionCount += (ammunitionLeft > 0) ? clipDeficit : currentAmmunitionCount;
			currentAmmunitionCount = (ammunitionLeft > 0) ? ammunitionLeft : 0;
			this.SetState(WeaponState.Ready);
		}

		return currentAmmunitionCount;
	}

	/*
	 * Unequip the weapon
	 */
	public override void Unequip() {
		base.Unequip();
	}
}
