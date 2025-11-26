using RTLTMPro;

namespace MJ.EditorTools
{
    public static class PersianStringUtils
    {
        private static readonly FastStringBuilder OutputBuffer = new FastStringBuilder(RTLSupport.DefaultBufferSize);

        public static string FixPersian(string text)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;

            OutputBuffer.Clear();
            
            RTLSupport.FixRTL(text, OutputBuffer, true, true, false);
            
            return OutputBuffer.ToString();
        }
    }
}