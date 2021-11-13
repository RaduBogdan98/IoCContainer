using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace IoCContainer.Reflection
{
   class ReflectionResolver
   {
      private List<Assembly> applicationAssemblies;

      public ReflectionResolver()
      {
         LoadAllAssemblies();
      }

      private void LoadAllAssemblies()
      {
         applicationAssemblies = new List<Assembly>();
         List<string> refferencedAssemblyPaths = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll").ToList();
         refferencedAssemblyPaths.ForEach(path => applicationAssemblies.Add(AppDomain.CurrentDomain.Load(AssemblyName.GetAssemblyName(path))));
      }

      internal Type FindTypeByName(string typeName)
      {
         Type type = applicationAssemblies.SelectMany(t => t.GetTypes())
                                          .FirstOrDefault(t => (t.IsInterface || t.IsClass) && t.Name == typeName);

         if (type != null)
         {
            return type;
         }
         else
         {
            throw new Exception("Type " + typeName + " was not found");
         }
      }

      internal object CallMethodWithGenericType<CallerType>(CallerType targetOfInvocation, string methodName, Type genericType)
      {
         MethodInfo method = typeof(CallerType).GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);
         MethodInfo generic = method.MakeGenericMethod(genericType);
         return generic.Invoke(targetOfInvocation, null);
      }

   }
}
