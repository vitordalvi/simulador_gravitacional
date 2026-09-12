namespace simulador_gravitacional.Entidades;

public class Universo
{
    public bool OcorreuColisao { get; set; } = false;
    public List<Corpo> Corpos { get; set; }
    public int QuantidadeIteracoes { get; set; }

    public double TempoEntreIteracoes { get; set; }

    private void CalcularForcas()
    {
        // Passar por todos os corpos, ver quem está puxando quem,
        // Calcular a força da gravidade e mudar a velocidade deles (baseado no tempo da iteração)

        foreach (Corpo corpoA in Corpos)
        {
            foreach (Corpo corpoB in Corpos)
            {
                // se for o mesmo corpo, ele não deve se puxar, entao skippa  
                if (corpoA == corpoB)
                {
                    continue;
                }
                
                double distanciaX = corpoB.PosX - corpoA.PosX; // distancia entre os corpos no eixo x
                double distanciaY = corpoB.PosY - corpoA.PosY; // distancia entre os corpos no eixo y
                
                // pitagoras para achar a distancia absoluta entre eles
                double distancia = Math.Sqrt(distanciaX * distanciaX + distanciaY * distanciaY);

                // valor da gravidade (6.674184 elevado a -11)
                double gravidade = 6.674184e-11;
                
                // verificar se a formula ta certa, acho q tem q contar o G
                double forca = gravidade * (corpoA.Massa * corpoB.Massa) / (distancia * distancia);

                double aceleracao = forca / corpoA.Massa;

                double aceleracaoX = aceleracao * (distanciaX / distancia);
                double aceleracaoY = aceleracao * (distanciaY / distancia);

                corpoA.VelX += aceleracaoX * TempoEntreIteracoes;
                corpoA.VelY += aceleracaoY * TempoEntreIteracoes;
            }
        }
    }

    // So = posição que o corpo se encontra no início da contagem do tempo (t = 0)
    // S = posição final do corpo em um determinado momento
    // v = velocidade
    // t = tempo
    // fórmula = S = So + v * t
    private void AtualizarPosicoes()
    {
        foreach (var corpo in Corpos)
        {
            double novoPosX = corpo.PosX + corpo.VelX * TempoEntreIteracoes;
            double novoPosY = corpo.PosY + corpo.VelY * TempoEntreIteracoes;

            corpo.PosX = novoPosX;
            corpo.PosY = novoPosY;
        }
    }

    private void TratarColisoes()
    {
        // Matemática, se os raios se encostarem, os corpos devem fundir ou quicar?
        // Usar a conservação da quantidade de movimento (Q = m * v) para calcular a nova velocidade
        
        List<Corpo> corposDestruidos = new List<Corpo>();
        
        foreach (Corpo corpoA in Corpos)
        {
            foreach (Corpo corpoB in Corpos)
            {
                if (corpoA == corpoB)
                {
                    continue;
                }

                if (corposDestruidos.Contains(corpoA) || corposDestruidos.Contains(corpoB))
                {
                    continue;
                }
                
                double distanciaX = corpoB.PosX - corpoA.PosX; // distancia entre os corpos no eixo x
                double distanciaY = corpoB.PosY - corpoA.PosY; // distancia entre os corpos no eixo y
                
                // pitagoras para achar a distancia absoluta entre eles
                double distancia = Math.Sqrt(distanciaX * distanciaX + distanciaY * distanciaY);

                if (distancia <= (corpoA.Raio + corpoB.Raio))
                {
                    OcorreuColisao = true;
                    
                    double novaVelX = ((corpoA.Massa * corpoA.VelX) + (corpoB.Massa * corpoB.
                        VelX)) / (corpoA.Massa + corpoB.Massa);
                
                    double novaVelY = ((corpoA.Massa * corpoA.VelY) + (corpoB.Massa * corpoB.
                        VelY)) / (corpoA.Massa + corpoB.Massa);
                    
                    corpoA.VelX = novaVelX;
                    corpoA.VelY = novaVelY; 
                    corpoA.Massa += corpoB.Massa;
                    
                    corposDestruidos.Add(corpoB);
                }
            }
        }

        foreach (Corpo corpo in corposDestruidos)
        {
            Corpos.Remove(corpo);
        }
    }

    public void AtualizarProximoPasso()
    {
        CalcularForcas();
        AtualizarPosicoes();
        TratarColisoes();
        // dps colocar os proximos métodos aqui
        // O Program vai chamar em loop, para fazer o cálculo do que vai ser apresentado
        // Vamo usar os métodos internos dessa classe aq
    }
}