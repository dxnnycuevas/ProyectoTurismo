namespace AppDonnyCuevas20210074.Helpers;

// Formatos de presentación usados en las vistas
public static class Formato
{
    private static readonly string[] Dias =
        { "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado", "Domingo" };

    public static string SiNo(bool valor) => valor ? "Sí" : "No";

    public static string SiNo(bool? valor) => valor switch
    {
        true => "Sí",
        false => "No",
        null => "Sin evaluar"
    };

    public static string Hora(TimeSpan? valor) => valor?.ToString(@"hh\:mm") ?? "";

    public static string Fecha(DateTime? valor) => valor?.ToString("dd/MM/yyyy HH:mm") ?? "";

    public static string Numero(decimal? valor) => valor?.ToString("#,##0.########") ?? "";

    public static string DiaSemana(byte dia) =>
        dia >= 1 && dia <= 7 ? Dias[dia - 1] : "";

    public static string NivelPrecio(byte? nivel) =>
        nivel is >= 1 and <= 4 ? new string('$', nivel.Value) : "";

    public static string Recortar(string? texto, int maximo = 80) =>
        string.IsNullOrEmpty(texto) || texto.Length <= maximo
            ? texto ?? ""
            : texto[..maximo] + "…";
}
