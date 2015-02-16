using UnityEngine;
using System.Collections;

public class HGAmmunitionPickupBehaviour : HGItemPickupBehaviour {
	public AmmunitionType ammunitionType;

	public int amount;

	private HGAmmunitionBundle _ammoBundle;

	public override void Start() {
		_ammoBundle = new HGAmmunitionBundle(ammunitionType, amount);
	}

	public override HGItem GetItem() {
		return _ammoBundle;
	}
}
