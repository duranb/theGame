using UnityEngine;
using System.Collections;

public enum ItemTypes
{
	AttributeModifier,
	Weapon,
	Ammunition
}

public class Item : MonoBehaviour {
	public ItemTypes _type;
	public string _itemName;
	public float _duration = Mathf.Infinity;

	public ItemTypes type {
		get { return _type; }
		set { _type = value; }
	}

	public float duration {
		get { return _duration; }
		set { _duration = value; }
	}

	void Start() {
		this.gameObject.tag = "Item";
	}
}