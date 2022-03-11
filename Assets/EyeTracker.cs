using MPDepthCore;
using MPDepthCore.Calibration.Camera;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeTracker : MonoBehaviour
{
    public enum Eye
    {
        LEFT,
        RIGHT
    };

    [SerializeField] public Eye eye;
    [SerializeField] TrackingSystemsManager trackingSystemsManager;

    private void Update()
    {
        EyeTrackingData eyeTrackingData = trackingSystemsManager.CurrentCalibratedTrackingData.EyeTrackingData;
        if( eye == Eye.LEFT)
        {
            // get left eye tracking info
            Vector3 pos = eyeTrackingData.LeftEyeTrackingData.Position;
            transform.position = pos;

        }

        if(eye == Eye.RIGHT)
        {
            // get right eye tracking info
            Vector3 pos = eyeTrackingData.RightEyeTrackingData.Position;
            transform.position = pos;
        }
    }


}
