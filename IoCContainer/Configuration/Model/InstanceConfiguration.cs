using Newtonsoft.Json;
using System.Collections.Generic;

namespace IoCContainer.Configuration
{
   internal class InstanceConfiguration
   {
      [JsonProperty]
      internal string Interface;

      [JsonProperty]
      internal string Implementation;

      [JsonProperty]
      internal string Lifetime;

      [JsonProperty]
      internal List<ConstructorParameter> ConstructorParameters;

      public InstanceConfiguration(string implementationName, string interfaceName, string lifetime, List<ConstructorParameter> constructorParameters)
      {
         this.Implementation = implementationName;
         this.Interface = interfaceName;
         this.ConstructorParameters = constructorParameters;
         this.Lifetime = lifetime;
      }

   }
}