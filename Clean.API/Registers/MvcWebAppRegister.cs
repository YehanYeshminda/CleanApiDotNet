namespace Clean.API.Registers;

public class MvcWebAppRegister : IWebApplicationRegister
{
    public void RegisterPipeLineComponents(WebApplication app)
    {
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
    }
}