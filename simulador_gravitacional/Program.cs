using simulador_gravitacional.Infraestrutura.Data;

namespace simulador_gravitacional;

class Program
{
    public static void Main(string[] args)
    {
        GerenciadorDb.Iniciar(TipoPersistencia.ARQUIVO);
    }
}