using Shared.Entities;
using Shared.Models;
using System.Text.Json;

namespace Shared.Data;

/// <summary>
/// Seeds Level 0 content: Programming Fundamentals (20 lessons)
/// Covers variables, data types, operators, control flow, functions, and basic debugging
/// </summary>
public partial class Level0ContentSeeder
{
    private readonly Guid _courseId = Guid.Parse("10000000-0000-0000-0000-000000000001");
    private readonly Guid _levelId = Guid.Parse("00000000-0000-0000-0000-000000000000");

    public Course CreateLevel0Course()
    {
        return new Course
        {
            Id = _courseId,
            LevelId = _levelId,
            Title = "Fundamentos de Programação",
            Description = "Aprenda os conceitos fundamentais de programação incluindo variáveis, tipos de dados, operadores, controle de fluxo, funções e depuração básica. Perfeito para iniciantes absolutos sem experiência prévia em programação.",
            Level = Level.Beginner,
            Duration = "4 semanas",
            LessonCount = 20,
            Topics = JsonSerializer.Serialize(new[] 
            { 
                "Variáveis", 
                "Tipos de Dados", 
                "Operadores", 
                "Controle de Fluxo", 
                "Funções", 
                "Depuração" 
            }),
            OrderIndex = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    public List<Lesson> CreateLevel0Lessons()
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
                "Entender o que é programação e por que ela é importante",
                "Conhecer os conceitos básicos de algoritmos",
                "Escrever e executar seu primeiro programa em C#"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "O que é Programação?",
                    Content = "Programação é o processo de criar instruções que um computador pode seguir para realizar tarefas específicas. Assim como você segue uma receita para cozinhar, um computador segue um programa para executar operações. A programação permite que você automatize tarefas, resolva problemas complexos e crie aplicações úteis. Programadores escrevem código usando linguagens de programação, que são formas estruturadas de comunicar instruções ao computador. C# é uma dessas linguagens, conhecida por sua versatilidade e poder. Neste curso, você aprenderá a pensar como um programador e a expressar suas ideias através de código. A programação não é apenas sobre escrever código, mas sobre resolver problemas de forma lógica e criativa. Você desenvolverá habilidades de pensamento crítico que são valiosas em muitas áreas da vida.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Algoritmos: A Base da Programação",
                    Content = "Um algoritmo é uma sequência de passos bem definidos para resolver um problema ou realizar uma tarefa. Pense em um algoritmo como uma receita: primeiro você faz isso, depois aquilo, e assim por diante. Por exemplo, o algoritmo para fazer um sanduíche seria: pegar duas fatias de pão, adicionar ingredientes, juntar as fatias. Na programação, algoritmos são fundamentais porque descrevem exatamente o que o programa deve fazer. Um bom algoritmo é claro, eficiente e produz o resultado correto. Antes de escrever código, programadores frequentemente planejam seus algoritmos no papel ou em pseudocódigo. Isso ajuda a organizar o pensamento e identificar problemas antes de começar a codificar. À medida que você avança neste curso, aprenderá a criar algoritmos cada vez mais sofisticados para resolver problemas do mundo real.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Seu Primeiro Programa",
                    Content = "Vamos escrever nosso primeiro programa em C#! O programa tradicional para iniciantes é o 'Olá, Mundo', que simplesmente exibe uma mensagem na tela. Embora seja simples, este programa demonstra conceitos importantes: como escrever código, como executá-lo e como ver os resultados. Em C#, usamos Console.WriteLine() para exibir texto no console (a janela de texto onde o programa é executado). O texto que queremos exibir vai entre aspas duplas. Quando você executa o programa, o computador lê o código, interpreta as instruções e realiza a ação especificada. Este é o ciclo básico da programação: escrever código, executar e ver os resultados. Não se preocupe se não entender tudo agora, cada conceito será explicado em detalhes nas próximas aulas. O importante é dar o primeiro passo e ver seu código funcionando!",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Primeiro Programa - Olá Mundo",
                    Code = "Console.WriteLine(\"Olá, Mundo!\");",
                    Language = "csharp",
                    Explanation = "Este código exibe a mensagem 'Olá, Mundo!' no console. Console.WriteLine() é um comando que imprime texto seguido de uma nova linha.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exibindo Múltiplas Mensagens",
                    Code = @"Console.WriteLine(""Bem-vindo ao C#!"");
Console.WriteLine(""Vamos aprender programação!"");
Console.WriteLine(""Este é o começo da sua jornada."");",
                    Language = "csharp",
                    Explanation = "Você pode usar Console.WriteLine() várias vezes para exibir múltiplas linhas de texto. Cada comando é executado em sequência, de cima para baixo.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Modifique a Mensagem",
                    Description = "Altere o programa para exibir seu nome em vez de 'Mundo'. Por exemplo, se seu nome é João, o programa deve exibir 'Olá, João!'.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = "Console.WriteLine(\"Olá, Mundo!\");",
                    Hints = new List<string> 
                    { 
                        "Substitua a palavra 'Mundo' pelo seu nome",
                        "Mantenha as aspas duplas ao redor do texto" 
                    }
                },
                new Exercise
                {
                    Title = "Crie Sua Própria Mensagem",
                    Description = "Escreva um programa que exiba três linhas de texto sobre você: seu nome, sua cidade e seu hobby favorito.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"// Escreva três linhas usando Console.WriteLine()
",
                    Hints = new List<string> 
                    { 
                        "Use Console.WriteLine() três vezes",
                        "Cada linha deve ter seu próprio comando",
                        "Lembre-se das aspas duplas ao redor do texto" 
                    }
                },
                new Exercise
                {
                    Title = "Mensagem de Boas-Vindas",
                    Description = "Crie um programa que exiba uma mensagem de boas-vindas criativa para novos usuários de um aplicativo.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Crie uma mensagem de boas-vindas com pelo menos 4 linhas
",
                    Hints = new List<string> 
                    { 
                        "Pense em uma mensagem amigável e acolhedora",
                        "Use múltiplos Console.WriteLine() para criar a mensagem",
                        "Seja criativo com o texto!" 
                    }
                }
            },
            Summary = "Nesta aula você aprendeu o que é programação, entendeu o conceito de algoritmos e escreveu seu primeiro programa em C#. Você viu como usar Console.WriteLine() para exibir mensagens no console. Este é o primeiro passo em sua jornada de programação! Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno. Grandes empresas como Microsoft, Google e Amazon aplicam estes conceitos em seus sistemas. Ao aprender estas técnicas, você está adquirindo conhecimento usado por desenvolvedores em todo o mundo. Considere como este conceito poderia ser aplicado em um projeto pessoal. Pensar em aplicações práticas ajuda a solidificar o entendimento e torna o aprendizado mais significativo. Iniciantes frequentemente cometem alguns erros ao aprender este tópico. Conhecer estes erros comuns ajuda a evitá-los e desenvolver código mais robusto desde o início. Um erro comum é não considerar casos extremos ou situações inesperadas. Sempre teste seu código com diferentes tipos de entrada, incluindo valores nulos, vazios ou inválidos. Outro erro frequente é otimizar prematuramente. Primeiro faça o código funcionar corretamente, depois otimize se necessário. Como diz o ditado: 'premature optimization is the root of all evil'. Quando encontrar problemas, use o debugger para entender o que está acontecendo. Coloque breakpoints, inspecione variáveis e execute o código passo a passo. Esta é uma habilidade essencial para todo desenvolvedor. Mensagens de erro podem parecer intimidadoras no início, mas elas são suas aliadas. Leia-as cuidadosamente - elas geralmente indicam exatamente onde e qual é o problema. Se algo não funcionar como esperado, não desanime. Debugging é uma parte normal do desenvolvimento. Até desenvolvedores experientes passam tempo significativo depurando código. Testar seu código é tão importante quanto escrevê-lo. Testes automatizados garantem que seu código funciona corretamente e continuará funcionando quando você fizer mudanças. Comece escrevendo testes simples para os casos mais comuns. Depois adicione testes para casos extremos e situações de erro. Uma boa cobertura de testes dá confiança para refatorar e melhorar o código. Considere a prática de TDD (Test-Driven Development), onde você escreve os testes antes do código. Isso ajuda a pensar claramente sobre o que o código deve fazer. Embora a correção seja sempre a prioridade, é importante estar ciente das implicações de performance de suas escolhas. Diferentes abordagens podem ter custos computacionais muito diferentes. Em muitos casos, a diferença de performance é negligenciável para aplicações pequenas. No entanto, à medida que seus programas crescem, escolhas eficientes se tornam cada vez mais importantes. Aprenda a usar ferramentas de profiling para identificar gargalos de performance. Otimize baseado em dados reais, não em suposições sobre onde o código é lento. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0001-000000000001"),
            CourseId = _courseId,
            Title = "Introdução à Programação",
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
                "Entender o conceito de variáveis e por que elas são necessárias",
                "Aprender a declarar e inicializar variáveis em C#",
                "Usar variáveis para armazenar e manipular dados"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "O que são Variáveis?",
                    Content = "Variáveis são como caixas na memória do computador onde você pode armazenar informações. Imagine que você tem várias caixas etiquetadas: uma para guardar números, outra para guardar texto, e assim por diante. Em programação, variáveis funcionam da mesma forma. Cada variável tem um nome (a etiqueta) e um valor (o conteúdo da caixa). Por exemplo, você pode ter uma variável chamada 'idade' que armazena o número 25, ou uma variável chamada 'nome' que armazena o texto 'Maria'. Variáveis são fundamentais porque permitem que seu programa trabalhe com dados que podem mudar. Sem variáveis, você só poderia trabalhar com valores fixos, o que limitaria muito o que seu programa pode fazer. O nome 'variável' vem do fato de que o valor pode variar durante a execução do programa.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Declarando Variáveis em C#",
                    Content = "Para usar uma variável em C#, você precisa primeiro declará-la. Declarar uma variável significa dizer ao computador: 'Ei, vou precisar de uma caixa para guardar este tipo de informação'. A sintaxe básica é: tipo nome = valor. Por exemplo, 'int idade = 25' declara uma variável do tipo inteiro chamada 'idade' com o valor 25. O tipo especifica que tipo de dado a variável pode armazenar. 'int' é para números inteiros, 'string' é para texto, 'double' é para números com casas decimais. Você também pode declarar uma variável sem atribuir um valor imediatamente: 'int idade;' e depois atribuir o valor mais tarde: 'idade = 25;'. É uma boa prática dar nomes descritivos às suas variáveis para que o código seja fácil de entender. Use nomes como 'precoTotal' em vez de 'x' ou 'temp'.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Trabalhando com Variáveis",
                    Content = "Depois de declarar uma variável, você pode usá-la em seu programa. Você pode ler o valor da variável, modificá-lo ou usá-lo em cálculos. Por exemplo, se você tem uma variável 'preco' com valor 100, pode criar outra variável 'precoComDesconto' que é 'preco * 0.9' (aplicando 10% de desconto). Você também pode atualizar o valor de uma variável: 'idade = idade + 1' aumenta a idade em 1. Uma característica importante das variáveis é que elas mantêm apenas o último valor atribuído. Se você atribui 25 a 'idade' e depois atribui 26, o valor 25 é perdido. Variáveis existem apenas enquanto o programa está rodando; quando o programa termina, os valores são perdidos. Por isso, se você precisa guardar dados permanentemente, precisa salvá-los em um arquivo ou banco de dados.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Declarando e Usando Variáveis",
                    Code = @"string nome = ""Carlos"";
int idade = 30;
Console.WriteLine(""Nome: "" + nome);
Console.WriteLine(""Idade: "" + idade);",
                    Language = "csharp",
                    Explanation = "Este código declara duas variáveis (nome e idade) e exibe seus valores. O operador + concatena (junta) texto com os valores das variáveis.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Modificando Valores de Variáveis",
                    Code = @"int pontos = 100;
Console.WriteLine(""Pontos iniciais: "" + pontos);
pontos = pontos + 50;
Console.WriteLine(""Pontos após ganhar mais: "" + pontos);",
                    Language = "csharp",
                    Explanation = "Este exemplo mostra como o valor de uma variável pode mudar. Começamos com 100 pontos e depois adicionamos 50, resultando em 150 pontos.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Crie Suas Variáveis",
                    Description = "Declare variáveis para armazenar seu nome, idade e cidade, depois exiba essas informações usando Console.WriteLine().",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"// Declare as variáveis aqui
string nome = """";
int idade = 0;
string cidade = """";

// Exiba as informações
",
                    Hints = new List<string> 
                    { 
                        "Use string para texto e int para números inteiros",
                        "Atribua seus próprios valores às variáveis",
                        "Use Console.WriteLine() para exibir cada variável" 
                    }
                },
                new Exercise
                {
                    Title = "Calculadora de Idade",
                    Description = "Crie uma variável com o ano atual e outra com seu ano de nascimento. Calcule e exiba sua idade.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"int anoAtual = 2024;
int anoNascimento = 0; // Coloque seu ano de nascimento

// Calcule e exiba a idade
",
                    Hints = new List<string> 
                    { 
                        "A idade é a diferença entre o ano atual e o ano de nascimento",
                        "Use o operador - para subtrair",
                        "Armazene o resultado em uma nova variável" 
                    }
                },
                new Exercise
                {
                    Title = "Atualizando Variáveis",
                    Description = "Crie uma variável 'saldo' com valor inicial 1000. Subtraia 250 (uma compra) e depois adicione 500 (um depósito). Exiba o saldo após cada operação.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"double saldo = 1000.0;
Console.WriteLine(""Saldo inicial: "" + saldo);

// Realize as operações e exiba o saldo após cada uma
",
                    Hints = new List<string> 
                    { 
                        "Use saldo = saldo - 250 para subtrair",
                        "Use saldo = saldo + 500 para adicionar",
                        "Exiba o saldo após cada operação" 
                    }
                }
            },
            Summary = "Nesta aula você aprendeu sobre variáveis, que são fundamentais para armazenar e manipular dados em programação. Você viu como declarar variáveis, atribuir valores e modificá-los. Variáveis são a base para criar programas dinâmicos e úteis. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno. Grandes empresas como Microsoft, Google e Amazon aplicam estes conceitos em seus sistemas. Ao aprender estas técnicas, você está adquirindo conhecimento usado por desenvolvedores em todo o mundo. Considere como este conceito poderia ser aplicado em um projeto pessoal. Pensar em aplicações práticas ajuda a solidificar o entendimento e torna o aprendizado mais significativo. Iniciantes frequentemente cometem alguns erros ao aprender este tópico. Conhecer estes erros comuns ajuda a evitá-los e desenvolver código mais robusto desde o início. Um erro comum é não considerar casos extremos ou situações inesperadas. Sempre teste seu código com diferentes tipos de entrada, incluindo valores nulos, vazios ou inválidos. Outro erro frequente é otimizar prematuramente. Primeiro faça o código funcionar corretamente, depois otimize se necessário. Como diz o ditado: 'premature optimization is the root of all evil'. Quando encontrar problemas, use o debugger para entender o que está acontecendo. Coloque breakpoints, inspecione variáveis e execute o código passo a passo. Esta é uma habilidade essencial para todo desenvolvedor. Mensagens de erro podem parecer intimidadoras no início, mas elas são suas aliadas. Leia-as cuidadosamente - elas geralmente indicam exatamente onde e qual é o problema. Se algo não funcionar como esperado, não desanime. Debugging é uma parte normal do desenvolvimento. Até desenvolvedores experientes passam tempo significativo depurando código. Testar seu código é tão importante quanto escrevê-lo. Testes automatizados garantem que seu código funciona corretamente e continuará funcionando quando você fizer mudanças. Comece escrevendo testes simples para os casos mais comuns. Depois adicione testes para casos extremos e situações de erro. Uma boa cobertura de testes dá confiança para refatorar e melhorar o código. Considere a prática de TDD (Test-Driven Development), onde você escreve os testes antes do código. Isso ajuda a pensar claramente sobre o que o código deve fazer. Embora a correção seja sempre a prioridade, é importante estar ciente das implicações de performance de suas escolhas. Diferentes abordagens podem ter custos computacionais muito diferentes. Em muitos casos, a diferença de performance é negligenciável para aplicações pequenas. No entanto, à medida que seus programas crescem, escolhas eficientes se tornam cada vez mais importantes. Aprenda a usar ferramentas de profiling para identificar gargalos de performance. Otimize baseado em dados reais, não em suposições sobre onde o código é lento. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0001-000000000002"),
            CourseId = _courseId,
            Title = "Variáveis e Armazenamento de Dados",
            Duration = "50 min",
            Difficulty = "Iniciante",
            EstimatedMinutes = 50,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0001-000000000001" }),
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
                "Conhecer os principais tipos de dados em C#",
                "Entender quando usar cada tipo de dado",
                "Aprender sobre conversão entre tipos"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Tipos de Dados Fundamentais",
                    Content = "Em C#, cada variável tem um tipo que define que tipo de dado ela pode armazenar. Os tipos mais comuns são: int (números inteiros como 42, -10, 0), double (números com casas decimais como 3.14, -0.5), string (texto como 'Olá'), bool (verdadeiro ou falso), e char (um único caractere como 'A'). Escolher o tipo certo é importante porque cada tipo ocupa uma quantidade diferente de memória e suporta operações diferentes. Por exemplo, você pode fazer operações matemáticas com int e double, mas não com string. Um int pode armazenar números de aproximadamente -2 bilhões a +2 bilhões. Se você precisa de números maiores, use long. Para números decimais, double é mais preciso que float. O tipo bool é usado para decisões lógicas, armazenando apenas true ou false.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Trabalhando com Diferentes Tipos",
                    Content = "Cada tipo de dado tem suas características e usos específicos. Strings são usadas para texto e são escritas entre aspas duplas. Você pode juntar strings usando o operador +, como 'Olá' + ' ' + 'Mundo'. Números inteiros (int) são usados para contagens, índices e valores sem casas decimais. Números decimais (double) são usados para medidas precisas, cálculos financeiros e científicos. O tipo bool é fundamental para lógica de programação, usado em condições e decisões. Char armazena um único caractere entre aspas simples, como 'A' ou '5'. É importante notar que '5' (char) é diferente de 5 (int). O primeiro é um caractere, o segundo é um número. Entender essas diferenças ajuda a evitar erros e escrever código mais eficiente.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Conversão de Tipos",
                    Content = "Às vezes você precisa converter um tipo de dado em outro. Por exemplo, converter uma string '123' em um número inteiro 123. Em C#, existem conversões implícitas (automáticas) e explícitas (manuais). Uma conversão implícita acontece quando não há risco de perda de dados, como converter int para double. Conversões explícitas são necessárias quando pode haver perda de dados, como converter double para int (as casas decimais são perdidas). Para converter string em int, use int.Parse() ou int.TryParse(). Para converter números em string, use .ToString(). Por exemplo, int numero = int.Parse('123') converte a string em número. É importante tratar possíveis erros de conversão, especialmente quando o dado vem de entrada do usuário, pois nem toda string pode ser convertida em número.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Usando Diferentes Tipos de Dados",
                    Code = @"int quantidade = 10;
