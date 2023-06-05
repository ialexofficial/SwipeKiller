using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Ji2.CommonCore.SaveDataContainer;
using SwipeKiller;
using Entities.Views;

namespace Level.Models
{
    public class MoneyDataModel
    {
        private readonly ISaveDataContainer _saveDataContainer;
        private readonly List<Coin> _coins;

        public event Action<int> OnMoneyAmountChange;
        
        public int MoneyAmount { get; private set; }

        public MoneyDataModel(ISaveDataContainer saveDataContainer, List<Coin> coins)
        {
            _saveDataContainer = saveDataContainer;
            _coins = coins;

            MoneyAmount = saveDataContainer.GetValue<int>(Constants.MONEY_SAVE_DATA_KEY);

            foreach (var coin in coins)
            {
                coin.OnCollect += EarnMoney;
            }
        }

        public void EarnMoney(int value)
        {
            MoneyAmount += value;
            
            OnMoneyAmountChange?.Invoke(MoneyAmount);
        }

        public void SpendMoney(int value)
        {
            MoneyAmount -= value;
            
            OnMoneyAmountChange?.Invoke(MoneyAmount);
        }

        public void SaveData()
        {
            _saveDataContainer.SaveValue(Constants.MONEY_SAVE_DATA_KEY, MoneyAmount);
        }
    }
}