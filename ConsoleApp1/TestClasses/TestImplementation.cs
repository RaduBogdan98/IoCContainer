namespace ConsoleApp1.TestClasses
{
   class TestImplementation : TestInterface
   {
      private string name;
      private int value;
      TestInterface3 implementation;

      public TestImplementation(string name, int value, TestInterface3 implementation)
      {
         this.name = name;
         this.value = value;
         this.implementation = implementation;
      }
   }
}
