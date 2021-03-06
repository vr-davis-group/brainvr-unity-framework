﻿using System.Collections.Generic;
using System.IO;
using Assets.GeneralScripts.Serialisation;
using BrainVR.UnityFramework.Experiment;
using BrainVR.UnityLogger;

namespace BrainVR.UnityFramework.DataHolders
{
    public class SettingsHolder : Singleton<SettingsHolder>
    {
        public ExperimentInfo ExperimentInfo;
        public List<ExperimentSettings> ExperimentSettings = new List<ExperimentSettings>();
        public List<string> ExperimentSettingsFilenames = new List<string>();
        public string LevelName;

        private int _currentExperiment;
        public int CurrentExperiment
        {
            get { return _currentExperiment; }
            set { _currentExperiment = value >= 0 && value < ExperimentSettings.Count? value : 0; }
        }
        public void AddExperimentSettings(string path)
        {
            var text = File.ReadAllText(path);
            var obj = NestedDeserialiser.Deserialize(text);
            var dict = (Dictionary<string, object>) obj;
            var expSettings = ExperimentLoader.PopulateExperimentSettings(dict["ExperimentName"].ToString(), path);
            expSettings.LevelName = dict["LevelName"].ToString();
            expSettings.ExperimentName = dict["ExperimentName"].ToString();
            ExperimentSettings.Add(expSettings);
            var filename = Path.GetFileName(path);
            ExperimentSettingsFilenames.Add(filename);
        }
        public ExperimentSettings CurrentExperimentSettings()
        {
            return ExperimentSettings.Count <= _currentExperiment ? null : ExperimentSettings[_currentExperiment];
        }
        public void SetSettings(int i)
        {
            CurrentExperiment = i;
        }
    }
}
