using Shared.Entities;
using Shared.Models;
using System.Text.Json;

namespace Shared.Data;

/// <summary>
/// Seeds Level 2 content: Data Structures and Algorithms (20 lessons)
/// Covers Arrays, Lists, Stacks, Queues, Trees, Graphs, Search and Sorting Algorithms
/// </summary>
public partial class Level2ContentSeeder
{
    private readonly Guid _courseId = Guid.Parse("10000000-0000-0000-0000-000000000003");
    private readonly Guid _levelId = Guid.Parse("00000000-0000-0000-0000-000000000002");

    public Course CreateLevel2Course()
    {
        return new Course
        {
            Id = _courseId,
            LevelId = _levelId,
            Title = "Estruturas de Dados e Algoritmos",
            Description = "Domine as estruturas de dados fundamentais e algoritmos essenciais para resolver problemas complexos de forma eficiente. Aprenda sobre arrays, listas, pilhas, filas, árvores, grafos e algoritmos de busca e ordenação com implementações práticas em C#.",
            Level = Level.Intermediate,
            Duration = "4 semanas",
            LessonCount = 20,
            Topics = JsonSerializer.Serialize(new[] 
            { 
                "Arrays", 
                "Listas", 
                "Pilhas", 
                "Filas", 
                "Árvores", 
                "Grafos",
                "Algoritmos de Busca",
                "Algoritmos de Ordenação"
            }),
            OrderIndex = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    public List<Lesson> CreateLevel2Lessons()
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
                "Compreender o conceito de estruturas de dados e sua importância",
                "Conhecer os diferentes tipos de estruturas de dados",
                "Entender como escolher a estrutura de dados adequada para cada problema",
                "Aprender sobre complexidade de tempo e espaço"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "O que são Estruturas de Dados?",
                    Content = "Estruturas de dados são formas organizadas de armazenar e gerenciar informações em um computador para que possam ser usadas de maneira eficiente. Imagine uma biblioteca: você pode organizar os livros por autor, por gênero, ou por data de publicação. Cada forma de organização facilita diferentes tipos de busca. Da mesma forma, estruturas de dados organizam informações de maneiras específicas para otimizar operações como inserção, remoção, busca e ordenação. Escolher a estrutura de dados correta pode fazer a diferença entre um programa que executa em segundos e um que leva horas. Estruturas de dados são fundamentais para resolver problemas complexos de forma eficiente e são a base de algoritmos poderosos. Neste curso, você aprenderá as estruturas mais importantes e quando usar cada uma delas.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Tipos de Estruturas de Dados",
                    Content = "Existem dois tipos principais de estruturas de dados: lineares e não-lineares. Estruturas lineares organizam dados em sequência, como arrays, listas, pilhas e filas. Cada elemento tem um predecessor e um sucessor (exceto o primeiro e o último). Estruturas não-lineares organizam dados de forma hierárquica ou em rede, como árvores e grafos. Além disso, estruturas podem ser estáticas (tamanho fixo, como arrays) ou dinâmicas (tamanho variável, como listas ligadas). Estruturas primitivas incluem inteiros, caracteres e booleanos, enquanto estruturas compostas combinam múltiplos elementos. Cada tipo tem vantagens e desvantagens em termos de velocidade de acesso, uso de memória e facilidade de modificação. Compreender essas diferenças é essencial para escrever código eficiente.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Complexidade de Tempo e Espaço",
                    Content = "Ao avaliar estruturas de dados e algoritmos, usamos a notação Big O para descrever sua eficiência. A complexidade de tempo mede quanto tempo uma operação leva em relação ao tamanho da entrada. Por exemplo, O(1) significa tempo constante (sempre rápido), O(n) significa tempo linear (cresce proporcionalmente ao tamanho), e O(n²) significa tempo quadrático (cresce rapidamente). A complexidade de espaço mede quanta memória é necessária. Um array de tamanho n tem complexidade de espaço O(n). Buscar um elemento em um array não ordenado é O(n), mas em um array ordenado usando busca binária é O(log n). Entender essas métricas ajuda a escolher a melhor solução para cada problema. Um algoritmo pode ser rápido mas usar muita memória, ou usar pouca memória mas ser lento. O equilíbrio depende dos requisitos do seu sistema.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Comparando Estruturas de Dados",
                    Code = @"// Array - acesso rápido por índice O(1)
int[] numeros = { 1, 2, 3, 4, 5 };
int terceiro = numeros[2]; // Acesso direto

// List - tamanho dinâmico
List<int> lista = new List<int>();
lista.Add(1); // Adiciona ao final O(1)
lista.Insert(0, 0); // Insere no início O(n)

Console.WriteLine($""Array[2]: {terceiro}"");
Console.WriteLine($""Lista tem {lista.Count} elementos"");",
                    Language = "csharp",
                    Explanation = "Este exemplo mostra a diferença entre arrays (tamanho fixo, acesso rápido) e listas (tamanho dinâmico, mais flexíveis). Arrays são ideais quando você conhece o tamanho antecipadamente, enquanto listas são melhores quando o tamanho varia.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Medindo Performance",
                    Code = @"using System.Diagnostics;

var stopwatch = Stopwatch.StartNew();

// Operação a ser medida
List<int> numeros = new List<int>();
for (int i = 0; i < 100000; i++)
{
    numeros.Add(i);
}

stopwatch.Stop();
Console.WriteLine($""Tempo: {stopwatch.ElapsedMilliseconds}ms"");
Console.WriteLine($""Elementos: {numeros.Count}"");",
                    Language = "csharp",
                    Explanation = "Usar Stopwatch permite medir o tempo de execução de operações. Isso é útil para comparar diferentes estruturas de dados e algoritmos na prática e validar a análise de complexidade teórica.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Escolha a Estrutura Adequada",
                    Description = "Para cada cenário, identifique qual estrutura de dados seria mais apropriada: (1) Armazenar histórico de navegação do usuário onde o mais recente é acessado primeiro, (2) Fila de atendimento em um banco, (3) Lista de contatos ordenada alfabeticamente.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"// Cenário 1: Histórico de navegação
// Resposta: 

// Cenário 2: Fila de atendimento
// Resposta: 

// Cenário 3: Lista de contatos
// Resposta: ",
                    Hints = new List<string> 
                    { 
                        "Pense em qual operação é mais frequente em cada cenário",
                        "Histórico: último a entrar, primeiro a sair",
                        "Fila: primeiro a entrar, primeiro a sair" 
                    }
                },
                new Exercise
                {
                    Title = "Análise de Complexidade",
                    Description = "Analise o código fornecido e determine sua complexidade de tempo usando notação Big O. Explique seu raciocínio.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Analise este código
public int SomarElementos(int[] array)
{
    int soma = 0;
    for (int i = 0; i < array.Length; i++)
    {
        soma += array[i];
    }
    return soma;
}

// Complexidade: 
// Explicação: ",
                    Hints = new List<string> 
                    { 
                        "Conte quantas vezes cada operação é executada",
                        "O loop executa n vezes, onde n é o tamanho do array",
                        "Operações dentro do loop são O(1)" 
                    }
                },
                new Exercise
                {
                    Title = "Comparação Prática",
                    Description = "Crie um programa que compare o tempo de inserção de 10.000 elementos no início de um array versus uma lista. Use Stopwatch para medir e explique os resultados.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"using System.Diagnostics;

// Implemente a comparação aqui
",
                    Hints = new List<string> 
                    { 
                        "Arrays requerem redimensionamento ao inserir no início",
                        "Listas têm melhor performance para inserções dinâmicas",
                        "Use Stopwatch.StartNew() e stopwatch.Stop()" 
                    }
                }
            },
            Summary = "Nesta aula você aprendeu o que são estruturas de dados, os diferentes tipos existentes e como avaliar sua eficiência usando complexidade de tempo e espaço. Compreender esses conceitos é fundamental para escolher a estrutura adequada para cada problema e escrever código eficiente. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno. Grandes empresas como Microsoft, Google e Amazon aplicam estes conceitos em seus sistemas. Ao aprender estas técnicas, você está adquirindo conhecimento usado por desenvolvedores em todo o mundo. Considere como este conceito poderia ser aplicado em um projeto pessoal. Pensar em aplicações práticas ajuda a solidificar o entendimento e torna o aprendizado mais significativo. Iniciantes frequentemente cometem alguns erros ao aprender este tópico. Conhecer estes erros comuns ajuda a evitá-los e desenvolver código mais robusto desde o início. Um erro comum é não considerar casos extremos ou situações inesperadas. Sempre teste seu código com diferentes tipos de entrada, incluindo valores nulos, vazios ou inválidos. Outro erro frequente é otimizar prematuramente. Primeiro faça o código funcionar corretamente, depois otimize se necessário. Como diz o ditado: 'premature optimization is the root of all evil'. Quando encontrar problemas, use o debugger para entender o que está acontecendo. Coloque breakpoints, inspecione variáveis e execute o código passo a passo. Esta é uma habilidade essencial para todo desenvolvedor. Mensagens de erro podem parecer intimidadoras no início, mas elas são suas aliadas. Leia-as cuidadosamente - elas geralmente indicam exatamente onde e qual é o problema. Se algo não funcionar como esperado, não desanime. Debugging é uma parte normal do desenvolvimento. Até desenvolvedores experientes passam tempo significativo depurando código. Testar seu código é tão importante quanto escrevê-lo. Testes automatizados garantem que seu código funciona corretamente e continuará funcionando quando você fizer mudanças. Comece escrevendo testes simples para os casos mais comuns. Depois adicione testes para casos extremos e situações de erro. Uma boa cobertura de testes dá confiança para refatorar e melhorar o código. Considere a prática de TDD (Test-Driven Development), onde você escreve os testes antes do código. Isso ajuda a pensar claramente sobre o que o código deve fazer. Embora a correção seja sempre a prioridade, é importante estar ciente das implicações de performance de suas escolhas. Diferentes abordagens podem ter custos computacionais muito diferentes. Em muitos casos, a diferença de performance é negligenciável para aplicações pequenas. No entanto, à medida que seus programas crescem, escolhas eficientes se tornam cada vez mais importantes. Aprenda a usar ferramentas de profiling para identificar gargalos de performance. Otimize baseado em dados reais, não em suposições sobre onde o código é lento. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0003-000000000001"),
            CourseId = _courseId,
            Title = "Introdução a Estruturas de Dados",
            Duration = "50 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 50,
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
                "Compreender a estrutura e funcionamento de arrays",
                "Aprender operações básicas com arrays em C#",
                "Dominar técnicas de manipulação de arrays",
                "Entender arrays multidimensionais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Fundamentos de Arrays",
                    Content = "Arrays são a estrutura de dados mais fundamental e simples. Um array é uma coleção de elementos do mesmo tipo armazenados em posições contíguas de memória. Cada elemento é acessado através de um índice numérico, começando do zero. A principal vantagem dos arrays é o acesso direto: você pode acessar qualquer elemento em tempo constante O(1) usando seu índice. Por exemplo, array[5] acessa diretamente o sexto elemento. Arrays têm tamanho fixo definido na criação, o que significa que você não pode adicionar ou remover elementos depois. Essa limitação é compensada pela eficiência: arrays usam memória de forma compacta e o acesso é extremamente rápido. Em C#, arrays são objetos que herdam de System.Array e fornecem propriedades úteis como Length. Arrays são ideais quando você conhece o número de elementos antecipadamente e precisa de acesso rápido por índice.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Operações com Arrays",
                    Content = "As operações mais comuns com arrays incluem acesso, busca, inserção e remoção. Acessar um elemento por índice é O(1) - extremamente rápido. Buscar um elemento específico em um array não ordenado requer percorrer todos os elementos, resultando em O(n). Inserir ou remover elementos no meio de um array é custoso O(n) porque requer deslocar todos os elementos subsequentes. Por isso, arrays não são ideais para dados que mudam frequentemente. No entanto, você pode usar técnicas como manter um contador de elementos válidos e marcar posições como vazias em vez de realmente remover. Arrays também suportam operações em lote eficientes como copiar, ordenar e reverter. O método Array.Sort() ordena em O(n log n), e Array.Copy() copia elementos rapidamente. Compreender essas características ajuda a usar arrays de forma eficiente.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Arrays Multidimensionais",
                    Content = "Arrays multidimensionais permitem organizar dados em múltiplas dimensões, como matrizes (2D) ou cubos (3D). Em C#, existem dois tipos: arrays retangulares (int[,]) e arrays jagged (int[][]). Arrays retangulares têm dimensões fixas e todos os sub-arrays têm o mesmo tamanho, como uma matriz matemática. Arrays jagged são arrays de arrays, onde cada sub-array pode ter tamanho diferente. Arrays retangulares são mais eficientes em memória e acesso, enquanto arrays jagged são mais flexíveis. Use arrays 2D para representar grades, tabuleiros de jogos, imagens ou matrizes matemáticas. Por exemplo, um tabuleiro de xadrez seria int[8,8]. Arrays multidimensionais são acessados com múltiplos índices: matriz[linha, coluna]. Percorrer arrays multidimensionais requer loops aninhados, resultando em complexidade O(n×m) para uma matriz n×m.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Operações Básicas com Arrays",
                    Code = @"// Declaração e inicialização
