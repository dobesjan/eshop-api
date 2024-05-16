using Eshop.Api.BusinessLayer.Services;
using Eshop.Api.BusinessLayer.Services.Images;
using Eshop.Api.BusinessLayer.Services.Interfaces;
using Eshop.Api.BusinessLayer.Services.Interfaces.Images;
using Eshop.Api.BusinessLayer.Services.Interfaces.Products;
using Eshop.Api.BusinessLayer.Services.Products;
using Eshop.Api.DataAccess.Data;
using Eshop.Api.DataAccess.Repository;
using Eshop.Api.DataAccess.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddScoped<IEshopService, EshopService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IImageService, ImageService>();

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

app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
