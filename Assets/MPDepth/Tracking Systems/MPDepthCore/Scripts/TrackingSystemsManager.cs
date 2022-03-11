using System;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using MPDepthCore.Calibration.Camera;
using MPDepthCore.TrackingSources;
using OffAxisCamera;
using UnityEngine;

namespace MPDepthCore
{
    public class TrackingSystemsManager : MonoBehaviour
    {
        int currentTrackingSystemIndex = 0;
        public static TrackingSystemsManager instance;

        [SerializeField] OffAxisCameraRig offAxisCameraRig = default;

        [SerializeField] TrackingSystem currentTrackingSystem;

        [SerializeField] List<TrackingSystem> trackingSystems = new List<TrackingSystem>();

        [SerializeField] ToggleUserEvent parallaxToggleEvent;

        [SerializeField] public CalibratedTrackingData CurrentCalibratedTrackingData;

        public TrackingSystem CurrentTrackingSystem => currentTrackingSystem;

        public event MPDepthTrackingSource.TrackingDataUpdatedEvent TrackingDataUpdated;

        public List<TrackingSystem> TrackingSystems => trackingSystems;


        void OnEnable()
        {
            foreach (TrackingSystem trackingSystem in trackingSystems)
            {
                trackingSystem.TurnOff();
            }

            TrackingSystemProviderSave.Load(this);
            instance = this;



            if (parallaxToggleEvent != null)
            {
                parallaxToggleEvent.Toggled += ToggleParallax;
            }

            ChangeSystemTo(currentTrackingSystem);
        }

        void ToggleParallax(bool parallaxIsOn)
        {
            if (parallaxIsOn) offAxisCameraRig.EnableCameraTracking();
            else offAxisCameraRig.DisableCameraTracking();
        }



        void TrackingDataWasUpdated(MPDepthTrackingData data)
        {
            TrackingDataUpdated?.Invoke(data);
            offAxisCameraRig.UpdateCameraLocation(data.CameraTrackingData.Position);
        }
        public void StartCalibration()
        {
            currentTrackingSystem.TrackingCalibrationProvider.Calibrate();
            currentTrackingSystem.ScreenCalibrationProvider.Calibrate();

        }

        public void StartScreenCalibration()
        {
            currentTrackingSystem.ScreenCalibrationProvider.Calibrate();
        }

        void OnDisable()
        {
            TrackingSystemProviderSave.Save(this);
            currentTrackingSystem.TrackingDataUpdated -= TrackingDataWasUpdated;

            if (parallaxToggleEvent != null)
            {
                parallaxToggleEvent.Toggled -= ToggleParallax;
            }
        }


        public void SelectSystem(int selectedIndex)
        {
            TrackingSystem newTrackingSystem;
            try
            {
                newTrackingSystem = trackingSystems[selectedIndex];
            }
            catch (IndexOutOfRangeException)
            {
                if (TrackingSystems.Count == 0)
                {
                    Debug.LogError("No Tracking Systems defined!");
                    throw;
                }

                Debug.LogWarning($"Saved Tracking System index out of range {selectedIndex}, reverting to default.");
                newTrackingSystem = trackingSystems[0];

            }
            ChangeSystemTo(newTrackingSystem);
        }

        public void ChangeSystemTo(TrackingSystem newTrackingSystem)
        {

            if (currentTrackingSystem != null)
            {
                currentTrackingSystem.TrackingDataUpdated -= TrackingDataWasUpdated;
                currentTrackingSystem.TurnOff();
            }

            currentTrackingSystem = newTrackingSystem;
            offAxisCameraRig.Screen = currentTrackingSystem.ScreenCalibrationProvider;

            currentTrackingSystem.TurnOn();
            currentTrackingSystem.TrackingDataUpdated += TrackingDataWasUpdated;
        }

        public void CycleTrackingSystem()
        {
            //currentTrackingSystem.TurnOff();
            for(int i = 0; i < trackingSystems.Count; i++)
            {
                if(trackingSystems[i] == currentTrackingSystem)
                {
                    currentTrackingSystemIndex = i;
                }
                //trackingSystems[i].TurnOff();
            }
            if(currentTrackingSystemIndex < trackingSystems.Count-1)
                {
                ChangeSystemTo(trackingSystems[currentTrackingSystemIndex + 1]);
             
                }
            else
            {
                ChangeSystemTo(trackingSystems[0]);
;
            }
        }




        [Serializable]
        public class TrackingSystemProviderSave
        {

            [SerializeField]
            int selectedIndexOfTrackingSystem = 0;

            public static void Save(TrackingSystemsManager provider)
            {

                Directory.CreateDirectory(FileFolder);

                var saveData = new TrackingSystemProviderSave();
                saveData.selectedIndexOfTrackingSystem = provider.trackingSystems.IndexOf(provider.currentTrackingSystem);
                string json = JsonUtility.ToJson(saveData);
                File.WriteAllText(FilePath, json);
            }

            static string Filename => "trackingSystemProviderSave.json";
            static string FilePath => Path.Combine(FileFolder, Filename);

            static string FileFolder => Path.Combine(Application.persistentDataPath, "Save");
            public static void Load(TrackingSystemsManager trackingSystemsManager)
            {
                if (File.Exists(FilePath))
                {
                    string json = File.ReadAllText(FilePath);
                    var loadedSaveData = JsonUtility.FromJson<TrackingSystemProviderSave>(json);
                    trackingSystemsManager.SelectSystem(loadedSaveData.selectedIndexOfTrackingSystem);
                }
            }

        }

    }
}