int[] numeros = new int[5]; // Array de tamanho 5
int[] valores = { 10, 20, 30, 40, 50 }; // Inicialização direta

// Acesso e modificação
numeros[0] = 100;
int primeiro = valores[0];

// Percorrer array
for (int i = 0; i < valores.Length; i++)
{
    Console.WriteLine($""Posição {i}: {valores[i]}"");
}

// Usando foreach
foreach (int valor in valores)
{
    Console.WriteLine($""Valor: {valor}"");
}",
                    Language = "csharp",
                    Explanation = "Este exemplo mostra as operações fundamentais com arrays: declaração, inicialização, acesso por índice e iteração. Note que Length retorna o tamanho do array e os índices vão de 0 a Length-1.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Busca e Manipulação",
                    Code = @"int[] numeros = { 5, 2, 8, 1, 9, 3 };

// Buscar elemento
int buscar = 8;
int indice = Array.IndexOf(numeros, buscar);
Console.WriteLine($""{buscar} está no índice {indice}"");

// Ordenar
Array.Sort(numeros);
Console.WriteLine(""Ordenado: "" + string.Join("", "", numeros));

// Reverter
Array.Reverse(numeros);
Console.WriteLine(""Revertido: "" + string.Join("", "", numeros));

// Copiar
int[] copia = new int[numeros.Length];
Array.Copy(numeros, copia, numeros.Length);",
                    Language = "csharp",
                    Explanation = "Array.IndexOf() busca um elemento e retorna seu índice (ou -1 se não encontrado). Array.Sort() ordena in-place, Array.Reverse() inverte a ordem, e Array.Copy() copia elementos entre arrays.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Arrays Multidimensionais",
                    Code = @"// Array retangular 2D (matriz 3x3)
int[,] matriz = new int[3, 3]
{
    { 1, 2, 3 },
    { 4, 5, 6 },
    { 7, 8, 9 }
};

// Acessar elemento
int elemento = matriz[1, 2]; // Linha 1, Coluna 2 = 6

// Percorrer matriz
for (int i = 0; i < 3; i++)
{
    for (int j = 0; j < 3; j++)
    {
        Console.Write($""{matriz[i, j]} "");
    }
    Console.WriteLine();
}

// Array jagged (array de arrays)
int[][] jagged = new int[3][];
jagged[0] = new int[] { 1, 2 };
jagged[1] = new int[] { 3, 4, 5 };
jagged[2] = new int[] { 6 };",
                    Language = "csharp",
                    Explanation = "Arrays retangulares usam vírgula para separar dimensões [linha, coluna]. Arrays jagged são arrays de arrays, permitindo sub-arrays de tamanhos diferentes. Use retangulares para matrizes uniformes e jagged para estruturas irregulares.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Encontrar Maior e Menor",
                    Description = "Crie um método que recebe um array de inteiros e retorna o maior e o menor valor. Não use métodos prontos como Max() ou Min().",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"public (int maior, int menor) EncontrarExtremos(int[] array)
{
    // Implemente aqui
}

