
using PAQK.Platform.Extensions;

namespace PAQK.Utils
{
    public static class StringExtensions
    {
       public static string HasValueOr(this string source, string anotherValue)
       {
           return source.HasValue() ? source : anotherValue;
       }
    }
}