using UnityEngine;
using System.Collections;

public class HGMeleeWeapon : HGWeapon {
	protected MeleeWeaponType _meleeWeaponType;

	public MeleeWeaponType meleeWeaponType {
		get { return _meleeWeaponType; }
	}

	/*
	 * weaponName - the name of the weapon
	 * meleeWeaponType - the type of the ranged weapon
	 * attackOffset - the offset to start an attack from
	 * baseDamage - the base damage of the weapon
	 * baseEquipTime - the base equipping time of the weapon
	 * baseAttackRate - the base attack rate of the weapon
	 * baseAccuracy(optional) - the base accuracy of the weapon
	 */
	public HGMeleeWeapon(string weaponName, MeleeWeaponType meleeWeaponType, Vector3 attackOffset, float baseDamage, float baseEquipTime, float baseAttackRate, float baseAccuracy = 1.0f) 
	: base(weaponName, WeaponType.Melee, attackOffset, baseDamage, baseEquipTime, baseAttackRate, baseAccuracy) {
		_meleeWeaponType = meleeWeaponType;
	}

	/*
	 * position - the position to swing the weapon from
	 * direction - the direction to point the weapon at
	 * attackDamageModifier - the attribute from the character to affect damage
	 * attackRateModifier - the attribute from the character to affect rate of attack
	 * attackAccuracyModifier - the attribute from the character to affect the weapon's accuracy
	 */
	public virtual float Swing(Vector3 position, Vector3 direction, float attackDamageModifier, float attackRateModifier, float velocityModifier = 1.0f, float attackAccuracyModifier = 1.0f) {
		float attackTime = 0;
		if(this._weaponState == WeaponState.Ready) {
			float accuracy = this._baseAccuracy * attackAccuracyModifier;
			direction.y += Random.Range(-(1f - (1f * accuracy)), (1f - (1f * accuracy)));
			
			// Swing the weapon

			this.SetState(WeaponState.Attacking);
			attackTime = this._baseAttackRate * attackRateModifier;
		}

		return attackTime;
	}

	/*
	 * Used to throttle the firing rate
	 */ 
	public override void AttackDone() {
		base.AttackDone();
		if(this._weaponState == WeaponState.Attacking) {
			this.SetState(WeaponState.Ready);
		}
	}

	/*
	 * Unequip the weapon
	 */
	public override void Unequip() {
		base.Unequip();
	}
}
