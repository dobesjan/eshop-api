using Eshop.Api.BusinessLayer.Services;
using Eshop.Api.BusinessLayer.Services.Currencies;
using Eshop.Api.BusinessLayer.Services.Images;
using Eshop.Api.BusinessLayer.Services.Orders;
using Eshop.Api.BusinessLayer.Services.Products;
using Eshop.Api.BusinessLayer.Services.Tokens;
using Eshop.Api.DataAccess.Data;
using Eshop.Api.DataAccess.UnitOfWork;
using Eshop.UI.ActionFilters;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
	options.Filters.Add<SessionTokenActionFilter>();
});

builder.Services.AddSession(options =>
{
	//TODO: Make it configurable
	options.IdleTimeout = TimeSpan.FromHours(168);
	options.Cookie.HttpOnly = true;
	options.Cookie.IsEssential = true;
});

builder.Services.AddHttpContextAccessor();

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

builder.Services.AddScoped<ITokenService, TokenService>();

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

app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
