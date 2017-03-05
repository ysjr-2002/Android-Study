
using Android.Hardware.Camera2;
using Android.Util;

namespace CameraApp6.Listeners
{
    public class CameraCaptureStillPictureSessionCallback : CameraCaptureSession.CaptureCallback
    {
        private static readonly string TAG = "CameraCaptureStillPictureSessionCallback";

        public Camera2BasicFragment Owner { get; set; }

        public CameraCaptureStillPictureSessionCallback(Camera2BasicFragment owner)
        {
            Owner = owner;
        }

        public override void OnCaptureCompleted(CameraCaptureSession session, CaptureRequest request, TotalCaptureResult result)
        {
            //Owner.ShowToast("Saved: " + Owner.mFile);
            Log.Debug(TAG, Owner.mFile.ToString());
            //Owner.UnloeckFocus();
        }
    }
}