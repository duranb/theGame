using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HGCharacterInventory {
	private HGItem[] _items;

	private List<HGAttributeModifier> _modifiers;

	private int[] _ammunitions = new int[(int)AmmunitionType.Length];
	private int[] _ammunitionCapacities = new int[(int)AmmunitionType.Length];

	// A temporary variable to store which index an item was chosen from
	private int _gotFromIndex = -1;

	#region equipment variables
	HGWeapon _equippedWeapon;

	#endregion

	public List<HGAttributeModifier> modifiers {
		get { return _modifiers; }
	}

	public HGWeapon equippedWeapon {
		get { return _equippedWeapon; }
	}

	/*
	 * size - the size of the inventory
	 */
	public HGCharacterInventory(int size) {
		_items = new HGItem[size];
		
		_modifiers = new List<HGAttributeModifier>();
	}

	/*
	 * Set a new capacity for the inventory
	 */
	public void SetCapacity(int newCapacity) {
		System.Array.Resize(ref _items, newCapacity);
	}

	/*
	 * Set a new capacity for an ammunition type
	 */
	public void SetAmmunitionCapacity(AmmunitionType ammunitionType, int newCapacity) {
		_ammunitionCapacities[(int)ammunitionType] = newCapacity;
	}

	/*
	 * Helper function to get the capacity for an ammunition type
	 */
	public int GetAmmunitionCapacity(AmmunitionType ammunitionType) {
		return _ammunitionCapacities[(int)ammunitionType];
	}

	/*
	 * Helper function to set a new amount of ammunition of a certain type
	 */
	public void SetAmmunitionCount(AmmunitionType ammunitionType, int newCount) {
		_ammunitions[(int)ammunitionType] = newCount;
	}

	/*
	 * Helper function to get a the amount of ammunition of a certain type
	 */
	public int GetAmmunitionCount(AmmunitionType ammunitionType) {
		return _ammunitions[(int)ammunitionType];
	}

	#region inventory interaction

	/*
	 * Attempts to add an item into the inventory
	 * item - the item
	 *
	 * returns true if successfully added, which should remove the item from the level
	 * returns false otherwise, which should leave it in play, but potentially modified
	 */
	public bool Add(HGItem item) {
		bool isAdded = false;
		switch(item.itemType) {
			case ItemType.AttributeModifier:
				_modifiers.Add((HGAttributeModifier)item);
				isAdded = true;
				break;
			case ItemType.Ammunition:
				HGAmmunitionBundle ammunitionPickup = (HGAmmunitionBundle)item;

				AmmunitionType ammunitionType = ammunitionPickup.ammunitionType;
				int ammunitionCapacity = GetAmmunitionCapacity(ammunitionType);
				int newAmmunitionAmount = GetAmmunitionCount(ammunitionType) + ammunitionPickup.amount;
				bool didOverflow = newAmmunitionAmount > ammunitionCapacity;

				// If the ammunitions overflowed we don't want to completely get rid of the item picked up
				isAdded = !didOverflow;
				if(didOverflow) {
					int overflowedBy = newAmmunitionAmount - ammunitionCapacity;

					// We set the picked up ammunition amount by the overflowed value
					ammunitionPickup.amount = overflowedBy;
				}

				newAmmunitionAmount = (didOverflow) ? ammunitionCapacity : newAmmunitionAmount;
				SetAmmunitionCount(ammunitionType, newAmmunitionAmount);
				break;
			default:
				int i = 0;
				while(i < _items.Length && !isAdded) {
					if(_items[i] == null) {
						_items[i] = item;

						isAdded = true;
					}
					i++;
				}
				break;
			
		}

		return isAdded;
	}

	/*
	 * Get the item at an index
	 * atIndex - the index to retrieve the item from
	 */
	public HGItem GetItem(int atIndex) {
		HGItem retrievedItem = null;
		if(_gotFromIndex != -1 && atIndex >= 0 && atIndex < _items.Length && _items[atIndex] != null) {
			retrievedItem = _items[atIndex];

			_items[atIndex] = null;
			
			// Keep track from where this item was retrieved in case they cancel
			_gotFromIndex = atIndex;
		}
		return retrievedItem;
	}

	/*
	 * Put an item in the provided index
	 * item - the HGItem to insert
	 * atIndex - the index to insert the item
	 *
	 * returns an item if an item was previously at the desired index 
	 * so that the user can choose to move it
	 */
	public HGItem SetItem(HGItem item, int atIndex) {
		HGItem replacedItem = null;
		if(atIndex >= 0 && atIndex < _items.Length) {
			_gotFromIndex = -1;

			replacedItem = GetItem(atIndex);
			
			_items[atIndex] = item;
		}

		return replacedItem;
	}

	/*
	 * Puts back an item retrieved but the user canceled the action
	 * If no index is found, then put the item in the first empty slot
	 */
	public void PutBackItem(HGItem item) {
		if(_gotFromIndex == -1) {
			Add(item);
		} else {
			SetItem(item, _gotFromIndex);
		}
	}

	/*
	 * Discards an item
	 */
	public void DropItem(HGItem itemToDrop) {
		// Spawn an Item to represent the dropped item
	}

	public void Sort(string by) {

	}

	#endregion

	#region equipment

	/*
	 * Equips the item at the selected index
	 */
	public float Equip(int atIndex) {
		// Item item = GetItem(atIndex); // Uncomment if we want to remove equipped items
		HGItem item = _items[atIndex];

		// Debug.Log("Equipping item:" + item.name);

		float equipTime = 0;
		if(item != null) {
			switch (item.itemType) {
				case ItemType.Weapon:
					equipTime = EquipWeapon((HGWeapon)item);
					break;
				default:
					break;
			}
		}

		return equipTime;
	}

	/*
	 * Equips a weapon 
	 */
	public float EquipWeapon(HGWeapon weapon) {
		if(weapon != null) {
			UnequipWeapon();

			_equippedWeapon = weapon;

			return _equippedWeapon.Equip();
		}

		return 0;
	}

	public void EquipDone() {
		if(_equippedWeapon != null) {
			_equippedWeapon.EquipDone();
		}
	}

	/*
	 * Unequips the item
	 */
	public void Unequip(HGItem item) {
		if(item != null) {
			// Add(item); // Uncomment if we want to add unequipped items
			Debug.Log("Unequipping item:" + item.name);
			if(item != null) {
				switch (item.itemType) {
					case ItemType.Weapon:
						UnequipWeapon();
						break;
					default:
						break;
				}
			}
		}
	}

	/*
	 * Unequips a weapon 
	 */
	public void UnequipWeapon() {
		if(_equippedWeapon != null) {
			_equippedWeapon.Unequip();

			switch (_equippedWeapon.weaponType) {
				case WeaponType.Ranged:
					break;
				default:
					break;
			}

			_equippedWeapon = null;
		}
	}

	#endregion

	#region ranged weapon logic

	/*
	 * Reloads the currently equipped ranged weapon if available
	 */
	public float Reload(float reloadTimeModifier) {
		if(_equippedWeapon != null && _equippedWeapon.weaponType == WeaponType.Ranged && _ammunitions[(int)((HGRangedWeapon)_equippedWeapon).ammunitionType] > 0) {
			HGRangedWeapon equippedRangedWeapon = (HGRangedWeapon)_equippedWeapon;

			return equippedRangedWeapon.Reload(reloadTimeModifier);
		}

		return 0f;
	}

	/*
	 * Callback for when the ranged weapon has finished reloading
	 */
    public void ReloadDone() {
    	if(_equippedWeapon != null) {
    		HGRangedWeapon equippedRangedWeapon = (HGRangedWeapon)_equippedWeapon;
    		AmmunitionType ammunitionType = equippedRangedWeapon.ammunitionType;
        	int newAmmunitionAmount = equippedRangedWeapon.ReloadDone(_ammunitions[(int)ammunitionType]);

        	SetAmmunitionCount(ammunitionType, newAmmunitionAmount);
        	
        	// Debug.Log("New ammunition: " + ammunitionType + " " + _ammunitions[(int)ammunitionType]);
        }

    }

    #endregion

	private static bool RemoveBurntModifiers(HGAttributeModifier modifier) {
		return modifier.duration <= 0;
	}

	public void BurnDownModifiers(float timeDelta) {
		_modifiers.RemoveAll(RemoveBurntModifiers);

		foreach(HGAttributeModifier modifier in _modifiers) {
			if(modifier.duration != Mathf.Infinity) {
				modifier.duration -= timeDelta;
			}
		}
	}
}
