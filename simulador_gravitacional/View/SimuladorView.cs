using simulador_gravitacional.Entidades;
    
namespace simulador_gravitacional.View;
    
public class SimuladorView
{
    // limite de largura e altura da tela padrao
    private int _larguraTela = 200;
    private int _alturaTela = 50; 
    
    // define a "distancia visivel" q temos do universo
    private double _zoom = 500000000.0; 
    
    public void Desenhar(Universo universo)
    {
        // se acontecer alguma colisão no universo
        if (universo.OcorreuColisao)
        {
            // altera o background do console para dar um efeito
            Console.BackgroundColor = ConsoleColor.DarkRed;
            
            // seta como false a propriedade para não piscar direto
            universo.OcorreuColisao = false;
        }
        else
        {
            // se não tiver com colisão, continua o fundo default
            Console.BackgroundColor = ConsoleColor.Black;
        }
        
        Console.Clear(); // limpa a tela do frame anterior
    
        foreach (var corpo in universo.Corpos)
        {
            // calculo dos metros fisicos em "pixeis" do console
            // somar metade da largura pra coordenada x = 0, y = 0 fiqeu no centro
            int telaX = (int)((corpo.PosX / _zoom) * _larguraTela) + (_larguraTela / 2);
            int telaY = (int)((corpo.PosY / _zoom) * _alturaTela) + (_alturaTela / 2);
    
            // calcula o tamanho do raio dos corpos pela escala da tela (x vai ser maior q y no console pra melhorar a visao)
            double raioX = (corpo.Raio / _zoom) * _larguraTela;
            double raioY = (corpo.Raio / _zoom) * _alturaTela;

            // define os limites da "box" onde o planeta vai ser desenhado
            int minX = (int)Math.Max(0, telaX - raioX);
            int maxX = (int)Math.Min(_larguraTela - 1, telaX + raioX);
            int minY = (int)Math.Max(0, telaY - raioY);
            int maxY = (int)Math.Min(_alturaTela - 1, telaY + raioY);

            // desenha o raio de cada corpo 
            for (int y = minY; y <= maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    // conta para corrigir o achatamento da view do console
                    double dx = (raioX > 0.5) ? Math.Pow((x - telaX) / raioX, 2) : 0;
                    double dy = (raioY > 0.5) ? Math.Pow((y - telaY) / raioY, 2) : 0;

                    // se a distancia <= 1.0, ta tudo dentro do raio
                    if (dx + dy <= 1.0)
                    {
                        Console.SetCursorPosition(x, y);
                        Console.Write("|"); // representação do raio
                    }
                }
            }

            // seta a posição central do corpo, o "nucleo" como a letra inicial do nome do corpo
            if (telaX >= 0 && telaX < _larguraTela && telaY >= 0 && telaY < _alturaTela)
            {
                Console.SetCursorPosition(telaX, telaY);
                Console.Write(corpo.Nome[0]); 
            }
        }
            
        // empurra o cursor pra baixo pra não piscar em cima dos planetas
        Console.SetCursorPosition(0, _alturaTela + 1);
    }
}