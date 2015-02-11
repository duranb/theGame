/*
 * A central file for all enumeration definitions
 */

public enum ItemType
{
	AttributeModifier,
	Weapon,
	Ammunition
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

public enum GunType
{
	Revolver,
	Rifle,
	Sniper
}

public enum WeaponType
{
	Melee,
	Gun
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