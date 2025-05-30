using JPOC_VM_DEMO.App_Code.CTRL;
using JPOC_VM_DEMO.Common;
using JPOC_VM_DEMO.Services;
using JPOC_VM_DEMO.Services.JpocData.T_JP_Disease.Data;
using JPOC_VM_DEMO.Services.JpocData.T_JP_Disease.Logic;
using JPOC_VM_DEMO.Services.JpocData.Utilities;
using JPOC_VM_DEMO.App_Code.CTRL.Logic;
using JPOC_VM_DEMO.App_Code.CTRL.DTO;
using JPOC_VM_DEMO.App_Code.DataAccess;
using Jpoc.Dac;
using GlobalVariables = JPOC_VM_DEMO.Common.GlobalVariables;
using JPOC_VM_DEMO.App_Code.Util;
using JPOC_VM_DEMO.App_Code.BaseClass;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDistributedMemoryCache(); // Required for session state
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Or your desired timeout
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Add your base classes
//builder.Services.AddScoped<PageBase>();
//builder.Services.AddScoped<PageBaseModel>();
builder.Services.AddScoped<MainContentsPageBase>();

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddScoped<IFooterService, FooterService>();
builder.Services.AddScoped<IDatabaseService, DatabaseService>();
builder.Services.AddScoped<ISearchService, SearchService>();
builder.Services.AddScoped<CtrlSearch>();
builder.Services.AddScoped<LogicSearch>();
builder.Services.AddScoped<AbstractDto, DtoSearch>();
builder.Services.AddScoped<GlobalVariables>();
builder.Services.AddScoped<ElsDataBase>(provider => new ElsDataBase(GlobalVariables.ConnectionString));
builder.Services.AddScoped<IDiseaseDataManager, DiseaseDataManager>();
builder.Services.AddScoped<IDiseaseBusinessManager, DiseaseBusinessManager>();
builder.Services.AddScoped<IDatabaseFactory, DatabaseFactoryNew>();
builder.Services.AddSingleton<JPoCUtility>();
builder.Services.AddSingleton<DatabaseFactory>();

builder.Services.AddHttpContextAccessor();


// Configure application settings
ConfigureApplication(builder);

var app = builder.Build();

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

app.UseAuthorization();

app.UseSession();

app.MapRazorPages();

app.Run();


void ConfigureApplication(WebApplicationBuilder builder)
{
    GlobalVariables.ApplicationType = Jpoc.Common.PublicEnum.eApplicationType.WebApplication;
    GlobalVariables.AppRootPhysicalPath = builder.Environment.ContentRootPath;
    GlobalVariables.RunTimeEnvironmentString = builder.Configuration["RuntimeEnvironment"];

    // TODO: Configure watermark
    //if (File.Exists(GlobalVariables.WaterMarkImagePhysicalPath))
    //{
    //    var waterMark = File.ReadAllBytes(GlobalVariables.WaterMarkImagePhysicalPath);
    //    GlobalVariables.SetWaterMark(waterMark);
    //}

    // TODO: Add other application services
    builder.Services.AddHttpContextAccessor();
    //builder.Services.AddScoped<JpocRequestTracker>();
}
