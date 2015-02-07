using UnityEngine;
using System.Collections;

public class Revolver : Gun {
	public override void Attack(float attackDamageModifier, float attackSpeedModifier) {
		base.Attack(attackDamageModifier, attackSpeedModifier);
	}

	public override void Start() {
		this._gunType = GunType.Revolver;

		this._baseDamage = 100f;
		this._baseReloadTime = 2f;
		this._baseRate = .5f;
		this._baseClipSize = 6;

		base.Start();
	}
}