double preco = 29.99;
string produto = ""Livro"";
bool emEstoque = true;

Console.WriteLine(""Produto: "" + produto);
Console.WriteLine(""Quantidade: "" + quantidade);
Console.WriteLine(""Preço: R$ "" + preco);
Console.WriteLine(""Em estoque: "" + emEstoque);",
                    Language = "csharp",
                    Explanation = "Este exemplo demonstra o uso de diferentes tipos de dados para representar informações de um produto. Cada tipo é escolhido de acordo com a natureza do dado.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Conversão de Tipos",
                    Code = @"string textoNumero = ""42"";
int numero = int.Parse(textoNumero);
Console.WriteLine(""Número convertido: "" + numero);

double valorDecimal = 3.14159;
int valorInteiro = (int)valorDecimal;
Console.WriteLine(""Valor inteiro: "" + valorInteiro);",
                    Language = "csharp",
                    Explanation = "Este código mostra como converter string em int usando Parse() e como converter double em int usando cast. Note que a conversão de double para int perde as casas decimais.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Declarando Tipos Variados",
                    Description = "Crie variáveis de diferentes tipos para representar informações de uma pessoa: nome (string), idade (int), altura em metros (double), e se é estudante (bool). Exiba todas as informações.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"// Declare as variáveis aqui

// Exiba as informações
",
                    Hints = new List<string> 
                    { 
                        "Use string para nome, int para idade, double para altura, bool para estudante",
                        "Atribua valores apropriados a cada variável",
                        "Use Console.WriteLine() para exibir cada informação" 
                    }
                },
                new Exercise
                {
                    Title = "Calculadora de Média",
                    Description = "Crie três variáveis double para armazenar notas de provas. Calcule a média e exiba o resultado.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"double nota1 = 8.5;
double nota2 = 7.0;
double nota3 = 9.5;

// Calcule a média e exiba
",
                    Hints = new List<string> 
                    { 
                        "A média é a soma das notas dividida por 3",
                        "Use o operador + para somar e / para dividir",
                        "Armazene o resultado em uma variável double" 
                    }
                },
                new Exercise
                {
                    Title = "Conversão de Temperatura",
                    Description = "Crie uma variável string com uma temperatura em Celsius (ex: '25'). Converta para int, depois calcule o equivalente em Fahrenheit (F = C * 9/5 + 32) e exiba.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"string temperaturaCelsiusTexto = ""25"";

// Converta para int e calcule Fahrenheit
",
                    Hints = new List<string> 
                    { 
                        "Use int.Parse() para converter string em int",
                        "A fórmula é: fahrenheit = celsius * 9 / 5 + 32",
                        "Cuidado com a ordem das operações matemáticas" 
                    }
                }
            },
            Summary = "Nesta aula você aprendeu sobre os principais tipos de dados em C#: int, double, string, bool e char. Você entendeu quando usar cada tipo e como converter entre eles. Escolher o tipo correto é fundamental para escrever programas eficientes e sem erros. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno. Grandes empresas como Microsoft, Google e Amazon aplicam estes conceitos em seus sistemas. Ao aprender estas técnicas, você está adquirindo conhecimento usado por desenvolvedores em todo o mundo. Considere como este conceito poderia ser aplicado em um projeto pessoal. Pensar em aplicações práticas ajuda a solidificar o entendimento e torna o aprendizado mais significativo. Iniciantes frequentemente cometem alguns erros ao aprender este tópico. Conhecer estes erros comuns ajuda a evitá-los e desenvolver código mais robusto desde o início. Um erro comum é não considerar casos extremos ou situações inesperadas. Sempre teste seu código com diferentes tipos de entrada, incluindo valores nulos, vazios ou inválidos. Outro erro frequente é otimizar prematuramente. Primeiro faça o código funcionar corretamente, depois otimize se necessário. Como diz o ditado: 'premature optimization is the root of all evil'. Quando encontrar problemas, use o debugger para entender o que está acontecendo. Coloque breakpoints, inspecione variáveis e execute o código passo a passo. Esta é uma habilidade essencial para todo desenvolvedor. Mensagens de erro podem parecer intimidadoras no início, mas elas são suas aliadas. Leia-as cuidadosamente - elas geralmente indicam exatamente onde e qual é o problema. Se algo não funcionar como esperado, não desanime. Debugging é uma parte normal do desenvolvimento. Até desenvolvedores experientes passam tempo significativo depurando código. Testar seu código é tão importante quanto escrevê-lo. Testes automatizados garantem que seu código funciona corretamente e continuará funcionando quando você fizer mudanças. Comece escrevendo testes simples para os casos mais comuns. Depois adicione testes para casos extremos e situações de erro. Uma boa cobertura de testes dá confiança para refatorar e melhorar o código. Considere a prática de TDD (Test-Driven Development), onde você escreve os testes antes do código. Isso ajuda a pensar claramente sobre o que o código deve fazer. Embora a correção seja sempre a prioridade, é importante estar ciente das implicações de performance de suas escolhas. Diferentes abordagens podem ter custos computacionais muito diferentes. Em muitos casos, a diferença de performance é negligenciável para aplicações pequenas. No entanto, à medida que seus programas crescem, escolhas eficientes se tornam cada vez mais importantes. Aprenda a usar ferramentas de profiling para identificar gargalos de performance. Otimize baseado em dados reais, não em suposições sobre onde o código é lento. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0001-000000000003"),
            CourseId = _courseId,
            Title = "Tipos de Dados em C#",
            Duration = "55 min",
            Difficulty = "Iniciante",
            EstimatedMinutes = 55,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0001-000000000002" }),
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
                "Entender e usar operadores aritméticos",
                "Aprender sobre operadores de comparação",
                "Conhecer operadores lógicos e sua aplicação"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Operadores Aritméticos",
                    Content = "Operadores aritméticos permitem realizar cálculos matemáticos em seus programas. Os operadores básicos são: + (adição), - (subtração), * (multiplicação), / (divisão) e % (módulo, que retorna o resto da divisão). Por exemplo, 10 + 5 resulta em 15, 10 - 5 resulta em 5, 10 * 5 resulta em 50, 10 / 5 resulta em 2, e 10 % 3 resulta em 1 (o resto de 10 dividido por 3). É importante entender a precedência dos operadores: multiplicação e divisão são executadas antes de adição e subtração. Use parênteses para controlar a ordem: (10 + 5) * 2 resulta em 30, enquanto 10 + 5 * 2 resulta em 20. O operador módulo é muito útil para verificar se um número é par (numero % 2 == 0) ou para trabalhar com ciclos.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Operadores de Comparação",
                    Content = "Operadores de comparação comparam dois valores e retornam true ou false. Os operadores são: == (igual a), != (diferente de), > (maior que), < (menor que), >= (maior ou igual a), <= (menor ou igual a). Por exemplo, 5 == 5 é true, 5 != 3 é true, 10 > 5 é true, 3 < 2 é false. Esses operadores são fundamentais para tomar decisões em programas. Note que == (dois sinais de igual) é usado para comparação, enquanto = (um sinal de igual) é usado para atribuição. Confundir esses dois é um erro comum. Você pode comparar números, strings e outros tipos. Para strings, == verifica se o conteúdo é idêntico. Comparações são a base das estruturas de controle de fluxo que você aprenderá nas próximas aulas.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Operadores Lógicos",
                    Content = "Operadores lógicos combinam expressões booleanas. Os três operadores principais são: && (E lógico), || (OU lógico) e ! (NÃO lógico). O operador && retorna true apenas se ambas as condições forem verdadeiras. Por exemplo, (idade >= 18 && temCarteira) é true apenas se a idade for 18 ou mais E a pessoa tiver carteira. O operador || retorna true se pelo menos uma condição for verdadeira. Por exemplo, (diaSemana == 'Sábado' || diaSemana == 'Domingo') é true se for sábado OU domingo. O operador ! inverte um valor booleano: !true é false e !false é true. Esses operadores permitem criar condições complexas. Por exemplo, para verificar se um número está entre 10 e 20: (numero >= 10 && numero <= 20). Dominar operadores lógicos é essencial para escrever lógica de programa sofisticada.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Operadores Aritméticos em Ação",
                    Code = @"int a = 10;
int b = 3;

Console.WriteLine(""Soma: "" + (a + b));
Console.WriteLine(""Subtração: "" + (a - b));
Console.WriteLine(""Multiplicação: "" + (a * b));
Console.WriteLine(""Divisão: "" + (a / b));
Console.WriteLine(""Módulo: "" + (a % b));",
                    Language = "csharp",
                    Explanation = "Este código demonstra todos os operadores aritméticos básicos. Note que a divisão de inteiros resulta em um inteiro (10/3 = 3, não 3.33).",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Comparações e Lógica",
                    Code = @"int idade = 20;
bool temCarteira = true;

bool podeCondu zir = (idade >= 18) && temCarteira;
Console.WriteLine(""Pode conduzir: "" + podeConduzir);

bool fimDeSemana = true;
bool feriado = false;
bool diaLivre = fimDeSemana || feriado;
Console.WriteLine(""Dia livre: "" + diaLivre);",
                    Language = "csharp",
                    Explanation = "Este exemplo mostra operadores de comparação e lógicos trabalhando juntos para tomar decisões baseadas em múltiplas condições.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Calculadora Básica",
                    Description = "Crie duas variáveis numéricas e exiba o resultado de todas as operações aritméticas básicas entre elas.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"int numero1 = 15;
int numero2 = 4;

// Realize e exiba todas as operações
",
                    Hints = new List<string> 
                    { 
                        "Use +, -, *, /, % para as operações",
                        "Exiba cada resultado com uma mensagem descritiva",
                        "Use parênteses para garantir a ordem correta" 
                    }
                },
                new Exercise
                {
                    Title = "Verificador de Elegibilidade",
                    Description = "Crie variáveis para idade e renda mensal. Verifique se a pessoa é elegível para um empréstimo (idade >= 21 E renda >= 2000). Exiba o resultado.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"int idade = 25;
double renda = 2500.0;

// Verifique a elegibilidade e exiba
",
                    Hints = new List<string> 
                    { 
                        "Use o operador && para combinar as condições",
                        "Use >= para as comparações",
                        "Armazene o resultado em uma variável bool" 
                    }
                },
                new Exercise
                {
                    Title = "Verificador de Número Par ou Ímpar",
                    Description = "Crie uma variável com um número inteiro. Use o operador módulo para verificar se é par (resto da divisão por 2 é 0). Exiba o resultado.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"int numero = 17;

// Verifique se é par e exiba
",
                    Hints = new List<string> 
                    { 
                        "Use o operador % para obter o resto da divisão",
                        "Um número é par se numero % 2 == 0",
                        "Use == para comparar o resultado com 0" 
                    }
                }
            },
            Summary = "Nesta aula você aprendeu sobre operadores em C#: aritméticos para cálculos, comparação para verificar relações entre valores, e lógicos para combinar condições. Esses operadores são ferramentas essenciais para criar lógica de programa. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno. Grandes empresas como Microsoft, Google e Amazon aplicam estes conceitos em seus sistemas. Ao aprender estas técnicas, você está adquirindo conhecimento usado por desenvolvedores em todo o mundo. Considere como este conceito poderia ser aplicado em um projeto pessoal. Pensar em aplicações práticas ajuda a solidificar o entendimento e torna o aprendizado mais significativo. Iniciantes frequentemente cometem alguns erros ao aprender este tópico. Conhecer estes erros comuns ajuda a evitá-los e desenvolver código mais robusto desde o início. Um erro comum é não considerar casos extremos ou situações inesperadas. Sempre teste seu código com diferentes tipos de entrada, incluindo valores nulos, vazios ou inválidos. Outro erro frequente é otimizar prematuramente. Primeiro faça o código funcionar corretamente, depois otimize se necessário. Como diz o ditado: 'premature optimization is the root of all evil'. Quando encontrar problemas, use o debugger para entender o que está acontecendo. Coloque breakpoints, inspecione variáveis e execute o código passo a passo. Esta é uma habilidade essencial para todo desenvolvedor. Mensagens de erro podem parecer intimidadoras no início, mas elas são suas aliadas. Leia-as cuidadosamente - elas geralmente indicam exatamente onde e qual é o problema. Se algo não funcionar como esperado, não desanime. Debugging é uma parte normal do desenvolvimento. Até desenvolvedores experientes passam tempo significativo depurando código. Testar seu código é tão importante quanto escrevê-lo. Testes automatizados garantem que seu código funciona corretamente e continuará funcionando quando você fizer mudanças. Comece escrevendo testes simples para os casos mais comuns. Depois adicione testes para casos extremos e situações de erro. Uma boa cobertura de testes dá confiança para refatorar e melhorar o código. Considere a prática de TDD (Test-Driven Development), onde você escreve os testes antes do código. Isso ajuda a pensar claramente sobre o que o código deve fazer. Embora a correção seja sempre a prioridade, é importante estar ciente das implicações de performance de suas escolhas. Diferentes abordagens podem ter custos computacionais muito diferentes. Em muitos casos, a diferença de performance é negligenciável para aplicações pequenas. No entanto, à medida que seus programas crescem, escolhas eficientes se tornam cada vez mais importantes. Aprenda a usar ferramentas de profiling para identificar gargalos de performance. Otimize baseado em dados reais, não em suposições sobre onde o código é lento. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0001-000000000004"),
            CourseId = _courseId,
            Title = "Operadores em C#",
            Duration = "60 min",
            Difficulty = "Iniciante",
            EstimatedMinutes = 60,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0001-000000000003" }),
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
                "Entender o conceito de controle de fluxo",
                "Aprender a usar a estrutura if-else",
                "Criar programas que tomam decisões"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "O que é Controle de Fluxo?",
                    Content = "Até agora, nossos programas executavam instruções em sequência, uma após a outra. Mas programas reais precisam tomar decisões baseadas em condições. Controle de fluxo permite que seu programa execute diferentes códigos dependendo de certas condições. É como um mapa com bifurcações: dependendo da situação, você segue um caminho ou outro. Por exemplo, um programa de caixa eletrônico verifica se você tem saldo suficiente antes de permitir um saque. Se tem saldo, executa o saque; se não tem, exibe uma mensagem de erro. Sem controle de fluxo, todos os programas seriam lineares e limitados. Com ele, você pode criar programas inteligentes que respondem a diferentes situações. As estruturas de controle de fluxo mais comuns são if-else, switch e loops.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "A Estrutura if-else",
                    Content = "A estrutura if-else é a forma mais básica de controle de fluxo. A sintaxe é: if (condição) { código se verdadeiro } else { código se falso }. A condição é uma expressão booleana que resulta em true ou false. Se for true, o código dentro do primeiro bloco é executado. Se for false, o código dentro do bloco else é executado. Por exemplo: if (idade >= 18) { Console.WriteLine('Maior de idade'); } else { Console.WriteLine('Menor de idade'); }. O bloco else é opcional. Se você não precisa fazer nada quando a condição é falsa, pode omiti-lo. Você também pode encadear múltiplas condições usando else if: if (nota >= 9) { 'Excelente' } else if (nota >= 7) { 'Bom' } else { 'Precisa melhorar' }. Isso permite verificar várias condições em sequência.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Boas Práticas com if-else",
                    Content = "Ao usar if-else, é importante escrever condições claras e código bem organizado. Use parênteses para tornar condições complexas mais legíveis. Evite aninhar muitos if-else (if dentro de if dentro de if), pois isso torna o código difícil de entender. Se você tem muitas condições, considere usar switch (que veremos em outra aula). Sempre considere todos os casos possíveis. Se você verifica if (x > 0), pense no que acontece quando x é 0 ou negativo. Use nomes de variáveis descritivos em suas condições para que o código seja auto-explicativo. Por exemplo, if (usuarioAutenticado && temPermissao) é mais claro que if (a && b). Lembre-se que o código é lido muito mais vezes do que é escrito, então priorize clareza sobre brevidade.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "if-else Básico",
                    Code = @"int idade = 20;

