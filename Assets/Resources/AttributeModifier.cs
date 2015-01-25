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
	public AttributeModifierTypes modifierType;
	public AttributeModifierMethod modifierMethod;

	public float Apply(float baseValue) {
		float newValue = baseValue;
		switch(modifierMethod) {
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
