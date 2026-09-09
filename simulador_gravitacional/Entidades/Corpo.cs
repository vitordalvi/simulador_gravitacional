namespace simulador_gravitacional.Entities;

public class Corpo
{
    public string Nome { get; set; }
    public double Massa { get; set; }
    public double Densidade { get; set; }
    public double PosX { get; set; }
    public double PosY { get; set; }
    public double VelX { get; set; }
    public double VelY { get; set; }

    // Retornar o raio atualizado com base nas propriedades no momento
    // Em que o atributo foi chamado
    public double Raio
    {
        get
        {
            double volume = Massa / Densidade;
            return Math.Cbrt((3 * volume) / (4 * Math.PI));
        }
    }

    public Corpo(string nome, double massa, double densidade, double posX, double posY, double velX, double velY)
    {
        Nome = nome;
        Massa = massa;
        Densidade = densidade;
        PosX = posX;
        PosY = posY;
        VelX = velX;
        VelY = velY;
    }
}