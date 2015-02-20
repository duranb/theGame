using UnityEngine;
using System.Collections;

public class HGmm38Behaviour : HGAmmunitionBehaviour {
    public override void Fire(float damage, Vector3 direction, TrajectoryType trajectoryType, float velocityModifier) {
        base.Fire(damage, direction, trajectoryType, velocityModifier);
    }
}
