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
         try
         {
            return container.RetrieveInterfaceImplementantion<TInterface>();
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
      #endregion

      #region Fields
      ImplementationGeneratorContainer container;
      #endregion
   }
}
