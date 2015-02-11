using UnityEngine;
using System.Collections;

public class HGItem {
	public ItemType _itemType;
	public string _name;
	public float _duration = Mathf.Infinity;

	public string name {
		get { return _name; }
	}

	public ItemType itemType {
		get { return _itemType; }
		set { _itemType = value; }
	}

	public float duration {
		get { return _duration; }
		set { _duration = value; }
	}

	public HGItem(string name, ItemType itemType) {
		_name = name;
		_itemType = itemType;
	}

	// void Start() {
	// 	this.gameObject.tag = "Item";
	// }
}