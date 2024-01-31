using Clean.Application.Services;

namespace Clean.API.Registers;

public class ApplicationLayerRegister : IWebApplicationBuilderRegister
{
    public void RegisterServices(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IdentityService>();
    }
}