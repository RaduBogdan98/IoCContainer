using IoCContainer.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IoCContainer.DependencyInjection
{
   class ImplementationsRepository
   {
      private enum ImplementationLifetime { Singleton, Transient }
      private ConfigurationContainer configurationContainer;
      private Dictionary<Type, ImplementationLifetime> interfaceRefferencesRegister;

      private List<object> RegisteredImplementations;

      public ImplementationsRepository(string configFilePath)
      {
         configurationContainer = new ConfigurationContainer(configFilePath, this);
         RegisteredImplementations = new List<object>();
         interfaceRefferencesRegister = new Dictionary<Type, ImplementationLifetime>();
      }

      internal void RegisterSingleton<TInterface>()
      {
         if (!interfaceRefferencesRegister.ContainsKey(typeof(TInterface)))
         {
            interfaceRefferencesRegister.Add(typeof(TInterface), ImplementationLifetime.Singleton);
         }
         else
         {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Type " + typeof(TInterface).Name + " was already registered.");
            Console.ResetColor();
         }
      }

      internal void RegisterTransient<TInterface>()
      {
         if (!interfaceRefferencesRegister.ContainsKey(typeof(TInterface)))
         {
            interfaceRefferencesRegister.Add(typeof(TInterface), ImplementationLifetime.Transient);
         }
         else
         {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Type " + typeof(TInterface).Name + " was already registered.");
            Console.ResetColor();
         }
      }

      internal TInterface GetInstance<TInterface>()
      {
         if (interfaceRefferencesRegister.ContainsKey(typeof(TInterface)))
         {
            ImplementationLifetime lifetime = interfaceRefferencesRegister.GetValueOrDefault(typeof(TInterface));

            if (lifetime == ImplementationLifetime.Singleton)
            {
               TInterface implementation = (TInterface)RegisteredImplementations.Find(x => x is TInterface);
               if (implementation == null)
               {
                  implementation = RegisterInterfaceImplementantion<TInterface>();
               }

               return implementation;
            }
            else if (lifetime == ImplementationLifetime.Transient)
            {
               return RegisterInterfaceImplementantion<TInterface>();
            }

            return default;
         }
         else
         {
            throw new Exception("Registration missing for type: " + typeof(TInterface).Name);
         }
      }

      private TInterface RegisterInterfaceImplementantion<TInterface>()
      {
         try
         {
            TInterface implementation = (TInterface)configurationContainer.GenerateInterfaceImplementation(typeof(TInterface).Name);
            RegisteredImplementations.Add(implementation);

            return implementation;
         }
         catch (Exception e)
         {
            Console.WriteLine(e.StackTrace);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(e.Message);
            Console.ResetColor();

            return default;
         }
      }
   }
}
