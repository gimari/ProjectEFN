using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AwesomeSnippets {

    public class ScriptTemplateSettings : ScriptableObject {
        [SerializeField] private DefaultAsset generateFolder = null;
        [SerializeField] private List<TextAsset> templates = new List<TextAsset> { null };
        public DefaultAsset GenerateFolder { get => generateFolder; set => generateFolder = value; }

        public string GenerateFolderPath {
            get => AssetDatabase.GetAssetPath(generateFolder);
        }

        public List<TextAsset> Templates { get => templates; set => templates = value; }
    }
}