if (idade >= 18)
{
    Console.WriteLine(""Você é maior de idade."");
}
else
{
    Console.WriteLine(""Você é menor de idade."");
}",
                    Language = "csharp",
                    Explanation = "Este código verifica se a idade é 18 ou mais. Dependendo do resultado, exibe uma mensagem diferente.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Múltiplas Condições com else if",
                    Code = @"double nota = 8.5;

if (nota >= 9.0)
{
    Console.WriteLine(""Conceito: A - Excelente!"");
}
else if (nota >= 7.0)
{
    Console.WriteLine(""Conceito: B - Bom!"");
}
else if (nota >= 5.0)
{
    Console.WriteLine(""Conceito: C - Regular"");
}
else
{
    Console.WriteLine(""Conceito: D - Insuficiente"");
}",
                    Language = "csharp",
                    Explanation = "Este exemplo usa else if para verificar múltiplas faixas de notas e atribuir conceitos diferentes. As condições são verificadas em ordem.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Verificador de Maioridade",
                    Description = "Crie um programa que verifica se uma pessoa pode votar (idade >= 16 no Brasil). Exiba uma mensagem apropriada.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"int idade = 15;

// Verifique se pode votar
",
                    Hints = new List<string> 
                    { 
                        "Use if para verificar se idade >= 16",
                        "Exiba mensagens diferentes para cada caso",
                        "Não esqueça das chaves { } ao redor do código" 
                    }
                },
                new Exercise
                {
                    Title = "Calculadora de Desconto",
                    Description = "Crie um programa que aplica desconto baseado no valor da compra: 10% se >= 100, 5% se >= 50, sem desconto se < 50. Exiba o valor final.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"double valorCompra = 75.0;

// Calcule o desconto e o valor final
",
                    Hints = new List<string> 
                    { 
                        "Use else if para verificar as faixas de valor",
                        "Calcule o desconto: valor * 0.10 para 10%",
                        "Subtraia o desconto do valor original" 
                    }
                },
                new Exercise
                {
                    Title = "Classificador de Temperatura",
                    Description = "Crie um programa que classifica a temperatura: 'Muito frio' (< 10), 'Frio' (10-20), 'Agradável' (20-30), 'Quente' (> 30).",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"int temperatura = 25;

// Classifique a temperatura
",
                    Hints = new List<string> 
                    { 
                        "Use múltiplos else if para as faixas",
                        "Comece verificando a temperatura mais baixa",
                        "Use && para verificar intervalos: temp >= 10 && temp < 20" 
                    }
                }
            },
            Summary = "Nesta aula você aprendeu sobre controle de fluxo e a estrutura if-else. Agora você pode criar programas que tomam decisões baseadas em condições, tornando seu código muito mais poderoso e flexível. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno. Grandes empresas como Microsoft, Google e Amazon aplicam estes conceitos em seus sistemas. Ao aprender estas técnicas, você está adquirindo conhecimento usado por desenvolvedores em todo o mundo. Considere como este conceito poderia ser aplicado em um projeto pessoal. Pensar em aplicações práticas ajuda a solidificar o entendimento e torna o aprendizado mais significativo. Iniciantes frequentemente cometem alguns erros ao aprender este tópico. Conhecer estes erros comuns ajuda a evitá-los e desenvolver código mais robusto desde o início. Um erro comum é não considerar casos extremos ou situações inesperadas. Sempre teste seu código com diferentes tipos de entrada, incluindo valores nulos, vazios ou inválidos. Outro erro frequente é otimizar prematuramente. Primeiro faça o código funcionar corretamente, depois otimize se necessário. Como diz o ditado: 'premature optimization is the root of all evil'. Quando encontrar problemas, use o debugger para entender o que está acontecendo. Coloque breakpoints, inspecione variáveis e execute o código passo a passo. Esta é uma habilidade essencial para todo desenvolvedor. Mensagens de erro podem parecer intimidadoras no início, mas elas são suas aliadas. Leia-as cuidadosamente - elas geralmente indicam exatamente onde e qual é o problema. Se algo não funcionar como esperado, não desanime. Debugging é uma parte normal do desenvolvimento. Até desenvolvedores experientes passam tempo significativo depurando código. Testar seu código é tão importante quanto escrevê-lo. Testes automatizados garantem que seu código funciona corretamente e continuará funcionando quando você fizer mudanças. Comece escrevendo testes simples para os casos mais comuns. Depois adicione testes para casos extremos e situações de erro. Uma boa cobertura de testes dá confiança para refatorar e melhorar o código. Considere a prática de TDD (Test-Driven Development), onde você escreve os testes antes do código. Isso ajuda a pensar claramente sobre o que o código deve fazer. Embora a correção seja sempre a prioridade, é importante estar ciente das implicações de performance de suas escolhas. Diferentes abordagens podem ter custos computacionais muito diferentes. Em muitos casos, a diferença de performance é negligenciável para aplicações pequenas. No entanto, à medida que seus programas crescem, escolhas eficientes se tornam cada vez mais importantes. Aprenda a usar ferramentas de profiling para identificar gargalos de performance. Otimize baseado em dados reais, não em suposições sobre onde o código é lento. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0001-000000000005"),
            CourseId = _courseId,
            Title = "Estruturas Condicionais: if-else",
            Duration = "55 min",
            Difficulty = "Iniciante",
            EstimatedMinutes = 55,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0001-000000000004" }),
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
                "Entender quando usar a estrutura switch",
                "Aprender a sintaxe do switch-case",
                "Comparar switch com if-else e escolher a melhor opção"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Introdução ao Switch",
                    Content = "A estrutura switch é uma alternativa ao if-else quando você precisa comparar uma variável com múltiplos valores possíveis. É especialmente útil quando você tem muitas condições baseadas no mesmo valor. Por exemplo, um menu com opções 1, 2, 3, 4, 5 fica mais limpo com switch do que com vários if-else. A sintaxe básica é: switch (variável) { case valor1: código; break; case valor2: código; break; default: código; }. O switch avalia a variável uma vez e compara com cada case. Quando encontra uma correspondência, executa o código daquele case. O break é importante para sair do switch após executar um case. Sem break, o código continuaria executando os cases seguintes (fall-through). O default é opcional e executa quando nenhum case corresponde, similar ao else final em if-else.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Quando Usar Switch vs If-Else",
                    Content = "Use switch quando você está comparando uma única variável com múltiplos valores específicos. Por exemplo, dias da semana, opções de menu, códigos de status. Use if-else quando você tem condições complexas, intervalos de valores ou múltiplas variáveis. Por exemplo, if (idade >= 18 && temCarteira) não pode ser feito com switch. Switch é mais legível quando você tem muitas opções discretas. Compare: switch (diaSemana) com 7 cases versus 7 if-else. O switch deixa claro que todas as condições testam a mesma variável. If-else é mais flexível e pode testar qualquer condição booleana. Em termos de performance, switch pode ser ligeiramente mais rápido com muitos cases, mas a diferença é geralmente insignificante. Priorize legibilidade: escolha a estrutura que torna seu código mais claro e fácil de entender.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Recursos Avançados do Switch",
                    Content = "Em C# moderno, switch tem recursos poderosos. Você pode usar múltiplos valores em um case: case 1 or 2 or 3. Pode usar expressões switch mais concisas: var resultado = diaSemana switch { 1 => 'Segunda', 2 => 'Terça', _ => 'Outro' }. O underscore _ funciona como default. Switch também pode trabalhar com strings: switch (comando) { case 'iniciar': ...; case 'parar': ...; }. Você pode agrupar cases que executam o mesmo código: case 'S': case 's': Console.WriteLine('Sim'); break;. Isso é útil para aceitar maiúsculas e minúsculas. Lembre-se sempre do break para evitar fall-through não intencional. Fall-through intencional (sem break) é raro e deve ser bem documentado. O default deve sempre ser considerado, mesmo que seja apenas para logar um erro ou lançar uma exceção.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Switch Básico com Dias da Semana",
                    Code = @"int diaSemana = 3;

switch (diaSemana)
{
    case 1:
        Console.WriteLine(""Segunda-feira"");
        break;
    case 2:
        Console.WriteLine(""Terça-feira"");
        break;
    case 3:
        Console.WriteLine(""Quarta-feira"");
        break;
    case 4:
        Console.WriteLine(""Quinta-feira"");
        break;
    case 5:
        Console.WriteLine(""Sexta-feira"");
        break;
    case 6:
        Console.WriteLine(""Sábado"");
        break;
    case 7:
        Console.WriteLine(""Domingo"");
        break;
    default:
        Console.WriteLine(""Dia inválido"");
        break;
}",
                    Language = "csharp",
                    Explanation = "Este switch converte um número (1-7) no nome do dia da semana correspondente. O default trata valores inválidos.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Switch com Strings",
                    Code = @"string comando = ""iniciar"";

switch (comando)
{
    case ""iniciar"":
        Console.WriteLine(""Sistema iniciando..."");
        break;
    case ""parar"":
        Console.WriteLine(""Sistema parando..."");
        break;
    case ""reiniciar"":
        Console.WriteLine(""Sistema reiniciando..."");
        break;
    default:
        Console.WriteLine(""Comando não reconhecido"");
        break;
}",
                    Language = "csharp",
                    Explanation = "Switch também funciona com strings. Este exemplo processa comandos de texto e executa ações correspondentes.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Menu de Opções",
                    Description = "Crie um switch que processa opções de menu (1-4): 1=Novo, 2=Abrir, 3=Salvar, 4=Sair. Exiba a ação correspondente.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"int opcao = 2;

