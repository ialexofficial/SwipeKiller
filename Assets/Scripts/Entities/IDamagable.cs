using UnityEngine;

namespace Entities
{
    public interface IDamagable
    {
        public void Damage(int damage, Collider part);
    }
}