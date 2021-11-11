# IoC Container Implementation

The IoC Container project is a dependency injection container built for .NET5 projects. The container is based on a json configuration file with the following layout:

## Config-File Example

```Perl
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
     "TypeRefference": "TestInterface3"
    }
   ]
  }
 ]
}
```
