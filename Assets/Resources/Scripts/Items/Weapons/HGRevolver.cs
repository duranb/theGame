using UnityEngine;
using System.Collections;

public class HGRevolver : HGGun {
	public HGRevolver(string revolverName, float baseDamage, float baseEquipTime, float baseRate, float baseReloadTime, int baseClipSize, int currentClipAmmoCount) 
	: base(revolverName, GunType.Revolver, AmmunitionType.Revolver, (GameObject)Resources.Load("Prefabs/Ammunitions/38mmBullet", typeof(GameObject)), baseDamage, baseEquipTime, baseRate, baseReloadTime, baseClipSize, currentClipAmmoCount) {
	}

	public override float Attack(Vector3 position, Quaternion direction, float attackDamageModifier, float attackSpeedModifier) {
		return base.Attack(position, direction, attackDamageModifier, attackSpeedModifier);
	}
}