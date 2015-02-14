using UnityEngine;
using System.Collections;

public class HGLegsArmor : HGArmor {
	public HGLegsArmor(string legsArmorName, float baseDefense, float baseDurability, float baseHealth) 
	: base(legsArmorName, ArmorType.Legs, baseDefense, baseDurability, baseHealth) {
	}
}