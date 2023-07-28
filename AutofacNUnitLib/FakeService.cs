using Autofac;

namespace AutofacNUnitLib; 

public class FakeService : IFakeService, IStartable, IDisposable {
    public void Start() { }

    public void Dispose() { }
}

public interface IFakeService { }