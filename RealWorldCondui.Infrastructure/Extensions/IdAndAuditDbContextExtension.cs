using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RealWorldCondui.Infrastructure.Extensions
{
    internal static class IdAndAuditDbContextExtension
    {
        public static void GenerateId<T>(this EntityTypeBuilder<T> builder) where T : class
        {
            builder.Property<Guid>("Id").HasDefaultValueSql("uuid_generator_v4()");
        }

        public static void GenerateAudit<T>(this EntityTypeBuilder<T> builder) where T : class
        {
            builder.Property<DateTime>("CreatedAt").HasDefaultValueSql("NOW()");
            builder.Property<DateTime>("LastUpdatedAt").HasDefaultValueSql("NOW()");
        }
    }
}
