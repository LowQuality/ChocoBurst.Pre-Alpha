using UnityEngine;

namespace Management
{
public static class ToastMessage
{
    public static void Send(string message)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            var unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            if (unityActivity == null) return;
            var toastClass = new AndroidJavaClass("android.widget.Toast");
            unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                toastClass.CallStatic<AndroidJavaObject>("makeText", unityActivity, message, 0).Call("show");
            }));
        }
        else
        {
            Debug.Log(message);
        }
    }
}
}