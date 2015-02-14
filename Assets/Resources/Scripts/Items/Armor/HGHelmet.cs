using UnityEngine;
using System.Collections;

public class HGHeadArmor : HGArmor {
	public HGHeadArmor(string headArmorName, float baseDefense, float baseDurability, float baseHealth) 
	: base(headArmorName, ArmorType.Head, baseDefense, baseDurability, baseHealth) {
	}
}