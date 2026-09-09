using simulador_gravitacional.Entities;
using simulador_gravitacional.Infraestrutura.Repositorios;

namespace simulador_gravitacional.Infraestrutura.Data.Adaptadores;

public class RepositorioArquivo : RepositorioUniverso
{
    public override void Salvar(Universo universo)
    {
        throw new NotImplementedException();
    }

    public override Universo Carregar()
    {
        throw new NotImplementedException();
        //return new Universo();
    }
}