// Crie o switch para processar a opção
",
                    Hints = new List<string> 
                    { 
                        "Use switch (opcao) e crie um case para cada número",
                        "Não esqueça do break após cada case",
                        "Adicione um default para opções inválidas" 
                    }
                },
                new Exercise
                {
                    Title = "Calculadora com Switch",
                    Description = "Crie uma calculadora que usa switch para processar operações (+, -, *, /). Dados dois números e um operador, calcule e exiba o resultado.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"double num1 = 10;
double num2 = 5;
string operador = ""+"";

// Use switch para processar a operação
",
                    Hints = new List<string> 
                    { 
                        "Use switch (operador) com cases para '+', '-', '*', '/'",
                        "Calcule o resultado dentro de cada case",
                        "Cuidado com divisão por zero no case '/'" 
                    }
                },
                new Exercise
                {
                    Title = "Conversor de Mês",
                    Description = "Crie um switch que converte o número do mês (1-12) no nome do mês em português. Agrupe meses com mesmo número de dias.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"int mes = 3;

// Converta o número em nome do mês
",
                    Hints = new List<string> 
                    { 
                        "Crie um case para cada número de 1 a 12",
                        "Use default para números inválidos",
                        "Você pode adicionar informação sobre quantos dias tem cada mês" 
                    }
                }
            },
            Summary = "Nesta aula você aprendeu sobre a estrutura switch, uma alternativa elegante ao if-else para comparar uma variável com múltiplos valores. Switch torna o código mais limpo quando você tem muitas opções discretas. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno. Grandes empresas como Microsoft, Google e Amazon aplicam estes conceitos em seus sistemas. Ao aprender estas técnicas, você está adquirindo conhecimento usado por desenvolvedores em todo o mundo. Considere como este conceito poderia ser aplicado em um projeto pessoal. Pensar em aplicações práticas ajuda a solidificar o entendimento e torna o aprendizado mais significativo. Iniciantes frequentemente cometem alguns erros ao aprender este tópico. Conhecer estes erros comuns ajuda a evitá-los e desenvolver código mais robusto desde o início. Um erro comum é não considerar casos extremos ou situações inesperadas. Sempre teste seu código com diferentes tipos de entrada, incluindo valores nulos, vazios ou inválidos. Outro erro frequente é otimizar prematuramente. Primeiro faça o código funcionar corretamente, depois otimize se necessário. Como diz o ditado: 'premature optimization is the root of all evil'. Quando encontrar problemas, use o debugger para entender o que está acontecendo. Coloque breakpoints, inspecione variáveis e execute o código passo a passo. Esta é uma habilidade essencial para todo desenvolvedor. Mensagens de erro podem parecer intimidadoras no início, mas elas são suas aliadas. Leia-as cuidadosamente - elas geralmente indicam exatamente onde e qual é o problema. Se algo não funcionar como esperado, não desanime. Debugging é uma parte normal do desenvolvimento. Até desenvolvedores experientes passam tempo significativo depurando código. Testar seu código é tão importante quanto escrevê-lo. Testes automatizados garantem que seu código funciona corretamente e continuará funcionando quando você fizer mudanças. Comece escrevendo testes simples para os casos mais comuns. Depois adicione testes para casos extremos e situações de erro. Uma boa cobertura de testes dá confiança para refatorar e melhorar o código. Considere a prática de TDD (Test-Driven Development), onde você escreve os testes antes do código. Isso ajuda a pensar claramente sobre o que o código deve fazer. Embora a correção seja sempre a prioridade, é importante estar ciente das implicações de performance de suas escolhas. Diferentes abordagens podem ter custos computacionais muito diferentes. Em muitos casos, a diferença de performance é negligenciável para aplicações pequenas. No entanto, à medida que seus programas crescem, escolhas eficientes se tornam cada vez mais importantes. Aprenda a usar ferramentas de profiling para identificar gargalos de performance. Otimize baseado em dados reais, não em suposições sobre onde o código é lento. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0001-000000000006"),
            CourseId = _courseId,
            Title = "Estrutura Switch-Case",
            Duration = "50 min",
            Difficulty = "Iniciante",
            EstimatedMinutes = 50,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0001-000000000005" }),
            OrderIndex = 6,
            Content = "",
            StructuredContent = JsonSerializer.Serialize(content),
            Version = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    private Lesson CreateLesson7()
    {
        var content = new LessonContent
        {
            Objectives = new List<string>
            {
                "Entender o conceito de loops e repetição",
                "Aprender a usar o loop while",
                "Evitar loops infinitos"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Por que Precisamos de Loops?",
                    Content = "Loops (laços de repetição) permitem executar o mesmo código múltiplas vezes sem precisar escrevê-lo repetidamente. Imagine que você precisa exibir números de 1 a 100. Sem loops, você teria que escrever Console.WriteLine() 100 vezes! Com loops, você escreve o código uma vez e ele se repete automaticamente. Loops são fundamentais para processar listas de dados, repetir ações até uma condição ser satisfeita, e automatizar tarefas repetitivas. Existem três tipos principais de loops em C#: while, do-while e for. Cada um tem seus usos específicos. O loop while é o mais básico e executa enquanto uma condição for verdadeira. É como dizer: 'enquanto ainda houver trabalho, continue trabalhando'. Loops tornam programas muito mais poderosos e eficientes.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "O Loop While",
                    Content = "A sintaxe do while é: while (condição) { código a repetir }. O loop verifica a condição antes de cada iteração. Se for true, executa o código e verifica novamente. Se for false, sai do loop. Por exemplo: int contador = 1; while (contador <= 5) { Console.WriteLine(contador); contador++; }. Este código exibe números de 1 a 5. É crucial que a condição eventualmente se torne false, senão você terá um loop infinito. Sempre certifique-se de que algo dentro do loop modifica a variável da condição. No exemplo, contador++ aumenta o contador a cada iteração, garantindo que eventualmente contador será maior que 5 e o loop terminará. While é ideal quando você não sabe quantas vezes o loop precisa executar, apenas a condição de parada.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Cuidados com Loops",
                    Content = "Loops infinitos ocorrem quando a condição nunca se torna false. Por exemplo: while (true) { Console.WriteLine('Infinito'); } nunca para. Isso trava seu programa! Sempre verifique se sua condição pode se tornar false. Use variáveis de controle que são modificadas dentro do loop. Teste seus loops com valores pequenos primeiro. Se você quer um loop de 1 a 1000, teste primeiro com 1 a 10 para garantir que funciona. Você pode usar break para sair de um loop prematuramente: if (condicaoEspecial) break;. Use continue para pular para a próxima iteração: if (devePular) continue;. Loops aninhados (loop dentro de loop) são possíveis mas aumentam a complexidade. Use com cuidado e sempre comente seu código para explicar a lógica.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "While Loop Básico",
                    Code = @"int contador = 1;

while (contador <= 5)
{
    Console.WriteLine(""Contagem: "" + contador);
    contador++;
}

Console.WriteLine(""Loop terminado!"");",
                    Language = "csharp",
                    Explanation = "Este loop conta de 1 a 5. A cada iteração, exibe o contador e o incrementa. Quando contador chega a 6, a condição se torna false e o loop termina.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "While com Condição Complexa",
                    Code = @"int saldo = 100;
int saque = 20;

while (saldo >= saque)
{
    saldo = saldo - saque;
    Console.WriteLine(""Saque realizado. Saldo: "" + saldo);
}

Console.WriteLine(""Saldo insuficiente para novo saque."");",
                    Language = "csharp",
                    Explanation = "Este loop simula saques de uma conta. Continua sacando enquanto houver saldo suficiente. Quando o saldo fica menor que o valor do saque, o loop para.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Contador Regressivo",
                    Description = "Crie um loop while que faz uma contagem regressiva de 10 até 1, exibindo cada número.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"int numero = 10;

// Crie o loop de contagem regressiva
",
                    Hints = new List<string> 
                    { 
                        "Use while (numero >= 1)",
                        "Exiba o número dentro do loop",
                        "Use numero-- para decrementar" 
                    }
                },
                new Exercise
                {
                    Title = "Soma Acumulada",
                    Description = "Use um loop while para somar todos os números de 1 a 10. Exiba o resultado final.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"int numero = 1;
int soma = 0;

// Use while para acumular a soma
",
                    Hints = new List<string> 
                    { 
                        "Dentro do loop, adicione numero à soma",
                        "Incremente numero a cada iteração",
                        "Exiba a soma após o loop terminar" 
                    }
                },
                new Exercise
                {
                    Title = "Validador de Senha",
                    Description = "Simule um sistema que pede senha até 3 tentativas. Use while para repetir enquanto a senha estiver errada e tentativas < 3.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"string senhaCorreta = ""1234"";
string tentativa = ""0000"";
int numeroTentativas = 0;

// Crie o loop de validação
",
                    Hints = new List<string> 
                    { 
                        "Use while (tentativa != senhaCorreta && numeroTentativas < 3)",
                        "Simule novas tentativas mudando o valor de tentativa",
                        "Incremente numeroTentativas a cada iteração" 
                    }
                }
            },
            Summary = "Nesta aula você aprendeu sobre loops e especificamente o loop while. Loops permitem repetir código múltiplas vezes, tornando programas muito mais eficientes. Sempre garanta que seus loops tenham uma condição de parada para evitar loops infinitos. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno. Grandes empresas como Microsoft, Google e Amazon aplicam estes conceitos em seus sistemas. Ao aprender estas técnicas, você está adquirindo conhecimento usado por desenvolvedores em todo o mundo. Considere como este conceito poderia ser aplicado em um projeto pessoal. Pensar em aplicações práticas ajuda a solidificar o entendimento e torna o aprendizado mais significativo. Iniciantes frequentemente cometem alguns erros ao aprender este tópico. Conhecer estes erros comuns ajuda a evitá-los e desenvolver código mais robusto desde o início. Um erro comum é não considerar casos extremos ou situações inesperadas. Sempre teste seu código com diferentes tipos de entrada, incluindo valores nulos, vazios ou inválidos. Outro erro frequente é otimizar prematuramente. Primeiro faça o código funcionar corretamente, depois otimize se necessário. Como diz o ditado: 'premature optimization is the root of all evil'. Quando encontrar problemas, use o debugger para entender o que está acontecendo. Coloque breakpoints, inspecione variáveis e execute o código passo a passo. Esta é uma habilidade essencial para todo desenvolvedor. Mensagens de erro podem parecer intimidadoras no início, mas elas são suas aliadas. Leia-as cuidadosamente - elas geralmente indicam exatamente onde e qual é o problema. Se algo não funcionar como esperado, não desanime. Debugging é uma parte normal do desenvolvimento. Até desenvolvedores experientes passam tempo significativo depurando código. Testar seu código é tão importante quanto escrevê-lo. Testes automatizados garantem que seu código funciona corretamente e continuará funcionando quando você fizer mudanças. Comece escrevendo testes simples para os casos mais comuns. Depois adicione testes para casos extremos e situações de erro. Uma boa cobertura de testes dá confiança para refatorar e melhorar o código. Considere a prática de TDD (Test-Driven Development), onde você escreve os testes antes do código. Isso ajuda a pensar claramente sobre o que o código deve fazer. Embora a correção seja sempre a prioridade, é importante estar ciente das implicações de performance de suas escolhas. Diferentes abordagens podem ter custos computacionais muito diferentes. Em muitos casos, a diferença de performance é negligenciável para aplicações pequenas. No entanto, à medida que seus programas crescem, escolhas eficientes se tornam cada vez mais importantes. Aprenda a usar ferramentas de profiling para identificar gargalos de performance. Otimize baseado em dados reais, não em suposições sobre onde o código é lento. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0001-000000000007"),
            CourseId = _courseId,
            Title = "Loops: While",
            Duration = "55 min",
            Difficulty = "Iniciante",
            EstimatedMinutes = 55,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0001-000000000006" }),
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
                "Entender a diferença entre while e do-while",
                "Aprender quando usar do-while",
                "Dominar loops com garantia de execução mínima"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "O Loop Do-While",
                    Content = "O loop do-while é similar ao while, mas com uma diferença crucial: ele sempre executa pelo menos uma vez. A sintaxe é: do { código } while (condição);. Note que a condição é verificada no final, não no início. Isso significa que o código dentro do do é executado primeiro, e só depois a condição é checada. Se for true, o loop repete; se for false, termina. Por exemplo: do { Console.WriteLine('Executou'); } while (false); exibe 'Executou' uma vez, mesmo que a condição seja false. Compare com while (false) { Console.WriteLine('Nunca executa'); } que não exibe nada. Do-while é útil quando você precisa garantir que o código execute pelo menos uma vez, como em menus interativos ou validação de entrada do usuário.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Quando Usar Do-While",
                    Content = "Use do-while quando a lógica do seu programa requer que o código execute pelo menos uma vez antes de verificar a condição. Casos comuns incluem: menus que devem ser exibidos pelo menos uma vez, validação de entrada onde você pede um valor e depois verifica se é válido, e jogos onde uma rodada deve acontecer antes de verificar se o jogador quer continuar. Por exemplo, um menu: do { exibir opções; ler escolha; processar escolha; } while (escolha != 'sair'). O menu é exibido pelo menos uma vez, e continua sendo exibido até o usuário escolher sair. Se você usasse while, teria que duplicar o código de exibição do menu antes do loop. Do-while elimina essa duplicação e torna o código mais limpo.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Comparando While e Do-While",
                    Content = "A escolha entre while e do-while depende da lógica do seu programa. Use while quando a condição deve ser verificada antes da primeira execução. Use do-while quando o código deve executar pelo menos uma vez. Em termos de performance, não há diferença significativa. A diferença é puramente lógica. Muitas vezes, um problema pode ser resolvido com ambos, mas um deles resultará em código mais claro. Por exemplo, validação de entrada é mais natural com do-while: do { pedir entrada; } while (entrada inválida). Com while, você teria que pedir entrada antes do loop e dentro dele, duplicando código. Sempre escolha a estrutura que torna seu código mais legível e mantém a lógica clara. Código claro é mais importante que código 'inteligente' ou 'compacto'.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Do-While Básico",
                    Code = @"int contador = 1;

do
{
    Console.WriteLine(""Contagem: "" + contador);
    contador++;
} while (contador <= 5);

Console.WriteLine(""Loop terminado!"");",
                    Language = "csharp",
                    Explanation = "Este do-while conta de 1 a 5. Funciona similar ao while, mas a condição é verificada no final de cada iteração.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Menu Interativo com Do-While",
                    Code = @"int opcao;

do
{
    Console.WriteLine(""=== MENU ==="" );
    Console.WriteLine(""1. Nova Partida"");
    Console.WriteLine(""2. Carregar Jogo"");
    Console.WriteLine(""3. Sair"");
    
    opcao = 2; // Simulando escolha do usuário
    
    if (opcao == 1)
        Console.WriteLine(""Iniciando nova partida..."");
    else if (opcao == 2)
        Console.WriteLine(""Carregando jogo..."");
        
} while (opcao != 3);

Console.WriteLine(""Até logo!"");",
                    Language = "csharp",
                    Explanation = "Este exemplo mostra um menu que é exibido pelo menos uma vez e continua até o usuário escolher sair (opção 3). Do-while é perfeito para este caso.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Validador de Número Positivo",
                    Description = "Use do-while para simular a leitura de um número até que seja positivo. Exiba mensagem de erro para números negativos ou zero.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"int numero;

// Use do-while para validar
do
{
    numero = -5; // Simule diferentes valores para testar
    // Adicione lógica de validação
} while (/* condição */);",
                    Hints = new List<string> 
                    { 
                        "A condição deve ser: numero <= 0",
                        "Exiba mensagem de erro dentro do loop",
                        "Teste com valores negativos, zero e positivos" 
                    }
                },
                new Exercise
                {
                    Title = "Jogo de Adivinhação",
                    Description = "Crie um jogo onde o usuário tenta adivinhar um número. Use do-while para repetir até acertar. Dê dicas se o palpite é maior ou menor.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"int numeroSecreto = 7;
int palpite;

// Crie o loop do jogo
",
                    Hints = new List<string> 
                    { 
                        "Use do-while com condição: palpite != numeroSecreto",
                        "Simule palpites diferentes para testar",
                        "Use if-else para dar dicas (maior/menor)" 
                    }
                },
                new Exercise
                {
                    Title = "Calculadora Contínua",
                    Description = "Crie uma calculadora que continua fazendo operações até o usuário escolher sair. Use do-while para o loop principal.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"string continuar;

do
{
    // Simule uma operação matemática
    // Pergunte se quer continuar
    continuar = ""n""; // Mude para testar
} while (/* condição */);",
                    Hints = new List<string> 
                    { 
                        "A condição pode ser: continuar == 's' ou continuar == 'S'",
                        "Realize uma operação dentro do loop",
                        "Simule a resposta do usuário" 
                    }
                }
            },
            Summary = "Nesta aula você aprendeu sobre o loop do-while, que garante pelo menos uma execução do código antes de verificar a condição. É ideal para menus, validações e situações onde o código deve executar antes da verificação. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno. Grandes empresas como Microsoft, Google e Amazon aplicam estes conceitos em seus sistemas. Ao aprender estas técnicas, você está adquirindo conhecimento usado por desenvolvedores em todo o mundo. Considere como este conceito poderia ser aplicado em um projeto pessoal. Pensar em aplicações práticas ajuda a solidificar o entendimento e torna o aprendizado mais significativo. Iniciantes frequentemente cometem alguns erros ao aprender este tópico. Conhecer estes erros comuns ajuda a evitá-los e desenvolver código mais robusto desde o início. Um erro comum é não considerar casos extremos ou situações inesperadas. Sempre teste seu código com diferentes tipos de entrada, incluindo valores nulos, vazios ou inválidos. Outro erro frequente é otimizar prematuramente. Primeiro faça o código funcionar corretamente, depois otimize se necessário. Como diz o ditado: 'premature optimization is the root of all evil'. Quando encontrar problemas, use o debugger para entender o que está acontecendo. Coloque breakpoints, inspecione variáveis e execute o código passo a passo. Esta é uma habilidade essencial para todo desenvolvedor. Mensagens de erro podem parecer intimidadoras no início, mas elas são suas aliadas. Leia-as cuidadosamente - elas geralmente indicam exatamente onde e qual é o problema. Se algo não funcionar como esperado, não desanime. Debugging é uma parte normal do desenvolvimento. Até desenvolvedores experientes passam tempo significativo depurando código. Testar seu código é tão importante quanto escrevê-lo. Testes automatizados garantem que seu código funciona corretamente e continuará funcionando quando você fizer mudanças. Comece escrevendo testes simples para os casos mais comuns. Depois adicione testes para casos extremos e situações de erro. Uma boa cobertura de testes dá confiança para refatorar e melhorar o código. Considere a prática de TDD (Test-Driven Development), onde você escreve os testes antes do código. Isso ajuda a pensar claramente sobre o que o código deve fazer. Embora a correção seja sempre a prioridade, é importante estar ciente das implicações de performance de suas escolhas. Diferentes abordagens podem ter custos computacionais muito diferentes. Em muitos casos, a diferença de performance é negligenciável para aplicações pequenas. No entanto, à medida que seus programas crescem, escolhas eficientes se tornam cada vez mais importantes. Aprenda a usar ferramentas de profiling para identificar gargalos de performance. Otimize baseado em dados reais, não em suposições sobre onde o código é lento. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0001-000000000008"),
            CourseId = _courseId,
            Title = "Loops: Do-While",
            Duration = "50 min",
            Difficulty = "Iniciante",
            EstimatedMinutes = 50,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0001-000000000007" }),
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
                "Entender o loop for e sua estrutura",
                "Aprender quando usar for em vez de while",
                "Dominar iterações com contador"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "O Loop For",
                    Content = "O loop for é ideal quando você sabe exatamente quantas vezes quer repetir o código. A sintaxe é: for (inicialização; condição; incremento) { código }. Por exemplo: for (int i = 0; i < 5; i++) { Console.WriteLine(i); }. Isso tem três partes: inicialização (int i = 0) cria a variável de controle, condição (i < 5) é verificada antes de cada iteração, e incremento (i++) é executado após cada iteração. O loop acima exibe 0, 1, 2, 3, 4. O for é mais compacto que while para contadores porque agrupa inicialização, condição e incremento em uma linha. É a escolha natural para iterar sobre sequências numéricas ou processar arrays. A variável i é convenção para índice, mas você pode usar qualquer nome descritivo.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Variações do Loop For",
                    Content = "O for é muito flexível. Você pode contar para trás: for (int i = 10; i >= 0; i--). Pode incrementar por valores diferentes de 1: for (int i = 0; i < 100; i += 10) conta de 10 em 10. Pode ter múltiplas variáveis: for (int i = 0, j = 10; i < j; i++, j--). Pode omitir partes: for (;;) é um loop infinito (equivalente a while (true)). A condição pode ser qualquer expressão booleana: for (int i = 0; i < array.Length; i++). O incremento pode ser qualquer operação: i *= 2 para dobrar a cada iteração. Loops for aninhados são comuns para trabalhar com matrizes: for (int linha = 0; linha < 3; linha++) { for (int coluna = 0; coluna < 3; coluna++) { } }. Isso cria um padrão de grade 3x3.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "For vs While: Quando Usar Cada Um",
                    Content = "Use for quando você sabe o número de iterações ou está trabalhando com um contador. Use while quando a condição de parada não é baseada em um contador simples. Por exemplo, 'repetir 10 vezes' é for, mas 'repetir até o usuário digitar sair' é while. For é mais legível para iterações numéricas porque toda a lógica de controle está em uma linha. While é mais legível para condições complexas. Ambos podem fazer o mesmo trabalho, mas a escolha certa torna o código mais claro. Um for pode sempre ser reescrito como while, mas geralmente fica menos elegante. Por exemplo: int i = 0; while (i < 5) { código; i++; } é menos claro que for (int i = 0; i < 5; i++) { código }. Escolha a estrutura que melhor expressa sua intenção.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "For Loop Básico",
                    Code = @"for (int i = 1; i <= 5; i++)
{
    Console.WriteLine(""Iteração: "" + i);
}