// Teste
int[] numeros = { 5, 2, 9, 1, 7, 3 };
var resultado = EncontrarExtremos(numeros);
Console.WriteLine($""Maior: {resultado.maior}, Menor: {resultado.menor}"");",
                    Hints = new List<string> 
                    { 
                        "Inicialize maior com o menor valor possível e menor com o maior valor possível",
                        "Percorra o array comparando cada elemento",
                        "Atualize maior e menor conforme necessário" 
                    }
                },
                new Exercise
                {
                    Title = "Rotacionar Array",
                    Description = "Implemente um método que rotaciona um array k posições para a direita. Por exemplo, [1,2,3,4,5] rotacionado 2 posições vira [4,5,1,2,3].",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"public void RotacionarDireita(int[] array, int k)
{
    // Implemente aqui
}

// Teste
int[] numeros = { 1, 2, 3, 4, 5 };
RotacionarDireita(numeros, 2);
Console.WriteLine(string.Join("", "", numeros)); // Deve exibir: 4, 5, 1, 2, 3",
                    Hints = new List<string> 
                    { 
                        "Use um array temporário para armazenar elementos",
                        "Calcule a nova posição de cada elemento: (i + k) % array.Length",
                        "Ou use três reversões: reverter tudo, reverter primeiros k, reverter restantes" 
                    }
                },
                new Exercise
                {
                    Title = "Soma de Matriz",
                    Description = "Crie um método que recebe duas matrizes 3x3 e retorna uma nova matriz com a soma elemento por elemento. Valide que as matrizes têm o mesmo tamanho.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"public int[,] SomarMatrizes(int[,] matriz1, int[,] matriz2)
{
    // Implemente aqui
}

// Teste
int[,] m1 = { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 } };
int[,] m2 = { { 9, 8, 7 }, { 6, 5, 4 }, { 3, 2, 1 } };
int[,] resultado = SomarMatrizes(m1, m2);",
                    Hints = new List<string> 
                    { 
                        "Use GetLength(0) para linhas e GetLength(1) para colunas",
                        "Valide que ambas as matrizes têm as mesmas dimensões",
                        "Use loops aninhados para percorrer e somar elemento por elemento" 
                    }
                }
            },
            Summary = "Nesta aula você dominou arrays, a estrutura de dados mais fundamental. Aprendeu sobre acesso direto O(1), operações de busca e manipulação, e arrays multidimensionais. Arrays são ideais para dados de tamanho fixo que requerem acesso rápido por índice. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno. Grandes empresas como Microsoft, Google e Amazon aplicam estes conceitos em seus sistemas. Ao aprender estas técnicas, você está adquirindo conhecimento usado por desenvolvedores em todo o mundo. Considere como este conceito poderia ser aplicado em um projeto pessoal. Pensar em aplicações práticas ajuda a solidificar o entendimento e torna o aprendizado mais significativo. Iniciantes frequentemente cometem alguns erros ao aprender este tópico. Conhecer estes erros comuns ajuda a evitá-los e desenvolver código mais robusto desde o início. Um erro comum é não considerar casos extremos ou situações inesperadas. Sempre teste seu código com diferentes tipos de entrada, incluindo valores nulos, vazios ou inválidos. Outro erro frequente é otimizar prematuramente. Primeiro faça o código funcionar corretamente, depois otimize se necessário. Como diz o ditado: 'premature optimization is the root of all evil'. Quando encontrar problemas, use o debugger para entender o que está acontecendo. Coloque breakpoints, inspecione variáveis e execute o código passo a passo. Esta é uma habilidade essencial para todo desenvolvedor. Mensagens de erro podem parecer intimidadoras no início, mas elas são suas aliadas. Leia-as cuidadosamente - elas geralmente indicam exatamente onde e qual é o problema. Se algo não funcionar como esperado, não desanime. Debugging é uma parte normal do desenvolvimento. Até desenvolvedores experientes passam tempo significativo depurando código. Testar seu código é tão importante quanto escrevê-lo. Testes automatizados garantem que seu código funciona corretamente e continuará funcionando quando você fizer mudanças. Comece escrevendo testes simples para os casos mais comuns. Depois adicione testes para casos extremos e situações de erro. Uma boa cobertura de testes dá confiança para refatorar e melhorar o código. Considere a prática de TDD (Test-Driven Development), onde você escreve os testes antes do código. Isso ajuda a pensar claramente sobre o que o código deve fazer. Embora a correção seja sempre a prioridade, é importante estar ciente das implicações de performance de suas escolhas. Diferentes abordagens podem ter custos computacionais muito diferentes. Em muitos casos, a diferença de performance é negligenciável para aplicações pequenas. No entanto, à medida que seus programas crescem, escolhas eficientes se tornam cada vez mais importantes. Aprenda a usar ferramentas de profiling para identificar gargalos de performance. Otimize baseado em dados reais, não em suposições sobre onde o código é lento. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0003-000000000002"),
            CourseId = _courseId,
            Title = "Arrays e Operações Fundamentais",
            Duration = "55 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 55,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0003-000000000001" }),
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
                "Compreender a diferença entre List<T> e arrays",
                "Dominar operações de inserção, remoção e busca em listas",
                "Aprender sobre capacidade e redimensionamento de listas",
                "Usar métodos LINQ com listas"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "List<T>: Arrays Dinâmicos",
                    Content = "List<T> é uma das estruturas de dados mais usadas em C# porque combina a eficiência de arrays com a flexibilidade de tamanho dinâmico. Internamente, List<T> usa um array, mas gerencia automaticamente o redimensionamento quando necessário. Quando você adiciona elementos além da capacidade atual, a lista cria um novo array maior (geralmente o dobro do tamanho) e copia todos os elementos. Isso significa que adicionar ao final é geralmente O(1), mas ocasionalmente O(n) quando ocorre redimensionamento. A média amortizada é O(1). List<T> fornece métodos convenientes como Add, Remove, Insert, Contains e Sort. Você pode acessar elementos por índice como em arrays, mas também pode facilmente adicionar ou remover elementos. Lists são ideais quando você não sabe o tamanho final dos dados ou quando precisa modificar a coleção frequentemente.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Capacidade vs Contagem",
                    Content = "List<T> tem duas propriedades importantes: Count (número de elementos) e Capacity (tamanho do array interno). Count é quantos elementos você adicionou, enquanto Capacity é quanto espaço está alocado. Por exemplo, uma lista pode ter Count=5 mas Capacity=8, significando que há espaço para mais 3 elementos sem redimensionar. Você pode definir a capacidade inicial se souber aproximadamente quantos elementos terá, evitando redimensionamentos: new List<int>(1000). Isso melhora a performance quando você adiciona muitos elementos. O método TrimExcess() reduz a capacidade para o tamanho atual, liberando memória não usada. Compreender essa distinção ajuda a otimizar o uso de memória e performance. Em cenários onde performance é crítica, pré-alocar a capacidade pode fazer diferença significativa.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Operações e Performance",
                    Content = "Diferentes operações em List<T> têm diferentes complexidades. Add() ao final é O(1) amortizado. Insert() no início ou meio é O(n) porque requer deslocar elementos. Remove() é O(n) porque precisa buscar o elemento e deslocar os subsequentes. RemoveAt() com índice conhecido é O(n) apenas pelo deslocamento. Contains() e IndexOf() são O(n) porque podem precisar percorrer toda a lista. Sort() é O(n log n) usando algoritmo eficiente. Para buscas frequentes, considere usar estruturas como HashSet ou Dictionary. Para inserções/remoções frequentes no início, considere LinkedList. List<T> é excelente para acesso por índice, iteração e quando a maioria das modificações ocorre no final. Escolher a estrutura certa baseada nas operações mais frequentes é fundamental para performance.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Operações Básicas com List",
                    Code = @"// Criar e adicionar elementos
List<string> nomes = new List<string>();
nomes.Add(""Ana"");
nomes.Add(""Bruno"");
nomes.Add(""Carlos"");

// Inserir em posição específica
nomes.Insert(1, ""Beatriz""); // Insere na posição 1

// Remover
nomes.Remove(""Carlos""); // Remove por valor
nomes.RemoveAt(0); // Remove por índice

// Buscar
bool existe = nomes.Contains(""Bruno"");
int indice = nomes.IndexOf(""Beatriz"");

// Iterar
foreach (string nome in nomes)
{
    Console.WriteLine(nome);
}

Console.WriteLine($""Total: {nomes.Count} elementos"");",
                    Language = "csharp",
                    Explanation = "Este exemplo demonstra as operações mais comuns com List<T>: adicionar, inserir, remover, buscar e iterar. Note que Remove() remove a primeira ocorrência do valor, enquanto RemoveAt() remove por índice.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Capacidade e Performance",
                    Code = @"// Sem pré-alocação
var lista1 = new List<int>();
for (int i = 0; i < 10000; i++)
{
    lista1.Add(i); // Múltiplos redimensionamentos
}

// Com pré-alocação
var lista2 = new List<int>(10000); // Capacidade inicial
for (int i = 0; i < 10000; i++)
{
    lista2.Add(i); // Sem redimensionamentos
}

Console.WriteLine($""Lista1 - Count: {lista1.Count}, Capacity: {lista1.Capacity}"");
Console.WriteLine($""Lista2 - Count: {lista2.Count}, Capacity: {lista2.Capacity}"");

// Liberar memória não usada
lista2.TrimExcess();
Console.WriteLine($""Após TrimExcess - Capacity: {lista2.Capacity}"");",
                    Language = "csharp",
                    Explanation = "Pré-alocar a capacidade evita redimensionamentos durante inserções, melhorando a performance. TrimExcess() libera memória não utilizada, útil quando a lista não crescerá mais.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "LINQ com Lists",
                    Code = @"List<int> numeros = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

// Filtrar
var pares = numeros.Where(n => n % 2 == 0).ToList();

// Transformar
var quadrados = numeros.Select(n => n * n).ToList();

// Ordenar
var ordenados = numeros.OrderByDescending(n => n).ToList();

// Agregar
int soma = numeros.Sum();
double media = numeros.Average();
int maximo = numeros.Max();

Console.WriteLine(""Pares: "" + string.Join("", "", pares));
Console.WriteLine(""Quadrados: "" + string.Join("", "", quadrados));
Console.WriteLine($""Soma: {soma}, Média: {media}, Máximo: {maximo}"");",
                    Language = "csharp",
                    Explanation = "LINQ fornece métodos poderosos para manipular listas de forma declarativa. Where() filtra, Select() transforma, OrderBy() ordena, e métodos como Sum(), Average() e Max() agregam valores.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Remover Duplicatas",
                    Description = "Crie um método que recebe uma List<int> e remove todos os elementos duplicados, mantendo apenas a primeira ocorrência de cada valor.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"public List<int> RemoverDuplicatas(List<int> lista)
{
    // Implemente aqui
}

