using System;

namespace IoCContainer
{
   public class IoCContainerAPI
   {
      public IoCContainerAPI(string configFilePath)
      {
         container = new ImplementationGeneratorContainer(configFilePath);
      }

      #region Methods
      public TInterface GetInterfaceImplementation<TInterface>()
      {
         return container.RetrieveInterfaceImplementantion<TInterface>();
      }
      #endregion

      #region Fields
      ImplementationGeneratorContainer container;
      #endregion
   }
}
