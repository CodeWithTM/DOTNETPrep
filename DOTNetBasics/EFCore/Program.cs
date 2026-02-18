using EFCore;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<EMSDBContext>(options =>
{
    //options.UseSqlServer(builder.Configuration.GetConnectionString("EMSDBConnection"));

    options.UseSqlServer(builder.Configuration.GetConnectionString("EMSDBConnection"), sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure();
    });
}
);

/*
builder.Services.Add(
    new ServiceDescriptor(typeof(IOperationTransient),
    new OperationTransient(), 
    ServiceLifetime.Transient)
    );
*/

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// enable static file serving
// we need to use UseStaticFiles to enable static file serving

app.UseStaticFiles();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

// if i modified something into DBContext, how can i update the database with the new changes?
// You can use the Entity Framework Core Migrations to update the database with the new changes. You can use the following commands in the Package Manager Console:
// Add-Migration MigrationName
// i ran this command but cannot see newly created Department tbale

//now help me create docker file
//what will be extension of this file


