using UnityEditor;
using UnityEngine;

namespace MJ.EditorTools.Editor
{
    public class PersianKeyboardWindow : EditorWindow
    {
        private SerializedProperty _targetProperty;
        private SerializedObject _serializedObject;
        private Vector2 _scrollPos;

        private GUIStyle _keyStyle;
        private GUIStyle _previewStyle;
        private GUIStyle _inputStyle;

        private Color _bgColor = new Color(0.18f, 0.18f, 0.18f);
        private Color _keyColor = new Color(0.25f, 0.25f, 0.28f);
        private Color _actionKeyColor = new Color(0.2f, 0.6f, 0.8f);

        private const float KEYBOARD_AREA_HEIGHT = 230f;

        private readonly string[][] _rows = new string[][]
        {
            new string[] { "۱", "۲", "۳", "۴", "۵", "۶", "۷", "۸", "۹", "۰" },
            new string[] { "ض", "ص", "ث", "ق", "ف", "غ", "ع", "ه", "خ", "ح", "ج", "چ" },
            new string[] { "ش", "س", "ی", "ب", "ل", "ا", "ت", "ن", "م", "ک", "گ" },
            new string[] { "ظ", "ط", "ز", "ر", "ذ", "د", "پ", "و", "،", "." }
        };

        public static void Show(SerializedProperty property)
        {
            var window = GetWindow<PersianKeyboardWindow>(true, "Persian Keyboard", true);
            window._targetProperty = property;
            window._serializedObject = property.serializedObject;

            window.minSize = new Vector2(450, 350);
            window.ShowUtility();
        }

        private void OnEnable()
        {
            var config = PersianEditorConfig.Instance;
            Font customFont = (config != null) ? config.CustomFont : null;

            _previewStyle = new GUIStyle(EditorStyles.label);
            _previewStyle.fontSize = 20;
            _previewStyle.alignment = TextAnchor.UpperRight;
            _previewStyle.normal.textColor = new Color(0.6f, 1f, 0.6f);
            _previewStyle.wordWrap = true;
            _previewStyle.richText = true;
            _previewStyle.padding = new RectOffset(5, 5, 5, 5);
            if (customFont != null) _previewStyle.font = customFont;

            _inputStyle = new GUIStyle(EditorStyles.textField);
            _inputStyle.alignment = TextAnchor.UpperRight;
            _inputStyle.normal.textColor = Color.gray;
        }

        private void OnGUI()
        {
            EditorGUI.DrawRect(new Rect(0, 0, position.width, position.height), _bgColor);

            if (_targetProperty == null || _serializedObject == null)
            {
                EditorGUILayout.HelpBox("Target property lost. Please reopen.", MessageType.Warning);
                return;
            }

            _serializedObject.Update();
            string currentText = _targetProperty.stringValue;

            float availableTextHeight = position.height - KEYBOARD_AREA_HEIGHT - 10;

            GUILayout.BeginVertical();

            Rect textBgRect = new Rect(0, 0, position.width, availableTextHeight);
            EditorGUI.DrawRect(textBgRect, new Color(0.12f, 0.12f, 0.12f));

            _scrollPos = GUILayout.BeginScrollView(_scrollPos, GUILayout.Height(availableTextHeight));

            string fixedText = SimplePersianFixer.Fix(currentText);
            if (string.IsNullOrEmpty(fixedText)) fixedText = " ";

            GUIContent content = new GUIContent(fixedText);
            float minHeight = _previewStyle.CalcHeight(content, position.width - 30);

            GUILayout.Label(content, _previewStyle, GUILayout.ExpandWidth(true),
                GUILayout.MinHeight(Mathf.Max(minHeight, availableTextHeight)));

            GUILayout.EndScrollView();

            GUILayout.FlexibleSpace();

            GUILayout.BeginVertical(new GUIStyle { padding = new RectOffset(10, 10, 0, 10) });

            GUI.backgroundColor = new Color(1f, 1f, 1f, 0.1f);
            string newText = EditorGUILayout.TextField(currentText, _inputStyle);
            GUI.backgroundColor = Color.white;

            if (newText != currentText)
            {
                _targetProperty.stringValue = newText;
                ApplyChanges();
            }

            GUILayout.Space(10);

            InitKeyStyle();
            DrawKeyboard();

            GUILayout.EndVertical();
            GUILayout.EndVertical();
        }

        private void DrawKeyboard()
        {
            foreach (var row in _rows)
            {
                GUILayout.BeginHorizontal();
                foreach (var key in row)
                {
                    GUI.backgroundColor = _keyColor;
                    if (GUILayout.Button(key, _keyStyle, GUILayout.Height(35)))
                    {
                        InsertText(key);
                    }
                }

                GUILayout.EndHorizontal();
            }

            GUILayout.Space(5);

            GUILayout.BeginHorizontal();

            GUI.backgroundColor = _keyColor;
            if (GUILayout.Button("Space", _keyStyle, GUILayout.Height(35), GUILayout.ExpandWidth(true)))
            {
                InsertText(" ");
            }

            GUI.backgroundColor = new Color(0.8f, 0.3f, 0.3f);
            if (GUILayout.Button("⌫", _keyStyle, GUILayout.Width(60), GUILayout.Height(35)))
            {
                Backspace();
            }

            GUI.backgroundColor = _actionKeyColor;
            if (GUILayout.Button("Done", _keyStyle, GUILayout.Width(80), GUILayout.Height(35)))
            {
                Close();
            }

            GUILayout.EndHorizontal();
            GUI.backgroundColor = Color.white;
        }

        private void InsertText(string txt)
        {
            _targetProperty.stringValue += txt;
            ApplyChanges();
            GUI.FocusControl("");
            _scrollPos.y = float.MaxValue;
        }

        private void Backspace()
        {
            string current = _targetProperty.stringValue;
            if (!string.IsNullOrEmpty(current))
            {
                _targetProperty.stringValue = current.Substring(0, current.Length - 1);
                ApplyChanges();
                _scrollPos.y = float.MaxValue;
            }
        }

        private void ApplyChanges()
        {
            _serializedObject.ApplyModifiedProperties();
            Repaint();
        }

        private void InitKeyStyle()
        {
            if (_keyStyle == null)
            {
                _keyStyle = new GUIStyle(GUI.skin.button);
                _keyStyle.fontSize = 16;
                _keyStyle.fontStyle = FontStyle.Bold;
                _keyStyle.normal.textColor = Color.white;

                var config = PersianEditorConfig.Instance;
                if (config != null && config.CustomFont != null)
                    _keyStyle.font = config.CustomFont;
            }
        }
    }
}