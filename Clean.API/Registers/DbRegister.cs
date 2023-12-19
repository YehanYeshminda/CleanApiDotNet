using Clean.DAL;
using Microsoft.EntityFrameworkCore;

namespace Clean.API.Registers;

public class DbRegister : IWebApplicationBuilderRegister
{
    public void RegisterServices(WebApplicationBuilder builder)
    {
        var cs = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(cs));
    }
}