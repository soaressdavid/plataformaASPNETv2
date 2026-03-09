using Shared.Entities;
using Shared.Models;
using System.Text.Json;

namespace Shared.Data;

/// <summary>
/// Part 2 of Level 0 Content Seeder - Lessons 17-20
/// </summary>
public partial class Level0ContentSeeder
{
    private Lesson CreateLesson17()
    {
        var content = new LessonContent
        {
            Objectives = new List<string>
            {
                "Revisar conceitos fundamentais aprendidos",
                "Integrar múltiplos conceitos em um programa",
                "Preparar-se para o projeto final"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Revisão de Variáveis e Tipos",
                    Content = "Você aprendeu que variáveis armazenam dados e cada variável tem um tipo. Os tipos básicos são int (números inteiros), double (números decimais), string (texto), bool (verdadeiro/falso) e char (caractere único). Variáveis devem ser declaradas antes de usar: tipo nome = valor. Você pode modificar valores de variáveis durante a execução. Tipos determinam que operações são possíveis - você pode somar números mas não strings (sem converter). Conversão entre tipos usa Parse() ou cast. Escolher o tipo certo é importante para eficiência e correção. Variáveis locais existem apenas em seu escopo. Nomes descritivos tornam código mais legível. Esses conceitos são fundamentais para toda programação - você os usará constantemente. Pratique declarar e usar diferentes tipos de variáveis até se sentir confortável.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Revisão de Controle de Fluxo e Loops",
                    Content = "Controle de fluxo permite que programas tomem decisões. If-else executa código diferente baseado em condições. Switch é útil para múltiplas opções discretas. Loops repetem código: while verifica condição antes, do-while garante uma execução, for é ideal para contadores. Operadores de comparação (==, !=, >, <) e lógicos (&&, ||, !) criam condições. Break sai de loops, continue pula para próxima iteração. Loops aninhados permitem estruturas complexas. Sempre garanta que loops tenham condição de parada para evitar loops infinitos. Controle de fluxo e loops são essenciais para criar programas dinâmicos que respondem a diferentes situações e processam coleções de dados. Combine-os com funções para criar programas poderosos e organizados.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Revisão de Funções e Arrays",
                    Content = "Funções encapsulam código reutilizável. Elas têm nome, parâmetros (entrada) e tipo de retorno (saída). Funções void não retornam valor. Sobrecarga permite múltiplas versões com diferentes parâmetros. Funções tornam código modular e fácil de manter. Arrays armazenam coleções de elementos do mesmo tipo. Acesse elementos por índice (começando em 0). Use Length para tamanho. Itere com for ou foreach. Arrays têm tamanho fixo. Strings são sequências de caracteres com muitos métodos úteis: ToUpper, ToLower, Substring, Replace, Split. Combine funções e arrays para processar coleções de dados eficientemente. Esses conceitos formam a base da programação estruturada. Com eles, você pode criar programas complexos e úteis. O projeto final integrará todos esses conceitos.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Integrando Conceitos",
                    Code = @"// Função que processa array de notas
double CalcularMediaAprovados(double[] notas)
{
    double soma = 0;
    int aprovados = 0;
    
    // Loop para processar cada nota
    foreach (double nota in notas)
    {
        // Controle de fluxo para filtrar aprovados
        if (nota >= 7.0)
        {
            soma += nota;
            aprovados++;
        }
    }
    
    // Retorna média ou 0 se nenhum aprovado
    return aprovados > 0 ? soma / aprovados : 0;
}

double[] notas = {8.5, 6.0, 7.5, 9.0, 5.5, 8.0};
double media = CalcularMediaAprovados(notas);
Console.WriteLine($""Média dos aprovados: {media:F2}"");",
                    Language = "csharp",
                    Explanation = "Este exemplo integra funções, arrays, loops e controle de fluxo. Demonstra como combinar conceitos para resolver problemas reais.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Programa Completo",
                    Code = @"// Função para verificar se número é primo
bool EhPrimo(int numero)
{
    if (numero < 2) return false;
    
    for (int i = 2; i <= Math.Sqrt(numero); i++)
    {
        if (numero % i == 0)
            return false;
    }
    
    return true;
}

// Encontra primos em um intervalo
Console.WriteLine(""Números primos de 1 a 20:"");
for (int num = 1; num <= 20; num++)
{
    if (EhPrimo(num))
    {
        Console.Write(num + "" "");
    }
}",
                    Language = "csharp",
                    Explanation = "Programa completo que usa função, loops, condicionais e operadores para encontrar números primos. Mostra como conceitos trabalham juntos.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Processador de Array",
                    Description = "Crie uma função que recebe um array de inteiros e retorna quantos são pares. Use loop e condicional.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"int ContarPares(int[] numeros)
{
    // Implemente aqui
    return 0;
}

int[] teste = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10};
Console.WriteLine(""Pares: "" + ContarPares(teste));",
                    Hints = new List<string> 
                    { 
                        "Use foreach ou for para iterar",
                        "Use if (numero % 2 == 0) para verificar par",
                        "Conte e retorne o total" 
                    }
                },
                new Exercise
                {
                    Title = "Validador de Senha",
                    Description = "Crie função que valida senha: mínimo 8 caracteres, contém número e letra maiúscula. Retorne bool.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"bool ValidarSenha(string senha)
{
    // Implemente a validação
    return false;
}

Console.WriteLine(ValidarSenha(""Senha123"")); // true
Console.WriteLine(ValidarSenha(""senha"")); // false",
                    Hints = new List<string> 
                    { 
                        "Verifique senha.Length >= 8",
                        "Use loop para verificar se contém número e maiúscula",
                        "Combine condições com &&" 
                    }
                },
                new Exercise
                {
                    Title = "Estatísticas de Array",
                    Description = "Crie função que recebe array de doubles e exibe: soma, média, maior e menor valor.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"void ExibirEstatisticas(double[] valores)
{
    // Calcule e exiba as estatísticas
}

double[] dados = {5.5, 3.2, 8.7, 2.1, 9.3};
ExibirEstatisticas(dados);",
                    Hints = new List<string> 
                    { 
                        "Use loop para calcular soma",
                        "Média = soma / valores.Length",
                        "Inicialize maior e menor com valores[0]" 
                    }
                }
            },
            Summary = "Nesta aula você revisou todos os conceitos fundamentais: variáveis, tipos, operadores, controle de fluxo, loops, funções, arrays e strings. Esses são os blocos de construção da programação. Agora você está pronto para o projeto final! Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno. Grandes empresas como Microsoft, Google e Amazon aplicam estes conceitos em seus sistemas. Ao aprender estas técnicas, você está adquirindo conhecimento usado por desenvolvedores em todo o mundo. Considere como este conceito poderia ser aplicado em um projeto pessoal. Pensar em aplicações práticas ajuda a solidificar o entendimento e torna o aprendizado mais significativo. Iniciantes frequentemente cometem alguns erros ao aprender este tópico. Conhecer estes erros comuns ajuda a evitá-los e desenvolver código mais robusto desde o início. Um erro comum é não considerar casos extremos ou situações inesperadas. Sempre teste seu código com diferentes tipos de entrada, incluindo valores nulos, vazios ou inválidos. Outro erro frequente é otimizar prematuramente. Primeiro faça o código funcionar corretamente, depois otimize se necessário. Como diz o ditado: 'premature optimization is the root of all evil'. Quando encontrar problemas, use o debugger para entender o que está acontecendo. Coloque breakpoints, inspecione variáveis e execute o código passo a passo. Esta é uma habilidade essencial para todo desenvolvedor. Mensagens de erro podem parecer intimidadoras no início, mas elas são suas aliadas. Leia-as cuidadosamente - elas geralmente indicam exatamente onde e qual é o problema. Se algo não funcionar como esperado, não desanime. Debugging é uma parte normal do desenvolvimento. Até desenvolvedores experientes passam tempo significativo depurando código. Testar seu código é tão importante quanto escrevê-lo. Testes automatizados garantem que seu código funciona corretamente e continuará funcionando quando você fizer mudanças. Comece escrevendo testes simples para os casos mais comuns. Depois adicione testes para casos extremos e situações de erro. Uma boa cobertura de testes dá confiança para refatorar e melhorar o código. Considere a prática de TDD (Test-Driven Development), onde você escreve os testes antes do código. Isso ajuda a pensar claramente sobre o que o código deve fazer. Embora a correção seja sempre a prioridade, é importante estar ciente das implicações de performance de suas escolhas. Diferentes abordagens podem ter custos computacionais muito diferentes. Em muitos casos, a diferença de performance é negligenciável para aplicações pequenas. No entanto, à medida que seus programas crescem, escolhas eficientes se tornam cada vez mais importantes. Aprenda a usar ferramentas de profiling para identificar gargalos de performance. Otimize baseado em dados reais, não em suposições sobre onde o código é lento. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0001-000000000017"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-000000000001"),
            Title = "Revisão de Conceitos Fundamentais",
            Duration = "60 min",
            Difficulty = "Iniciante",
            EstimatedMinutes = 60,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0001-000000000016" }),
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
                "Planejar um projeto de software",
                "Entender requisitos do projeto calculadora",
                "Aplicar boas práticas de desenvolvimento"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Planejamento de Projeto",
                    Content = "Antes de escrever código, é importante planejar. Planejamento economiza tempo e evita retrabalho. Primeiro, entenda os requisitos: o que o programa deve fazer? Para nossa calculadora: realizar operações básicas (+, -, *, /), aceitar entrada do usuário, exibir resultados, permitir múltiplas operações, ter opção de sair. Segundo, divida o problema em partes menores. Nossa calculadora precisa de: função para exibir menu, funções para cada operação, loop principal para continuar até usuário sair, validação de entrada. Terceiro, pense na estrutura de dados: que variáveis precisamos? Quarto, considere casos especiais: divisão por zero, entrada inválida. Quinto, esboce o fluxo do programa: exibir menu, ler escolha, executar operação, exibir resultado, repetir. Planejamento não precisa ser perfeito - você ajustará durante o desenvolvimento. Mas ter um plano inicial torna o desenvolvimento mais suave.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Requisitos da Calculadora",
                    Content = "Nossa calculadora console terá estas funcionalidades: Menu com opções: 1-Somar, 2-Subtrair, 3-Multiplicar, 4-Dividir, 5-Sair. Para cada operação, pedir dois números ao usuário. Realizar o cálculo e exibir o resultado. Tratar divisão por zero com mensagem de erro. Validar entrada do usuário (verificar se é número válido). Permitir realizar múltiplas operações sem reiniciar o programa. Interface amigável com mensagens claras. Código organizado em funções para cada responsabilidade. Comentários explicando lógica importante. Tratamento de erros básico. O programa deve ser robusto - não travar com entrada inválida. Deve ser fácil de usar - usuário não precisa ler manual. Deve ser fácil de manter - código limpo e bem estruturado. Esses requisitos guiarão nosso desenvolvimento nas próximas aulas.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Boas Práticas para o Projeto",
                    Content = "Ao desenvolver a calculadora, siga estas boas práticas: Use nomes descritivos para variáveis e funções. Mantenha funções pequenas e focadas - cada função faz uma coisa. Comente código não óbvio, especialmente decisões de design. Teste cada função isoladamente antes de integrar. Trate erros graciosamente - não deixe o programa travar. Valide toda entrada do usuário - nunca confie que será válida. Use constantes para valores fixos (como opções do menu). Organize código logicamente - funções relacionadas juntas. Evite duplicação - se código se repete, crie uma função. Pense no usuário - mensagens claras e interface intuitiva. Desenvolva incrementalmente - comece simples e adicione funcionalidades. Teste frequentemente - não espere terminar tudo para testar. Refatore quando necessário - melhore código que ficou confuso. Essas práticas resultam em código de qualidade profissional.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Estrutura Básica do Menu",
                    Code = @"void ExibirMenu()
{
    Console.WriteLine(""\n=== CALCULADORA ==="" );
    Console.WriteLine(""1. Somar"");
    Console.WriteLine(""2. Subtrair"");
    Console.WriteLine(""3. Multiplicar"");
    Console.WriteLine(""4. Dividir"");
    Console.WriteLine(""5. Sair"");
    Console.Write(""Escolha uma opção: "");
}

// Exemplo de uso
ExibirMenu();",
                    Language = "csharp",
                    Explanation = "Função simples e focada que exibe o menu. Separar em função torna o código mais organizado e reutilizável.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Função de Operação Básica",
                    Code = @"double Somar(double a, double b)
{
    return a + b;
}

double Subtrair(double a, double b)
{
    return a - b;
}

// Teste das funções
Console.WriteLine(""10 + 5 = "" + Somar(10, 5));
Console.WriteLine(""10 - 5 = "" + Subtrair(10, 5));",
                    Language = "csharp",
                    Explanation = "Funções simples para operações. Cada função tem uma responsabilidade clara. Fácil de testar e manter.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Planeje Suas Funções",
                    Description = "Liste todas as funções que você precisará para a calculadora. Para cada uma, escreva o que ela faz (não precisa implementar ainda).",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"// Liste as funções necessárias como comentários
// Exemplo:
// ExibirMenu() - exibe opções para o usuário
// Somar(a, b) - retorna soma de dois números
// ...",
                    Hints = new List<string> 
                    { 
                        "Pense em: menu, operações, validação, loop principal",
                        "Cada função deve ter uma responsabilidade clara",
                        "Considere funções auxiliares para tarefas comuns" 
                    }
                },
                new Exercise
                {
                    Title = "Crie Funções de Operação",
                    Description = "Implemente as quatro funções de operação: Somar, Subtrair, Multiplicar, Dividir. Teste cada uma.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"// Implemente as quatro funções

// Teste todas
",
                    Hints = new List<string> 
                    { 
                        "Cada função recebe dois doubles e retorna double",
                        "Divisão deve verificar se divisor != 0",
                        "Teste com valores positivos, negativos e zero" 
                    }
                },
                new Exercise
                {
                    Title = "Função de Validação",
                    Description = "Crie uma função que tenta converter string em double. Retorne true se sucesso, false se falha.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"bool TentarConverterNumero(string texto, out double resultado)
{
    // Implemente usando double.TryParse
    resultado = 0;
    return false;
}",
                    Hints = new List<string> 
                    { 
                        "Use double.TryParse(texto, out resultado)",
                        "TryParse retorna true se conversão funcionar",
                        "out permite retornar o valor convertido" 
                    }
                }
            },
            Summary = "Nesta aula você aprendeu a planejar um projeto de software. Planejamento é crucial para desenvolvimento eficiente. Você entendeu os requisitos da calculadora e boas práticas a seguir. Nas próximas aulas, implementaremos o projeto completo! Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno. Grandes empresas como Microsoft, Google e Amazon aplicam estes conceitos em seus sistemas. Ao aprender estas técnicas, você está adquirindo conhecimento usado por desenvolvedores em todo o mundo. Considere como este conceito poderia ser aplicado em um projeto pessoal. Pensar em aplicações práticas ajuda a solidificar o entendimento e torna o aprendizado mais significativo. Iniciantes frequentemente cometem alguns erros ao aprender este tópico. Conhecer estes erros comuns ajuda a evitá-los e desenvolver código mais robusto desde o início. Um erro comum é não considerar casos extremos ou situações inesperadas. Sempre teste seu código com diferentes tipos de entrada, incluindo valores nulos, vazios ou inválidos. Outro erro frequente é otimizar prematuramente. Primeiro faça o código funcionar corretamente, depois otimize se necessário. Como diz o ditado: 'premature optimization is the root of all evil'. Quando encontrar problemas, use o debugger para entender o que está acontecendo. Coloque breakpoints, inspecione variáveis e execute o código passo a passo. Esta é uma habilidade essencial para todo desenvolvedor. Mensagens de erro podem parecer intimidadoras no início, mas elas são suas aliadas. Leia-as cuidadosamente - elas geralmente indicam exatamente onde e qual é o problema. Se algo não funcionar como esperado, não desanime. Debugging é uma parte normal do desenvolvimento. Até desenvolvedores experientes passam tempo significativo depurando código. Testar seu código é tão importante quanto escrevê-lo. Testes automatizados garantem que seu código funciona corretamente e continuará funcionando quando você fizer mudanças. Comece escrevendo testes simples para os casos mais comuns. Depois adicione testes para casos extremos e situações de erro. Uma boa cobertura de testes dá confiança para refatorar e melhorar o código. Considere a prática de TDD (Test-Driven Development), onde você escreve os testes antes do código. Isso ajuda a pensar claramente sobre o que o código deve fazer. Embora a correção seja sempre a prioridade, é importante estar ciente das implicações de performance de suas escolhas. Diferentes abordagens podem ter custos computacionais muito diferentes. Em muitos casos, a diferença de performance é negligenciável para aplicações pequenas. No entanto, à medida que seus programas crescem, escolhas eficientes se tornam cada vez mais importantes. Aprenda a usar ferramentas de profiling para identificar gargalos de performance. Otimize baseado em dados reais, não em suposições sobre onde o código é lento. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0001-000000000018"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-000000000001"),
            Title = "Projeto: Planejamento da Calculadora",
            Duration = "50 min",
            Difficulty = "Iniciante",
            EstimatedMinutes = 50,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0001-000000000017" }),
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
                "Implementar a calculadora console completa",
                "Integrar todos os conceitos aprendidos",
                "Criar um programa funcional e robusto"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Implementando o Loop Principal",
                    Content = "O loop principal é o coração da calculadora. Ele mantém o programa rodando até o usuário escolher sair. Use um loop do-while porque queremos exibir o menu pelo menos uma vez. Dentro do loop: exiba o menu, leia a escolha do usuário, use switch para processar a escolha, execute a operação correspondente, exiba o resultado. A condição do loop verifica se o usuário escolheu sair. Estrutura básica: do { ExibirMenu(); int opcao = LerOpcao(); if (opcao == 5) break; ProcessarOpcao(opcao); } while (true). O break sai do loop quando usuário escolhe sair. Separe responsabilidades em funções - não coloque toda lógica no loop. Isso torna o código mais limpo e fácil de entender. O loop principal deve ser simples e delegar trabalho para funções especializadas.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Tratamento de Entrada do Usuário",
                    Content = "Entrada do usuário é imprevisível - sempre valide. Para ler números, use double.TryParse() em vez de double.Parse(). TryParse retorna false se conversão falhar, evitando exceções. Exemplo: if (!double.TryParse(Console.ReadLine(), out double numero)) { Console.WriteLine('Entrada inválida'); }. Para opções do menu, valide se está no intervalo correto (1-5). Se inválida, exiba mensagem e peça novamente. Use loops para repetir até entrada válida: double LerNumero() { while (true) { if (double.TryParse(Console.ReadLine(), out double num)) return num; Console.WriteLine('Inválido. Tente novamente:'); } }. Sempre dê feedback claro ao usuário sobre o que deu errado. Nunca deixe o programa travar por entrada inválida. Validação robusta é marca de software profissional.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Finalizando e Testando",
                    Content = "Após implementar todas as funcionalidades, teste extensivamente. Teste cada operação com diferentes valores: positivos, negativos, zero, decimais. Teste divisão por zero - deve exibir erro, não travar. Teste entrada inválida - letras em vez de números, opções de menu inválidas. Teste o fluxo completo - realizar múltiplas operações e sair. Teste casos extremos - números muito grandes, muito pequenos. Se encontrar bugs, corrija e teste novamente. Adicione comentários finais explicando partes complexas. Revise o código - há duplicação? Funções muito longas? Nomes confusos? Refatore se necessário. Peça feedback de outros - eles conseguem usar sem instruções? A interface é clara? Celebre - você criou um programa completo! Este projeto demonstra que você domina os fundamentos da programação. Você está pronto para desafios mais avançados!",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Calculadora Completa - Parte 1",
                    Code = @"// Funções de operação
