using Shared.Entities;
using Shared.Models;
using System.Text.Json;

namespace Shared.Data;

/// <summary>
/// Seeds Level 1 content: C# Basics (20 lessons)
/// Covers C# syntax, collections (List, Dictionary), string manipulation, and file I/O
/// </summary>
public partial class Level1ContentSeeder
{
    private readonly Guid _courseId = Guid.Parse("10000000-0000-0000-0000-000000000002");
    private readonly Guid _levelId = Guid.Parse("00000000-0000-0000-0000-000000000001");

    public Course CreateLevel1Course()
    {
        return new Course
        {
            Id = _courseId,
            LevelId = _levelId,
            Title = "Programação Orientada a Objetos",
            Description = "Domine os fundamentos específicos do C# incluindo sintaxe, convenções, coleções (List, Dictionary), manipulação avançada de strings e operações básicas de arquivo. Construa uma base sólida para desenvolvimento profissional em C#.",
            Level = Level.Beginner,
            Duration = "4 semanas",
            LessonCount = 20,
            Topics = JsonSerializer.Serialize(new[] 
            { 
                "Sintaxe C#", 
                "Convenções", 
                "List", 
                "Dictionary", 
                "Strings", 
                "File I/O" 
            }),
            OrderIndex = 2,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    public List<Lesson> CreateLevel1Lessons()
    {
        var lessons = new List<Lesson>
        {
            CreateLesson1(),
            CreateLesson2(),
            CreateLesson3(),
            CreateLesson4(),
            CreateLesson5(),
            CreateLesson6(),
            CreateLesson7(),
            CreateLesson8(),
            CreateLesson9(),
            CreateLesson10(),
            CreateLesson11(),
            CreateLesson12(),
            CreateLesson13(),
            CreateLesson14(),
            CreateLesson15(),
            CreateLesson16(),
            CreateLesson17(),
            CreateLesson18(),
            CreateLesson19(),
            CreateLesson20()
        };

        return lessons;
    }

    private Lesson CreateLesson1()
    {
        var content = new LessonContent
        {
            Objectives = new List<string>
            {
                "Compreender as convenções de nomenclatura do C#",
                "Aplicar PascalCase e camelCase corretamente",
                "Escrever código que segue as diretrizes de estilo do C#"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Convenções de Nomenclatura em C#",
                    Content = "C# possui convenções de nomenclatura bem definidas que tornam o código mais legível e profissional. PascalCase é usado para classes, métodos e propriedades públicas - cada palavra começa com letra maiúscula, como 'CalcularPrecoTotal'. camelCase é usado para variáveis locais e parâmetros - primeira palavra em minúscula, demais em maiúscula, como 'precoUnitario'. Constantes usam UPPER_CASE com underscores. Interfaces começam com 'I', como 'IRepositorio'. Seguir essas convenções não é apenas estética - facilita a leitura do código por outros desenvolvedores e demonstra profissionalismo. O Visual Studio e outras IDEs ajudam sugerindo nomes corretos. Essas convenções são parte da cultura C# e são esperadas em código profissional.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Organização de Código",
                    Content = "C# incentiva organização clara do código. Namespaces agrupam classes relacionadas, como 'System.Collections.Generic'. Using statements no topo do arquivo importam namespaces necessários. Classes devem ter responsabilidade única e nomes descritivos. Métodos devem ser pequenos e focados em uma tarefa. Indentação consistente (geralmente 4 espaços) melhora legibilidade. Comentários explicam 'por quê', não 'o quê' - o código deve ser autoexplicativo. Linhas em branco separam blocos lógicos. Chaves sempre em nova linha no estilo C#. Essas práticas tornam o código mais fácil de manter e entender. Código bem organizado é código profissional.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Tipos de Valor vs Tipos de Referência",
                    Content = "C# tem dois tipos fundamentais de dados: tipos de valor e tipos de referência. Tipos de valor (int, double, bool, struct) armazenam dados diretamente na memória stack. Quando você copia um tipo de valor, cria uma cópia independente. Tipos de referência (classes, strings, arrays) armazenam uma referência ao objeto na heap. Copiar uma referência não copia o objeto, apenas cria outra referência ao mesmo objeto. Isso afeta como você trabalha com dados. Modificar um objeto através de uma referência afeta todas as referências a ele. Tipos de valor são geralmente mais rápidos e usam menos memória. Tipos de referência permitem estruturas mais complexas e compartilhamento de dados. Entender essa diferença é crucial para evitar bugs sutis.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Convenções de Nomenclatura",
                    Code = @"// PascalCase para classes e métodos
public class GerenciadorPedidos
{
    // PascalCase para propriedades
    public string NomeCliente { get; set; }
    
    // camelCase para variáveis locais
    public void ProcessarPedido()
    {
        int quantidadeItens = 5;
        double precoTotal = 99.99;
        string codigoPedido = ""PED-001"";
    }
}",
                    Language = "csharp",
                    Explanation = "Este exemplo demonstra as convenções de nomenclatura do C#: PascalCase para classes, métodos e propriedades; camelCase para variáveis locais.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Tipos de Valor vs Referência",
                    Code = @"// Tipo de valor
int a = 10;
int b = a;  // Copia o valor
b = 20;
Console.WriteLine($""a = {a}, b = {b}"");  // a = 10, b = 20

// Tipo de referência
int[] array1 = { 1, 2, 3 };
int[] array2 = array1;  // Copia a referência
array2[0] = 99;
Console.WriteLine($""array1[0] = {array1[0]}"");  // 99",
                    Language = "csharp",
                    Explanation = "Demonstra a diferença entre tipos de valor (cópia independente) e tipos de referência (referência compartilhada ao mesmo objeto).",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Aplicar Convenções de Nomenclatura",
                    Description = "Corrija os nomes das variáveis e métodos para seguir as convenções do C#.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"public class calculadora
{
    public int Somar(int Numero1, int Numero2)
    {
        int RESULTADO = Numero1 + Numero2;
        return RESULTADO;
    }
}",
                    Hints = new List<string> 
                    { 
                        "Classes usam PascalCase",
                        "Parâmetros e variáveis locais usam camelCase",
                        "Métodos usam PascalCase" 
                    }
                },
                new Exercise
                {
                    Title = "Entender Tipos de Valor",
                    Description = "Crie duas variáveis int, copie uma para outra, modifique a cópia e exiba ambos os valores para verificar que são independentes.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"// Declare e teste tipos de valor
",
                    Hints = new List<string> 
                    { 
                        "Declare int x = 10",
                        "Copie para int y = x",
                        "Modifique y e exiba ambos" 
                    }
                },
                new Exercise
                {
                    Title = "Criar Classe Bem Organizada",
                    Description = "Crie uma classe 'Produto' com propriedades Nome e Preco, e um método CalcularDesconto que retorna o preço com 10% de desconto.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Crie a classe Produto aqui
",
                    Hints = new List<string> 
                    { 
                        "Use PascalCase para classe e propriedades",
                        "Propriedades: public string Nome { get; set; }",
                        "Método retorna Preco * 0.9" 
                    }
                }
            },
            Summary = "Nesta aula você aprendeu as convenções de nomenclatura do C# (PascalCase e camelCase), como organizar código de forma profissional e a diferença fundamental entre tipos de valor e tipos de referência. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno. Grandes empresas como Microsoft, Google e Amazon aplicam estes conceitos em seus sistemas. Ao aprender estas técnicas, você está adquirindo conhecimento usado por desenvolvedores em todo o mundo. Considere como este conceito poderia ser aplicado em um projeto pessoal. Pensar em aplicações práticas ajuda a solidificar o entendimento e torna o aprendizado mais significativo. Iniciantes frequentemente cometem alguns erros ao aprender este tópico. Conhecer estes erros comuns ajuda a evitá-los e desenvolver código mais robusto desde o início. Um erro comum é não considerar casos extremos ou situações inesperadas. Sempre teste seu código com diferentes tipos de entrada, incluindo valores nulos, vazios ou inválidos. Outro erro frequente é otimizar prematuramente. Primeiro faça o código funcionar corretamente, depois otimize se necessário. Como diz o ditado: 'premature optimization is the root of all evil'. Quando encontrar problemas, use o debugger para entender o que está acontecendo. Coloque breakpoints, inspecione variáveis e execute o código passo a passo. Esta é uma habilidade essencial para todo desenvolvedor. Mensagens de erro podem parecer intimidadoras no início, mas elas são suas aliadas. Leia-as cuidadosamente - elas geralmente indicam exatamente onde e qual é o problema. Se algo não funcionar como esperado, não desanime. Debugging é uma parte normal do desenvolvimento. Até desenvolvedores experientes passam tempo significativo depurando código. Testar seu código é tão importante quanto escrevê-lo. Testes automatizados garantem que seu código funciona corretamente e continuará funcionando quando você fizer mudanças. Comece escrevendo testes simples para os casos mais comuns. Depois adicione testes para casos extremos e situações de erro. Uma boa cobertura de testes dá confiança para refatorar e melhorar o código. Considere a prática de TDD (Test-Driven Development), onde você escreve os testes antes do código. Isso ajuda a pensar claramente sobre o que o código deve fazer. Embora a correção seja sempre a prioridade, é importante estar ciente das implicações de performance de suas escolhas. Diferentes abordagens podem ter custos computacionais muito diferentes. Em muitos casos, a diferença de performance é negligenciável para aplicações pequenas. No entanto, à medida que seus programas crescem, escolhas eficientes se tornam cada vez mais importantes. Aprenda a usar ferramentas de profiling para identificar gargalos de performance. Otimize baseado em dados reais, não em suposições sobre onde o código é lento. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0002-000000000001"),
            CourseId = _courseId,
            Title = "Convenções e Sintaxe do C#",
            Duration = "45 min",
            Difficulty = "Iniciante",
            EstimatedMinutes = 45,
            Prerequisites = "[]",
            OrderIndex = 1,
            Content = "",
            StructuredContent = JsonSerializer.Serialize(content),
            Version = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    private Lesson CreateLesson2()
    {
        var content = new LessonContent
        {
            Objectives = new List<string>
            {
                "Compreender o conceito de nullable types",
                "Usar operadores null-conditional e null-coalescing",
                "Evitar NullReferenceException em seu código"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "O Problema do Null",
                    Content = "Null representa a ausência de valor e é uma das fontes mais comuns de erros em programação. NullReferenceException ocorre quando você tenta acessar membros de um objeto que é null. Por exemplo, se uma variável string é null e você tenta chamar .Length, o programa falha. C# oferece várias ferramentas para lidar com null de forma segura. Entender null é crucial porque muitas operações podem retornar null: buscar um item que não existe, ler um arquivo que não está lá, etc. Código robusto sempre considera a possibilidade de null. Ignorar null leva a crashes em produção. C# moderno oferece recursos poderosos para tornar o código null-safe.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Nullable Types",
                    Content = "Por padrão, tipos de valor (int, double, bool) não podem ser null. Mas às vezes você precisa representar 'sem valor'. Nullable types permitem isso adicionando '?' ao tipo: int? pode ser um número ou null. Você verifica se tem valor com HasValue e acessa o valor com Value. Nullable types são úteis para representar dados opcionais, como 'data de nascimento' que pode não estar preenchida. Tipos de referência sempre podem ser null, mas C# 8+ introduziu nullable reference types para tornar isso explícito. string? indica que pode ser null, string indica que não deveria ser. Isso ajuda o compilador a avisar sobre possíveis problemas de null.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Operadores Null-Safe",
                    Content = "C# oferece operadores especiais para trabalhar com null de forma segura. O operador null-conditional '?.' acessa membros apenas se o objeto não for null: 'pessoa?.Nome' retorna null se pessoa for null, evitando exceção. O operador null-coalescing '??' fornece valor padrão: 'nome ?? \"Desconhecido\"' usa nome se não for null, senão usa \"Desconhecido\". O operador '??=' atribui apenas se for null. Esses operadores tornam o código mais conciso e seguro. Em vez de múltiplos if checks, você usa operadores expressivos. Combine-os para código elegante: 'pessoa?.Nome ?? \"Anônimo\"' retorna o nome ou \"Anônimo\" se pessoa ou Nome forem null.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Nullable Types",
                    Code = @"// int normal não pode ser null
int numero = 10;

// int? pode ser null
int? numeroOpcional = null;

if (numeroOpcional.HasValue)
{
    Console.WriteLine($""Valor: {numeroOpcional.Value}"");
}
else
{
    Console.WriteLine(""Sem valor"");
}

// Atribuir valor
numeroOpcional = 42;
Console.WriteLine($""Agora tem valor: {numeroOpcional}"");",
                    Language = "csharp",
                    Explanation = "Demonstra nullable types com int?. HasValue verifica se tem valor, Value acessa o valor. Útil para representar dados opcionais.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Operadores Null-Safe",
                    Code = @"string? nome = null;

// Null-conditional: acessa apenas se não for null
int? tamanho = nome?.Length;
Console.WriteLine($""Tamanho: {tamanho}"");  // null

// Null-coalescing: fornece valor padrão
string nomeExibir = nome ?? ""Anônimo"";
Console.WriteLine($""Nome: {nomeExibir}"");  // Anônimo

// Combinados
nome = ""Carlos"";
string resultado = nome?.ToUpper() ?? ""SEM NOME"";
Console.WriteLine(resultado);  // CARLOS",
                    Language = "csharp",
                    Explanation = "Mostra operadores ?. (null-conditional) e ?? (null-coalescing) para trabalhar com null de forma segura e concisa.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Usar Nullable Int",
                    Description = "Crie uma variável int? que representa a idade de uma pessoa. Se a idade não estiver definida (null), exiba 'Idade não informada', senão exiba a idade.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"int? idade = null;
// Verifique e exiba a idade
",
                    Hints = new List<string> 
                    { 
                        "Use HasValue para verificar",
                        "Use Value para acessar o número",
                        "Ou use ?? para valor padrão" 
                    }
                },
                new Exercise
                {
                    Title = "Operador Null-Conditional",
                    Description = "Dada uma string que pode ser null, use o operador ?. para obter o tamanho da string de forma segura.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"string? texto = null;
// Obtenha o tamanho de forma segura
",
                    Hints = new List<string> 
                    { 
                        "Use texto?.Length",
                        "O resultado será int? (nullable)",
                        "Exiba o resultado" 
                    }
                },
                new Exercise
                {
                    Title = "Combinar Operadores",
                    Description = "Crie uma função que recebe um nome (string?) e retorna o nome em maiúsculas, ou 'VISITANTE' se o nome for null ou vazio.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"string ObterNomeExibicao(string? nome)
{
    // Implemente aqui
}",
                    Hints = new List<string> 
                    { 
                        "Use nome?.ToUpper()",
                        "Use ?? para valor padrão",
                        "Considere string.IsNullOrEmpty()" 
                    }
                }
            },
            Summary = "Nesta aula você aprendeu sobre nullable types (int?, string?), como usar operadores null-safe (?. e ??) para evitar NullReferenceException e escrever código mais seguro e conciso. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno. Grandes empresas como Microsoft, Google e Amazon aplicam estes conceitos em seus sistemas. Ao aprender estas técnicas, você está adquirindo conhecimento usado por desenvolvedores em todo o mundo. Considere como este conceito poderia ser aplicado em um projeto pessoal. Pensar em aplicações práticas ajuda a solidificar o entendimento e torna o aprendizado mais significativo. Iniciantes frequentemente cometem alguns erros ao aprender este tópico. Conhecer estes erros comuns ajuda a evitá-los e desenvolver código mais robusto desde o início. Um erro comum é não considerar casos extremos ou situações inesperadas. Sempre teste seu código com diferentes tipos de entrada, incluindo valores nulos, vazios ou inválidos. Outro erro frequente é otimizar prematuramente. Primeiro faça o código funcionar corretamente, depois otimize se necessário. Como diz o ditado: 'premature optimization is the root of all evil'. Quando encontrar problemas, use o debugger para entender o que está acontecendo. Coloque breakpoints, inspecione variáveis e execute o código passo a passo. Esta é uma habilidade essencial para todo desenvolvedor. Mensagens de erro podem parecer intimidadoras no início, mas elas são suas aliadas. Leia-as cuidadosamente - elas geralmente indicam exatamente onde e qual é o problema. Se algo não funcionar como esperado, não desanime. Debugging é uma parte normal do desenvolvimento. Até desenvolvedores experientes passam tempo significativo depurando código. Testar seu código é tão importante quanto escrevê-lo. Testes automatizados garantem que seu código funciona corretamente e continuará funcionando quando você fizer mudanças. Comece escrevendo testes simples para os casos mais comuns. Depois adicione testes para casos extremos e situações de erro. Uma boa cobertura de testes dá confiança para refatorar e melhorar o código. Considere a prática de TDD (Test-Driven Development), onde você escreve os testes antes do código. Isso ajuda a pensar claramente sobre o que o código deve fazer. Embora a correção seja sempre a prioridade, é importante estar ciente das implicações de performance de suas escolhas. Diferentes abordagens podem ter custos computacionais muito diferentes. Em muitos casos, a diferença de performance é negligenciável para aplicações pequenas. No entanto, à medida que seus programas crescem, escolhas eficientes se tornam cada vez mais importantes. Aprenda a usar ferramentas de profiling para identificar gargalos de performance. Otimize baseado em dados reais, não em suposições sobre onde o código é lento. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0002-000000000002"),
            CourseId = _courseId,
            Title = "Trabalhando com Null",
            Duration = "50 min",
            Difficulty = "Iniciante",
            EstimatedMinutes = 50,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0002-000000000001" }),
            OrderIndex = 2,
            Content = "",
            StructuredContent = JsonSerializer.Serialize(content),
            Version = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    private Lesson CreateLesson3()
    {
        var content = new LessonContent
        {
            Objectives = new List<string>
            {
                "Compreender o que é uma List<T> e quando usá-la",
                "Adicionar, remover e acessar elementos em uma lista",
                "Iterar sobre listas usando foreach e for"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Introdução às Listas",
                    Content = "List<T> é uma coleção dinâmica que pode crescer e diminuir conforme necessário. Diferente de arrays que têm tamanho fixo, listas são flexíveis. O 'T' é um tipo genérico - você especifica o tipo dos elementos: List<int> para números, List<string> para textos. Listas são parte do namespace System.Collections.Generic. Elas são extremamente comuns em C# porque combinam facilidade de uso com bom desempenho. Internamente, uma lista usa um array que é redimensionado automaticamente quando necessário. Listas mantêm a ordem de inserção dos elementos. Você pode ter elementos duplicados. Listas são tipos de referência, então passar uma lista para um método não cria uma cópia.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Operações Básicas com Listas",
                    Content = "Para criar uma lista, use 'new List<T>()' ou a sintaxe de inicialização de coleção. Add() adiciona um elemento ao final. Remove() remove a primeira ocorrência de um elemento. RemoveAt() remove por índice. Insert() adiciona em posição específica. Clear() remove todos os elementos. Count retorna o número de elementos. Contains() verifica se um elemento existe. IndexOf() retorna o índice de um elemento. Você acessa elementos por índice como arrays: lista[0] é o primeiro elemento. Listas lançam exceção se você acessa índice inválido. Sort() ordena os elementos. Reverse() inverte a ordem. Essas operações tornam listas muito versáteis para gerenciar coleções de dados.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Iterando sobre Listas",
                    Content = "Há várias formas de iterar sobre uma lista. foreach é a mais simples e legível: 'foreach (var item in lista)' processa cada elemento. Use quando não precisa do índice. for tradicional dá acesso ao índice: 'for (int i = 0; i < lista.Count; i++)' permite acessar lista[i]. Útil quando precisa modificar elementos ou saber a posição. ForEach() é um método que aceita uma ação: 'lista.ForEach(item => Console.WriteLine(item))'. LINQ oferece métodos poderosos como Where, Select, Any, All para consultas complexas. Escolha o método de iteração baseado em suas necessidades: foreach para simplicidade, for para controle de índice, LINQ para transformações.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Criando e Manipulando Listas",
                    Code = @"// Criar lista vazia
List<string> nomes = new List<string>();

// Adicionar elementos
nomes.Add(""Ana"");
nomes.Add(""Bruno"");
nomes.Add(""Carlos"");

Console.WriteLine($""Total: {nomes.Count}"");

// Acessar por índice
Console.WriteLine($""Primeiro: {nomes[0]}"");

// Remover elemento
nomes.Remove(""Bruno"");

// Verificar se contém
bool temAna = nomes.Contains(""Ana"");
Console.WriteLine($""Tem Ana? {temAna}"");",
                    Language = "csharp",
                    Explanation = "Demonstra operações básicas com List: criar, adicionar, acessar, remover e verificar elementos.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Iterando sobre Listas",
                    Code = @"List<int> numeros = new List<int> { 10, 20, 30, 40, 50 };

// foreach - mais simples
Console.WriteLine(""Usando foreach:"");
foreach (int num in numeros)
{
    Console.WriteLine(num);
}

// for - com índice
Console.WriteLine(""\nUsando for:"");
for (int i = 0; i < numeros.Count; i++)
{
    Console.WriteLine($""Índice {i}: {numeros[i]}"");
}",
                    Language = "csharp",
                    Explanation = "Mostra duas formas de iterar: foreach (simples) e for (com acesso ao índice).",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Criar Lista de Números",
                    Description = "Crie uma List<int> com os números 1 a 5, depois adicione o número 6 e exiba todos os elementos.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"// Crie e manipule a lista
",
                    Hints = new List<string> 
                    { 
                        "Use new List<int> { 1, 2, 3, 4, 5 }",
                        "Use Add(6) para adicionar",
                        "Use foreach para exibir" 
                    }
                },
                new Exercise
                {
                    Title = "Remover Elementos",
                    Description = "Crie uma lista com 5 nomes, remova o segundo nome e exiba a lista resultante.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"List<string> nomes = new List<string> { ""Ana"", ""Bruno"", ""Carlos"", ""Diana"", ""Eduardo"" };
// Remova o segundo nome e exiba
",
                    Hints = new List<string> 
                    { 
                        "Use RemoveAt(1) para remover por índice",
                        "Ou use Remove(\"Bruno\") para remover por valor",
                        "Exiba com foreach" 
                    }
                },
                new Exercise
                {
                    Title = "Filtrar Números Pares",
                    Description = "Dada uma lista de números, crie uma nova lista contendo apenas os números pares.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"List<int> numeros = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
List<int> pares = new List<int>();
// Filtre os pares
",
                    Hints = new List<string> 
                    { 
                        "Use foreach para iterar",
                        "Verifique se num % 2 == 0",
                        "Adicione à lista pares" 
                    }
                }
            },
            Summary = "Nesta aula você aprendeu sobre List<T>, uma coleção dinâmica essencial em C#. Você viu como adicionar, remover, acessar elementos e iterar sobre listas usando foreach e for. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno. Grandes empresas como Microsoft, Google e Amazon aplicam estes conceitos em seus sistemas. Ao aprender estas técnicas, você está adquirindo conhecimento usado por desenvolvedores em todo o mundo. Considere como este conceito poderia ser aplicado em um projeto pessoal. Pensar em aplicações práticas ajuda a solidificar o entendimento e torna o aprendizado mais significativo. Iniciantes frequentemente cometem alguns erros ao aprender este tópico. Conhecer estes erros comuns ajuda a evitá-los e desenvolver código mais robusto desde o início. Um erro comum é não considerar casos extremos ou situações inesperadas. Sempre teste seu código com diferentes tipos de entrada, incluindo valores nulos, vazios ou inválidos. Outro erro frequente é otimizar prematuramente. Primeiro faça o código funcionar corretamente, depois otimize se necessário. Como diz o ditado: 'premature optimization is the root of all evil'. Quando encontrar problemas, use o debugger para entender o que está acontecendo. Coloque breakpoints, inspecione variáveis e execute o código passo a passo. Esta é uma habilidade essencial para todo desenvolvedor. Mensagens de erro podem parecer intimidadoras no início, mas elas são suas aliadas. Leia-as cuidadosamente - elas geralmente indicam exatamente onde e qual é o problema. Se algo não funcionar como esperado, não desanime. Debugging é uma parte normal do desenvolvimento. Até desenvolvedores experientes passam tempo significativo depurando código. Testar seu código é tão importante quanto escrevê-lo. Testes automatizados garantem que seu código funciona corretamente e continuará funcionando quando você fizer mudanças. Comece escrevendo testes simples para os casos mais comuns. Depois adicione testes para casos extremos e situações de erro. Uma boa cobertura de testes dá confiança para refatorar e melhorar o código. Considere a prática de TDD (Test-Driven Development), onde você escreve os testes antes do código. Isso ajuda a pensar claramente sobre o que o código deve fazer. Embora a correção seja sempre a prioridade, é importante estar ciente das implicações de performance de suas escolhas. Diferentes abordagens podem ter custos computacionais muito diferentes. Em muitos casos, a diferença de performance é negligenciável para aplicações pequenas. No entanto, à medida que seus programas crescem, escolhas eficientes se tornam cada vez mais importantes. Aprenda a usar ferramentas de profiling para identificar gargalos de performance. Otimize baseado em dados reais, não em suposições sobre onde o código é lento. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0002-000000000003"),
            CourseId = _courseId,
            Title = "Introdução a List<T>",
            Duration = "50 min",
            Difficulty = "Iniciante",
            EstimatedMinutes = 50,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0002-000000000002" }),
            OrderIndex = 3,
            Content = "",
            StructuredContent = JsonSerializer.Serialize(content),
            Version = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    private Lesson CreateLesson4()
    {
        var content = new LessonContent
        {
            Objectives = new List<string>
            {
                "Usar métodos avançados de List como Find, FindAll e Sort",
                "Aplicar predicados e comparadores personalizados",
                "Otimizar operações com listas grandes"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Métodos de Busca",
                    Content = "List oferece métodos poderosos para buscar elementos. Find() retorna o primeiro elemento que satisfaz uma condição, ou null se não encontrar. FindAll() retorna uma nova lista com todos os elementos que satisfazem a condição. FindIndex() retorna o índice do primeiro elemento que satisfaz a condição. Exists() verifica se pelo menos um elemento satisfaz a condição. Esses métodos usam predicados - funções que retornam bool. Você pode usar lambda expressions para definir predicados de forma concisa: 'lista.Find(x => x > 10)' encontra o primeiro número maior que 10. Predicados tornam buscas muito expressivas e legíveis. São mais eficientes que iterar manualmente porque o método pode otimizar a busca.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Ordenação de Listas",
                    Content = "Sort() ordena a lista in-place (modifica a lista original). Para tipos simples como int e string, a ordenação é automática. Para objetos personalizados, você precisa especificar como ordenar. Há três formas: implementar IComparable na classe, passar um IComparer para Sort(), ou usar lambda com Sort((a, b) => a.Propriedade.CompareTo(b.Propriedade)). OrderBy() do LINQ retorna uma nova sequência ordenada sem modificar a original. Reverse() inverte a ordem dos elementos. Para ordenação descendente, use Sort() seguido de Reverse(), ou OrderByDescending() do LINQ. Ordenação é O(n log n) - eficiente mesmo para listas grandes. Escolha Sort() para modificar in-place ou OrderBy() para criar nova sequência.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Performance e Boas Práticas",
                    Content = "Listas são eficientes para a maioria dos casos, mas há considerações. Adicionar ao final (Add) é O(1) amortizado - rápido. Inserir no meio (Insert) é O(n) - lento para listas grandes porque precisa deslocar elementos. Remover do meio também é O(n). Acessar por índice é O(1) - muito rápido. Buscar elemento (Contains, IndexOf) é O(n) - percorre a lista. Se você faz muitas buscas, considere HashSet ou Dictionary. Especifique capacidade inicial se souber o tamanho: 'new List<int>(1000)' evita redimensionamentos. RemoveAll() é mais eficiente que múltiplos Remove(). Use LINQ para consultas complexas, mas cuidado com performance em listas muito grandes. Perfil antes de otimizar.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Métodos de Busca",
                    Code = @"List<int> numeros = new List<int> { 5, 12, 8, 21, 3, 15, 7 };

// Find - primeiro elemento > 10
int? primeiro = numeros.Find(x => x > 10);
Console.WriteLine($""Primeiro > 10: {primeiro}"");

// FindAll - todos > 10
List<int> maiores = numeros.FindAll(x => x > 10);
Console.WriteLine($""Maiores que 10: {string.Join("", "", maiores)}"");

// Exists - verifica se existe
bool temPar = numeros.Exists(x => x % 2 == 0);
Console.WriteLine($""Tem número par? {temPar}"");",
                    Language = "csharp",
                    Explanation = "Demonstra Find, FindAll e Exists usando lambda expressions para definir condições de busca.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Ordenação de Listas",
                    Code = @"List<string> nomes = new List<string> { ""Carlos"", ""Ana"", ""Bruno"", ""Diana"" };

// Ordenar alfabeticamente
nomes.Sort();
Console.WriteLine(""Ordenado: "" + string.Join("", "", nomes));

// Ordenar por tamanho
nomes.Sort((a, b) => a.Length.CompareTo(b.Length));
Console.WriteLine(""Por tamanho: "" + string.Join("", "", nomes));

// Reverter
nomes.Reverse();
Console.WriteLine(""Invertido: "" + string.Join("", "", nomes));",
                    Language = "csharp",
                    Explanation = "Mostra diferentes formas de ordenar: alfabética, por propriedade customizada e inversão.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Buscar Primeiro Par",
                    Description = "Dada uma lista de números, use Find() para encontrar o primeiro número par.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"List<int> numeros = new List<int> { 1, 3, 5, 8, 9, 12 };
// Use Find para encontrar o primeiro par
",
                    Hints = new List<string> 
                    { 
                        "Use numeros.Find(x => ...)",
                        "Condição: x % 2 == 0",
                        "Resultado pode ser null" 
                    }
                },
                new Exercise
                {
                    Title = "Filtrar e Ordenar",
                    Description = "Dada uma lista de números, crie uma nova lista com apenas números maiores que 5, ordenada em ordem crescente.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"List<int> numeros = new List<int> { 3, 8, 1, 9, 5, 12, 4, 7 };
// Filtre e ordene
",
                    Hints = new List<string> 
                    { 
                        "Use FindAll(x => x > 5)",
                        "Use Sort() no resultado",
                        "Ou use LINQ: Where().OrderBy()" 
                    }
                },
                new Exercise
                {
                    Title = "Ordenar Objetos",
                    Description = "Crie uma lista de pessoas (com Nome e Idade) e ordene por idade.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"class Pessoa
{
    public string Nome { get; set; }
    public int Idade { get; set; }
}

// Crie lista e ordene por idade
",
                    Hints = new List<string> 
                    { 
                        "Use Sort((a, b) => a.Idade.CompareTo(b.Idade))",
                        "Ou use OrderBy(p => p.Idade)",
                        "Crie algumas pessoas para testar" 
                    }
                }
            },
            Summary = "Nesta aula você aprendeu métodos avançados de List como Find, FindAll e Sort, como usar predicados e lambda expressions, e considerações de performance ao trabalhar com listas. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno. Grandes empresas como Microsoft, Google e Amazon aplicam estes conceitos em seus sistemas. Ao aprender estas técnicas, você está adquirindo conhecimento usado por desenvolvedores em todo o mundo. Considere como este conceito poderia ser aplicado em um projeto pessoal. Pensar em aplicações práticas ajuda a solidificar o entendimento e torna o aprendizado mais significativo. Iniciantes frequentemente cometem alguns erros ao aprender este tópico. Conhecer estes erros comuns ajuda a evitá-los e desenvolver código mais robusto desde o início. Um erro comum é não considerar casos extremos ou situações inesperadas. Sempre teste seu código com diferentes tipos de entrada, incluindo valores nulos, vazios ou inválidos. Outro erro frequente é otimizar prematuramente. Primeiro faça o código funcionar corretamente, depois otimize se necessário. Como diz o ditado: 'premature optimization is the root of all evil'. Quando encontrar problemas, use o debugger para entender o que está acontecendo. Coloque breakpoints, inspecione variáveis e execute o código passo a passo. Esta é uma habilidade essencial para todo desenvolvedor. Mensagens de erro podem parecer intimidadoras no início, mas elas são suas aliadas. Leia-as cuidadosamente - elas geralmente indicam exatamente onde e qual é o problema. Se algo não funcionar como esperado, não desanime. Debugging é uma parte normal do desenvolvimento. Até desenvolvedores experientes passam tempo significativo depurando código. Testar seu código é tão importante quanto escrevê-lo. Testes automatizados garantem que seu código funciona corretamente e continuará funcionando quando você fizer mudanças. Comece escrevendo testes simples para os casos mais comuns. Depois adicione testes para casos extremos e situações de erro. Uma boa cobertura de testes dá confiança para refatorar e melhorar o código. Considere a prática de TDD (Test-Driven Development), onde você escreve os testes antes do código. Isso ajuda a pensar claramente sobre o que o código deve fazer. Embora a correção seja sempre a prioridade, é importante estar ciente das implicações de performance de suas escolhas. Diferentes abordagens podem ter custos computacionais muito diferentes. Em muitos casos, a diferença de performance é negligenciável para aplicações pequenas. No entanto, à medida que seus programas crescem, escolhas eficientes se tornam cada vez mais importantes. Aprenda a usar ferramentas de profiling para identificar gargalos de performance. Otimize baseado em dados reais, não em suposições sobre onde o código é lento. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0002-000000000004"),
            CourseId = _courseId,
            Title = "Métodos Avançados de List",
            Duration = "55 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 55,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0002-000000000003" }),
            OrderIndex = 4,
            Content = "",
            StructuredContent = JsonSerializer.Serialize(content),
            Version = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    private Lesson CreateLesson5()
    {
        var content = new LessonContent
        {
            Objectives = new List<string>
            {
                "Compreender o conceito de Dictionary<TKey, TValue>",
                "Adicionar, acessar e remover pares chave-valor",
                "Escolher entre List e Dictionary baseado no caso de uso"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Introdução a Dictionary",
                    Content = "Dictionary<TKey, TValue> armazena pares chave-valor, como um dicionário real onde você procura uma palavra (chave) para encontrar sua definição (valor). Cada chave deve ser única, mas valores podem se repetir. Dicionários são extremamente rápidos para buscar valores por chave - operação O(1) em média. Isso os torna ideais quando você precisa acessar dados por um identificador. Por exemplo, armazenar usuários por ID, produtos por código, configurações por nome. Internamente, Dictionary usa hash table. A chave é transformada em hash que determina onde o valor é armazenado. Isso permite acesso quase instantâneo independente do tamanho do dicionário. Dicionários não mantêm ordem de inserção (use SortedDictionary se precisar).",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Operações Básicas",
                    Content = "Para criar um dicionário, especifique tipos de chave e valor: 'new Dictionary<string, int>()'. Add(chave, valor) adiciona um par. Se a chave já existe, lança exceção. Use indexador para adicionar ou atualizar: 'dict[chave] = valor' adiciona se não existe, atualiza se existe. Acesse valores por chave: 'dict[chave]' lança exceção se chave não existe. TryGetValue(chave, out valor) é mais seguro - retorna bool indicando sucesso. Remove(chave) remove o par. ContainsKey(chave) verifica se chave existe. ContainsValue(valor) verifica se valor existe (mais lento). Keys retorna coleção de chaves. Values retorna coleção de valores. Count retorna número de pares.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Quando Usar Dictionary vs List",
                    Content = "Use List quando: precisa manter ordem, acessa elementos por posição, itera frequentemente sobre todos os elementos, não precisa buscar por identificador. Use Dictionary quando: precisa buscar elementos por chave rapidamente, cada elemento tem identificador único, ordem não importa, faz muitas buscas. Dictionary é muito mais rápido para buscas - O(1) vs O(n) da List. Mas Dictionary usa mais memória e é mais lento para iterar sobre todos os elementos. Se você se pega fazendo 'lista.Find(x => x.Id == id)' frequentemente, provavelmente deveria usar Dictionary<int, Item>. Combine ambos quando apropriado: Dictionary para acesso rápido, List para manter ordem.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Criando e Usando Dictionary",
                    Code = @"// Dicionário de idades por nome
Dictionary<string, int> idades = new Dictionary<string, int>();

// Adicionar pares
idades.Add(""Ana"", 25);
idades.Add(""Bruno"", 30);
idades[""Carlos""] = 28;  // Forma alternativa

// Acessar valor
Console.WriteLine($""Idade de Ana: {idades[""Ana""]}"");

// Verificar se existe
if (idades.ContainsKey(""Diana""))
{
    Console.WriteLine($""Diana tem {idades[""Diana""]} anos"");
}
else
{
    Console.WriteLine(""Diana não encontrada"");
}",
                    Language = "csharp",
                    Explanation = "Demonstra criação, adição e acesso de elementos em Dictionary. Mostra ContainsKey para verificação segura.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "TryGetValue - Acesso Seguro",
                    Code = @"Dictionary<string, double> precos = new Dictionary<string, double>
{
    { ""Maçã"", 3.50 },
    { ""Banana"", 2.00 },
    { ""Laranja"", 4.00 }
};

// TryGetValue - não lança exceção
if (precos.TryGetValue(""Maçã"", out double preco))
{
    Console.WriteLine($""Maçã custa R$ {preco}"");
}

if (precos.TryGetValue(""Uva"", out double precoUva))
{
    Console.WriteLine($""Uva custa R$ {precoUva}"");
}
else
{
    Console.WriteLine(""Uva não encontrada"");
}",
                    Language = "csharp",
                    Explanation = "TryGetValue é a forma mais segura de acessar valores - retorna bool e não lança exceção se chave não existe.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Criar Dicionário de Contatos",
                    Description = "Crie um Dictionary que mapeia nomes para números de telefone. Adicione 3 contatos e exiba o telefone de um deles.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"// Crie o dicionário de contatos
",
                    Hints = new List<string> 
                    { 
                        "Use Dictionary<string, string>",
                        "Add(\"Nome\", \"Telefone\")",
                        "Acesse com contatos[\"Nome\"]" 
                    }
                },
                new Exercise
                {
                    Title = "Contar Palavras",
                    Description = "Dada uma lista de palavras, crie um Dictionary que conta quantas vezes cada palavra aparece.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"List<string> palavras = new List<string> { ""casa"", ""carro"", ""casa"", ""moto"", ""carro"", ""casa"" };
Dictionary<string, int> contagem = new Dictionary<string, int>();
// Conte as palavras
",
                    Hints = new List<string> 
                    { 
                        "Itere sobre as palavras",
                        "Se palavra existe, incremente contador",
                        "Se não existe, adicione com valor 1" 
                    }
                },
                new Exercise
                {
                    Title = "Busca Segura",
                    Description = "Crie uma função que busca um produto por código em um Dictionary e retorna o preço, ou -1 se não encontrar.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"double BuscarPreco(Dictionary<string, double> produtos, string codigo)
{
    // Implemente usando TryGetValue
}",
                    Hints = new List<string> 
                    { 
                        "Use TryGetValue",
                        "Retorne o valor se encontrar",
                        "Retorne -1 se não encontrar" 
                    }
                }
            },
            Summary = "Nesta aula você aprendeu sobre Dictionary<TKey, TValue>, uma estrutura de dados essencial para mapear chaves a valores com acesso O(1). Você viu quando usar Dictionary vs List e como usar TryGetValue para acesso seguro. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno. Grandes empresas como Microsoft, Google e Amazon aplicam estes conceitos em seus sistemas. Ao aprender estas técnicas, você está adquirindo conhecimento usado por desenvolvedores em todo o mundo. Considere como este conceito poderia ser aplicado em um projeto pessoal. Pensar em aplicações práticas ajuda a solidificar o entendimento e torna o aprendizado mais significativo. Iniciantes frequentemente cometem alguns erros ao aprender este tópico. Conhecer estes erros comuns ajuda a evitá-los e desenvolver código mais robusto desde o início. Um erro comum é não considerar casos extremos ou situações inesperadas. Sempre teste seu código com diferentes tipos de entrada, incluindo valores nulos, vazios ou inválidos. Outro erro frequente é otimizar prematuramente. Primeiro faça o código funcionar corretamente, depois otimize se necessário. Como diz o ditado: 'premature optimization is the root of all evil'. Quando encontrar problemas, use o debugger para entender o que está acontecendo. Coloque breakpoints, inspecione variáveis e execute o código passo a passo. Esta é uma habilidade essencial para todo desenvolvedor. Mensagens de erro podem parecer intimidadoras no início, mas elas são suas aliadas. Leia-as cuidadosamente - elas geralmente indicam exatamente onde e qual é o problema. Se algo não funcionar como esperado, não desanime. Debugging é uma parte normal do desenvolvimento. Até desenvolvedores experientes passam tempo significativo depurando código. Testar seu código é tão importante quanto escrevê-lo. Testes automatizados garantem que seu código funciona corretamente e continuará funcionando quando você fizer mudanças. Comece escrevendo testes simples para os casos mais comuns. Depois adicione testes para casos extremos e situações de erro. Uma boa cobertura de testes dá confiança para refatorar e melhorar o código. Considere a prática de TDD (Test-Driven Development), onde você escreve os testes antes do código. Isso ajuda a pensar claramente sobre o que o código deve fazer. Embora a correção seja sempre a prioridade, é importante estar ciente das implicações de performance de suas escolhas. Diferentes abordagens podem ter custos computacionais muito diferentes. Em muitos casos, a diferença de performance é negligenciável para aplicações pequenas. No entanto, à medida que seus programas crescem, escolhas eficientes se tornam cada vez mais importantes. Aprenda a usar ferramentas de profiling para identificar gargalos de performance. Otimize baseado em dados reais, não em suposições sobre onde o código é lento. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0002-000000000005"),
            CourseId = _courseId,
            Title = "Introdução a Dictionary",
            Duration = "50 min",
            Difficulty = "Iniciante",
            EstimatedMinutes = 50,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0002-000000000004" }),
            OrderIndex = 5,
            Content = "",
            StructuredContent = JsonSerializer.Serialize(content),
            Version = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    private Lesson CreateLesson6()
    {
        var content = new LessonContent
        {
            Objectives = new List<string>
            {
                "Iterar sobre Dictionary usando foreach",
                "Trabalhar com Keys e Values collections",
                "Aplicar Dictionary em cenários práticos"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Iterando sobre Dictionary",
                    Content = "Há várias formas de iterar sobre um Dictionary. foreach sobre o dicionário itera sobre KeyValuePair<TKey, TValue> - cada item tem propriedades Key e Value. Você pode iterar apenas sobre Keys: 'foreach (var chave in dict.Keys)' e então acessar dict[chave]. Ou iterar sobre Values: 'foreach (var valor in dict.Values)' quando não precisa das chaves. A ordem de iteração não é garantida em Dictionary normal - se precisar de ordem, use SortedDictionary ou OrderedDictionary. Iterar sobre Dictionary é mais lento que sobre List porque precisa processar a estrutura de hash. Se você itera frequentemente, considere se Dictionary é a estrutura certa.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Modificando Durante Iteração",
                    Content = "Você não pode adicionar ou remover elementos de um Dictionary enquanto itera sobre ele - isso lança InvalidOperationException. Se precisa modificar, há duas abordagens: criar uma lista de chaves para remover e depois remover fora do loop, ou usar ToList() para criar uma cópia da coleção e iterar sobre a cópia. Você pode modificar valores durante iteração se usar o indexador. RemoveAll não existe em Dictionary como em List - você precisa coletar chaves e remover manualmente. Essa restrição existe porque modificar a estrutura durante iteração invalidaria o iterador. É uma proteção contra bugs sutis.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Casos de Uso Práticos",
                    Content = "Dictionary é ideal para muitos cenários: cache de dados (chave = ID, valor = objeto), configurações (chave = nome, valor = valor), contadores (chave = item, valor = contagem), índices (chave = propriedade, valor = lista de objetos), mapeamentos (chave = código antigo, valor = código novo). Em aplicações web, Dictionary é usado para query strings, headers HTTP, session data. Em jogos, para inventário de itens, estatísticas de jogadores. Em análise de dados, para agregações e agrupamentos. Dictionary é uma das estruturas mais versáteis e usadas em C#. Dominar Dictionary é essencial para programação eficiente.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Iterando sobre Dictionary",
                    Code = @"Dictionary<string, int> estoque = new Dictionary<string, int>
{
    { ""Notebook"", 15 },
    { ""Mouse"", 50 },
    { ""Teclado"", 30 }
};

// Iterar sobre pares chave-valor
Console.WriteLine(""Estoque completo:"");
foreach (KeyValuePair<string, int> item in estoque)
{
    Console.WriteLine($""{item.Key}: {item.Value} unidades"");
}

// Iterar apenas sobre chaves
Console.WriteLine(""\nProdutos:"");
foreach (string produto in estoque.Keys)
{
    Console.WriteLine(produto);
}",
                    Language = "csharp",
                    Explanation = "Demonstra duas formas de iterar: sobre KeyValuePair (acesso a chave e valor) e sobre Keys (apenas chaves).",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Aplicação Prática - Cache",
                    Code = @"// Cache simples de usuários
Dictionary<int, string> cacheUsuarios = new Dictionary<int, string>();

string ObterUsuario(int id)
{
    // Verifica se está no cache
    if (cacheUsuarios.TryGetValue(id, out string nome))
    {
        Console.WriteLine(""Cache hit!"");
        return nome;
    }
    
    // Simula busca no banco de dados
    Console.WriteLine(""Cache miss - buscando no BD..."");
    string nomeUsuario = $""Usuario{id}"";
    
    // Adiciona ao cache
    cacheUsuarios[id] = nomeUsuario;
    return nomeUsuario;
}

// Primeira chamada - busca no BD
Console.WriteLine(ObterUsuario(1));
// Segunda chamada - usa cache
Console.WriteLine(ObterUsuario(1));",
                    Language = "csharp",
                    Explanation = "Exemplo prático de Dictionary como cache - primeira busca é lenta, buscas subsequentes são instantâneas.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exibir Todos os Pares",
                    Description = "Dado um Dictionary de produtos e preços, exiba todos os produtos com seus preços formatados.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"Dictionary<string, double> precos = new Dictionary<string, double>
{
    { ""Arroz"", 25.90 },
    { ""Feijão"", 8.50 },
    { ""Açúcar"", 4.20 }
};
// Exiba todos os produtos e preços
",
                    Hints = new List<string> 
                    { 
                        "Use foreach com KeyValuePair",
                        "Formate preço com :C ou :F2",
                        "item.Key e item.Value" 
                    }
                },
                new Exercise
                {
                    Title = "Filtrar por Valor",
                    Description = "Dado um Dictionary de produtos e quantidades, crie uma lista com nomes dos produtos que têm quantidade menor que 10.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"Dictionary<string, int> estoque = new Dictionary<string, int>
{
    { ""Produto A"", 5 },
    { ""Produto B"", 15 },
    { ""Produto C"", 8 }
};
// Crie lista de produtos com estoque baixo
",
                    Hints = new List<string> 
                    { 
                        "Crie List<string> para resultado",
                        "Itere sobre o Dictionary",
                        "Adicione chave se valor < 10" 
                    }
                },
                new Exercise
                {
                    Title = "Inverter Dictionary",
                    Description = "Crie uma função que inverte um Dictionary (chaves viram valores, valores viram chaves). Assuma que valores são únicos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"Dictionary<string, int> InverterDictionary(Dictionary<int, string> original)
{
    // Implemente aqui
}",
                    Hints = new List<string> 
                    { 
                        "Crie novo Dictionary<string, int>",
                        "Itere sobre original",
                        "Adicione (item.Value, item.Key)" 
                    }
                }
            },
            Summary = "Nesta aula você aprendeu a iterar sobre Dictionary de várias formas, entendeu as restrições de modificação durante iteração e viu casos de uso práticos como cache e contadores. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno. Grandes empresas como Microsoft, Google e Amazon aplicam estes conceitos em seus sistemas. Ao aprender estas técnicas, você está adquirindo conhecimento usado por desenvolvedores em todo o mundo. Considere como este conceito poderia ser aplicado em um projeto pessoal. Pensar em aplicações práticas ajuda a solidificar o entendimento e torna o aprendizado mais significativo. Iniciantes frequentemente cometem alguns erros ao aprender este tópico. Conhecer estes erros comuns ajuda a evitá-los e desenvolver código mais robusto desde o início. Um erro comum é não considerar casos extremos ou situações inesperadas. Sempre teste seu código com diferentes tipos de entrada, incluindo valores nulos, vazios ou inválidos. Outro erro frequente é otimizar prematuramente. Primeiro faça o código funcionar corretamente, depois otimize se necessário. Como diz o ditado: 'premature optimization is the root of all evil'. Quando encontrar problemas, use o debugger para entender o que está acontecendo. Coloque breakpoints, inspecione variáveis e execute o código passo a passo. Esta é uma habilidade essencial para todo desenvolvedor. Mensagens de erro podem parecer intimidadoras no início, mas elas são suas aliadas. Leia-as cuidadosamente - elas geralmente indicam exatamente onde e qual é o problema. Se algo não funcionar como esperado, não desanime. Debugging é uma parte normal do desenvolvimento. Até desenvolvedores experientes passam tempo significativo depurando código. Testar seu código é tão importante quanto escrevê-lo. Testes automatizados garantem que seu código funciona corretamente e continuará funcionando quando você fizer mudanças. Comece escrevendo testes simples para os casos mais comuns. Depois adicione testes para casos extremos e situações de erro. Uma boa cobertura de testes dá confiança para refatorar e melhorar o código. Considere a prática de TDD (Test-Driven Development), onde você escreve os testes antes do código. Isso ajuda a pensar claramente sobre o que o código deve fazer. Embora a correção seja sempre a prioridade, é importante estar ciente das implicações de performance de suas escolhas. Diferentes abordagens podem ter custos computacionais muito diferentes. Em muitos casos, a diferença de performance é negligenciável para aplicações pequenas. No entanto, à medida que seus programas crescem, escolhas eficientes se tornam cada vez mais importantes. Aprenda a usar ferramentas de profiling para identificar gargalos de performance. Otimize baseado em dados reais, não em suposições sobre onde o código é lento. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0002-000000000006"),
            CourseId = _courseId,
            Title = "Trabalhando com Dictionary",
            Duration = "50 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 50,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0002-000000000005" }),
            OrderIndex = 6,
            Content = "",
            StructuredContent = JsonSerializer.Serialize(content),
            Version = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }
}
