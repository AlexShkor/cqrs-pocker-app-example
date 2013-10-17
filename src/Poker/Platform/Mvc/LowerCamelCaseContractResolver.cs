using System;
using Newtonsoft.Json.Serialization;

namespace Poker.Platform.Mvc
{
    public class LowerCamelCaseContractResolver : DefaultContractResolver
    {
        protected override string ResolvePropertyName(string propertyName)
        {
            return String.Format("{0}{1}", propertyName.ToLower()[0], propertyName.Substring(1));
        }
    }
}