using UnityEngine;
using System.Collections;

using System.Timers;

public enum GunType
{
	Revolver,
	Rifle,
	Sniper
}

public class Gun : Weapon {
	public GameObject _ammunitionPrefab;

	protected GunType _gunType;
	protected AmmunitionType _ammunitionType;

	protected float _baseReloadTime;
	protected int _baseClipSize;

	public int _currentClipAmmoCount;

	private bool _isFiring = false;
	private bool _isReloading = false;


	// Callback for when reloading is done
	public delegate void OnReloadDelegate(AmmunitionType type, int ammoLeft);
	public OnReloadDelegate OnReloadDone;

	// Callback to indicate to the character that the clip is empty
	public delegate void OnEmptyReloadDelegate();
	public OnEmptyReloadDelegate OnEmptyReload;


	// Callback to throttle the firing rate
	private delegate void OnAttackDelegate();
	private OnAttackDelegate OnAttackDone;

	/*
	 * attackDamageModifier - the attribute from the character to affect damage
	 * attackRateModifier - the attribute from the character to affect rate of fire
	 */
	public override void Attack(float attackDamageModifier, float attackRateModifier) {
		if(_currentClipAmmoCount > 0 && !_isFiring && !_isReloading) {
			// Fire projectile
			GameObject ammoObject = (GameObject)Instantiate(_ammunitionPrefab, this.transform.position, this.transform.rotation);
			Ammunition ammo = ammoObject.GetComponent<Ammunition>();

			ammo.Fire(this._baseDamage * attackDamageModifier);

			_currentClipAmmoCount--;
			_isFiring = true;

			StartCoroutine(AttackWait(attackRateModifier));
		}
		
		if(_currentClipAmmoCount == 0) {
			OnEmptyReload();
		}
	}

	/*
	 * Used to throttle the firing rate
	 *
	 * attackRateModifier - the attribute from the character to affect rate of fire
	 */ 
	private IEnumerator AttackWait(float attackRateModifier) {
		yield return new WaitForSeconds(this._baseRate * attackRateModifier);
		_isFiring = false;
	}

	/*
	 * currentAmmoCount - the count of ammo of the type used by the gun
	 * reloadTimeModifier - the attribute from the character to affect reload time
	 */
	public virtual void Reload(float reloadTimeModifier, int currentAmmoCount) {
		Debug.Log("RELOAD" + ", " + reloadTimeModifier + ", " + currentAmmoCount);
		if(currentAmmoCount == 0 || _currentClipAmmoCount == _baseClipSize) {
			return;
		}

		var clipDeficit = _baseClipSize - _currentClipAmmoCount;

		var ammoLeft = currentAmmoCount - clipDeficit;
		_currentClipAmmoCount += (ammoLeft > 0) ? clipDeficit : currentAmmoCount;
		currentAmmoCount = (ammoLeft > 0) ? ammoLeft : 0;

		_isReloading = true;

		StartCoroutine(ReloadWait(reloadTimeModifier, currentAmmoCount));
	}

	/*
	 * Used to impose reload time
	 *
	 * currentAmmoCount - the count of ammo of the type used by the gun
	 * reloadTimeModifier - the attribute from the character to affect reload time
	 */ 
	private IEnumerator ReloadWait(float reloadTimeModifier, int currentAmmoCount) {
		yield return new WaitForSeconds(_baseReloadTime * reloadTimeModifier);
		_isReloading = false;
		OnReloadDone(_ammunitionType, currentAmmoCount);
	}


	public override void Start() {
		base.Start();

		this._weaponType = WeaponType.Gun;
	}

	public override void Update() {
		base.Update();
	}
}
