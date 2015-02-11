using UnityEngine;
using System.Collections;
using System.Timers;

public class HGCharacterAnimator {
	private Timer _attackTimer;
	private Timer _reloadTimer;
	private Timer _equipTimer;

	// Callback for when reloading is done
	public delegate void OnReloadDoneDelegate();
	public OnReloadDoneDelegate OnReloadDone;

	public delegate void OnAttackDoneDelegate();
	public OnAttackDoneDelegate OnAttackDone;

	public delegate void OnEquipDoneDelegate();
	public OnEquipDoneDelegate OnEquipDone;

	public HGCharacterAnimator() {

	}

	public void CancelAnimations() {
		if(_attackTimer != null) {
			_attackTimer.Dispose();
		}
		if(_reloadTimer != null) {
			_reloadTimer.Dispose();
		}
		if(_equipTimer != null) {
			_equipTimer.Dispose();
		}
	}

	public void Attack(float time) {
	}

	public void Reload(float time) {
		if(time > 0) {
			CancelAnimations();

	        _reloadTimer = new Timer(time);
	        _reloadTimer.Enabled = true;
			_reloadTimer.AutoReset = false; //Stops it from repeating
	        // Hook up the Elapsed event for the timer. 
	        _reloadTimer.Elapsed += delegate {
	        	OnReloadDone(); 
	        };
	    }
	}

	public void Equip(float time) {
		if(time > 0) {
			CancelAnimations();

	        _equipTimer = new Timer(time);
	        _equipTimer.Enabled = true;
			_equipTimer.AutoReset = false; //Stops it from repeating
	        // Hook up the Elapsed event for the timer. 
	        _equipTimer.Elapsed += delegate {
	        	OnEquipDone(); 
	        };
	    }
	}
}