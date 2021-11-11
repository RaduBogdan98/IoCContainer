using Newtonsoft.Json;

namespace IoCContainer.Configuration
{
   internal class ConstructorParameter
   {
      [JsonProperty]
      internal object Value;

      [JsonProperty]
      internal string ValueType;

      public ConstructorParameter(object value)
      {
         this.Value = value;
         this.ValueType = value.GetType().ToString();
      }

   }
}