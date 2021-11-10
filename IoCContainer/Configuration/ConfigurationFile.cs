using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace IoCContainer.Configuration
{
   class ConfigurationFile
   {
      [JsonProperty]
      internal List<DependencyContainerDescription> DependencyContainers;

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

      public ConfigurationFile(string configFilePath, bool serialize)
      {
         Serialize(configFilePath);
      }

      [JsonConstructor]
      internal ConfigurationFile(List<DependencyContainerDescription> dependencyContainers)
      {
         this.DependencyContainers = dependencyContainers;
      }

      
      private void Serialize(string configFilePath)
      {
         JsonSerializerSettings settings = new JsonSerializerSettings();
         settings.TypeNameHandling = TypeNameHandling.Objects;
         ConstructorParameter p1 = new ConstructorParameter("name", "someName");
         ConstructorParameter p2 = new ConstructorParameter("value", 12);
         ConstructorParameter p3 = new ConstructorParameter("class", new DependencyContainerDescription("", "", "", new List<ConstructorParameter>()));

         DependencyContainerDescription classDescription = new DependencyContainerDescription("TestImplementation", "TestInterface", "IoCContainer.TestClasses", new List<ConstructorParameter>() { p1, p2, p3 });
         DependencyContainers = new List<DependencyContainerDescription>();
         DependencyContainers.Add(classDescription);

         File.WriteAllText(configFilePath, JsonConvert.SerializeObject(this, Formatting.Indented, settings));
      }
      
   }
}
