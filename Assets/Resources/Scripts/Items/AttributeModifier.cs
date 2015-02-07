using UnityEngine;
using System.Collections;

public enum AttributeModifierTypes
{
	Speed = 0,
	Jump = 1
}

public enum AttributeModifierMethod
{
	Additive = 0,
	Multiplicative = 1
}

public class AttributeModifier : Item {
	public AttributeModifierTypes _modifierType;
	public AttributeModifierMethod _modifierMethod;
	
	public float _value;

	public float value {
		get { return _value; }
		set { _value = value; }
	}

	public float Apply(float baseValue) {
		float newValue = baseValue;
		switch(_modifierMethod) {
			case AttributeModifierMethod.Additive:
				newValue += this.value;
				break;
			case AttributeModifierMethod.Multiplicative:
				newValue *= this.value;
				break;
		}

		return newValue;
	}
}
