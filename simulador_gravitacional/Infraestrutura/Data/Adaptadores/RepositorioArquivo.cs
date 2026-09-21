using simulador_gravitacional.Entidades;
using simulador_gravitacional.Infraestrutura.Repositorios;

namespace simulador_gravitacional.Infraestrutura.Data.Adaptadores;

// Classe herdada do repositório padrão, implementando os métodos principais
public class RepositorioArquivo : RepositorioUniverso
{
    // variáveis utilitárias para acesso aos caminhos estaticos do simulador
    private static string _caminhoBase = AppContext.BaseDirectory;
    private static string _caminhoPastaDb = Path.Combine(_caminhoBase, "Data", "db");
    
    // método herdado do repositorio padrao para salvar quando o usuário vai usar a persistencia por arquivo
    public override void Salvar(Universo universo)
    {
        // se o ambiente não existe (checado por meio da funcao utilitária)
        if (!AmbienteExiste())
        {
            // cria o ambiente dos arquivos (por meio da função utilitária)
            CriarAmbiente();
        }

        // concatena o caminho do arquivo que será escrito
        string caminhoArquivo = Path.Combine(_caminhoPastaDb, "universo.txt");

        // utiliza StreamWriter para fazer a escrita dos arquivos, sempre vai reescrever por causa do append = false
        using (StreamWriter writer = new StreamWriter(caminhoArquivo, false))
        {
            // try para dar o catch caso dê algum erro
            try
            {
                // escreve a primeira linha padrao (infos do universo)
                writer.WriteLine(
                    $"{universo.Corpos.Count};{universo.QuantidadeIteracoes};{universo.TempoEntreIteracoes}");

                // começa a parte dos corpos, que vai ter mais campos
                foreach (var corpo in universo.Corpos)
                {
                    writer.WriteLine(
                        $"{corpo.Nome};{corpo.Massa};{corpo.Densidade};{corpo.PosX};{corpo.PosY};{corpo.VelX};{corpo.VelY}");
                }
            }
            // se deu algum problema, da o catch na exception
            catch (Exception ex)
            {
                // joga o erro da exception com uma mensagem pro usuário
                throw new Exception("Erro ao tentar salvar o arquivo.", ex);
            }
        }
        
    }

    // método herdado da "interface" (classe mae) para carregar a persistencia que já foi feita pelo arquivo
    public override Universo Carregar()
    {
        // se o ambiente não existe
        if (!AmbienteExiste())
        {
            // retorna mensagem para o usuário
            Console.WriteLine("Nenhum arquivo encontrado.");
            // universo vai ser nulo, aí pode escolher entre iniciar um novo por exemplo
            return null;
        }

        // concatenação do arquivo, talvez poderia transformar isso na própria variável estatica la em cima
        string caminhoArquivo = Path.Combine(_caminhoPastaDb, "universo.txt");
        
        // armazena todas as linhas no vetor
        string[] linhas = File.ReadAllLines(caminhoArquivo);
        
        // variavel pra guardar o estado de primeira linha, porque precisamos carregar ela diferente, já
        // que ela contem menos informações do que os corpos 
        bool PrimeiraLinha = true;

        // instancia do universo e da lista de corpos (eles serão utilizados em memória, durante a simulação)
        Universo universo = new Universo();
        List<Corpo> corpos = new List<Corpo>();
        
        int quantidadeIteracoes = 0;
        double tempoEntreIteracoes = 0;

        // passando por cada linha
        foreach (string linha in linhas)
        {
            // se a linha for nula ou em branco, pula ela (pra não dar problema de parar o nosso simulador)
            if (string.IsNullOrWhiteSpace(linha))
            {
                continue;
            }
            
            // divide a linha em partes, a cada ";"
            string[] partes =  linha.Split(';');

            // se for a primeira linha, armazena as informações do universo
            if (PrimeiraLinha)
            {
                quantidadeIteracoes = int.Parse(partes[1]);
                tempoEntreIteracoes = double.Parse(partes[2]);
                
                Console.WriteLine($"--- Carregando as informações do Universo ---");
                Console.WriteLine($"Quantidade de Corpos: {partes[0]}\n" +
                                  $"Quantidade de Iterações: {partes[1]}" +
                                  $"Tempo entre iterações: {partes[2]}");
                
                // remove o estado de primeira linha
                PrimeiraLinha = false;
                continue; // continua o loop
            }

            // começa a criação de cada corpo
            // alocando os campos correspondentes
            string nomeCorpo = partes[0];
            double massa =  double.Parse(partes[1]);
            double densidade = double.Parse(partes[2]);
            double posX = double.Parse(partes[3]);
            double posY = double.Parse(partes[4]);
            double velX = double.Parse(partes[5]);
            double velY = double.Parse(partes[6]);
            
            // instancia desse novo corpo
            Corpo corpo = new Corpo(nomeCorpo, massa, densidade, posX, posY, velX, velY);
            corpos.Add(corpo); // adiciona ele na lista de corpos
        }
        
        // utiliza a lista de corpos instanciada como atributo do corpo, além dos outros atributos
        universo.Corpos = corpos;
        universo.QuantidadeIteracoes = quantidadeIteracoes;
        universo.TempoEntreIteracoes = tempoEntreIteracoes;
        
        return universo;
    }

    // função auxiliar para criar o ambiente
    private bool CriarAmbiente()
    {
        // se o ambiente não existe
        if (!AmbienteExiste())
        {
            // tenta criar o diretorio de acordo com os caminhos oferecidos
            try
            {
                Directory.CreateDirectory(_caminhoPastaDb);
                
                File.Create(Path.Combine(_caminhoPastaDb, "universo.txt")).Close();
                return true; // criou
            }
            // deu problema
            catch (Exception ex)
            {
                Console.WriteLine($"Houve uma falha ao tentar criar a bae de dados: {ex}");
                // retorna false, não vamos dar throw na exceção pq vamos utilizar o estado que a variável
                // vai devolver para tratar, no caso o valor booleano
                return false;
            }
        }

        // se não deu certo a criação, retorna false
        return false;
    }
    
    // função auxiliar para verificar se o ambiente existe
    private bool AmbienteExiste()
    {
        if (Directory.Exists(_caminhoPastaDb))
        {
            return true;
        }

        return false;
    }
}