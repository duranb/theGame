using UnityEngine;
using System.Collections;

public class HGBodyArmor : HGArmor {
	public HGBodyArmor(string bodyArmorName, float baseDefense, float baseDurability, float baseHealth) 
	: base(bodyArmorName, ArmorType.Body, baseDefense, baseDurability, baseHealth) {
	}
}