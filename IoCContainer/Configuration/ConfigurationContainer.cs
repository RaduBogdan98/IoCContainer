using IoCContainer.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace IoCContainer.Configuration
{
   class ConfigurationContainer
   {
      private ConfigurationFile configuration;
      private ImplementationsRepository parentRepository;

      public ConfigurationContainer(string configFilePath, ImplementationsRepository parentRepository)
      {
         configuration = new ConfigurationFile(configFilePath);
         this.parentRepository = parentRepository;
      }

      internal object GenerateInterfaceImplementation(string interfaceName)
      {
         DependencyContainerDescription dependencyContainerDescription = configuration.DependencyContainers.FirstOrDefault(x => x.Interface.Equals(interfaceName));
         if (dependencyContainerDescription != null)
         {
            return this.InstantiateClass(dependencyContainerDescription);
         }
         else
         {
            throw new Exception("Interface implementation was not configured: " + interfaceName);
         }
      }

      private object InstantiateClass(DependencyContainerDescription dependencyContainerDescription)
      {
         Type classType = AppDomain.CurrentDomain.GetAssemblies()
                                   .SelectMany(t => t.GetTypes())
                                   .FirstOrDefault(t => t.IsClass &&
                                                        t.Namespace == dependencyContainerDescription.Namespace &&
                                                        t.Name == dependencyContainerDescription.Implementation);

         if (classType != null)
         {
            List<ConstructorParameter> constructorParameters = dependencyContainerDescription.ConstructorParameters;
            object[] parametersArray = new object[constructorParameters.Count];

            for (int i = 0; i < constructorParameters.Count; i++)
            {
               ConstructorParameter currentParameter = constructorParameters[i];
               Type paramGenericType = currentParameter.ValueType;

               if (currentParameter.Value.Equals("Ref"))
               {
                  MethodInfo method = typeof(ImplementationsRepository).GetMethod("GetInstance", BindingFlags.Instance | BindingFlags.NonPublic);
                  MethodInfo generic = method.MakeGenericMethod(paramGenericType);
                  var implementation = generic.Invoke(parentRepository, null);

                  if (implementation != null)
                  {
                     parametersArray[i] = implementation;
                  }
                  else
                  {
                     throw new Exception("Refference not registered for type: " + paramGenericType.Name);
                  }
               }
               else
               {
                  parametersArray[i] = Convert.ChangeType(currentParameter.Value, currentParameter.ValueType);
               }
            }

            return Activator.CreateInstance(classType, parametersArray);
         }
         else
         {
            throw new Exception("Interface implementation does not exist: " + classType.Name);
         }
      }
   }
}