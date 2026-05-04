using MudBlazor;
using MudBlazor.Services;
using NORCE.Drilling.GeologicalProperties.WebApp;
using NORCE.Drilling.GeologicalProperties.WebPages;
using Plotly.Blazor.Traces.SankeyLib;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomLeft;
    config.SnackbarConfiguration.PreventDuplicates = false;
    config.SnackbarConfiguration.NewestOnTop = false;
    config.SnackbarConfiguration.ShowCloseIcon = true;
    config.SnackbarConfiguration.VisibleStateDuration = 5000;
    config.SnackbarConfiguration.HideTransitionDuration = 500;
    config.SnackbarConfiguration.ShowTransitionDuration = 500;
    config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
});

var webPagesConfiguration = new WebPagesHostConfiguration
{
    FieldHostURL = builder.Configuration["FieldHostURL"],
    ClusterHostURL = builder.Configuration["ClusterHostURL"],
    WellHostURL = builder.Configuration["WellHostURL"],
    WellBoreHostURL = builder.Configuration["WellBoreHostURL"],
    GeologicalPropertiesHostURL = builder.Configuration["GeologicalPropertiesHostURL"],
    UnitConversionHostURL = builder.Configuration["UnitConversionHostURL"]
};

builder.Services.AddSingleton<IGeologicalPropertiesWebPagesConfiguration>(webPagesConfiguration);
builder.Services.AddSingleton<IGeologicalPropertiesAPIUtils, GeologicalPropertiesAPIUtils>();

var app = builder.Build();

app.UseForwardedHeaders();
var basePath = "/geologicalproperties/webapp";
app.UsePathBase(basePath);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();

