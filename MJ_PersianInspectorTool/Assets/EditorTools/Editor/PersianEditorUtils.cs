namespace MJ.EditorTools.Editor
{
    /// <summary>
    /// Utility class for handling Persian text rendering within the Unity Editor (Inspectors, Windows).
    /// </summary>
    public static class PersianEditorUtils
    {
        /// <summary>
        /// Fixes and reshapes Persian/Arabic text for correct display in Unity's standard Inspector fields.
        /// <para>
        /// Use this method when drawing custom editors or property drawers to ensure RTL text is readable.
        /// </para>
        /// </summary>
        /// <param name="text">The raw input text (e.g., from a SerializedProperty).</param>
        /// <returns>The shaped and reversed string ready for EditorGUI rendering.</returns>
        public static string FixForInspector(string text)
        {
            // Directly use our internal standalone fixer (No dependencies)
            return SimplePersianFixer.Fix(text);
        }
    }
}