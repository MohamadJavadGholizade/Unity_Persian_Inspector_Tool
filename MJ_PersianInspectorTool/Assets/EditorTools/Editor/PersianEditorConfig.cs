using UnityEngine;
using UnityEditor;

namespace MJ.EditorTools.Editor
{
    public class PersianEditorConfig : ScriptableObject
    {
        [Header("Global Persian Settings")]
        [Tooltip("Drop your font here")]
        public Font CustomFont;

        [Range(10, 20)]
        public int FontSize = 14;

        [Header("Colors")]
        public Color PreviewTextColor = new Color(1f, 1f, 1f); 
        public Color PreviewBackgroundColor = new Color(0.15f, 0.15f, 0.15f);

        private static PersianEditorConfig _instance;
        public static PersianEditorConfig Instance
        {
            get
            {
                if (_instance == null)
                {
                    string[] guids = AssetDatabase.FindAssets("t:PersianEditorConfig");
                    if (guids.Length > 0)
                    {
                        string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                        _instance = AssetDatabase.LoadAssetAtPath<PersianEditorConfig>(path);
                    }
                }
                return _instance;
            }
        }
    }

    public static class PersianConfigMenu
    {
        [MenuItem("Tools/MJ Tools/Persian Settings")]
        public static void SelectConfig()
        {
            var config = PersianEditorConfig.Instance;
            if (config == null)
            {
                config = ScriptableObject.CreateInstance<PersianEditorConfig>();
                if (!AssetDatabase.IsValidFolder("Assets/Resources"))
                    AssetDatabase.CreateFolder("Assets", "Resources");
                
                AssetDatabase.CreateAsset(config, "Assets/Resources/PersianEditorConfig.asset");
                AssetDatabase.SaveAssets();
            }
            Selection.activeObject = config;
        }
    }
}