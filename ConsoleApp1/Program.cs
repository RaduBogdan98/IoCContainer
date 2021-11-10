using ConsoleApp1.TestClasses;
using IoCContainer;
using System;

namespace ConsoleApp1
{
   class Program
   {
      static void Main(string[] args)
      {
         try
         {
            IoCContainerAPI api = new IoCContainerAPI("config.json");

            api.RegisterTransient<TestInterface2>();
            api.RegisterTransient<TestInterface2>();
            api.RegisterSingleton<TestInterface>();
            api.RegisterSingleton<TestInterface>();

            //TestInterface2 testVar1 = api.GetInterfaceImplementation<TestInterface2>();
            //TestInterface2 testVar2 = api.GetInterfaceImplementation<TestInterface2>();
            //TestInterface testVar3 = api.GetInterfaceImplementation<TestInterface>();
            TestInterface testVar4 = api.GetInterfaceImplementation<TestInterface>();

            //Console.WriteLine(testVar1.Equals(testVar2));
            //Console.WriteLine(testVar3.Equals(testVar4));
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
