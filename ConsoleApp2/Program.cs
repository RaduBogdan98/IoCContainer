using ConsoleApp1.TestClasses;
using IoCContainer;
using System;

namespace ConsoleApp2
{
   class Program
   {
      static void Main(string[] args)
      {
         try
         {
            IoCContainerAPI api = new IoCContainerAPI("config.json");
            TestInterface testVar1 = api.GetInterfaceImplementation<TestInterface>();
            TestInterface2 testVar2 = api.GetInterfaceImplementation<TestInterface2>();
         }
         catch (Exception e)
         {
            Console.WriteLine(e.StackTrace);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(e.Message);
            Console.ResetColor();
         }
      }
   }
}