Console.WriteLine(""Loop terminado!"");",
                    Language = "csharp",
                    Explanation = "Este for loop conta de 1 a 5. A variável i é inicializada com 1, incrementada a cada iteração, e o loop continua enquanto i <= 5.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Tabuada com For",
                    Code = @"int numero = 7;

Console.WriteLine(""Tabuada do "" + numero + "":"");
for (int i = 1; i <= 10; i++)
{
    int resultado = numero * i;
    Console.WriteLine(numero + "" x "" + i + "" = "" + resultado);
}",
                    Language = "csharp",
                    Explanation = "Este exemplo usa for para gerar a tabuada de um número. O loop itera de 1 a 10, calculando e exibindo cada multiplicação.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Números Pares",
                    Description = "Use um loop for para exibir todos os números pares de 0 a 20.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"// Crie o loop for
",
                    Hints = new List<string> 
                    { 
                        "Use for (int i = 0; i <= 20; i += 2)",
                        "Ou use i++ e verifique if (i % 2 == 0)",
                        "Exiba cada número dentro do loop" 
                    }
                },
                new Exercise
                {
                    Title = "Soma de Números",
                    Description = "Use for para calcular a soma de todos os números de 1 a 100. Exiba o resultado final.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"int soma = 0;

// Use for para acumular a soma
",
                    Hints = new List<string> 
                    { 
                        "Use for (int i = 1; i <= 100; i++)",
                        "Dentro do loop: soma += i",
                        "Exiba a soma após o loop" 
                    }
                },
                new Exercise
                {
                    Title = "Padrão de Asteriscos",
                    Description = "Use loops for aninhados para criar um padrão triangular de asteriscos com 5 linhas (1 asterisco na primeira, 2 na segunda, etc).",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Crie loops for aninhados
",
                    Hints = new List<string> 
                    { 
                        "Loop externo: for (int linha = 1; linha <= 5; linha++)",
                        "Loop interno: for (int col = 1; col <= linha; col++)",
                        "Exiba asterisco no loop interno e quebra de linha após" 
                    }
                }
            },
            Summary = "Nesta aula você aprendeu sobre o loop for, ideal para iterações com contador. For é mais compacto e legível que while quando você sabe quantas vezes quer repetir o código. É a escolha natural para trabalhar com sequências numéricas. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno. Grandes empresas como Microsoft, Google e Amazon aplicam estes conceitos em seus sistemas. Ao aprender estas técnicas, você está adquirindo conhecimento usado por desenvolvedores em todo o mundo. Considere como este conceito poderia ser aplicado em um projeto pessoal. Pensar em aplicações práticas ajuda a solidificar o entendimento e torna o aprendizado mais significativo. Iniciantes frequentemente cometem alguns erros ao aprender este tópico. Conhecer estes erros comuns ajuda a evitá-los e desenvolver código mais robusto desde o início. Um erro comum é não considerar casos extremos ou situações inesperadas. Sempre teste seu código com diferentes tipos de entrada, incluindo valores nulos, vazios ou inválidos. Outro erro frequente é otimizar prematuramente. Primeiro faça o código funcionar corretamente, depois otimize se necessário. Como diz o ditado: 'premature optimization is the root of all evil'. Quando encontrar problemas, use o debugger para entender o que está acontecendo. Coloque breakpoints, inspecione variáveis e execute o código passo a passo. Esta é uma habilidade essencial para todo desenvolvedor. Mensagens de erro podem parecer intimidadoras no início, mas elas são suas aliadas. Leia-as cuidadosamente - elas geralmente indicam exatamente onde e qual é o problema. Se algo não funcionar como esperado, não desanime. Debugging é uma parte normal do desenvolvimento. Até desenvolvedores experientes passam tempo significativo depurando código. Testar seu código é tão importante quanto escrevê-lo. Testes automatizados garantem que seu código funciona corretamente e continuará funcionando quando você fizer mudanças. Comece escrevendo testes simples para os casos mais comuns. Depois adicione testes para casos extremos e situações de erro. Uma boa cobertura de testes dá confiança para refatorar e melhorar o código. Considere a prática de TDD (Test-Driven Development), onde você escreve os testes antes do código. Isso ajuda a pensar claramente sobre o que o código deve fazer. Embora a correção seja sempre a prioridade, é importante estar ciente das implicações de performance de suas escolhas. Diferentes abordagens podem ter custos computacionais muito diferentes. Em muitos casos, a diferença de performance é negligenciável para aplicações pequenas. No entanto, à medida que seus programas crescem, escolhas eficientes se tornam cada vez mais importantes. Aprenda a usar ferramentas de profiling para identificar gargalos de performance. Otimize baseado em dados reais, não em suposições sobre onde o código é lento. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0001-000000000009"),
            CourseId = _courseId,
            Title = "Loops: For",
            Duration = "55 min",
            Difficulty = "Iniciante",
            EstimatedMinutes = 55,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0001-000000000008" }),
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
                "Entender o conceito de funções e métodos",
                "Aprender a criar e chamar funções",
                "Compreender parâmetros e valores de retorno"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "O que são Funções?",
                    Content = "Funções (também chamadas de métodos em C#) são blocos de código reutilizáveis que realizam uma tarefa específica. Imagine que você precisa calcular a área de um retângulo em vários lugares do seu programa. Sem funções, você teria que escrever base * altura toda vez. Com funções, você escreve o código uma vez e o reutiliza quantas vezes quiser. Funções tornam o código mais organizado, fácil de entender e manter. Elas seguem o princípio DRY (Don't Repeat Yourself - Não Se Repita). Uma função tem um nome descritivo, pode receber dados de entrada (parâmetros), executa alguma lógica, e pode retornar um resultado. Por exemplo, uma função CalcularArea(base, altura) recebe dois números e retorna a área. Funções são fundamentais para estruturar programas complexos em partes gerenciáveis.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Criando e Chamando Funções",
                    Content = "Em C#, a sintaxe para criar uma função é: tipoRetorno NomeFuncao(parametros) { código; return valor; }. Por exemplo: int Somar(int a, int b) { return a + b; }. O tipo de retorno (int) indica que tipo de dado a função retorna. O nome (Somar) deve ser descritivo. Os parâmetros (int a, int b) são os dados de entrada. O return envia o resultado de volta. Para chamar a função: int resultado = Somar(5, 3);. Isso executa a função com os valores 5 e 3, e armazena o resultado (8) na variável resultado. Funções que não retornam nada usam void como tipo: void ExibirMensagem() { Console.WriteLine('Olá'); }. Funções podem chamar outras funções, criando uma hierarquia de operações. Isso permite decompor problemas complexos em partes simples.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Boas Práticas com Funções",
                    Content = "Funções devem fazer uma coisa e fazê-la bem. Uma função chamada CalcularArea não deveria também exibir o resultado - isso são duas responsabilidades. Mantenha funções pequenas e focadas. Use nomes descritivos que indicam o que a função faz: CalcularDesconto é melhor que Calcular. Parâmetros devem ter nomes claros: CalcularArea(base, altura) é melhor que CalcularArea(x, y). Evite muitos parâmetros (mais de 3-4 é sinal de que a função está fazendo demais). Funções devem ser previsíveis: mesmos parâmetros sempre produzem mesmo resultado. Evite efeitos colaterais (modificar variáveis globais). Documente funções complexas com comentários explicando o que fazem, quais parâmetros esperam e o que retornam. Teste suas funções isoladamente antes de integrá-las ao programa maior. Funções bem escritas são como ferramentas confiáveis que você pode usar repetidamente.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Função Simples com Retorno",
                    Code = @"int Somar(int a, int b)
{
    return a + b;
}

int resultado = Somar(10, 5);
Console.WriteLine(""Resultado: "" + resultado);

int outroResultado = Somar(100, 200);
Console.WriteLine(""Outro resultado: "" + outroResultado);",
                    Language = "csharp",
                    Explanation = "Esta função Somar recebe dois inteiros e retorna a soma. Podemos chamá-la múltiplas vezes com diferentes valores, reutilizando o código.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Função Void sem Retorno",
                    Code = @"void ExibirBoasVindas(string nome)
{
    Console.WriteLine(""Bem-vindo, "" + nome + ""!"");
    Console.WriteLine(""É ótimo ter você aqui."");
}

ExibirBoasVindas(""Maria"");
ExibirBoasVindas(""João"");",
                    Language = "csharp",
                    Explanation = "Esta função void não retorna valor, apenas executa ações (exibir mensagens). Recebe um parâmetro nome e o usa nas mensagens.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Função de Multiplicação",
                    Description = "Crie uma função Multiplicar que recebe dois números inteiros e retorna o produto. Teste chamando a função com diferentes valores.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"// Crie a função Multiplicar aqui

