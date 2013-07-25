using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AttributeRouting.Helpers;

namespace AKQ.Domain.Utils
{
    public static class StringExtensions
    {
       public static string HasValueOr(this string source, string anotherValue)
       {
           return source.HasValue() ? source : anotherValue;
       }
    }
}