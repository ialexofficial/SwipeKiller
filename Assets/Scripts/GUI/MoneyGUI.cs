using TMPro;
using UnityEngine;

namespace GUI
{
    public class MoneyGUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text countText;
        [SerializeField] private Animator coinAnimator;

        public void OnCoinsChanged(int coins)
        {
            countText.text = coins.ToString();
            coinAnimator.SetTrigger("Earn");
        }
    }
}