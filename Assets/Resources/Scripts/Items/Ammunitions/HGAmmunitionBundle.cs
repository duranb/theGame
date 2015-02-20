using UnityEngine;
using System.Collections;

public class HGAmmunitionBundle : HGItem {
	protected AmmunitionType _ammunitionType;

	protected int _amount;

	public int amount {
		get { return _amount; }
		set { _amount = value; }
	}

	public AmmunitionType ammunitionType {
		get { return _ammunitionType; }
	}
	
	public HGAmmunitionBundle(AmmunitionType ammunitionType, int amount) 
	: base("Ammunition", ItemType.Ammunition) {
		_ammunitionType = ammunitionType;
		_amount = amount;
	}
}
