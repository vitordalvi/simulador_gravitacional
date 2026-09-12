using simulador_gravitacional.Entidades;
using simulador_gravitacional.Infraestrutura.Data;
using simulador_gravitacional.Infraestrutura.Repositorios;
using simulador_gravitacional.View;

namespace simulador_gravitacional;

class Program
{
    public static void Main(string[] args)
    {
        var db = GerenciadorDb.Iniciar(TipoPersistencia.ARQUIVO);
        Universo universo = null;

        Console.WriteLine("SIMULADOR GRAVITACIONAL");
        Console.WriteLine("1 - Iniciar Cenário de Teste (Terra vs Meteoro)");
        Console.WriteLine("2 - Carregar Simulação Salva");
        Console.Write("Escolha uma opção: ");
        string opcao = Console.ReadLine();

        if (opcao == "2")
        {
            universo = db.Carregar();
            // se o arquivo não existir ou vier vazio, forçamos a criação mockada
            if (universo == null || universo.Corpos == null || universo.Corpos.Count == 0)
            {
                Console.WriteLine("Falha ao carregar ou arquivo vazio. Iniciando novo cenário...");
                Thread.Sleep(2000);
                opcao = "1"; 
            }
        }

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