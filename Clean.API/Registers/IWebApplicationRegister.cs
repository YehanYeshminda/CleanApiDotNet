namespace Clean.API.Registers;

public interface IWebApplicationRegister: IRegistrar
{
    public void RegisterPipeLineComponents(WebApplication app);
}