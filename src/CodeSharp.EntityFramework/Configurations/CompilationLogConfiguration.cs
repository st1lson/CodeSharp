using CodeSharp.Core.Executors.Models.Shared;
using CodeSharp.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;

namespace CodeSharp.EntityFramework.Configurations;

public class CompilationLogConfiguration : IEntityTypeConfiguration<CompilationLog>
{
    public void Configure(EntityTypeBuilder<CompilationLog> builder)
    {
        var jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var converter = new ValueConverter<CodeAnalysisReport, string>(
                   v => JsonSerializer.Serialize(v, jsonSerializerOptions),
                   v => JsonSerializer.Deserialize<CodeAnalysisReport>(v, jsonSerializerOptions) ?? new CodeAnalysisReport());

        builder
            .Property(e => e.CodeReport)
            .HasConversion(converter)
            .IsUnicode(false);

        builder
            .HasKey(e => e.Id);
    }
}
