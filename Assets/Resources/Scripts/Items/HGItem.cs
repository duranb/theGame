using UnityEngine;
using System.Collections;

public class HGItem {
	protected ItemType _itemType;
	protected string _name;
	protected float _duration = Mathf.Infinity;

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

	public HGItem(string name, ItemType itemType, float duration = Mathf.Infinity) {
		_name = name;
		_itemType = itemType;
		_duration = duration;
	}
}