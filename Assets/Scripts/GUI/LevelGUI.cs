using System.Collections;
using TMPro;
using UnityEngine;

namespace GUI
{
    public class LevelGUI : MonoBehaviour
    {
        [Header("Enemy Count")]
        [SerializeField] private TMP_Text enemyCountText;

        [Header("Swipe Count")]
        [SerializeField] private TMP_Text swipeCountText;
        [SerializeField] private GameObject infiniteImage;
        
        [Header("Menus")]
        [SerializeField] private Animator winnerMenuAnimator;
        [SerializeField] private Animator loserMenuAnimator;
        [SerializeField] private float animationDelay = 1f;
        [SerializeField] private Animator mainMenuAnimator;

        [Header("Particles")]
        [Tooltip("Calculated by taking the entered percent of animationDelay")]
        [Range(0, 100)]
        [SerializeField]
        private int winParticlesDelay = 20;
        [SerializeField] private ParticleSystem[] winParticles;
        
#if UNITY_EDITOR
        public ParticleSystem[] WinParticles => winParticles;
#endif

        public void Started()
        {
            mainMenuAnimator.SetTrigger("Hide");
        }
        
        public void OnEnemyCountChanged(int count)
        {
            enemyCountText.text = $"x{count}";
        }

        public void OnSwipeCountChanged(int count)
        {
            if (count <= -1)
            {
                swipeCountText.gameObject.SetActive(false);
                infiniteImage.SetActive(true);
            }
            else
            {
                infiniteImage.SetActive(false);
                swipeCountText.gameObject.SetActive(true);
                swipeCountText.text = count.ToString();
            }
        }

        public void ShowWinnerMenu()
        {
            winnerMenuAnimator.gameObject.SetActive(true);
            StartCoroutine(AnimateMenuShowing(winnerMenuAnimator, winParticles, winParticlesDelay));
        }

        public void ShowLoserMenu()
        {
            loserMenuAnimator.gameObject.SetActive(true);
            StartCoroutine(AnimateMenuShowing(loserMenuAnimator, null, 0));
        }

        private IEnumerator AnimateMenuShowing(Animator animator, ParticleSystem[] particleSystems, int particlesDelay)
        {
            yield return new WaitForSecondsRealtime(animationDelay * particlesDelay / 100);

            if (particleSystems != null)
            {
                foreach (ParticleSystem particle in particleSystems)
                {
                    particle.Play();
                }
            }

            yield return new WaitForSecondsRealtime(animationDelay * (100 - particlesDelay) / 100);
            
            animator.SetTrigger("Show");
        }
    }
}