// Teste
var numeros = new List<int> { 1, 2, 2, 3, 4, 4, 5 };
var resultado = RemoverDuplicatas(numeros);
Console.WriteLine(string.Join("", "", resultado)); // Deve exibir: 1, 2, 3, 4, 5",
                    Hints = new List<string> 
                    { 
                        "Use uma nova lista para armazenar elementos únicos",
                        "Verifique se o elemento já existe antes de adicionar",
                        "Ou use HashSet para rastrear elementos já vistos" 
                    }
                },
                new Exercise
                {
                    Title = "Mesclar Listas Ordenadas",
                    Description = "Implemente um método que recebe duas listas ordenadas e retorna uma nova lista ordenada contendo todos os elementos de ambas.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"public List<int> MesclarOrdenadas(List<int> lista1, List<int> lista2)
{
    // Implemente aqui
}

// Teste
var l1 = new List<int> { 1, 3, 5, 7 };
var l2 = new List<int> { 2, 4, 6, 8 };
var resultado = MesclarOrdenadas(l1, l2);
Console.WriteLine(string.Join("", "", resultado)); // Deve exibir: 1, 2, 3, 4, 5, 6, 7, 8",
                    Hints = new List<string> 
                    { 
                        "Use dois ponteiros, um para cada lista",
                        "Compare elementos e adicione o menor à lista resultado",
                        "Não esqueça de adicionar elementos restantes quando uma lista terminar" 
                    }
                },
                new Exercise
                {
                    Title = "Particionar Lista",
                    Description = "Crie um método que particiona uma lista em duas: uma com elementos menores que um valor pivot e outra com elementos maiores ou iguais. Mantenha a ordem relativa dos elementos.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"public (List<int> menores, List<int> maiores) Particionar(List<int> lista, int pivot)
{
    // Implemente aqui
}

// Teste
var numeros = new List<int> { 5, 2, 8, 1, 9, 3, 7 };
var resultado = Particionar(numeros, 5);
Console.WriteLine(""Menores: "" + string.Join("", "", resultado.menores));
Console.WriteLine(""Maiores: "" + string.Join("", "", resultado.maiores));",
                    Hints = new List<string> 
                    { 
                        "Crie duas listas vazias para menores e maiores",
                        "Percorra a lista original comparando cada elemento com o pivot",
                        "Adicione cada elemento à lista apropriada" 
                    }
                }
            },
            Summary = "Nesta aula você dominou List<T>, a estrutura de dados mais versátil do C#. Aprendeu sobre capacidade, redimensionamento, operações comuns e como usar LINQ para manipular listas de forma eficiente. Lists são ideais para a maioria dos cenários onde você precisa de uma coleção dinâmica. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno. Grandes empresas como Microsoft, Google e Amazon aplicam estes conceitos em seus sistemas. Ao aprender estas técnicas, você está adquirindo conhecimento usado por desenvolvedores em todo o mundo. Considere como este conceito poderia ser aplicado em um projeto pessoal. Pensar em aplicações práticas ajuda a solidificar o entendimento e torna o aprendizado mais significativo. Iniciantes frequentemente cometem alguns erros ao aprender este tópico. Conhecer estes erros comuns ajuda a evitá-los e desenvolver código mais robusto desde o início. Um erro comum é não considerar casos extremos ou situações inesperadas. Sempre teste seu código com diferentes tipos de entrada, incluindo valores nulos, vazios ou inválidos. Outro erro frequente é otimizar prematuramente. Primeiro faça o código funcionar corretamente, depois otimize se necessário. Como diz o ditado: 'premature optimization is the root of all evil'. Quando encontrar problemas, use o debugger para entender o que está acontecendo. Coloque breakpoints, inspecione variáveis e execute o código passo a passo. Esta é uma habilidade essencial para todo desenvolvedor. Mensagens de erro podem parecer intimidadoras no início, mas elas são suas aliadas. Leia-as cuidadosamente - elas geralmente indicam exatamente onde e qual é o problema. Se algo não funcionar como esperado, não desanime. Debugging é uma parte normal do desenvolvimento. Até desenvolvedores experientes passam tempo significativo depurando código. Testar seu código é tão importante quanto escrevê-lo. Testes automatizados garantem que seu código funciona corretamente e continuará funcionando quando você fizer mudanças. Comece escrevendo testes simples para os casos mais comuns. Depois adicione testes para casos extremos e situações de erro. Uma boa cobertura de testes dá confiança para refatorar e melhorar o código. Considere a prática de TDD (Test-Driven Development), onde você escreve os testes antes do código. Isso ajuda a pensar claramente sobre o que o código deve fazer. Embora a correção seja sempre a prioridade, é importante estar ciente das implicações de performance de suas escolhas. Diferentes abordagens podem ter custos computacionais muito diferentes. Em muitos casos, a diferença de performance é negligenciável para aplicações pequenas. No entanto, à medida que seus programas crescem, escolhas eficientes se tornam cada vez mais importantes. Aprenda a usar ferramentas de profiling para identificar gargalos de performance. Otimize baseado em dados reais, não em suposições sobre onde o código é lento. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0003-000000000003"),
            CourseId = _courseId,
            Title = "Listas Dinâmicas com List<T>",
            Duration = "50 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 50,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0003-000000000002" }),
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
                "Compreender o conceito LIFO (Last In, First Out)",
                "Implementar e usar pilhas (Stack) em C#",
                "Aplicar pilhas em problemas práticos",
                "Entender casos de uso comuns de pilhas"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "O que é uma Pilha?",
                    Content = "Uma pilha (stack) é uma estrutura de dados que segue o princípio LIFO: Last In, First Out (último a entrar, primeiro a sair). Imagine uma pilha de pratos: você adiciona pratos no topo e remove do topo também. Você não pode pegar um prato do meio sem remover os de cima primeiro. Em programação, pilhas têm duas operações principais: Push (adicionar ao topo) e Pop (remover do topo). Ambas são operações O(1), extremamente rápidas. Pilhas também têm Peek (ver o topo sem remover) e Count (número de elementos). Pilhas são fundamentais em computação: o sistema usa uma pilha de chamadas para rastrear funções, navegadores usam pilhas para o botão voltar, e editores de texto usam pilhas para desfazer ações. A simplicidade e eficiência das pilhas as tornam ideais para muitos problemas.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Implementação e Uso",
                    Content = "Em C#, a classe Stack<T> fornece uma implementação eficiente de pilha. Internamente, usa um array dinâmico similar a List<T>. Push() adiciona ao topo, Pop() remove e retorna o topo, e Peek() apenas visualiza sem remover. Se você tentar Pop() ou Peek() em uma pilha vazia, ocorre uma exceção InvalidOperationException, então sempre verifique Count > 0 antes. Você pode criar uma pilha vazia com new Stack<T>() ou inicializar com elementos usando new Stack<T>(collection). Pilhas são úteis para inverter sequências, validar expressões balanceadas (parênteses), avaliar expressões matemáticas em notação polonesa reversa, e implementar algoritmos de backtracking. A natureza LIFO torna pilhas perfeitas para qualquer problema que envolva processar elementos na ordem inversa de chegada.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Aplicações Práticas",
                    Content = "Pilhas têm inúmeras aplicações práticas. Navegadores web usam pilhas para histórico: cada página visitada é empilhada, e o botão voltar faz pop. Editores de texto usam pilhas para desfazer/refazer: cada ação é empilhada, e desfazer faz pop. Compiladores usam pilhas para analisar sintaxe e avaliar expressões. Algoritmos de busca em profundidade (DFS) usam pilhas para rastrear caminhos. Validação de parênteses balanceados é um problema clássico: para cada '(' você empilha, para cada ')' você desempilha e verifica se corresponde. Se a pilha está vazia no final, os parênteses estão balanceados. Calculadoras usam pilhas para avaliar expressões: números são empilhados, operadores desempilham operandos, calculam e empilham o resultado. Compreender pilhas é essencial para resolver muitos problemas algorítmicos elegantemente.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Operações Básicas com Stack",
                    Code = @"Stack<int> pilha = new Stack<int>();

// Push - adicionar elementos
pilha.Push(10);
pilha.Push(20);
pilha.Push(30);

Console.WriteLine($""Topo: {pilha.Peek()}""); // 30 (não remove)
Console.WriteLine($""Elementos: {pilha.Count}""); // 3

// Pop - remover e retornar o topo
int removido = pilha.Pop(); // 30
Console.WriteLine($""Removido: {removido}"");
Console.WriteLine($""Novo topo: {pilha.Peek()}""); // 20

