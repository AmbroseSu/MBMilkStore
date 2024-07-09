using Repository.Interfaces;
using Repository;
using Service;
using Service.Interfaces;
using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using DataAccessLayer.DAO;
using BussinessObject;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddRazorPages();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddDbContext<M_BMilkStoreDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DB"));

});
builder.Services.AddScoped(typeof(UserDAO));
builder.Services.AddScoped(typeof(UserRoleDAO));


builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserRoleRepository, UserRoleRepository>();
builder.Services.AddScoped<IProductLineService, ProductLineService>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRoleService, UserRoleService>();
builder.Services.AddSingleton<IProductRepository, ProductRepository>();
builder.Services.AddSingleton<IProductService, ProductService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});
var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<M_BMilkStoreDBContext>();
        await SeedDataAsync(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred seeding the DB.");
    }
}

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
app.UseSession();
app.UseSession();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
async Task SeedDataAsync(M_BMilkStoreDBContext context)
{
    // Ensure database is created
    await context.Database.EnsureCreatedAsync();

    // Check if data already exists
    if (!await context.UserRoles.AnyAsync())
    {
        await context.UserRoles.AddRangeAsync(
            new UserRole { UserRoleId = 1, UserRoleName = "Admin" },
            new UserRole { UserRoleId = 2, UserRoleName = "Customer" },
            new UserRole { UserRoleId = 3, UserRoleName = "Staff" }

        );
        await context.SaveChangesAsync();
    }
}
