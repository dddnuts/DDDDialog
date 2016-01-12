#if UNITY_ANDROID
using UnityEngine;

namespace DDD
{
    internal class AndroidDialogImpl : IDialogImpl
    {
        public void Show(string message, string positive, string negative, IDialogDelegate dialogDelegate)
        {
            var unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            var activity = unity.GetStatic<AndroidJavaObject>("currentActivity");

            activity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
                    {
                        var builder = new AndroidJavaObject("android.app.AlertDialog$Builder", activity);
                        builder.Call<AndroidJavaObject>("setMessage", message);
                        builder.Call<AndroidJavaObject>("setPositiveButton", positive, new OnClickListener(dialogDelegate, true));
                        builder.Call<AndroidJavaObject>("setNegativeButton", negative, new OnClickListener(dialogDelegate, false));

                        var dialog = builder.Call<AndroidJavaObject>("create");
                        dialog.Call("show");
                    }));
        }

        private class OnClickListener : AndroidJavaProxy
        {
            private IDialogDelegate dialogDelegate;
            private bool positive;

            public OnClickListener(IDialogDelegate dialogDelegate, bool positive)
                : base("android.content.DialogInterface$OnClickListener")
            {
                this.dialogDelegate = dialogDelegate;
                this.positive = positive;
            }

            public void onClick(AndroidJavaObject dialog, int which)
            {
                if (dialogDelegate == null)
                    return;

                dialogDelegate.HandleResult(positive);
            }
        }
    }
}
#endif