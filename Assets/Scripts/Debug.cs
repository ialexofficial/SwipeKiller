using TMPro;
using UnityEngine;

namespace DebugSpace 
{
    public class Debug : MonoBehaviour
    {
        [SerializeField] private TMP_Text _fpsText;
        [SerializeField] private TMP_Text _timescaleText;

        private void Update()
        {
            _fpsText.text = ((int) (Time.timeScale / Time.deltaTime)).ToString();
            _timescaleText.text = $"Scale: {Time.timeScale.ToString()}; Fixed: {Time.fixedDeltaTime.ToString()}";
        }
    }
}