// Teste a função
",
                    Hints = new List<string> 
                    { 
                        "Use: int Multiplicar(int a, int b)",
                        "Retorne a * b",
                        "Chame a função e exiba o resultado" 
                    }
                },
                new Exercise
                {
                    Title = "Função de Verificação de Maioridade",
                    Description = "Crie uma função EhMaiorDeIdade que recebe uma idade e retorna true se >= 18, false caso contrário. Teste com várias idades.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Crie a função EhMaiorDeIdade

// Teste com diferentes idades
",
                    Hints = new List<string> 
                    { 
                        "Use: bool EhMaiorDeIdade(int idade)",
                        "Retorne idade >= 18",
                        "Teste com idades menores e maiores que 18" 
                    }
                },
                new Exercise
                {
                    Title = "Calculadora com Funções",
                    Description = "Crie quatro funções: Somar, Subtrair, Multiplicar e Dividir. Cada uma recebe dois doubles e retorna o resultado. Teste todas.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Crie as quatro funções

// Teste todas as funções
",
                    Hints = new List<string> 
                    { 
                        "Cada função: double NomeOperacao(double a, double b)",
                        "Cuidado com divisão por zero",
                        "Teste cada função com valores diferentes" 
                    }
                }
            },
            Summary = "Nesta aula você aprendeu sobre funções, blocos de código reutilizáveis que tornam programas mais organizados e eficientes. Funções podem receber parâmetros e retornar valores, permitindo criar código modular e fácil de manter. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno. Grandes empresas como Microsoft, Google e Amazon aplicam estes conceitos em seus sistemas. Ao aprender estas técnicas, você está adquirindo conhecimento usado por desenvolvedores em todo o mundo. Considere como este conceito poderia ser aplicado em um projeto pessoal. Pensar em aplicações práticas ajuda a solidificar o entendimento e torna o aprendizado mais significativo. Iniciantes frequentemente cometem alguns erros ao aprender este tópico. Conhecer estes erros comuns ajuda a evitá-los e desenvolver código mais robusto desde o início. Um erro comum é não considerar casos extremos ou situações inesperadas. Sempre teste seu código com diferentes tipos de entrada, incluindo valores nulos, vazios ou inválidos. Outro erro frequente é otimizar prematuramente. Primeiro faça o código funcionar corretamente, depois otimize se necessário. Como diz o ditado: 'premature optimization is the root of all evil'. Quando encontrar problemas, use o debugger para entender o que está acontecendo. Coloque breakpoints, inspecione variáveis e execute o código passo a passo. Esta é uma habilidade essencial para todo desenvolvedor. Mensagens de erro podem parecer intimidadoras no início, mas elas são suas aliadas. Leia-as cuidadosamente - elas geralmente indicam exatamente onde e qual é o problema. Se algo não funcionar como esperado, não desanime. Debugging é uma parte normal do desenvolvimento. Até desenvolvedores experientes passam tempo significativo depurando código. Testar seu código é tão importante quanto escrevê-lo. Testes automatizados garantem que seu código funciona corretamente e continuará funcionando quando você fizer mudanças. Comece escrevendo testes simples para os casos mais comuns. Depois adicione testes para casos extremos e situações de erro. Uma boa cobertura de testes dá confiança para refatorar e melhorar o código. Considere a prática de TDD (Test-Driven Development), onde você escreve os testes antes do código. Isso ajuda a pensar claramente sobre o que o código deve fazer. Embora a correção seja sempre a prioridade, é importante estar ciente das implicações de performance de suas escolhas. Diferentes abordagens podem ter custos computacionais muito diferentes. Em muitos casos, a diferença de performance é negligenciável para aplicações pequenas. No entanto, à medida que seus programas crescem, escolhas eficientes se tornam cada vez mais importantes. Aprenda a usar ferramentas de profiling para identificar gargalos de performance. Otimize baseado em dados reais, não em suposições sobre onde o código é lento. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0001-000000000010"),
            CourseId = _courseId,
            Title = "Introdução a Funções",
            Duration = "60 min",
            Difficulty = "Iniciante",
            EstimatedMinutes = 60,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0001-000000000009" }),
            OrderIndex = 10,
            Content = "",
            StructuredContent = JsonSerializer.Serialize(content),
            Version = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    // Lessons 11-20 will be created with similar structure but more concisely
    private Lesson CreateLesson11()
    {
        var content = new LessonContent
        {
            Objectives = new List<string>
            {
                "Entender parâmetros e argumentos",
                "Aprender sobre sobrecarga de funções",
                "Dominar passagem de parâmetros por valor"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Parâmetros e Argumentos",
                    Content = "Parâmetros são as variáveis definidas na assinatura da função, enquanto argumentos são os valores reais passados quando você chama a função. Por exemplo, em void Saudar(string nome), 'nome' é um parâmetro. Quando você chama Saudar('Maria'), 'Maria' é o argumento. Parâmetros permitem que funções sejam flexíveis e trabalhem com diferentes dados. Você pode ter múltiplos parâmetros: double CalcularMedia(double nota1, double nota2, double nota3). A ordem dos argumentos deve corresponder à ordem dos parâmetros. Parâmetros podem ter valores padrão: void Saudar(string nome = 'Visitante'), permitindo chamar a função sem argumentos. Isso torna funções mais versáteis e fáceis de usar. Escolha nomes de parâmetros descritivos que indicam claramente o que representam.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Sobrecarga de Funções",
                    Content = "Sobrecarga (overloading) permite ter múltiplas funções com o mesmo nome mas diferentes parâmetros. Por exemplo, você pode ter int Somar(int a, int b) e double Somar(double a, double b). O compilador escolhe qual versão chamar baseado nos tipos dos argumentos. Isso torna o código mais intuitivo - você não precisa de nomes como SomarInteiros e SomarDecimais. A sobrecarga pode variar no número de parâmetros: Somar(int a, int b) e Somar(int a, int b, int c). Ou no tipo: Somar(int, int) e Somar(double, double). Mas não pode variar apenas no tipo de retorno. Sobrecarga é útil quando a mesma operação lógica pode ser aplicada a diferentes tipos de dados. Use com moderação - muitas sobrecargas podem confundir.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Passagem por Valor",
                    Content = "Em C#, parâmetros são passados por valor por padrão. Isso significa que a função recebe uma cópia do valor, não a variável original. Se você modifica o parâmetro dentro da função, a variável original não é afetada. Por exemplo: void Dobrar(int numero) { numero = numero * 2; }. Se você chama int x = 5; Dobrar(x);, x ainda será 5 após a chamada. A função dobrou sua cópia local, não o x original. Para modificar a variável original, você precisa retornar o novo valor: int Dobrar(int numero) { return numero * 2; } e então x = Dobrar(x);. Ou usar ref/out (tópico avançado). Passagem por valor é mais segura porque funções não podem acidentalmente modificar dados externos. Isso torna o código mais previsível e fácil de debugar.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Função com Múltiplos Parâmetros",
                    Code = @"double CalcularMedia(double nota1, double nota2, double nota3)
{
    return (nota1 + nota2 + nota3) / 3.0;
}

double media = CalcularMedia(8.5, 7.0, 9.5);
Console.WriteLine(""Média: "" + media);",
                    Language = "csharp",
                    Explanation = "Esta função recebe três parâmetros e calcula a média. Os argumentos são passados na mesma ordem dos parâmetros.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Sobrecarga de Funções",
                    Code = @"int Somar(int a, int b)
{
    return a + b;
}

double Somar(double a, double b)
{
    return a + b;
}

Console.WriteLine(""Soma inteiros: "" + Somar(5, 3));
Console.WriteLine(""Soma decimais: "" + Somar(5.5, 3.2));",
                    Language = "csharp",
                    Explanation = "Duas funções Somar com diferentes tipos de parâmetros. O compilador escolhe automaticamente qual usar baseado nos argumentos.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Função com Três Parâmetros",
                    Description = "Crie uma função CalcularVolume que recebe comprimento, largura e altura, e retorna o volume (comprimento * largura * altura).",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"// Crie a função CalcularVolume

// Teste a função
",
                    Hints = new List<string> 
                    { 
                        "Use: double CalcularVolume(double comp, double larg, double alt)",
                        "Retorne comp * larg * alt",
                        "Teste com valores como 10, 5, 2" 
                    }
                },
                new Exercise
                {
                    Title = "Sobrecarga de Área",
                    Description = "Crie duas funções CalcularArea: uma para retângulo (base, altura) e outra para círculo (raio). Use Math.PI para o círculo.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Crie as duas versões de CalcularArea

// Teste ambas
",
                    Hints = new List<string> 
                    { 
                        "Retângulo: double CalcularArea(double base, double altura)",
                        "Círculo: double CalcularArea(double raio) - use Math.PI * raio * raio",
                        "Teste cada versão" 
                    }
                },
                new Exercise
                {
                    Title = "Função de Formatação",
                    Description = "Crie uma função FormatarNomeCompleto que recebe nome e sobrenome e retorna 'Sobrenome, Nome' (formato formal).",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"// Crie a função FormatarNomeCompleto

// Teste com diferentes nomes
",
                    Hints = new List<string> 
                    { 
                        "Use: string FormatarNomeCompleto(string nome, string sobrenome)",
                        "Retorne sobrenome + ', ' + nome",
                        "Teste com vários nomes" 
                    }
                }
            },
            Summary = "Nesta aula você aprendeu sobre parâmetros, argumentos e sobrecarga de funções. Parâmetros tornam funções flexíveis, e sobrecarga permite ter múltiplas versões da mesma função para diferentes tipos de dados. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno. Grandes empresas como Microsoft, Google e Amazon aplicam estes conceitos em seus sistemas. Ao aprender estas técnicas, você está adquirindo conhecimento usado por desenvolvedores em todo o mundo. Considere como este conceito poderia ser aplicado em um projeto pessoal. Pensar em aplicações práticas ajuda a solidificar o entendimento e torna o aprendizado mais significativo. Iniciantes frequentemente cometem alguns erros ao aprender este tópico. Conhecer estes erros comuns ajuda a evitá-los e desenvolver código mais robusto desde o início. Um erro comum é não considerar casos extremos ou situações inesperadas. Sempre teste seu código com diferentes tipos de entrada, incluindo valores nulos, vazios ou inválidos. Outro erro frequente é otimizar prematuramente. Primeiro faça o código funcionar corretamente, depois otimize se necessário. Como diz o ditado: 'premature optimization is the root of all evil'. Quando encontrar problemas, use o debugger para entender o que está acontecendo. Coloque breakpoints, inspecione variáveis e execute o código passo a passo. Esta é uma habilidade essencial para todo desenvolvedor. Mensagens de erro podem parecer intimidadoras no início, mas elas são suas aliadas. Leia-as cuidadosamente - elas geralmente indicam exatamente onde e qual é o problema. Se algo não funcionar como esperado, não desanime. Debugging é uma parte normal do desenvolvimento. Até desenvolvedores experientes passam tempo significativo depurando código. Testar seu código é tão importante quanto escrevê-lo. Testes automatizados garantem que seu código funciona corretamente e continuará funcionando quando você fizer mudanças. Comece escrevendo testes simples para os casos mais comuns. Depois adicione testes para casos extremos e situações de erro. Uma boa cobertura de testes dá confiança para refatorar e melhorar o código. Considere a prática de TDD (Test-Driven Development), onde você escreve os testes antes do código. Isso ajuda a pensar claramente sobre o que o código deve fazer. Embora a correção seja sempre a prioridade, é importante estar ciente das implicações de performance de suas escolhas. Diferentes abordagens podem ter custos computacionais muito diferentes. Em muitos casos, a diferença de performance é negligenciável para aplicações pequenas. No entanto, à medida que seus programas crescem, escolhas eficientes se tornam cada vez mais importantes. Aprenda a usar ferramentas de profiling para identificar gargalos de performance. Otimize baseado em dados reais, não em suposições sobre onde o código é lento. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0001-000000000011"),
            CourseId = _courseId,
            Title = "Parâmetros e Sobrecarga de Funções",
            Duration = "55 min",
            Difficulty = "Iniciante",
            EstimatedMinutes = 55,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0001-000000000010" }),
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
                "Entender o conceito de escopo de variáveis",
                "Aprender sobre variáveis locais e globais",
                "Evitar conflitos de escopo"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "O que é Escopo?",
                    Content = "Escopo define onde uma variável pode ser acessada no código. Uma variável declarada dentro de uma função só existe dentro daquela função - é uma variável local. Quando a função termina, a variável é destruída. Por exemplo, se você declara int x = 5 dentro de uma função, não pode acessar x fora dela. Isso é bom porque evita conflitos - diferentes funções podem ter suas próprias variáveis x sem interferir umas nas outras. Variáveis declaradas fora de funções (no nível da classe) são variáveis de instância ou estáticas, acessíveis por múltiplas funções. Blocos de código (dentro de if, loops, etc) também criam escopos. Uma variável declarada dentro de um if só existe dentro daquele if. Entender escopo é crucial para evitar erros e escrever código organizado.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Variáveis Locais vs Globais",
                    Content = "Variáveis locais são declaradas dentro de funções ou blocos e só existem ali. São criadas quando o bloco é executado e destruídas quando termina. Variáveis globais (ou de classe em C#) são declaradas fora de funções e podem ser acessadas por todo o código. Use variáveis locais sempre que possível - elas são mais seguras e fáceis de entender. Variáveis globais podem ser modificadas por qualquer parte do código, tornando difícil rastrear mudanças e causando bugs sutis. Se múltiplas funções precisam compartilhar dados, considere passar como parâmetros em vez de usar globais. Variáveis locais também são mais eficientes em memória porque são liberadas quando não são mais necessárias. Uma boa regra: mantenha o escopo o mais restrito possível. Declare variáveis no menor escopo onde são necessárias.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Sombreamento e Boas Práticas",
                    Content = "Sombreamento (shadowing) ocorre quando uma variável local tem o mesmo nome que uma variável de escopo maior. A variável local 'esconde' a externa dentro de seu escopo. Por exemplo, se há um int x global e você declara int x dentro de uma função, a função usa sua própria x. Isso pode causar confusão. Evite sombreamento usando nomes diferentes. Use nomes descritivos que deixam claro o propósito da variável. Prefira nomes como 'contador', 'soma', 'nomeUsuario' em vez de 'x', 'temp', 'var'. Declare variáveis o mais próximo possível de onde são usadas. Não declare todas no início da função se só são usadas no final. Inicialize variáveis quando as declara sempre que possível. Variáveis não inicializadas são fonte comum de bugs. O compilador C# ajuda detectando uso de variáveis não inicializadas.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Escopo de Variáveis Locais",
                    Code = @"void FuncaoA()
{
    int x = 10;
    Console.WriteLine(""FuncaoA - x: "" + x);
}

void FuncaoB()
{
    int x = 20; // Diferente x, escopo diferente
    Console.WriteLine(""FuncaoB - x: "" + x);
}

FuncaoA();
FuncaoB();",
                    Language = "csharp",
                    Explanation = "Cada função tem sua própria variável x. Elas não interferem uma com a outra porque têm escopos diferentes.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Escopo em Blocos",
                    Code = @"int numero = 10;

if (numero > 5)
{
    int mensagem = ""Número é maior que 5"";
    Console.WriteLine(mensagem);
}

// mensagem não existe aqui - erro se tentar usar
Console.WriteLine(""Número: "" + numero); // numero existe aqui",
                    Language = "csharp",
                    Explanation = "A variável mensagem só existe dentro do bloco if. A variável numero existe em todo o método porque foi declarada fora do if.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Variáveis Locais",
                    Description = "Crie duas funções que cada uma declara uma variável local com o mesmo nome mas valores diferentes. Chame ambas e observe que não há conflito.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"// Crie as duas funções

// Chame ambas
",
                    Hints = new List<string> 
                    { 
                        "Cada função declara sua própria variável",
                        "Use o mesmo nome em ambas para ver que não conflitam",
                        "Exiba o valor dentro de cada função" 
                    }
                },
                new Exercise
                {
                    Title = "Escopo em Loop",
                    Description = "Crie um loop for onde a variável de controle i é declarada no for. Tente acessar i após o loop e observe o erro de compilação.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"// Crie o loop for

// Tente acessar i aqui (causará erro)
",
                    Hints = new List<string> 
                    { 
                        "Use: for (int i = 0; i < 5; i++)",
                        "i só existe dentro do loop",
                        "Comentar a tentativa de acesso após o loop para compilar" 
                    }
                },
                new Exercise
                {
                    Title = "Função com Variável Temporária",
                    Description = "Crie uma função que calcula o quadrado de um número. Use uma variável local temporária para armazenar o resultado antes de retornar.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"// Crie a função Quadrado

// Teste a função
",
                    Hints = new List<string> 
                    { 
                        "int Quadrado(int numero)",
                        "Declare int resultado = numero * numero dentro da função",
                        "Retorne resultado" 
                    }
                }
            },
            Summary = "Nesta aula você aprendeu sobre escopo de variáveis - onde elas podem ser acessadas no código. Variáveis locais existem apenas em seu bloco, enquanto variáveis de escopo maior podem ser acessadas mais amplamente. Mantenha o escopo restrito para código mais seguro e organizado. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno. Grandes empresas como Microsoft, Google e Amazon aplicam estes conceitos em seus sistemas. Ao aprender estas técnicas, você está adquirindo conhecimento usado por desenvolvedores em todo o mundo. Considere como este conceito poderia ser aplicado em um projeto pessoal. Pensar em aplicações práticas ajuda a solidificar o entendimento e torna o aprendizado mais significativo. Iniciantes frequentemente cometem alguns erros ao aprender este tópico. Conhecer estes erros comuns ajuda a evitá-los e desenvolver código mais robusto desde o início. Um erro comum é não considerar casos extremos ou situações inesperadas. Sempre teste seu código com diferentes tipos de entrada, incluindo valores nulos, vazios ou inválidos. Outro erro frequente é otimizar prematuramente. Primeiro faça o código funcionar corretamente, depois otimize se necessário. Como diz o ditado: 'premature optimization is the root of all evil'. Quando encontrar problemas, use o debugger para entender o que está acontecendo. Coloque breakpoints, inspecione variáveis e execute o código passo a passo. Esta é uma habilidade essencial para todo desenvolvedor. Mensagens de erro podem parecer intimidadoras no início, mas elas são suas aliadas. Leia-as cuidadosamente - elas geralmente indicam exatamente onde e qual é o problema. Se algo não funcionar como esperado, não desanime. Debugging é uma parte normal do desenvolvimento. Até desenvolvedores experientes passam tempo significativo depurando código. Testar seu código é tão importante quanto escrevê-lo. Testes automatizados garantem que seu código funciona corretamente e continuará funcionando quando você fizer mudanças. Comece escrevendo testes simples para os casos mais comuns. Depois adicione testes para casos extremos e situações de erro. Uma boa cobertura de testes dá confiança para refatorar e melhorar o código. Considere a prática de TDD (Test-Driven Development), onde você escreve os testes antes do código. Isso ajuda a pensar claramente sobre o que o código deve fazer. Embora a correção seja sempre a prioridade, é importante estar ciente das implicações de performance de suas escolhas. Diferentes abordagens podem ter custos computacionais muito diferentes. Em muitos casos, a diferença de performance é negligenciável para aplicações pequenas. No entanto, à medida que seus programas crescem, escolhas eficientes se tornam cada vez mais importantes. Aprenda a usar ferramentas de profiling para identificar gargalos de performance. Otimize baseado em dados reais, não em suposições sobre onde o código é lento. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0001-000000000012"),
            CourseId = _courseId,
            Title = "Escopo de Variáveis",
            Duration = "50 min",
            Difficulty = "Iniciante",
            EstimatedMinutes = 50,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0001-000000000011" }),
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
                "Entender o conceito de arrays",
                "Aprender a declarar e inicializar arrays",
                "Acessar e modificar elementos de arrays"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Introdução a Arrays",
                    Content = "Arrays são coleções de elementos do mesmo tipo armazenados em sequência na memória. Imagine uma fileira de caixas numeradas, cada uma contendo um valor. Em vez de declarar int numero1, numero2, numero3, você declara int[] numeros = new int[3] e acessa cada elemento por índice: numeros[0], numeros[1], numeros[2]. Arrays têm tamanho fixo definido na criação. O índice começa em 0, então um array de tamanho 5 tem índices 0, 1, 2, 3, 4. Arrays são úteis para armazenar coleções de dados relacionados como notas de alunos, temperaturas diárias, ou lista de nomes. Você pode iterar sobre arrays com loops for, processando cada elemento. Arrays tornam o código mais limpo e escalável - é mais fácil trabalhar com um array de 100 elementos do que 100 variáveis separadas.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Declarando e Inicializando Arrays",
                    Content = "Há várias formas de criar arrays em C#. Declaração com tamanho: int[] numeros = new int[5] cria um array de 5 inteiros (inicializados com 0). Declaração com valores: int[] numeros = {1, 2, 3, 4, 5} cria e inicializa em uma linha. Declaração explícita: int[] numeros = new int[] {1, 2, 3} também funciona. Para acessar elementos, use o índice entre colchetes: int primeiro = numeros[0]. Para modificar: numeros[0] = 10. A propriedade Length retorna o tamanho: numeros.Length. Cuidado com índices fora dos limites - acessar numeros[5] em um array de tamanho 5 causa erro (IndexOutOfRangeException). Sempre verifique que o índice está entre 0 e Length-1. Arrays podem conter qualquer tipo: string[] nomes, double[] precos, bool[] flags.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Trabalhando com Arrays",
                    Content = "Loops for são perfeitos para arrays: for (int i = 0; i < numeros.Length; i++) { Console.WriteLine(numeros[i]); }. O loop foreach é ainda mais simples: foreach (int numero in numeros) { Console.WriteLine(numero); }. Foreach itera sobre cada elemento sem precisar de índice. Use for quando precisa do índice, foreach quando só precisa dos valores. Você pode calcular estatísticas: somar todos os elementos, encontrar o maior/menor, calcular média. Arrays podem ser passados para funções: void ProcessarArray(int[] dados). Funções podem modificar elementos do array (arrays são tipos de referência). Para copiar um array, use Array.Copy() ou Clone() - atribuição simples (array2 = array1) faz ambos apontarem para o mesmo array. Arrays multidimensionais existem mas são tópico avançado.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Criando e Acessando Arrays",
                    Code = @"int[] numeros = {10, 20, 30, 40, 50};

