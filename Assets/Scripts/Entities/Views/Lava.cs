using System.Collections;
using System.Collections.Generic;
using Entities;
using UnityEngine;
using Utilities;

namespace Entities.Views
{
    public class Lava : MonoBehaviour
    {
        [SerializeField] private ParticleSystem destroyEffect;
        [SerializeField] private LayerMask destroyableLayers;
        [SerializeField] private AudioSource destroySound;

        private Stack<ParticleSystem> effectsPull = new Stack<ParticleSystem>();

        private void Start()
        {
            effectsPull.Push(destroyEffect);

            for (int i = 1; i < 64; ++i)
            {
                effectsPull.Push(Instantiate(destroyEffect, transform));
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!LayerMasker.CheckLayer(destroyableLayers, other.gameObject.layer))
                return;
            
            if (other.GetComponentInParent<ICombustible>().BurnDown())
            {
                ParticleSystem effect = effectsPull.Pop();
                effect.transform.position = other.transform.position;
                effect.Play();
                destroySound.Play();
                StartCoroutine(ReturnEffectToPull(effect));
            }
        }

        private IEnumerator ReturnEffectToPull(ParticleSystem effect)
        {
            yield return new WaitForSeconds(effect.main.duration);
            
            effectsPull.Push(effect);
        }
    }
}