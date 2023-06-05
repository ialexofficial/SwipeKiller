using UnityEngine;

namespace Entities.Views.Weapon
{
    public class SwordWeapon : BaseWeapon
    {
        protected override void OnForceAdded(Vector2 delta)
        {
            float aimRotationZ = Vector2.SignedAngle(Vector2.up, delta.normalized);
            
            Vector3 aimRotation = transform.eulerAngles;
            aimRotation.z = aimRotationZ;

            _rigidbody.angularVelocity = Vector3.zero;
            transform.eulerAngles = aimRotation;
            
            base.OnForceAdded(delta);
        }
    }
}