double Somar(double a, double b) => a + b;
double Subtrair(double a, double b) => a - b;
double Multiplicar(double a, double b) => a * b;

double Dividir(double a, double b)
{
    if (b == 0)
    {
        Console.WriteLine(""Erro: Divisão por zero!"");
        return 0;
    }
    return a / b;
}

void ExibirMenu()
{
    Console.WriteLine(""\n=== CALCULADORA ==="" );
    Console.WriteLine(""1. Somar"");
    Console.WriteLine(""2. Subtrair"");
    Console.WriteLine(""3. Multiplicar"");
    Console.WriteLine(""4. Dividir"");
    Console.WriteLine(""5. Sair"");
    Console.Write(""Escolha: "");
}",
                    Language = "csharp",
                    Explanation = "Funções básicas da calculadora. Cada operação é uma função simples. Divisão trata o caso especial de divisor zero.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Calculadora Completa - Parte 2",
                    Code = @"double LerNumero(string mensagem)
{
    while (true)
    {
        Console.Write(mensagem);
        if (double.TryParse(Console.ReadLine(), out double numero))
            return numero;
        Console.WriteLine(""Entrada inválida. Tente novamente."");
    }
}

void ExecutarOperacao(int opcao)
{
    double a = LerNumero(""Primeiro número: "");
    double b = LerNumero(""Segundo número: "");
    double resultado = 0;
    
    switch (opcao)
    {
        case 1: resultado = Somar(a, b); break;
        case 2: resultado = Subtrair(a, b); break;
        case 3: resultado = Multiplicar(a, b); break;
        case 4: resultado = Dividir(a, b); break;
    }
    
    Console.WriteLine($""Resultado: {resultado}"");
}",
                    Language = "csharp",
                    Explanation = "Funções auxiliares para ler entrada e executar operações. LerNumero valida entrada, ExecutarOperacao coordena o processo.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Implemente o Loop Principal",
                    Description = "Crie o loop principal que exibe menu, lê opção, executa operação e repete até usuário sair.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o loop principal aqui
