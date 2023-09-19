using Microsoft.EntityFrameworkCore;
using ApiStudyBuddy.Data;
using Microsoft.Identity.Client;
using NTCStudyBuddyMVC;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Deserializes the AppConfig section and injects the resulting object - making it available to the rest of our application.
builder.Services.Configure<AppConfig>(builder.Configuration.GetSection("AppConfig"));

builder.Services.AddDbContext<ApiStudyBuddyContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
	.AddCookie(options =>
	{
		options.LoginPath = "/sign-in";
		options.LogoutPath = "/sign-out";
		options.AccessDeniedPath = "/access-denied";
		options.Cookie.Name = "UserAuth";
		options.ExpireTimeSpan = TimeSpan.FromDays(2);
		options.SlidingExpiration = true;
	});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
