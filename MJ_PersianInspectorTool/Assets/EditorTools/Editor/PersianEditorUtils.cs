using RTLTMPro;

namespace MJ.EditorTools.Editor
{
    public static class PersianEditorUtils
    {
        private static FastStringBuilder _outputBuffer = new FastStringBuilder(RTLSupport.DefaultBufferSize);

        public static string FixForInspector(string text)
        {
            if (string.IsNullOrEmpty(text)) return "";

            _outputBuffer.Clear();

            RTLSupport.FixRTL(text, _outputBuffer, true, true, false);

            return _outputBuffer.ToString();
        }
    }
}