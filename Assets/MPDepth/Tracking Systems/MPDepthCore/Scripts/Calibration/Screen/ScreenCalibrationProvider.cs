using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using MPDepthCore.Calibration.Camera;
using OffAxisCamera.ScreenConfiguration;
using UnityEngine;

namespace MPDepthCore.Calibration.Screen {
    
    public abstract class ScreenCalibrationProvider : OffAxisScreenProvider {

        public abstract SavedScreenCalibration CurrentCalibration { get; }

        public abstract List<SavedScreenCalibration> AllCalibrations { get; }
        
        public override Vector2 Dimensions => new Vector2(Width, Height);
        
        
        public abstract Task StartCalibration();

        void OnEnable() {
            if (File.Exists(FilePath)) {
                string json = File.ReadAllText(FilePath);
                LoadSelfFromJson(json);
            }
            FinishSetupAfterLoad();
            
            if (AllCalibrations.Count == 0) {
                SetCurrentToDefaultCalibration();
            }
        }
        public abstract void Calibrate();
        protected abstract void FinishSetupAfterLoad();

        protected abstract void SetCurrentToDefaultCalibration();

        protected abstract void LoadSelfFromJson(string json);
        protected abstract string GetSelfAsJson();
        
        void OnDisable() {
            Directory.CreateDirectory(BaseFolder);
            string json = GetSelfAsJson();
            File.WriteAllText(FilePath, json);
        }
        
        protected string FilePath => Path.Combine(BaseFolder, Filename);
        protected abstract string Filename { get; }

        protected string BaseFolder => Path.Combine(Application.persistentDataPath, "Save");


        public abstract void SelectCalibration(int selectedIndex);
        
        
        
    }
    
    public interface SavedScreenCalibration : ICalibration {
        
    }
}
