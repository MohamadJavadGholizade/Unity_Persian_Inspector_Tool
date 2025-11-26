using UnityEngine;

namespace MJ.EditorTools
{
    /// <summary>
    /// Use this attribute on string fields to render them with proper Persian/Arabic RTL shaping in the Inspector.
    /// <para>Supports multi-line text, custom fonts, and a virtual keyboard.</para>
    /// </summary>
    public class PersianTextAttribute : PropertyAttribute
    {
        /// <summary>
        /// Minimum number of lines for the text area height.
        /// </summary>
        public readonly int MinLines;

        /// <summary>
        /// Maximum number of lines before the text area stops growing (if content is huge).
        /// </summary>
        public readonly int MaxLines;

        public readonly int FontSize;
        
        /// <summary>
        /// Renders the string field with Persian shaping logic.
        /// </summary>
        /// <param name="minLines">Minimum height in lines (Default: 3)</param>
        /// <param name="maxLines">Maximum height in lines (Default: 10)</param>
        /// <param name="fontSize">Font Size (Default: 12)</param>
        public PersianTextAttribute(int minLines = 3, int maxLines = 10, int fontSize = 12)
        {
            MinLines = minLines;
            MaxLines = maxLines;
            FontSize = fontSize;
        }
        
        

    }
}