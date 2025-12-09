namespace MJ.EditorTools
{
    /// <summary>
    /// A runtime utility class for fixing Persian/Arabic text in game components (e.g., UI, 3D Text).
    /// </summary>
    public static class PersianStringUtils
    {
        /// <summary>
        /// Converts a raw Persian string into a properly shaped and reversed format for RTL display.
        /// <para>
        /// Useful for legacy UI Text, 3D TextMesh, or any renderer that does not support RTL natively.
        /// </para>
        /// </summary>
        /// <param name="text">The raw Persian text string.</param>
        /// <returns>The corrected string ready for UI display.</returns>
        /// <example>
        /// <code>
        /// myTextComponent.text = PersianStringUtils.FixPersian("سلام دنیا");
        /// </code>
        /// </example>
        public static string FixPersian(string text)
        {
            // Directly use our internal standalone fixer
            return SimplePersianFixer.Fix(text);
        }
    }
}