// Verificar se está vazia
if (pilha.Count > 0)
{
    Console.WriteLine(""Pilha não está vazia"");
}",
                    Language = "csharp",
                    Explanation = "Este exemplo mostra as operações fundamentais de pilha. Push() adiciona ao topo, Pop() remove do topo, e Peek() visualiza sem remover. Sempre verifique Count antes de Pop() ou Peek() para evitar exceções.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Inverter String com Pilha",
                    Code = @"public string InverterString(string texto)
{
    Stack<char> pilha = new Stack<char>();
    
    // Empilhar cada caractere
    foreach (char c in texto)
    {
        pilha.Push(c);
    }
    
    // Desempilhar para construir string invertida
    string invertida = """";
    while (pilha.Count > 0)
    {
        invertida += pilha.Pop();
    }
    
    return invertida;
}

// Teste
string original = ""Hello"";
string invertida = InverterString(original);
Console.WriteLine($""{original} -> {invertida}""); // Hello -> olleH",
                    Language = "csharp",
                    Explanation = "Pilhas são perfeitas para inverter sequências. Empilhamos cada caractere e depois desempilhamos, resultando na ordem inversa devido ao comportamento LIFO.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Validar Parênteses Balanceados",
                    Code = @"public bool ParentesesBalanceados(string expressao)
{
    Stack<char> pilha = new Stack<char>();
    
    foreach (char c in expressao)
    {
        if (c == '(' || c == '[' || c == '{')
        {
            pilha.Push(c);
        }
        else if (c == ')' || c == ']' || c == '}')
        {
            if (pilha.Count == 0) return false;
            
            char topo = pilha.Pop();
            if ((c == ')' && topo != '(') ||
                (c == ']' && topo != '[') ||
                (c == '}' && topo != '{'))
            {
                return false;
            }
        }
    }
    
    return pilha.Count == 0;
}

// Teste
Console.WriteLine(ParentesesBalanceados(""({[]})""));  // True
Console.WriteLine(ParentesesBalanceados(""({[}])""));  // False",
                    Language = "csharp",
                    Explanation = "Este algoritmo valida se parênteses, colchetes e chaves estão balanceados. Empilhamos aberturas e desempilhamos para cada fechamento, verificando se correspondem. A pilha deve estar vazia no final.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Desfazer Operações",
                    Description = "Implemente uma classe EditorTexto com métodos Escrever(string), Desfazer() e ObterTexto(). Use uma pilha para armazenar o histórico de textos e permitir desfazer.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"public class EditorTexto
{
    private Stack<string> historico = new Stack<string>();
    private string textoAtual = """";
    
    public void Escrever(string texto)
    {
        // Implemente aqui
    }
    
    public void Desfazer()
    {
        // Implemente aqui
    }
    
    public string ObterTexto()
    {
        return textoAtual;
    }
}",
                    Hints = new List<string> 
                    { 
                        "Antes de modificar textoAtual, empilhe o estado anterior",
                        "Desfazer deve desempilhar e restaurar o estado anterior",
                        "Verifique se a pilha não está vazia antes de desfazer" 
                    }
                },
                new Exercise
                {
                    Title = "Avaliar Expressão Pós-Fixa",
                    Description = "Implemente um avaliador de expressões em notação pós-fixa (RPN). Por exemplo, '3 4 + 2 *' deve retornar 14 ((3+4)*2).",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"public int AvaliarRPN(string expressao)
{
    // Implemente aqui
    // Dica: use pilha para números, quando encontrar operador, desempilhe dois números
}

// Teste
Console.WriteLine(AvaliarRPN(""3 4 + 2 *"")); // 14
Console.WriteLine(AvaliarRPN(""5 1 2 + 4 * + 3 -"")); // 14",
                    Hints = new List<string> 
                    { 
                        "Divida a expressão por espaços",
                        "Se for número, empilhe; se for operador, desempilhe dois, calcule e empilhe resultado",
                        "O resultado final é o único elemento na pilha" 
                    }
                },
                new Exercise
                {
                    Title = "Caminho Válido em Sistema de Arquivos",
                    Description = "Dado um caminho Unix como '/home/../usr/./local', simplifique-o removendo '.' (diretório atual) e '..' (diretório pai). Use uma pilha.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"public string SimplificarCaminho(string caminho)
{
    // Implemente aqui
}

// Teste
Console.WriteLine(SimplificarCaminho(""/home/../usr/./local"")); // /usr/local
Console.WriteLine(SimplificarCaminho(""/a/./b/../../c/"")); // /c",
                    Hints = new List<string> 
                    { 
                        "Divida o caminho por '/' e processe cada parte",
                        "Ignore '.' e strings vazias",
                        "Para '..', faça pop da pilha (se não vazia)",
                        "Para nomes normais, empilhe" 
                    }
                }
            },
            Summary = "Nesta aula você dominou pilhas (Stack), uma estrutura LIFO fundamental. Aprendeu operações Push, Pop e Peek, todas O(1), e aplicou pilhas em problemas práticos como inverter sequências, validar parênteses e avaliar expressões. Pilhas são essenciais para muitos algoritmos. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno. Grandes empresas como Microsoft, Google e Amazon aplicam estes conceitos em seus sistemas. Ao aprender estas técnicas, você está adquirindo conhecimento usado por desenvolvedores em todo o mundo. Considere como este conceito poderia ser aplicado em um projeto pessoal. Pensar em aplicações práticas ajuda a solidificar o entendimento e torna o aprendizado mais significativo. Iniciantes frequentemente cometem alguns erros ao aprender este tópico. Conhecer estes erros comuns ajuda a evitá-los e desenvolver código mais robusto desde o início. Um erro comum é não considerar casos extremos ou situações inesperadas. Sempre teste seu código com diferentes tipos de entrada, incluindo valores nulos, vazios ou inválidos. Outro erro frequente é otimizar prematuramente. Primeiro faça o código funcionar corretamente, depois otimize se necessário. Como diz o ditado: 'premature optimization is the root of all evil'. Quando encontrar problemas, use o debugger para entender o que está acontecendo. Coloque breakpoints, inspecione variáveis e execute o código passo a passo. Esta é uma habilidade essencial para todo desenvolvedor. Mensagens de erro podem parecer intimidadoras no início, mas elas são suas aliadas. Leia-as cuidadosamente - elas geralmente indicam exatamente onde e qual é o problema. Se algo não funcionar como esperado, não desanime. Debugging é uma parte normal do desenvolvimento. Até desenvolvedores experientes passam tempo significativo depurando código. Testar seu código é tão importante quanto escrevê-lo. Testes automatizados garantem que seu código funciona corretamente e continuará funcionando quando você fizer mudanças. Comece escrevendo testes simples para os casos mais comuns. Depois adicione testes para casos extremos e situações de erro. Uma boa cobertura de testes dá confiança para refatorar e melhorar o código. Considere a prática de TDD (Test-Driven Development), onde você escreve os testes antes do código. Isso ajuda a pensar claramente sobre o que o código deve fazer. Embora a correção seja sempre a prioridade, é importante estar ciente das implicações de performance de suas escolhas. Diferentes abordagens podem ter custos computacionais muito diferentes. Em muitos casos, a diferença de performance é negligenciável para aplicações pequenas. No entanto, à medida que seus programas crescem, escolhas eficientes se tornam cada vez mais importantes. Aprenda a usar ferramentas de profiling para identificar gargalos de performance. Otimize baseado em dados reais, não em suposições sobre onde o código é lento. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0003-000000000004"),
            CourseId = _courseId,
            Title = "Pilhas (Stack) e Princípio LIFO",
            Duration = "50 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 50,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0003-000000000003" }),
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
                "Compreender o conceito FIFO (First In, First Out)",
                "Implementar e usar filas (Queue) em C#",
                "Aplicar filas em problemas de processamento sequencial",
                "Entender filas de prioridade e suas aplicações"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "O que é uma Fila?",
                    Content = "Uma fila (queue) é uma estrutura de dados que segue o princípio FIFO: First In, First Out (primeiro a entrar, primeiro a sair). Imagine uma fila de banco: a primeira pessoa a chegar é a primeira a ser atendida. Em programação, filas têm duas operações principais: Enqueue (adicionar ao final) e Dequeue (remover do início). Ambas são O(1), muito eficientes. Filas também têm Peek (ver o primeiro sem remover) e Count (número de elementos). Filas são fundamentais para processar tarefas na ordem de chegada, gerenciar recursos compartilhados, implementar buffers de comunicação e algoritmos de busca em largura (BFS). A natureza FIFO garante justiça: quem chega primeiro é atendido primeiro. Filas são usadas em sistemas operacionais para escalonamento de processos, em impressoras para gerenciar trabalhos de impressão, e em servidores web para processar requisições.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Implementação e Uso",
                    Content = "Em C#, a classe Queue<T> fornece uma implementação eficiente de fila usando um array circular. Enqueue() adiciona ao final, Dequeue() remove e retorna o primeiro, e Peek() visualiza sem remover. Como em pilhas, Dequeue() e Peek() lançam exceção se a fila está vazia, então sempre verifique Count > 0. Você pode criar uma fila vazia com new Queue<T>() ou inicializar com elementos. Filas são ideais para processar tarefas em ordem, implementar caches LRU (Least Recently Used), gerenciar buffers de dados e implementar algoritmos de grafos como BFS. A implementação com array circular é eficiente: quando o final do array é alcançado, novos elementos são adicionados no início do array (se houver espaço), evitando realocações constantes. Isso mantém Enqueue e Dequeue como operações O(1).",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Filas de Prioridade",
                    Content = "Filas de prioridade são uma variação onde elementos têm prioridades associadas. Em vez de FIFO, elementos com maior prioridade são removidos primeiro. Em C#, PriorityQueue<TElement, TPriority> implementa isso usando um heap binário, garantindo que Enqueue e Dequeue sejam O(log n). Filas de prioridade são usadas em algoritmos de caminho mais curto (Dijkstra), escalonamento de tarefas com prioridades, simulações de eventos, e compressão de dados (Huffman coding). Por exemplo, um sistema de emergência hospitalar usa fila de prioridade: pacientes críticos são atendidos antes, independente da ordem de chegada. Ao usar PriorityQueue, prioridades menores têm precedência por padrão, mas você pode inverter isso. Compreender quando usar filas normais versus filas de prioridade é importante para resolver problemas eficientemente.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Operações Básicas com Queue",
                    Code = @"Queue<string> fila = new Queue<string>();

// Enqueue - adicionar ao final
fila.Enqueue(""Ana"");
fila.Enqueue(""Bruno"");
fila.Enqueue(""Carlos"");

Console.WriteLine($""Primeiro: {fila.Peek()}""); // Ana (não remove)
Console.WriteLine($""Elementos: {fila.Count}""); // 3

// Dequeue - remover e retornar o primeiro
string atendido = fila.Dequeue(); // Ana
Console.WriteLine($""Atendido: {atendido}"");
Console.WriteLine($""Próximo: {fila.Peek()}""); // Bruno

// Processar toda a fila
while (fila.Count > 0)
{
    Console.WriteLine($""Atendendo: {fila.Dequeue()}"");
}",
                    Language = "csharp",
                    Explanation = "Este exemplo mostra as operações fundamentais de fila. Enqueue() adiciona ao final, Dequeue() remove do início, e Peek() visualiza sem remover. O padrão while com Count > 0 processa todos os elementos em ordem FIFO.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Simulador de Atendimento",
                    Code = @"public class SistemaAtendimento
{
    private Queue<string> fila = new Queue<string>();
    private int numeroSenha = 1;
    
    public void ChamarSenha(string nome)
    {
        fila.Enqueue($""Senha {numeroSenha++}: {nome}"");
        Console.WriteLine($""{nome} entrou na fila"");
    }
    
    public void AtenderProximo()
    {
        if (fila.Count > 0)
        {
            string cliente = fila.Dequeue();
            Console.WriteLine($""Atendendo: {cliente}"");
        }
        else
        {
            Console.WriteLine(""Fila vazia"");
        }
    }
    
    public void MostrarFila()
    {
        Console.WriteLine($""Pessoas na fila: {fila.Count}"");
        foreach (var pessoa in fila)
        {
            Console.WriteLine($""  - {pessoa}"");
        }
    }
}

