using Microsoft.EntityFrameworkCore;
using SignalRLab3;
using SignalRLab3.DataAccess;
using SignalRLab3.Helper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddDbContext<SignalRLab3DbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DBConnectionString"));
});

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(4);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddSignalR();
builder.Services.AddTransient<IDataBuilderService, DataBuilderService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapRazorPages();
app.MapHub<ChatHub>("/chathub");

//seed data

var scope = app.Services.CreateScope();
var dataBuilderService = scope.ServiceProvider.GetRequiredService<IDataBuilderService>();
if (dataBuilderService != null)
{
    dataBuilderService.InitData().GetAwaiter().GetResult();
}

app.Run();
