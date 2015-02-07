using UnityEngine;
using System.Collections;

using System.Timers;

public enum GunType
{
	Revolver = 0,
	Rifle = 1
}

public class Gun : Weapon {

	#region events, properties and fields

	public GameObject _ammunitionPrefab;

	protected GunType _gunType;
	protected AmmunitionType _ammunitionType;

	protected float _baseReloadTime;
	protected int _baseClipSize;

	private int _currentClipSize;

	public int _currentClipCount;

	private bool _isFiring = false;
	private bool _isReloading = false;

	#endregion

	#region delegates

	public delegate void OnReloadDelegate(AmmunitionType type, int ammoLeft);
	public OnReloadDelegate OnReloadDone;

	public delegate void OnEmptyReloadDelegate();
	public OnEmptyReloadDelegate OnEmptyReload;

	private delegate void OnAttackDelegate();
	private OnAttackDelegate OnAttackDone;

	#endregion

	#region public

	/*
	 * attackSpeedModifier - the attribute from the character for firing rate
	 */
	public override void Attack(float attackSpeedModifier) {
		if(_currentClipCount > 0 && !_isFiring && !_isReloading) {
			// Fire projectile
			GameObject ammoObject = (GameObject)Instantiate(_ammunitionPrefab, this.transform.position, this.transform.rotation);
			Ammunition ammo = ammoObject.GetComponent<Ammunition>();

			ammo.Fire();

			_currentClipCount--;
			_isFiring = true;

			StartCoroutine(AttackWait(attackSpeedModifier));
		}
		
		if(_currentClipCount == 0) {
			OnEmptyReload();
		}
	}

	/*
	 * currentAmmoCount - the count of ammo of the type used by the gun
	 * reloadTimeModifier - the attribute from the character for reload time
	 */
	public virtual void Reload(int currentAmmoCount, float reloadTimeModifier) {
		if(currentAmmoCount == 0 || _currentClipCount == _currentClipSize) {
			return;
		}

		var clipDeficit = _currentClipSize - _currentClipCount;

		var ammoLeft = currentAmmoCount - clipDeficit;
		_currentClipCount += (ammoLeft > 0) ? clipDeficit : currentAmmoCount;
		currentAmmoCount = (ammoLeft > 0) ? ammoLeft : 0;

		_isReloading = true;

		StartCoroutine(ReloadWait(reloadTimeModifier, currentAmmoCount));
	}

	private IEnumerator ReloadWait(float reloadTimeModifier, int currentAmmoCount) {
		yield return new WaitForSeconds(_baseReloadTime - reloadTimeModifier);
		_isReloading = false;
		OnReloadDone(_ammunitionType, currentAmmoCount);
	}

	private IEnumerator AttackWait(float attackSpeedModifier) {
		yield return new WaitForSeconds(_baseRate - attackSpeedModifier);
		_isFiring = false;
	}
	
	#endregion

	#region Monobehaviour

	public override void Start() {
		base.Start();

		this._weaponType = WeaponType.Gun;

		_currentClipSize = _baseClipSize;
	}

	public override void Update() {
		base.Update();
	}

	#endregion
}