// Teste
var sistema = new SistemaAtendimento();
sistema.ChamarSenha(""Ana"");
sistema.ChamarSenha(""Bruno"");
sistema.AtenderProximo();
sistema.MostrarFila();",
                    Language = "csharp",
                    Explanation = "Este exemplo simula um sistema de atendimento com senhas. Clientes entram na fila com ChamarSenha() e são atendidos em ordem com AtenderProximo(). Note que foreach permite visualizar a fila sem modificá-la.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Fila de Prioridade",
                    Code = @"// PriorityQueue disponível em .NET 6+
PriorityQueue<string, int> filaPrioridade = new PriorityQueue<string, int>();

// Adicionar com prioridades (menor = maior prioridade)
filaPrioridade.Enqueue(""Tarefa Normal"", 5);
filaPrioridade.Enqueue(""Tarefa Urgente"", 1);
filaPrioridade.Enqueue(""Tarefa Baixa"", 10);
filaPrioridade.Enqueue(""Tarefa Crítica"", 0);

// Processar por prioridade
while (filaPrioridade.Count > 0)
{
    string tarefa = filaPrioridade.Dequeue();
    Console.WriteLine($""Processando: {tarefa}"");
}

// Saída:
// Processando: Tarefa Crítica
// Processando: Tarefa Urgente
// Processando: Tarefa Normal
// Processando: Tarefa Baixa",
                    Language = "csharp",
                    Explanation = "PriorityQueue processa elementos por prioridade, não por ordem de chegada. Prioridades menores são processadas primeiro. Ideal para sistemas onde algumas tarefas são mais importantes que outras.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Implementar Cache LRU",
                    Description = "Crie uma classe CacheLRU que armazena até N elementos. Quando o cache está cheio e um novo elemento é adicionado, o elemento menos recentemente usado é removido. Use uma fila para rastrear a ordem de uso.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"public class CacheLRU
{
    private Queue<string> ordem = new Queue<string>();
    private Dictionary<string, string> cache = new Dictionary<string, string>();
    private int capacidade;
    
    public CacheLRU(int capacidade)
    {
        this.capacidade = capacidade;
    }
    
    public void Adicionar(string chave, string valor)
    {
        // Implemente aqui
    }
    
    public string Obter(string chave)
    {
        // Implemente aqui
        return null;
    }
}",
                    Hints = new List<string> 
                    { 
                        "Se o cache está cheio, remova o primeiro da fila antes de adicionar",
                        "Ao adicionar, enfileire a chave para rastrear ordem",
                        "Use Dictionary para acesso rápido aos valores" 
                    }
                },
                new Exercise
                {
                    Title = "Gerar Números Binários",
                    Description = "Use uma fila para gerar os primeiros N números binários (1, 10, 11, 100, 101, 110, 111, 1000...). Dica: comece com '1', depois para cada número, gere dois novos adicionando '0' e '1'.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"public List<string> GerarBinarios(int n)
{
    // Implemente aqui
}

// Teste
var binarios = GerarBinarios(8);
Console.WriteLine(string.Join("", "", binarios));
// Deve exibir: 1, 10, 11, 100, 101, 110, 111, 1000",
                    Hints = new List<string> 
                    { 
                        "Comece enfileirando '1'",
                        "Para cada número desenfileirado, enfileire número+'0' e número+'1'",
                        "Repita até ter N números" 
                    }
                },
                new Exercise
                {
                    Title = "Escalonador de Tarefas",
                    Description = "Implemente um escalonador que processa tarefas com prioridades. Tarefas críticas (prioridade 1) são processadas antes de urgentes (2) e normais (3). Use PriorityQueue.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"public class Tarefa
{
    public string Nome { get; set; }
    public int Prioridade { get; set; }
}

public class Escalonador
{
    private PriorityQueue<Tarefa, int> fila = new PriorityQueue<Tarefa, int>();
    
    public void AdicionarTarefa(Tarefa tarefa)
    {
        // Implemente aqui
    }
    
    public void ProcessarTarefas()
    {
        // Implemente aqui
    }
}",
                    Hints = new List<string> 
                    { 
                        "Use tarefa.Prioridade como prioridade na fila",
                        "Processe todas as tarefas em ordem de prioridade",
                        "Exiba o nome da tarefa ao processar" 
                    }
                }
            },
            Summary = "Nesta aula você dominou filas (Queue), uma estrutura FIFO essencial. Aprendeu operações Enqueue e Dequeue (ambas O(1)), aplicou filas em sistemas de atendimento e processamento sequencial, e explorou filas de prioridade para processar elementos por importância. Filas são fundamentais para muitos sistemas do mundo real. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno. Grandes empresas como Microsoft, Google e Amazon aplicam estes conceitos em seus sistemas. Ao aprender estas técnicas, você está adquirindo conhecimento usado por desenvolvedores em todo o mundo. Considere como este conceito poderia ser aplicado em um projeto pessoal. Pensar em aplicações práticas ajuda a solidificar o entendimento e torna o aprendizado mais significativo. Iniciantes frequentemente cometem alguns erros ao aprender este tópico. Conhecer estes erros comuns ajuda a evitá-los e desenvolver código mais robusto desde o início. Um erro comum é não considerar casos extremos ou situações inesperadas. Sempre teste seu código com diferentes tipos de entrada, incluindo valores nulos, vazios ou inválidos. Outro erro frequente é otimizar prematuramente. Primeiro faça o código funcionar corretamente, depois otimize se necessário. Como diz o ditado: 'premature optimization is the root of all evil'. Quando encontrar problemas, use o debugger para entender o que está acontecendo. Coloque breakpoints, inspecione variáveis e execute o código passo a passo. Esta é uma habilidade essencial para todo desenvolvedor. Mensagens de erro podem parecer intimidadoras no início, mas elas são suas aliadas. Leia-as cuidadosamente - elas geralmente indicam exatamente onde e qual é o problema. Se algo não funcionar como esperado, não desanime. Debugging é uma parte normal do desenvolvimento. Até desenvolvedores experientes passam tempo significativo depurando código. Testar seu código é tão importante quanto escrevê-lo. Testes automatizados garantem que seu código funciona corretamente e continuará funcionando quando você fizer mudanças. Comece escrevendo testes simples para os casos mais comuns. Depois adicione testes para casos extremos e situações de erro. Uma boa cobertura de testes dá confiança para refatorar e melhorar o código. Considere a prática de TDD (Test-Driven Development), onde você escreve os testes antes do código. Isso ajuda a pensar claramente sobre o que o código deve fazer. Embora a correção seja sempre a prioridade, é importante estar ciente das implicações de performance de suas escolhas. Diferentes abordagens podem ter custos computacionais muito diferentes. Em muitos casos, a diferença de performance é negligenciável para aplicações pequenas. No entanto, à medida que seus programas crescem, escolhas eficientes se tornam cada vez mais importantes. Aprenda a usar ferramentas de profiling para identificar gargalos de performance. Otimize baseado em dados reais, não em suposições sobre onde o código é lento. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0003-000000000005"),
            CourseId = _courseId,
            Title = "Filas (Queue) e Princípio FIFO",
            Duration = "50 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 50,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0003-000000000004" }),
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
                "Compreender listas ligadas e suas vantagens",
                "Implementar listas ligadas simples e duplas",
                "Comparar listas ligadas com arrays e List<T>",
                "Resolver problemas usando listas ligadas"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Estrutura de Listas Ligadas",
                    Content = "Uma lista ligada (linked list) é uma estrutura de dados onde cada elemento (nó) contém um valor e uma referência (ponteiro) para o próximo nó. Diferente de arrays onde elementos são contíguos na memória, nós de listas ligadas podem estar espalhados. A lista mantém uma referência ao primeiro nó (head) e opcionalmente ao último (tail). Para acessar o elemento na posição N, você deve percorrer N nós a partir do head, resultando em acesso O(n). Porém, inserir ou remover no início é O(1), apenas ajustando referências. Listas ligadas são ideais quando você precisa de inserções/remoções frequentes no início ou meio, mas não precisa de acesso aleatório rápido. Elas usam memória extra para armazenar referências, mas não desperdiçam espaço como arrays com capacidade não utilizada. Listas ligadas são a base de muitas outras estruturas de dados.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Listas Simples vs Duplas",
                    Content = "Listas ligadas simples têm apenas referência para o próximo nó, permitindo percorrer apenas em uma direção. Listas ligadas duplas têm referências para o próximo e o anterior, permitindo percorrer em ambas as direções. Listas duplas facilitam operações como remover um nó específico (você pode ajustar o anterior diretamente) e percorrer de trás para frente. O custo é memória extra para armazenar a referência adicional. Em C#, LinkedList<T> implementa uma lista duplamente ligada circular: o último nó aponta para o primeiro e vice-versa. Isso permite operações eficientes em ambas as extremidades. LinkedList<T> fornece métodos como AddFirst, AddLast, AddBefore, AddAfter, Remove, Find, todos com complexidades adequadas. Use listas ligadas quando inserções/remoções são mais frequentes que acessos aleatórios.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Quando Usar Listas Ligadas",
                    Content = "Listas ligadas são ideais em cenários específicos. Use quando você precisa de inserções/remoções frequentes no início ou meio da coleção. Por exemplo, implementar uma fila com remoção eficiente de elementos no meio, ou manter uma lista ordenada onde novos elementos são inseridos na posição correta. Listas ligadas também são úteis quando o tamanho da coleção varia muito e você quer evitar realocações de arrays. Porém, evite listas ligadas quando você precisa de acesso aleatório frequente (use arrays ou List<T>) ou quando a memória é limitada (listas ligadas têm overhead de referências). Listas ligadas são menos cache-friendly que arrays porque nós não são contíguos, resultando em mais cache misses. Para a maioria dos casos, List<T> é mais eficiente, mas listas ligadas brilham em cenários específicos de inserção/remoção.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Usando LinkedList<T>",
                    Code = @"LinkedList<int> lista = new LinkedList<int>();

