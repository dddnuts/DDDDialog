using UnityEngine;

namespace DDD
{
    internal class UnsupportedDialogImpl : IDialogImpl
    {
        public void Show(string message, string positive, string negative, IDialogDelegate dialogDelegate)
        {
            Debug.LogError("DDDDialog doesn't support this platform: " + Application.platform);
        }
    }
}