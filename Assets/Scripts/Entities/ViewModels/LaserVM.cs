using System;
using Entities.Models;

namespace Entities.ViewModels
{
    public class LaserVM
    {
        private readonly LaserModel _model;

        public event Action<bool> OnSwitch;

        public LaserVM(LaserModel model)
        {
            _model = model;

            _model.OnSwitch += isActive => OnSwitch?.Invoke(isActive);
        }

        public void Switch()
        {
            _model.Switch();
        }

        public void Switch(bool isEnabled)
        {
            _model.Switch(isEnabled);
        }
    }
}