using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AwesomeSnippets {

    [InitializeOnLoad]
    public class MultipleTemplatesWindow : EditorWindow {
        private ScriptTemplateSettings settings;

        private Vector2 scroll;

        private TextAsset previewTextAsset;

        public ScriptTemplateSettings Settings {
            get {
                if (settings == null) {
                    string[] vs = AssetDatabase.FindAssets("t:ScriptTemplateSettings");

                    if (vs.Length > 0) {
                        settings = AssetDatabase.LoadAssetAtPath<ScriptTemplateSettings>(AssetDatabase.GUIDToAssetPath(vs[0]));
                    } else {
                        AssetDatabase.CreateAsset(CreateInstance<ScriptTemplateSettings>(), "Assets/Script Template Settings.asset");
                        AssetDatabase.SaveAssets();

                        settings = AssetDatabase.LoadAssetAtPath<ScriptTemplateSettings>("Assets/Script Template Settings.asset");
                    }
                }

                return settings;
            }
        }

        [MenuItem("AwesomeSnippets/Script Templates")]
        public static void ShowWindow() {
            GetWindow<MultipleTemplatesWindow>(false, "Script Templates");
        }

        private void DeleteGeneratedScript() {
            string[] vs = AssetDatabase.FindAssets(MultipleTemplatesMenuItemGenerator.Name_Generate_Class);

            foreach (string foundAssetGUID in vs) {
                AssetDatabase.DeleteAsset(AssetDatabase.GUIDToAssetPath(foundAssetGUID));
            }
        }

        private void ButtonSaveAndUpdate() {
            EditorGUILayout.Space(50);

            Vector2 btnSize = new Vector2(180, 30);
            Rect rect = new Rect(
                new Vector2(EditorGUIUtility.currentViewWidth - btnSize.x - 10, 10),
                btnSize
            );

            GUILayout.BeginArea(rect);
            GUILayoutOption[] options = {
                GUILayout.Height(btnSize.y),
                GUILayout.Width(btnSize.x)
            };
            if (GUILayout.Button("Save & Update", options)) {
                EditorUtility.SetDirty(Settings);
                AssetDatabase.SaveAssets();

                DeleteGeneratedScript();
                MultipleTemplatesMenuItemGenerator.GenerateMenuItemScript(Settings);
            }
            GUILayout.EndArea();
        }

        private void OnGUI() {
            minSize = new Vector2(320, 300);

            EditorGUILayout.BeginVertical();
            {
                ButtonSaveAndUpdate();

                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.BeginVertical(GUILayout.Width(320));
                    {
                        SelectTargetFolder();

                        EditorGUILayout.Space(30);

                        OnGUI_SelectScript();
                    }
                    EditorGUILayout.EndVertical();

                    OnGUI_ScriptPreview();
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
        }

        private void SelectTargetFolder() {
            GUILayout.Label("Auto Generate Script Folder", EditorStyles.boldLabel);

            EditorGUILayout.Space(3);

            Settings.GenerateFolder = (DefaultAsset)EditorGUILayout.ObjectField(Settings.GenerateFolder, typeof(DefaultAsset), false);

            if (Settings.GenerateFolder == null) {
                EditorGUILayout.HelpBox(
                    "Folder named 'editor' must be included in parent folders\nAssets/Editor will be used if none selected",
                    MessageType.Info,
                    true);
            }
        }

        private void OnGUI_SelectScript() {
            GUILayout.Label("Templates", EditorStyles.boldLabel);

            for (int loop = 0; loop < Settings.Templates.Count; loop++) {
                EditorGUILayout.BeginHorizontal();

                TextAsset taBefore = Settings.Templates[loop];
                Settings.Templates[loop] = (TextAsset)EditorGUILayout.ObjectField(Settings.Templates[loop], typeof(TextAsset), false);
                if (taBefore != Settings.Templates[loop]) {
                    if (HasDuplicate(Settings.Templates, loop)) {
                        Settings.Templates[loop] = null;
                    } else {
                        previewTextAsset = Settings.Templates[loop];
                    }
                }

                GUILayout.Space(5);

                if (GUILayout.Button(EditorGUIUtility.FindTexture("ViewToolOrbit"), GUILayout.Width(30))) {
                    previewTextAsset = Settings.Templates[loop];
                }
                if (GUILayout.Button(EditorGUIUtility.FindTexture("Toolbar Minus"), GUILayout.Width(30))) {
                    if (previewTextAsset == Settings.Templates[loop]) {
                        previewTextAsset = null;
                    }
                    Settings.Templates.RemoveAt(loop);
                }

                EditorGUILayout.EndHorizontal();
            }

            if (GUILayout.Button("Add Template")) {
                Settings.Templates.Add(null);
            }
        }

        private void OnGUI_ScriptPreview() {
            EditorGUILayout.BeginVertical();

            GUIStyle style = new GUIStyle(EditorStyles.textArea);
            style.wordWrap = true;
            style.stretchHeight = true;

            string strPreviewName = "";
            string strPreview = "";
            if (previewTextAsset != null) {
                strPreview = previewTextAsset.text;
                strPreviewName = previewTextAsset.name;
            }

            GUILayout.Label(strPreviewName, EditorStyles.boldLabel);
            scroll = EditorGUILayout.BeginScrollView(scroll);

            GUILayout.Label(strPreview, style);

            EditorGUILayout.EndScrollView();

            EditorGUILayout.EndVertical();
        }

        private bool HasDuplicate(List<TextAsset> textAssets, int idx) {
            for (int loop = 0; loop < textAssets.Count; loop++) {
                if (loop == idx) {
                    continue;
                }

                if (textAssets[loop] == textAssets[idx]) {
                    return true;
                }
            }

            return false;
        }
    }
}