using System;
using Ji2.Presenters.Tutorial;
using Ji2Core.Core;
using Ji2Core.Core.States;
using Utilities;

namespace GUI.Tutorial
{
    public class TutorialFactory : ITutorialFactory
    {
        private readonly Context _context;
        private readonly StateMachine _stateMachine;

        public TutorialFactory(Context context)
        {
            _context = context;
            _stateMachine = _context.GetService<StateMachine>();
        }
        
        public ITutorialStep Create<TStep>() where TStep : ITutorialStep
        {
            if (typeof(TStep) == typeof(SwipeTutorialStep))
            {
                return new SwipeTutorialStep(_stateMachine, 
                    _context.GetService<LevelDataProvider>(),
                    _context.GetService<TutorialPointerView>(),
                    _context.GetService<VirtualCameraProvider>().MainCamera
                );
            }

            throw new NotImplementedException($"No such tutorial step type: {typeof(TStep)}");
        }
    }
}