using IoCContainer.Configuration;
using IoCContainer.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace IoCContainer
{
   /// <summary>
   /// Dependencies:
   ///   - Resolves responsabilities based on the configuration file given by the user.
   ///   
   /// Responsabilities:
   ///   - Register configured instances with their respective lifetime type.
   ///   - Generate and retrieve interface implementations.
   /// </summary>
   class ImplementationGeneratorContainer
   {
      private ReflectionResolver reflectionResolver;
      private ConfigurationFile configuration;
      private enum Lifetime { Singleton, Transient }
      private Dictionary<Type, Tuple<Type, Lifetime>> instanceLifetimeRegister;

      private List<object> registeredSingletonImplementations;

      public ImplementationGeneratorContainer(string configFilePath)
      {
         reflectionResolver = new ReflectionResolver();
         configuration = new ConfigurationFile(configFilePath);
         registeredSingletonImplementations = new List<object>();
         RegisterConfiguredClasses();
      }

      /// <summary>
      /// Registeres the interfaces and implementations configured in the configuration file specifying their lifetime.
      /// </summary>
      private void RegisterConfiguredClasses()
      {
         instanceLifetimeRegister = new Dictionary<Type, Tuple<Type, Lifetime>>();

         foreach (var dependencyContainer in configuration.DependencyContainers)
         {
            Type implementationType = this.reflectionResolver.FindTypeByName(dependencyContainer.Implementation, true);
            Type interfaceType = dependencyContainer.Interface != "" ? this.reflectionResolver.FindTypeByName(dependencyContainer.Interface, false) : implementationType;

            if (!instanceLifetimeRegister.ContainsKey(interfaceType))
            {
               switch (dependencyContainer.Lifetime)
               {
                  case "Singleton": instanceLifetimeRegister.Add(interfaceType, Tuple.Create(implementationType, Lifetime.Singleton)); break;
                  case "Transient": instanceLifetimeRegister.Add(interfaceType, Tuple.Create(implementationType, Lifetime.Transient)); break;
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

      /// <summary>
      /// Either retrieves an existing implementation or generates a new implementation for a given interface type
      /// using the algorithms described bellow.
      /// </summary>
      /// <typeparam name="TInterface">The given interface type</typeparam>
      /// <returns>Concretion of the given interface type</returns>
      internal TInterface RetrieveInterfaceImplementantion<TInterface>()
      {
         if (instanceLifetimeRegister.ContainsKey(typeof(TInterface)))
         {
            Lifetime lifetime = instanceLifetimeRegister.GetValueOrDefault(typeof(TInterface)).Item2;

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
            throw new Exception("Registration missing for type " + typeof(TInterface).Name);
         }
      }

      /// <summary>
      /// Retrieves the interface configuration and calls the instatiation algorithm for the given interface 
      /// based on this configuration.
      /// </summary>
      /// <typeparam name="TInterface">The type of the interface that needs to be instantiated</typeparam>
      /// <returns>Concretion of the given interface type</returns>
      private TInterface GenerateInterfaceImplementation<TInterface>()
      {
         DependencyContainer dependencyContainerDescription = FindDependencyContainerForInterface(typeof(TInterface).Name);
         if (dependencyContainerDescription != null)
         {
            return this.InstantiateConcretion<TInterface>(dependencyContainerDescription);
         }
         else
         {
            throw new Exception("Interface implementation was not configured: " + typeof(TInterface).Name);
         }
      }

      /// <summary>
      /// Generates the implementation of the given interface type based on the given dependency container configuration.
      /// </summary>
      /// <typeparam name="TInterface">The type of the interface that needs to be instantiated</typeparam>
      /// <param name="dependencyContainerDescription">dependency container specification for given interface</param>
      /// <returns>Concretion of the given interface type</returns>
      private TInterface InstantiateConcretion<TInterface>(DependencyContainer dependencyContainerDescription)
      {
         Type implementationType = instanceLifetimeRegister.GetValueOrDefault(typeof(TInterface))?.Item1;

         if (implementationType != null)
         {
            List<ConstructorParameter> constructorParameters = dependencyContainerDescription.ConstructorParameters;
            object[] parametersArray = new object[constructorParameters.Count];

            for (int i = 0; i < constructorParameters.Count; i++)
            {
               ConstructorParameter currentParameter = constructorParameters[i];

               if (currentParameter.Value.Equals("Ref"))
               {
                  Type parameterType = this.reflectionResolver.FindTypeByName(currentParameter.TypeRefference, false);

                  try
                  {
                     MethodInfo method = typeof(ImplementationGeneratorContainer).GetMethod("RetrieveInterfaceImplementantion", BindingFlags.Instance | BindingFlags.NonPublic);
                     MethodInfo generic = method.MakeGenericMethod(parameterType);
                     parametersArray[i] = generic.Invoke(this, null);
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

            return (TInterface)Activator.CreateInstance(implementationType, parametersArray);
         }
         else
         {
            throw new Exception("Interface implementation does not exist: " + implementationType.Name);
         }
      }

      /// <summary>
      /// Finds the dependency container refferenced by the given interface name.
      /// </summary>
      /// <param name="interfaceName">Searched interface name</param>
      /// <returns>The found dependency container or null if type does not exist</returns>
      private DependencyContainer FindDependencyContainerForInterface(string interfaceName)
      {
         return configuration.DependencyContainers.FirstOrDefault(x => x.Interface.Equals(interfaceName));
      }
   }
}