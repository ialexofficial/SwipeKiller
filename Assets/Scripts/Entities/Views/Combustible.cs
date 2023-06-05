using Entities;
using UnityEngine;

namespace Entities.Views
{
    public class Combustible : MonoBehaviour, ICombustible
    {
        public bool BurnDown()
        {
            if (!gameObject.activeSelf)
                return false;
            
            gameObject.SetActive(false);
            return true;
        }
    }
}