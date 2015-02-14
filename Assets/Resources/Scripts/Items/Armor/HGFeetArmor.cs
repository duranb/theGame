using UnityEngine;
using System.Collections;

public class HGFeetArmor : HGArmor {
	public HGFeetArmor(string feetArmorName, float baseDefense, float baseDurability, float baseHealth) 
	: base(feetArmorName, ArmorType.Feet, baseDefense, baseDurability, baseHealth) {
	}
}