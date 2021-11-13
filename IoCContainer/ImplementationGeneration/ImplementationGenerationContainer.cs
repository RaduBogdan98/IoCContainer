using IoCContainer.Configuration;
using IoCContainer.Reflection;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace IoCContainer.ImplementationGeneration
{
   class ImplementationGenerationContainer
   {
      private ReflectionResolver reflectionResolver;
      private ConfigurationFile configuration;
      private Dictionary<Type, ImplementationDescription> configurationsRegister;

      private List<object> registeredSingletonImplementations;

      public ImplementationGenerationContainer(string configFilePath)
      {
         reflectionResolver = new ReflectionResolver();
         configuration = new ConfigurationFile(configFilePath);
         registeredSingletonImplementations = new List<object>();
         RegisterConfigurations();
      }

      private void RegisterConfigurations()
      {
         configurationsRegister = new Dictionary<Type, ImplementationDescription>();

         foreach (var instanceConfiguration in configuration.InstanceConfigurations)
         {
            Type implementationType = this.reflectionResolver.FindTypeByName(instanceConfiguration.Implementation);
            Type interfaceType = string.IsNullOrEmpty(instanceConfiguration.Interface) ? implementationType : this.reflectionResolver.FindTypeByName(instanceConfiguration.Interface);

            if (!configurationsRegister.ContainsKey(interfaceType))
            {
               switch (instanceConfiguration.Lifetime)
               {
                  case "Singleton": configurationsRegister.Add(interfaceType, new ImplementationDescription(implementationType, Lifetime.Singleton, instanceConfiguration.ConstructorParameters)); break;
                  case "Transient": configurationsRegister.Add(interfaceType, new ImplementationDescription(implementationType, Lifetime.Transient, instanceConfiguration.ConstructorParameters)); break;
                  default: throw new Exception("Lifetime was not configured properly for type " + instanceConfiguration.Interface);
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

      internal TInterface RetrieveInterfaceImplementantion<TInterface>()
      {
         if (configurationsRegister.ContainsKey(typeof(TInterface)))
         {
            Lifetime lifetime = configurationsRegister.GetValueOrDefault(typeof(TInterface)).ImplementationLifetime;

            if (lifetime == Lifetime.Singleton)
            {
               TInterface implementation = (TInterface)registeredSingletonImplementations.Find(x => x is TInterface);
               if (implementation == null)
               {
                  implementation = GenerateInterfaceImplementation<TInterface>();
                  registeredSingletonImplementations.Add(implementation);
               }

               return implementation;
            }
            else if (lifetime == Lifetime.Transient)
            {
               return GenerateInterfaceImplementation<TInterface>();
            }

            return default;
         }
         else
         {
            throw new Exception("Configuration missing for type " + typeof(TInterface).Name);
         }
      }

      private TInterface GenerateInterfaceImplementation<TInterface>()
      {
         ImplementationDescription implementationDescription = configurationsRegister.GetValueOrDefault(typeof(TInterface));
         Type implementationType = implementationDescription.ImplementationType;

         if (implementationType != null)
         {
            return (TInterface)Activator.CreateInstance(implementationType, BuildConstructorParameters(implementationDescription.ConstructorParameters));
         }
         else
         {
            throw new Exception("Interface implementation does not exist: " + implementationType.Name);
         }
      }

      private object[] BuildConstructorParameters(List<ConstructorParameter> constructorParameters)
      {
         object[] parametersArray = new object[constructorParameters.Count];

         for (int i = 0; i < constructorParameters.Count; i++)
         {
            ConstructorParameter currentParameter = constructorParameters[i];
            if (currentParameter.Value.Equals("Ref"))
            {
               try
               {
                  Type parameterType = this.reflectionResolver.FindTypeByName(currentParameter.TypeRefference);
                  parametersArray[i] = reflectionResolver.CallMethodWithGenericType(this, nameof(RetrieveInterfaceImplementantion), parameterType);
               }
               catch (TargetInvocationException e)
               {
                  throw new Exception(e.InnerException.Message);
               }
            }
            else
            {
               Type parameterType = Type.GetType(currentParameter.TypeRefference);
               parametersArray[i] = Convert.ChangeType(currentParameter.Value, parameterType);
            }
         }

         return parametersArray;
      }
   }
}