using simulador_gravitacional.Entidades;

namespace simulador_gravitacional.Infraestrutura.Repositorios;

// Classe abstrata que serve como interface dos métodos que deverão ser implementados pelo repositório
public abstract class RepositorioUniverso
{
    public abstract void Salvar(Universo universo);
    public abstract Universo Carregar();
}