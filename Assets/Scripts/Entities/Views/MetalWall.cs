using UnityEngine;
using Utilities;

namespace Entities.Views
{
    public class MetalWall : MonoBehaviour
    {
        [SerializeField] private ParticleSystem sparksEffect;
        [SerializeField] private LayerMask sparksEffectLayers;
        [SerializeField] private AudioSource sparksSound;

        private void OnCollisionEnter(Collision collision)
        {
            if (LayerMasker.CheckLayer(sparksEffectLayers, collision.gameObject.layer))
            {
                sparksEffect.transform.position = collision.GetContact(0).point;
                sparksEffect.Play();
                sparksSound.Play();
            }
        }
    }
}