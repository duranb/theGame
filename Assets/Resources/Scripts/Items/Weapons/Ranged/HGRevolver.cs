using UnityEngine;
using System.Collections;

public class HGRevolver : HGRangedWeapon {
	public HGRevolver(string revolverName, Vector3 attackOffset, float baseDamage, float baseEquipTime, float baseAttackRate, float baseReloadTime, int baseClipSize, float baseAccuracy = 1.0f, int currentClipAmmunitionCount = 0, float ammunitionVelocityModifier = 1.0f)
	: base(revolverName, RangedWeaponType.Revolver, attackOffset, AmmunitionType.Revolver, (GameObject)Resources.Load("Prefabs/Ammunitions/38mmBullet", typeof(GameObject)), Vector3.one, TrajectoryType.Straight, baseDamage, baseEquipTime, baseAttackRate, baseReloadTime, baseClipSize, baseAccuracy, currentClipAmmunitionCount, ammunitionVelocityModifier) {
	}

	public override float Shoot(Vector3 position, Vector3 direction, float attackDamageModifier, float attackSpeedModifier, float velocityModifier = 1.0f, float attackAccuracyModifier = 1.0f) {
		return base.Shoot(position, direction, attackDamageModifier, attackSpeedModifier, velocityModifier, attackAccuracyModifier);
	}
}