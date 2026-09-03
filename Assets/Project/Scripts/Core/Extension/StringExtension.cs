using JetBrains.Annotations;

namespace Project.Scripts.Core.Extension
{
    public static class StringExtension
    {
        public static bool IsNullOrEmpty([CanBeNull] this string str)
        {
            return string.IsNullOrEmpty(str);
        }
    }
}
