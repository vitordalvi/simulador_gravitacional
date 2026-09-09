using simulador_gravitacional.Entities;

namespace simulador_gravitacional.Infraestrutura.Repositorios;

public abstract class RepositorioUniverso
{
    public abstract void Salvar(Universo universo);
    public abstract Universo Carregar();
}