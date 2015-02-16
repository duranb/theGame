using UnityEngine;
using System.Collections;

public class HGItemPickupBehaviour : MonoBehaviour {
	public string itemName;

	public virtual void Start() {}

	public virtual HGItem GetItem() { 
		return null;
	}

	public virtual void Pickup() {
		Destroy(this.gameObject);
	}
}