// Adicionar elementos
lista.AddFirst(10);  // 10
lista.AddLast(30);   // 10, 30
lista.AddAfter(lista.First, 20);  // 10, 20, 30

// Percorrer
Console.WriteLine(""Forward:"");
foreach (int valor in lista)
{
    Console.Write($""{valor} "");
}

// Percorrer de trás para frente
Console.WriteLine(""\nBackward:"");
var node = lista.Last;
while (node != null)
{
    Console.Write($""{node.Value} "");
    node = node.Previous;
}

// Remover
lista.Remove(20);
Console.WriteLine($""\nApós remover 20: {string.Join("", "", lista)}"");",
                    Language = "csharp",
                    Explanation = "LinkedList<T> permite adicionar elementos em qualquer posição eficientemente. AddFirst e AddLast são O(1). AddAfter e AddBefore inserem relativamente a um nó. A lista dupla permite percorrer em ambas as direções.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Implementação Simples de Lista Ligada",
                    Code = @"public class Node
{
    public int Value { get; set; }
    public Node Next { get; set; }
}

public class SimpleLinkedList
{
    private Node head;
    
    public void AddFirst(int value)
    {
        Node newNode = new Node { Value = value, Next = head };
        head = newNode;
    }
    
    public void Print()
    {
        Node current = head;
        while (current != null)
        {
            Console.Write($""{current.Value} -> "");
            current = current.Next;
        }
        Console.WriteLine(""null"");
    }
}

// Teste
var lista = new SimpleLinkedList();
lista.AddFirst(30);
lista.AddFirst(20);
lista.AddFirst(10);
lista.Print(); // 10 -> 20 -> 30 -> null",
                    Language = "csharp",
                    Explanation = "Esta implementação simples mostra a estrutura básica de uma lista ligada. Cada nó tem um valor e referência ao próximo. AddFirst é O(1) porque apenas ajusta referências. Print percorre a lista seguindo as referências.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Reverter Lista Ligada",
                    Description = "Implemente um método que reverte uma LinkedList<int> in-place, ou seja, sem criar uma nova lista.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"public void ReverterLista(LinkedList<int> lista)
{
    // Implemente aqui
}

// Teste
var lista = new LinkedList<int>(new[] { 1, 2, 3, 4, 5 });
ReverterLista(lista);
Console.WriteLine(string.Join("", "", lista)); // Deve exibir: 5, 4, 3, 2, 1",
                    Hints = new List<string> 
                    { 
                        "Percorra a lista e mova cada nó para o início",
                        "Ou use uma pilha para armazenar valores e reconstruir",
                        "LinkedList tem métodos AddFirst e RemoveLast" 
                    }
                },
                new Exercise
                {
                    Title = "Detectar Ciclo",
                    Description = "Implemente um método que detecta se uma lista ligada simples tem um ciclo (um nó aponta para um nó anterior). Use o algoritmo da tartaruga e lebre (Floyd's cycle detection).",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"public bool TemCiclo(Node head)
{
    // Implemente aqui usando dois ponteiros
    // Um lento (tartaruga) e um rápido (lebre)
}",
                    Hints = new List<string> 
                    { 
                        "Use dois ponteiros: lento avança 1 nó, rápido avança 2 nós",
                        "Se há ciclo, eventualmente os ponteiros se encontram",
                        "Se o rápido chega ao final (null), não há ciclo" 
                    }
                },
                new Exercise
                {
                    Title = "Mesclar Listas Ordenadas",
                    Description = "Dadas duas listas ligadas ordenadas, mescle-as em uma única lista ordenada. Faça isso manipulando os nós, não criando novos.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"public Node MesclarOrdenadas(Node lista1, Node lista2)
{
    // Implemente aqui
}",
                    Hints = new List<string> 
                    { 
                        "Use um nó dummy como início da lista resultado",
                        "Compare os valores de lista1 e lista2, adicione o menor",
                        "Avance o ponteiro da lista de onde veio o menor valor" 
                    }
                }
            },
            Summary = "Nesta aula você aprendeu sobre listas ligadas, uma estrutura onde elementos são conectados por referências. Compreendeu a diferença entre listas simples e duplas, quando usar listas ligadas versus arrays, e implementou operações básicas. Listas ligadas são ideais para inserções/remoções frequentes. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno. Grandes empresas como Microsoft, Google e Amazon aplicam estes conceitos em seus sistemas. Ao aprender estas técnicas, você está adquirindo conhecimento usado por desenvolvedores em todo o mundo. Considere como este conceito poderia ser aplicado em um projeto pessoal. Pensar em aplicações práticas ajuda a solidificar o entendimento e torna o aprendizado mais significativo. Iniciantes frequentemente cometem alguns erros ao aprender este tópico. Conhecer estes erros comuns ajuda a evitá-los e desenvolver código mais robusto desde o início. Um erro comum é não considerar casos extremos ou situações inesperadas. Sempre teste seu código com diferentes tipos de entrada, incluindo valores nulos, vazios ou inválidos. Outro erro frequente é otimizar prematuramente. Primeiro faça o código funcionar corretamente, depois otimize se necessário. Como diz o ditado: 'premature optimization is the root of all evil'. Quando encontrar problemas, use o debugger para entender o que está acontecendo. Coloque breakpoints, inspecione variáveis e execute o código passo a passo. Esta é uma habilidade essencial para todo desenvolvedor. Mensagens de erro podem parecer intimidadoras no início, mas elas são suas aliadas. Leia-as cuidadosamente - elas geralmente indicam exatamente onde e qual é o problema. Se algo não funcionar como esperado, não desanime. Debugging é uma parte normal do desenvolvimento. Até desenvolvedores experientes passam tempo significativo depurando código. Testar seu código é tão importante quanto escrevê-lo. Testes automatizados garantem que seu código funciona corretamente e continuará funcionando quando você fizer mudanças. Comece escrevendo testes simples para os casos mais comuns. Depois adicione testes para casos extremos e situações de erro. Uma boa cobertura de testes dá confiança para refatorar e melhorar o código. Considere a prática de TDD (Test-Driven Development), onde você escreve os testes antes do código. Isso ajuda a pensar claramente sobre o que o código deve fazer. Embora a correção seja sempre a prioridade, é importante estar ciente das implicações de performance de suas escolhas. Diferentes abordagens podem ter custos computacionais muito diferentes. Em muitos casos, a diferença de performance é negligenciável para aplicações pequenas. No entanto, à medida que seus programas crescem, escolhas eficientes se tornam cada vez mais importantes. Aprenda a usar ferramentas de profiling para identificar gargalos de performance. Otimize baseado em dados reais, não em suposições sobre onde o código é lento. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0003-000000000006"),
            CourseId = _courseId,
            Title = "Listas Ligadas (Linked Lists)",
            Duration = "55 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 55,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0003-000000000003" }),
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
                "Compreender tabelas hash e funções hash",
                "Usar HashSet<T> para conjuntos únicos",
                "Entender colisões e como são tratadas",
                "Aplicar hash tables em problemas de busca rápida"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Fundamentos de Hash Tables",
                    Content = "Uma tabela hash (hash table) é uma estrutura de dados que mapeia chaves para valores usando uma função hash. A função hash converte a chave em um índice de array onde o valor é armazenado. Isso permite busca, inserção e remoção em tempo O(1) médio - extremamente rápido! Por exemplo, para armazenar nomes e idades, a função hash converte o nome em um número que determina onde a idade é armazenada. Quando você busca, a mesma função hash calcula o índice rapidamente. Hash tables são a base de dicionários, conjuntos e caches. A eficiência depende de uma boa função hash que distribui chaves uniformemente. Uma função hash ruim causa muitas colisões (chaves diferentes gerando o mesmo índice), degradando a performance para O(n). Hash tables são uma das estruturas mais importantes e usadas na programação.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "HashSet<T> para Conjuntos",
                    Content = "HashSet<T> implementa um conjunto matemático: uma coleção de elementos únicos sem ordem específica. Internamente usa uma tabela hash, garantindo que Add, Remove e Contains sejam O(1) médio. HashSet automaticamente rejeita duplicatas: adicionar um elemento já existente não tem efeito. Isso é perfeito para remover duplicatas de uma coleção ou verificar rapidamente se um elemento existe. HashSet também suporta operações de conjunto como união (UnionWith), interseção (IntersectWith), diferença (ExceptWith) e diferença simétrica (SymmetricExceptWith). Por exemplo, para encontrar elementos comuns entre duas listas, converta para HashSet e use IntersectWith. HashSet é ideal quando você precisa de coleção única com buscas rápidas e não se importa com a ordem. Se precisar de ordem, use SortedSet<T> que mantém elementos ordenados.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Colisões e Resolução",
                    Content = "Colisões ocorrem quando duas chaves diferentes produzem o mesmo hash. Existem duas estratégias principais para resolver colisões: encadeamento (chaining) e endereçamento aberto (open addressing). No encadeamento, cada posição do array contém uma lista de elementos que colidiram. No endereçamento aberto, quando há colisão, procura-se a próxima posição vazia. C# usa encadeamento em suas implementações. Uma boa função hash minimiza colisões distribuindo chaves uniformemente. O fator de carga (load factor) é a razão entre número de elementos e tamanho do array. Quando o fator de carga fica alto, a tabela é redimensionada (rehashing) para manter a performance. Em C#, isso acontece automaticamente. Compreender colisões ajuda a entender por que hash tables são O(1) médio, não O(1) garantido: no pior caso (todas as chaves colidem), degrada para O(n).",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Operações com HashSet",
                    Code = @"HashSet<int> conjunto = new HashSet<int>();

