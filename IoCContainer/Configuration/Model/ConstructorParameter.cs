using Newtonsoft.Json;

namespace IoCContainer.Configuration
{
   internal class ConstructorParameter
   {
      [JsonProperty]
      internal object Value;

      [JsonProperty]
      internal string TypeRefference;

      public ConstructorParameter(object value)
      {
         this.Value = value;
         this.TypeRefference = value.GetType().ToString();
      }

   }
}