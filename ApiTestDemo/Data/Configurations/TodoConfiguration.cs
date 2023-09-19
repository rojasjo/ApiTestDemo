using ApiTestDemo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiTestDemo.Data.Configurations;

public class TodoConfiguration : IEntityTypeConfiguration<Todo>
{
    public void Configure(EntityTypeBuilder<Todo> builder)
    {
        builder.HasKey(todo => todo.Id);
        builder.Property(todo => todo.Id).ValueGeneratedOnAdd();

        builder.Property(todo => todo.Title).IsRequired().HasMaxLength(100);
        builder.Property(todo => todo.Description).HasMaxLength(500);
    }
}