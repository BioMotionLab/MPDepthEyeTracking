using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MPDepthCore.Calibration.Camera;
using UnityEngine;
using UnityEngine.Serialization;

namespace MPDepthCore.Calibration.Screen {
    public class BasicScreenCalibrationProvider : ScreenCalibrationProvider {

        [SerializeField] SavedBasicScreenCalibration currentCalibration;
        
        [SerializeField]
        List<SavedBasicScreenCalibration> savedCalibrations = new List<SavedBasicScreenCalibration>();

        [SerializeField] DetachedScreenCalibrator screenCalibrator;
        
        public override SavedScreenCalibration CurrentCalibration => currentCalibration;

        public override List<SavedScreenCalibration> AllCalibrations =>
            new List<SavedScreenCalibration>(savedCalibrations);

        public override async Task StartCalibration() {
            var oldCalibration = currentCalibration;
            currentCalibration = new SavedBasicScreenCalibration();
            bool success = await screenCalibrator.RunCalibrationProcedure(currentCalibration);
            if (success) {
                savedCalibrations.Add(currentCalibration);
            }
            else {
                currentCalibration = oldCalibration;
            }
            
        }

        public override void Calibrate()
        {
            screenCalibrator.Calibrate(currentCalibration);
        }

        public override float Width => currentCalibration.Width;

        public override float Height => currentCalibration.Height;
        
        protected override string Filename => $"BasicScreenCalibrationSave.json";
        public override void SelectCalibration(int selectedIndex) {
            currentCalibration = savedCalibrations[selectedIndex];
        }

        protected override void FinishSetupAfterLoad() {
            // no further setup
        }

        protected override void SetCurrentToDefaultCalibration() {
            currentCalibration = new SavedBasicScreenCalibration();
        }

        protected override void LoadSelfFromJson(string json) {
            try {
                SaveData loadedObject = JsonUtility.FromJson<SaveData>(json);
                this.savedCalibrations = loadedObject.savedCalibrations;
                this.currentCalibration = loadedObject.currentCalibration;
            }
            catch (ArgumentException e) {
                Debug.LogError($"error loading {nameof(BasicScreenCalibrationProvider)}, {e},{e.Message}");
            }
        }
        
        protected override string GetSelfAsJson() {
            SaveData saveData = new SaveData(savedCalibrations, currentCalibration);
            return JsonUtility.ToJson(saveData, true);
        }

        [ContextMenu("testsave")]
        public void TestSave() {
            savedCalibrations.Add(currentCalibration);
        }

        [Serializable]
        public class SavedBasicScreenCalibration : SavedScreenCalibration{
            
            [FormerlySerializedAs("width")] [SerializeField] public float Width = 0.8f;
            [SerializeField] public float Height = 0.45f;

            public string name = "Default calibration";
            public string Name {
                get => name;
                set => name = value;
            }
        }

        [Serializable]
        public class SaveData {
            
            public SavedBasicScreenCalibration currentCalibration;
            public List<SavedBasicScreenCalibration> savedCalibrations;

            public SaveData(List<SavedBasicScreenCalibration> savedCalibrations, SavedBasicScreenCalibration currentCalibration) {
                this.savedCalibrations = savedCalibrations;
                this.currentCalibration = currentCalibration;
            }
        }
        
    }
}