using simulador_gravitacional.Entities;
using simulador_gravitacional.Infraestrutura.Repositorios;

namespace simulador_gravitacional.Infraestrutura.Data.Adaptadores;

public class RepositorioArquivo : RepositorioUniverso
{
    private static string _caminhoBase = AppContext.BaseDirectory;
    private static string _caminhoPastaDb = Path.Combine(_caminhoBase, "Data", "db");
    
    public override void Salvar(Universo universo)
    {
        throw new NotImplementedException();
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
        List<Corpo> Corpos = new List<Corpo>();

        foreach (string linha in linhas)
        {
            if (string.IsNullOrWhiteSpace(linha))
            {
                continue;
            }
            
            string[] partes =  linha.Split(';');

            if (PrimeiraLinha)
            {
                Console.WriteLine($"Carregando as informações do Universo:");
                Console.WriteLine($"Quantidade de Corpos: {partes[0]}\n" +
                                  $"Quantidade de Iterações: {partes[1]}" +
                                  $"Tempo entre iterações: {partes[2]}");
                
                PrimeiraLinha = false;
            }
            
            // Vai dar b.o se for as primeiras linhas (universo) eu acho, possivelmente corrigir

            string NomeCorpo = partes[0];
            double Massa =  double.Parse(partes[1]);
            double Densidade = double.Parse(partes[2]);
            double PosX = double.Parse(partes[3]);
            double PosY = double.Parse(partes[4]);
            double VelX = double.Parse(partes[5]);
            double VelY = double.Parse(partes[6]);
            
            Corpo corpo = new Corpo(NomeCorpo, Massa, Densidade, PosX, PosY, VelX, VelY);
            Corpos.Add(corpo);
            
            
        }
        
        throw new NotImplementedException();
        //return new Universo();
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