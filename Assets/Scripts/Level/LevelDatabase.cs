using Level;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    [CreateAssetMenu(menuName = "Game Data/Level Database")]
    public class LevelDatabase : ScriptableObject
    {
        public LevelConfig FirstLevel;
        [Tooltip("Levels are saving by order in array. Changing level order can crash player saves!")]
        public List<LevelConfig> Data;
    }
}