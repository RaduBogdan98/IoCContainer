using IoCContainer.Configuration;
using IoCContainer.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoCContainer
{
   public class IoCContainerAPI
   {
      public IoCContainerAPI(string configFilePath)
      {
         repository = new ImplementationsRepository(configFilePath);
      }

      #region Methods
      public void RegisterSingleton<TInterface>()
      {
         repository.RegisterSingleton<TInterface>();
      }

      public void RegisterTransient<TInterface>()
      {
         repository.RegisterTransient<TInterface>();
      }

      public TInterface GetInterfaceImplementation<TInterface>()
      {
         return repository.GetInstance<TInterface>();
      }
      #endregion

      #region Fields
      ImplementationsRepository repository;
      #endregion
   }
}
