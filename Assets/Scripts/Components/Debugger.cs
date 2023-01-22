using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Components
{
    public class Debugger : MonoBehaviour
    {
        [SerializeField] private TMP_Text textOutput;
        [SerializeField] private int rowCount = 3;

        private Queue<string> _data = new Queue<string>();

        public void Write(string data)
        {
            if (_data.Count == rowCount)
                _data.Dequeue();
            
            _data.Enqueue(data);
            
            DisplayData();
        }

        private void DisplayData()
        {
            textOutput.text = _data.Aggregate((first, second) => $"{first}\n{second}");
        }
    }
}