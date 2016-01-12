using UnityEngine;

namespace DDD
{
    public class Dialog : MonoBehaviour, IDialogDelegate
    {
        public delegate void DialogResultDelegate(Dialog dialog,bool isPositive);

        public bool IsShowing { get; private set; }

        #if UNITY_IOS && !UNITY_EDITOR
        private IDialogImpl dialogImpl = new UnsupportedDialogImpl();
        #elif UNITY_ANDROID && !UNITY_EDITOR
        private IDialogImpl dialogImpl = new AndroidDialogImpl();
        #else
        private IDialogImpl dialogImpl = new UnsupportedDialogImpl();
        #endif

        private DialogResultDelegate dialogDelegate;

        public void Show(string message, string positive, string negative, DialogResultDelegate resultDelegate)
        {
            if (IsShowing)
            {
                return;
            }

            IsShowing = true;

            dialogDelegate = resultDelegate;
            dialogImpl.Show(message, positive, negative, this);
        }

        public void HandleResult(bool positive)
        {
            IsShowing = false;

            if (dialogDelegate == null)
                return;

            dialogDelegate(this, positive);
            dialogDelegate = null;
        }
    }
}