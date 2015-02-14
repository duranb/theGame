using UnityEngine;
using System.Collections;

public class HGArmsArmor : HGArmor {
	public HGArmsArmor(string armsArmorName, float baseDefense, float baseDurability, float baseHealth) 
	: base(armsArmorName, ArmorType.Arms, baseDefense, baseDurability, baseHealth) {
	}
}