// Use do-while
// Chame ExibirMenu, leia opção, processe com switch
",
                    Hints = new List<string> 
                    { 
                        "Use do-while para garantir execução inicial",
                        "Leia opção com int.TryParse",
                        "Use switch para processar opções 1-5" 
                    }
                },
                new Exercise
                {
                    Title = "Adicione Validação Completa",
                    Description = "Melhore a calculadora adicionando validação para todas as entradas. Trate todos os casos de erro.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Adicione validação robusta
// Verifique opção do menu (1-5)
// Valide números com TryParse
// Trate divisão por zero
",
                    Hints = new List<string> 
                    { 
                        "Crie função LerOpcaoValida() que só retorna 1-5",
                        "Use loops para repetir até entrada válida",
                        "Exiba mensagens claras de erro" 
                    }
                },
                new Exercise
                {
                    Title = "Teste Extensivamente",
                    Description = "Teste sua calculadora com todos os casos: operações normais, divisão por zero, entradas inválidas, múltiplas operações.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Execute sua calculadora e teste:
// 1. Todas as operações com números normais
// 2. Divisão por zero
// 3. Entrada de texto em vez de número
// 4. Opção de menu inválida
// 5. Múltiplas operações seguidas
// 6. Sair do programa",
                    Hints = new List<string> 
                    { 
                        "Anote os resultados de cada teste",
                        "Corrija bugs encontrados",
                        "Teste novamente após correções" 
                    }
                }
            },
            Summary = "Nesta aula você implementou uma calculadora console completa! Você integrou variáveis, tipos, operadores, controle de fluxo, loops, funções e validação. Este projeto demonstra que você domina os fundamentos da programação. Parabéns! Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno. Grandes empresas como Microsoft, Google e Amazon aplicam estes conceitos em seus sistemas. Ao aprender estas técnicas, você está adquirindo conhecimento usado por desenvolvedores em todo o mundo. Considere como este conceito poderia ser aplicado em um projeto pessoal. Pensar em aplicações práticas ajuda a solidificar o entendimento e torna o aprendizado mais significativo. Iniciantes frequentemente cometem alguns erros ao aprender este tópico. Conhecer estes erros comuns ajuda a evitá-los e desenvolver código mais robusto desde o início. Um erro comum é não considerar casos extremos ou situações inesperadas. Sempre teste seu código com diferentes tipos de entrada, incluindo valores nulos, vazios ou inválidos. Outro erro frequente é otimizar prematuramente. Primeiro faça o código funcionar corretamente, depois otimize se necessário. Como diz o ditado: 'premature optimization is the root of all evil'. Quando encontrar problemas, use o debugger para entender o que está acontecendo. Coloque breakpoints, inspecione variáveis e execute o código passo a passo. Esta é uma habilidade essencial para todo desenvolvedor. Mensagens de erro podem parecer intimidadoras no início, mas elas são suas aliadas. Leia-as cuidadosamente - elas geralmente indicam exatamente onde e qual é o problema. Se algo não funcionar como esperado, não desanime. Debugging é uma parte normal do desenvolvimento. Até desenvolvedores experientes passam tempo significativo depurando código. Testar seu código é tão importante quanto escrevê-lo. Testes automatizados garantem que seu código funciona corretamente e continuará funcionando quando você fizer mudanças. Comece escrevendo testes simples para os casos mais comuns. Depois adicione testes para casos extremos e situações de erro. Uma boa cobertura de testes dá confiança para refatorar e melhorar o código. Considere a prática de TDD (Test-Driven Development), onde você escreve os testes antes do código. Isso ajuda a pensar claramente sobre o que o código deve fazer. Embora a correção seja sempre a prioridade, é importante estar ciente das implicações de performance de suas escolhas. Diferentes abordagens podem ter custos computacionais muito diferentes. Em muitos casos, a diferença de performance é negligenciável para aplicações pequenas. No entanto, à medida que seus programas crescem, escolhas eficientes se tornam cada vez mais importantes. Aprenda a usar ferramentas de profiling para identificar gargalos de performance. Otimize baseado em dados reais, não em suposições sobre onde o código é lento. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0001-000000000019"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-000000000001"),
            Title = "Projeto: Implementação da Calculadora",
            Duration = "90 min",
            Difficulty = "Iniciante",
            EstimatedMinutes = 90,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0001-000000000018" }),
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
                "Revisar a jornada de aprendizado",
                "Entender próximos passos",
                "Celebrar conquistas e planejar futuro"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Sua Jornada até Aqui",
                    Content = "Parabéns por completar o Nível 0! Você começou sem saber nada de programação e agora pode criar programas funcionais. Você aprendeu conceitos fundamentais que são a base de toda programação: variáveis para armazenar dados, tipos para definir que dados são válidos, operadores para manipular dados, controle de fluxo para tomar decisões, loops para repetir ações, funções para organizar código, arrays para coleções de dados, strings para trabalhar com texto. Você também aprendeu habilidades essenciais: depuração para encontrar erros, documentação para explicar código, planejamento para estruturar projetos. E mais importante: você desenvolveu pensamento computacional - a habilidade de decompor problemas em passos lógicos. Essas habilidades são transferíveis - você as usará em qualquer linguagem de programação. Você não é mais um iniciante absoluto. Você é um programador!",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "O que Vem a Seguir",
                    Content = "O Nível 0 cobriu fundamentos de programação. O Nível 1 focará em C# especificamente: sintaxe avançada, coleções (List, Dictionary), manipulação de arquivos, tratamento de exceções, LINQ. Você aprenderá recursos poderosos que tornam C# uma linguagem moderna e produtiva. Nível 2 introduzirá Programação Orientada a Objetos (POO): classes, objetos, herança, polimorfismo, interfaces. POO é fundamental para desenvolvimento profissional. Níveis seguintes cobrem tópicos avançados: estruturas de dados, algoritmos, bancos de dados, desenvolvimento web, APIs, microsserviços, cloud. Cada nível constrói sobre o anterior. O caminho pode parecer longo, mas você já deu o passo mais difícil - começar. Continue praticando, seja paciente consigo mesmo, e celebre cada conquista. Programação é uma jornada, não um destino.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Dicas para Continuar Aprendendo",
                    Content = "Para consolidar seu aprendizado: Pratique regularmente - mesmo 30 minutos por dia fazem diferença. Crie projetos pessoais - aplique o que aprendeu em algo que te interessa. Leia código de outros - veja como programadores experientes resolvem problemas. Participe de comunidades - fóruns, Discord, grupos locais. Não tenha medo de errar - erros são oportunidades de aprendizado. Quando travar em um problema, tente por 20 minutos, depois peça ajuda. Ensine outros - explicar conceitos solidifica seu entendimento. Mantenha-se atualizado - tecnologia evolui constantemente. Seja curioso - explore além do currículo. Cuide da saúde - programação é maratona, não sprint. Faça pausas, exercite-se, durma bem. Celebre pequenas vitórias - cada bug corrigido, cada programa funcionando é uma conquista. Você tem potencial ilimitado. Continue aprendendo, continue crescendo. O mundo da programação está aberto para você!",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Seu Primeiro Programa Revisitado",
                    Code = @"// Lembra do seu primeiro programa?
