using Newtonsoft.Json;

namespace Poker.Helpers
{
    public static class JsonHelper
    {
         public static string ToJson(object o)
         {
             return JsonConvert.SerializeObject(o);
         }
    }
}