using System;
using Components;
using UnityEngine;

namespace Utilities
{
    public static class Vibration
    {
    #if UNITY_ANDROID && !UNITY_EDITOR
        private static AndroidJavaClass _unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        private static AndroidJavaObject _currentActivity = _unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        private static AndroidJavaObject _vibrator = _currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
    #else
        private static AndroidJavaClass _unityPlayer;
        private static AndroidJavaObject _currentActivity;
        private static AndroidJavaObject _vibrator;
    #endif
        private static Debugger _debugger;

        public static bool IsAndroid =>
        #if UNITY_ANDROID && !UNITY_EDITOR
            true;
        #else
            false;
        #endif

        public static void Vibrate()
        {
            if (IsAndroid)
                _vibrator.Call("vibrate");
            else
                Handheld.Vibrate();
        }
        
        public static void Vibrate(long milliseconds)
        {
            FindDebugger();
            _debugger?.Write($"Vibration time: {milliseconds}");
            
            try
            {
                if (IsAndroid)
                {
                    _vibrator.Call("vibrate", milliseconds);
                }
                else
                {
                    Handheld.Vibrate();
                }
            }
            catch (Exception exception)
            {
                _debugger?.Write(exception.Message);
            }
        }
        
        public static void Vibrate(long[] pattern, int repeat)
        {
            if (IsAndroid)
                _vibrator.Call("vibrate", pattern, repeat);
            else
                Handheld.Vibrate();
        }

        public static void Cancel()
        {
            if (IsAndroid)
                _vibrator.Call("cancel");
        }

        private static void FindDebugger()
        {
            if (_debugger != null)
                return;
            
            _debugger = GameObject.FindObjectOfType<Debugger>();
            
            _debugger?.Write($"Vibration found debugger");
        }
    }
}