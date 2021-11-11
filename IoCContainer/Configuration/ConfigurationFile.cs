using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace IoCContainer.Configuration
{
   class ConfigurationFile
   {
      [JsonProperty]
      internal List<DependencyContainer> DependencyContainers;

      public ConfigurationFile(string configFilePath)
      {
         if (File.Exists(configFilePath))
         {
            string text = File.ReadAllText(configFilePath);
            this.DependencyContainers = JsonConvert.DeserializeObject<ConfigurationFile>(text).DependencyContainers;
         }
         else
         {
            throw new Exception("Config file doesn't exist!!");
         }
      }

      [JsonConstructor]
      internal ConfigurationFile(List<DependencyContainer> dependencyContainers)
      {
         this.DependencyContainers = dependencyContainers;
      }
   }
}
