using CodeSharp.Core.Executors.Models.Shared;
using CodeSharp.Core.Executors.Models.Testing;
using CodeSharp.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;

namespace CodeSharp.EntityFramework.Configurations;

public sealed class TestLogConfiguration : IEntityTypeConfiguration<TestLog>
{
    public void Configure(EntityTypeBuilder<TestLog> builder)
    {
        var jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var codeAnalysisConverter = new ValueConverter<CodeAnalysisReport, string>(
                   v => JsonSerializer.Serialize(v, jsonSerializerOptions),
                   v => JsonSerializer.Deserialize<CodeAnalysisReport>(v, jsonSerializerOptions) ?? new CodeAnalysisReport());

        var testResultsConverter = new ValueConverter<IList<TestingResult>, string>(
                   v => JsonSerializer.Serialize(v, jsonSerializerOptions),
                   v => JsonSerializer.Deserialize<IList<TestingResult>>(v, jsonSerializerOptions) ?? new List<TestingResult>());

        builder.Property(e => e.CodeReport)
            .HasConversion(codeAnalysisConverter)
            .IsUnicode(false);

        builder.Property(e => e.TestResults)
            .HasConversion(testResultsConverter)
            .IsUnicode(false);

        builder
            .HasKey(e => e.Id);
    }
}
