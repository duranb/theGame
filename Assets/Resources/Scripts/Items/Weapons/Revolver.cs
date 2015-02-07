using UnityEngine;
using System.Collections;

public class Revolver : Gun {
	public override void Attack(float attackSpeedModifier) {
		base.Attack(attackSpeedModifier);
	}

	// public Revolver(string name, float damage, float rate) : base(name, damage, rate) {
	// 	this._gunType = GunType.Revolver;
	// }

	public override void Start() {
		this._gunType = GunType.Revolver;

		this._baseDamage = 100f;
		this._baseReloadTime = 2f;
		this._baseRate = .5f;
		this._baseClipSize = 6;

		base.Start();
	}
}