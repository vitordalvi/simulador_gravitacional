using simulador_gravitacional.Entidades;
using simulador_gravitacional.Infraestrutura.Repositorios;

namespace simulador_gravitacional.Infraestrutura.Data.Adaptadores;

public class RepositorioArquivo : RepositorioUniverso
{
    private static string _caminhoBase = AppContext.BaseDirectory;
    private static string _caminhoPastaDb = Path.Combine(_caminhoBase, "Data", "db");
    
    public override void Salvar(Universo universo)
    {
        if (!AmbienteExiste())
        {
            CriarAmbiente();
        }

        string caminhoArquivo = Path.Combine(_caminhoPastaDb, "universo.txt");

        using (StreamWriter writer = new StreamWriter(caminhoArquivo, false))
        {
            try
            {
                writer.WriteLine(
                    $"{universo.Corpos.Count};{universo.QuantidadeIteracoes};{universo.TempoEntreIteracoes}");

                foreach (var corpo in universo.Corpos)
                {
                    writer.WriteLine(
                        $"{corpo.Nome};{corpo.Massa};{corpo.Densidade};{corpo.PosX};{corpo.PosY};{corpo.VelX};{corpo.VelY}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao tentar salvar o arquivo.", ex);
            }
        }
        
    }

    public override Universo Carregar()
    {
        if (!AmbienteExiste())
        {
            Console.WriteLine("Nenhum arquivo encontrado.");
            return null;
        }

        string caminhoArquivo = Path.Combine(_caminhoPastaDb, "universo.txt");
        string[] linhas = File.ReadAllLines(caminhoArquivo);

        bool PrimeiraLinha = true;

        Universo universo = new Universo();
        List<Corpo> corpos = new List<Corpo>();
        int quantidadeIteracoes = 0;
        DateTime tempoEntreIteracoes = new DateTime();

        foreach (string linha in linhas)
        {
            if (string.IsNullOrWhiteSpace(linha))
            {
                continue;
            }
            
            string[] partes =  linha.Split(';');

            if (PrimeiraLinha)
            {
                quantidadeIteracoes = int.Parse(partes[1]);
                tempoEntreIteracoes = DateTime.Parse(partes[2]);
                
                Console.WriteLine($"Carregando as informações do Universo:");
                Console.WriteLine($"Quantidade de Corpos: {partes[0]}\n" +
                                  $"Quantidade de Iterações: {partes[1]}" +
                                  $"Tempo entre iterações: {partes[2]}");
                
                PrimeiraLinha = false;
                continue;
            }

            string nomeCorpo = partes[0];
            double massa =  double.Parse(partes[1]);
            double densidade = double.Parse(partes[2]);
            double posX = double.Parse(partes[3]);
            double posY = double.Parse(partes[4]);
            double velX = double.Parse(partes[5]);
            double velY = double.Parse(partes[6]);
            
            Corpo corpo = new Corpo(nomeCorpo, massa, densidade, posX, posY, velX, velY);
            corpos.Add(corpo);
        }
        
        universo.Corpos = corpos;
        universo.QuantidadeIteracoes = quantidadeIteracoes;
        universo.TempoEntreIteracoes = tempoEntreIteracoes;
        
        return universo;
    }

    private bool CriarAmbiente()
    {
        if (!AmbienteExiste())
        {
            try
            {
                Directory.CreateDirectory(_caminhoPastaDb);
                
                File.Create(Path.Combine(_caminhoPastaDb, "universo.txt")).Close();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Houve uma falha ao tentar criar a bae de dados: {ex}");
                return false;
            }
        }

        return false;
    }
    
    private bool AmbienteExiste()
    {
        if (Directory.Exists(_caminhoPastaDb))
        {
            return true;
        }

        return false;
    }
}