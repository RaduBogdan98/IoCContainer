using Newtonsoft.Json;
using System;

namespace IoCContainer.Configuration
{
   internal class ConstructorParameter
   {
      [JsonProperty]
      internal string Name;

      [JsonProperty]
      internal object Value;

      [JsonProperty]
      internal Type ValueType;

      public ConstructorParameter(string name, object value)
      {
         this.Name = name;
         this.Value = value;
         this.ValueType = value.GetType();
      }

   }
}