namespace Clean.API.Registers;

public interface IWebApplicationBuilderRegister: IRegistrar
{
    void RegisterServices(WebApplicationBuilder builder);
}