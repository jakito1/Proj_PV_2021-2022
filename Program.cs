using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NutriFitWeb.Models;
using NutriFitWeb.Data;
using Microsoft.AspNetCore.Identity.UI.Services;
using NutriFitWeb.Services;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<IEmailSender>(new EmailSender (builder.Configuration.GetConnectionString("SendGridKey")));

builder.Services.AddScoped<ICountUsers, CountUsers>();

builder.Services.AddDefaultIdentity<UserAccountModel>(options => ***REMOVED*** 
    options.SignIn.RequireConfirmedAccount = true;
    options.User.RequireUniqueEmail = true;
***REMOVED***)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
***REMOVED***
    app.UseMigrationsEndPoint();
***REMOVED***
else
***REMOVED***
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
***REMOVED***

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "***REMOVED***controller=Home***REMOVED***/***REMOVED***action=Index***REMOVED***/***REMOVED***id?***REMOVED***");
app.MapRazorPages();

app.Run();
