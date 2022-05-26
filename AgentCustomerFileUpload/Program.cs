using AgentCustomer.FileDataAccess;
using AgentCustomer.Files;
using AgentCustomer.Files.AgentAdapter;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.IO.Abstractions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddMemoryCache();

builder.Services.AddDbContext<FileDBContext>(opt =>
{
    opt.UseInMemoryDatabase(databaseName: "ACFilesDB");
});

builder.Services.AddScoped<IFileSystem, FileSystem>();
builder.Services.AddScoped<IFileRepository, FileRepository>();
builder.Services.AddScoped<IHttpClientWrapper, HttpClientWrapper>();
builder.Services.AddScoped<IAgentAdapter, AgentAdapter>();
builder.Services.AddScoped<ICustomerFileValidator, CustomerFileValidator>();
builder.Services.AddScoped<IFileProcessor, FileProcessor>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<ICloudFileService, CloudFileService>();
builder.Services.AddScoped<ICustomerNotificationService, CustomerNotificationService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ICustomerFileService, CustomerFileService>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

