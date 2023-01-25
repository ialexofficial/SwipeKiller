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
        [SerializeField] private GameObject winnerMenu;
        [SerializeField] private GameObject loserMenu;

        public void OnEnemyCountChanged(int count)
        {
            enemyCountText.text = count.ToString();
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
            winnerMenu.SetActive(true);
        }

        public void ShowLoserMenu()
        {
            loserMenu.SetActive(true);
        }
    }
}