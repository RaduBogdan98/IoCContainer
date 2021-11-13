using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace IoCContainer.Configuration
{
   class ConfigurationFile
   {
      [JsonProperty("Configurations")]
      internal List<InstanceConfiguration> InstanceConfigurations;

      public ConfigurationFile(string configFilePath)
      {
         if (File.Exists(configFilePath))
         {
            string text = File.ReadAllText(configFilePath);
            this.InstanceConfigurations = JsonConvert.DeserializeObject<ConfigurationFile>(text).InstanceConfigurations;
         }
         else
         {
            throw new Exception("Config file doesn't exist!!");
         }
      }

      [JsonConstructor]
      internal ConfigurationFile(List<InstanceConfiguration> instanceConfigurations)
      {
         this.InstanceConfigurations = instanceConfigurations;
      }
   }
}
