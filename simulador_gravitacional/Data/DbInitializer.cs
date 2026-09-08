namespace simulador_gravitacional.Data;

public abstract class DbInitializer
{
    private static string basePath = AppContext.BaseDirectory;
    private static string dbFolderPath = Path.Combine(basePath, "Data", "db");
    private static string[] entities = ["Body", "Universe"];
    
    // pensar em alguma forma de como fazer essa classe abstrata

}