using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MPDepthCore;
using MPDepthCore.Calibration.Camera;
using MPDepthCore.TrackingSources;
using OffAxisCamera;
using UnityEngine;

public class MotiveTrackingCalibration : MonoBehaviour
{
    [SerializeField] MPDepthTrackingSource trackingSource = default;
    [SerializeField] GameObject calibrationObjects = default;
    [SerializeField] GameObject calibrationUI;
    [SerializeField] Transform TrackedScreen;

    bool calibrating = false;
    MPDepthTrackingData trackingData;
    bool cancelled;

    [SerializeField] float defaultDistanceFromScreenCenter;
    [SerializeField] Transform calibrationTransform;
    GameObject mainUI;
    OffAxisCameraRig offAxisCameraRig;
    Camera offAxisCamera;

    private void Awake()
    {
        TrackingSystem trackingSystem = GetComponentInParent<TrackingSystem>();
        mainUI = trackingSystem.mainUI;
        offAxisCameraRig = trackingSystem.offAxisCameraRig;
        offAxisCamera = trackingSystem.offAxisCamera;
        trackingSource.TrackingDataUpdated += TrackingUpdated;

    }

    
    public void CalculateCalibrationFromTrackingInfo()
    {
        calibrationTransform.position = Vector3.zero;
        calibrationTransform.rotation = Quaternion.Euler(Vector3.zero);
        offAxisCamera.transform.localPosition = new Vector3(0, 0, 0);
        GameObject tempFace = new GameObject();

        tempFace.transform.eulerAngles = new Vector3(TrackedScreen.eulerAngles.x, TrackedScreen.eulerAngles.y, TrackedScreen.eulerAngles.z);
        tempFace.name = "tempFace";

        GameObject tempOffset = new GameObject();
        tempOffset.transform.parent = tempFace.transform;
        tempOffset.name = "tempOffset";

    
        //calibrationTransform.position = tempOffset.transform.position;
       // Debug.Log(calibrationTransform.position);
        calibrationTransform.rotation = tempOffset.transform.rotation;
        Debug.Log(calibrationTransform.rotation);
    }

    void TrackingUpdated(MPDepthTrackingData data)
    {
        this.trackingData = data;
    }


}
