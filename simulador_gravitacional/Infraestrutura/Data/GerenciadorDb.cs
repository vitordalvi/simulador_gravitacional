using simulador_gravitacional.Infraestrutura.Data.Adaptadores;
using simulador_gravitacional.Infraestrutura.Repositorios;

namespace simulador_gravitacional.Infraestrutura.Data;

public abstract class GerenciadorDb
{
    public static RepositorioUniverso Iniciar(TipoPersistencia tipoPersistencia)
    {
        switch (tipoPersistencia)
        {
            case TipoPersistencia.ARQUIVO:
                return new RepositorioArquivo();
            
            case TipoPersistencia.MYSQL:
                throw new NotImplementedException("MySQL ainda não foi implementado.");
            // return new RepositorioMysql();
            
            default:
                throw new Exception("Tipo de persistencia inválido.");
        }
    }
}