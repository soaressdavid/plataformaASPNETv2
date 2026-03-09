using Shared.Entities;
using Shared.Models;
using System.Text.Json;

namespace Shared.Data;

/// <summary>
/// Part 3 of Level 1 Content Seeder - Lessons 16-20 and Project
/// Final lessons covering advanced topics and integration
/// </summary>
public partial class Level1ContentSeeder
{
    private Lesson CreateLesson16()
    {
        var content = new LessonContent
        {
            Objectives = new List<string>
            {
                "Entender e usar eventos para notificações",
                "Criar e disparar eventos customizados",
                "Implementar padrão Observer com eventos"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Introdução a Eventos",
                    Content = "Eventos permitem que objetos notifiquem outros quando algo acontece. É implementação do padrão Observer. Classe que dispara evento é o publisher, classes que respondem são subscribers. Eventos são baseados em delegates. Declare evento com 'event': 'public event EventHandler NomeEvento'. EventHandler é delegate padrão para eventos. Subscribers se inscrevem com +=: 'objeto.NomeEvento += MetodoHandler'. Publisher dispara evento verificando se não é null: 'NomeEvento?.Invoke(this, EventArgs.Empty)'. Eventos são fundamentais em UI (clique de botão), notificações de mudança de estado, arquitetura desacoplada. Eventos tornam código mais modular - publisher não precisa conhecer subscribers.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Criando Eventos Customizados",
                    Content = "Para eventos customizados, crie classe que herda de EventArgs para passar dados: 'class MeuEventArgs : EventArgs { public string Mensagem { get; set; } }'. Declare delegate customizado se necessário: 'public delegate void MeuEventHandler(object sender, MeuEventArgs e)'. Declare evento: 'public event MeuEventHandler MeuEvento'. Dispare passando dados: 'MeuEvento?.Invoke(this, new MeuEventArgs { Mensagem = \"Dados\" })'. Subscribers recebem dados no handler: 'void OnMeuEvento(object sender, MeuEventArgs e) { Console.WriteLine(e.Mensagem); }'. Convenção: métodos handler começam com 'On'. Sempre verifique se evento não é null antes de disparar.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Boas Práticas com Eventos",
                    Content = "Sempre desinscreva de eventos quando não precisar mais: 'objeto.Evento -= Handler'. Não desinscrever causa memory leaks - objeto permanece na memória porque evento mantém referência. Use -= no Dispose ou quando objeto não é mais necessário. Eventos devem ser disparados apenas pela classe que os declara - por isso são 'event', não apenas delegates. Não exponha delegate diretamente. Use EventHandler<T> genérico em vez de criar delegates customizados. Mantenha handlers rápidos - não bloqueie thread. Para operações longas, use async ou background thread. Eventos são síncronos por padrão - todos handlers executam antes de continuar.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Evento Simples",
                    Code = @"class Contador
{
    private int _valor = 0;
    
    // Declarar evento
    public event EventHandler LimiteAtingido;
    
    public void Incrementar()
    {
        _valor++;
        Console.WriteLine($""Valor: {_valor}"");
        
        if (_valor >= 5)
        {
            // Disparar evento
            LimiteAtingido?.Invoke(this, EventArgs.Empty);
        }
    }
}

// Usar evento
Contador contador = new Contador();

// Inscrever
contador.LimiteAtingido += (sender, e) =>
{
    Console.WriteLine(""Limite de 5 atingido!"");
};

// Disparar evento
for (int i = 0; i < 6; i++)
{
    contador.Incrementar();
}",
                    Language = "csharp",
                    Explanation = "Demonstra evento simples que notifica quando contador atinge limite.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Evento com Dados Customizados",
                    Code = @"// EventArgs customizado
class PedidoEventArgs : EventArgs
{
    public int NumeroPedido { get; set; }
    public double Valor { get; set; }
}

class ProcessadorPedidos
{
    // Evento com dados customizados
    public event EventHandler<PedidoEventArgs> PedidoProcessado;
    
    public void ProcessarPedido(int numero, double valor)
    {
        Console.WriteLine($""Processando pedido {numero}..."");
        
        // Disparar evento com dados
        PedidoProcessado?.Invoke(this, new PedidoEventArgs
        {
            NumeroPedido = numero,
            Valor = valor
        });
    }
}

// Usar
ProcessadorPedidos processador = new ProcessadorPedidos();

processador.PedidoProcessado += (sender, e) =>
{
    Console.WriteLine($""Pedido {e.NumeroPedido} processado: R$ {e.Valor:F2}"");
};

processador.ProcessarPedido(1001, 150.00);",
                    Language = "csharp",
                    Explanation = "Mostra evento customizado que passa dados específicos através de EventArgs.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Criar Evento de Mudança",
                    Description = "Crie uma classe Temperatura com propriedade Valor. Dispare evento quando valor mudar.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"class Temperatura
{
    private double _valor;
    
    public event EventHandler ValorMudou;
    
    public double Valor
    {
        get => _valor;
        set
        {
            // Implemente: atribua e dispare evento
        }
    }
}",
                    Hints = new List<string> 
                    { 
                        "Atribua _valor = value",
                        "Dispare ValorMudou?.Invoke(this, EventArgs.Empty)",
                        "Teste inscrevendo no evento" 
                    }
                },
                new Exercise
                {
                    Title = "Evento com Dados",
                    Description = "Crie classe ContaBancaria que dispara evento quando saldo fica negativo, passando o saldo atual.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"class SaldoEventArgs : EventArgs
{
    public double Saldo { get; set; }
}

class ContaBancaria
{
    // Implemente evento e método Sacar
}",
                    Hints = new List<string> 
                    { 
                        "Declare event EventHandler<SaldoEventArgs>",
                        "No Sacar, verifique se saldo < 0",
                        "Dispare evento passando saldo" 
                    }
                },
                new Exercise
                {
                    Title = "Múltiplos Subscribers",
                    Description = "Crie um sistema onde um evento é observado por múltiplos handlers que fazem coisas diferentes.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Crie classe com evento e múltiplos subscribers
",
                    Hints = new List<string> 
                    { 
                        "Use += para adicionar múltiplos handlers",
                        "Cada handler executa quando evento dispara",
                        "Teste com 2-3 handlers diferentes" 
                    }
                }
            },
            Summary = "Nesta aula você aprendeu sobre eventos, uma forma poderosa de implementar notificações e o padrão Observer. Você viu como criar, disparar e se inscrever em eventos, incluindo eventos customizados com dados. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno. Grandes empresas como Microsoft, Google e Amazon aplicam estes conceitos em seus sistemas. Ao aprender estas técnicas, você está adquirindo conhecimento usado por desenvolvedores em todo o mundo. Considere como este conceito poderia ser aplicado em um projeto pessoal. Pensar em aplicações práticas ajuda a solidificar o entendimento e torna o aprendizado mais significativo. Iniciantes frequentemente cometem alguns erros ao aprender este tópico. Conhecer estes erros comuns ajuda a evitá-los e desenvolver código mais robusto desde o início. Um erro comum é não considerar casos extremos ou situações inesperadas. Sempre teste seu código com diferentes tipos de entrada, incluindo valores nulos, vazios ou inválidos. Outro erro frequente é otimizar prematuramente. Primeiro faça o código funcionar corretamente, depois otimize se necessário. Como diz o ditado: 'premature optimization is the root of all evil'. Quando encontrar problemas, use o debugger para entender o que está acontecendo. Coloque breakpoints, inspecione variáveis e execute o código passo a passo. Esta é uma habilidade essencial para todo desenvolvedor. Mensagens de erro podem parecer intimidadoras no início, mas elas são suas aliadas. Leia-as cuidadosamente - elas geralmente indicam exatamente onde e qual é o problema. Se algo não funcionar como esperado, não desanime. Debugging é uma parte normal do desenvolvimento. Até desenvolvedores experientes passam tempo significativo depurando código. Testar seu código é tão importante quanto escrevê-lo. Testes automatizados garantem que seu código funciona corretamente e continuará funcionando quando você fizer mudanças. Comece escrevendo testes simples para os casos mais comuns. Depois adicione testes para casos extremos e situações de erro. Uma boa cobertura de testes dá confiança para refatorar e melhorar o código. Considere a prática de TDD (Test-Driven Development), onde você escreve os testes antes do código. Isso ajuda a pensar claramente sobre o que o código deve fazer. Embora a correção seja sempre a prioridade, é importante estar ciente das implicações de performance de suas escolhas. Diferentes abordagens podem ter custos computacionais muito diferentes. Em muitos casos, a diferença de performance é negligenciável para aplicações pequenas. No entanto, à medida que seus programas crescem, escolhas eficientes se tornam cada vez mais importantes. Aprenda a usar ferramentas de profiling para identificar gargalos de performance. Otimize baseado em dados reais, não em suposições sobre onde o código é lento. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0002-000000000016"),
            CourseId = _courseId,
            Title = "Eventos e Notificações",
            Duration = "55 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 55,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0002-000000000015" }),
            OrderIndex = 16,
            Content = "",
            StructuredContent = JsonSerializer.Serialize(content),
            Version = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    private Lesson CreateLesson17()
    {
        var content = new LessonContent
        {
            Objectives = new List<string>
            {
                "Entender extension methods e como criá-los",
                "Usar extension methods para estender tipos existentes",
                "Aplicar extension methods para código mais limpo"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "O Que São Extension Methods",
                    Content = "Extension methods permitem adicionar métodos a tipos existentes sem modificar o tipo original ou criar subclasse. Parecem métodos de instância mas são métodos estáticos especiais. Úteis para estender classes que você não controla (string, int, List, etc). Declare em classe estática: método estático com primeiro parâmetro usando 'this': 'public static bool EhPar(this int numero)'. Use como método de instância: '5.EhPar()'. Compilador converte para chamada estática. Extension methods são descobertos via using - devem estar em namespace importado. LINQ é implementado com extension methods (Where, Select, etc). Tornam código mais fluente e legível.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Criando Extension Methods",
                    Content = "Para criar extension method: crie classe estática (convenção: nome termina com Extensions), crie método estático público, primeiro parâmetro usa 'this' indicando tipo estendido. Exemplo: 'public static string Inverter(this string str)' adiciona método Inverter a string. Pode ter parâmetros adicionais: 'public static string Truncar(this string str, int tamanho)'. Extension methods não podem acessar membros privados do tipo - apenas públicos. Se tipo já tem método com mesmo nome, método original tem precedência. Use extension methods para operações utilitárias, não para lógica de negócio central. Mantenha-os simples e focados.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Boas Práticas",
                    Content = "Use extension methods para: operações utilitárias comuns, melhorar legibilidade, criar DSLs fluentes, estender tipos de biblioteca. Não use para: lógica de negócio complexa, operações que deveriam estar na classe original, substituir herança apropriada. Nomeie classe Extensions claramente: StringExtensions, ListExtensions. Agrupe por tipo estendido. Documente bem - não é óbvio que são extensions. Considere performance - extension methods têm overhead mínimo mas não zero. Não abuse - muitos extensions podem poluir IntelliSense. Use namespaces específicos para extensions opcionais. Extension methods são poderosos mas use com moderação.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Extension Methods Básicos",
                    Code = @"// Classe estática para extensions
public static class StringExtensions
{
    public static bool EhVazio(this string str)
    {
        return string.IsNullOrWhiteSpace(str);
    }
    
    public static string Truncar(this string str, int tamanho)
    {
        if (str.Length <= tamanho)
            return str;
        return str.Substring(0, tamanho) + ""..."";
    }
}

// Usar extensions
string texto = ""Olá, Mundo!"";
Console.WriteLine(texto.EhVazio());  // false

string longo = ""Este é um texto muito longo"";
Console.WriteLine(longo.Truncar(10));  // ""Este é um...""",
                    Language = "csharp",
                    Explanation = "Demonstra criação de extension methods para string. Usados como métodos de instância.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Extensions para Coleções",
                    Code = @"public static class ListExtensions
{
    public static void AdicionarSeNaoExistir<T>(this List<T> lista, T item)
    {
        if (!lista.Contains(item))
        {
            lista.Add(item);
        }
    }
    
    public static T ObterAleatorio<T>(this List<T> lista)
    {
        if (lista.Count == 0)
            throw new InvalidOperationException(""Lista vazia"");
        
        Random random = new Random();
        int indice = random.Next(lista.Count);
        return lista[indice];
    }
}

// Usar
List<string> nomes = new List<string> { ""Ana"", ""Bruno"" };
nomes.AdicionarSeNaoExistir(""Carlos"");
nomes.AdicionarSeNaoExistir(""Ana"");  // Não adiciona

Console.WriteLine($""Aleatório: {nomes.ObterAleatorio()}"");",
                    Language = "csharp",
                    Explanation = "Extension methods genéricos para List<T>, adicionando funcionalidades úteis.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Extension para Int",
                    Description = "Crie extension method EhPar() para int que retorna true se número é par.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"public static class IntExtensions
{
    // Implemente EhPar
}",
                    Hints = new List<string> 
                    { 
                        "public static bool EhPar(this int numero)",
                        "Retorne numero % 2 == 0",
                        "Teste com 5.EhPar()" 
                    }
                },
                new Exercise
                {
                    Title = "Extension para String",
                    Description = "Crie extension method ContarPalavras() para string que retorna número de palavras.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"public static class StringExtensions
{
    // Implemente ContarPalavras
}",
                    Hints = new List<string> 
                    { 
                        "public static int ContarPalavras(this string str)",
                        "Use str.Split(' ')",
                        "Retorne array.Length" 
                    }
                },
                new Exercise
                {
                    Title = "Extension Fluente",
                    Description = "Crie extension methods para List<int> que permitem encadeamento: lista.Filtrar(x => x > 5).Ordenar().Exibir().",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"public static class ListExtensions
{
    // Implemente Filtrar, Ordenar e Exibir
}",
                    Hints = new List<string> 
                    { 
                        "Filtrar retorna List<int>",
                        "Ordenar retorna List<int> (this)",
                        "Exibir retorna List<int> para encadear" 
                    }
                }
            },
            Summary = "Nesta aula você aprendeu sobre extension methods, uma forma poderosa de estender tipos existentes. Você viu como criar extension methods e aplicá-los para tornar código mais limpo e fluente. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno. Grandes empresas como Microsoft, Google e Amazon aplicam estes conceitos em seus sistemas. Ao aprender estas técnicas, você está adquirindo conhecimento usado por desenvolvedores em todo o mundo. Considere como este conceito poderia ser aplicado em um projeto pessoal. Pensar em aplicações práticas ajuda a solidificar o entendimento e torna o aprendizado mais significativo. Iniciantes frequentemente cometem alguns erros ao aprender este tópico. Conhecer estes erros comuns ajuda a evitá-los e desenvolver código mais robusto desde o início. Um erro comum é não considerar casos extremos ou situações inesperadas. Sempre teste seu código com diferentes tipos de entrada, incluindo valores nulos, vazios ou inválidos. Outro erro frequente é otimizar prematuramente. Primeiro faça o código funcionar corretamente, depois otimize se necessário. Como diz o ditado: 'premature optimization is the root of all evil'. Quando encontrar problemas, use o debugger para entender o que está acontecendo. Coloque breakpoints, inspecione variáveis e execute o código passo a passo. Esta é uma habilidade essencial para todo desenvolvedor. Mensagens de erro podem parecer intimidadoras no início, mas elas são suas aliadas. Leia-as cuidadosamente - elas geralmente indicam exatamente onde e qual é o problema. Se algo não funcionar como esperado, não desanime. Debugging é uma parte normal do desenvolvimento. Até desenvolvedores experientes passam tempo significativo depurando código. Testar seu código é tão importante quanto escrevê-lo. Testes automatizados garantem que seu código funciona corretamente e continuará funcionando quando você fizer mudanças. Comece escrevendo testes simples para os casos mais comuns. Depois adicione testes para casos extremos e situações de erro. Uma boa cobertura de testes dá confiança para refatorar e melhorar o código. Considere a prática de TDD (Test-Driven Development), onde você escreve os testes antes do código. Isso ajuda a pensar claramente sobre o que o código deve fazer. Embora a correção seja sempre a prioridade, é importante estar ciente das implicações de performance de suas escolhas. Diferentes abordagens podem ter custos computacionais muito diferentes. Em muitos casos, a diferença de performance é negligenciável para aplicações pequenas. No entanto, à medida que seus programas crescem, escolhas eficientes se tornam cada vez mais importantes. Aprenda a usar ferramentas de profiling para identificar gargalos de performance. Otimize baseado em dados reais, não em suposições sobre onde o código é lento. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0002-000000000017"),
            CourseId = _courseId,
            Title = "Extension Methods",
            Duration = "50 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 50,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0002-000000000016" }),
            OrderIndex = 17,
            Content = "",
            StructuredContent = JsonSerializer.Serialize(content),
            Version = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    private Lesson CreateLesson18()
    {
        var content = new LessonContent
        {
            Objectives = new List<string>
            {
                "Entender tuplas e quando usá-las",
                "Retornar múltiplos valores de métodos com tuplas",
                "Usar deconstruction para extrair valores de tuplas"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Introdução a Tuplas",
                    Content = "Tuplas permitem agrupar múltiplos valores em uma única estrutura leve. Úteis quando você precisa retornar múltiplos valores de um método sem criar classe dedicada. Sintaxe: (tipo1, tipo2) para declarar, (valor1, valor2) para criar. Exemplo: '(int, string) pessoa = (25, \"Ana\")'. Acesse com .Item1, .Item2, etc. Ou use nomes: '(int idade, string nome) pessoa = (25, \"Ana\")' e acesse com .idade, .nome. Tuplas são tipos de valor (struct) - eficientes. Diferentes de Tuple<> antigo que é classe. Use tuplas para retornos temporários, não para estruturas de dados permanentes. Para dados complexos, crie classe apropriada.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Retornando Múltiplos Valores",
                    Content = "Tuplas são ideais para retornar múltiplos valores. Antes, você criava classe, usava out parameters ou retornava array/lista. Tuplas são mais simples. Declare retorno: '(int min, int max) EncontrarExtremos(int[] numeros)'. Retorne: 'return (numeros.Min(), numeros.Max())'. Chame: 'var (min, max) = EncontrarExtremos(array)' - deconstruction extrai valores diretamente. Ou: 'var resultado = EncontrarExtremos(array)' e acesse 'resultado.min'. Tuplas tornam código mais limpo que out parameters. Use para 2-3 valores relacionados. Para mais valores ou estrutura complexa, crie classe.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Deconstruction",
                    Content = "Deconstruction extrai valores de tuplas em variáveis separadas. Sintaxe: 'var (var1, var2) = tupla'. Você pode especificar tipos: '(int x, string y) = tupla'. Descarte valores não necessários com _: 'var (nome, _) = ObterPessoa()' ignora segundo valor. Deconstruction funciona com qualquer tipo que implementa método Deconstruct. Você pode adicionar Deconstruct a suas classes para suportar deconstruction. Exemplo: 'public void Deconstruct(out string nome, out int idade)'. Deconstruction torna código mais conciso e legível. Especialmente útil em loops: 'foreach (var (chave, valor) in dicionario)'.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Tuplas Básicas",
                    Code = @"// Criar tupla
(int idade, string nome) pessoa = (28, ""Carlos"");
Console.WriteLine($""{pessoa.nome} tem {pessoa.idade} anos"");

// Retornar tupla de método
(int min, int max) EncontrarExtremos(int[] numeros)
{
    return (numeros.Min(), numeros.Max());
}

int[] valores = { 5, 2, 8, 1, 9, 3 };
var (minimo, maximo) = EncontrarExtremos(valores);
Console.WriteLine($""Min: {minimo}, Max: {maximo}"");",
                    Language = "csharp",
                    Explanation = "Demonstra criação de tuplas com nomes e retorno de múltiplos valores de método.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Deconstruction",
                    Code = @"// Método que retorna tupla
(bool sucesso, string mensagem, int codigo) ProcessarDados(string dados)
{
    if (string.IsNullOrEmpty(dados))
        return (false, ""Dados vazios"", 400);
    
    return (true, ""Sucesso"", 200);
}

// Deconstruction - extrair valores
var (sucesso, mensagem, codigo) = ProcessarDados(""teste"");
Console.WriteLine($""Sucesso: {sucesso}, Mensagem: {mensagem}, Código: {codigo}"");

// Descartar valores não necessários
var (_, msg, _) = ProcessarDados("""");
Console.WriteLine($""Mensagem: {msg}"");",
                    Language = "csharp",
                    Explanation = "Mostra deconstruction para extrair valores de tuplas e uso de _ para descartar valores.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Dividir Nome",
                    Description = "Crie um método que recebe nome completo e retorna tupla (primeiroNome, sobrenome).",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"(string primeiro, string sobrenome) DividirNome(string nomeCompleto)
{
    // Implemente aqui
}",
                    Hints = new List<string> 
                    { 
                        "Use Split(' ')",
                        "Retorne (partes[0], partes[1])",
                        "Considere nomes com mais de 2 partes" 
                    }
                },
                new Exercise
                {
                    Title = "Estatísticas de Lista",
                    Description = "Crie um método que retorna tupla com (soma, média, contagem) de uma lista de números.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"(int soma, double media, int contagem) CalcularEstatisticas(List<int> numeros)
{
    // Implemente aqui
}",
                    Hints = new List<string> 
                    { 
                        "soma = numeros.Sum()",
                        "media = numeros.Average()",
                        "contagem = numeros.Count" 
                    }
                },
                new Exercise
                {
                    Title = "Validar e Processar",
                    Description = "Crie um método que valida entrada e retorna tupla (valido, resultado, mensagemErro). Se válido, resultado contém valor processado.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"(bool valido, int resultado, string erro) ProcessarNumero(string entrada)
{
    // Implemente: tente converter para int
}",
                    Hints = new List<string> 
                    { 
                        "Use int.TryParse()",
                        "Se sucesso: (true, numero, null)",
                        "Se falha: (false, 0, \"Inválido\")" 
                    }
                }
            },
            Summary = "Nesta aula você aprendeu sobre tuplas, uma forma leve de agrupar múltiplos valores. Você viu como retornar múltiplos valores de métodos e usar deconstruction para extrair valores de forma concisa. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno. Grandes empresas como Microsoft, Google e Amazon aplicam estes conceitos em seus sistemas. Ao aprender estas técnicas, você está adquirindo conhecimento usado por desenvolvedores em todo o mundo. Considere como este conceito poderia ser aplicado em um projeto pessoal. Pensar em aplicações práticas ajuda a solidificar o entendimento e torna o aprendizado mais significativo. Iniciantes frequentemente cometem alguns erros ao aprender este tópico. Conhecer estes erros comuns ajuda a evitá-los e desenvolver código mais robusto desde o início. Um erro comum é não considerar casos extremos ou situações inesperadas. Sempre teste seu código com diferentes tipos de entrada, incluindo valores nulos, vazios ou inválidos. Outro erro frequente é otimizar prematuramente. Primeiro faça o código funcionar corretamente, depois otimize se necessário. Como diz o ditado: 'premature optimization is the root of all evil'. Quando encontrar problemas, use o debugger para entender o que está acontecendo. Coloque breakpoints, inspecione variáveis e execute o código passo a passo. Esta é uma habilidade essencial para todo desenvolvedor. Mensagens de erro podem parecer intimidadoras no início, mas elas são suas aliadas. Leia-as cuidadosamente - elas geralmente indicam exatamente onde e qual é o problema. Se algo não funcionar como esperado, não desanime. Debugging é uma parte normal do desenvolvimento. Até desenvolvedores experientes passam tempo significativo depurando código. Testar seu código é tão importante quanto escrevê-lo. Testes automatizados garantem que seu código funciona corretamente e continuará funcionando quando você fizer mudanças. Comece escrevendo testes simples para os casos mais comuns. Depois adicione testes para casos extremos e situações de erro. Uma boa cobertura de testes dá confiança para refatorar e melhorar o código. Considere a prática de TDD (Test-Driven Development), onde você escreve os testes antes do código. Isso ajuda a pensar claramente sobre o que o código deve fazer. Embora a correção seja sempre a prioridade, é importante estar ciente das implicações de performance de suas escolhas. Diferentes abordagens podem ter custos computacionais muito diferentes. Em muitos casos, a diferença de performance é negligenciável para aplicações pequenas. No entanto, à medida que seus programas crescem, escolhas eficientes se tornam cada vez mais importantes. Aprenda a usar ferramentas de profiling para identificar gargalos de performance. Otimize baseado em dados reais, não em suposições sobre onde o código é lento. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno. Grandes empresas como Microsoft, Google e Amazon aplicam estes conceitos em seus sistemas. Ao aprender estas técnicas, você está adquirindo conhecimento usado por desenvolvedores em todo o mundo. Considere como este conceito poderia ser aplicado em um projeto pessoal. Pensar em aplicações práticas ajuda a solidificar o entendimento e torna o aprendizado mais significativo."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0002-000000000018"),
            CourseId = _courseId,
            Title = "Tuplas e Múltiplos Retornos",
            Duration = "50 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 50,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0002-000000000017" }),
            OrderIndex = 18,
            Content = "",
            StructuredContent = JsonSerializer.Serialize(content),
            Version = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    private Lesson CreateLesson19()
    {
        var content = new LessonContent
        {
            Objectives = new List<string>
            {
                "Entender pattern matching e quando usá-lo",
                "Aplicar diferentes tipos de patterns",
                "Usar pattern matching para código mais expressivo"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Introdução a Pattern Matching",
                    Content = "Pattern matching permite testar se um valor corresponde a um padrão e extrair informações. Mais poderoso que if/else simples. C# oferece vários tipos de patterns. Type pattern verifica tipo: 'if (obj is string s)' verifica se obj é string e atribui a s. Constant pattern compara com constante: 'if (valor is null)'. Relational pattern usa operadores: 'if (idade is >= 18)'. Logical patterns combinam com and, or, not: 'if (x is > 0 and < 100)'. Pattern matching torna código mais conciso e expressivo. Especialmente útil com switch expressions.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Switch Expressions",
                    Content = "Switch expressions são forma moderna de switch. Retornam valor diretamente. Sintaxe: 'var resultado = valor switch { padrão1 => resultado1, padrão2 => resultado2, _ => padrãoPadrão }'. Underscore _ é discard pattern - corresponde a qualquer coisa. Exemplo: 'string tipo = idade switch { < 13 => \"Criança\", < 18 => \"Adolescente\", < 60 => \"Adulto\", _ => \"Idoso\" }'. Mais conciso que switch tradicional. Compilador avisa se você não cobre todos os casos. Use quando switch retorna valor. Para lógica complexa sem retorno, use switch tradicional. Switch expressions tornam código funcional e declarativo.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Property Patterns",
                    Content = "Property patterns testam propriedades de objetos. Sintaxe: 'objeto is { Propriedade: valor }'. Exemplo: 'if (pessoa is { Idade: >= 18, Nome: not null })'. Você pode aninhar patterns: 'if (pedido is { Cliente: { Ativo: true } })'. Combine com switch expressions: 'var desconto = cliente switch { { Vip: true } => 0.2, { Compras: > 10 } => 0.1, _ => 0 }'. Property patterns eliminam null checks verbosos. Tornam validações mais legíveis. Use para verificar múltiplas condições de forma declarativa. Especialmente útil com objetos complexos.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Pattern Matching Básico",
                    Code = @"// Type pattern
object obj = ""Olá"";
if (obj is string s)
{
    Console.WriteLine($""String: {s.ToUpper()}"");
}

// Relational pattern
int idade = 25;
if (idade is >= 18 and < 60)
{
    Console.WriteLine(""Adulto"");
}

// Null check
string? nome = null;
if (nome is not null)
{
    Console.WriteLine(nome);
}
else
{
    Console.WriteLine(""Nome não definido"");
}",
                    Language = "csharp",
                    Explanation = "Demonstra type pattern, relational pattern e null check com pattern matching.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Switch Expressions",
                    Code = @"// Classificar idade
string ClassificarIdade(int idade) => idade switch
{
    < 0 => ""Inválido"",
    < 13 => ""Criança"",
    < 18 => ""Adolescente"",
    < 60 => ""Adulto"",
    _ => ""Idoso""
};

Console.WriteLine(ClassificarIdade(10));
Console.WriteLine(ClassificarIdade(25));
Console.WriteLine(ClassificarIdade(70));

// Com múltiplas condições
string AvaliarNota(int nota) => nota switch
{
    >= 90 => ""A"",
    >= 80 => ""B"",
    >= 70 => ""C"",
    >= 60 => ""D"",
    _ => ""F""
};

Console.WriteLine($""Nota 85: {AvaliarNota(85)}"");",
                    Language = "csharp",
                    Explanation = "Switch expressions para classificação baseada em ranges. Mais conciso que if/else.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Classificar Número",
                    Description = "Use switch expression para classificar número como 'Negativo', 'Zero' ou 'Positivo'.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"string ClassificarNumero(int numero)
{
    // Implemente com switch expression
}",
                    Hints = new List<string> 
                    { 
                        "numero switch { ... }",
                        "< 0 => \"Negativo\"",
                        "0 => \"Zero\", _ => \"Positivo\"" 
                    }
                },
                new Exercise
                {
                    Title = "Calcular Desconto",
                    Description = "Use pattern matching para calcular desconto: VIP 20%, mais de 10 compras 10%, senão 0%.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"class Cliente
{
    public bool Vip { get; set; }
    public int Compras { get; set; }
}

double CalcularDesconto(Cliente cliente)
{
    // Implemente com switch expression e property patterns
}",
                    Hints = new List<string> 
                    { 
                        "cliente switch { ... }",
                        "{ Vip: true } => 0.2",
                        "{ Compras: > 10 } => 0.1" 
                    }
                },
                new Exercise
                {
                    Title = "Validar Objeto",
                    Description = "Use pattern matching para validar se pessoa tem nome não vazio e idade entre 0 e 150.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"class Pessoa
{
    public string Nome { get; set; }
    public int Idade { get; set; }
}

bool ValidarPessoa(Pessoa pessoa)
{
    // Implemente com property patterns
}",
                    Hints = new List<string> 
                    { 
                        "pessoa is { ... }",
                        "Nome: not null and not \"\"",
                        "Idade: >= 0 and <= 150" 
                    }
                }
            },
            Summary = "Nesta aula você aprendeu sobre pattern matching, uma forma poderosa e expressiva de testar valores. Você viu switch expressions, property patterns e como usar pattern matching para código mais limpo. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno. Grandes empresas como Microsoft, Google e Amazon aplicam estes conceitos em seus sistemas. Ao aprender estas técnicas, você está adquirindo conhecimento usado por desenvolvedores em todo o mundo. Considere como este conceito poderia ser aplicado em um projeto pessoal. Pensar em aplicações práticas ajuda a solidificar o entendimento e torna o aprendizado mais significativo. Iniciantes frequentemente cometem alguns erros ao aprender este tópico. Conhecer estes erros comuns ajuda a evitá-los e desenvolver código mais robusto desde o início. Um erro comum é não considerar casos extremos ou situações inesperadas. Sempre teste seu código com diferentes tipos de entrada, incluindo valores nulos, vazios ou inválidos. Outro erro frequente é otimizar prematuramente. Primeiro faça o código funcionar corretamente, depois otimize se necessário. Como diz o ditado: 'premature optimization is the root of all evil'. Quando encontrar problemas, use o debugger para entender o que está acontecendo. Coloque breakpoints, inspecione variáveis e execute o código passo a passo. Esta é uma habilidade essencial para todo desenvolvedor. Mensagens de erro podem parecer intimidadoras no início, mas elas são suas aliadas. Leia-as cuidadosamente - elas geralmente indicam exatamente onde e qual é o problema. Se algo não funcionar como esperado, não desanime. Debugging é uma parte normal do desenvolvimento. Até desenvolvedores experientes passam tempo significativo depurando código. Testar seu código é tão importante quanto escrevê-lo. Testes automatizados garantem que seu código funciona corretamente e continuará funcionando quando você fizer mudanças. Comece escrevendo testes simples para os casos mais comuns. Depois adicione testes para casos extremos e situações de erro. Uma boa cobertura de testes dá confiança para refatorar e melhorar o código. Considere a prática de TDD (Test-Driven Development), onde você escreve os testes antes do código. Isso ajuda a pensar claramente sobre o que o código deve fazer. Embora a correção seja sempre a prioridade, é importante estar ciente das implicações de performance de suas escolhas. Diferentes abordagens podem ter custos computacionais muito diferentes. Em muitos casos, a diferença de performance é negligenciável para aplicações pequenas. No entanto, à medida que seus programas crescem, escolhas eficientes se tornam cada vez mais importantes. Aprenda a usar ferramentas de profiling para identificar gargalos de performance. Otimize baseado em dados reais, não em suposições sobre onde o código é lento. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0002-000000000019"),
            CourseId = _courseId,
            Title = "Pattern Matching",
            Duration = "55 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 55,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0002-000000000018" }),
            OrderIndex = 19,
            Content = "",
            StructuredContent = JsonSerializer.Serialize(content),
            Version = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    private Lesson CreateLesson20()
    {
        var content = new LessonContent
        {
            Objectives = new List<string>
            {
                "Revisar todos os conceitos de C# Básico",
                "Integrar múltiplos conceitos em exemplos práticos",
                "Preparar-se para o projeto final: Sistema de Gerenciamento de Contatos"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Revisão de Coleções",
                    Content = "Você dominou as principais coleções do C#. List<T> para coleções ordenadas e dinâmicas - use quando precisa manter ordem e acessar por índice. Dictionary<TKey, TValue> para mapear chaves a valores com acesso O(1) - ideal para buscas rápidas. HashSet<T> para garantir unicidade e operações de conjunto - perfeito para remover duplicatas e verificar existência. Arrays para coleções de tamanho fixo. Cada coleção tem seu caso de uso ideal. Escolha baseado em: necessidade de ordem, unicidade, tipo de acesso (índice vs chave), operações principais. Combine coleções quando apropriado: Dictionary de Lists, HashSet para filtrar List, etc.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Revisão de Strings e File I/O",
                    Content = "Strings são imutáveis - operações criam novas strings. Use StringBuilder para construção eficiente. Métodos essenciais: Trim, ToUpper, ToLower, Replace, Split, Join, Substring, Contains. Regex para padrões complexos. File I/O: File.ReadAllText/WriteAllText para arquivos pequenos, StreamReader/StreamWriter para arquivos grandes. Directory para operações com pastas. JSON para serializar/desserializar objetos. Sempre use try-catch para I/O. Verifique existência com File.Exists. Use Path.Combine para caminhos portáveis. Using statement garante fechamento de streams. Essas ferramentas permitem persistir dados e processar arquivos.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Revisão de Recursos Avançados",
                    Content = "Nullable types (int?, string?) representam ausência de valor. Operadores null-safe (?., ??) tornam código seguro. Delegates e eventos permitem callbacks e notificações - base para arquitetura desacoplada. Extension methods estendem tipos existentes sem modificá-los. Tuplas retornam múltiplos valores de forma leve. Pattern matching torna testes e validações mais expressivos. Esses recursos tornam C# moderno e poderoso. Use-os para código mais limpo, seguro e expressivo. No projeto final, você integrará todos esses conceitos em um sistema completo de gerenciamento de contatos com persistência em arquivo JSON.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Integrando Conceitos",
                    Code = @"using System.Text.Json;

// Classe de domínio
class Contato
{
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Telefone { get; set; }
}

// Gerenciador com múltiplos conceitos
class GerenciadorContatos
{
    private List<Contato> _contatos = new List<Contato>();
    private HashSet<string> _emails = new HashSet<string>();
    
    public event EventHandler<Contato> ContatoAdicionado;
    
    public bool AdicionarContato(Contato contato)
    {
        // HashSet garante email único
        if (!_emails.Add(contato.Email))
            return false;
        
        _contatos.Add(contato);
        ContatoAdicionado?.Invoke(this, contato);
        return true;
    }
    
    public void Salvar(string caminho)
    {
        string json = JsonSerializer.Serialize(_contatos, 
            new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(caminho, json);
    }
}",
                    Language = "csharp",
                    Explanation = "Exemplo integrando classes, coleções, eventos e JSON - base para o projeto final.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Extension Methods Práticos",
                    Code = @"public static class ContatoExtensions
{
    public static bool EmailValido(this Contato contato)
    {
        return !string.IsNullOrEmpty(contato.Email) && 
               contato.Email.Contains(""@"");
    }
    
    public static string FormatarExibicao(this Contato contato)
    {
        return $""{contato.Nome} ({contato.Email})"";
    }
}

// Usar extensions
Contato contato = new Contato 
{ 
    Nome = ""Ana"", 
    Email = ""ana@example.com"" 
};

if (contato.EmailValido())
{
    Console.WriteLine(contato.FormatarExibicao());
}",
                    Language = "csharp",
                    Explanation = "Extension methods para adicionar funcionalidades a classe Contato de forma limpa.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Sistema Simples de Tarefas",
                    Description = "Crie um sistema que gerencia lista de tarefas, permite adicionar/remover, salvar em JSON e carregar.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"class Tarefa
{
    public string Descricao { get; set; }
    public bool Concluida { get; set; }
}

class GerenciadorTarefas
{
    // Implemente: lista, adicionar, remover, salvar, carregar
}",
                    Hints = new List<string> 
                    { 
                        "Use List<Tarefa>",
                        "JsonSerializer para salvar/carregar",
                        "Métodos: Adicionar, Remover, Salvar, Carregar" 
                    }
                },
                new Exercise
                {
                    Title = "Validador de Dados",
                    Description = "Crie classe que valida dados de contato (nome não vazio, email com @, telefone com 10-11 dígitos) usando pattern matching.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"class ValidadorContato
{
    public (bool valido, List<string> erros) Validar(Contato contato)
    {
        // Implemente validações
    }
}",
                    Hints = new List<string> 
                    { 
                        "Crie List<string> para erros",
                        "Verifique cada campo",
                        "Retorne tupla (erros.Count == 0, erros)" 
                    }
                },
                new Exercise
                {
                    Title = "Busca Avançada",
                    Description = "Implemente busca de contatos por nome (parcial), email ou telefone, retornando lista de resultados.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"class GerenciadorContatos
{
    private List<Contato> _contatos;
    
    public List<Contato> Buscar(string termo)
    {
        // Implemente busca em nome, email e telefone
    }
}",
                    Hints = new List<string> 
                    { 
                        "Use FindAll ou Where",
                        "Verifique Contains em cada campo",
                        "Use ToLower() para case-insensitive" 
                    }
                }
            },
            Summary = "Nesta aula você revisou todos os conceitos de C# Básico: coleções (List, Dictionary, HashSet), manipulação de strings, file I/O, JSON, nullable types, delegates, eventos, extension methods, tuplas e pattern matching. Você está pronto para o projeto final! Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno. Grandes empresas como Microsoft, Google e Amazon aplicam estes conceitos em seus sistemas. Ao aprender estas técnicas, você está adquirindo conhecimento usado por desenvolvedores em todo o mundo. Considere como este conceito poderia ser aplicado em um projeto pessoal. Pensar em aplicações práticas ajuda a solidificar o entendimento e torna o aprendizado mais significativo. Iniciantes frequentemente cometem alguns erros ao aprender este tópico. Conhecer estes erros comuns ajuda a evitá-los e desenvolver código mais robusto desde o início. Um erro comum é não considerar casos extremos ou situações inesperadas. Sempre teste seu código com diferentes tipos de entrada, incluindo valores nulos, vazios ou inválidos. Outro erro frequente é otimizar prematuramente. Primeiro faça o código funcionar corretamente, depois otimize se necessário. Como diz o ditado: 'premature optimization is the root of all evil'. Quando encontrar problemas, use o debugger para entender o que está acontecendo. Coloque breakpoints, inspecione variáveis e execute o código passo a passo. Esta é uma habilidade essencial para todo desenvolvedor. Mensagens de erro podem parecer intimidadoras no início, mas elas são suas aliadas. Leia-as cuidadosamente - elas geralmente indicam exatamente onde e qual é o problema. Se algo não funcionar como esperado, não desanime. Debugging é uma parte normal do desenvolvimento. Até desenvolvedores experientes passam tempo significativo depurando código. Testar seu código é tão importante quanto escrevê-lo. Testes automatizados garantem que seu código funciona corretamente e continuará funcionando quando você fizer mudanças. Comece escrevendo testes simples para os casos mais comuns. Depois adicione testes para casos extremos e situações de erro. Uma boa cobertura de testes dá confiança para refatorar e melhorar o código. Considere a prática de TDD (Test-Driven Development), onde você escreve os testes antes do código. Isso ajuda a pensar claramente sobre o que o código deve fazer. Embora a correção seja sempre a prioridade, é importante estar ciente das implicações de performance de suas escolhas. Diferentes abordagens podem ter custos computacionais muito diferentes. Em muitos casos, a diferença de performance é negligenciável para aplicações pequenas. No entanto, à medida que seus programas crescem, escolhas eficientes se tornam cada vez mais importantes. Aprenda a usar ferramentas de profiling para identificar gargalos de performance. Otimize baseado em dados reais, não em suposições sobre onde o código é lento. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0002-000000000020"),
            CourseId = _courseId,
            Title = "Revisão e Integração",
            Duration = "60 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 60,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0002-000000000019" }),
            OrderIndex = 20,
            Content = "",
            StructuredContent = JsonSerializer.Serialize(content),
            Version = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }
}
