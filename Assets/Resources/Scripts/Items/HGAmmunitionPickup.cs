using UnityEngine;
using System.Collections;

public class HGAmmunitionPickup : HGItem {
	protected AmmunitionType _ammoType;

	protected int _amount;

	public int amount {
		get { return _amount; }
		set { _amount = value; }
	}

	public AmmunitionType ammoType {
		get { return _ammoType; }
	}
	
	public HGAmmunitionPickup(AmmunitionType ammoType, int amount) : base("Ammo", ItemType.Ammunition) {
		_ammoType = ammoType;
		_amount = amount;
	}
}