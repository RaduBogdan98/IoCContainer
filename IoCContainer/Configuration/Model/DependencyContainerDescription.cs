using Newtonsoft.Json;
using System.Collections.Generic;

namespace IoCContainer.Configuration
{
   internal class DependencyContainerDescription
   {
      [JsonProperty]
      internal string Namespace;

      [JsonProperty]
      internal string Interface;

      [JsonProperty]
      internal string Implementation;

      [JsonProperty]
      internal List<ConstructorParameter> ConstructorParameters;

      public DependencyContainerDescription(string implementationName, string interfaceName, string implementationNamespace, List<ConstructorParameter> constructorParameters)
      {
         this.Implementation = implementationName;
         this.Interface = interfaceName;
         this.ConstructorParameters = constructorParameters;
         this.Namespace = implementationNamespace;
      }

   }
}