Console.WriteLine(""Primeiro elemento: "" + numeros[0]);
Console.WriteLine(""Terceiro elemento: "" + numeros[2]);
Console.WriteLine(""Tamanho do array: "" + numeros.Length);

numeros[1] = 25; // Modificando elemento
Console.WriteLine(""Segundo elemento modificado: "" + numeros[1]);",
                    Language = "csharp",
                    Explanation = "Este código cria um array, acessa elementos por índice, verifica o tamanho e modifica um elemento.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Iterando sobre Arrays",
                    Code = @"string[] frutas = {""Maçã"", ""Banana"", ""Laranja"", ""Uva""};

Console.WriteLine(""Usando for:"");
for (int i = 0; i < frutas.Length; i++)
{
    Console.WriteLine((i + 1) + "". "" + frutas[i]);
}

Console.WriteLine(""\nUsando foreach:"");
foreach (string fruta in frutas)
{
    Console.WriteLine(""- "" + fruta);
}",
                    Language = "csharp",
                    Explanation = "Demonstra duas formas de iterar sobre arrays: for (com índice) e foreach (apenas valores). Ambas são úteis em diferentes situações.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Criar e Exibir Array",
                    Description = "Crie um array com 5 números inteiros de sua escolha. Use um loop for para exibir todos os elementos.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"// Crie o array

// Use for para exibir
",
                    Hints = new List<string> 
                    { 
                        "int[] numeros = {valor1, valor2, ...}",
                        "Use for (int i = 0; i < numeros.Length; i++)",
                        "Exiba numeros[i] dentro do loop" 
                    }
                },
                new Exercise
                {
                    Title = "Soma de Array",
                    Description = "Crie um array de números. Use um loop para calcular e exibir a soma de todos os elementos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"int[] numeros = {5, 10, 15, 20, 25};
int soma = 0;

// Calcule a soma
",
                    Hints = new List<string> 
                    { 
                        "Use foreach ou for para iterar",
                        "Acumule: soma += numero",
                        "Exiba a soma após o loop" 
                    }
                },
                new Exercise
                {
                    Title = "Encontrar Maior Elemento",
                    Description = "Crie um array de números. Encontre e exiba o maior elemento do array.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"int[] numeros = {23, 45, 12, 67, 34, 89, 15};

// Encontre o maior
",
                    Hints = new List<string> 
                    { 
                        "Inicialize maior = numeros[0]",
                        "Itere e compare: if (numeros[i] > maior) maior = numeros[i]",
                        "Exiba o maior após o loop" 
                    }
                }
            },
            Summary = "Nesta aula você aprendeu sobre arrays, coleções de elementos do mesmo tipo. Arrays permitem armazenar múltiplos valores em uma única variável e acessá-los por índice. São fundamentais para trabalhar com coleções de dados. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno. Grandes empresas como Microsoft, Google e Amazon aplicam estes conceitos em seus sistemas. Ao aprender estas técnicas, você está adquirindo conhecimento usado por desenvolvedores em todo o mundo. Considere como este conceito poderia ser aplicado em um projeto pessoal. Pensar em aplicações práticas ajuda a solidificar o entendimento e torna o aprendizado mais significativo. Iniciantes frequentemente cometem alguns erros ao aprender este tópico. Conhecer estes erros comuns ajuda a evitá-los e desenvolver código mais robusto desde o início. Um erro comum é não considerar casos extremos ou situações inesperadas. Sempre teste seu código com diferentes tipos de entrada, incluindo valores nulos, vazios ou inválidos. Outro erro frequente é otimizar prematuramente. Primeiro faça o código funcionar corretamente, depois otimize se necessário. Como diz o ditado: 'premature optimization is the root of all evil'. Quando encontrar problemas, use o debugger para entender o que está acontecendo. Coloque breakpoints, inspecione variáveis e execute o código passo a passo. Esta é uma habilidade essencial para todo desenvolvedor. Mensagens de erro podem parecer intimidadoras no início, mas elas são suas aliadas. Leia-as cuidadosamente - elas geralmente indicam exatamente onde e qual é o problema. Se algo não funcionar como esperado, não desanime. Debugging é uma parte normal do desenvolvimento. Até desenvolvedores experientes passam tempo significativo depurando código. Testar seu código é tão importante quanto escrevê-lo. Testes automatizados garantem que seu código funciona corretamente e continuará funcionando quando você fizer mudanças. Comece escrevendo testes simples para os casos mais comuns. Depois adicione testes para casos extremos e situações de erro. Uma boa cobertura de testes dá confiança para refatorar e melhorar o código. Considere a prática de TDD (Test-Driven Development), onde você escreve os testes antes do código. Isso ajuda a pensar claramente sobre o que o código deve fazer. Embora a correção seja sempre a prioridade, é importante estar ciente das implicações de performance de suas escolhas. Diferentes abordagens podem ter custos computacionais muito diferentes. Em muitos casos, a diferença de performance é negligenciável para aplicações pequenas. No entanto, à medida que seus programas crescem, escolhas eficientes se tornam cada vez mais importantes. Aprenda a usar ferramentas de profiling para identificar gargalos de performance. Otimize baseado em dados reais, não em suposições sobre onde o código é lento. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0001-000000000013"),
            CourseId = _courseId,
            Title = "Introdução a Arrays",
            Duration = "60 min",
            Difficulty = "Iniciante",
            EstimatedMinutes = 60,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0001-000000000012" }),
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
                "Trabalhar com strings em C#",
                "Aprender métodos comuns de string",
                "Manipular e formatar texto"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Strings em C#",
                    Content = "Strings são sequências de caracteres usadas para representar texto. Em C#, strings são imutáveis - uma vez criadas, não podem ser modificadas. Quando você 'modifica' uma string, na verdade está criando uma nova. Strings são declaradas com aspas duplas: string nome = 'João'. Você pode concatenar strings com +: string nomeCompleto = nome + ' ' + sobrenome. Strings têm muitos métodos úteis: Length retorna o tamanho, ToUpper() converte para maiúsculas, ToLower() para minúsculas, Trim() remove espaços nas extremidades. Você pode acessar caracteres individuais como array: char primeira = nome[0]. Strings podem ser comparadas com == ou .Equals(). Para verificar se contém texto, use .Contains(). Strings são tipos de referência mas se comportam como valores em comparações.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Métodos Úteis de String",
                    Content = "C# oferece muitos métodos para manipular strings. Substring(inicio, tamanho) extrai parte da string: 'Olá Mundo'.Substring(0, 3) retorna 'Olá'. Replace(antigo, novo) substitui texto: 'Olá'.Replace('O', 'A') retorna 'Alá'. Split(separador) divide string em array: 'a,b,c'.Split(',') retorna ['a', 'b', 'c']. StartsWith() e EndsWith() verificam início e fim. IndexOf() encontra posição de texto. String.IsNullOrEmpty() verifica se string é nula ou vazia. Para formatar strings, use interpolação: string mensagem = $'Olá, {nome}!'. Ou String.Format(): String.Format('Olá, {0}!', nome). Para múltiplas concatenações, use StringBuilder (mais eficiente). Strings são case-sensitive por padrão - 'Olá' != 'olá'. Use ToLower() em ambas para comparação case-insensitive.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Boas Práticas com Strings",
                    Content = "Sempre verifique se strings são nulas antes de usar métodos: if (texto != null && texto.Length > 0). Ou use String.IsNullOrEmpty(texto). Para concatenar muitas strings em loop, use StringBuilder em vez de +, pois é muito mais eficiente. Evite comparar strings com == quando case não importa - use .Equals(outra, StringComparison.OrdinalIgnoreCase). Ao trabalhar com entrada do usuário, sempre use Trim() para remover espaços extras. Cuidado com caracteres especiais em strings - use @ para strings literais. Para strings multilinha, use @ também. Strings são imutáveis, então operações como Replace não modificam a original - você deve atribuir o resultado: texto = texto.Replace('a', 'b'). Lembre-se que strings vazias são diferentes de null.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Métodos Básicos de String",
                    Code = @"string texto = ""  Olá, Mundo!  "";

Console.WriteLine(""Original: '"" + texto + ""'"");
Console.WriteLine(""Tamanho: "" + texto.Length);
Console.WriteLine(""Maiúsculas: "" + texto.ToUpper());
Console.WriteLine(""Minúsculas: "" + texto.ToLower());
Console.WriteLine(""Sem espaços: '"" + texto.Trim() + ""'"");",
                    Language = "csharp",
                    Explanation = "Demonstra métodos comuns: Length, ToUpper, ToLower e Trim. Note que os métodos não modificam a string original.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Interpolação e Formatação",
                    Code = @"string nome = ""Maria"";
int idade = 25;
double altura = 1.65;

string mensagem1 = $""{nome} tem {idade} anos e {altura}m de altura."";
Console.WriteLine(mensagem1);

string mensagem2 = String.Format(""{0} tem {1} anos."", nome, idade);
Console.WriteLine(mensagem2);",
                    Language = "csharp",
                    Explanation = "Mostra duas formas de formatar strings: interpolação ($) e String.Format(). Interpolação é mais moderna e legível.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Manipulação Básica",
                    Description = "Crie uma string com seu nome completo. Exiba em maiúsculas, minúsculas, e o tamanho da string.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"string nomeCompleto = ""Seu Nome Aqui"";

// Exiba as transformações
",
                    Hints = new List<string> 
                    { 
                        "Use .ToUpper() para maiúsculas",
                        "Use .ToLower() para minúsculas",
                        "Use .Length para o tamanho" 
                    }
                },
                new Exercise
                {
                    Title = "Verificador de Email",
                    Description = "Crie uma string com um email. Verifique se contém '@' e '.com'. Exiba se é válido ou não.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"string email = ""usuario@exemplo.com"";

// Verifique se é válido
",
                    Hints = new List<string> 
                    { 
                        "Use .Contains(\"@\") e .Contains(\".com\")",
                        "Combine com && para verificar ambos",
                        "Use if-else para exibir o resultado" 
                    }
                },
                new Exercise
                {
                    Title = "Formatador de Nome",
                    Description = "Crie variáveis para nome e sobrenome. Use interpolação para criar uma mensagem de boas-vindas personalizada.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"string nome = ""João"";
string sobrenome = ""Silva"";

// Crie mensagem com interpolação
",
                    Hints = new List<string> 
                    { 
                        "Use $\"Bem-vindo, {nome} {sobrenome}!\"",
                        "Você pode adicionar mais informações na mensagem",
                        "Exiba a mensagem formatada" 
                    }
                }
            },
            Summary = "Nesta aula você aprendeu a trabalhar com strings em C#. Strings são fundamentais para manipular texto, e C# oferece muitos métodos úteis para transformar, buscar e formatar strings. Dominar strings é essencial para qualquer programador. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno. Grandes empresas como Microsoft, Google e Amazon aplicam estes conceitos em seus sistemas. Ao aprender estas técnicas, você está adquirindo conhecimento usado por desenvolvedores em todo o mundo. Considere como este conceito poderia ser aplicado em um projeto pessoal. Pensar em aplicações práticas ajuda a solidificar o entendimento e torna o aprendizado mais significativo. Iniciantes frequentemente cometem alguns erros ao aprender este tópico. Conhecer estes erros comuns ajuda a evitá-los e desenvolver código mais robusto desde o início. Um erro comum é não considerar casos extremos ou situações inesperadas. Sempre teste seu código com diferentes tipos de entrada, incluindo valores nulos, vazios ou inválidos. Outro erro frequente é otimizar prematuramente. Primeiro faça o código funcionar corretamente, depois otimize se necessário. Como diz o ditado: 'premature optimization is the root of all evil'. Quando encontrar problemas, use o debugger para entender o que está acontecendo. Coloque breakpoints, inspecione variáveis e execute o código passo a passo. Esta é uma habilidade essencial para todo desenvolvedor. Mensagens de erro podem parecer intimidadoras no início, mas elas são suas aliadas. Leia-as cuidadosamente - elas geralmente indicam exatamente onde e qual é o problema. Se algo não funcionar como esperado, não desanime. Debugging é uma parte normal do desenvolvimento. Até desenvolvedores experientes passam tempo significativo depurando código. Testar seu código é tão importante quanto escrevê-lo. Testes automatizados garantem que seu código funciona corretamente e continuará funcionando quando você fizer mudanças. Comece escrevendo testes simples para os casos mais comuns. Depois adicione testes para casos extremos e situações de erro. Uma boa cobertura de testes dá confiança para refatorar e melhorar o código. Considere a prática de TDD (Test-Driven Development), onde você escreve os testes antes do código. Isso ajuda a pensar claramente sobre o que o código deve fazer. Embora a correção seja sempre a prioridade, é importante estar ciente das implicações de performance de suas escolhas. Diferentes abordagens podem ter custos computacionais muito diferentes. Em muitos casos, a diferença de performance é negligenciável para aplicações pequenas. No entanto, à medida que seus programas crescem, escolhas eficientes se tornam cada vez mais importantes. Aprenda a usar ferramentas de profiling para identificar gargalos de performance. Otimize baseado em dados reais, não em suposições sobre onde o código é lento. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0001-000000000014"),
            CourseId = _courseId,
            Title = "Trabalhando com Strings",
            Duration = "55 min",
            Difficulty = "Iniciante",
            EstimatedMinutes = 55,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0001-000000000013" }),
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
                "Entender o que são erros e exceções",
                "Aprender a identificar erros comuns",
                "Conhecer técnicas básicas de depuração"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Tipos de Erros",
                    Content = "Existem três tipos principais de erros em programação. Erros de sintaxe ocorrem quando o código não segue as regras da linguagem, como esquecer um ponto e vírgula ou fechar chaves. O compilador detecta esses erros e não permite executar o programa. Erros de tempo de execução (runtime errors) acontecem durante a execução, como dividir por zero ou acessar índice inválido de array. Esses causam exceções e podem travar o programa. Erros lógicos são os mais difíceis - o código compila e executa, mas produz resultados incorretos. Por exemplo, usar + em vez de * em um cálculo. O compilador não pode detectar erros lógicos; você precisa testar e debugar. Entender esses tipos ajuda a identificar e corrigir problemas mais rapidamente. Sempre leia mensagens de erro cuidadosamente - elas indicam o problema e onde ocorreu.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Erros Comuns para Iniciantes",
                    Content = "Alguns erros são muito comuns ao aprender programação. Esquecer ponto e vírgula no final de instruções. Usar = (atribuição) em vez de == (comparação) em condições. Acessar índices fora dos limites de arrays. Esquecer de incrementar contador em loops, causando loops infinitos. Confundir tipos de dados, como tentar somar string com int sem converter. Esquecer de retornar valor em funções não-void. Usar variáveis antes de inicializá-las. Erros de digitação em nomes de variáveis (C# é case-sensitive). Esquecer de fechar chaves, parênteses ou aspas. Não entender escopo e tentar acessar variáveis fora dele. A boa notícia é que com prática, você cometerá menos desses erros. E quando cometer, reconhecerá e corrigirá rapidamente. Erros são parte normal do aprendizado - todo programador, mesmo experiente, comete erros.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Técnicas Básicas de Depuração",
                    Content = "Depuração (debugging) é o processo de encontrar e corrigir erros. A técnica mais simples é usar Console.WriteLine() para exibir valores de variáveis em pontos chave do código. Isso ajuda a entender o que está acontecendo. Verifique se variáveis têm os valores esperados. Use mensagens descritivas: Console.WriteLine('Valor de x antes do loop: ' + x). Comente partes do código para isolar o problema - se o erro desaparece, está naquela seção. Leia mensagens de erro cuidadosamente - elas indicam o tipo de erro e a linha. Use o debugger da IDE para executar código passo a passo e inspecionar variáveis. Teste com valores simples primeiro. Se um cálculo falha com números grandes, teste com 1, 2, 3. Divida problemas grandes em partes menores e teste cada parte. Explique o código em voz alta (rubber duck debugging) - frequentemente você encontra o erro ao explicar.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Depuração com Console.WriteLine",
                    Code = @"int[] numeros = {5, 10, 15, 20};
