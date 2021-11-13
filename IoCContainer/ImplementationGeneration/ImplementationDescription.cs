using IoCContainer.Configuration;
using System;
using System.Collections.Generic;

namespace IoCContainer.ImplementationGeneration
{
   class ImplementationDescription
   {
      internal Type ImplementationType;
      internal Lifetime ImplementationLifetime;
      internal List<ConstructorParameter> ConstructorParameters;

      public ImplementationDescription(Type implementationType, Lifetime implementationLifetime, List<ConstructorParameter> constructorParameters)
      {
         this.ImplementationType = implementationType;
         this.ImplementationLifetime = implementationLifetime;
         this.ConstructorParameters = constructorParameters;
      }
   }
}
