using UnityEngine;
using System;

namespace DDD
{
    internal interface IDialogImpl
    {
        void Show(string message, string positive, string negative, IDialogDelegate dialogDelegate);
    }
}