using Clean.API.Registers;

namespace Clean.API.Extensions;

public static class RegisterExtensions
{
    public static void RegisterServices(this WebApplicationBuilder builder, Type scanningType)
    {
        var registrars = GetRegistrars<IWebApplicationBuilderRegister>(scanningType);

        foreach (var registrar in registrars)
        {
            registrar.RegisterServices(builder);
        }
    }

    public static void RegisterPipelineComponents(this WebApplication app, Type scanningType)
    {
        var registrars = GetRegistrars<IWebApplicationRegister>(scanningType);
        foreach (var registrar in registrars)
        {
            registrar.RegisterPipeLineComponents(app);
        }
    }

    private static IEnumerable<T> GetRegistrars<T>(Type scanningType) where T: IRegistrar
    {
        return scanningType.Assembly.GetTypes()
            .Where(t => t.IsAssignableTo(typeof(T)) && !t.IsAbstract && !t.IsInterface)
            .Select(Activator.CreateInstance)
            .Cast<T>();
    }
}