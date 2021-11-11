using IoCContainer.Configuration;

namespace IoCContainer
{
   public class IoCContainerAPI
   {
      public IoCContainerAPI(string configFilePath)
      {
         container = new ConfigurationContainer(configFilePath);
      }

      #region Methods
      public TInterface GetInterfaceImplementation<TInterface>()
      {
         return container.GetInterfaceImplementantion<TInterface>();
      }
      #endregion

      #region Fields
      ConfigurationContainer container;
      #endregion
   }
}
