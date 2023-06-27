using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Entities.Views
{
    public class Wood : Destroyable
    {
        private List<Wood> _connectedWoods = new List<Wood>();
        private bool _isDestroyed = false;
        
        protected new void Start()
        {
            base.Start();

            foreach (var collider in Physics.OverlapBox(
                         transform.position,
                         _collider.bounds.extents * 1.5f,
                         transform.rotation,
                         1 << gameObject.layer
                     ))
            {
                if (collider.gameObject == gameObject || !collider.TryGetComponent<Wood>(out var wood))
                    continue;
                
                _connectedWoods.Add(wood);
            }
        }

        public override void Damage(int damage, Collider part)
        {
            if (_isDestroyed)
                return;

            _isDestroyed = true;
            
            foreach (var connected in _connectedWoods)
            {
                connected.Damage(damage, null);
            }
            
            base.Damage(damage, part);
        }
    }
}