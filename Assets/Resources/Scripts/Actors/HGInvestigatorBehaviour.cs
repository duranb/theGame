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

		_controller.OnControllerCollidedEvent += OnControllerCollider;
		_controller.OnTriggerEnterEvent += OnTriggerEnterEvent;
		_controller.OnTriggerExitEvent += OnTriggerExitEvent;

		_characterInventory = new HGCharacterInventory(_inventorySize);
		
		_characterAnimator = new HGCharacterAnimator();

		_characterInventory.SetAmmunitionCapacity(AmmunitionType.Revolver, 30);
	}

	#region Event Listeners

	void OnControllerCollider( RaycastHit2D hit )
	{
		// bail out on plain old ground hits cause they arent very interesting
		if( hit.normal.y == 1f )
			return;
	}


	void OnTriggerEnterEvent( Collider2D col )
	{
		//Debug.Log( "onTriggerEnterEvent: " + col.gameObject.name );

		HGItemPickupBehaviour itemPickup = col.gameObject.GetComponent<HGItemPickupBehaviour>();
		if(itemPickup != null) {
			HGItem item = itemPickup.GetItem();

			if(_characterInventory.Add(item)) {
				itemPickup.Pickup();
			}
		}
	}


	void OnTriggerExitEvent( Collider2D col )
	{
		//Debug.Log( "onTriggerExitEvent: " + col.gameObject.name );
	}

	#endregion

    private void DoMovementInput()
    {
        // grab our current _velocity to use as a base for all calculations
        _velocity = _controller._velocity;

        if (_controller._isGrounded)
            _velocity.y = 0;

        //if (Input.GetKeyDown(KeyCode.RightArrow))
        //    transform.rotation = Quaternion.LookRotation(Vector3.right);
        //else if (Input.GetKeyDown(KeyCode.LeftArrow))
        //    transform.rotation = Quaternion.LookRotation(-Vector3.right);

        if (Input.GetKey(KeyCode.RightArrow))
        {
            _normalizedHorizontalSpeed = 1;
            if (transform.localScale.x < 0f)
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            _normalizedHorizontalSpeed = -1;
            if (transform.localScale.x > 0f)
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        else
        {
            _normalizedHorizontalSpeed = 0;
        }

        // we can only jump while grounded
        if (_controller._isGrounded && Input.GetKeyDown(KeyCode.UpArrow))
        {
            _velocity.y = Mathf.Sqrt(2f * _jumpHeight * -_gravity);
        }


        // apply horizontal speed smoothing it
        var smoothedMovementFactor = _controller._isGrounded ? _groundDamping : _inAirDamping; // how fast do we change direction?
        _velocity.x = Mathf.Lerp(_velocity.x, _normalizedHorizontalSpeed * _runSpeed, Time.deltaTime * smoothedMovementFactor);

        // apply gravity before moving
        _velocity.y += _gravity * Time.deltaTime;

        _controller.move(_velocity * Time.deltaTime);
    }

    private void DoWeaponInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (_characterInventory.equippedWeapon != null)
            {
                float attackTime = 0f;
                if(_characterInventory.equippedWeapon.weaponType == WeaponType.Ranged) {
                    attackTime = ((HGRangedWeapon)_characterInventory.equippedWeapon).Shoot(this.transform.position, this.transform.right * this.transform.localScale.x, 1f, 1f, 1f);
                } else if(_characterInventory.equippedWeapon.weaponType == WeaponType.Melee) {
                    attackTime = ((HGMeleeWeapon)_characterInventory.equippedWeapon).Swing(this.transform.position, this.transform.right * this.transform.localScale.x, 1f, 1f, 1f);
                }
                _characterAnimator.Attack(attackTime);
                _characterAnimator.OnAttackDone = _characterInventory.equippedWeapon.AttackDone;
                if (_characterInventory.equippedWeapon.weaponState == WeaponState.Empty)
                {
                    float reloadTime = _characterInventory.Reload(1);
                    _characterAnimator.Reload(reloadTime);
                    _characterAnimator.OnReloadDone = _characterInventory.ReloadDone;
                }
            }
        }
    }

    private void DoInventoryInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            float equipTime = _characterInventory.Equip(0);
            _characterAnimator.Equip(equipTime);
            _characterAnimator.OnEquipDone = _characterInventory.EquipDone;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            float equipTime = _characterInventory.Equip(1);
            _characterAnimator.Equip(equipTime);
            _characterAnimator.OnEquipDone = _characterInventory.EquipDone;
        }
    }

	// the Update loop contains a very simple example of moving the character around and controlling the animation
	void Update()
	{
        DoMovementInput();
        DoInventoryInput();
        DoWeaponInput();
	}
}