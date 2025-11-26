using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MJ.EditorTools.Editor
{
    [CustomPropertyDrawer(typeof(PersianTextAttribute))]
    public class PersianTextDrawer : PropertyDrawer
    {
        private static string _activePropertyPath = "";

        private const float OUTER_PADDING = 2f;
        private const float INNER_PADDING = 4f;
        private const float STATS_HEIGHT = 14f;
        private const float SPACING = 10f;

        private const float EXTRA_LINE_SPACING = 4f;

        private const float BUTTON_SIZE = 22f;
        private const float BUTTON_SPACING = 2f;

        private static Font _cachedFont;
        private static GUIStyle _cachedPreviewStyle;
        private static GUIStyle _cachedStatsStyle;

        private void UpdateStyles(int overrideFontSize)
        {
            var config = PersianEditorConfig.Instance;

            if (_cachedPreviewStyle == null || (config != null && config.CustomFont != _cachedFont))
            {
                _cachedPreviewStyle = new GUIStyle(EditorStyles.label)
                {
                    alignment = TextAnchor.UpperRight,
                    richText = true,
                    wordWrap = false,
                    fontStyle = FontStyle.Bold,
                    padding = new RectOffset(0, 5, 5, 0),
                    margin = new RectOffset(0, 0, 0, 0),
                    clipping = TextClipping.Overflow
                };

                if (config != null)
                {
                    _cachedFont = config.CustomFont;
                    if (_cachedFont != null) _cachedPreviewStyle.font = _cachedFont;
                    _cachedPreviewStyle.normal.textColor = config.PreviewTextColor;
                }
            }

            int finalSize = 12;
            if (overrideFontSize > 0) finalSize = overrideFontSize;
            else if (config != null) finalSize = config.FontSize;

            if (_cachedPreviewStyle.fontSize != finalSize)
                _cachedPreviewStyle.fontSize = finalSize;

            if (_cachedStatsStyle == null)
            {
                _cachedStatsStyle = new GUIStyle(EditorStyles.miniLabel)
                {
                    alignment = TextAnchor.MiddleRight,
                    fontSize = 9,
                    normal = { textColor = new Color(0.5f, 0.5f, 0.5f) }
                };
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            PersianTextAttribute attr = (PersianTextAttribute)attribute;
            UpdateStyles(attr.FontSize);

            bool isEditing = (_activePropertyPath == property.propertyPath);
            float width = EditorGUIUtility.currentViewWidth - 40f;
            float innerWidth = width - (INNER_PADDING * 2);

            float labelHeight = EditorGUIUtility.singleLineHeight;
            float rawHeight = 0f;
            float previewHeight = 0f;

            string rawText = property.stringValue;

            previewHeight = CalculateVisualHeight(rawText, innerWidth);

            if (isEditing)
            {
                float buttonsWidth = (BUTTON_SIZE * 2) + (BUTTON_SPACING * 3);
                float textAreaWidth = innerWidth - buttonsWidth;

                rawHeight = EditorStyles.textArea.CalcHeight(new GUIContent(rawText), textAreaWidth);
                rawHeight = Mathf.Max(rawHeight, EditorGUIUtility.singleLineHeight * 2);

                return labelHeight + OUTER_PADDING + INNER_PADDING + rawHeight + SPACING + previewHeight + SPACING +
                       STATS_HEIGHT + INNER_PADDING + OUTER_PADDING;
            }
            else
            {
                previewHeight = Mathf.Max(previewHeight, EditorGUIUtility.singleLineHeight + 2);
                return labelHeight + OUTER_PADDING + INNER_PADDING + previewHeight + SPACING + STATS_HEIGHT +
                       INNER_PADDING + OUTER_PADDING;
            }
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            PersianTextAttribute attr = (PersianTextAttribute)attribute;
            UpdateStyles(attr.FontSize);

            EditorGUI.BeginProperty(position, label, property);

            Rect labelRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.LabelField(labelRect, label);

            Rect boxRect = new Rect(
                position.x,
                position.y + EditorGUIUtility.singleLineHeight + OUTER_PADDING,
                position.width,
                position.height - EditorGUIUtility.singleLineHeight - (OUTER_PADDING * 2)
            );

            GUI.Box(boxRect, "", EditorStyles.helpBox);

            Rect innerRect = new Rect(
                boxRect.x + INNER_PADDING,
                boxRect.y + INNER_PADDING,
                boxRect.width - (INNER_PADDING * 2),
                boxRect.height - (INNER_PADDING * 2)
            );

            bool isEditing = (_activePropertyPath == property.propertyPath);
            string rawValue = property.stringValue;

            if (isEditing)
            {
                float buttonsTotalWidth = (BUTTON_SIZE * 2) + (BUTTON_SPACING * 2);
                float textAreaWidth = innerRect.width - buttonsTotalWidth;

                GUIStyle rawStyle = new GUIStyle(EditorStyles.textArea)
                    { alignment = TextAnchor.UpperRight, wordWrap = true };
                float rawHeight = rawStyle.CalcHeight(new GUIContent(rawValue), textAreaWidth);
                rawHeight = Mathf.Max(rawHeight, EditorGUIUtility.singleLineHeight * 2);

                Rect rawRect = new Rect(innerRect.x, innerRect.y, textAreaWidth, rawHeight);
                Rect kbdRect = new Rect(rawRect.xMax + BUTTON_SPACING, rawRect.y, BUTTON_SIZE, BUTTON_SIZE);
                Rect doneRect = new Rect(kbdRect.xMax + BUTTON_SPACING, rawRect.y, BUTTON_SIZE, BUTTON_SIZE);

                GUI.SetNextControlName(property.propertyPath);
                EditorGUI.BeginChangeCheck();
                string newValue = EditorGUI.TextArea(rawRect, rawValue, rawStyle);
                if (EditorGUI.EndChangeCheck()) property.stringValue = newValue;

                GUIContent kbdIcon = EditorGUIUtility.IconContent("d_Keyboard");
                if (kbdIcon == null || kbdIcon.image == null) kbdIcon = new GUIContent("K");
                if (GUI.Button(kbdRect, kbdIcon, EditorStyles.miniButtonLeft)) PersianKeyboardWindow.Show(property);

                GUI.backgroundColor = new Color(0.7f, 1f, 0.7f);
                GUIContent checkIcon = EditorGUIUtility.IconContent("vcs_check");
                if (checkIcon == null || checkIcon.image == null) checkIcon = new GUIContent("✔");
                if (GUI.Button(doneRect, checkIcon, EditorStyles.miniButtonRight))
                {
                    _activePropertyPath = "";
                    GUI.FocusControl("");
                    Event.current.Use();
                }

                GUI.backgroundColor = Color.white;

                Rect lineRect = new Rect(boxRect.x + 2, rawRect.yMax + (SPACING / 2), boxRect.width - 4, 1);
                EditorGUI.DrawRect(lineRect, new Color(0.5f, 0.5f, 0.5f, 0.2f));

                Rect previewRect = new Rect(innerRect.x, rawRect.yMax + SPACING, innerRect.width,
                    innerRect.height - rawHeight - SPACING - STATS_HEIGHT);

                DrawVisualText(previewRect, newValue);

                if (Event.current.type == EventType.MouseDown && !boxRect.Contains(Event.current.mousePosition))
                {
                    _activePropertyPath = "";
                    GUI.FocusControl("");
                    Event.current.Use();
                }
            }
            else
            {
                Rect viewRect = new Rect(innerRect.x, innerRect.y, innerRect.width, innerRect.height - STATS_HEIGHT);

                if (GUI.Button(boxRect, "", GUIStyle.none))
                {
                    _activePropertyPath = property.propertyPath;
                    GUI.FocusControl(property.propertyPath);
                }

                DrawVisualText(viewRect, rawValue);

                Rect iconRect = new Rect(boxRect.x + 4, boxRect.yMax - 14, 12, 12);
                EditorGUI.LabelField(iconRect, EditorGUIUtility.IconContent("d_editicon.sml"));
            }

            DrawStatsBar(boxRect, rawValue);
            EditorGUI.EndProperty();
        }

        private float GetLineHeight()
        {
            return (_cachedPreviewStyle.fontSize * 1.15f) + EXTRA_LINE_SPACING;
        }

        private float CalculateVisualHeight(string rawText, float width)
        {
            if (string.IsNullOrEmpty(rawText)) return 0;
            List<string> visualLines = GetVisualLines(rawText, width);

            return visualLines.Count * GetLineHeight();
        }

        private void DrawVisualText(Rect rect, string rawText)
        {
            if (string.IsNullOrEmpty(rawText))
            {
                EditorGUI.LabelField(rect, "<color=grey>Empty...</color>", _cachedPreviewStyle);
                return;
            }

            List<string> visualLines = GetVisualLines(rawText, rect.width);

            float currentY = rect.y;
            Color originalColor = _cachedPreviewStyle.normal.textColor;
            if (_activePropertyPath != "") _cachedPreviewStyle.normal.textColor = new Color(0.6f, 1f, 0.6f);

            float lineHeight = GetLineHeight();

            foreach (var line in visualLines)
            {
                string fixedLine = SimplePersianFixer.Fix(line);

                Rect lineRect = new Rect(rect.x, currentY, rect.width, lineHeight);

                EditorGUI.LabelField(lineRect, fixedLine, _cachedPreviewStyle);

                currentY += lineHeight;
            }

            _cachedPreviewStyle.normal.textColor = originalColor;
        }

        private List<string> GetVisualLines(string text, float width)
        {
            List<string> lines = new List<string>();
            string[] paragraphs = text.Split('\n');

            foreach (var paragraph in paragraphs)
            {
                if (string.IsNullOrEmpty(paragraph))
                {
                    lines.Add("");
                    continue;
                }

                string[] words = paragraph.Split(' ');
                string currentLine = "";

                foreach (var word in words)
                {
                    string testLine = string.IsNullOrEmpty(currentLine) ? word : currentLine + " " + word;

                    float w = _cachedPreviewStyle.CalcSize(new GUIContent(testLine)).x;

                    if (w < width)
                    {
                        currentLine = testLine;
                    }
                    else
                    {
                        lines.Add(currentLine);
                        currentLine = word;
                    }
                }

                if (!string.IsNullOrEmpty(currentLine)) lines.Add(currentLine);
            }

            return lines;
        }

        private void DrawStatsBar(Rect boxRect, string text)
        {
            Rect statsRect = new Rect(boxRect.x + INNER_PADDING, boxRect.yMax - STATS_HEIGHT - 1,
                boxRect.width - (INNER_PADDING * 2), STATS_HEIGHT);
            int charCount = text.Length;
            int wordCount = string.IsNullOrEmpty(text)
                ? 0
                : text.Split(new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries).Length;
            string stats = $"{charCount} :Char  |  {wordCount} :Word";
            EditorGUI.LabelField(statsRect, stats, _cachedStatsStyle);
        }
    }
}