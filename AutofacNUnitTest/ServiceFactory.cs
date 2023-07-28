using System.Reflection;
using Autofac;
using NUnit.Framework;

namespace AutofacNUnitTest; 

public static class ServiceFactory {
    public static IContainer CreateContainer(string typeName, string interfaceName) {
        var builder = new ContainerBuilder();

        var foundType = Type.GetType(typeName);
        var foundInterface = Type.GetType(interfaceName);

        foundType ??= FindAssemblyInBin(typeName);
        foundInterface ??= FindAssemblyInBin(interfaceName);

        TestContext.Progress.WriteLine(foundType.FullName + ":");
        foreach (var inter in foundType.GetInterfaces()) {
            TestContext.Progress.WriteLine(inter.FullName);
        }

        builder.RegisterType(foundType)
            .As(foundInterface)
            .As<IStartable>()
            .AsSelf()
            .SingleInstance();

        return builder.Build();
    }
    
    private static Type? FindAssemblyInBin(string typeName) {
        //Workaround: load all dlls in bin folder:
        //https://dotnetcoretutorials.com/getting-assemblies-is-harder-than-you-think-in-c/?expand_article=1
        //https://stackoverflow.com/questions/1288288/how-to-load-all-assemblies-from-within-your-bin-directory
        Type? foundType = null;
        var assemblies =
            Directory.GetFiles(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
                               ?? AppDomain.CurrentDomain.BaseDirectory, "*.dll", SearchOption.TopDirectoryOnly);
        
        foreach (var assemblyFile in assemblies) {
            try {
                Assembly assembly = Assembly.LoadFrom(assemblyFile);
                Type? tmpType = typeName.Contains(".") ? assembly.GetTypes().FirstOrDefault(t => t.FullName == typeName)
                    : assembly.GetTypes().SingleOrDefault(t => t.Name == typeName);
                 
                if (tmpType is not null && foundType is not null) {
                    throw new InvalidOperationException($"Ambiguous type {typeName}.");
                }

                if (tmpType is null) continue;
            
                foundType = tmpType;
            } catch (Exception) {
                //ignore
            }
        }

        return foundType;
    }
}