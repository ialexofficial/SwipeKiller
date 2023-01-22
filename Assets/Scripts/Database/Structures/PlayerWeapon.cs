using System;
using System.Collections.Generic;

namespace Database.Structures
{
    [Serializable]
    public struct PlayerWeapon
    {
        public string SelectedWeapon;
        public HashSet<string> BoughtWeapon;
    }
}