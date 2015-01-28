using UnityEngine;
using System.Collections;

public enum ItemTypes
{
	AttributeModifier = 0,
	Weapon = 1
}

public class Item : MonoBehaviour {
	public ItemTypes type;
	public string itemName;
	public float duration = Mathf.Infinity;
	public float value;

	void Start() {
		this.gameObject.tag = "Item";
	}
}