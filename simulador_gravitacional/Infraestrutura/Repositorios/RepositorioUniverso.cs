using simulador_gravitacional.Entidades;

namespace simulador_gravitacional.Infraestrutura.Repositorios;

public abstract class RepositorioUniverso
{
    public abstract void Salvar(Universo universo);
    public abstract Universo Carregar();
}