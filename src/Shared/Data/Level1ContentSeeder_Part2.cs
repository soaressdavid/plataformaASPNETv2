using Shared.Entities;
using Shared.Models;
using System.Text.Json;

namespace Shared.Data;

/// <summary>
/// Part 2 of Level 1 Content Seeder - Lessons 7-15
/// Covers string manipulation, StringBuilder, and file I/O
/// </summary>
public partial class Level1ContentSeeder
{
    private Lesson CreateLesson7()
    {
        var content = new LessonContent
        {
            Objectives = new List<string>
            {
                "Dominar métodos essenciais de manipulação de strings",
                "Usar Substring, Replace, Split e Join eficientemente",
                "Entender imutabilidade de strings"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Strings São Imutáveis",
                    Content = "Em C#, strings são imutáveis - uma vez criadas, não podem ser modificadas. Quando você faz 'str = str.ToUpper()', não está modificando a string original, está criando uma nova string em maiúsculas e atribuindo à variável. A string original continua existindo na memória até ser coletada pelo garbage collector. Isso tem implicações importantes: operações de string sempre criam novas strings, o que pode ser ineficiente se feito repetidamente. Por exemplo, concatenar strings em um loop cria muitas strings temporárias. Para construção de strings complexas, use StringBuilder. Imutabilidade torna strings thread-safe - múltiplas threads podem ler a mesma string sem problemas. Também permite otimizações como string interning.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Métodos de Manipulação",
                    Content = "C# oferece muitos métodos para trabalhar com strings. ToUpper() e ToLower() convertem caixa. Trim() remove espaços do início e fim. Substring(start, length) extrai parte da string. Replace(old, new) substitui texto. Split(separator) divide string em array. String.Join(separator, array) junta array em string. Contains(text) verifica se contém texto. StartsWith e EndsWith verificam início e fim. IndexOf retorna posição de texto. Length retorna tamanho. Esses métodos cobrem a maioria das necessidades. Combine-os para operações complexas: 'texto.Trim().ToLower().Replace(\" \", \"-\")' normaliza texto. Lembre-se: cada método retorna nova string.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Trabalhando com Substrings",
                    Content = "Substring é poderoso mas requer cuidado. Substring(start) retorna do índice até o fim. Substring(start, length) retorna length caracteres a partir de start. Índices começam em 0. Se start ou length são inválidos, lança exceção. Sempre verifique Length antes de usar Substring. Para extrair últimos N caracteres: 'str.Substring(str.Length - N)'. Para extrair entre dois delimitadores, use IndexOf para encontrar posições e Substring para extrair. Split é mais simples quando delimitadores são claros. Substring é útil para formatar dados: extrair ano de data, primeiros caracteres de nome, etc. Combine com Trim para remover espaços indesejados.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Métodos Básicos de String",
                    Code = @"string texto = ""  Olá, Mundo!  "";

// Trim - remove espaços
string limpo = texto.Trim();
Console.WriteLine($""'{limpo}'"");

// ToUpper e ToLower
Console.WriteLine(limpo.ToUpper());
Console.WriteLine(limpo.ToLower());

// Replace
string modificado = limpo.Replace(""Mundo"", ""C#"");
Console.WriteLine(modificado);

// Contains
bool temOla = limpo.Contains(""Olá"");
Console.WriteLine($""Contém 'Olá': {temOla}"");",
                    Language = "csharp",
                    Explanation = "Demonstra métodos essenciais: Trim, ToUpper, ToLower, Replace e Contains.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Split e Join",
                    Code = @"// Split - dividir string
string frase = ""maçã,banana,laranja,uva"";
string[] frutas = frase.Split(',');

Console.WriteLine(""Frutas:"");
foreach (string fruta in frutas)
{
    Console.WriteLine($""- {fruta}"");
}

// Join - juntar array
string[] palavras = { ""C#"", ""é"", ""incrível"" };
string resultado = string.Join("" "", palavras);
Console.WriteLine(resultado);",
                    Language = "csharp",
                    Explanation = "Split divide string em array usando separador. Join faz o oposto - junta array em string.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Normalizar Texto",
                    Description = "Crie uma função que recebe um texto e retorna ele em minúsculas, sem espaços extras no início/fim.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"string Normalizar(string texto)
{
    // Implemente aqui
}",
                    Hints = new List<string> 
                    { 
                        "Use Trim() para remover espaços",
                        "Use ToLower() para minúsculas",
                        "Encadeie os métodos" 
                    }
                },
                new Exercise
                {
                    Title = "Contar Palavras",
                    Description = "Crie uma função que conta quantas palavras há em uma frase (palavras separadas por espaço).",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"int ContarPalavras(string frase)
{
    // Implemente aqui
}",
                    Hints = new List<string> 
                    { 
                        "Use Split(' ') para dividir",
                        "Retorne o Length do array",
                        "Considere Trim() antes" 
                    }
                },
                new Exercise
                {
                    Title = "Extrair Domínio de Email",
                    Description = "Crie uma função que extrai o domínio de um email (parte depois do @).",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"string ExtrairDominio(string email)
{
    // Implemente aqui
}",
                    Hints = new List<string> 
                    { 
                        "Use IndexOf('@') para encontrar posição",
                        "Use Substring(posicao + 1) para extrair",
                        "Verifique se @ existe" 
                    }
                }
            },
            Summary = "Nesta aula você aprendeu que strings são imutáveis, dominou métodos essenciais como Trim, Replace, Split e Join, e aprendeu a trabalhar com Substring de forma segura. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno. Grandes empresas como Microsoft, Google e Amazon aplicam estes conceitos em seus sistemas. Ao aprender estas técnicas, você está adquirindo conhecimento usado por desenvolvedores em todo o mundo. Considere como este conceito poderia ser aplicado em um projeto pessoal. Pensar em aplicações práticas ajuda a solidificar o entendimento e torna o aprendizado mais significativo. Iniciantes frequentemente cometem alguns erros ao aprender este tópico. Conhecer estes erros comuns ajuda a evitá-los e desenvolver código mais robusto desde o início. Um erro comum é não considerar casos extremos ou situações inesperadas. Sempre teste seu código com diferentes tipos de entrada, incluindo valores nulos, vazios ou inválidos. Outro erro frequente é otimizar prematuramente. Primeiro faça o código funcionar corretamente, depois otimize se necessário. Como diz o ditado: 'premature optimization is the root of all evil'. Quando encontrar problemas, use o debugger para entender o que está acontecendo. Coloque breakpoints, inspecione variáveis e execute o código passo a passo. Esta é uma habilidade essencial para todo desenvolvedor. Mensagens de erro podem parecer intimidadoras no início, mas elas são suas aliadas. Leia-as cuidadosamente - elas geralmente indicam exatamente onde e qual é o problema. Se algo não funcionar como esperado, não desanime. Debugging é uma parte normal do desenvolvimento. Até desenvolvedores experientes passam tempo significativo depurando código. Testar seu código é tão importante quanto escrevê-lo. Testes automatizados garantem que seu código funciona corretamente e continuará funcionando quando você fizer mudanças. Comece escrevendo testes simples para os casos mais comuns. Depois adicione testes para casos extremos e situações de erro. Uma boa cobertura de testes dá confiança para refatorar e melhorar o código. Considere a prática de TDD (Test-Driven Development), onde você escreve os testes antes do código. Isso ajuda a pensar claramente sobre o que o código deve fazer. Embora a correção seja sempre a prioridade, é importante estar ciente das implicações de performance de suas escolhas. Diferentes abordagens podem ter custos computacionais muito diferentes. Em muitos casos, a diferença de performance é negligenciável para aplicações pequenas. No entanto, à medida que seus programas crescem, escolhas eficientes se tornam cada vez mais importantes. Aprenda a usar ferramentas de profiling para identificar gargalos de performance. Otimize baseado em dados reais, não em suposições sobre onde o código é lento. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0002-000000000007"),
            CourseId = _courseId,
            Title = "Manipulação de Strings",
            Duration = "50 min",
            Difficulty = "Iniciante",
            EstimatedMinutes = 50,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0002-000000000006" }),
            OrderIndex = 7,
            Content = "",
            StructuredContent = JsonSerializer.Serialize(content),
            Version = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    private Lesson CreateLesson8()
    {
        var content = new LessonContent
        {
            Objectives = new List<string>
            {
                "Entender quando usar StringBuilder vs string",
                "Construir strings eficientemente com StringBuilder",
                "Otimizar operações de concatenação de strings"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "O Problema da Concatenação",
                    Content = "Concatenar strings com + é conveniente mas ineficiente quando feito repetidamente. Cada concatenação cria uma nova string, descartando as anteriores. Em um loop de 1000 iterações, você cria 1000 strings temporárias. Isso desperdiça memória e tempo de CPU. O problema piora exponencialmente com o tamanho das strings. Para construir strings em loops ou com muitas operações, use StringBuilder. StringBuilder mantém um buffer mutável que pode ser modificado sem criar novas strings. Apenas no final, quando você chama ToString(), a string final é criada. Isso é muito mais eficiente. Para concatenações simples (2-3 strings), + é adequado. Para loops ou muitas operações, StringBuilder é essencial.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Usando StringBuilder",
                    Content = "StringBuilder está em System.Text. Crie com 'new StringBuilder()' ou especifique capacidade inicial se souber o tamanho aproximado. Append() adiciona ao final - aceita qualquer tipo. AppendLine() adiciona com quebra de linha. Insert(index, text) insere em posição. Remove(start, length) remove caracteres. Replace(old, new) substitui texto. Clear() limpa tudo. Length retorna tamanho atual. ToString() converte para string. StringBuilder é mutável - métodos modificam o objeto. Métodos retornam o próprio StringBuilder, permitindo encadeamento: 'sb.Append(\"A\").Append(\"B\").AppendLine()'. Especificar capacidade inicial evita redimensionamentos.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Quando Usar Cada Um",
                    Content = "Use string + quando: concatena 2-3 strings, operação única, código simples e legível. Use StringBuilder quando: concatena em loop, muitas operações de modificação, constrói strings grandes, performance é crítica. Regra prática: se você vê concatenação em loop, use StringBuilder. Para interpolação simples, string é melhor: '$\"Nome: {nome}\"' é mais legível que StringBuilder. Para construir HTML, JSON ou SQL dinamicamente, StringBuilder é ideal. Não otimize prematuramente - use string até que performance seja problema. Mas em loops conhecidos, use StringBuilder desde o início. Perfil mostra onde otimizar. StringBuilder pode ser 10-100x mais rápido em cenários apropriados.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Problema da Concatenação",
                    Code = @"using System.Text;

// Ineficiente - cria muitas strings temporárias
string resultado = """";
for (int i = 1; i <= 5; i++)
{
    resultado += $""Número {i}\n"";  // Cria nova string a cada iteração
}
Console.WriteLine(resultado);

// Eficiente - usa StringBuilder
StringBuilder sb = new StringBuilder();
for (int i = 1; i <= 5; i++)
{
    sb.AppendLine($""Número {i}"");  // Modifica buffer existente
}
Console.WriteLine(sb.ToString());",
                    Language = "csharp",
                    Explanation = "Compara concatenação com + (ineficiente) vs StringBuilder (eficiente) em loop.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Métodos de StringBuilder",
                    Code = @"using System.Text;

StringBuilder sb = new StringBuilder();

// Append - adicionar
sb.Append(""Olá"");
sb.Append("" "");
sb.Append(""Mundo"");

// AppendLine - adicionar com quebra
sb.AppendLine(""!"");
sb.AppendLine(""Nova linha"");

// Insert - inserir
sb.Insert(0, "">>> "");

// Replace - substituir
sb.Replace(""Mundo"", ""C#"");

Console.WriteLine(sb.ToString());",
                    Language = "csharp",
                    Explanation = "Demonstra métodos principais de StringBuilder: Append, AppendLine, Insert e Replace.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Construir Lista Numerada",
                    Description = "Use StringBuilder para criar uma string com números de 1 a 10, cada um em uma linha.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using System.Text;

// Use StringBuilder para criar lista
",
                    Hints = new List<string> 
                    { 
                        "Crie StringBuilder sb = new StringBuilder()",
                        "Use for de 1 a 10",
                        "Use sb.AppendLine(i.ToString())" 
                    }
                },
                new Exercise
                {
                    Title = "Construir Tabela HTML",
                    Description = "Crie uma função que recebe uma lista de nomes e retorna uma tabela HTML usando StringBuilder.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"using System.Text;

string CriarTabelaHTML(List<string> nomes)
{
    // Implemente com StringBuilder
}",
                    Hints = new List<string> 
                    { 
                        "Comece com <table>",
                        "Para cada nome, adicione <tr><td>nome</td></tr>",
                        "Termine com </table>" 
                    }
                },
                new Exercise
                {
                    Title = "Comparar Performance",
                    Description = "Crie dois métodos que concatenam 1000 números: um com + e outro com StringBuilder. Compare o tempo.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"using System.Text;

// Método 1: com +
// Método 2: com StringBuilder
// Use DateTime.Now para medir tempo
",
                    Hints = new List<string> 
                    { 
                        "var inicio = DateTime.Now",
                        "Execute o método",
                        "var fim = DateTime.Now; var duracao = fim - inicio" 
                    }
                }
            },
            Summary = "Nesta aula você aprendeu sobre StringBuilder, uma ferramenta essencial para construir strings eficientemente. Você entendeu quando usar StringBuilder vs concatenação simples e como otimizar operações de string. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno. Grandes empresas como Microsoft, Google e Amazon aplicam estes conceitos em seus sistemas. Ao aprender estas técnicas, você está adquirindo conhecimento usado por desenvolvedores em todo o mundo. Considere como este conceito poderia ser aplicado em um projeto pessoal. Pensar em aplicações práticas ajuda a solidificar o entendimento e torna o aprendizado mais significativo. Iniciantes frequentemente cometem alguns erros ao aprender este tópico. Conhecer estes erros comuns ajuda a evitá-los e desenvolver código mais robusto desde o início. Um erro comum é não considerar casos extremos ou situações inesperadas. Sempre teste seu código com diferentes tipos de entrada, incluindo valores nulos, vazios ou inválidos. Outro erro frequente é otimizar prematuramente. Primeiro faça o código funcionar corretamente, depois otimize se necessário. Como diz o ditado: 'premature optimization is the root of all evil'. Quando encontrar problemas, use o debugger para entender o que está acontecendo. Coloque breakpoints, inspecione variáveis e execute o código passo a passo. Esta é uma habilidade essencial para todo desenvolvedor. Mensagens de erro podem parecer intimidadoras no início, mas elas são suas aliadas. Leia-as cuidadosamente - elas geralmente indicam exatamente onde e qual é o problema. Se algo não funcionar como esperado, não desanime. Debugging é uma parte normal do desenvolvimento. Até desenvolvedores experientes passam tempo significativo depurando código. Testar seu código é tão importante quanto escrevê-lo. Testes automatizados garantem que seu código funciona corretamente e continuará funcionando quando você fizer mudanças. Comece escrevendo testes simples para os casos mais comuns. Depois adicione testes para casos extremos e situações de erro. Uma boa cobertura de testes dá confiança para refatorar e melhorar o código. Considere a prática de TDD (Test-Driven Development), onde você escreve os testes antes do código. Isso ajuda a pensar claramente sobre o que o código deve fazer. Embora a correção seja sempre a prioridade, é importante estar ciente das implicações de performance de suas escolhas. Diferentes abordagens podem ter custos computacionais muito diferentes. Em muitos casos, a diferença de performance é negligenciável para aplicações pequenas. No entanto, à medida que seus programas crescem, escolhas eficientes se tornam cada vez mais importantes. Aprenda a usar ferramentas de profiling para identificar gargalos de performance. Otimize baseado em dados reais, não em suposições sobre onde o código é lento. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0002-000000000008"),
            CourseId = _courseId,
            Title = "StringBuilder para Performance",
            Duration = "50 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 50,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0002-000000000007" }),
            OrderIndex = 8,
            Content = "",
            StructuredContent = JsonSerializer.Serialize(content),
            Version = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    private Lesson CreateLesson9()
    {
        var content = new LessonContent
        {
            Objectives = new List<string>
            {
                "Ler arquivos de texto usando File.ReadAllText e File.ReadAllLines",
                "Escrever arquivos de texto usando File.WriteAllText e File.WriteAllLines",
                "Entender caminhos de arquivo absolutos e relativos"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Introdução a File I/O",
                    Content = "File I/O (Input/Output) permite que programas leiam e escrevam arquivos no disco. Isso é essencial para persistir dados, processar arquivos de configuração, gerar relatórios, etc. A classe File em System.IO oferece métodos estáticos para operações comuns. ReadAllText(path) lê todo o conteúdo como string. ReadAllLines(path) lê como array de strings (uma por linha). WriteAllText(path, content) escreve string em arquivo, sobrescrevendo se existir. WriteAllLines(path, lines) escreve array de strings. AppendAllText(path, content) adiciona ao final sem sobrescrever. Esses métodos são simples mas carregam arquivo inteiro na memória - adequados para arquivos pequenos/médios. Para arquivos grandes, use StreamReader/StreamWriter.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Caminhos de Arquivo",
                    Content = "Caminhos podem ser absolutos ou relativos. Caminho absoluto especifica localização completa: 'C:\\Users\\Nome\\arquivo.txt' no Windows, '/home/user/arquivo.txt' no Linux. Caminho relativo é relativo ao diretório de trabalho atual: 'arquivo.txt' busca no diretório atual, 'dados/arquivo.txt' busca em subpasta. Use Path.Combine() para construir caminhos de forma portável: 'Path.Combine(\"dados\", \"arquivo.txt\")' funciona em qualquer OS. Directory.GetCurrentDirectory() retorna diretório atual. Para caminhos relativos ao executável, use AppDomain.CurrentDomain.BaseDirectory. Sempre use Path.Combine em vez de concatenar strings - ele lida com separadores corretamente.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Tratamento de Erros",
                    Content = "Operações de arquivo podem falhar por vários motivos: arquivo não existe, sem permissão, disco cheio, arquivo em uso. Sempre use try-catch ao trabalhar com arquivos. FileNotFoundException ocorre quando arquivo não existe. UnauthorizedAccessException quando sem permissão. IOException para outros erros de I/O. File.Exists(path) verifica se arquivo existe antes de ler. Directory.Exists(path) verifica diretório. Criar diretório se não existir: 'Directory.CreateDirectory(path)'. Deletar arquivo: 'File.Delete(path)'. Mover/renomear: 'File.Move(origem, destino)'. Copiar: 'File.Copy(origem, destino)'. Sempre considere o que fazer se operação falhar - ignorar, logar, notificar usuário?",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Ler e Escrever Arquivo",
                    Code = @"using System.IO;

// Escrever arquivo
string conteudo = ""Olá, este é um arquivo de texto!"";
File.WriteAllText(""mensagem.txt"", conteudo);
Console.WriteLine(""Arquivo criado!"");

// Ler arquivo
string lido = File.ReadAllText(""mensagem.txt"");
Console.WriteLine($""Conteúdo: {lido}"");

// Adicionar ao arquivo
File.AppendAllText(""mensagem.txt"", ""\nNova linha!"");

// Ler todas as linhas
string[] linhas = File.ReadAllLines(""mensagem.txt"");
Console.WriteLine($""Total de linhas: {linhas.Length}"");",
                    Language = "csharp",
                    Explanation = "Demonstra operações básicas: escrever, ler, adicionar e ler linhas de arquivo de texto.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Tratamento de Erros",
                    Code = @"using System.IO;

try
{
    // Verificar se arquivo existe
    if (File.Exists(""dados.txt""))
    {
        string conteudo = File.ReadAllText(""dados.txt"");
        Console.WriteLine(conteudo);
    }
    else
    {
        Console.WriteLine(""Arquivo não encontrado!"");
        // Criar arquivo padrão
        File.WriteAllText(""dados.txt"", ""Conteúdo padrão"");
    }
}
catch (UnauthorizedAccessException)
{
    Console.WriteLine(""Sem permissão para acessar arquivo"");
}
catch (IOException ex)
{
    Console.WriteLine($""Erro de I/O: {ex.Message}"");
}",
                    Language = "csharp",
                    Explanation = "Mostra como verificar existência de arquivo e tratar erros comuns de I/O.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Criar Arquivo de Log",
                    Description = "Crie um programa que escreve a data e hora atual em um arquivo 'log.txt'.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"// Escreva data/hora em log.txt
",
                    Hints = new List<string> 
                    { 
                        "Use DateTime.Now.ToString()",
                        "Use File.WriteAllText()",
                        "Caminho: \"log.txt\"" 
                    }
                },
                new Exercise
                {
                    Title = "Contar Linhas",
                    Description = "Crie uma função que recebe um caminho de arquivo e retorna o número de linhas, ou -1 se arquivo não existir.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"int ContarLinhas(string caminho)
{
    // Implemente aqui
}",
                    Hints = new List<string> 
                    { 
                        "Use File.Exists() para verificar",
                        "Use File.ReadAllLines()",
                        "Retorne array.Length" 
                    }
                },
                new Exercise
                {
                    Title = "Adicionar a Log",
                    Description = "Crie uma função que adiciona uma mensagem com timestamp a um arquivo de log sem sobrescrever conteúdo anterior.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"void AdicionarLog(string mensagem)
{
    // Implemente aqui
}",
                    Hints = new List<string> 
                    { 
                        "Use File.AppendAllText()",
                        "Formate: $\"{DateTime.Now}: {mensagem}\\n\"",
                        "Use try-catch" 
                    }
                }
            },
            Summary = "Nesta aula você aprendeu operações básicas de arquivo com File.ReadAllText, WriteAllText e ReadAllLines. Você entendeu caminhos absolutos vs relativos e a importância de tratar erros em operações de I/O. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno. Grandes empresas como Microsoft, Google e Amazon aplicam estes conceitos em seus sistemas. Ao aprender estas técnicas, você está adquirindo conhecimento usado por desenvolvedores em todo o mundo. Considere como este conceito poderia ser aplicado em um projeto pessoal. Pensar em aplicações práticas ajuda a solidificar o entendimento e torna o aprendizado mais significativo. Iniciantes frequentemente cometem alguns erros ao aprender este tópico. Conhecer estes erros comuns ajuda a evitá-los e desenvolver código mais robusto desde o início. Um erro comum é não considerar casos extremos ou situações inesperadas. Sempre teste seu código com diferentes tipos de entrada, incluindo valores nulos, vazios ou inválidos. Outro erro frequente é otimizar prematuramente. Primeiro faça o código funcionar corretamente, depois otimize se necessário. Como diz o ditado: 'premature optimization is the root of all evil'. Quando encontrar problemas, use o debugger para entender o que está acontecendo. Coloque breakpoints, inspecione variáveis e execute o código passo a passo. Esta é uma habilidade essencial para todo desenvolvedor. Mensagens de erro podem parecer intimidadoras no início, mas elas são suas aliadas. Leia-as cuidadosamente - elas geralmente indicam exatamente onde e qual é o problema. Se algo não funcionar como esperado, não desanime. Debugging é uma parte normal do desenvolvimento. Até desenvolvedores experientes passam tempo significativo depurando código. Testar seu código é tão importante quanto escrevê-lo. Testes automatizados garantem que seu código funciona corretamente e continuará funcionando quando você fizer mudanças. Comece escrevendo testes simples para os casos mais comuns. Depois adicione testes para casos extremos e situações de erro. Uma boa cobertura de testes dá confiança para refatorar e melhorar o código. Considere a prática de TDD (Test-Driven Development), onde você escreve os testes antes do código. Isso ajuda a pensar claramente sobre o que o código deve fazer. Embora a correção seja sempre a prioridade, é importante estar ciente das implicações de performance de suas escolhas. Diferentes abordagens podem ter custos computacionais muito diferentes. Em muitos casos, a diferença de performance é negligenciável para aplicações pequenas. No entanto, à medida que seus programas crescem, escolhas eficientes se tornam cada vez mais importantes. Aprenda a usar ferramentas de profiling para identificar gargalos de performance. Otimize baseado em dados reais, não em suposições sobre onde o código é lento. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0002-000000000009"),
            CourseId = _courseId,
            Title = "Leitura e Escrita de Arquivos",
            Duration = "55 min",
            Difficulty = "Iniciante",
            EstimatedMinutes = 55,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0002-000000000008" }),
            OrderIndex = 9,
            Content = "",
            StructuredContent = JsonSerializer.Serialize(content),
            Version = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    private Lesson CreateLesson10()
    {
        var content = new LessonContent
        {
            Objectives = new List<string>
            {
                "Usar StreamReader para ler arquivos grandes linha por linha",
                "Usar StreamWriter para escrever arquivos eficientemente",
                "Entender quando usar Stream vs métodos File"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Por Que Usar Streams",
                    Content = "File.ReadAllText carrega arquivo inteiro na memória - problema para arquivos grandes (centenas de MB ou GB). StreamReader lê arquivo incrementalmente, linha por linha ou em blocos, usando pouca memória. Isso permite processar arquivos maiores que a RAM disponível. StreamWriter escreve incrementalmente, ideal para gerar arquivos grandes. Streams também oferecem mais controle: você pode ler até encontrar algo específico e parar. Para arquivos pequenos (< 10MB), File.ReadAllText é mais simples. Para arquivos grandes ou quando memória é limitada, use Streams. Streams são mais verbosos mas mais poderosos e eficientes.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Usando StreamReader",
                    Content = "StreamReader lê texto de um stream. Crie com 'new StreamReader(path)' ou 'File.OpenText(path)'. ReadLine() lê uma linha (retorna null no fim). ReadToEnd() lê tudo (como ReadAllText). Padrão comum: while ((linha = reader.ReadLine()) != null) processa linha por linha. Sempre feche o stream com Close() ou use using statement que fecha automaticamente. using garante que stream é fechado mesmo se exceção ocorrer. StreamReader usa buffer interno para eficiência. Você pode especificar encoding se arquivo não for UTF-8. Peek() verifica próximo caractere sem consumir. EndOfStream indica se chegou ao fim.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Usando StreamWriter",
                    Content = "StreamWriter escreve texto em stream. Crie com 'new StreamWriter(path)' ou 'File.CreateText(path)'. Write() escreve sem quebra de linha. WriteLine() escreve com quebra. Flush() força escrita do buffer para disco. Close() fecha e libera recursos. Use using statement para garantir fechamento. Para adicionar a arquivo existente, use 'new StreamWriter(path, append: true)'. StreamWriter também usa buffer - escritas são acumuladas e escritas em lote para eficiência. AutoFlush = true desabilita buffer (mais lento mas garante escrita imediata). Para arquivos grandes, StreamWriter é muito mais eficiente que múltiplos File.AppendAllText.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "StreamReader - Ler Linha por Linha",
                    Code = @"using System.IO;

// Criar arquivo de teste
File.WriteAllLines(""numeros.txt"", new[] { ""1"", ""2"", ""3"", ""4"", ""5"" });

// Ler com StreamReader
using (StreamReader reader = new StreamReader(""numeros.txt""))
{
    string linha;
    int contador = 0;
    
    while ((linha = reader.ReadLine()) != null)
    {
        contador++;
        Console.WriteLine($""Linha {contador}: {linha}"");
    }
}  // Stream fechado automaticamente",
                    Language = "csharp",
                    Explanation = "Demonstra StreamReader com using statement. Lê linha por linha sem carregar arquivo inteiro na memória.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "StreamWriter - Escrever Eficientemente",
                    Code = @"using System.IO;

// Escrever arquivo com StreamWriter
using (StreamWriter writer = new StreamWriter(""saida.txt""))
{
    writer.WriteLine(""Cabeçalho do arquivo"");
    writer.WriteLine(""===================="");
    
    for (int i = 1; i <= 5; i++)
    {
        writer.WriteLine($""Item {i}"");
    }
    
    writer.WriteLine(""Fim do arquivo"");
}

// Ler para verificar
string conteudo = File.ReadAllText(""saida.txt"");
Console.WriteLine(conteudo);",
                    Language = "csharp",
                    Explanation = "Mostra StreamWriter para escrever múltiplas linhas eficientemente. Using garante fechamento do stream.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Contar Palavras em Arquivo",
                    Description = "Use StreamReader para ler um arquivo linha por linha e contar o total de palavras.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"using System.IO;

int ContarPalavras(string caminho)
{
    // Implemente com StreamReader
}",
                    Hints = new List<string> 
                    { 
                        "Use using (StreamReader reader = ...)",
                        "Para cada linha, use Split(' ')",
                        "Some o Length de cada array" 
                    }
                },
                new Exercise
                {
                    Title = "Filtrar Linhas",
                    Description = "Leia um arquivo e escreva em outro apenas as linhas que contêm uma palavra específica.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"using System.IO;

void FiltrarLinhas(string entrada, string saida, string palavra)
{
    // Implemente com StreamReader e StreamWriter
}",
                    Hints = new List<string> 
                    { 
                        "Use StreamReader para ler",
                        "Use StreamWriter para escrever",
                        "Use linha.Contains(palavra)" 
                    }
                },
                new Exercise
                {
                    Title = "Gerar Arquivo Grande",
                    Description = "Use StreamWriter para gerar um arquivo com 1000 linhas numeradas.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"// Gere arquivo com 1000 linhas
",
                    Hints = new List<string> 
                    { 
                        "Use using (StreamWriter writer = ...)",
                        "Use for de 1 a 1000",
                        "writer.WriteLine($\"Linha {i}\")" 
                    }
                }
            },
            Summary = "Nesta aula você aprendeu sobre StreamReader e StreamWriter para processar arquivos grandes eficientemente. Você entendeu quando usar Streams vs métodos File e a importância do using statement. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno. Grandes empresas como Microsoft, Google e Amazon aplicam estes conceitos em seus sistemas. Ao aprender estas técnicas, você está adquirindo conhecimento usado por desenvolvedores em todo o mundo. Considere como este conceito poderia ser aplicado em um projeto pessoal. Pensar em aplicações práticas ajuda a solidificar o entendimento e torna o aprendizado mais significativo. Iniciantes frequentemente cometem alguns erros ao aprender este tópico. Conhecer estes erros comuns ajuda a evitá-los e desenvolver código mais robusto desde o início. Um erro comum é não considerar casos extremos ou situações inesperadas. Sempre teste seu código com diferentes tipos de entrada, incluindo valores nulos, vazios ou inválidos. Outro erro frequente é otimizar prematuramente. Primeiro faça o código funcionar corretamente, depois otimize se necessário. Como diz o ditado: 'premature optimization is the root of all evil'. Quando encontrar problemas, use o debugger para entender o que está acontecendo. Coloque breakpoints, inspecione variáveis e execute o código passo a passo. Esta é uma habilidade essencial para todo desenvolvedor. Mensagens de erro podem parecer intimidadoras no início, mas elas são suas aliadas. Leia-as cuidadosamente - elas geralmente indicam exatamente onde e qual é o problema. Se algo não funcionar como esperado, não desanime. Debugging é uma parte normal do desenvolvimento. Até desenvolvedores experientes passam tempo significativo depurando código. Testar seu código é tão importante quanto escrevê-lo. Testes automatizados garantem que seu código funciona corretamente e continuará funcionando quando você fizer mudanças. Comece escrevendo testes simples para os casos mais comuns. Depois adicione testes para casos extremos e situações de erro. Uma boa cobertura de testes dá confiança para refatorar e melhorar o código. Considere a prática de TDD (Test-Driven Development), onde você escreve os testes antes do código. Isso ajuda a pensar claramente sobre o que o código deve fazer. Embora a correção seja sempre a prioridade, é importante estar ciente das implicações de performance de suas escolhas. Diferentes abordagens podem ter custos computacionais muito diferentes. Em muitos casos, a diferença de performance é negligenciável para aplicações pequenas. No entanto, à medida que seus programas crescem, escolhas eficientes se tornam cada vez mais importantes. Aprenda a usar ferramentas de profiling para identificar gargalos de performance. Otimize baseado em dados reais, não em suposições sobre onde o código é lento. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0002-000000000010"),
            CourseId = _courseId,
            Title = "StreamReader e StreamWriter",
            Duration = "55 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 55,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0002-000000000009" }),
            OrderIndex = 10,
            Content = "",
            StructuredContent = JsonSerializer.Serialize(content),
            Version = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    private Lesson CreateLesson11()
    {
        var content = new LessonContent
        {
            Objectives = new List<string>
            {
                "Trabalhar com diretórios usando Directory e DirectoryInfo",
                "Listar arquivos e subdiretórios",
                "Criar, mover e deletar diretórios"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Trabalhando com Diretórios",
                    Content = "A classe Directory oferece métodos estáticos para operações com diretórios. Directory.Exists(path) verifica se diretório existe. Directory.CreateDirectory(path) cria diretório (e pais se necessário). Directory.Delete(path) deleta diretório vazio. Directory.Delete(path, recursive: true) deleta diretório e todo conteúdo. Directory.Move(origem, destino) move/renomeia diretório. Directory.GetFiles(path) retorna array de caminhos de arquivos. Directory.GetDirectories(path) retorna subdiretórios. Directory.GetFileSystemEntries(path) retorna ambos. Você pode especificar padrão de busca: GetFiles(path, \"*.txt\") retorna apenas arquivos .txt. SearchOption.AllDirectories busca recursivamente em subdiretórios.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "DirectoryInfo para Operações Complexas",
                    Content = "DirectoryInfo é alternativa orientada a objetos. Crie com 'new DirectoryInfo(path)'. Propriedades: Name, FullName, Parent, Root, Exists, CreationTime, LastWriteTime. Métodos: Create(), Delete(), MoveTo(), GetFiles(), GetDirectories(). DirectoryInfo é útil quando você faz múltiplas operações no mesmo diretório - cria objeto uma vez e reutiliza. FileInfo é equivalente para arquivos. Use Directory/File para operações únicas e simples. Use DirectoryInfo/FileInfo para operações múltiplas ou quando precisa de metadados. Ambos fazem as mesmas coisas, apenas com APIs diferentes.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Navegação e Busca",
                    Content = "Para navegar estrutura de diretórios, use GetDirectories recursivamente. Padrão comum: função recursiva que processa diretório atual e chama a si mesma para subdiretórios. Cuidado com loops infinitos em links simbólicos. Directory.EnumerateFiles é mais eficiente que GetFiles para muitos arquivos - retorna IEnumerable que itera sob demanda. Para buscar arquivo específico, use SearchOption.AllDirectories. Path.GetFileName extrai nome de arquivo de caminho completo. Path.GetDirectoryName extrai diretório. Path.GetExtension extrai extensão. Combine essas ferramentas para navegar e processar árvores de diretórios complexas.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Operações com Diretórios",
                    Code = @"using System.IO;

// Criar diretório
string pasta = ""MinhaPasta"";
if (!Directory.Exists(pasta))
{
    Directory.CreateDirectory(pasta);
    Console.WriteLine($""Diretório '{pasta}' criado"");
}

// Criar arquivo dentro
File.WriteAllText(Path.Combine(pasta, ""teste.txt""), ""Conteúdo"");

// Listar arquivos
string[] arquivos = Directory.GetFiles(pasta);
Console.WriteLine($""Arquivos em '{pasta}':"");
foreach (string arquivo in arquivos)
{
    Console.WriteLine($""- {Path.GetFileName(arquivo)}"");
}",
                    Language = "csharp",
                    Explanation = "Demonstra criar diretório, adicionar arquivo e listar conteúdo. Usa Path.Combine para portabilidade.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Buscar Arquivos Recursivamente",
                    Code = @"using System.IO;

void ListarArquivos(string diretorio, string padrao = ""*.*"")
{
    try
    {
        // Arquivos no diretório atual
        foreach (string arquivo in Directory.GetFiles(diretorio, padrao))
        {
            Console.WriteLine(arquivo);
        }
        
        // Recursão em subdiretórios
        foreach (string subdir in Directory.GetDirectories(diretorio))
        {
            ListarArquivos(subdir, padrao);
        }
    }
    catch (UnauthorizedAccessException)
    {
        Console.WriteLine($""Sem acesso a: {diretorio}"");
    }
}

ListarArquivos(""."", ""*.txt"");",
                    Language = "csharp",
                    Explanation = "Função recursiva que lista todos os arquivos .txt em diretório e subdiretórios, tratando erros de acesso.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Criar Estrutura de Pastas",
                    Description = "Crie uma estrutura de diretórios: 'Projeto/src', 'Projeto/tests', 'Projeto/docs'.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"// Crie a estrutura de pastas
",
                    Hints = new List<string> 
                    { 
                        "Use Directory.CreateDirectory()",
                        "Use Path.Combine(\"Projeto\", \"src\")",
                        "CreateDirectory cria pais automaticamente" 
                    }
                },
                new Exercise
                {
                    Title = "Contar Arquivos",
                    Description = "Crie uma função que conta quantos arquivos existem em um diretório (sem subdiretórios).",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"int ContarArquivos(string diretorio)
{
    // Implemente aqui
}",
                    Hints = new List<string> 
                    { 
                        "Use Directory.GetFiles(diretorio)",
                        "Retorne array.Length",
                        "Trate exceção se diretório não existir" 
                    }
                },
                new Exercise
                {
                    Title = "Buscar Arquivos por Extensão",
                    Description = "Crie uma função que retorna lista de todos os arquivos com extensão específica em um diretório e subdiretórios.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"List<string> BuscarArquivos(string diretorio, string extensao)
{
    // Implemente recursivamente
}",
                    Hints = new List<string> 
                    { 
                        "Use Directory.GetFiles(dir, $\"*.{extensao}\")",
                        "Use Directory.GetDirectories() para recursão",
                        "Combine resultados em List<string>" 
                    }
                }
            },
            Summary = "Nesta aula você aprendeu a trabalhar com diretórios usando Directory e DirectoryInfo. Você viu como criar, listar, buscar e navegar estruturas de diretórios, incluindo busca recursiva. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno. Grandes empresas como Microsoft, Google e Amazon aplicam estes conceitos em seus sistemas. Ao aprender estas técnicas, você está adquirindo conhecimento usado por desenvolvedores em todo o mundo. Considere como este conceito poderia ser aplicado em um projeto pessoal. Pensar em aplicações práticas ajuda a solidificar o entendimento e torna o aprendizado mais significativo. Iniciantes frequentemente cometem alguns erros ao aprender este tópico. Conhecer estes erros comuns ajuda a evitá-los e desenvolver código mais robusto desde o início. Um erro comum é não considerar casos extremos ou situações inesperadas. Sempre teste seu código com diferentes tipos de entrada, incluindo valores nulos, vazios ou inválidos. Outro erro frequente é otimizar prematuramente. Primeiro faça o código funcionar corretamente, depois otimize se necessário. Como diz o ditado: 'premature optimization is the root of all evil'. Quando encontrar problemas, use o debugger para entender o que está acontecendo. Coloque breakpoints, inspecione variáveis e execute o código passo a passo. Esta é uma habilidade essencial para todo desenvolvedor. Mensagens de erro podem parecer intimidadoras no início, mas elas são suas aliadas. Leia-as cuidadosamente - elas geralmente indicam exatamente onde e qual é o problema. Se algo não funcionar como esperado, não desanime. Debugging é uma parte normal do desenvolvimento. Até desenvolvedores experientes passam tempo significativo depurando código. Testar seu código é tão importante quanto escrevê-lo. Testes automatizados garantem que seu código funciona corretamente e continuará funcionando quando você fizer mudanças. Comece escrevendo testes simples para os casos mais comuns. Depois adicione testes para casos extremos e situações de erro. Uma boa cobertura de testes dá confiança para refatorar e melhorar o código. Considere a prática de TDD (Test-Driven Development), onde você escreve os testes antes do código. Isso ajuda a pensar claramente sobre o que o código deve fazer. Embora a correção seja sempre a prioridade, é importante estar ciente das implicações de performance de suas escolhas. Diferentes abordagens podem ter custos computacionais muito diferentes. Em muitos casos, a diferença de performance é negligenciável para aplicações pequenas. No entanto, à medida que seus programas crescem, escolhas eficientes se tornam cada vez mais importantes. Aprenda a usar ferramentas de profiling para identificar gargalos de performance. Otimize baseado em dados reais, não em suposições sobre onde o código é lento. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno. Grandes empresas como Microsoft, Google e Amazon aplicam estes conceitos em seus sistemas. Ao aprender estas técnicas, você está adquirindo conhecimento usado por desenvolvedores em todo o mundo."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0002-000000000011"),
            CourseId = _courseId,
            Title = "Trabalhando com Diretórios",
            Duration = "50 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 50,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0002-000000000010" }),
            OrderIndex = 11,
            Content = "",
            StructuredContent = JsonSerializer.Serialize(content),
            Version = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    private Lesson CreateLesson12()
    {
        var content = new LessonContent
        {
            Objectives = new List<string>
            {
                "Serializar objetos para JSON usando System.Text.Json",
                "Desserializar JSON para objetos C#",
                "Salvar e carregar dados estruturados em arquivos"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Introdução a JSON",
                    Content = "JSON (JavaScript Object Notation) é formato de texto para representar dados estruturados. É legível por humanos e fácil de processar por máquinas. JSON usa pares chave-valor: {\"nome\": \"João\", \"idade\": 30}. Arrays usam colchetes: [1, 2, 3]. JSON é amplamente usado em APIs web, arquivos de configuração e armazenamento de dados. C# oferece System.Text.Json para trabalhar com JSON. Serialização converte objeto C# em string JSON. Desserialização converte JSON em objeto C#. Isso permite salvar objetos complexos em arquivos e carregá-los depois. JSON é mais legível que formatos binários e portável entre linguagens.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Serializando Objetos",
                    Content = "JsonSerializer.Serialize(objeto) converte objeto em string JSON. Funciona com classes, listas, dicionários, tipos primitivos. Propriedades públicas são serializadas. Campos privados são ignorados. Você pode customizar com atributos: [JsonPropertyName(\"nome_json\")] muda nome da propriedade no JSON. [JsonIgnore] exclui propriedade. JsonSerializerOptions permite configurar: WriteIndented = true formata JSON com indentação (mais legível), PropertyNameCaseInsensitive ignora maiúsculas/minúsculas. Para salvar em arquivo: File.WriteAllText(path, JsonSerializer.Serialize(objeto)). Serialização é útil para salvar estado de aplicação, configurações, dados de usuário.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Desserializando JSON",
                    Content = "JsonSerializer.Deserialize<T>(json) converte string JSON em objeto do tipo T. O tipo deve ter propriedades que correspondem às chaves JSON. Propriedades não encontradas no JSON ficam com valor padrão. Chaves JSON não correspondentes são ignoradas. Se JSON é inválido, lança JsonException. Para carregar de arquivo: JsonSerializer.Deserialize<T>(File.ReadAllText(path)). Desserialização é útil para carregar configurações, dados salvos, respostas de API. Combine serialização e desserialização para persistência simples: salve objetos em JSON, carregue quando necessário. Para dados complexos ou grandes volumes, considere banco de dados.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Serializar e Desserializar",
                    Code = @"using System.Text.Json;

class Pessoa
{
    public string Nome { get; set; }
    public int Idade { get; set; }
    public List<string> Hobbies { get; set; }
}

// Criar objeto
Pessoa pessoa = new Pessoa
{
    Nome = ""Ana"",
    Idade = 28,
    Hobbies = new List<string> { ""Leitura"", ""Música"", ""Viagens"" }
};

// Serializar para JSON
string json = JsonSerializer.Serialize(pessoa, new JsonSerializerOptions { WriteIndented = true });
Console.WriteLine(""JSON:"");
Console.WriteLine(json);

// Desserializar de volta
Pessoa pessoaCarregada = JsonSerializer.Deserialize<Pessoa>(json);
Console.WriteLine($""\nNome: {pessoaCarregada.Nome}, Idade: {pessoaCarregada.Idade}"");",
                    Language = "csharp",
                    Explanation = "Demonstra serialização de objeto para JSON e desserialização de volta. WriteIndented torna JSON legível.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Salvar e Carregar de Arquivo",
                    Code = @"using System.Text.Json;
using System.IO;

class Configuracao
{
    public string Tema { get; set; }
    public int TamanhoFonte { get; set; }
    public bool ModoEscuro { get; set; }
}

// Salvar configuração
Configuracao config = new Configuracao
{
    Tema = ""Azul"",
    TamanhoFonte = 14,
    ModoEscuro = true
};

string json = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
File.WriteAllText(""config.json"", json);
Console.WriteLine(""Configuração salva!"");

// Carregar configuração
string jsonCarregado = File.ReadAllText(""config.json"");
Configuracao configCarregada = JsonSerializer.Deserialize<Configuracao>(jsonCarregado);
Console.WriteLine($""Tema: {configCarregada.Tema}, Modo Escuro: {configCarregada.ModoEscuro}"");",
                    Language = "csharp",
                    Explanation = "Exemplo prático de salvar configurações em arquivo JSON e carregar depois.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Serializar Lista",
                    Description = "Crie uma lista de produtos (nome e preço) e serialize para JSON formatado.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"class Produto
{
    public string Nome { get; set; }
    public double Preco { get; set; }
}

// Crie lista e serialize
",
                    Hints = new List<string> 
                    { 
                        "Crie List<Produto>",
                        "Use JsonSerializer.Serialize()",
                        "Use WriteIndented = true" 
                    }
                },
                new Exercise
                {
                    Title = "Salvar e Carregar Contatos",
                    Description = "Crie uma classe Contato (nome, telefone, email). Salve uma lista de contatos em arquivo JSON e carregue de volta.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"class Contato
{
    public string Nome { get; set; }
    public string Telefone { get; set; }
    public string Email { get; set; }
}

// Implemente salvar e carregar
",
                    Hints = new List<string> 
                    { 
                        "Serialize List<Contato>",
                        "Use File.WriteAllText()",
                        "Deserialize ao carregar" 
                    }
                },
                new Exercise
                {
                    Title = "Tratamento de Erro",
                    Description = "Crie uma função que carrega configuração de arquivo JSON, retornando configuração padrão se arquivo não existir ou JSON for inválido.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"Configuracao CarregarConfiguracao(string caminho)
{
    // Implemente com tratamento de erro
}",
                    Hints = new List<string> 
                    { 
                        "Use try-catch para JsonException",
                        "Verifique File.Exists()",
                        "Retorne new Configuracao() como padrão" 
                    }
                }
            },
            Summary = "Nesta aula você aprendeu a trabalhar com JSON usando System.Text.Json. Você viu como serializar objetos para JSON, desserializar JSON para objetos e usar JSON para persistir dados em arquivos. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno. Grandes empresas como Microsoft, Google e Amazon aplicam estes conceitos em seus sistemas. Ao aprender estas técnicas, você está adquirindo conhecimento usado por desenvolvedores em todo o mundo. Considere como este conceito poderia ser aplicado em um projeto pessoal. Pensar em aplicações práticas ajuda a solidificar o entendimento e torna o aprendizado mais significativo. Iniciantes frequentemente cometem alguns erros ao aprender este tópico. Conhecer estes erros comuns ajuda a evitá-los e desenvolver código mais robusto desde o início. Um erro comum é não considerar casos extremos ou situações inesperadas. Sempre teste seu código com diferentes tipos de entrada, incluindo valores nulos, vazios ou inválidos. Outro erro frequente é otimizar prematuramente. Primeiro faça o código funcionar corretamente, depois otimize se necessário. Como diz o ditado: 'premature optimization is the root of all evil'. Quando encontrar problemas, use o debugger para entender o que está acontecendo. Coloque breakpoints, inspecione variáveis e execute o código passo a passo. Esta é uma habilidade essencial para todo desenvolvedor. Mensagens de erro podem parecer intimidadoras no início, mas elas são suas aliadas. Leia-as cuidadosamente - elas geralmente indicam exatamente onde e qual é o problema. Se algo não funcionar como esperado, não desanime. Debugging é uma parte normal do desenvolvimento. Até desenvolvedores experientes passam tempo significativo depurando código. Testar seu código é tão importante quanto escrevê-lo. Testes automatizados garantem que seu código funciona corretamente e continuará funcionando quando você fizer mudanças. Comece escrevendo testes simples para os casos mais comuns. Depois adicione testes para casos extremos e situações de erro. Uma boa cobertura de testes dá confiança para refatorar e melhorar o código. Considere a prática de TDD (Test-Driven Development), onde você escreve os testes antes do código. Isso ajuda a pensar claramente sobre o que o código deve fazer. Embora a correção seja sempre a prioridade, é importante estar ciente das implicações de performance de suas escolhas. Diferentes abordagens podem ter custos computacionais muito diferentes. Em muitos casos, a diferença de performance é negligenciável para aplicações pequenas. No entanto, à medida que seus programas crescem, escolhas eficientes se tornam cada vez mais importantes. Aprenda a usar ferramentas de profiling para identificar gargalos de performance. Otimize baseado em dados reais, não em suposições sobre onde o código é lento. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno. Grandes empresas como Microsoft, Google e Amazon aplicam estes conceitos em seus sistemas. Ao aprender estas técnicas, você está adquirindo conhecimento usado por desenvolvedores em todo o mundo. Considere como este conceito poderia ser aplicado em um projeto pessoal. Pensar em aplicações práticas ajuda a solidificar o entendimento e torna o aprendizado mais significativo."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0002-000000000012"),
            CourseId = _courseId,
            Title = "Trabalhando com JSON",
            Duration = "55 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 55,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0002-000000000011" }),
            OrderIndex = 12,
            Content = "",
            StructuredContent = JsonSerializer.Serialize(content),
            Version = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    private Lesson CreateLesson13()
    {
        var content = new LessonContent
        {
            Objectives = new List<string>
            {
                "Entender e usar HashSet<T> para coleções únicas",
                "Aplicar operações de conjunto (união, interseção, diferença)",
                "Escolher a coleção apropriada para cada cenário"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Introdução a HashSet",
                    Content = "HashSet<T> é uma coleção que armazena elementos únicos sem ordem específica. Diferente de List que permite duplicatas, HashSet automaticamente ignora elementos duplicados. Internamente usa hash table, oferecendo operações O(1) para Add, Remove e Contains - muito mais rápido que List para verificar existência. HashSet é ideal quando: precisa garantir unicidade, faz muitas verificações de existência, ordem não importa, não precisa acessar por índice. Comum em cenários como: remover duplicatas de lista, verificar se item já foi processado, implementar conjuntos matemáticos. HashSet não tem indexador - não pode acessar elementos por posição.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Operações de Conjunto",
                    Content = "HashSet oferece operações matemáticas de conjunto. UnionWith(outra) adiciona todos elementos de outra coleção. IntersectWith(outra) mantém apenas elementos que existem em ambas. ExceptWith(outra) remove elementos que existem em outra. SymmetricExceptWith(outra) mantém elementos que existem em apenas uma das coleções. IsSubsetOf(outra) verifica se é subconjunto. IsSupersetOf(outra) verifica se é superconjunto. Overlaps(outra) verifica se há elementos em comum. SetEquals(outra) verifica se conjuntos são iguais. Essas operações são eficientes e expressivas para lógica de conjuntos.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Quando Usar Cada Coleção",
                    Content = "List: precisa ordem, acessa por índice, permite duplicatas, itera frequentemente. Dictionary: mapeia chaves a valores, busca rápida por chave, cada chave única. HashSet: garante unicidade, verifica existência rapidamente, ordem não importa, operações de conjunto. Queue: FIFO (primeiro a entrar, primeiro a sair), processamento em ordem. Stack: LIFO (último a entrar, primeiro a sair), desfazer operações. SortedSet: como HashSet mas mantém ordem. LinkedList: inserção/remoção eficiente no meio. Escolha baseada em: operações principais, necessidade de ordem, unicidade, performance. Não otimize prematuramente - comece com List/Dictionary e mude se necessário.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "HashSet Básico",
                    Code = @"HashSet<int> numeros = new HashSet<int>();

// Adicionar elementos
numeros.Add(1);
numeros.Add(2);
numeros.Add(3);
numeros.Add(2);  // Duplicata - ignorada

Console.WriteLine($""Total: {numeros.Count}"");  // 3, não 4

// Verificar existência - O(1)
bool tem2 = numeros.Contains(2);
Console.WriteLine($""Contém 2: {tem2}"");

// Remover duplicatas de lista
List<int> lista = new List<int> { 1, 2, 2, 3, 3, 3, 4 };
HashSet<int> unicos = new HashSet<int>(lista);
Console.WriteLine($""Únicos: {string.Join("", "", unicos)}"");",
                    Language = "csharp",
                    Explanation = "Demonstra HashSet ignorando duplicatas automaticamente e uso para remover duplicatas de lista.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Operações de Conjunto",
                    Code = @"HashSet<string> grupoA = new HashSet<string> { ""Ana"", ""Bruno"", ""Carlos"" };
HashSet<string> grupoB = new HashSet<string> { ""Bruno"", ""Carlos"", ""Diana"" };

// Interseção - elementos em ambos
HashSet<string> ambos = new HashSet<string>(grupoA);
ambos.IntersectWith(grupoB);
Console.WriteLine($""Em ambos: {string.Join("", "", ambos)}"");

// União - todos elementos
HashSet<string> todos = new HashSet<string>(grupoA);
todos.UnionWith(grupoB);
Console.WriteLine($""Todos: {string.Join("", "", todos)}"");

// Diferença - apenas em A
HashSet<string> apenasA = new HashSet<string>(grupoA);
apenasA.ExceptWith(grupoB);
Console.WriteLine($""Apenas A: {string.Join("", "", apenasA)}"");",
                    Language = "csharp",
                    Explanation = "Mostra operações de conjunto: interseção, união e diferença entre HashSets.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Remover Duplicatas",
                    Description = "Dada uma lista de palavras com duplicatas, use HashSet para criar uma lista sem duplicatas.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"List<string> palavras = new List<string> { ""casa"", ""carro"", ""casa"", ""moto"", ""carro"" };
// Remova duplicatas
",
                    Hints = new List<string> 
                    { 
                        "Crie HashSet<string>(palavras)",
                        "Converta de volta para lista se necessário",
                        "new List<string>(hashSet)" 
                    }
                },
                new Exercise
                {
                    Title = "Encontrar Comuns",
                    Description = "Dadas duas listas de números, encontre os números que aparecem em ambas.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"List<int> lista1 = new List<int> { 1, 2, 3, 4, 5 };
List<int> lista2 = new List<int> { 3, 4, 5, 6, 7 };
// Encontre números comuns
",
                    Hints = new List<string> 
                    { 
                        "Converta lista1 para HashSet",
                        "Use IntersectWith(lista2)",
                        "Resultado é a interseção" 
                    }
                },
                new Exercise
                {
                    Title = "Verificar Processados",
                    Description = "Crie um sistema que processa IDs únicos. Use HashSet para garantir que cada ID é processado apenas uma vez.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"HashSet<int> processados = new HashSet<int>();

void ProcessarID(int id)
{
    // Implemente verificação e processamento
}",
                    Hints = new List<string> 
                    { 
                        "Use processados.Contains(id) para verificar",
                        "Use processados.Add(id) para marcar",
                        "Add retorna false se já existe" 
                    }
                }
            },
            Summary = "Nesta aula você aprendeu sobre HashSet<T>, uma coleção que garante unicidade e oferece operações rápidas. Você viu operações de conjunto e quando usar HashSet vs List vs Dictionary. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno. Grandes empresas como Microsoft, Google e Amazon aplicam estes conceitos em seus sistemas. Ao aprender estas técnicas, você está adquirindo conhecimento usado por desenvolvedores em todo o mundo. Considere como este conceito poderia ser aplicado em um projeto pessoal. Pensar em aplicações práticas ajuda a solidificar o entendimento e torna o aprendizado mais significativo. Iniciantes frequentemente cometem alguns erros ao aprender este tópico. Conhecer estes erros comuns ajuda a evitá-los e desenvolver código mais robusto desde o início. Um erro comum é não considerar casos extremos ou situações inesperadas. Sempre teste seu código com diferentes tipos de entrada, incluindo valores nulos, vazios ou inválidos. Outro erro frequente é otimizar prematuramente. Primeiro faça o código funcionar corretamente, depois otimize se necessário. Como diz o ditado: 'premature optimization is the root of all evil'. Quando encontrar problemas, use o debugger para entender o que está acontecendo. Coloque breakpoints, inspecione variáveis e execute o código passo a passo. Esta é uma habilidade essencial para todo desenvolvedor. Mensagens de erro podem parecer intimidadoras no início, mas elas são suas aliadas. Leia-as cuidadosamente - elas geralmente indicam exatamente onde e qual é o problema. Se algo não funcionar como esperado, não desanime. Debugging é uma parte normal do desenvolvimento. Até desenvolvedores experientes passam tempo significativo depurando código. Testar seu código é tão importante quanto escrevê-lo. Testes automatizados garantem que seu código funciona corretamente e continuará funcionando quando você fizer mudanças. Comece escrevendo testes simples para os casos mais comuns. Depois adicione testes para casos extremos e situações de erro. Uma boa cobertura de testes dá confiança para refatorar e melhorar o código. Considere a prática de TDD (Test-Driven Development), onde você escreve os testes antes do código. Isso ajuda a pensar claramente sobre o que o código deve fazer. Embora a correção seja sempre a prioridade, é importante estar ciente das implicações de performance de suas escolhas. Diferentes abordagens podem ter custos computacionais muito diferentes. Em muitos casos, a diferença de performance é negligenciável para aplicações pequenas. No entanto, à medida que seus programas crescem, escolhas eficientes se tornam cada vez mais importantes. Aprenda a usar ferramentas de profiling para identificar gargalos de performance. Otimize baseado em dados reais, não em suposições sobre onde o código é lento. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0002-000000000013"),
            CourseId = _courseId,
            Title = "HashSet e Coleções Únicas",
            Duration = "50 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 50,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0002-000000000012" }),
            OrderIndex = 13,
            Content = "",
            StructuredContent = JsonSerializer.Serialize(content),
            Version = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    private Lesson CreateLesson14()
    {
        var content = new LessonContent
        {
            Objectives = new List<string>
            {
                "Usar expressões regulares (Regex) para busca e validação",
                "Aplicar padrões comuns de Regex",
                "Validar formatos como email, telefone e CPF"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Introdução a Expressões Regulares",
                    Content = "Expressões regulares (regex) são padrões para buscar e manipular texto. Permitem buscas complexas que seriam difíceis com métodos de string normais. Por exemplo, encontrar todos os emails em um texto, validar formato de telefone, extrair números de um documento. Regex usa sintaxe especial: '.' representa qualquer caractere, '*' significa zero ou mais, '+' significa um ou mais, '?' significa opcional. Caracteres especiais precisam ser escapados com \\. Regex é poderoso mas pode ser complexo. Use para validação de formatos, extração de dados, substituições complexas. C# oferece classe Regex em System.Text.RegularExpressions.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Padrões Comuns",
                    Content = "Alguns padrões úteis: \\d representa dígito (0-9), \\w representa letra/número/underscore, \\s representa espaço em branco. [abc] representa a, b ou c. [0-9] representa qualquer dígito. [a-z] representa letra minúscula. ^ indica início da string, $ indica fim. {n} significa exatamente n ocorrências, {n,m} significa entre n e m. Email: @\"^[\\w.-]+@[\\w.-]+\\.\\w+$\". Telefone: @\"^\\(\\d{2}\\)\\s?\\d{4,5}-\\d{4}$\". CPF: @\"^\\d{3}\\.\\d{3}\\.\\d{3}-\\d{2}$\". Use @ antes da string para evitar escape duplo. Teste padrões em sites como regex101.com.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Usando Regex em C#",
                    Content = "Regex.IsMatch(texto, padrão) verifica se texto corresponde ao padrão. Regex.Match(texto, padrão) retorna primeira correspondência. Regex.Matches(texto, padrão) retorna todas correspondências. Regex.Replace(texto, padrão, substituição) substitui correspondências. Para performance, compile regex usado frequentemente: 'new Regex(padrão, RegexOptions.Compiled)'. RegexOptions.IgnoreCase ignora maiúsculas/minúsculas. Match tem propriedades Success, Value, Index. Groups permite capturar partes do padrão. Regex é poderoso mas pode ser lento para textos grandes - use com cuidado. Para validações simples, métodos de string podem ser suficientes.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Validação com Regex",
                    Code = @"using System.Text.RegularExpressions;

// Validar email
string email = ""usuario@exemplo.com"";
string padraoEmail = @""^[\w.-]+@[\w.-]+\.\w+$"";
bool emailValido = Regex.IsMatch(email, padraoEmail);
Console.WriteLine($""Email válido: {emailValido}"");

// Validar telefone (XX) XXXXX-XXXX
string telefone = ""(11) 98765-4321"";
string padraoTelefone = @""^\(\d{2}\)\s?\d{4,5}-\d{4}$"";
bool telefoneValido = Regex.IsMatch(telefone, padraoTelefone);
Console.WriteLine($""Telefone válido: {telefoneValido}"");",
                    Language = "csharp",
                    Explanation = "Demonstra validação de email e telefone usando expressões regulares com IsMatch.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Extrair e Substituir",
                    Code = @"using System.Text.RegularExpressions;

string texto = ""Meu telefone é (11) 98765-4321 e meu email é user@example.com"";

// Extrair todos os números
MatchCollection numeros = Regex.Matches(texto, @""\d+"");
Console.WriteLine(""Números encontrados:"");
foreach (Match match in numeros)
{
    Console.WriteLine(match.Value);
}

// Substituir números por X
string censurado = Regex.Replace(texto, @""\d"", ""X"");
Console.WriteLine($""\nCensurado: {censurado}"");",
                    Language = "csharp",
                    Explanation = "Mostra como extrair padrões com Matches e substituir com Replace.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Validar CPF",
                    Description = "Crie uma função que valida formato de CPF (XXX.XXX.XXX-XX) usando Regex.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"using System.Text.RegularExpressions;

bool ValidarCPF(string cpf)
{
    // Implemente com Regex
}",
                    Hints = new List<string> 
                    { 
                        "Padrão: @\"^\\d{3}\\.\\d{3}\\.\\d{3}-\\d{2}$\"",
                        "Use Regex.IsMatch()",
                        "Teste com \"123.456.789-00\"" 
                    }
                },
                new Exercise
                {
                    Title = "Extrair Emails",
                    Description = "Crie uma função que extrai todos os emails de um texto e retorna como lista.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"using System.Text.RegularExpressions;

List<string> ExtrairEmails(string texto)
{
    // Implemente com Regex.Matches
}",
                    Hints = new List<string> 
                    { 
                        "Use Regex.Matches()",
                        "Padrão: @\"[\\w.-]+@[\\w.-]+\\.\\w+\"",
                        "Itere sobre matches e adicione à lista" 
                    }
                },
                new Exercise
                {
                    Title = "Limpar Texto",
                    Description = "Crie uma função que remove todos os caracteres não alfanuméricos de um texto, mantendo apenas letras, números e espaços.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"using System.Text.RegularExpressions;

string LimparTexto(string texto)
{
    // Implemente com Regex.Replace
}",
                    Hints = new List<string> 
                    { 
                        "Use Regex.Replace()",
                        "Padrão: @\"[^\\w\\s]\" (^ nega)",
                        "Substitua por string vazia" 
                    }
                }
            },
            Summary = "Nesta aula você aprendeu sobre expressões regulares (Regex) para busca e validação de padrões em texto. Você viu padrões comuns e como usar Regex.IsMatch, Match, Matches e Replace. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno. Grandes empresas como Microsoft, Google e Amazon aplicam estes conceitos em seus sistemas. Ao aprender estas técnicas, você está adquirindo conhecimento usado por desenvolvedores em todo o mundo. Considere como este conceito poderia ser aplicado em um projeto pessoal. Pensar em aplicações práticas ajuda a solidificar o entendimento e torna o aprendizado mais significativo. Iniciantes frequentemente cometem alguns erros ao aprender este tópico. Conhecer estes erros comuns ajuda a evitá-los e desenvolver código mais robusto desde o início. Um erro comum é não considerar casos extremos ou situações inesperadas. Sempre teste seu código com diferentes tipos de entrada, incluindo valores nulos, vazios ou inválidos. Outro erro frequente é otimizar prematuramente. Primeiro faça o código funcionar corretamente, depois otimize se necessário. Como diz o ditado: 'premature optimization is the root of all evil'. Quando encontrar problemas, use o debugger para entender o que está acontecendo. Coloque breakpoints, inspecione variáveis e execute o código passo a passo. Esta é uma habilidade essencial para todo desenvolvedor. Mensagens de erro podem parecer intimidadoras no início, mas elas são suas aliadas. Leia-as cuidadosamente - elas geralmente indicam exatamente onde e qual é o problema. Se algo não funcionar como esperado, não desanime. Debugging é uma parte normal do desenvolvimento. Até desenvolvedores experientes passam tempo significativo depurando código. Testar seu código é tão importante quanto escrevê-lo. Testes automatizados garantem que seu código funciona corretamente e continuará funcionando quando você fizer mudanças. Comece escrevendo testes simples para os casos mais comuns. Depois adicione testes para casos extremos e situações de erro. Uma boa cobertura de testes dá confiança para refatorar e melhorar o código. Considere a prática de TDD (Test-Driven Development), onde você escreve os testes antes do código. Isso ajuda a pensar claramente sobre o que o código deve fazer. Embora a correção seja sempre a prioridade, é importante estar ciente das implicações de performance de suas escolhas. Diferentes abordagens podem ter custos computacionais muito diferentes. Em muitos casos, a diferença de performance é negligenciável para aplicações pequenas. No entanto, à medida que seus programas crescem, escolhas eficientes se tornam cada vez mais importantes. Aprenda a usar ferramentas de profiling para identificar gargalos de performance. Otimize baseado em dados reais, não em suposições sobre onde o código é lento. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0002-000000000014"),
            CourseId = _courseId,
            Title = "Expressões Regulares (Regex)",
            Duration = "55 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 55,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0002-000000000013" }),
            OrderIndex = 14,
            Content = "",
            StructuredContent = JsonSerializer.Serialize(content),
            Version = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    private Lesson CreateLesson15()
    {
        var content = new LessonContent
        {
            Objectives = new List<string>
            {
                "Entender e usar delegates para referências a métodos",
                "Aplicar Action e Func delegates",
                "Usar delegates para callbacks e eventos simples"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "O Que São Delegates",
                    Content = "Delegate é um tipo que representa referência a um método. Pense nele como um ponteiro para função. Delegates permitem passar métodos como parâmetros, armazenar métodos em variáveis, chamar métodos indiretamente. Isso é fundamental para callbacks, eventos e programação funcional. Declare delegate especificando assinatura: 'delegate int Operacao(int a, int b)'. Crie instância apontando para método: 'Operacao op = Somar'. Chame através do delegate: 'int resultado = op(5, 3)'. Delegates são type-safe - compilador verifica que método corresponde à assinatura. Delegates são base para eventos em C#.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Action e Func",
                    Content = "C# oferece delegates genéricos que cobrem a maioria dos casos. Action representa método que não retorna valor (void). Action<T> aceita um parâmetro, Action<T1, T2> aceita dois, etc. Func representa método que retorna valor. Func<TResult> não aceita parâmetros e retorna TResult. Func<T, TResult> aceita um parâmetro e retorna valor. Último tipo genérico é sempre o retorno. Exemplo: Func<int, int, int> aceita dois ints e retorna int. Use Action/Func em vez de declarar delegates customizados - mais simples e padronizado. Lambda expressions funcionam perfeitamente com Action/Func.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Casos de Uso Práticos",
                    Content = "Delegates são úteis para: callbacks (passar método para ser chamado depois), estratégias (passar algoritmo diferente), eventos (notificar quando algo acontece), LINQ (Where, Select usam delegates), processamento assíncrono. Exemplo: método ProcessarLista que aceita Action<T> para processar cada item - você passa o que fazer com cada item. Ou método Filtrar que aceita Func<T, bool> para decidir quais itens manter. Delegates tornam código mais flexível e reutilizável. Em vez de código rígido, você passa comportamento como parâmetro. Isso é programação funcional em C#.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Delegates Básicos",
                    Code = @"// Declarar delegate
delegate int Operacao(int a, int b);

// Métodos que correspondem à assinatura
int Somar(int a, int b) => a + b;
int Multiplicar(int a, int b) => a * b;

// Usar delegate
Operacao op = Somar;
Console.WriteLine($""5 + 3 = {op(5, 3)}"");

op = Multiplicar;
Console.WriteLine($""5 * 3 = {op(5, 3)}"");

// Passar como parâmetro
void Executar(Operacao operacao, int x, int y)
{
    Console.WriteLine($""Resultado: {operacao(x, y)}"");
}

Executar(Somar, 10, 20);",
                    Language = "csharp",
                    Explanation = "Demonstra declaração de delegate, atribuição de métodos e uso como parâmetro.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Action e Func",
                    Code = @"// Action - sem retorno
Action<string> exibir = mensagem => Console.WriteLine(mensagem);
exibir(""Olá, Delegates!"");

// Func - com retorno
Func<int, int, int> somar = (a, b) => a + b;
Console.WriteLine($""10 + 5 = {somar(10, 5)}"");

// Usar em método
void ProcessarNumeros(List<int> numeros, Action<int> acao)
{
    foreach (int num in numeros)
    {
        acao(num);
    }
}

List<int> lista = new List<int> { 1, 2, 3, 4, 5 };
ProcessarNumeros(lista, n => Console.WriteLine($""Número: {n}""));",
                    Language = "csharp",
                    Explanation = "Mostra Action e Func com lambda expressions e uso prático em método genérico.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Criar Calculadora com Delegates",
                    Description = "Crie um método Calcular que aceita dois números e um delegate Operacao, executando a operação especificada.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"delegate double Operacao(double a, double b);

double Calcular(double x, double y, Operacao op)
{
    // Implemente aqui
}",
                    Hints = new List<string> 
                    { 
                        "Simplesmente retorne op(x, y)",
                        "Teste com diferentes operações",
                        "Calcular(10, 5, (a, b) => a + b)" 
                    }
                },
                new Exercise
                {
                    Title = "Filtrar com Func",
                    Description = "Crie um método Filtrar que aceita uma lista e um Func<T, bool>, retornando nova lista com elementos que satisfazem a condição.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"List<int> Filtrar(List<int> lista, Func<int, bool> condicao)
{
    // Implemente aqui
}",
                    Hints = new List<string> 
                    { 
                        "Crie nova List<int>",
                        "Itere sobre lista original",
                        "Adicione se condicao(item) for true" 
                    }
                },
                new Exercise
                {
                    Title = "Processar com Action",
                    Description = "Crie um método ProcessarArquivo que lê um arquivo linha por linha e executa uma Action<string> em cada linha.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"using System.IO;

void ProcessarArquivo(string caminho, Action<string> processar)
{
    // Implemente com StreamReader
}",
                    Hints = new List<string> 
                    { 
                        "Use StreamReader",
                        "Para cada linha, chame processar(linha)",
                        "Use using statement" 
                    }
                }
            },
            Summary = "Nesta aula você aprendeu sobre delegates, referências a métodos que permitem passar comportamento como parâmetro. Você viu Action e Func delegates e casos de uso práticos para callbacks e estratégias. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno. Grandes empresas como Microsoft, Google e Amazon aplicam estes conceitos em seus sistemas. Ao aprender estas técnicas, você está adquirindo conhecimento usado por desenvolvedores em todo o mundo. Considere como este conceito poderia ser aplicado em um projeto pessoal. Pensar em aplicações práticas ajuda a solidificar o entendimento e torna o aprendizado mais significativo. Iniciantes frequentemente cometem alguns erros ao aprender este tópico. Conhecer estes erros comuns ajuda a evitá-los e desenvolver código mais robusto desde o início. Um erro comum é não considerar casos extremos ou situações inesperadas. Sempre teste seu código com diferentes tipos de entrada, incluindo valores nulos, vazios ou inválidos. Outro erro frequente é otimizar prematuramente. Primeiro faça o código funcionar corretamente, depois otimize se necessário. Como diz o ditado: 'premature optimization is the root of all evil'. Quando encontrar problemas, use o debugger para entender o que está acontecendo. Coloque breakpoints, inspecione variáveis e execute o código passo a passo. Esta é uma habilidade essencial para todo desenvolvedor. Mensagens de erro podem parecer intimidadoras no início, mas elas são suas aliadas. Leia-as cuidadosamente - elas geralmente indicam exatamente onde e qual é o problema. Se algo não funcionar como esperado, não desanime. Debugging é uma parte normal do desenvolvimento. Até desenvolvedores experientes passam tempo significativo depurando código. Testar seu código é tão importante quanto escrevê-lo. Testes automatizados garantem que seu código funciona corretamente e continuará funcionando quando você fizer mudanças. Comece escrevendo testes simples para os casos mais comuns. Depois adicione testes para casos extremos e situações de erro. Uma boa cobertura de testes dá confiança para refatorar e melhorar o código. Considere a prática de TDD (Test-Driven Development), onde você escreve os testes antes do código. Isso ajuda a pensar claramente sobre o que o código deve fazer. Embora a correção seja sempre a prioridade, é importante estar ciente das implicações de performance de suas escolhas. Diferentes abordagens podem ter custos computacionais muito diferentes. Em muitos casos, a diferença de performance é negligenciável para aplicações pequenas. No entanto, à medida que seus programas crescem, escolhas eficientes se tornam cada vez mais importantes. Aprenda a usar ferramentas de profiling para identificar gargalos de performance. Otimize baseado em dados reais, não em suposições sobre onde o código é lento. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0002-000000000015"),
            CourseId = _courseId,
            Title = "Delegates e Callbacks",
            Duration = "55 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 55,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0002-000000000014" }),
            OrderIndex = 15,
            Content = "",
            StructuredContent = JsonSerializer.Serialize(content),
            Version = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }
}
