using System;
using Android.Runtime;
using Android.Telephony;

namespace FaceVisualExt
{

    class GsmSignalStrengthListener : PhoneStateListener
    {
        public delegate void SignalStrengthChangedDelegate(int strength);
        public event SignalStrengthChangedDelegate SignalStrengthChanged;

        public override void OnSignalStrengthsChanged(SignalStrength signalStrength)
        {
            if (signalStrength.IsGsm)
            {
                if (SignalStrengthChanged != null)
                {
                    SignalStrengthChanged(signalStrength.GsmSignalStrength);
                }
            }
        }
    }
}