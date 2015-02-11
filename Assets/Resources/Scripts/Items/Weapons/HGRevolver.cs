using UnityEngine;
using System.Collections;

public class HGRevolver : HGGun {
	public HGRevolver(string revolverName, float baseDamage, float baseEquipTime, float baseRate, float baseReloadTime, int baseClipSize) : base(revolverName, GunType.Revolver, AmmunitionType.Revolver, (GameObject)Resources.Load("Prefabs/38mmBullet", typeof(GameObject)), baseDamage, baseEquipTime, baseRate, baseReloadTime, baseClipSize) {
	}

	public override WeaponState Attack(Vector3 position, Quaternion direction, float attackDamageModifier, float attackSpeedModifier) {
		return base.Attack(position, direction, attackDamageModifier, attackSpeedModifier);
	}
}