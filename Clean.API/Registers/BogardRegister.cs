using Clean.Application.UserProfiles.Queries;
using MediatR;

namespace Clean.API.Registers;

public class BogardRegister : IWebApplicationBuilderRegister
{
    public void RegisterServices(WebApplicationBuilder builder)
    {
        // just reference whatever if a program is not present
        builder.Services.AddAutoMapper(typeof(Program), typeof(GetAllUserProfiles));
        builder.Services.AddMediatR(typeof(GetAllUserProfiles));
    }
}