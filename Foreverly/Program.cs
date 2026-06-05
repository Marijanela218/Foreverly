using Foreverly.Data;
using Foreverly.Hubs;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// =====================================================
// SERVICES
// =====================================================
builder.Services.AddControllersWithViews();

// DB Context
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// 🔴 SIGNALR (REAL-TIME SEATING)
builder.Services.AddSignalR();

var app = builder.Build();


// =====================================================
// 🔍 DEBUG DB CONNECTION (TEMPORARY)
// =====================================================
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var conn = db.Database.GetDbConnection();

    await conn.OpenAsync();

    Console.WriteLine("DB NAME: " + conn.Database);
    Console.WriteLine("DATA SOURCE: " + conn.DataSource);
}


// =====================================================
// SEED DATABASE (DEV ONLY)
// =====================================================
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();

    var context = scope.ServiceProvider
        .GetRequiredService<AppDbContext>();

    await context.Database.MigrateAsync();
    await DataSeeder.SeedAsync(context);
}

// =====================================================
// ERROR HANDLING
// =====================================================
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// =====================================================
// ROUTES
// =====================================================
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Weddings}/{action=Index}/{id?}"
);

// 🔴 SIGNALR HUB ROUTE
app.MapHub<SeatingHub>("/seatingHub");

app.Run();