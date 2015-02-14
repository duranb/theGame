using UnityEngine;
using System.Collections;
using System.Timers;

public class HGArmor : HGItem {
	protected ArmorType _armorType;

	protected float _baseDefense;

	public ArmorType armorType {
		get { return _armorType; }
	}

	public HGArmor(string armorName, ArmorType armorType, float baseDefense, float baseDurability, float baseHealth) : base(armorName, ItemType.Armor) {
		_armorType = armorType;

		_baseDefense = baseDefense;
	}

	public virtual float Defend(float incomingDamage, float defenseModifier) {
		return incomingDamage - (_baseDefense * defenseModifier);
	}
}