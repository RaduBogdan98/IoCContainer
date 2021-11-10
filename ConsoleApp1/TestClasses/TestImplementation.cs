namespace ConsoleApp1.TestClasses
{
   class TestImplementation : TestInterface
   {
      private string name;
      private int value;
      TestInterface2 implementation;

      public TestImplementation(string name, int value, TestInterface2 implementation)
      {
         this.name = name;
         this.value = value;
         this.implementation = implementation;
      }
   }
}
