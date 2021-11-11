using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace IoCContainer.Configuration
{
   class ConfigurationContainer
   {
      private ConfigurationFile configuration;
      private enum ImplementationLifetime { Singleton, Transient }
      private Dictionary<Type, Tuple<Type, ImplementationLifetime>> containerLifetimeRegister;

      private List<object> registeredImplementations;

      public ConfigurationContainer(string configFilePath)
      {
         configuration = new ConfigurationFile(configFilePath);
         registeredImplementations = new List<object>();
         RegisterConfiguredClasses();
      }

      private void RegisterConfiguredClasses()
      {
         containerLifetimeRegister = new Dictionary<Type, Tuple<Type, ImplementationLifetime>>();

         foreach (var dependencyContainer in configuration.DependencyContainers)
         {
            Type interfaceType = this.FindType(dependencyContainer.Interface, false);
            if (!containerLifetimeRegister.ContainsKey(interfaceType))
            {
               Type implementationType = this.FindType(dependencyContainer.Implementation, true);

               switch (dependencyContainer.Lifetime)
               {
                  case "Singleton": containerLifetimeRegister.Add(interfaceType, Tuple.Create(implementationType, ImplementationLifetime.Singleton)); break;
                  case "Transient": containerLifetimeRegister.Add(interfaceType, Tuple.Create(implementationType, ImplementationLifetime.Transient)); break;
                  default: throw new Exception("Lifetime was not configured properly for type " + dependencyContainer.Interface);
               }
            }
            else
            {
               Console.ForegroundColor = ConsoleColor.Cyan;
               Console.WriteLine("Type " + interfaceType.Name + " was already registered.");
               Console.ResetColor();
            }
         }
      }

      internal TInterface GetInterfaceImplementantion<TInterface>()
      {
         if (containerLifetimeRegister.ContainsKey(typeof(TInterface)))
         {
            ImplementationLifetime lifetime = containerLifetimeRegister.GetValueOrDefault(typeof(TInterface)).Item2;

            if (lifetime == ImplementationLifetime.Singleton)
            {
               TInterface implementation = (TInterface)registeredImplementations.Find(x => x is TInterface);
               if (implementation == null)
               {
                  implementation = GenerateInterfaceImplementation<TInterface>();
                  registeredImplementations.Add(implementation);
               }

               return implementation;
            }
            else if (lifetime == ImplementationLifetime.Transient)
            {
               return GenerateInterfaceImplementation<TInterface>();
            }

            return default;
         }
         else
         {
            throw new Exception("Registration missing for type: " + typeof(TInterface).Name);
         }
      }

      private TInterface GenerateInterfaceImplementation<TInterface>()
      {
         DependencyContainerDescription dependencyContainerDescription = FindDependencyContainer(typeof(TInterface).Name);
         if (dependencyContainerDescription != null)
         {
            return this.InstantiateClass<TInterface>(dependencyContainerDescription);
         }
         else
         {
            throw new Exception("Interface implementation was not configured: " + typeof(TInterface).Name);
         }
      }

      private TInterface InstantiateClass<TInterface>(DependencyContainerDescription dependencyContainerDescription)
      {
         Type implementationType = containerLifetimeRegister.GetValueOrDefault(typeof(TInterface))?.Item1;

         if (implementationType != null)
         {
            List<ConstructorParameter> constructorParameters = dependencyContainerDescription.ConstructorParameters;
            object[] parametersArray = new object[constructorParameters.Count];

            for (int i = 0; i < constructorParameters.Count; i++)
            {
               ConstructorParameter currentParameter = constructorParameters[i];

               if (currentParameter.Value.Equals("Ref"))
               {
                  Type parameterType = this.FindType(currentParameter.ValueType, false);

                  MethodInfo method = typeof(ConfigurationContainer).GetMethod("InstantiateClass", BindingFlags.Instance | BindingFlags.NonPublic);
                  MethodInfo generic = method.MakeGenericMethod(parameterType);
                  parametersArray[i] = generic.Invoke(this, new object[] { FindDependencyContainer(currentParameter.ValueType) });
               }
               else
               {
                  Type parameterType = Type.GetType(currentParameter.ValueType);
                  parametersArray[i] = Convert.ChangeType(currentParameter.Value, parameterType);
               }  
            }

            return (TInterface)Activator.CreateInstance(implementationType, parametersArray);
         }
         else
         {
            throw new Exception("Interface implementation does not exist: " + implementationType.Name);
         }
      }

      private Type FindType(string typeName, bool isClass)
      {
         Type type = AppDomain.CurrentDomain.GetAssemblies()
                                  .SelectMany(t => t.GetTypes())
                                  .FirstOrDefault(t => t.IsClass == isClass && t.Name == typeName);

         if (type != null)
         {
            return type;
         }
         else
         {
            throw new Exception("Type " + typeName + " was not found");
         }
      }

      private DependencyContainerDescription FindDependencyContainer(string interfaceName)
      {
         return configuration.DependencyContainers.FirstOrDefault(x => x.Interface.Equals(interfaceName));
      }
   }
}