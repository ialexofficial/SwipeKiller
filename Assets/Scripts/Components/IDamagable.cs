using UnityEngine;

namespace Components
{
    public interface IDamagable
    {
        public void Damage(int damage, Collider part);
    }
}