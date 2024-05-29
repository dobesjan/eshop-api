using Eshop.Api.BusinessLayer.Services;
using Eshop.Api.BusinessLayer.Services.Currencies;
using Eshop.Api.BusinessLayer.Services.Images;
using Eshop.Api.BusinessLayer.Services.Orders;
using Eshop.Api.BusinessLayer.Services.Products;
using Eshop.Api.DataAccess.Data;
using Eshop.Api.DataAccess.Repository;
using Eshop.Api.DataAccess.Repository.Contacts;
using Eshop.Api.DataAccess.Repository.Currencies;
using Eshop.Api.DataAccess.Repository.Orders;
using Eshop.Api.DataAccess.UnitOfWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowAllOrigins",
		builder =>
		{
			builder.AllowAnyOrigin()
				   .AllowAnyMethod()
				   .AllowAnyHeader();
		});
});

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//TODO: Consider how to refactor
builder.Services.AddScoped<IEshopService, EshopService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IImageService, ImageService>();

builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ICurrencyService, CurrencyService>();

// Configure authentication

var domain = builder.Configuration["Authentication:Domain"];
var audience = builder.Configuration["Authentication:Audience"];

builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
	options.Authority = domain;
	options.Audience = audience;
	options.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuer = true,
		ValidateAudience = true,
		ValidateLifetime = true,
		ValidateIssuerSigningKey = true,
		ValidIssuer = domain,
		ValidAudience = audience
	};
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

app.UseCors("AllowAllOrigins");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
