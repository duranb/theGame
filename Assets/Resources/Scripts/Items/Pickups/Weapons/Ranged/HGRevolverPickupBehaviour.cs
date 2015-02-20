using UnityEngine;
using System.Collections;

public class HGRevolverPickupBehaviour : HGItemPickupBehaviour {
	public float baseDamage;
	public float baseEquipTime;
	public float baseRate;
	public float baseReloadTime;
	public float baseAccuracy = 1.0f;
	public int baseClipSize;
	public int currentClipAmmoCount;

	private HGRevolver _revolver;

	public override void Start() {
		_revolver = new HGRevolver(name, baseDamage, baseEquipTime, baseRate, baseReloadTime, baseClipSize, baseAccuracy, currentClipAmmoCount, 1.0f);
	}

	public override HGItem GetItem() {
		return _revolver;
	}
}