int soma = 0;

Console.WriteLine(""Iniciando cálculo da soma..."");

for (int i = 0; i < numeros.Length; i++)
{
    Console.WriteLine($""Iteração {i}: adicionando {numeros[i]}"");
    soma += numeros[i];
    Console.WriteLine($""Soma parcial: {soma}"");
}

Console.WriteLine($""Soma final: {soma}"");",
                    Language = "csharp",
                    Explanation = "Usa Console.WriteLine para rastrear o progresso do loop e valores das variáveis. Isso ajuda a entender o que o código está fazendo e identificar problemas.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Evitando Erro de Índice",
                    Code = @"int[] numeros = {10, 20, 30};

Console.WriteLine(""Tamanho do array: "" + numeros.Length);

// Correto: i < numeros.Length (não <=)
for (int i = 0; i < numeros.Length; i++)
{
    Console.WriteLine($""numeros[{i}] = {numeros[i]}"");
}

// Errado causaria: IndexOutOfRangeException
// for (int i = 0; i <= numeros.Length; i++)",
                    Language = "csharp",
                    Explanation = "Demonstra como evitar erro comum de acessar índice fora dos limites. Use < em vez de <= com Length.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Encontre o Erro de Sintaxe",
                    Description = "O código abaixo tem um erro de sintaxe. Identifique e corrija: int x = 10 Console.WriteLine(x);",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"// Corrija o erro
int x = 10
Console.WriteLine(x);",
                    Hints = new List<string> 
                    { 
                        "Falta ponto e vírgula após int x = 10",
                        "Toda instrução em C# termina com ;",
                        "O compilador indica a linha do erro" 
                    }
                },
                new Exercise
                {
                    Title = "Debug com Mensagens",
                    Description = "Adicione Console.WriteLine para debugar este código que calcula fatorial. Exiba o valor de resultado a cada iteração.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"int numero = 5;
int resultado = 1;

for (int i = 1; i <= numero; i++)
{
    resultado *= i;
    // Adicione debug aqui
}

Console.WriteLine(""Fatorial: "" + resultado);",
                    Hints = new List<string> 
                    { 
                        "Adicione Console.WriteLine dentro do loop",
                        "Exiba i e resultado a cada iteração",
                        "Isso ajuda a ver como o cálculo progride" 
                    }
                },
                new Exercise
                {
                    Title = "Corrija o Erro Lógico",
                    Description = "Este código deveria calcular a média mas está errado. Encontre e corrija o erro lógico.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"int[] notas = {8, 7, 9, 6};
int soma = 0;

for (int i = 0; i < notas.Length; i++)
{
    soma += notas[i];
}

double media = soma + notas.Length; // Erro aqui
Console.WriteLine(""Média: "" + media);",
                    Hints = new List<string> 
                    { 
                        "Média é soma dividida pela quantidade, não soma mais quantidade",
                        "Use / em vez de +",
                        "Converta para double se necessário" 
                    }
                }
            },
            Summary = "Nesta aula você aprendeu sobre erros e depuração. Erros são normais na programação - o importante é saber identificá-los e corrigi-los. Use Console.WriteLine, leia mensagens de erro e teste seu código sistematicamente. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno. Grandes empresas como Microsoft, Google e Amazon aplicam estes conceitos em seus sistemas. Ao aprender estas técnicas, você está adquirindo conhecimento usado por desenvolvedores em todo o mundo. Considere como este conceito poderia ser aplicado em um projeto pessoal. Pensar em aplicações práticas ajuda a solidificar o entendimento e torna o aprendizado mais significativo. Iniciantes frequentemente cometem alguns erros ao aprender este tópico. Conhecer estes erros comuns ajuda a evitá-los e desenvolver código mais robusto desde o início. Um erro comum é não considerar casos extremos ou situações inesperadas. Sempre teste seu código com diferentes tipos de entrada, incluindo valores nulos, vazios ou inválidos. Outro erro frequente é otimizar prematuramente. Primeiro faça o código funcionar corretamente, depois otimize se necessário. Como diz o ditado: 'premature optimization is the root of all evil'. Quando encontrar problemas, use o debugger para entender o que está acontecendo. Coloque breakpoints, inspecione variáveis e execute o código passo a passo. Esta é uma habilidade essencial para todo desenvolvedor. Mensagens de erro podem parecer intimidadoras no início, mas elas são suas aliadas. Leia-as cuidadosamente - elas geralmente indicam exatamente onde e qual é o problema. Se algo não funcionar como esperado, não desanime. Debugging é uma parte normal do desenvolvimento. Até desenvolvedores experientes passam tempo significativo depurando código. Testar seu código é tão importante quanto escrevê-lo. Testes automatizados garantem que seu código funciona corretamente e continuará funcionando quando você fizer mudanças. Comece escrevendo testes simples para os casos mais comuns. Depois adicione testes para casos extremos e situações de erro. Uma boa cobertura de testes dá confiança para refatorar e melhorar o código. Considere a prática de TDD (Test-Driven Development), onde você escreve os testes antes do código. Isso ajuda a pensar claramente sobre o que o código deve fazer. Embora a correção seja sempre a prioridade, é importante estar ciente das implicações de performance de suas escolhas. Diferentes abordagens podem ter custos computacionais muito diferentes. Em muitos casos, a diferença de performance é negligenciável para aplicações pequenas. No entanto, à medida que seus programas crescem, escolhas eficientes se tornam cada vez mais importantes. Aprenda a usar ferramentas de profiling para identificar gargalos de performance. Otimize baseado em dados reais, não em suposições sobre onde o código é lento. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0001-000000000015"),
            CourseId = _courseId,
            Title = "Introdução a Erros e Depuração",
            Duration = "50 min",
            Difficulty = "Iniciante",
            EstimatedMinutes = 50,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0001-000000000014" }),
            OrderIndex = 15,
            Content = "",
            StructuredContent = JsonSerializer.Serialize(content),
            Version = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    private Lesson CreateLesson16()
    {
        var content = new LessonContent
        {
            Objectives = new List<string>
            {
                "Entender comentários e sua importância",
                "Aprender boas práticas de documentação",
                "Escrever código legível e bem documentado"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Por que Comentar Código?",
                    Content = "Comentários são anotações no código que o compilador ignora mas que ajudam humanos a entender o que o código faz. Você escreve código uma vez mas o lê muitas vezes - comentários facilitam essa leitura. Eles explicam a intenção por trás do código, não apenas o que ele faz. Por exemplo, não escreva 'incrementa i' ao lado de i++, isso é óbvio. Escreva 'processa próximo cliente da fila' para explicar o propósito. Comentários são especialmente úteis para lógica complexa, decisões de design, e avisos sobre casos especiais. Eles ajudam outros programadores (e você mesmo no futuro) a entender o código rapidamente. Código sem comentários é difícil de manter. Mas cuidado: comentários desatualizados são piores que nenhum comentário, pois enganam. Mantenha comentários sincronizados com o código.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Tipos de Comentários em C#",
                    Content = "C# tem três tipos de comentários. Comentários de linha única começam com // e vão até o fim da linha: // Este é um comentário. Use para explicações breves. Comentários de múltiplas linhas começam com /* e terminam com */: /* Este comentário pode ocupar várias linhas */. Use para explicações longas ou para comentar blocos de código temporariamente. Comentários de documentação começam com /// e são usados antes de funções e classes: /// <summary>Calcula a soma de dois números</summary>. Esses geram documentação automática. Use // para comentários inline e explicações rápidas. Use /* */ para desabilitar temporariamente blocos de código durante testes. Use /// para documentar APIs e funções públicas. Comentários devem ser claros e concisos. Evite comentários óbvios que apenas repetem o código.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Boas Práticas de Comentários",
                    Content = "Comente o 'porquê', não o 'o quê'. O código já mostra o que faz; comentários devem explicar por que você fez dessa forma. Exemplo ruim: // soma a e b. Exemplo bom: // aplica desconto de 10% para clientes VIP. Use nomes de variáveis e funções descritivos para reduzir necessidade de comentários. int idadeUsuario é melhor que int x com comentário explicando. Comente decisões não óbvias: // usando algoritmo X porque Y é mais lento para grandes datasets. Comente casos especiais e limitações: // não funciona para números negativos. Mantenha comentários atualizados quando mudar o código. Delete comentários obsoletos. Evite comentar código ruim - refatore o código para ser claro. Use TODO: para marcar trabalho pendente: // TODO: adicionar validação de email. Seja profissional - evite comentários informais ou ofensivos em código de produção.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Comentários Úteis",
                    Code = @"// Calcula o preço final aplicando desconto progressivo
// Desconto aumenta com o valor da compra
double CalcularPrecoFinal(double precoOriginal)
{
    double desconto = 0;
    
    // Clientes que gastam mais recebem desconto maior
    if (precoOriginal >= 1000)
        desconto = 0.15; // 15% para compras acima de R$ 1000
    else if (precoOriginal >= 500)
        desconto = 0.10; // 10% para compras acima de R$ 500
    else if (precoOriginal >= 100)
        desconto = 0.05; // 5% para compras acima de R$ 100
    
    return precoOriginal * (1 - desconto);
}",
                    Language = "csharp",
                    Explanation = "Comentários explicam a lógica de negócio e as faixas de desconto. Isso ajuda a entender por que o código está estruturado assim.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Comentários de Documentação",
                    Code = @"/// <summary>
/// Calcula a área de um retângulo
/// </summary>
/// <param name=""baseRetangulo"">A base do retângulo em metros</param>
/// <param name=""altura"">A altura do retângulo em metros</param>
/// <returns>A área em metros quadrados</returns>
double CalcularAreaRetangulo(double baseRetangulo, double altura)
{
    return baseRetangulo * altura;
}

// Exemplo de uso
double area = CalcularAreaRetangulo(5.0, 3.0);
Console.WriteLine($""Área: {area} m²"");",
                    Language = "csharp",
                    Explanation = "Comentários /// documentam a função formalmente, explicando parâmetros e retorno. IDEs usam isso para mostrar ajuda ao usar a função.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Adicione Comentários",
                    Description = "Adicione comentários úteis a este código que calcula se um ano é bissexto.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"bool EhBissexto(int ano)
{
    if (ano % 400 == 0)
        return true;
    if (ano % 100 == 0)
        return false;
    if (ano % 4 == 0)
        return true;
    return false;
}",
                    Hints = new List<string> 
                    { 
                        "Explique as regras de ano bissexto",
                        "Comente cada condição",
                        "Adicione comentário de documentação no topo" 
                    }
                },
                new Exercise
                {
                    Title = "Melhore os Comentários",
                    Description = "Este código tem comentários ruins. Reescreva-os para serem mais úteis.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// soma
int Somar(int a, int b)
{
    // retorna
    return a + b; // a mais b
}",
                    Hints = new List<string> 
                    { 
                        "Comentários atuais apenas repetem o código",
                        "Explique o propósito da função, não o óbvio",
                        "Ou remova comentários desnecessários" 
                    }
                },
                new Exercise
                {
                    Title = "Documente uma Função",
                    Description = "Crie uma função que converte Celsius para Fahrenheit. Adicione comentários de documentação completos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Adicione documentação aqui
double CelsiusParaFahrenheit(double celsius)
{
    return celsius * 9.0 / 5.0 + 32.0;
}",
                    Hints = new List<string> 
                    { 
                        "Use /// <summary> para descrever a função",
                        "Use /// <param> para documentar o parâmetro",
                        "Use /// <returns> para documentar o retorno" 
                    }
                }
            },
            Summary = "Nesta aula você aprendeu sobre comentários e documentação de código. Comentários bem escritos tornam o código mais fácil de entender e manter. Comente o 'porquê', mantenha comentários atualizados, e use documentação formal para funções públicas. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno. Grandes empresas como Microsoft, Google e Amazon aplicam estes conceitos em seus sistemas. Ao aprender estas técnicas, você está adquirindo conhecimento usado por desenvolvedores em todo o mundo. Considere como este conceito poderia ser aplicado em um projeto pessoal. Pensar em aplicações práticas ajuda a solidificar o entendimento e torna o aprendizado mais significativo. Iniciantes frequentemente cometem alguns erros ao aprender este tópico. Conhecer estes erros comuns ajuda a evitá-los e desenvolver código mais robusto desde o início. Um erro comum é não considerar casos extremos ou situações inesperadas. Sempre teste seu código com diferentes tipos de entrada, incluindo valores nulos, vazios ou inválidos. Outro erro frequente é otimizar prematuramente. Primeiro faça o código funcionar corretamente, depois otimize se necessário. Como diz o ditado: 'premature optimization is the root of all evil'. Quando encontrar problemas, use o debugger para entender o que está acontecendo. Coloque breakpoints, inspecione variáveis e execute o código passo a passo. Esta é uma habilidade essencial para todo desenvolvedor. Mensagens de erro podem parecer intimidadoras no início, mas elas são suas aliadas. Leia-as cuidadosamente - elas geralmente indicam exatamente onde e qual é o problema. Se algo não funcionar como esperado, não desanime. Debugging é uma parte normal do desenvolvimento. Até desenvolvedores experientes passam tempo significativo depurando código. Testar seu código é tão importante quanto escrevê-lo. Testes automatizados garantem que seu código funciona corretamente e continuará funcionando quando você fizer mudanças. Comece escrevendo testes simples para os casos mais comuns. Depois adicione testes para casos extremos e situações de erro. Uma boa cobertura de testes dá confiança para refatorar e melhorar o código. Considere a prática de TDD (Test-Driven Development), onde você escreve os testes antes do código. Isso ajuda a pensar claramente sobre o que o código deve fazer. Embora a correção seja sempre a prioridade, é importante estar ciente das implicações de performance de suas escolhas. Diferentes abordagens podem ter custos computacionais muito diferentes. Em muitos casos, a diferença de performance é negligenciável para aplicações pequenas. No entanto, à medida que seus programas crescem, escolhas eficientes se tornam cada vez mais importantes. Aprenda a usar ferramentas de profiling para identificar gargalos de performance. Otimize baseado em dados reais, não em suposições sobre onde o código é lento. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0001-000000000016"),
            CourseId = _courseId,
            Title = "Comentários e Documentação",
            Duration = "45 min",
            Difficulty = "Iniciante",
            EstimatedMinutes = 45,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0001-000000000015" }),
            OrderIndex = 16,
            Content = "",
            StructuredContent = JsonSerializer.Serialize(content),
            Version = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }
}
