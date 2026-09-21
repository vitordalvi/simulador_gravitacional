using simulador_gravitacional.Infraestrutura.Data.Adaptadores;
using simulador_gravitacional.Infraestrutura.Repositorios;

namespace simulador_gravitacional.Infraestrutura.Data;

public class GerenciadorDb
{
    public static RepositorioUniverso Iniciar(TipoPersistencia tipoPersistencia)
    {
        switch (tipoPersistencia)
        {
            // se o tipo de persistencia escolhido for arquivo, instancia o repositorio arquivo
            case TipoPersistencia.ARQUIVO:
                return new RepositorioArquivo();
            
            // não utilizamos ainda o mysql e tem q setar toda a criação do ambiente, entao deixar pra proxima
            case TipoPersistencia.MYSQL:
                throw new NotImplementedException("MySQL ainda não foi implementado.");
            // return new RepositorioMysql();
            
            // se não tiver dentro dos enums validos, retorna que o tipo da persistencia eh inválido
            default:
                throw new Exception("Tipo de persistencia inválido.");
        }
    }
}