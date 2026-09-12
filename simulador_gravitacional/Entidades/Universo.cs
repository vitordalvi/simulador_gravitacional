namespace simulador_gravitacional.Entidades;

public class Universo
{
    public List<Corpo> Corpos { get; set; }
    public int QuantidadeIteracoes { get; set; }

    public DateTime
        TempoEntreIteracoes
    {
        get;
        set;
    } // Transformar isso no get {} com a funcao retornando direto o tempo entre cada iteracao (iteracao - datetime.now) algo assim

    private void CalcularForcas()
    {
        // Passar por todos os corpos, ver quem está puxando quem,
        // Calcular a força da gravidade e mudar a velocidade deles (baseado no tempo da iteração)
    }

    private void AtualizarPosicoes()
    {
        // Com as novas velocidades, empurar os corpos no mapa (mudar o x e y)
    }

    private void TratarColisoes()
    {
        // Matemática, se os raios se encostarem, os corpos devem fundir ou quicar?
        // Usar a conservação da quantidade de movimento (Q = m * v) para calcular a nova velocidade
    }

    public void AtualizarProximoPasso()
    {
        // O Program vai chamar em loop, para fazer o cálculo do que vai ser apresentado
        // Vamo usar os métodos internos dessa classe aq
    }
}