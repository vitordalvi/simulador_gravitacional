using simulador_gravitacional.Entidades;
using simulador_gravitacional.Infraestrutura.Data;
using simulador_gravitacional.Infraestrutura.Repositorios;
using simulador_gravitacional.View;

namespace simulador_gravitacional;

class Program
{
    public static void Main(string[] args)
    {
        // start das funções pré-inicializadas
        var db = GerenciadorDb.Iniciar(TipoPersistencia.ARQUIVO);
        Universo universo = null;

        Console.WriteLine("SIMULADOR GRAVITACIONAL");
        Console.WriteLine("1 - Iniciar Cenário de Teste (Terra vs Meteoro)");
        Console.WriteLine("2 - Carregar Simulação Salva");
        Console.WriteLine("3 - Iniciar Cenário Aleatório");
        Console.Write("Escolha uma opção: ");
        string opcao = Console.ReadLine();

        
        // se usuário deseja carregar uma simulação salva
        if (opcao == "2")
        {
            universo = db.Carregar();
            // se o arquivo não existir ou vier vazio, forçamos a criação mockada
            if (universo == null || universo.Corpos == null || universo.Corpos.Count == 0)
            {
                Console.WriteLine("Falha ao carregar ou arquivo vazio. Iniciando novo cenário...");
                
                // delay visual para experiencia do usuario
                Thread.Sleep(2000);
                opcao = "1"; 
            }
        }
        else if (opcao == "3")
        {
            universo = new Universo();
            universo.Corpos = new List<Corpo>();
            Random rand = new Random();
            
            // Estrela grande massiva não vai engolir a tela porque tá com uma densidade muito grande
            Corpo estrela = new Corpo("Estrela", 3e27, 500000.0, 0, 0, 0, 0);
            universo.Corpos.Add(estrela);

            int numPlanetas = rand.Next(3, 7); // quantidade aleatória de planetas (corpos) a serem adicionados

            for (int i = 0; i < numPlanetas; i++)
            {
                // massa do planeta
                double massa = (rand.NextDouble() * 5 + 0.1) * Math.Pow(10, rand.Next(22, 24));
                
                // densidade mais alta também para ficarem como pequenos pontos
                double densidade = rand.NextDouble() * 5000 + 5000; 
                
                // Distância aleatória do centro, segura fora da estrela (entre 50.000.000 e 400.000.000)
                double distancia = rand.NextDouble() * 350000000 + 50000000;
                
                // Ângulo aleatório
                double angulo = rand.NextDouble() * Math.PI * 2;
                
                double posX = Math.Cos(angulo) * distancia;
                double posY = Math.Sin(angulo) * distancia;

                // Velocidade para órbita circular: v = sqrt(G * M / r)
                // G = 6.674184e-11
                double velocidadeOrbital = Math.Sqrt(6.674184e-11 * estrela.Massa / distancia);
                
                // Vetor velocidade perpendicular ao vetor posição
                double velX = -Math.Sin(angulo) * velocidadeOrbital;
                double velY = Math.Cos(angulo) * velocidadeOrbital;

                Corpo planeta = new Corpo($"Planeta_{i+1}", massa, densidade, posX, posY, velX, velY);
                universo.Corpos.Add(planeta);
            }
            universo.QuantidadeIteracoes = 800; // Mais iterações para ver a órbita
            universo.TempoEntreIteracoes = 200.0; // Um tempo maior por frame 
        }

        // se usuário for criar o cenário teste padrão
        if (opcao == "1" || universo == null)
        {
            Corpo corpo = new Corpo("Terra", 5.97e24, 5.0, 0, 0, 0, 0); 
            Corpo corpo2 = new Corpo("Meteoro", 7.34e22, 5.0, 200000000, 0, -20000000, 0); 
            
            universo = new Universo();
            universo.Corpos = new List<Corpo>();
            universo.Corpos.Add(corpo);
            universo.Corpos.Add(corpo2);
            universo.QuantidadeIteracoes = 100; // quantos frames vai durar a simulação
            universo.TempoEntreIteracoes = 1.0;
        }

        // instancia a classe responsável pela visualização 
        SimuladorView tela = new SimuladorView(); 

        // loop para mostrar o universo
        for (int i = 0; i < universo.QuantidadeIteracoes; i++)
        {
            universo.AtualizarProximoPasso();
            tela.Desenhar(universo);
            Thread.Sleep(100); // 100 milissegundos para dar um ar de "FPS" suave
        }

        // fim da Simulação
        Console.Clear();
        Console.WriteLine("Simulação finalizada!");
        Console.Write("Deseja salvar a posição final do universo? (S/N): ");
        string salvar = Console.ReadLine();

        if (salvar != null && salvar.ToUpper() == "S")
        {
            db.Salvar(universo);
            Console.WriteLine("Universo salvo com sucesso na sua pasta Data/db1");
        }
    }
}