using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using AppDonnyCuevas20210074.Data;

var builder = WebApplication.CreateBuilder(args);

// Base de datos
builder.Services.AddDbContext<TurismoJimaniContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("TurismoJimani")));

// Autenticación mediante cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Cuenta/Login";
        options.LogoutPath = "/Cuenta/Logout";
        options.AccessDeniedPath = "/Cuenta/AccesoDenegado";

        options.Cookie.Name = "TurismoJimani.Auth";
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        options.Cookie.SameSite = SameSiteMode.Lax;

        options.ExpireTimeSpan = TimeSpan.FromHours(2);
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization();

// Todas las acciones requieren usuario autenticado, salvo las marcadas con [AllowAnonymous]
builder.Services.AddControllersWithViews(options =>
{
    var politica = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();

    options.Filters.Add(new AuthorizeFilter(politica));
});

var app = builder.Build();

// Roles (Administrador, Editor) y usuario administrador inicial
DbInitializer.Inicializar(app.Services);

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// IMPORTANTE: primero autenticación y luego autorización
app.UseAuthentication();
app.UseAuthorization();

// Evita que el navegador guarde en caché las páginas (tras el Logout no se
// podrá volver a ver una página protegida con el botón "Atrás")
app.Use(async (context, next) =>
{
    context.Response.OnStarting(() =>
    {
        context.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
        context.Response.Headers["Pragma"] = "no-cache";
        context.Response.Headers["Expires"] = "0";
        return Task.CompletedTask;
    });

    await next();
});

// La aplicación inicia en /Cuenta/Login (si ya inició sesión, va al menú)
app.MapGet("/", context =>
{
    var destino = context.User.Identity?.IsAuthenticated == true
        ? "/Home/Index"
        : "/Cuenta/Login";

    context.Response.Redirect(destino);
    return Task.CompletedTask;
}).AllowAnonymous();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