Console.WriteLine(""Olá, Mundo!"");

// Agora você pode fazer muito mais!
string nome = ""Programador"";
int diasEstudando = 30;
double horasPorDia = 2.5;
double totalHoras = diasEstudando * horasPorDia;

Console.WriteLine($""Olá, {nome}!"");
Console.WriteLine($""Você estudou {totalHoras} horas!"");
Console.WriteLine(""Continue assim e você irá longe!"");",
                    Language = "csharp",
                    Explanation = "Compare este código com seu primeiro 'Olá, Mundo'. Veja quanto você aprendeu! Agora você usa variáveis, tipos, operadores e interpolação naturalmente.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Desafio Final",
                    Code = @"// Desafio: Crie um programa que gera tabuada de qualquer número
void GerarTabuada(int numero)
{
    Console.WriteLine($""\n=== Tabuada do {numero} ==="" );
    for (int i = 1; i <= 10; i++)
    {
        Console.WriteLine($""{numero} x {i} = {numero * i}"");
    }
}

// Teste com diferentes números
GerarTabuada(7);
GerarTabuada(12);

// Você consegue melhorar? Adicione:
// - Permitir usuário escolher o número
// - Permitir escolher até que número multiplicar
// - Adicionar outras operações (divisão, potência)",
                    Language = "csharp",
                    Explanation = "Este desafio usa tudo que você aprendeu. Tente melhorá-lo! Adicione funcionalidades, torne-o mais interativo. Seja criativo!",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Reflexão Pessoal",
                    Description = "Escreva (em comentários) três coisas que você aprendeu, três desafios que superou, e três metas para o próximo nível.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"// O que aprendi:
