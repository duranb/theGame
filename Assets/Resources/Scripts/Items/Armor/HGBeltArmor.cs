using UnityEngine;
using System.Collections;

public class HGBeltArmor : HGArmor {
	public HGBeltArmor(string beltArmorName, float baseDefense, float baseDurability, float baseHealth) 
	: base(beltArmorName, ArmorType.Belt, baseDefense, baseDurability, baseHealth) {
	}
}