using UnityEngine;

namespace Utilities
{
    public class ParticlesProvider : MonoBehaviour
    {
        [SerializeField] private ParticleSystem particleSystem;

        public ParticleSystem ParticleSystem => particleSystem;
    }
}