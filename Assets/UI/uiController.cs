using System.Collections;
using System.Collections.Generic;
using StreamTrackingSystem;
using System.Net;
using UnityEngine.UI;
using UnityEngine;

public class uiController : MonoBehaviour
{
    [SerializeField] InputField deviceStreamInputField;
    [SerializeField] InputField streamCalibrationInputField;
    [SerializeField] StreamTrackingDataReceiver streamTrackingDataReceiver;
    [SerializeField] GameObject deviceStreamCanvas;
    [SerializeField] GameObject streamCalibrationCanvas;
    [SerializeField] StreamTrackingCalibrator streamTrackingCalibrator;
    public void SubmitDeviceStreamIP()
    {

        streamTrackingDataReceiver.SetIP(IPAddress.Parse(deviceStreamInputField.text));
        StartCoroutine(ResetReceiver());
        deviceStreamCanvas.SetActive(false);
    }

    public void CancelDeviceStreamIP()
    {
        deviceStreamInputField.text = "";
        deviceStreamCanvas.SetActive(false);
    }

    public void ShowDeviceStreamUI()
    {
        deviceStreamCanvas.SetActive(true);
    }

    IEnumerator ResetReceiver()
    {
        streamTrackingDataReceiver.enabled = false;
        yield return new WaitForSeconds(1);
        streamTrackingDataReceiver.enabled = true;
    }

    public void SetDistanceToScreen()
    {
        streamTrackingCalibrator.manualCalibrationDistance = float.Parse(streamCalibrationInputField.text);
        streamTrackingCalibrator.CalculateCalibrationFromTrackingInfo();
        streamCalibrationCanvas.SetActive(false);
    }

    public void CancelStreamCalibration()
    {
        streamCalibrationInputField.text = "";
        streamCalibrationCanvas.SetActive(false);
    }



}
