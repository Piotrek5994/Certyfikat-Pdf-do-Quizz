using DinkToPdf.Contracts;
using DinkToPdf;
using PdfService.Service;
using PdfService.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc(options =>
{
    options.EnableDetailedErrors = true;
});
builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
builder.Services.AddSingleton<HtmlToPdfConfiguration>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapGrpcService<PdfGenerationService>();

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
