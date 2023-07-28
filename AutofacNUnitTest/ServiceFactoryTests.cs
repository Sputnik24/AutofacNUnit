using Autofac;
using AutofacNUnitLib;
using NUnit.Framework;

namespace AutofacNUnitTest; 

[TestFixture]
[NonParallelizable]
public class ServiceFactoryTests {

    
    [Test]
    [NonParallelizable]
    public void FakeServiceDiffProjects() {
        IFakeService? interf;
        FakeService? service;

        var container = ServiceFactory.CreateContainer(
            "AutofacNUnitLib.FakeService",
            "AutofacNUnitLib.IFakeService");
        
        Assert.IsTrue(container.TryResolve(out IFakeService? foundInterf));
        Assert.IsTrue(container.TryResolve(out FakeService? foundService));

        interf = foundInterf;
        service = foundService;

        container.Dispose();
    }
    
    [Test]
    public void FakeServiceSameProject() {
        IFakeService? interf;
        FakeService? service;

        var container = AutofacNUnitLib.ServiceFactory.CreateContainer(
            "AutofacNUnitLib.FakeService",
            "AutofacNUnitLib.IFakeService");
        
        Assert.IsTrue(container.TryResolve(out IFakeService? foundInterf));
        Assert.IsTrue(container.TryResolve(out FakeService? foundService));

        interf = foundInterf;
        service = foundService;

        container.Dispose();
    }
}
