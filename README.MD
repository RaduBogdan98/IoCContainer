# IoC Container Implementation

The IoC Container project is a dependency injection container built for .NET5 projects. The container is based on a json configuration file with the following layout:

## Config-File Example

```json
{
    "DependencyContainers": [
        {
            "Interface": "TestInterface2",
            "Implementation": "TestImplementation2",
            "Lifetime": "Transient",
            "ConstructorParameters": [
                {
                    "Value": "someStringValue",
                    "TypeRefference": "System.String"
                }
            ]
        },
        {
            "Interface": "TestInterface",
            "Implementation": "TestImplementation",
            "Lifetime": "Singleton",
            "ConstructorParameters": [
                {
                    "Value": "someName",
                    "TypeRefference": "System.String"
                },
                {
                    "Value": 12,
                    "TypeRefference": "System.Int32"
                },
                {
                    "Value": "Ref",
                    "TypeRefference": "TestInterface2"
                }
            ]
        }
    ]
}
```

To use the container implementation you have to import the IoCContainer project in your solution. The container exposes an API which facilitates the interaction with the container. An example of using the container can be seen bellow:

```csharp
using IoCContainer;

namespace Application
{
   class Program
   {
      static void Main(string[] args)
      {
         IoCContainerAPI api = new IoCContainerAPI("<configuration-file-path>");
         TestInterface interfaceImplementation1 = api.GetInterfaceImplementation<TestInterface>();
         TestInterface2 interfaceImplementation2 = api.GetInterfaceImplementation<TestInterface2>();
      }
   }
}
```

The container is responsible with retrieving the correct implementation of the given interface type based on the configuration given by the user.