// 1. 
// 2. 
// 3. 

// Desafios que superei:
// 1. 
// 2. 
// 3. 

// Metas para o Nível 1:
// 1. 
// 2. 
// 3. ",
                    Hints = new List<string> 
                    { 
                        "Seja honesto e específico",
                        "Celebre suas conquistas",
                        "Defina metas realistas e mensuráveis" 
                    }
                },
                new Exercise
                {
                    Title = "Projeto Livre",
                    Description = "Crie um programa de sua escolha usando os conceitos aprendidos. Ideias: conversor de unidades, jogo de adivinhação, gerenciador de tarefas simples.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Crie seu projeto aqui
// Seja criativo!
// Use funções, loops, arrays, validação
// Faça algo que te interessa
",
                    Hints = new List<string> 
                    { 
                        "Comece simples e adicione funcionalidades",
                        "Planeje antes de codificar",
                        "Teste frequentemente" 
                    }
                },
                new Exercise
                {
                    Title = "Melhore a Calculadora",
                    Description = "Adicione funcionalidades à calculadora: histórico de operações, mais operações (potência, raiz), modo científico.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Pegue sua calculadora do Lesson 19
// Adicione novas funcionalidades
// Ideias:
// - Array para armazenar histórico
// - Função para exibir histórico
// - Novas operações matemáticas
// - Modo de operação contínua (usar resultado anterior)
",
                    Hints = new List<string> 
                    { 
                        "Use List<string> para histórico",
                        "Math.Pow() para potência, Math.Sqrt() para raiz",
                        "Adicione novas opções ao menu" 
                    }
                }
            },
            Summary = "Parabéns por completar o Nível 0 - Fundamentos de Programação! Você aprendeu os conceitos essenciais e criou programas funcionais. Você desenvolveu habilidades que usará por toda sua carreira. Continue praticando, continue aprendendo, continue crescendo. O futuro é brilhante para você. Nos vemos no Nível 1! Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno. Grandes empresas como Microsoft, Google e Amazon aplicam estes conceitos em seus sistemas. Ao aprender estas técnicas, você está adquirindo conhecimento usado por desenvolvedores em todo o mundo. Considere como este conceito poderia ser aplicado em um projeto pessoal. Pensar em aplicações práticas ajuda a solidificar o entendimento e torna o aprendizado mais significativo. Iniciantes frequentemente cometem alguns erros ao aprender este tópico. Conhecer estes erros comuns ajuda a evitá-los e desenvolver código mais robusto desde o início. Um erro comum é não considerar casos extremos ou situações inesperadas. Sempre teste seu código com diferentes tipos de entrada, incluindo valores nulos, vazios ou inválidos. Outro erro frequente é otimizar prematuramente. Primeiro faça o código funcionar corretamente, depois otimize se necessário. Como diz o ditado: 'premature optimization is the root of all evil'. Quando encontrar problemas, use o debugger para entender o que está acontecendo. Coloque breakpoints, inspecione variáveis e execute o código passo a passo. Esta é uma habilidade essencial para todo desenvolvedor. Mensagens de erro podem parecer intimidadoras no início, mas elas são suas aliadas. Leia-as cuidadosamente - elas geralmente indicam exatamente onde e qual é o problema. Se algo não funcionar como esperado, não desanime. Debugging é uma parte normal do desenvolvimento. Até desenvolvedores experientes passam tempo significativo depurando código. Testar seu código é tão importante quanto escrevê-lo. Testes automatizados garantem que seu código funciona corretamente e continuará funcionando quando você fizer mudanças. Comece escrevendo testes simples para os casos mais comuns. Depois adicione testes para casos extremos e situações de erro. Uma boa cobertura de testes dá confiança para refatorar e melhorar o código. Considere a prática de TDD (Test-Driven Development), onde você escreve os testes antes do código. Isso ajuda a pensar claramente sobre o que o código deve fazer. Embora a correção seja sempre a prioridade, é importante estar ciente das implicações de performance de suas escolhas. Diferentes abordagens podem ter custos computacionais muito diferentes. Em muitos casos, a diferença de performance é negligenciável para aplicações pequenas. No entanto, à medida que seus programas crescem, escolhas eficientes se tornam cada vez mais importantes. Aprenda a usar ferramentas de profiling para identificar gargalos de performance. Otimize baseado em dados reais, não em suposições sobre onde o código é lento. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0001-000000000020"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-000000000001"),
            Title = "Conclusão e Próximos Passos",
            Duration = "45 min",
            Difficulty = "Iniciante",
            EstimatedMinutes = 45,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0001-000000000019" }),
            OrderIndex = 20,
            Content = "",
            StructuredContent = JsonSerializer.Serialize(content),
            Version = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }
}
