/*
 * A central file for all enumeration definitions
 */

public enum ItemType
{
	AttributeModifier,
	Weapon,
	Ammunition,
	Armor
}

public enum AttributeModifierTypes
{
	Speed,
	Jump,
	Damage,
	ReloadSpeed
}

public enum AttributeModifierMethod
{
	Additive,
	Multiplicative
}

public enum AmmunitionType
{
	Revolver,
	Rifle,
	Length
}

public enum TrajectoryType {
	Arch,
	Straight,
	Wave
}

public enum RangedWeaponType
{
	Revolver,
	Rifle,
	Sniper
}

public enum WeaponType
{
	Melee,
	Ranged
}

public enum WeaponState {
	Equipping,
	Attacking,
	Reloading,
	Ready,
	Empty,
	Unequipped,
	Broken
}

public enum ArmorType {
	Head,
	Body,
	Arms,
	Hands,
	Legs,
	Feet,
	Belt
}