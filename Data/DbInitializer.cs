using AppDonnyCuevas20210074.Models;
using Microsoft.AspNetCore.Identity;

namespace AppDonnyCuevas20210074.Data
{
    public static class DbInitializer
    {
        public static void Inicializar(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<TurismoJimaniContext>();

            foreach (var nombreRol in new[] { "Administrador", "Editor" })
            {
                if (!db.Roles.Any(r => r.Nombre == nombreRol))
                {
                    db.Roles.Add(new Rol { Nombre = nombreRol });
                }
            }
            db.SaveChanges();

            if (!db.Usuarios.Any())
            {
                var rolAdmin = db.Roles.First(r => r.Nombre == "Administrador");

                var admin = new Usuario
                {
                    IdRol = rolAdmin.IdRol,
                    NombreCompleto = "Administrador",
                    Correo = "admin@turismojimani.com",
                    Activo = true,
                    FechaCreacion = DateTime.Now
                };
                admin.ContrasenaHash = new PasswordHasher<Usuario>().HashPassword(admin, "Admin123*");

                db.Usuarios.Add(admin);
                db.SaveChanges();
            }
        }
    }
}
