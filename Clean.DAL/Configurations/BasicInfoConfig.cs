using Clean.Domain.Aggregates.UserProfileAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Clean.DAL.Configurations;

public class BasicInfoConfig : IEntityTypeConfiguration<BasicInfo>
{
    public void Configure(EntityTypeBuilder<BasicInfo> builder)
    {
        // OWNED ENTITY CONCEPT AND DECLARING IT AS OWNED AND ASSIGNING TO ANOTHER TO BE THE OWNER
        
    }
}