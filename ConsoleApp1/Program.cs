using ConsoleApp1.TestClasses;
using IoCContainer;
using System;

namespace ConsoleApp1
{
   class Program
   {
      static void Main(string[] args)
      {
         IoCContainerAPI api = new IoCContainerAPI("config.json");
         TestInterface testVar1 = api.GetInterfaceImplementation<TestInterface>();
         TestInterface2 testVar2 = api.GetInterfaceImplementation<TestInterface2>();
      }
   }
}