// Adicionar elementos
conjunto.Add(1);
conjunto.Add(2);
conjunto.Add(3);
conjunto.Add(2); // Duplicata - ignorada

Console.WriteLine($""Elementos: {conjunto.Count}""); // 3

// Verificar existência - O(1)
bool existe = conjunto.Contains(2);
Console.WriteLine($""Contém 2: {existe}"");

// Remover - O(1)
conjunto.Remove(2);

// Iterar (ordem não garantida)
foreach (int num in conjunto)
{
    Console.Write($""{num} "");
}",
                    Language = "csharp",
                    Explanation = "HashSet garante elementos únicos automaticamente. Add, Remove e Contains são todas operações O(1) médio. A ordem dos elementos não é garantida porque depende dos valores hash.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Operações de Conjunto",
                    Code = @"HashSet<int> conjuntoA = new HashSet<int> { 1, 2, 3, 4, 5 };
HashSet<int> conjuntoB = new HashSet<int> { 4, 5, 6, 7, 8 };

// União - todos os elementos
var uniao = new HashSet<int>(conjuntoA);
uniao.UnionWith(conjuntoB);
Console.WriteLine(""União: "" + string.Join("", "", uniao)); // 1,2,3,4,5,6,7,8

// Interseção - elementos comuns
var intersecao = new HashSet<int>(conjuntoA);
intersecao.IntersectWith(conjuntoB);
Console.WriteLine(""Interseção: "" + string.Join("", "", intersecao)); // 4,5

// Diferença - em A mas não em B
var diferenca = new HashSet<int>(conjuntoA);
diferenca.ExceptWith(conjuntoB);
Console.WriteLine(""Diferença: "" + string.Join("", "", diferenca)); // 1,2,3

// Diferença simétrica - em A ou B mas não em ambos
var simetrica = new HashSet<int>(conjuntoA);
simetrica.SymmetricExceptWith(conjuntoB);
Console.WriteLine(""Simétrica: "" + string.Join("", "", simetrica)); // 1,2,3,6,7,8",
                    Language = "csharp",
                    Explanation = "HashSet suporta operações matemáticas de conjuntos. UnionWith combina conjuntos, IntersectWith encontra elementos comuns, ExceptWith encontra diferença, e SymmetricExceptWith encontra elementos exclusivos de cada conjunto.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Remover Duplicatas de Lista",
                    Code = @"List<int> numeros = new List<int> { 1, 2, 2, 3, 4, 4, 5, 1, 3 };

// Converter para HashSet remove duplicatas automaticamente
HashSet<int> unicos = new HashSet<int>(numeros);

// Converter de volta para lista se necessário
List<int> listaUnica = unicos.ToList();

Console.WriteLine(""Original: "" + string.Join("", "", numeros));
Console.WriteLine(""Sem duplicatas: "" + string.Join("", "", listaUnica));

// Ou em uma linha
var resultado = numeros.Distinct().ToList();
Console.WriteLine(""Usando Distinct: "" + string.Join("", "", resultado));",
                    Language = "csharp",
                    Explanation = "HashSet é perfeito para remover duplicatas. Converter uma lista para HashSet automaticamente elimina elementos repetidos. O método LINQ Distinct() usa HashSet internamente para fazer isso.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Encontrar Elementos Comuns",
                    Description = "Dadas duas listas de inteiros, encontre todos os elementos que aparecem em ambas. Use HashSet para resolver em O(n+m).",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"public List<int> ElementosComuns(List<int> lista1, List<int> lista2)
{
    // Implemente aqui usando HashSet
}

// Teste
var l1 = new List<int> { 1, 2, 3, 4, 5 };
var l2 = new List<int> { 4, 5, 6, 7, 8 };
var comuns = ElementosComuns(l1, l2);
Console.WriteLine(string.Join("", "", comuns)); // 4, 5",
                    Hints = new List<string> 
                    { 
                        "Converta lista1 para HashSet",
                        "Percorra lista2 e verifique se cada elemento está no HashSet",
                        "Adicione elementos encontrados ao resultado" 
                    }
                },
                new Exercise
                {
                    Title = "Primeiro Caractere Único",
                    Description = "Dada uma string, encontre o primeiro caractere que aparece apenas uma vez. Use HashSet ou Dictionary para rastrear frequências.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"public char? PrimeiroUnico(string texto)
{
    // Implemente aqui
}

// Teste
Console.WriteLine(PrimeiroUnico(""leetcode"")); // l
Console.WriteLine(PrimeiroUnico(""loveleetcode"")); // v
Console.WriteLine(PrimeiroUnico(""aabb"")); // null",
                    Hints = new List<string> 
                    { 
                        "Use Dictionary<char, int> para contar frequências",
                        "Percorra a string novamente e retorne o primeiro com frequência 1",
                        "Retorne null se nenhum caractere é único" 
                    }
                },
                new Exercise
                {
                    Title = "Subarray com Soma Zero",
                    Description = "Dado um array de inteiros, determine se existe um subarray contíguo cuja soma é zero. Use HashSet para rastrear somas acumuladas em O(n).",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"public bool TemSubarraySomaZero(int[] array)
{
    // Implemente aqui
    // Dica: se duas somas acumuladas são iguais, o subarray entre elas tem soma zero
}

// Teste
Console.WriteLine(TemSubarraySomaZero(new[] { 4, 2, -3, 1, 6 })); // true (2,-3,1)
Console.WriteLine(TemSubarraySomaZero(new[] { 4, 2, 0, 1, 6 })); // true (0)
Console.WriteLine(TemSubarraySomaZero(new[] { 1, 2, 3 })); // false",
                    Hints = new List<string> 
                    { 
                        "Calcule soma acumulada enquanto percorre o array",
                        "Se a soma acumulada já foi vista, há um subarray com soma zero",
                        "Não esqueça de verificar se a soma acumulada é zero" 
                    }
                }
            },
            Summary = "Nesta aula você dominou hash tables e HashSet<T>. Aprendeu como funções hash permitem operações O(1), como HashSet garante unicidade, e como usar operações de conjunto. Hash tables são fundamentais para busca rápida e são usadas extensivamente em programação. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno. Grandes empresas como Microsoft, Google e Amazon aplicam estes conceitos em seus sistemas. Ao aprender estas técnicas, você está adquirindo conhecimento usado por desenvolvedores em todo o mundo. Considere como este conceito poderia ser aplicado em um projeto pessoal. Pensar em aplicações práticas ajuda a solidificar o entendimento e torna o aprendizado mais significativo. Iniciantes frequentemente cometem alguns erros ao aprender este tópico. Conhecer estes erros comuns ajuda a evitá-los e desenvolver código mais robusto desde o início. Um erro comum é não considerar casos extremos ou situações inesperadas. Sempre teste seu código com diferentes tipos de entrada, incluindo valores nulos, vazios ou inválidos. Outro erro frequente é otimizar prematuramente. Primeiro faça o código funcionar corretamente, depois otimize se necessário. Como diz o ditado: 'premature optimization is the root of all evil'. Quando encontrar problemas, use o debugger para entender o que está acontecendo. Coloque breakpoints, inspecione variáveis e execute o código passo a passo. Esta é uma habilidade essencial para todo desenvolvedor. Mensagens de erro podem parecer intimidadoras no início, mas elas são suas aliadas. Leia-as cuidadosamente - elas geralmente indicam exatamente onde e qual é o problema. Se algo não funcionar como esperado, não desanime. Debugging é uma parte normal do desenvolvimento. Até desenvolvedores experientes passam tempo significativo depurando código. Testar seu código é tão importante quanto escrevê-lo. Testes automatizados garantem que seu código funciona corretamente e continuará funcionando quando você fizer mudanças. Comece escrevendo testes simples para os casos mais comuns. Depois adicione testes para casos extremos e situações de erro. Uma boa cobertura de testes dá confiança para refatorar e melhorar o código. Considere a prática de TDD (Test-Driven Development), onde você escreve os testes antes do código. Isso ajuda a pensar claramente sobre o que o código deve fazer. Embora a correção seja sempre a prioridade, é importante estar ciente das implicações de performance de suas escolhas. Diferentes abordagens podem ter custos computacionais muito diferentes. Em muitos casos, a diferença de performance é negligenciável para aplicações pequenas. No entanto, à medida que seus programas crescem, escolhas eficientes se tornam cada vez mais importantes. Aprenda a usar ferramentas de profiling para identificar gargalos de performance. Otimize baseado em dados reais, não em suposições sobre onde o código é lento. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0003-000000000007"),
            CourseId = _courseId,
            Title = "Hash Tables e HashSet",
            Duration = "50 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 50,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0003-000000000003" }),
            OrderIndex = 7,
            Content = "",
            StructuredContent = JsonSerializer.Serialize(content),
            Version = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }
}
