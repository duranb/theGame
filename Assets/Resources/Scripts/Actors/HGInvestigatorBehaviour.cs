using UnityEngine;
using System.Collections;


public class HGInvestigatorBehaviour : MonoBehaviour
{
	// movement config
	public float _gravity = -25f;
	public float _runSpeed = 8f;
	public float _groundDamping = 20f; // how fast do we change direction? higher means faster
	public float _inAirDamping = 5f;
	public float _jumpHeight = 3f;

	[HideInInspector]
	private float _normalizedHorizontalSpeed = 0;

	private HGCharacterController _controller;
	private RaycastHit2D _lastControllerColliderHit;
	private Vector3 _velocity;

	private HGCharacterInventory _characterInventory;

	private HGCharacterAnimator _characterAnimator;

	public int _inventorySize = 20;

	void Awake()
	{
		_controller = GetComponent<HGCharacterController>();

		// listen to some events for illustration purposes
		_controller.OnControllerCollidedEvent += OnControllerCollider;
		_controller.OnTriggerEnterEvent += OnTriggerEnterEvent;
		_controller.OnTriggerExitEvent += OnTriggerExitEvent;

		_characterInventory = new HGCharacterInventory(_inventorySize);
		
		_characterAnimator = new HGCharacterAnimator();

		// For testing
		HGRevolver revolver = new HGRevolver("Betsy", 100, 2000, 1000, 2000, 6);
		HGRevolver revolverr = new HGRevolver("Bettsy", 100, 3000, 1000, 2000, 6);
		HGAmmunitionPickup revolverAmmo = new HGAmmunitionPickup(AmmunitionType.Revolver, 40);

		// Set up inventory
		_characterInventory.Add(revolver);
		_characterInventory.Add(revolverr);

		_characterInventory.SetAmmoCapacity(AmmunitionType.Revolver, 30);

		bool added = _characterInventory.Add(revolverAmmo);

		// Should print "false, 10"
		Debug.Log(added + ", " + revolverAmmo.amount);
	}

	#region Event Listeners

	void OnControllerCollider( RaycastHit2D hit )
	{
		// bail out on plain old ground hits cause they arent very interesting
		if( hit.normal.y == 1f )
			return;

		// logs any collider hits if uncommented. it gets noisy so it is commented out for the demo
		//Debug.Log( "flags: " + _controller.collisionState + ", hit.normal: " + hit.normal );
	}


	void OnTriggerEnterEvent( Collider2D col )
	{
		Debug.Log( "onTriggerEnterEvent: " + col.gameObject.name );
	}


	void OnTriggerExitEvent( Collider2D col )
	{
		Debug.Log( "onTriggerExitEvent: " + col.gameObject.name );
	}

	#endregion


	// the Update loop contains a very simple example of moving the character around and controlling the animation
	void Update()
	{
		// grab our current _velocity to use as a base for all calculations
		_velocity = _controller._velocity;

		if( _controller._isGrounded )
			_velocity.y = 0;

		if( Input.GetKey( KeyCode.RightArrow ) )
		{
			_normalizedHorizontalSpeed = 1;
			if( transform.localScale.x < 0f )
				transform.localScale = new Vector3( -transform.localScale.x, transform.localScale.y, transform.localScale.z );
		}
		else if( Input.GetKey( KeyCode.LeftArrow ) )
		{
			_normalizedHorizontalSpeed = -1;
			if( transform.localScale.x > 0f )
				transform.localScale = new Vector3( -transform.localScale.x, transform.localScale.y, transform.localScale.z );
		}
		else
		{
			_normalizedHorizontalSpeed = 0;
		}


		// we can only jump whilst grounded
		if( _controller._isGrounded && Input.GetKeyDown( KeyCode.UpArrow ) )
		{
			_velocity.y = Mathf.Sqrt( 2f * _jumpHeight * -_gravity );
		}


		// apply horizontal speed smoothing it
		var smoothedMovementFactor = _controller._isGrounded ? _groundDamping : _inAirDamping; // how fast do we change direction?
		_velocity.x = Mathf.Lerp( _velocity.x, _normalizedHorizontalSpeed * _runSpeed, Time.deltaTime * smoothedMovementFactor );

		// apply gravity before moving
		_velocity.y += _gravity * Time.deltaTime;

		_controller.move( _velocity * Time.deltaTime );

		if(Input.GetKey(KeyCode.Space)) {
			if(_characterInventory.equippedWeapon != null) {
				WeaponState weaponState = _characterInventory.equippedWeapon.Attack(this.transform.position, this.transform.rotation, 1, 1);
				if(weaponState == WeaponState.Empty) {
					float reloadTime = _characterInventory.Reload(1);
					_characterAnimator.Reload(reloadTime);
					_characterAnimator.OnReloadDone = _characterInventory.OnReloadDone;
				}
			}
		}

		if(Input.GetKeyDown(KeyCode.Alpha1)) {
			float equipTime = _characterInventory.Equip(0);
			_characterAnimator.Equip(equipTime);
			_characterAnimator.OnEquipDone = _characterInventory.OnEquipDone;
		}
		if(Input.GetKeyDown(KeyCode.Alpha2)) {
			float equipTime = _characterInventory.Equip(1);
			_characterAnimator.Equip(equipTime);
			_characterAnimator.OnEquipDone = _characterInventory.OnEquipDone;
		}
	}

}
