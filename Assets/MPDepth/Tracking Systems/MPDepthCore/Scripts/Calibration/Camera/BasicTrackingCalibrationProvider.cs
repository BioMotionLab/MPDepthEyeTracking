using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

namespace MPDepthCore.Calibration.Camera {
    public class BasicTrackingCalibrationProvider : TrackingCalibrationProvider {

        public override SavedTrackerCalibration CurrentTrackerCalibration => currentTrackerCalibration;
        public override List<SavedTrackerCalibration> AllCalibrations {
            get {
                List<SavedTrackerCalibration> allCalibrations = new List<SavedTrackerCalibration>();
                allCalibrations.AddRange(savedCalibrations);
                return allCalibrations;
            }
        }
        
        public override TrackerOffsetCalibration GetTrackerOffsetCalibration => currentTrackerCalibration.OffsetCalibration;
        
        [SerializeField] SavedTrackerBasicCalibration currentTrackerCalibration;
        
        [SerializeField]
        List<SavedTrackerBasicCalibration> savedCalibrations = new List<SavedTrackerBasicCalibration>();

       
        public override Task StartCalibration() {
            Debug.LogWarning($"This Tracking System Currently Requires Manual Calibration");
            return Task.CompletedTask;
        }

        
        protected override string Filename => $"BasicCalibrationProviderSave.json";
        public override void SelectCalibration(int selectedIndex) {
            currentTrackerCalibration = savedCalibrations[selectedIndex];
        }


        protected override void FinishSetupAfterLoad() {
            // no more setup
        }

        protected override void SetCurrentToDefaultCalibration() {
            currentTrackerCalibration = new SavedTrackerBasicCalibration();
        }

        protected override void LoadSelfFromJson(string json) {
            JsonUtility.FromJsonOverwrite(json, this);
        }
        
        protected override string GetSelfAsJson() {
            return JsonUtility.ToJson(this);
        }

        [ContextMenu("testsave")]
        public void TestSave() {
            savedCalibrations.Add(currentTrackerCalibration);
        }

        public override void Calibrate()
        {
            calibrationTransform.position = Vector3.zero;
            calibrationTransform.rotation = Quaternion.Euler(Vector3.zero);
            calibrationTransform.position = currentTrackerCalibration.OffsetCalibration.Position;
        }

        [Serializable]
        public class SavedTrackerBasicCalibration : SavedTrackerCalibration {
            
            public TrackerOffsetCalibration OffsetCalibration = new TrackerOffsetCalibration();
            public string name = "Default calibration";
            public string Name {
                get => name;
                set => name = value;
            }
        }
    }
}