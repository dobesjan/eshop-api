using Auth0.AspNetCore.Authentication;
using Eshop.Api.BusinessLayer;
using Eshop.Api.BusinessLayer.Services;
using Eshop.Api.BusinessLayer.Services.Contacts;
using Eshop.Api.BusinessLayer.Services.Currencies;
using Eshop.Api.BusinessLayer.Services.Images;
using Eshop.Api.BusinessLayer.Services.Orders;
using Eshop.Api.BusinessLayer.Services.Products;
using Eshop.Api.BusinessLayer.Services.Tokens;
using Eshop.Api.DataAccess.Data;
using Eshop.Api.DataAccess.UnitOfWork;
using Eshop.UI.ActionFilters;
using Eshop.UI.Configuration;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<AuthenticationOptions>(builder.Configuration.GetSection("Authentication"));
builder.Services.Configure<AppSessionOptions>(builder.Configuration.GetSection("Session"));

builder.Services.Configure<CookiePolicyOptions>(options =>
	options.MinimumSameSitePolicy = SameSiteMode.None
);

builder.Services.AddAuth0WebAppAuthentication(options =>
{
	var authOptions = builder.Configuration.GetSection("Authentication").Get<AuthenticationOptions>();
	options.Domain = authOptions.Domain;
	options.ClientId = authOptions.ClientId;
	options.ClientSecret = authOptions.ClientSecret;
	options.Scope = "openid profile email";
	options.CallbackPath = new PathString("/callback");
});

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
	options.Filters.Add<SessionTokenActionFilter>();
});

builder.Services.AddSession(options =>
{
	var sessionOptions = builder.Configuration.GetSection("Authentication").Get<AppSessionOptions>();
	options.IdleTimeout = TimeSpan.FromHours(sessionOptions.IdleTimeout);
	options.Cookie.HttpOnly = true;
	options.Cookie.IsEssential = true;
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddEshopServices();

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

//forward headers from the LB
var forwardOpts = new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedFor
};

forwardOpts.KnownNetworks.Clear();
forwardOpts.KnownProxies.Clear();
app.UseForwardedHeaders(forwardOpts);

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
