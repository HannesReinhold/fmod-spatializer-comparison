﻿/*
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * All rights reserved.
 *
 * This source code is licensed under the license found in the
 * LICENSE file in the root directory of this source tree.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using Meta.Voice.TelemetryUtilities;
using UnityEditor;
using UnityEngine;

namespace Meta.WitAi.Windows
{
    public class WitWindow : WitConfigurationWindow
    {
        protected WitConfigurationEditor witInspector;
        protected string serverToken;
        protected override GUIContent Title => WitTexts.SettingsTitleContent;
        protected override string HeaderUrl => witInspector ? witInspector.HeaderUrl : base.HeaderUrl;

        // VLog log level
        private static int _logLevel = -1;
        private static string[] _logLevelNames;
        private static readonly VLogLevel[] _logLevels = (Enum.GetValues(typeof(VLogLevel)) as VLogLevel[])?.Reverse().ToArray();
        private string _newFilter;

#if VSDK_TELEMETRY_AVAILABLE
        private static int _telemetryLogLevel = -1;
        private static string[] _telemetryLogLevelNames;
        private static readonly TelemetryLogLevel[] _telemetryLogLevels = new TelemetryLogLevel[]
            { TelemetryLogLevel.Off, TelemetryLogLevel.Basic, TelemetryLogLevel.Verbose };
#endif
        public virtual bool ShowWitConfiguration => true;
        public virtual bool ShowGeneralSettings => true;

        public static bool ShowTooltips
        {
            get => EditorPrefs.GetBool("VSDK::Settings::Tooltips", true);
            set => EditorPrefs.SetBool("VSDK::Settings::Tooltips", value);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            if (string.IsNullOrEmpty(serverToken))
            {
                serverToken = WitAuthUtility.ServerToken;
            }
            RefreshLogLevel();
            InitializeTelemetryLevelOptions();
            SetWitEditor();
        }

        protected virtual void SetWitEditor()
        {
            // Destroy inspector
            if (witInspector != null)
            {
                DestroyImmediate(witInspector);
                witInspector = null;
            }
            // Generate new inspector & initialize immediately
            if (witConfiguration)
            {
                witInspector = (WitConfigurationEditor)UnityEditor.Editor.CreateEditor(witConfiguration);
                witInspector.drawHeader = false;
                witInspector.Initialize();
            }
        }

        protected override void LayoutContent()
        {
            if (ShowGeneralSettings) DrawGeneralSettings();
            if (ShowWitConfiguration) DrawWitConfigurations();
        }

        private void DrawGeneralSettings()
        {
            // VLog level
            bool updated = false;
            RefreshLogLevel();
            int logLevel = _logLevel;
            WitEditorUI.LayoutPopup(WitTexts.Texts.VLogLevelLabel, _logLevelNames, ref logLevel, ref updated);
            if (updated)
            {
                SetLogLevel(logLevel);
            }
            
            GUILayout.Label("Log Filters", EditorStyles.boldLabel);
            for (int i = 0; i < VLog.FilteredTags.Count; i++)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label(VLog.FilteredTags[i]);
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("X", GUILayout.Width(EditorGUIUtility.singleLineHeight)))
                {
                    VLog.RemoveTagFilter(VLog.FilteredTags[i]);
                }
                GUILayout.EndHorizontal();
            }

            GUILayout.BeginHorizontal();
            _newFilter = GUILayout.TextField(_newFilter);
            if (!string.IsNullOrEmpty(_newFilter) && GUILayout.Button("Add"))
            {
                VLog.AddTagFilter(_newFilter);
            }
            GUILayout.EndHorizontal();

            var showTooltips = ShowTooltips;
            WitEditorUI.LayoutToggle(new GUIContent(WitTexts.Texts.ShowTooltipsLabel), ref showTooltips, ref updated);
            if (updated)
            {
                ShowTooltips = showTooltips;
            }

#if VSDK_TELEMETRY_AVAILABLE && UNITY_EDITOR_WIN
            var enableTelemetry = TelemetryConsentManager.ConsentProvided;
            WitEditorUI.LayoutToggle(new GUIContent(WitTexts.Texts.TelemetryEnabledLabel), ref enableTelemetry, ref updated);
            if (updated)
            {
                TelemetryConsentManager.ConsentProvided = enableTelemetry;
            }

            var telemetryLogLevel = _telemetryLogLevel;
            WitEditorUI.LayoutPopup(WitTexts.Texts.TelemetryLevelLabel, _telemetryLogLevelNames, ref telemetryLogLevel, ref updated);
            if (updated)
            {
                _telemetryLogLevel = Math.Max(0, telemetryLogLevel);
                Telemetry.LogLevel = _telemetryLogLevels[_telemetryLogLevel];
            }
#endif
        }

        private void DrawWitConfigurations()
        {
            // Configuration select
            base.LayoutContent();
            // Update inspector if needed
            if (witInspector == null || witConfiguration == null || witInspector.Configuration != witConfiguration)
            {
                SetWitEditor();
            }

            // Layout configuration inspector
            if (witConfiguration && witInspector)
            {
                witInspector.OnInspectorGUI();
            }
        }
        
        private static void RefreshLogLevel()
        {
            if (_logLevelNames != null && _logLevelNames.Length == _logLevels.Length)
            {
                return;
            }
            List<string> logLevelOptions = new List<string>();
            foreach (var level in _logLevels)
            {
                logLevelOptions.Add(level.ToString());
            }
            _logLevelNames = logLevelOptions.ToArray();
            VLog.Init();
            _logLevel = logLevelOptions.IndexOf(VLog.EditorLogLevel.ToString());
        }
        private void SetLogLevel(int newLevel)
        {
            _logLevel = Mathf.Clamp(0, newLevel, _logLevels.Length);
            VLog.EditorLogLevel = _logLevels[_logLevel];
        }

        private static void InitializeTelemetryLevelOptions()
        {
#if VSDK_TELEMETRY_AVAILABLE
            _telemetryLogLevelNames = new string [_telemetryLogLevels.Length];
            for (int i = 0; i < _telemetryLogLevelNames.Length; ++i)
            {
                _telemetryLogLevelNames[i] = _telemetryLogLevels[i].ToString();
            }

            var currentLevel = Telemetry.LogLevel.ToString();
            for (int i = 0; i < _telemetryLogLevelNames.Length; ++i)
            {
                if (_telemetryLogLevelNames[i] == currentLevel)
                {
                    _telemetryLogLevel = i;
                    return;
                }
            }
#endif
        }
    }
}
