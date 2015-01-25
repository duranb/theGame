using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterInventory {
	private ArrayList _items;
	private List<AttributeModifier> _modifiers;

	private int _capacity = 1;

	public List<AttributeModifier> modifiers {
		get { return _modifiers; }
	}

	public void SetCapacity(int newCapacity) {
		_capacity = newCapacity;
	}

	public CharacterInventory(int size) {
		_capacity = size;
		_items = new ArrayList(_capacity);
		_modifiers = new List<AttributeModifier>();
	}

	public bool Add(Item item) {
		if(_items.Count < _capacity) {
			switch(item.type) {
				case ItemTypes.AttributeModifier:
					_modifiers.Add((AttributeModifier)item);
					break;
				default:
					_items.Add(item);			
					break;
				
			}

			return true;
		} else {
			return false;
		}
	}

	public void Sort(string by) {

	}

	private static bool RemoveBurntModifiers(AttributeModifier modifier) {
		return modifier.duration <= 0;
	}

	public void BurnDownModifiers(float timeDelta) {
		_modifiers.RemoveAll(RemoveBurntModifiers);

		foreach(AttributeModifier modifier in _modifiers) {
			if(modifier.duration != Mathf.Infinity) {
				modifier.duration -= timeDelta;
			}
		}
	}
}
