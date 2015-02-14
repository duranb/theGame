using UnityEngine;
using System.Collections;

public class HGHandsArmor : HGArmor {
	public HGHandsArmor(string handsArmorName, float baseDefense, float baseDurability, float baseHealth) 
	: base(handsArmorName, ArmorType.Hands, baseDefense, baseDurability, baseHealth) {
	}
}