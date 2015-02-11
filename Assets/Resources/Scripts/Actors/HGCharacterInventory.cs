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
	public void SetAmmoCapacity(AmmunitionType ammoType, int newCapacity) {
		_ammunitionCapacities[(int)ammoType] = newCapacity;
	}

	/*
	 * Helper function to get the capacity for an ammunition type
	 */
	public int GetAmmoCapacity(AmmunitionType ammoType) {
		return _ammunitionCapacities[(int)ammoType];
	}

	/*
	 * Helper function to set a new amount of ammunition of a certain type
	 */
	public void SetAmmoCount(AmmunitionType ammoType, int newCount) {
		_ammunitions[(int)ammoType] = newCount;
	}

	/*
	 * Helper function to get a the amount of ammunition of a certain type
	 */
	public int GetAmmoCount(AmmunitionType ammoType) {
		return _ammunitions[(int)ammoType];
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
				HGAmmunitionPickup ammoPickup = (HGAmmunitionPickup)item;

				// int ammoTypeIndex = (int)ammoPickup.ammoType;
				AmmunitionType ammoType = ammoPickup.ammoType;
				int ammoCapacity = GetAmmoCapacity(ammoType);
				int newAmmoAmount = GetAmmoCount(ammoType) + ammoPickup.amount;
				// _ammunitions[ammoTypeIndex] += ammoPickup.amount;

				// bool didOverflow = _ammunitions[ammoTypeIndex] > _ammunitionCapacities[ammoTypeIndex];
				bool didOverflow = newAmmoAmount > ammoCapacity;

				// If the ammunitions overflowed we don't want to completely get rid of the item picked up
				isAdded = !didOverflow;
				if(didOverflow) {
					int overflowedBy = newAmmoAmount - ammoCapacity;
					// int overflowedBy = _ammunitions[ammoTypeIndex] - _ammunitionCapacities[ammoTypeIndex];

					// We set the picked up ammo amount by the overflowed value
					ammoPickup.amount = overflowedBy;
				}

				// _ammunitions[ammoTypeIndex] = (didOverflow) ? _ammunitionCapacities[ammoTypeIndex] : _ammunitions[ammoTypeIndex];				
				newAmmoAmount = (didOverflow) ? ammoCapacity : newAmmoAmount;
				SetAmmoCount(ammoType, newAmmoAmount);
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
	public void Equip(int atIndex) {
		// Item item = GetItem(atIndex); // Uncomment if we want to remove equipped items
		HGItem item = _items[atIndex];

		Debug.Log("Equipping item:" + item.name);
		if(item != null) {
			switch (item.itemType) {
				case ItemType.Weapon:
					EquipWeapon((HGWeapon)item);
					break;
				default:
					break;
			}
		}
	}

	/*
	 * Equips a weapon 
	 */
	public void EquipWeapon(HGWeapon weapon) {
		if(weapon != null) {
			UnequipWeapon();

			_equippedWeapon = weapon;
			_equippedWeapon.Equip();

			switch (weapon.weaponType) {
				case WeaponType.Gun:
		            ((HGGun)_equippedWeapon).OnReloadDone = OnReloadDone;
					break;
				default:
					break;
			}
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
				case WeaponType.Gun:
		            ((HGGun)_equippedWeapon).OnReloadDone -= OnReloadDone;
					break;
				default:
					break;
			}

			_equippedWeapon = null;
		}
	}

	#endregion

	#region gun logic

	public bool Reload(float reloadTimeModifier) {
		if(_equippedWeapon != null && _equippedWeapon.weaponType == WeaponType.Gun) {
			HGGun equippedGun = (HGGun)_equippedWeapon;

			return equippedGun.Reload(reloadTimeModifier, GetAmmoCount(equippedGun.ammunitionType));
		}

		return false;
	}

    private void OnReloadDone(AmmunitionType ammunitionType, int currentAmmoCount) {
        SetAmmoCount(ammunitionType, currentAmmoCount);

        Debug.Log("New ammo: " + ammunitionType + " " + _ammunitions[(int)ammunitionType]);
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
