using System;
using Entities.Views;
using UnityEngine;

namespace Utilities
{
    public static class Vibration
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        private static AndroidJavaClass _unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        private static AndroidJavaObject _currentActivity =
 _unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        private static AndroidJavaObject _vibrator =
 _currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
#else
        private static AndroidJavaClass _unityPlayer;
        private static AndroidJavaObject _currentActivity;
        private static AndroidJavaObject _vibrator;
#endif

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
                // ignored
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
    }
}