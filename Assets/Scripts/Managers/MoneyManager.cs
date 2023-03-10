using Database;
using Database.Interfaces;
using UnityEngine;
using UnityEngine.Events;

namespace Managers
{
    public class MoneyManager : MonoBehaviour
    {
        public UnityEvent<int> OnCoinsChange = new UnityEvent<int>();
        
        private int _earned = 0;
        
        public void EarnMoney(int money)
        {
            _earned += money;
            
            OnCoinsChange.Invoke(_earned);
        }

        public void SaveCoins()
        {
            IProgressDatabase database = ProgressDatabase.GetInstance;
            
            database.SetCoins(_earned);
        }

        private void Start()
        {
            IProgressDatabase database = ProgressDatabase.GetInstance;

            _earned = database.GetCoins();
            OnCoinsChange.Invoke(_earned);
        }

        private void OnDestroy()
        {
            SaveCoins();
        }
    }
}