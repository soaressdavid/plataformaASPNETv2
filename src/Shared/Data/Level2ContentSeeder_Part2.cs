using Shared.Entities;
using Shared.Models;
using System.Text.Json;

namespace Shared.Data;

/// <summary>
/// Level 2 Content Seeder - Part 2 (Lessons 8-20)
/// </summary>
public partial class Level2ContentSeeder
{
    private Lesson CreateLesson8()
    {
        var content = new LessonContent
        {
            Objectives = new List<string>
            {
                "Compreender Dictionary<TKey, TValue> e mapeamento chave-valor",
                "Usar dicionários para busca e armazenamento eficientes",
                "Entender quando usar Dictionary vs outras estruturas",
                "Implementar caches e índices com dicionários"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Dicionários: Mapeamento Chave-Valor",
                    Content = "Dictionary<TKey, TValue> é uma das estruturas mais úteis em C#, mapeando chaves únicas para valores. Internamente usa uma tabela hash, garantindo que busca, inserção e remoção sejam O(1) médio. Cada chave pode aparecer apenas uma vez, mas valores podem se repetir. Por exemplo, um dicionário pode mapear nomes de usuários (chaves únicas) para idades (valores que podem se repetir). Dicionários são ideais para lookups rápidos: dado uma chave, obter o valor correspondente é instantâneo. Isso é muito mais rápido que percorrer uma lista procurando um elemento. Dicionários são usados para caches, índices, contadores de frequência, mapeamento de IDs para objetos, e muitos outros cenários. A sintaxe é simples: dict[chave] = valor para adicionar/atualizar, e dict[chave] para acessar. Dicionários são fundamentais para escrever código eficiente.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Operações e Métodos",
                    Content = "Dictionary fornece métodos poderosos. Add(chave, valor) adiciona um par, mas lança exceção se a chave já existe. dict[chave] = valor adiciona ou atualiza sem exceção. ContainsKey(chave) verifica se uma chave existe em O(1). TryGetValue(chave, out valor) tenta obter o valor e retorna bool indicando sucesso, evitando exceções. Remove(chave) remove o par. Keys e Values retornam coleções de chaves e valores respectivamente. Você pode iterar sobre o dicionário com foreach, onde cada elemento é um KeyValuePair<TKey, TValue>. Dicionários não garantem ordem de iteração. Se precisar de ordem, use SortedDictionary<TKey, TValue> que mantém chaves ordenadas, mas com operações O(log n). Para ordem de inserção, considere usar uma lista de pares ou uma estrutura customizada.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Padrões de Uso Comuns",
                    Content = "Dicionários são versáteis e aparecem em muitos padrões. Contadores de frequência: use Dictionary<T, int> para contar ocorrências de elementos. Agrupamento: use Dictionary<TKey, List<TValue>> para agrupar valores por chave. Cache: armazene resultados de computações caras em um dicionário para evitar recalcular. Índice: crie um dicionário mapeando IDs para objetos para acesso rápido. Memoização: armazene resultados de funções recursivas para evitar recomputação. Graph adjacency: use Dictionary<Node, List<Node>> para representar grafos. O padrão TryGetValue é importante: em vez de verificar ContainsKey e depois acessar (duas operações), TryGetValue faz ambos em uma operação. Sempre prefira TryGetValue quando possível. Dicionários são tão úteis que muitas linguagens os têm como tipo built-in.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Operações Básicas com Dictionary",
                    Code = @"Dictionary<string, int> idades = new Dictionary<string, int>();

// Adicionar
idades.Add(""Ana"", 25);
idades[""Bruno""] = 30; // Adiciona ou atualiza

// Acessar
int idadeAna = idades[""Ana""];
Console.WriteLine($""Ana tem {idadeAna} anos"");

// Verificar existência
if (idades.ContainsKey(""Carlos""))
{
    Console.WriteLine(""Carlos existe"");
}

// TryGetValue - mais eficiente
if (idades.TryGetValue(""Bruno"", out int idadeBruno))
{
    Console.WriteLine($""Bruno tem {idadeBruno} anos"");
}

// Remover
idades.Remove(""Ana"");

// Iterar
foreach (var par in idades)
{
    Console.WriteLine($""{par.Key}: {par.Value}"");
}",
                    Language = "csharp",
                    Explanation = "Este exemplo mostra operações fundamentais com Dictionary. TryGetValue é preferível a ContainsKey + acesso porque faz ambos em uma operação. Iterar retorna KeyValuePair com propriedades Key e Value.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Contador de Frequência",
                    Code = @"string texto = ""hello world"";
Dictionary<char, int> frequencias = new Dictionary<char, int>();

foreach (char c in texto)
{
    if (c == ' ') continue; // Ignorar espaços
    
    if (frequencias.ContainsKey(c))
    {
        frequencias[c]++;
    }
    else
    {
        frequencias[c] = 1;
    }
}

// Ou usando TryGetValue (mais eficiente)
foreach (char c in texto)
{
    if (c == ' ') continue;
    
    if (frequencias.TryGetValue(c, out int count))
    {
        frequencias[c] = count + 1;
    }
    else
    {
        frequencias[c] = 1;
    }
}

// Exibir frequências
foreach (var par in frequencias.OrderByDescending(p => p.Value))
{
    Console.WriteLine($""{par.Key}: {par.Value}"");
}",
                    Language = "csharp",
                    Explanation = "Contadores de frequência são um padrão comum. Para cada elemento, incrementamos seu contador ou inicializamos em 1 se não existe. TryGetValue é mais eficiente que ContainsKey + acesso.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Agrupamento com Dictionary",
                    Code = @"List<string> palavras = new List<string> 
{ 
    ""apple"", ""banana"", ""apricot"", ""blueberry"", ""avocado"" 
};

// Agrupar por primeira letra
Dictionary<char, List<string>> grupos = new Dictionary<char, List<string>>();

foreach (string palavra in palavras)
{
    char primeiraLetra = palavra[0];
    
    if (!grupos.ContainsKey(primeiraLetra))
    {
        grupos[primeiraLetra] = new List<string>();
    }
    
    grupos[primeiraLetra].Add(palavra);
}

// Exibir grupos
foreach (var grupo in grupos)
{
    Console.WriteLine($""{grupo.Key}: {string.Join("", "", grupo.Value)}"");
}

// Ou usando LINQ
var gruposLinq = palavras.GroupBy(p => p[0])
                         .ToDictionary(g => g.Key, g => g.ToList());",
                    Language = "csharp",
                    Explanation = "Agrupar elementos por uma chave é um padrão comum. Usamos Dictionary<TKey, List<TValue>> onde cada chave mapeia para uma lista de valores. LINQ fornece GroupBy para fazer isso de forma declarativa.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Dois Números com Soma Alvo",
                    Description = "Dado um array de inteiros e um valor alvo, encontre dois números que somam o alvo. Retorne seus índices. Use Dictionary para resolver em O(n).",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"public int[] DoisNumerosSoma(int[] numeros, int alvo)
{
    // Implemente aqui usando Dictionary
}

// Teste
int[] nums = { 2, 7, 11, 15 };
int[] resultado = DoisNumerosSoma(nums, 9);
Console.WriteLine($""Índices: {resultado[0]}, {resultado[1]}""); // 0, 1",
                    Hints = new List<string> 
                    { 
                        "Use Dictionary<int, int> para mapear valores para índices",
                        "Para cada número, calcule complemento = alvo - número",
                        "Verifique se o complemento já está no dicionário" 
                    }
                },
                new Exercise
                {
                    Title = "Anagramas",
                    Description = "Dadas duas strings, determine se são anagramas (mesmas letras em ordem diferente). Use Dictionary para contar frequências de caracteres.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"public bool SaoAnagramas(string s1, string s2)
{
    // Implemente aqui
}

// Teste
Console.WriteLine(SaoAnagramas(""listen"", ""silent"")); // true
Console.WriteLine(SaoAnagramas(""hello"", ""world"")); // false",
                    Hints = new List<string> 
                    { 
                        "Se os tamanhos são diferentes, não são anagramas",
                        "Conte frequências de caracteres em s1",
                        "Verifique se s2 tem as mesmas frequências" 
                    }
                },
                new Exercise
                {
                    Title = "Cache com Expiração",
                    Description = "Implemente um cache simples que armazena pares chave-valor com tempo de expiração. Valores expirados devem ser removidos automaticamente ao acessar.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"public class CacheComExpiracao<TKey, TValue>
{
    private Dictionary<TKey, (TValue valor, DateTime expiracao)> cache;
    
    public void Adicionar(TKey chave, TValue valor, TimeSpan tempoVida)
    {
        // Implemente aqui
    }
    
    public bool TryObter(TKey chave, out TValue valor)
    {
        // Implemente aqui
        // Remova se expirado
        valor = default;
        return false;
    }
}",
                    Hints = new List<string> 
                    { 
                        "Armazene valor e DateTime de expiração juntos",
                        "Ao adicionar, calcule expiracao = DateTime.Now + tempoVida",
                        "Ao obter, verifique se DateTime.Now < expiracao" 
                    }
                }
            },
            Summary = "Nesta aula você dominou Dictionary<TKey, TValue>, uma estrutura essencial para mapeamento chave-valor. Aprendeu operações O(1), padrões como contadores de frequência e agrupamento, e quando usar dicionários. Dicionários são fundamentais para código eficiente e aparecem em inúmeros cenários. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno. Grandes empresas como Microsoft, Google e Amazon aplicam estes conceitos em seus sistemas. Ao aprender estas técnicas, você está adquirindo conhecimento usado por desenvolvedores em todo o mundo. Considere como este conceito poderia ser aplicado em um projeto pessoal. Pensar em aplicações práticas ajuda a solidificar o entendimento e torna o aprendizado mais significativo. Iniciantes frequentemente cometem alguns erros ao aprender este tópico. Conhecer estes erros comuns ajuda a evitá-los e desenvolver código mais robusto desde o início. Um erro comum é não considerar casos extremos ou situações inesperadas. Sempre teste seu código com diferentes tipos de entrada, incluindo valores nulos, vazios ou inválidos. Outro erro frequente é otimizar prematuramente. Primeiro faça o código funcionar corretamente, depois otimize se necessário. Como diz o ditado: 'premature optimization is the root of all evil'. Quando encontrar problemas, use o debugger para entender o que está acontecendo. Coloque breakpoints, inspecione variáveis e execute o código passo a passo. Esta é uma habilidade essencial para todo desenvolvedor. Mensagens de erro podem parecer intimidadoras no início, mas elas são suas aliadas. Leia-as cuidadosamente - elas geralmente indicam exatamente onde e qual é o problema. Se algo não funcionar como esperado, não desanime. Debugging é uma parte normal do desenvolvimento. Até desenvolvedores experientes passam tempo significativo depurando código. Testar seu código é tão importante quanto escrevê-lo. Testes automatizados garantem que seu código funciona corretamente e continuará funcionando quando você fizer mudanças. Comece escrevendo testes simples para os casos mais comuns. Depois adicione testes para casos extremos e situações de erro. Uma boa cobertura de testes dá confiança para refatorar e melhorar o código. Considere a prática de TDD (Test-Driven Development), onde você escreve os testes antes do código. Isso ajuda a pensar claramente sobre o que o código deve fazer. Embora a correção seja sempre a prioridade, é importante estar ciente das implicações de performance de suas escolhas. Diferentes abordagens podem ter custos computacionais muito diferentes. Em muitos casos, a diferença de performance é negligenciável para aplicações pequenas. No entanto, à medida que seus programas crescem, escolhas eficientes se tornam cada vez mais importantes. Aprenda a usar ferramentas de profiling para identificar gargalos de performance. Otimize baseado em dados reais, não em suposições sobre onde o código é lento. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0003-000000000008"),
            CourseId = _courseId,
            Title = "Dicionários (Dictionary)",
            Duration = "50 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 50,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0003-000000000007" }),
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
                "Compreender árvores binárias e sua estrutura hierárquica",
                "Implementar nós e operações básicas de árvores",
                "Aprender diferentes formas de percorrer árvores",
                "Entender aplicações práticas de árvores"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Estrutura de Árvores",
                    Content = "Uma árvore é uma estrutura de dados hierárquica composta por nós conectados por arestas. O nó no topo é a raiz (root), e cada nó pode ter zero ou mais filhos. Nós sem filhos são folhas (leaves). Uma árvore binária é uma árvore onde cada nó tem no máximo dois filhos: esquerdo e direito. Árvores são usadas para representar hierarquias naturais: sistemas de arquivos, estruturas organizacionais, árvores de decisão, e muito mais. A altura de uma árvore é o comprimento do caminho mais longo da raiz até uma folha. Árvores balanceadas têm altura O(log n), permitindo operações eficientes. Árvores são fundamentais em ciência da computação: compiladores usam árvores sintáticas, bancos de dados usam B-trees para índices, e sistemas operacionais usam árvores para gerenciar memória.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Percursos em Árvores",
                    Content = "Existem três formas principais de percorrer árvores binárias: pré-ordem (pre-order), em-ordem (in-order) e pós-ordem (post-order). Pré-ordem visita raiz, depois subárvore esquerda, depois direita. Em-ordem visita esquerda, raiz, direita. Pós-ordem visita esquerda, direita, raiz. Em árvores binárias de busca, percurso em-ordem produz elementos em ordem crescente. Percursos podem ser implementados recursivamente (simples e elegante) ou iterativamente (usando pilha). Há também percurso em largura (breadth-first ou level-order) que visita nós nível por nível, usando uma fila. Cada tipo de percurso é útil para diferentes problemas: pré-ordem para copiar árvores, em-ordem para obter elementos ordenados, pós-ordem para deletar árvores, e largura para encontrar o caminho mais curto.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Árvores Binárias de Busca",
                    Content = "Uma árvore binária de busca (BST - Binary Search Tree) é uma árvore binária onde para cada nó, todos os valores na subárvore esquerda são menores e todos na direita são maiores. Essa propriedade permite busca eficiente: compare o valor procurado com a raiz, se menor vá para esquerda, se maior vá para direita. Em uma BST balanceada, busca, inserção e remoção são O(log n). Porém, se a árvore fica desbalanceada (por exemplo, inserindo elementos em ordem crescente), degrada para O(n), virando essencialmente uma lista ligada. Por isso, árvores auto-balanceadas como AVL e Red-Black são usadas na prática. Em C#, SortedSet e SortedDictionary usam árvores Red-Black internamente. BSTs são fundamentais para implementar conjuntos e mapas ordenados eficientemente.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Estrutura Básica de Árvore Binária",
                    Code = @"public class TreeNode
{
    public int Value { get; set; }
    public TreeNode Left { get; set; }
    public TreeNode Right { get; set; }
    
    public TreeNode(int value)
    {
        Value = value;
    }
}

// Criar uma árvore simples
//       1
//      / \
//     2   3
//    / \
//   4   5
TreeNode raiz = new TreeNode(1);
raiz.Left = new TreeNode(2);
raiz.Right = new TreeNode(3);
raiz.Left.Left = new TreeNode(4);
raiz.Left.Right = new TreeNode(5);",
                    Language = "csharp",
                    Explanation = "TreeNode é a estrutura básica de uma árvore binária. Cada nó tem um valor e referências para filhos esquerdo e direito. Árvores são construídas conectando nós através dessas referências.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Percursos Recursivos",
                    Code = @"public void PreOrdem(TreeNode node)
{
    if (node == null) return;
    Console.Write($""{node.Value} "");  // Visita raiz
    PreOrdem(node.Left);                // Esquerda
    PreOrdem(node.Right);               // Direita
}

public void EmOrdem(TreeNode node)
{
    if (node == null) return;
    EmOrdem(node.Left);                 // Esquerda
    Console.Write($""{node.Value} "");  // Visita raiz
    EmOrdem(node.Right);                // Direita
}

public void PosOrdem(TreeNode node)
{
    if (node == null) return;
    PosOrdem(node.Left);                // Esquerda
    PosOrdem(node.Right);               // Direita
    Console.Write($""{node.Value} "");  // Visita raiz
}

// Para a árvore acima:
// Pré-ordem: 1 2 4 5 3
// Em-ordem: 4 2 5 1 3
// Pós-ordem: 4 5 2 3 1",
                    Language = "csharp",
                    Explanation = "Os três percursos recursivos diferem apenas na ordem de visita. Pré-ordem processa raiz primeiro, em-ordem processa raiz entre filhos, pós-ordem processa raiz por último. A recursão torna o código elegante e simples.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Busca em Árvore Binária de Busca",
                    Code = @"public bool Buscar(TreeNode node, int valor)
{
    if (node == null) return false;
    
    if (valor == node.Value)
        return true;
    else if (valor < node.Value)
        return Buscar(node.Left, valor);  // Busca à esquerda
    else
        return Buscar(node.Right, valor); // Busca à direita
}

public TreeNode Inserir(TreeNode node, int valor)
{
    if (node == null)
        return new TreeNode(valor);
    
    if (valor < node.Value)
        node.Left = Inserir(node.Left, valor);
    else if (valor > node.Value)
        node.Right = Inserir(node.Right, valor);
    
    return node;
}

// Criar BST e buscar
TreeNode raiz = null;
raiz = Inserir(raiz, 5);
raiz = Inserir(raiz, 3);
raiz = Inserir(raiz, 7);
raiz = Inserir(raiz, 1);

Console.WriteLine(Buscar(raiz, 3));  // True
Console.WriteLine(Buscar(raiz, 10)); // False",
                    Language = "csharp",
                    Explanation = "Em uma BST, busca e inserção seguem a propriedade de ordenação: menores à esquerda, maiores à direita. Isso permite operações O(log n) em árvores balanceadas, muito mais rápido que busca linear O(n).",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Altura da Árvore",
                    Description = "Implemente um método que calcula a altura de uma árvore binária (número de arestas no caminho mais longo da raiz até uma folha).",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"public int Altura(TreeNode node)
{
    // Implemente aqui recursivamente
}

// Teste
// Para árvore: 1 -> 2 -> 4
//                -> 3
// Altura deve ser 2",
                    Hints = new List<string> 
                    { 
                        "Caso base: árvore vazia tem altura -1 (ou 0 dependendo da definição)",
                        "Altura = 1 + máximo entre altura da esquerda e direita",
                        "Use Math.Max para comparar alturas" 
                    }
                },
                new Exercise
                {
                    Title = "Verificar se é BST",
                    Description = "Implemente um método que verifica se uma árvore binária é uma árvore binária de busca válida.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"public bool EhBST(TreeNode node)
{
    // Implemente aqui
    // Dica: use um método auxiliar com limites min e max
}",
                    Hints = new List<string> 
                    { 
                        "Não basta verificar que esquerda < raiz < direita",
                        "Todos os nós da subárvore esquerda devem ser menores que a raiz",
                        "Use um método auxiliar: EhBSTAux(node, min, max)" 
                    }
                },
                new Exercise
                {
                    Title = "Percurso em Largura",
                    Description = "Implemente percurso em largura (level-order) que visita nós nível por nível. Use uma fila.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"public List<int> PercursoLargura(TreeNode raiz)
{
    // Implemente aqui usando Queue
}

// Para árvore:
//       1
//      / \
//     2   3
//    / \
//   4   5
// Deve retornar: [1, 2, 3, 4, 5]",
                    Hints = new List<string> 
                    { 
                        "Use uma fila para processar nós nível por nível",
                        "Enfileire a raiz, depois processe cada nó",
                        "Ao processar um nó, enfileire seus filhos" 
                    }
                }
            },
            Summary = "Nesta aula você aprendeu sobre árvores binárias, uma estrutura hierárquica fundamental. Compreendeu diferentes percursos (pré-ordem, em-ordem, pós-ordem, largura) e árvores binárias de busca que permitem operações O(log n). Árvores são essenciais para muitos algoritmos e aplicações. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno. Grandes empresas como Microsoft, Google e Amazon aplicam estes conceitos em seus sistemas. Ao aprender estas técnicas, você está adquirindo conhecimento usado por desenvolvedores em todo o mundo. Considere como este conceito poderia ser aplicado em um projeto pessoal. Pensar em aplicações práticas ajuda a solidificar o entendimento e torna o aprendizado mais significativo. Iniciantes frequentemente cometem alguns erros ao aprender este tópico. Conhecer estes erros comuns ajuda a evitá-los e desenvolver código mais robusto desde o início. Um erro comum é não considerar casos extremos ou situações inesperadas. Sempre teste seu código com diferentes tipos de entrada, incluindo valores nulos, vazios ou inválidos. Outro erro frequente é otimizar prematuramente. Primeiro faça o código funcionar corretamente, depois otimize se necessário. Como diz o ditado: 'premature optimization is the root of all evil'. Quando encontrar problemas, use o debugger para entender o que está acontecendo. Coloque breakpoints, inspecione variáveis e execute o código passo a passo. Esta é uma habilidade essencial para todo desenvolvedor. Mensagens de erro podem parecer intimidadoras no início, mas elas são suas aliadas. Leia-as cuidadosamente - elas geralmente indicam exatamente onde e qual é o problema. Se algo não funcionar como esperado, não desanime. Debugging é uma parte normal do desenvolvimento. Até desenvolvedores experientes passam tempo significativo depurando código. Testar seu código é tão importante quanto escrevê-lo. Testes automatizados garantem que seu código funciona corretamente e continuará funcionando quando você fizer mudanças. Comece escrevendo testes simples para os casos mais comuns. Depois adicione testes para casos extremos e situações de erro. Uma boa cobertura de testes dá confiança para refatorar e melhorar o código. Considere a prática de TDD (Test-Driven Development), onde você escreve os testes antes do código. Isso ajuda a pensar claramente sobre o que o código deve fazer. Embora a correção seja sempre a prioridade, é importante estar ciente das implicações de performance de suas escolhas. Diferentes abordagens podem ter custos computacionais muito diferentes. Em muitos casos, a diferença de performance é negligenciável para aplicações pequenas. No entanto, à medida que seus programas crescem, escolhas eficientes se tornam cada vez mais importantes. Aprenda a usar ferramentas de profiling para identificar gargalos de performance. Otimize baseado em dados reais, não em suposições sobre onde o código é lento. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0003-000000000009"),
            CourseId = _courseId,
            Title = "Árvores Binárias e Percursos",
            Duration = "60 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 60,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0003-000000000003" }),
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
                "Compreender grafos e suas representações",
                "Implementar grafos usando listas de adjacência e matrizes",
                "Aprender conceitos básicos: vértices, arestas, direcionados vs não-direcionados"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "O que são Grafos?",
                    Content = "Grafos são estruturas que modelam relações entre objetos. Um grafo consiste em vértices (nós) conectados por arestas. Grafos podem ser direcionados (arestas têm direção) ou não-direcionados (conexões bidirecionais). Exemplos incluem redes sociais (pessoas são vértices, amizades são arestas), mapas (cidades são vértices, estradas são arestas), e internet (páginas são vértices, links são arestas). Grafos podem ter pesos nas arestas representando distâncias, custos ou capacidades. Grafos são fundamentais para modelar problemas do mundo real e existem algoritmos poderosos para resolver problemas em grafos como encontrar caminhos mais curtos, detectar ciclos, e encontrar componentes conectados. Compreender grafos é essencial para resolver problemas complexos de forma eficiente.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Representações de Grafos",
                    Content = "Existem duas formas principais de representar grafos: lista de adjacência e matriz de adjacência. Lista de adjacência usa um dicionário ou array onde cada vértice mapeia para uma lista de seus vizinhos. É eficiente em espaço para grafos esparsos (poucas arestas) usando O(V+E) memória. Matriz de adjacência usa uma matriz 2D onde matriz[i][j] indica se há aresta entre vértices i e j. Usa O(V²) memória, eficiente para grafos densos (muitas arestas). Lista de adjacência é melhor para iterar sobre vizinhos O(grau do vértice), enquanto matriz permite verificar se há aresta entre dois vértices em O(1). A escolha depende da densidade do grafo e das operações mais frequentes. Para a maioria dos casos, lista de adjacência é preferível.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Aplicações Práticas",
                    Content = "Grafos têm inúmeras aplicações práticas. Redes sociais usam grafos para modelar conexões e recomendar amigos. GPS usa grafos para encontrar rotas mais curtas entre locais. Compiladores usam grafos de dependência para determinar ordem de compilação. Motores de busca usam grafos da web para ranquear páginas (PageRank). Sistemas de recomendação usam grafos para sugerir produtos ou conteúdo. Redes de computadores usam grafos para roteamento de pacotes. Jogos usam grafos para pathfinding (encontrar caminhos para personagens). Análise de redes sociais identifica influenciadores e comunidades. Grafos são uma das estruturas mais versáteis e poderosas em ciência da computação, permitindo modelar e resolver problemas complexos do mundo real.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Grafo com Lista de Adjacência",
                    Code = @"public class Grafo
{
    private Dictionary<int, List<int>> adjacencias;
    
    public Grafo()
    {
        adjacencias = new Dictionary<int, List<int>>();
    }
    
    public void AdicionarVertice(int v)
    {
        if (!adjacencias.ContainsKey(v))
        {
            adjacencias[v] = new List<int>();
        }
    }
    
    public void AdicionarAresta(int origem, int destino)
    {
        AdicionarVertice(origem);
        AdicionarVertice(destino);
        adjacencias[origem].Add(destino);
        // Para grafo não-direcionado, adicione também:
        // adjacencias[destino].Add(origem);
    }
    
    public List<int> ObterVizinhos(int v)
    {
        return adjacencias.ContainsKey(v) ? adjacencias[v] : new List<int>();
    }
}

// Uso
var grafo = new Grafo();
grafo.AdicionarAresta(1, 2);
grafo.AdicionarAresta(1, 3);
grafo.AdicionarAresta(2, 4);",
                    Language = "csharp",
                    Explanation = "Lista de adjacência usa Dictionary<int, List<int>> onde cada vértice mapeia para lista de vizinhos. É eficiente em espaço e permite iterar sobre vizinhos rapidamente.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Grafo com Matriz de Adjacência",
                    Code = @"public class GrafoMatriz
{
    private int[,] matriz;
    private int numVertices;
    
    public GrafoMatriz(int n)
    {
        numVertices = n;
        matriz = new int[n, n];
    }
    
    public void AdicionarAresta(int origem, int destino, int peso = 1)
    {
        matriz[origem, destino] = peso;
        // Para grafo não-direcionado:
        // matriz[destino, origem] = peso;
    }
    
    public bool TemAresta(int origem, int destino)
    {
        return matriz[origem, destino] != 0;
    }
    
    public void Imprimir()
    {
        for (int i = 0; i < numVertices; i++)
        {
            for (int j = 0; j < numVertices; j++)
            {
                Console.Write($""{matriz[i, j]} "");
            }
            Console.WriteLine();
        }
    }
}",
                    Language = "csharp",
                    Explanation = "Matriz de adjacência usa array 2D onde matriz[i][j] indica peso da aresta (0 = sem aresta). Permite verificar existência de aresta em O(1) mas usa O(V²) memória.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Contar Arestas",
                    Description = "Implemente um método que conta o número total de arestas em um grafo representado por lista de adjacência.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"public int ContarArestas(Dictionary<int, List<int>> grafo, bool direcionado)
{
    // Implemente aqui
}",
                    Hints = new List<string> 
                    { 
                        "Some o tamanho de todas as listas de adjacência",
                        "Para grafo não-direcionado, divida por 2 (cada aresta é contada duas vezes)",
                        "Para direcionado, não divida" 
                    }
                },
                new Exercise
                {
                    Title = "Verificar se Grafo é Completo",
                    Description = "Um grafo completo tem aresta entre todo par de vértices. Implemente um método que verifica se um grafo é completo.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"public bool EhCompleto(Dictionary<int, List<int>> grafo)
{
    // Implemente aqui
}",
                    Hints = new List<string> 
                    { 
                        "Em grafo completo com n vértices, cada vértice tem n-1 vizinhos",
                        "Verifique se todos os vértices têm grau n-1",
                        "Conte o número de vértices primeiro" 
                    }
                },
                new Exercise
                {
                    Title = "Converter Representações",
                    Description = "Implemente métodos para converter entre lista de adjacência e matriz de adjacência.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"public int[,] ListaParaMatriz(Dictionary<int, List<int>> lista, int n)
{
    // Implemente aqui
}

public Dictionary<int, List<int>> MatrizParaLista(int[,] matriz)
{
    // Implemente aqui
}",
                    Hints = new List<string> 
                    { 
                        "Para lista->matriz: percorra cada vértice e seus vizinhos",
                        "Para matriz->lista: percorra a matriz e adicione arestas onde matriz[i][j] != 0",
                        "Assuma que vértices são numerados de 0 a n-1" 
                    }
                }
            },
            Summary = "Nesta aula você aprendeu sobre grafos, estruturas que modelam relações entre objetos. Compreendeu as duas representações principais (lista e matriz de adjacência) e suas vantagens. Grafos são fundamentais para modelar problemas do mundo real como redes sociais, mapas e internet. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno. Grandes empresas como Microsoft, Google e Amazon aplicam estes conceitos em seus sistemas. Ao aprender estas técnicas, você está adquirindo conhecimento usado por desenvolvedores em todo o mundo. Considere como este conceito poderia ser aplicado em um projeto pessoal. Pensar em aplicações práticas ajuda a solidificar o entendimento e torna o aprendizado mais significativo. Iniciantes frequentemente cometem alguns erros ao aprender este tópico. Conhecer estes erros comuns ajuda a evitá-los e desenvolver código mais robusto desde o início. Um erro comum é não considerar casos extremos ou situações inesperadas. Sempre teste seu código com diferentes tipos de entrada, incluindo valores nulos, vazios ou inválidos. Outro erro frequente é otimizar prematuramente. Primeiro faça o código funcionar corretamente, depois otimize se necessário. Como diz o ditado: 'premature optimization is the root of all evil'. Quando encontrar problemas, use o debugger para entender o que está acontecendo. Coloque breakpoints, inspecione variáveis e execute o código passo a passo. Esta é uma habilidade essencial para todo desenvolvedor. Mensagens de erro podem parecer intimidadoras no início, mas elas são suas aliadas. Leia-as cuidadosamente - elas geralmente indicam exatamente onde e qual é o problema. Se algo não funcionar como esperado, não desanime. Debugging é uma parte normal do desenvolvimento. Até desenvolvedores experientes passam tempo significativo depurando código. Testar seu código é tão importante quanto escrevê-lo. Testes automatizados garantem que seu código funciona corretamente e continuará funcionando quando você fizer mudanças. Comece escrevendo testes simples para os casos mais comuns. Depois adicione testes para casos extremos e situações de erro. Uma boa cobertura de testes dá confiança para refatorar e melhorar o código. Considere a prática de TDD (Test-Driven Development), onde você escreve os testes antes do código. Isso ajuda a pensar claramente sobre o que o código deve fazer. Embora a correção seja sempre a prioridade, é importante estar ciente das implicações de performance de suas escolhas. Diferentes abordagens podem ter custos computacionais muito diferentes. Em muitos casos, a diferença de performance é negligenciável para aplicações pequenas. No entanto, à medida que seus programas crescem, escolhas eficientes se tornam cada vez mais importantes. Aprenda a usar ferramentas de profiling para identificar gargalos de performance. Otimize baseado em dados reais, não em suposições sobre onde o código é lento. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0003-00000000000a"),
            CourseId = _courseId,
            Title = "Introdução a Grafos",
            Duration = "55 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 55,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0003-000000000009" }),
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
                "Compreender busca em profundidade (DFS)",
                "Implementar DFS recursiva e iterativa",
                "Aplicar DFS em problemas práticos"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Busca em Profundidade",
                    Content = "DFS (Depth-First Search) explora um grafo indo o mais fundo possível antes de retroceder. Começa em um vértice, visita um vizinho não visitado, depois um vizinho desse vizinho, e assim por diante. Quando não há mais vizinhos não visitados, retrocede. DFS pode ser implementada recursivamente (usando a pilha de chamadas) ou iterativamente (usando uma pilha explícita). A complexidade é O(V+E) onde V é vértices e E é arestas. DFS é usada para detectar ciclos, encontrar componentes conectados, ordenação topológica, resolver labirintos e puzzles. DFS explora caminhos completamente antes de tentar outros, sendo ideal para problemas que requerem exploração exaustiva.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Implementação e Aplicações",
                    Content = "DFS requer rastrear vértices visitados para evitar ciclos infinitos. Use um HashSet para marcar visitados. A versão recursiva é elegante: visite o vértice atual, marque como visitado, depois chame DFS recursivamente para cada vizinho não visitado. A versão iterativa usa uma pilha: empilhe o vértice inicial, depois em loop desempilhe, processe e empilhe vizinhos não visitados. DFS é usada em muitos algoritmos: encontrar caminhos, detectar ciclos em grafos direcionados, ordenação topológica (ordem de dependências), resolver Sudoku e outros puzzles, e encontrar componentes fortemente conectados. Compreender DFS é fundamental para resolver problemas complexos em grafos.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "DFS vs BFS",
                    Content = "DFS e BFS (Breadth-First Search) são complementares. DFS usa pilha (ou recursão) e explora profundamente, enquanto BFS usa fila e explora em largura. DFS é melhor para: explorar todos os caminhos, detectar ciclos, ordenação topológica, e problemas que requerem backtracking. BFS é melhor para: encontrar caminho mais curto (em grafos não ponderados), encontrar nível de nós, e problemas que requerem exploração por níveis. Ambas têm complexidade O(V+E). A escolha depende do problema: use DFS quando precisar explorar completamente antes de retroceder, e BFS quando precisar encontrar a solução mais próxima. Dominar ambas é essencial para resolver problemas de grafos eficientemente.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "DFS Recursiva",
                    Code = @"public void DFS(Dictionary<int, List<int>> grafo, int inicio, HashSet<int> visitados)
{
    visitados.Add(inicio);
    Console.Write($""{inicio} "");
    
    foreach (int vizinho in grafo[inicio])
    {
        if (!visitados.Contains(vizinho))
        {
            DFS(grafo, vizinho, visitados);
        }
    }
}

// Uso
var grafo = new Dictionary<int, List<int>>
{
    { 1, new List<int> { 2, 3 } },
    { 2, new List<int> { 4 } },
    { 3, new List<int> { 5 } },
    { 4, new List<int>() },
    { 5, new List<int>() }
};

var visitados = new HashSet<int>();
DFS(grafo, 1, visitados); // Saída: 1 2 4 3 5",
                    Language = "csharp",
                    Explanation = "DFS recursiva é elegante: marca como visitado, processa, depois chama recursivamente para vizinhos não visitados. A recursão usa a pilha de chamadas implicitamente.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "DFS Iterativa",
                    Code = @"public void DFSIterativa(Dictionary<int, List<int>> grafo, int inicio)
{
    var visitados = new HashSet<int>();
    var pilha = new Stack<int>();
    
    pilha.Push(inicio);
    
    while (pilha.Count > 0)
    {
        int atual = pilha.Pop();
        
        if (visitados.Contains(atual))
            continue;
        
        visitados.Add(atual);
        Console.Write($""{atual} "");
        
        // Empilhar vizinhos não visitados
        foreach (int vizinho in grafo[atual])
        {
            if (!visitados.Contains(vizinho))
            {
                pilha.Push(vizinho);
            }
        }
    }
}",
                    Language = "csharp",
                    Explanation = "DFS iterativa usa pilha explícita. Desempilha vértice, marca como visitado, processa e empilha vizinhos não visitados. Útil quando recursão pode causar stack overflow.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Detectar Ciclo",
                    Description = "Use DFS para detectar se um grafo direcionado tem ciclo.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"public bool TemCiclo(Dictionary<int, List<int>> grafo)
{
    // Implemente aqui usando DFS
    // Dica: use três estados: não visitado, visitando, visitado
}",
                    Hints = new List<string> 
                    { 
                        "Use três estados: branco (não visitado), cinza (visitando), preto (visitado)",
                        "Se encontrar um vértice cinza durante DFS, há ciclo",
                        "Vértices pretos não indicam ciclo" 
                    }
                },
                new Exercise
                {
                    Title = "Contar Componentes Conectados",
                    Description = "Use DFS para contar quantos componentes conectados existem em um grafo não-direcionado.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"public int ContarComponentes(Dictionary<int, List<int>> grafo)
{
    // Implemente aqui
}",
                    Hints = new List<string> 
                    { 
                        "Inicie DFS de cada vértice não visitado",
                        "Cada DFS explora um componente completo",
                        "Conte quantas vezes você inicia DFS" 
                    }
                },
                new Exercise
                {
                    Title = "Encontrar Todos os Caminhos",
                    Description = "Use DFS para encontrar todos os caminhos entre dois vértices em um grafo.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"public List<List<int>> TodosCaminhos(Dictionary<int, List<int>> grafo, int origem, int destino)
{
    // Implemente aqui
}",
                    Hints = new List<string> 
                    { 
                        "Use DFS com backtracking",
                        "Mantenha caminho atual em uma lista",
                        "Quando chegar ao destino, adicione cópia do caminho ao resultado" 
                    }
                }
            },
            Summary = "Nesta aula você dominou DFS (Busca em Profundidade), um algoritmo fundamental para explorar grafos. Aprendeu implementações recursiva e iterativa, ambas O(V+E), e aplicou DFS em problemas como detectar ciclos e contar componentes. DFS é essencial para muitos algoritmos de grafos. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno. Grandes empresas como Microsoft, Google e Amazon aplicam estes conceitos em seus sistemas. Ao aprender estas técnicas, você está adquirindo conhecimento usado por desenvolvedores em todo o mundo. Considere como este conceito poderia ser aplicado em um projeto pessoal. Pensar em aplicações práticas ajuda a solidificar o entendimento e torna o aprendizado mais significativo. Iniciantes frequentemente cometem alguns erros ao aprender este tópico. Conhecer estes erros comuns ajuda a evitá-los e desenvolver código mais robusto desde o início. Um erro comum é não considerar casos extremos ou situações inesperadas. Sempre teste seu código com diferentes tipos de entrada, incluindo valores nulos, vazios ou inválidos. Outro erro frequente é otimizar prematuramente. Primeiro faça o código funcionar corretamente, depois otimize se necessário. Como diz o ditado: 'premature optimization is the root of all evil'. Quando encontrar problemas, use o debugger para entender o que está acontecendo. Coloque breakpoints, inspecione variáveis e execute o código passo a passo. Esta é uma habilidade essencial para todo desenvolvedor. Mensagens de erro podem parecer intimidadoras no início, mas elas são suas aliadas. Leia-as cuidadosamente - elas geralmente indicam exatamente onde e qual é o problema. Se algo não funcionar como esperado, não desanime. Debugging é uma parte normal do desenvolvimento. Até desenvolvedores experientes passam tempo significativo depurando código. Testar seu código é tão importante quanto escrevê-lo. Testes automatizados garantem que seu código funciona corretamente e continuará funcionando quando você fizer mudanças. Comece escrevendo testes simples para os casos mais comuns. Depois adicione testes para casos extremos e situações de erro. Uma boa cobertura de testes dá confiança para refatorar e melhorar o código. Considere a prática de TDD (Test-Driven Development), onde você escreve os testes antes do código. Isso ajuda a pensar claramente sobre o que o código deve fazer. Embora a correção seja sempre a prioridade, é importante estar ciente das implicações de performance de suas escolhas. Diferentes abordagens podem ter custos computacionais muito diferentes. Em muitos casos, a diferença de performance é negligenciável para aplicações pequenas. No entanto, à medida que seus programas crescem, escolhas eficientes se tornam cada vez mais importantes. Aprenda a usar ferramentas de profiling para identificar gargalos de performance. Otimize baseado em dados reais, não em suposições sobre onde o código é lento. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0003-00000000000b"),
            CourseId = _courseId,
            Title = "Busca em Profundidade (DFS)",
            Duration = "55 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 55,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0003-00000000000a" }),
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
                "Compreender busca em largura (BFS)",
                "Implementar BFS usando filas",
                "Encontrar caminhos mais curtos com BFS"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Busca em Largura",
                    Content = "BFS (Breadth-First Search) explora um grafo nível por nível. Começa no vértice inicial, visita todos os vizinhos diretos, depois os vizinhos dos vizinhos, e assim por diante. BFS usa uma fila: enfileira o vértice inicial, depois em loop desenfileira, processa e enfileira vizinhos não visitados. A complexidade é O(V+E). BFS garante encontrar o caminho mais curto em grafos não ponderados porque explora por distância crescente. É usada para encontrar menor número de saltos, verificar se grafo é bipartido, encontrar nível de nós, e resolver puzzles como menor número de movimentos. BFS é fundamental quando você precisa da solução mais próxima ou explorar por níveis.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Implementação e Aplicações",
                    Content = "BFS requer uma fila e um conjunto de visitados. Enfileire o início, marque como visitado. Em loop: desenfileire, processe, enfileire vizinhos não visitados. Para rastrear caminhos, mantenha um dicionário de predecessores: ao visitar um vizinho, registre de onde veio. Para reconstruir o caminho, siga os predecessores de trás para frente. BFS é usada em GPS para encontrar rotas com menos paradas, em redes sociais para sugerir amigos (amigos de amigos), em crawlers web para indexar páginas por proximidade, e em jogos para pathfinding quando todos os movimentos têm mesmo custo. BFS é mais simples que algoritmos como Dijkstra quando não há pesos nas arestas.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "BFS para Caminho Mais Curto",
                    Content = "Em grafos não ponderados, BFS encontra o caminho mais curto porque explora por distância. O primeiro caminho encontrado é o mais curto. Para rastrear distâncias, mantenha um dicionário de distâncias: distancia[inicio] = 0, e ao visitar vizinho, distancia[vizinho] = distancia[atual] + 1. Para reconstruir o caminho, use predecessores. BFS garante que quando um vértice é visitado pela primeira vez, é pela rota mais curta. Isso não vale para grafos ponderados (use Dijkstra). BFS é O(V+E), muito eficiente. É a escolha padrão para caminho mais curto em grafos não ponderados, sendo mais simples e rápida que alternativas.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "BFS Básica",
                    Code = @"public void BFS(Dictionary<int, List<int>> grafo, int inicio)
{
    var visitados = new HashSet<int>();
    var fila = new Queue<int>();
    
    fila.Enqueue(inicio);
    visitados.Add(inicio);
    
    while (fila.Count > 0)
    {
        int atual = fila.Dequeue();
        Console.Write($""{atual} "");
        
        foreach (int vizinho in grafo[atual])
        {
            if (!visitados.Contains(vizinho))
            {
                visitados.Add(vizinho);
                fila.Enqueue(vizinho);
            }
        }
    }
}",
                    Language = "csharp",
                    Explanation = "BFS usa fila para explorar nível por nível. Marca como visitado ao enfileirar (não ao desenfileirar) para evitar enfileirar duplicatas.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "BFS com Caminho Mais Curto",
                    Code = @"public List<int> CaminhoMaisCurto(Dictionary<int, List<int>> grafo, int inicio, int fim)
{
    var visitados = new HashSet<int>();
    var fila = new Queue<int>();
    var predecessores = new Dictionary<int, int>();
    
    fila.Enqueue(inicio);
    visitados.Add(inicio);
    
    while (fila.Count > 0)
    {
        int atual = fila.Dequeue();
        
        if (atual == fim)
        {
            // Reconstruir caminho
            var caminho = new List<int>();
            int v = fim;
            while (v != inicio)
            {
                caminho.Add(v);
                v = predecessores[v];
            }
            caminho.Add(inicio);
            caminho.Reverse();
            return caminho;
        }
        
        foreach (int vizinho in grafo[atual])
        {
            if (!visitados.Contains(vizinho))
            {
                visitados.Add(vizinho);
                predecessores[vizinho] = atual;
                fila.Enqueue(vizinho);
            }
        }
    }
    
    return new List<int>(); // Sem caminho
}",
                    Language = "csharp",
                    Explanation = "Rastreando predecessores, podemos reconstruir o caminho mais curto. Quando encontramos o destino, seguimos predecessores de trás para frente.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Distância Mínima",
                    Description = "Implemente um método que retorna a distância mínima (número de arestas) entre dois vértices usando BFS.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"public int DistanciaMinima(Dictionary<int, List<int>> grafo, int origem, int destino)
{
    // Implemente aqui
}",
                    Hints = new List<string> 
                    { 
                        "Use BFS mantendo distâncias em um dicionário",
                        "distancia[origem] = 0",
                        "distancia[vizinho] = distancia[atual] + 1" 
                    }
                },
                new Exercise
                {
                    Title = "Verificar se é Bipartido",
                    Description = "Use BFS para verificar se um grafo é bipartido (pode ser colorido com 2 cores sem vizinhos da mesma cor).",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"public bool EhBipartido(Dictionary<int, List<int>> grafo)
{
    // Implemente aqui
}",
                    Hints = new List<string> 
                    { 
                        "Use BFS atribuindo cores alternadas",
                        "Se encontrar vizinho com mesma cor, não é bipartido",
                        "Trate componentes desconectados" 
                    }
                },
                new Exercise
                {
                    Title = "Menor Número de Movimentos",
                    Description = "Em um tabuleiro 8x8, encontre o menor número de movimentos para um cavalo ir de uma posição para outra. Use BFS.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"public int MovimentosCavalo((int x, int y) inicio, (int x, int y) fim)
{
    // Implemente aqui
    // Cavalo move em L: (±2,±1) ou (±1,±2)
}",
                    Hints = new List<string> 
                    { 
                        "Trate cada posição como vértice",
                        "Vizinhos são posições alcançáveis em um movimento",
                        "Use BFS para encontrar caminho mais curto" 
                    }
                }
            },
            Summary = "Nesta aula você dominou BFS (Busca em Largura), algoritmo que explora grafos nível por nível usando filas. BFS garante encontrar o caminho mais curto em grafos não ponderados e é O(V+E). É fundamental para problemas que requerem a solução mais próxima. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno. Grandes empresas como Microsoft, Google e Amazon aplicam estes conceitos em seus sistemas. Ao aprender estas técnicas, você está adquirindo conhecimento usado por desenvolvedores em todo o mundo. Considere como este conceito poderia ser aplicado em um projeto pessoal. Pensar em aplicações práticas ajuda a solidificar o entendimento e torna o aprendizado mais significativo. Iniciantes frequentemente cometem alguns erros ao aprender este tópico. Conhecer estes erros comuns ajuda a evitá-los e desenvolver código mais robusto desde o início. Um erro comum é não considerar casos extremos ou situações inesperadas. Sempre teste seu código com diferentes tipos de entrada, incluindo valores nulos, vazios ou inválidos. Outro erro frequente é otimizar prematuramente. Primeiro faça o código funcionar corretamente, depois otimize se necessário. Como diz o ditado: 'premature optimization is the root of all evil'. Quando encontrar problemas, use o debugger para entender o que está acontecendo. Coloque breakpoints, inspecione variáveis e execute o código passo a passo. Esta é uma habilidade essencial para todo desenvolvedor. Mensagens de erro podem parecer intimidadoras no início, mas elas são suas aliadas. Leia-as cuidadosamente - elas geralmente indicam exatamente onde e qual é o problema. Se algo não funcionar como esperado, não desanime. Debugging é uma parte normal do desenvolvimento. Até desenvolvedores experientes passam tempo significativo depurando código. Testar seu código é tão importante quanto escrevê-lo. Testes automatizados garantem que seu código funciona corretamente e continuará funcionando quando você fizer mudanças. Comece escrevendo testes simples para os casos mais comuns. Depois adicione testes para casos extremos e situações de erro. Uma boa cobertura de testes dá confiança para refatorar e melhorar o código. Considere a prática de TDD (Test-Driven Development), onde você escreve os testes antes do código. Isso ajuda a pensar claramente sobre o que o código deve fazer. Embora a correção seja sempre a prioridade, é importante estar ciente das implicações de performance de suas escolhas. Diferentes abordagens podem ter custos computacionais muito diferentes. Em muitos casos, a diferença de performance é negligenciável para aplicações pequenas. No entanto, à medida que seus programas crescem, escolhas eficientes se tornam cada vez mais importantes. Aprenda a usar ferramentas de profiling para identificar gargalos de performance. Otimize baseado em dados reais, não em suposições sobre onde o código é lento. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0003-00000000000c"),
            CourseId = _courseId,
            Title = "Busca em Largura (BFS)",
            Duration = "55 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 55,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0003-00000000000b" }),
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
                "Compreender busca linear e binária",
                "Implementar algoritmos de busca eficientes",
                "Analisar complexidade de algoritmos de busca"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Busca Linear",
                    Content = "Busca linear percorre um array elemento por elemento até encontrar o valor procurado ou chegar ao final. É O(n) no pior caso, mas funciona em arrays não ordenados. É simples de implementar e ideal para coleções pequenas ou quando você precisa buscar apenas uma vez. Para múltiplas buscas, considere ordenar primeiro e usar busca binária, ou usar estruturas como HashSet ou Dictionary que oferecem busca O(1). Busca linear é também usada quando você precisa encontrar todos os elementos que satisfazem uma condição, não apenas o primeiro. Apesar de ser lenta para grandes conjuntos, sua simplicidade e versatilidade a tornam útil em muitos cenários práticos.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Busca Binária",
                    Content = "Busca binária funciona apenas em arrays ordenados, mas é muito mais rápida: O(log n). A ideia é dividir o espaço de busca pela metade a cada iteração. Compare o elemento do meio com o valor procurado: se igual, encontrou; se menor, busque na metade direita; se maior, busque na esquerda. Repita até encontrar ou o espaço ficar vazio. Busca binária pode ser implementada recursiva ou iterativamente. É extremamente eficiente: em um array de 1 milhão de elementos, busca binária precisa de no máximo 20 comparações, enquanto busca linear pode precisar de 1 milhão. Busca binária é fundamental e aparece em muitos algoritmos e problemas.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Quando Usar Cada Busca",
                    Content = "Use busca linear quando: array não está ordenado e ordenar seria mais caro que buscar, array é pequeno (menos de 100 elementos), você precisa buscar apenas uma vez, ou precisa encontrar todos os elementos que satisfazem uma condição. Use busca binária quando: array está ordenado, você fará múltiplas buscas (vale a pena ordenar primeiro), array é grande, ou você precisa de performance O(log n). Para buscas muito frequentes, considere estruturas como HashSet ou Dictionary que oferecem O(1). A escolha do algoritmo de busca pode fazer enorme diferença na performance de aplicações que processam grandes volumes de dados.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Busca Linear",
                    Code = @"public int BuscaLinear(int[] array, int valor)
{
    for (int i = 0; i < array.Length; i++)
    {
        if (array[i] == valor)
        {
            return i; // Retorna índice
        }
    }
    return -1; // Não encontrado
}

// Teste
int[] numeros = { 5, 2, 8, 1, 9 };
int indice = BuscaLinear(numeros, 8);
Console.WriteLine($""Encontrado no índice: {indice}""); // 2",
                    Language = "csharp",
                    Explanation = "Busca linear é simples: percorre o array comparando cada elemento. Retorna o índice quando encontra ou -1 se não encontrar. É O(n) mas funciona em arrays não ordenados.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Busca Binária Iterativa",
                    Code = @"public int BuscaBinaria(int[] array, int valor)
{
    int esquerda = 0;
    int direita = array.Length - 1;
    
    while (esquerda <= direita)
    {
        int meio = esquerda + (direita - esquerda) / 2;
        
        if (array[meio] == valor)
        {
            return meio;
        }
        else if (array[meio] < valor)
        {
            esquerda = meio + 1; // Buscar na direita
        }
        else
        {
            direita = meio - 1; // Buscar na esquerda
        }
    }
    
    return -1; // Não encontrado
}

// Teste
int[] numeros = { 1, 2, 5, 8, 9 }; // Deve estar ordenado!
int indice = BuscaBinaria(numeros, 5);
Console.WriteLine($""Encontrado no índice: {indice}""); // 2",
                    Language = "csharp",
                    Explanation = "Busca binária divide o espaço pela metade a cada iteração. Use (esquerda + direita) / 2 cuidadosamente para evitar overflow. É O(log n) mas requer array ordenado.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Busca Binária Recursiva",
                    Code = @"public int BuscaBinariaRecursiva(int[] array, int valor, int esquerda, int direita)
{
    if (esquerda > direita)
    {
        return -1; // Não encontrado
    }
    
    int meio = esquerda + (direita - esquerda) / 2;
    
    if (array[meio] == valor)
    {
        return meio;
    }
    else if (array[meio] < valor)
    {
        return BuscaBinariaRecursiva(array, valor, meio + 1, direita);
    }
    else
    {
        return BuscaBinariaRecursiva(array, valor, esquerda, meio - 1);
    }
}

// Uso
int[] numeros = { 1, 2, 5, 8, 9 };
int indice = BuscaBinariaRecursiva(numeros, 8, 0, numeros.Length - 1);",
                    Language = "csharp",
                    Explanation = "Versão recursiva de busca binária é mais elegante mas usa espaço na pilha O(log n). A versão iterativa é preferível para arrays muito grandes.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Primeira e Última Ocorrência",
                    Description = "Em um array ordenado com duplicatas, encontre a primeira e última ocorrência de um valor usando busca binária modificada.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"public (int primeira, int ultima) EncontrarRange(int[] array, int valor)
{
    // Implemente aqui
}",
                    Hints = new List<string> 
                    { 
                        "Use busca binária duas vezes: uma para primeira, outra para última",
                        "Para primeira: quando encontrar, continue buscando à esquerda",
                        "Para última: quando encontrar, continue buscando à direita" 
                    }
                },
                new Exercise
                {
                    Title = "Buscar em Array Rotacionado",
                    Description = "Um array ordenado foi rotacionado (ex: [4,5,6,7,0,1,2]). Encontre um elemento usando busca binária modificada em O(log n).",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"public int BuscarRotacionado(int[] array, int valor)
{
    // Implemente aqui
}",
                    Hints = new List<string> 
                    { 
                        "Uma metade sempre está ordenada",
                        "Determine qual metade está ordenada",
                        "Verifique se o valor está nessa metade ordenada" 
                    }
                },
                new Exercise
                {
                    Title = "Raiz Quadrada Inteira",
                    Description = "Calcule a raiz quadrada inteira de um número usando busca binária (sem usar Math.Sqrt).",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"public int RaizQuadrada(int n)
{
    // Implemente aqui usando busca binária
}",
                    Hints = new List<string> 
                    { 
                        "Busque entre 0 e n",
                        "Para cada meio, verifique se meio * meio == n",
                        "Retorne o maior inteiro cujo quadrado é <= n" 
                    }
                }
            },
            Summary = "Nesta aula você dominou algoritmos de busca: linear O(n) para arrays não ordenados e binária O(log n) para arrays ordenados. Busca binária é extremamente eficiente e fundamental para muitos problemas. Saber quando usar cada algoritmo é essencial para escrever código eficiente. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno. Grandes empresas como Microsoft, Google e Amazon aplicam estes conceitos em seus sistemas. Ao aprender estas técnicas, você está adquirindo conhecimento usado por desenvolvedores em todo o mundo. Considere como este conceito poderia ser aplicado em um projeto pessoal. Pensar em aplicações práticas ajuda a solidificar o entendimento e torna o aprendizado mais significativo. Iniciantes frequentemente cometem alguns erros ao aprender este tópico. Conhecer estes erros comuns ajuda a evitá-los e desenvolver código mais robusto desde o início. Um erro comum é não considerar casos extremos ou situações inesperadas. Sempre teste seu código com diferentes tipos de entrada, incluindo valores nulos, vazios ou inválidos. Outro erro frequente é otimizar prematuramente. Primeiro faça o código funcionar corretamente, depois otimize se necessário. Como diz o ditado: 'premature optimization is the root of all evil'. Quando encontrar problemas, use o debugger para entender o que está acontecendo. Coloque breakpoints, inspecione variáveis e execute o código passo a passo. Esta é uma habilidade essencial para todo desenvolvedor. Mensagens de erro podem parecer intimidadoras no início, mas elas são suas aliadas. Leia-as cuidadosamente - elas geralmente indicam exatamente onde e qual é o problema. Se algo não funcionar como esperado, não desanime. Debugging é uma parte normal do desenvolvimento. Até desenvolvedores experientes passam tempo significativo depurando código. Testar seu código é tão importante quanto escrevê-lo. Testes automatizados garantem que seu código funciona corretamente e continuará funcionando quando você fizer mudanças. Comece escrevendo testes simples para os casos mais comuns. Depois adicione testes para casos extremos e situações de erro. Uma boa cobertura de testes dá confiança para refatorar e melhorar o código. Considere a prática de TDD (Test-Driven Development), onde você escreve os testes antes do código. Isso ajuda a pensar claramente sobre o que o código deve fazer. Embora a correção seja sempre a prioridade, é importante estar ciente das implicações de performance de suas escolhas. Diferentes abordagens podem ter custos computacionais muito diferentes. Em muitos casos, a diferença de performance é negligenciável para aplicações pequenas. No entanto, à medida que seus programas crescem, escolhas eficientes se tornam cada vez mais importantes. Aprenda a usar ferramentas de profiling para identificar gargalos de performance. Otimize baseado em dados reais, não em suposições sobre onde o código é lento. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0003-00000000000d"),
            CourseId = _courseId,
            Title = "Algoritmos de Busca",
            Duration = "50 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 50,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0003-000000000002" }),
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
                "Compreender algoritmos de ordenação simples",
                "Implementar Bubble Sort, Selection Sort e Insertion Sort",
                "Analisar complexidade e quando usar cada algoritmo"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Algoritmos de Ordenação Simples",
                    Content = "Algoritmos de ordenação simples são fáceis de entender e implementar, mas têm complexidade O(n²), sendo lentos para grandes conjuntos. Bubble Sort compara pares adjacentes e troca se estiverem fora de ordem, repetindo até o array estar ordenado. Selection Sort encontra o menor elemento e coloca na primeira posição, depois o segundo menor na segunda posição, e assim por diante. Insertion Sort constrói o array ordenado um elemento por vez, inserindo cada novo elemento na posição correta. Apesar de serem lentos, esses algoritmos são úteis para ensinar conceitos de ordenação, para arrays pequenos (menos de 50 elementos), ou quando o array já está quase ordenado (Insertion Sort é O(n) nesse caso).",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Comparação e Características",
                    Content = "Bubble Sort é O(n²) no pior e médio caso, mas O(n) no melhor caso (array já ordenado). É estável (mantém ordem relativa de elementos iguais) e in-place (não usa memória extra). Selection Sort é sempre O(n²), não é estável mas é in-place. Insertion Sort é O(n²) no pior caso mas O(n) quando array está quase ordenado, é estável e in-place. Para arrays pequenos ou quase ordenados, Insertion Sort é a melhor escolha. Para arrays grandes, use algoritmos mais eficientes como Quick Sort ou Merge Sort. Compreender esses algoritmos simples é fundamental antes de aprender algoritmos mais complexos.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Aplicações Práticas",
                    Content = "Apesar de serem O(n²), algoritmos simples têm aplicações práticas. Insertion Sort é usado em algoritmos híbridos como Timsort (usado em Python e Java) para ordenar pequenas partições. É também eficiente para ordenar dados que chegam em tempo real (streaming). Selection Sort é útil quando o custo de escrita é muito maior que leitura (como em memória flash), pois minimiza o número de escritas. Bubble Sort raramente é usado na prática, mas é excelente para ensino. Em geral, para produção use Array.Sort() em C# que implementa Introsort (híbrido de Quick Sort, Heap Sort e Insertion Sort), garantindo O(n log n) no pior caso.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Bubble Sort",
                    Code = @"public void BubbleSort(int[] array)
{
    int n = array.Length;
    for (int i = 0; i < n - 1; i++)
    {
        bool trocou = false;
        for (int j = 0; j < n - i - 1; j++)
        {
            if (array[j] > array[j + 1])
            {
                // Trocar
                int temp = array[j];
                array[j] = array[j + 1];
                array[j + 1] = temp;
                trocou = true;
            }
        }
        if (!trocou) break; // Otimização: se não trocou, já está ordenado
    }
}

int[] numeros = { 64, 34, 25, 12, 22, 11, 90 };
BubbleSort(numeros);
Console.WriteLine(string.Join("", "", numeros));",
                    Language = "csharp",
                    Explanation = "Bubble Sort compara pares adjacentes e troca se necessário. A flag 'trocou' otimiza para arrays já ordenados, tornando o melhor caso O(n).",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Selection Sort",
                    Code = @"public void SelectionSort(int[] array)
{
    int n = array.Length;
    for (int i = 0; i < n - 1; i++)
    {
        int minIndex = i;
        for (int j = i + 1; j < n; j++)
        {
            if (array[j] < array[minIndex])
            {
                minIndex = j;
            }
        }
        // Trocar mínimo com posição i
        int temp = array[i];
        array[i] = array[minIndex];
        array[minIndex] = temp;
    }
}",
                    Language = "csharp",
                    Explanation = "Selection Sort encontra o menor elemento e coloca na primeira posição não ordenada. Faz sempre O(n²) comparações mas minimiza trocas.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Insertion Sort",
                    Code = @"public void InsertionSort(int[] array)
{
    int n = array.Length;
    for (int i = 1; i < n; i++)
    {
        int chave = array[i];
        int j = i - 1;
        
        // Move elementos maiores que chave uma posição à frente
        while (j >= 0 && array[j] > chave)
        {
            array[j + 1] = array[j];
            j--;
        }
        array[j + 1] = chave;
    }
}",
                    Language = "csharp",
                    Explanation = "Insertion Sort insere cada elemento na posição correta na parte já ordenada. É eficiente para arrays pequenos ou quase ordenados.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Contar Trocas",
                    Description = "Modifique Bubble Sort para contar e retornar o número de trocas realizadas.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"public int BubbleSortComContador(int[] array)
{
    // Implemente aqui
}",
                    Hints = new List<string> 
                    { 
                        "Adicione uma variável contador",
                        "Incremente a cada troca",
                        "Retorne o contador no final" 
                    }
                },
                new Exercise
                {
                    Title = "Ordenação Estável",
                    Description = "Verifique se Selection Sort é estável implementando um teste com objetos que têm chave e valor.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"public class Item
{
    public int Chave { get; set; }
    public string Valor { get; set; }
}

public void TestarEstabilidade()
{
    // Implemente aqui
}",
                    Hints = new List<string> 
                    { 
                        "Crie items com mesma chave mas valores diferentes",
                        "Ordene por chave",
                        "Verifique se a ordem relativa dos valores foi mantida" 
                    }
                },
                new Exercise
                {
                    Title = "Insertion Sort Binário",
                    Description = "Otimize Insertion Sort usando busca binária para encontrar a posição de inserção.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"public void InsertionSortBinario(int[] array)
{
    // Implemente aqui
}",
                    Hints = new List<string> 
                    { 
                        "Use busca binária na parte ordenada",
                        "Encontre a posição correta em O(log n)",
                        "Ainda precisa mover elementos, então complexidade total é O(n²)" 
                    }
                }
            },
            Summary = "Nesta aula você aprendeu algoritmos de ordenação simples: Bubble Sort, Selection Sort e Insertion Sort. Todos são O(n²) mas têm características diferentes. Insertion Sort é o melhor para arrays pequenos ou quase ordenados. Para arrays grandes, use algoritmos mais eficientes. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno. Grandes empresas como Microsoft, Google e Amazon aplicam estes conceitos em seus sistemas. Ao aprender estas técnicas, você está adquirindo conhecimento usado por desenvolvedores em todo o mundo. Considere como este conceito poderia ser aplicado em um projeto pessoal. Pensar em aplicações práticas ajuda a solidificar o entendimento e torna o aprendizado mais significativo. Iniciantes frequentemente cometem alguns erros ao aprender este tópico. Conhecer estes erros comuns ajuda a evitá-los e desenvolver código mais robusto desde o início. Um erro comum é não considerar casos extremos ou situações inesperadas. Sempre teste seu código com diferentes tipos de entrada, incluindo valores nulos, vazios ou inválidos. Outro erro frequente é otimizar prematuramente. Primeiro faça o código funcionar corretamente, depois otimize se necessário. Como diz o ditado: 'premature optimization is the root of all evil'. Quando encontrar problemas, use o debugger para entender o que está acontecendo. Coloque breakpoints, inspecione variáveis e execute o código passo a passo. Esta é uma habilidade essencial para todo desenvolvedor. Mensagens de erro podem parecer intimidadoras no início, mas elas são suas aliadas. Leia-as cuidadosamente - elas geralmente indicam exatamente onde e qual é o problema. Se algo não funcionar como esperado, não desanime. Debugging é uma parte normal do desenvolvimento. Até desenvolvedores experientes passam tempo significativo depurando código. Testar seu código é tão importante quanto escrevê-lo. Testes automatizados garantem que seu código funciona corretamente e continuará funcionando quando você fizer mudanças. Comece escrevendo testes simples para os casos mais comuns. Depois adicione testes para casos extremos e situações de erro. Uma boa cobertura de testes dá confiança para refatorar e melhorar o código. Considere a prática de TDD (Test-Driven Development), onde você escreve os testes antes do código. Isso ajuda a pensar claramente sobre o que o código deve fazer. Embora a correção seja sempre a prioridade, é importante estar ciente das implicações de performance de suas escolhas. Diferentes abordagens podem ter custos computacionais muito diferentes. Em muitos casos, a diferença de performance é negligenciável para aplicações pequenas. No entanto, à medida que seus programas crescem, escolhas eficientes se tornam cada vez mais importantes. Aprenda a usar ferramentas de profiling para identificar gargalos de performance. Otimize baseado em dados reais, não em suposições sobre onde o código é lento. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0003-00000000000e"),
            CourseId = _courseId,
            Title = "Algoritmos de Ordenação Simples",
            Duration = "55 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 55,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0003-00000000000d" }),
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
                "Compreender Merge Sort e dividir para conquistar",
                "Implementar Merge Sort recursivo",
                "Analisar complexidade O(n log n)"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Merge Sort e Dividir para Conquistar",
                    Content = "Merge Sort usa a estratégia dividir para conquistar: divide o array em duas metades, ordena cada metade recursivamente, depois mescla as metades ordenadas. A complexidade é O(n log n) em todos os casos (melhor, médio e pior), tornando-o muito mais eficiente que algoritmos O(n²) para arrays grandes. Merge Sort é estável (mantém ordem relativa) e previsível (sempre O(n log n)). A desvantagem é usar O(n) memória extra para o array temporário durante a mesclagem. Merge Sort é usado em situações onde estabilidade é importante ou quando você precisa de performance garantida. É também a base do Timsort, usado em Python e Java.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Implementação e Análise",
                    Content = "Merge Sort tem duas partes: dividir e mesclar. A divisão é simples: divide o array no meio recursivamente até ter subarrays de tamanho 1 (já ordenados). A mesclagem é onde o trabalho acontece: dados dois arrays ordenados, mescle-os em um array ordenado comparando elementos e copiando o menor. A recursão cria uma árvore de log n níveis, e cada nível processa n elementos, resultando em O(n log n). O espaço extra O(n) pode ser problema para arrays muito grandes, mas existem variações in-place mais complexas. Merge Sort é excelente para ordenar listas ligadas (pode ser feito in-place) e para ordenação externa (dados que não cabem na memória).",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Aplicações e Variações",
                    Content = "Merge Sort é usado quando estabilidade é crucial (ordenar por múltiplos critérios mantendo ordem anterior) ou quando você precisa de performance previsível O(n log n). É ideal para ordenar listas ligadas porque não requer acesso aleatório. Merge Sort é também usado em ordenação externa: quando dados são muito grandes para memória, divide-se em blocos que cabem na memória, ordena cada bloco, depois mescla os blocos ordenados. Variações incluem Bottom-up Merge Sort (iterativo em vez de recursivo) e Natural Merge Sort (aproveita sequências já ordenadas). Timsort combina Merge Sort com Insertion Sort para arrays pequenos, sendo muito eficiente na prática.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Merge Sort Completo",
                    Code = @"public void MergeSort(int[] array, int esquerda, int direita)
{
    if (esquerda < direita)
    {
        int meio = esquerda + (direita - esquerda) / 2;
        
        // Ordenar metades
        MergeSort(array, esquerda, meio);
        MergeSort(array, meio + 1, direita);
        
        // Mesclar metades ordenadas
        Merge(array, esquerda, meio, direita);
    }
}

public void Merge(int[] array, int esquerda, int meio, int direita)
{
    int n1 = meio - esquerda + 1;
    int n2 = direita - meio;
    
    int[] L = new int[n1];
    int[] R = new int[n2];
    
    Array.Copy(array, esquerda, L, 0, n1);
    Array.Copy(array, meio + 1, R, 0, n2);
    
    int i = 0, j = 0, k = esquerda;
    
    while (i < n1 && j < n2)
    {
        if (L[i] <= R[j])
        {
            array[k++] = L[i++];
        }
        else
        {
            array[k++] = R[j++];
        }
    }
    
    while (i < n1) array[k++] = L[i++];
    while (j < n2) array[k++] = R[j++];
}

// Uso
int[] numeros = { 38, 27, 43, 3, 9, 82, 10 };
MergeSort(numeros, 0, numeros.Length - 1);",
                    Language = "csharp",
                    Explanation = "Merge Sort divide recursivamente até subarrays de tamanho 1, depois mescla pares ordenados. A mesclagem compara elementos e copia o menor, garantindo ordem.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Contar Inversões",
                    Description = "Modifique Merge Sort para contar o número de inversões (pares onde i < j mas array[i] > array[j]).",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"public int ContarInversoes(int[] array)
{
    // Implemente aqui
}",
                    Hints = new List<string> 
                    { 
                        "Durante a mesclagem, quando copia de R, há inversões",
                        "Número de inversões = elementos restantes em L",
                        "Some inversões de todas as mesclagens" 
                    }
                },
                new Exercise
                {
                    Title = "Merge Sort Bottom-Up",
                    Description = "Implemente Merge Sort iterativo (bottom-up) sem recursão.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"public void MergeSortIterativo(int[] array)
{
    // Implemente aqui
}",
                    Hints = new List<string> 
                    { 
                        "Comece mesclando pares de elementos",
                        "Depois mescle pares de pares, e assim por diante",
                        "Use um loop externo para tamanho crescente" 
                    }
                },
                new Exercise
                {
                    Title = "Mesclar K Arrays Ordenados",
                    Description = "Dados K arrays ordenados, mescle-os em um único array ordenado eficientemente.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"public int[] MesclarKArrays(List<int[]> arrays)
{
    // Implemente aqui
}",
                    Hints = new List<string> 
                    { 
                        "Use uma fila de prioridade (min-heap)",
                        "Insira o primeiro elemento de cada array no heap",
                        "Extraia o mínimo e insira o próximo elemento daquele array" 
                    }
                }
            },
            Summary = "Nesta aula você dominou Merge Sort, um algoritmo eficiente O(n log n) que usa dividir para conquistar. É estável, previsível e ideal para grandes conjuntos. Apesar de usar O(n) memória extra, é uma escolha excelente quando estabilidade e performance garantida são importantes. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno. Grandes empresas como Microsoft, Google e Amazon aplicam estes conceitos em seus sistemas. Ao aprender estas técnicas, você está adquirindo conhecimento usado por desenvolvedores em todo o mundo. Considere como este conceito poderia ser aplicado em um projeto pessoal. Pensar em aplicações práticas ajuda a solidificar o entendimento e torna o aprendizado mais significativo. Iniciantes frequentemente cometem alguns erros ao aprender este tópico. Conhecer estes erros comuns ajuda a evitá-los e desenvolver código mais robusto desde o início. Um erro comum é não considerar casos extremos ou situações inesperadas. Sempre teste seu código com diferentes tipos de entrada, incluindo valores nulos, vazios ou inválidos. Outro erro frequente é otimizar prematuramente. Primeiro faça o código funcionar corretamente, depois otimize se necessário. Como diz o ditado: 'premature optimization is the root of all evil'. Quando encontrar problemas, use o debugger para entender o que está acontecendo. Coloque breakpoints, inspecione variáveis e execute o código passo a passo. Esta é uma habilidade essencial para todo desenvolvedor. Mensagens de erro podem parecer intimidadoras no início, mas elas são suas aliadas. Leia-as cuidadosamente - elas geralmente indicam exatamente onde e qual é o problema. Se algo não funcionar como esperado, não desanime. Debugging é uma parte normal do desenvolvimento. Até desenvolvedores experientes passam tempo significativo depurando código. Testar seu código é tão importante quanto escrevê-lo. Testes automatizados garantem que seu código funciona corretamente e continuará funcionando quando você fizer mudanças. Comece escrevendo testes simples para os casos mais comuns. Depois adicione testes para casos extremos e situações de erro. Uma boa cobertura de testes dá confiança para refatorar e melhorar o código. Considere a prática de TDD (Test-Driven Development), onde você escreve os testes antes do código. Isso ajuda a pensar claramente sobre o que o código deve fazer. Embora a correção seja sempre a prioridade, é importante estar ciente das implicações de performance de suas escolhas. Diferentes abordagens podem ter custos computacionais muito diferentes. Em muitos casos, a diferença de performance é negligenciável para aplicações pequenas. No entanto, à medida que seus programas crescem, escolhas eficientes se tornam cada vez mais importantes. Aprenda a usar ferramentas de profiling para identificar gargalos de performance. Otimize baseado em dados reais, não em suposições sobre onde o código é lento. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0003-00000000000f"),
            CourseId = _courseId,
            Title = "Merge Sort",
            Duration = "55 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 55,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0003-00000000000e" }),
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
                "Compreender Quick Sort e particionamento",
                "Implementar Quick Sort com diferentes estratégias de pivot",
                "Analisar complexidade e casos de uso"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Quick Sort",
                    Content = "Quick Sort é um dos algoritmos mais rápidos na prática, usando dividir para conquistar. Escolhe um elemento como pivot, particiona o array colocando elementos menores à esquerda e maiores à direita do pivot, depois ordena recursivamente as partições. A complexidade média é O(n log n), mas no pior caso (pivot sempre o menor ou maior) é O(n²). Quick Sort é in-place (usa O(log n) espaço para recursão) e geralmente mais rápido que Merge Sort por ter melhor localidade de cache. Não é estável. É o algoritmo usado em muitas bibliotecas padrão, incluindo C# (como parte do Introsort). A escolha do pivot é crucial para performance.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Estratégias de Pivot",
                    Content = "A escolha do pivot afeta drasticamente a performance. Pivot fixo (primeiro ou último elemento) é simples mas resulta em O(n²) para arrays ordenados. Pivot aleatório evita o pior caso com alta probabilidade. Mediana de três (primeiro, meio, último) é um bom compromisso entre simplicidade e eficiência. Para arrays pequenos (menos de 10 elementos), Quick Sort pode trocar para Insertion Sort. Introsort (usado em C#) começa com Quick Sort mas troca para Heap Sort se a recursão fica muito profunda, garantindo O(n log n) no pior caso. Compreender essas otimizações explica por que Quick Sort é tão usado na prática apesar do pior caso O(n²).",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Aplicações e Comparações",
                    Content = "Quick Sort é preferido quando: você precisa de ordenação in-place, performance média é mais importante que pior caso, e estabilidade não é necessária. Use Merge Sort quando: estabilidade é crucial, você precisa de O(n log n) garantido, ou está ordenando listas ligadas. Use Heap Sort quando: precisa de O(n log n) garantido e in-place. Para arrays pequenos, Insertion Sort é mais rápido. Na prática, algoritmos híbridos como Introsort combinam as vantagens de múltiplos algoritmos. C# usa Introsort que começa com Quick Sort, troca para Heap Sort se necessário, e usa Insertion Sort para partições pequenas.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Quick Sort Básico",
                    Code = @"public void QuickSort(int[] array, int baixo, int alto)
{
    if (baixo < alto)
    {
        int pi = Particionar(array, baixo, alto);
        QuickSort(array, baixo, pi - 1);
        QuickSort(array, pi + 1, alto);
    }
}

public int Particionar(int[] array, int baixo, int alto)
{
    int pivot = array[alto]; // Pivot = último elemento
    int i = baixo - 1;
    
    for (int j = baixo; j < alto; j++)
    {
        if (array[j] < pivot)
        {
            i++;
            // Trocar array[i] e array[j]
            int temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }
    }
    
    // Trocar array[i+1] e array[alto] (pivot)
    int temp2 = array[i + 1];
    array[i + 1] = array[alto];
    array[alto] = temp2;
    
    return i + 1;
}

// Uso
int[] numeros = { 10, 7, 8, 9, 1, 5 };
QuickSort(numeros, 0, numeros.Length - 1);",
                    Language = "csharp",
                    Explanation = "Quick Sort particiona o array em torno do pivot, depois ordena recursivamente as partições. O particionamento coloca elementos menores à esquerda e maiores à direita.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Quick Sort com Pivot Aleatório",
                    Description = "Modifique Quick Sort para escolher pivot aleatório em vez do último elemento.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"public void QuickSortAleatorio(int[] array, int baixo, int alto)
{
    // Implemente aqui
}",
                    Hints = new List<string> 
                    { 
                        "Use Random para escolher índice aleatório entre baixo e alto",
                        "Troque elemento aleatório com o último",
                        "Depois use particionamento normal" 
                    }
                },
                new Exercise
                {
                    Title = "K-ésimo Menor Elemento",
                    Description = "Use a ideia de Quick Sort para encontrar o k-ésimo menor elemento em O(n) médio.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"public int KesimoMenor(int[] array, int k)
{
    // Implemente aqui usando Quick Select
}",
                    Hints = new List<string> 
                    { 
                        "Particione o array",
                        "Se k está na posição do pivot, encontrou",
                        "Senão, busque recursivamente na partição correta" 
                    }
                },
                new Exercise
                {
                    Title = "Quick Sort com Três Vias",
                    Description = "Implemente Quick Sort de três vias que trata eficientemente arrays com muitos duplicados.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"public void QuickSortTresVias(int[] array, int baixo, int alto)
{
    // Implemente aqui
    // Particione em: < pivot, == pivot, > pivot
}",
                    Hints = new List<string> 
                    { 
                        "Use dois ponteiros para particionar em três partes",
                        "Elementos iguais ao pivot ficam no meio",
                        "Recursão apenas nas partes < e >" 
                    }
                }
            },
            Summary = "Nesta aula você dominou Quick Sort, um dos algoritmos mais rápidos na prática. É O(n log n) médio, in-place, mas não estável. A escolha do pivot é crucial. Quick Sort é amplamente usado e é a base de algoritmos híbridos como Introsort. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno. Grandes empresas como Microsoft, Google e Amazon aplicam estes conceitos em seus sistemas. Ao aprender estas técnicas, você está adquirindo conhecimento usado por desenvolvedores em todo o mundo. Considere como este conceito poderia ser aplicado em um projeto pessoal. Pensar em aplicações práticas ajuda a solidificar o entendimento e torna o aprendizado mais significativo. Iniciantes frequentemente cometem alguns erros ao aprender este tópico. Conhecer estes erros comuns ajuda a evitá-los e desenvolver código mais robusto desde o início. Um erro comum é não considerar casos extremos ou situações inesperadas. Sempre teste seu código com diferentes tipos de entrada, incluindo valores nulos, vazios ou inválidos. Outro erro frequente é otimizar prematuramente. Primeiro faça o código funcionar corretamente, depois otimize se necessário. Como diz o ditado: 'premature optimization is the root of all evil'. Quando encontrar problemas, use o debugger para entender o que está acontecendo. Coloque breakpoints, inspecione variáveis e execute o código passo a passo. Esta é uma habilidade essencial para todo desenvolvedor. Mensagens de erro podem parecer intimidadoras no início, mas elas são suas aliadas. Leia-as cuidadosamente - elas geralmente indicam exatamente onde e qual é o problema. Se algo não funcionar como esperado, não desanime. Debugging é uma parte normal do desenvolvimento. Até desenvolvedores experientes passam tempo significativo depurando código. Testar seu código é tão importante quanto escrevê-lo. Testes automatizados garantem que seu código funciona corretamente e continuará funcionando quando você fizer mudanças. Comece escrevendo testes simples para os casos mais comuns. Depois adicione testes para casos extremos e situações de erro. Uma boa cobertura de testes dá confiança para refatorar e melhorar o código. Considere a prática de TDD (Test-Driven Development), onde você escreve os testes antes do código. Isso ajuda a pensar claramente sobre o que o código deve fazer. Embora a correção seja sempre a prioridade, é importante estar ciente das implicações de performance de suas escolhas. Diferentes abordagens podem ter custos computacionais muito diferentes. Em muitos casos, a diferença de performance é negligenciável para aplicações pequenas. No entanto, à medida que seus programas crescem, escolhas eficientes se tornam cada vez mais importantes. Aprenda a usar ferramentas de profiling para identificar gargalos de performance. Otimize baseado em dados reais, não em suposições sobre onde o código é lento. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0003-000000000010"),
            CourseId = _courseId,
            Title = "Quick Sort",
            Duration = "55 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 55,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0003-00000000000f" }),
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
                "Compreender Heap e Heap Sort",
                "Implementar operações de heap",
                "Usar heap para ordenação e filas de prioridade"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Estrutura de Heap",
                    Content = "Um heap é uma árvore binária completa onde cada nó satisfaz a propriedade de heap: em um max-heap, cada pai é maior ou igual aos filhos; em um min-heap, cada pai é menor ou igual aos filhos. Heaps são geralmente implementados usando arrays: para nó no índice i, filho esquerdo está em 2i+1, direito em 2i+2, e pai em (i-1)/2. Essa representação é eficiente em espaço e permite acesso rápido. Heaps suportam inserção e remoção em O(log n) e encontrar máximo/mínimo em O(1). São usados para implementar filas de prioridade, Heap Sort, e algoritmos como Dijkstra. Compreender heaps é fundamental para muitos algoritmos eficientes.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Heap Sort",
                    Content = "Heap Sort usa um heap para ordenar: constrói um max-heap do array, depois repetidamente remove o máximo (raiz) e reconstrói o heap. Construir o heap é O(n), e cada remoção é O(log n), resultando em O(n log n) total. Heap Sort é in-place e tem O(n log n) garantido, mas não é estável. É mais lento que Quick Sort na prática devido a pior localidade de cache, mas é usado em Introsort como fallback quando Quick Sort degrada. Heap Sort é ideal quando você precisa de O(n log n) garantido e in-place. A construção bottom-up do heap é uma otimização importante que reduz a complexidade de O(n log n) para O(n).",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Aplicações de Heaps",
                    Content = "Heaps têm muitas aplicações além de ordenação. Filas de prioridade usam heaps para operações eficientes: inserir O(log n), remover mínimo/máximo O(log n), ver mínimo/máximo O(1). Algoritmo de Dijkstra usa heap para encontrar caminhos mais curtos eficientemente. Huffman coding usa heap para compressão de dados. Encontrar os K maiores elementos em um stream usa min-heap de tamanho K. Mesclar K arrays ordenados usa heap. Mediana em stream usa dois heaps (max e min). Em C#, PriorityQueue<T, P> implementa um min-heap. Compreender heaps permite resolver muitos problemas eficientemente.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Operações Básicas de Heap",
                    Code = @"public class MaxHeap
{
    private List<int> heap = new List<int>();
    
    private int Pai(int i) => (i - 1) / 2;
    private int Esquerda(int i) => 2 * i + 1;
    private int Direita(int i) => 2 * i + 2;
    
    public void Inserir(int valor)
    {
        heap.Add(valor);
        int i = heap.Count - 1;
        
        // Subir (heapify up)
        while (i > 0 && heap[Pai(i)] < heap[i])
        {
            int temp = heap[i];
            heap[i] = heap[Pai(i)];
            heap[Pai(i)] = temp;
            i = Pai(i);
        }
    }
    
    public int RemoverMax()
    {
        if (heap.Count == 0) throw new InvalidOperationException();
        
        int max = heap[0];
        heap[0] = heap[heap.Count - 1];
        heap.RemoveAt(heap.Count - 1);
        
        if (heap.Count > 0)
            Heapify(0);
        
        return max;
    }
    
    private void Heapify(int i)
    {
        int maior = i;
        int esq = Esquerda(i);
        int dir = Direita(i);
        
        if (esq < heap.Count && heap[esq] > heap[maior])
            maior = esq;
        if (dir < heap.Count && heap[dir] > heap[maior])
            maior = dir;
        
        if (maior != i)
        {
            int temp = heap[i];
            heap[i] = heap[maior];
            heap[maior] = temp;
            Heapify(maior);
        }
    }
}",
                    Language = "csharp",
                    Explanation = "MaxHeap mantém propriedade onde pai >= filhos. Inserir adiciona ao final e sobe. RemoverMax troca raiz com último, remove e desce (heapify).",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Heap Sort",
                    Code = @"public void HeapSort(int[] array)
{
    int n = array.Length;
    
    // Construir heap (bottom-up)
    for (int i = n / 2 - 1; i >= 0; i--)
    {
        Heapify(array, n, i);
    }
    
    // Extrair elementos um por um
    for (int i = n - 1; i > 0; i--)
    {
        // Mover raiz para o final
        int temp = array[0];
        array[0] = array[i];
        array[i] = temp;
        
        // Heapify na raiz
        Heapify(array, i, 0);
    }
}

private void Heapify(int[] array, int n, int i)
{
    int maior = i;
    int esq = 2 * i + 1;
    int dir = 2 * i + 2;
    
    if (esq < n && array[esq] > array[maior])
        maior = esq;
    if (dir < n && array[dir] > array[maior])
        maior = dir;
    
    if (maior != i)
    {
        int temp = array[i];
        array[i] = array[maior];
        array[maior] = temp;
        Heapify(array, n, maior);
    }
}",
                    Language = "csharp",
                    Explanation = "Heap Sort constrói max-heap, depois repetidamente move o máximo para o final e reconstrói o heap. É O(n log n) garantido e in-place.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "K Maiores Elementos",
                    Description = "Encontre os K maiores elementos em um array usando um min-heap de tamanho K.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"public List<int> KMaiores(int[] array, int k)
{
    // Implemente aqui
}",
                    Hints = new List<string> 
                    { 
                        "Use PriorityQueue (min-heap) de tamanho K",
                        "Para cada elemento, se maior que o mínimo do heap, substitua",
                        "No final, o heap contém os K maiores" 
                    }
                },
                new Exercise
                {
                    Title = "Mediana em Stream",
                    Description = "Implemente uma estrutura que mantém a mediana de números em um stream usando dois heaps.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"public class MedianFinder
{
    public void AddNum(int num) { }
    public double FindMedian() { }
}",
                    Hints = new List<string> 
                    { 
                        "Use max-heap para metade inferior e min-heap para superior",
                        "Mantenha heaps balanceados (diferença de tamanho <= 1)",
                        "Mediana é topo do heap maior ou média dos topos" 
                    }
                },
                new Exercise
                {
                    Title = "Verificar se é Heap",
                    Description = "Implemente um método que verifica se um array representa um max-heap válido.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"public bool EhMaxHeap(int[] array)
{
    // Implemente aqui
}",
                    Hints = new List<string> 
                    { 
                        "Para cada nó i, verifique se é >= seus filhos",
                        "Filhos de i estão em 2i+1 e 2i+2",
                        "Verifique apenas até n/2 (folhas não têm filhos)" 
                    }
                }
            },
            Summary = "Nesta aula você dominou heaps e Heap Sort. Heaps são árvores binárias completas com propriedade de ordenação, permitindo operações O(log n). Heap Sort é O(n log n) garantido e in-place. Heaps são fundamentais para filas de prioridade e muitos algoritmos eficientes. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno. Grandes empresas como Microsoft, Google e Amazon aplicam estes conceitos em seus sistemas. Ao aprender estas técnicas, você está adquirindo conhecimento usado por desenvolvedores em todo o mundo. Considere como este conceito poderia ser aplicado em um projeto pessoal. Pensar em aplicações práticas ajuda a solidificar o entendimento e torna o aprendizado mais significativo. Iniciantes frequentemente cometem alguns erros ao aprender este tópico. Conhecer estes erros comuns ajuda a evitá-los e desenvolver código mais robusto desde o início. Um erro comum é não considerar casos extremos ou situações inesperadas. Sempre teste seu código com diferentes tipos de entrada, incluindo valores nulos, vazios ou inválidos. Outro erro frequente é otimizar prematuramente. Primeiro faça o código funcionar corretamente, depois otimize se necessário. Como diz o ditado: 'premature optimization is the root of all evil'. Quando encontrar problemas, use o debugger para entender o que está acontecendo. Coloque breakpoints, inspecione variáveis e execute o código passo a passo. Esta é uma habilidade essencial para todo desenvolvedor. Mensagens de erro podem parecer intimidadoras no início, mas elas são suas aliadas. Leia-as cuidadosamente - elas geralmente indicam exatamente onde e qual é o problema. Se algo não funcionar como esperado, não desanime. Debugging é uma parte normal do desenvolvimento. Até desenvolvedores experientes passam tempo significativo depurando código. Testar seu código é tão importante quanto escrevê-lo. Testes automatizados garantem que seu código funciona corretamente e continuará funcionando quando você fizer mudanças. Comece escrevendo testes simples para os casos mais comuns. Depois adicione testes para casos extremos e situações de erro. Uma boa cobertura de testes dá confiança para refatorar e melhorar o código. Considere a prática de TDD (Test-Driven Development), onde você escreve os testes antes do código. Isso ajuda a pensar claramente sobre o que o código deve fazer. Embora a correção seja sempre a prioridade, é importante estar ciente das implicações de performance de suas escolhas. Diferentes abordagens podem ter custos computacionais muito diferentes. Em muitos casos, a diferença de performance é negligenciável para aplicações pequenas. No entanto, à medida que seus programas crescem, escolhas eficientes se tornam cada vez mais importantes. Aprenda a usar ferramentas de profiling para identificar gargalos de performance. Otimize baseado em dados reais, não em suposições sobre onde o código é lento. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0003-000000000011"),
            CourseId = _courseId,
            Title = "Heap e Heap Sort",
            Duration = "60 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 60,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0003-000000000010" }),
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
                "Compreender recursão e casos base",
                "Implementar algoritmos recursivos clássicos",
                "Otimizar recursão com memoização"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Fundamentos de Recursão",
                    Content = "Recursão é quando uma função chama a si mesma para resolver um problema dividindo-o em subproblemas menores. Toda função recursiva precisa de: caso base (condição de parada) e caso recursivo (chamada com problema menor). Sem caso base, a recursão é infinita causando stack overflow. Recursão é natural para problemas com estrutura recursiva: árvores, grafos, dividir para conquistar. Exemplos clássicos incluem fatorial, Fibonacci, torres de Hanói, e percursos de árvores. Recursão pode ser mais elegante que iteração, mas usa memória da pilha O(profundidade). Para recursões profundas, considere iteração ou otimização de cauda. Compreender recursão é fundamental para muitos algoritmos e estruturas de dados.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Memoização e Programação Dinâmica",
                    Content = "Recursão ingênua pode ser ineficiente quando subproblemas se repetem. Fibonacci recursivo é O(2^n) porque recalcula os mesmos valores. Memoização armazena resultados de subproblemas em um dicionário, evitando recálculo. Com memoização, Fibonacci vira O(n). Programação dinâmica é uma técnica que usa memoização (top-down) ou tabulação (bottom-up) para resolver problemas otimamente. Problemas clássicos incluem: caminho mais curto, subsequência comum mais longa, problema da mochila, e corte de hastes. Identificar subproblemas sobrepostos e subestrutura ótima é chave para aplicar programação dinâmica. É uma das técnicas mais poderosas para otimização.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Backtracking",
                    Content = "Backtracking é uma técnica recursiva para explorar todas as soluções possíveis, descartando caminhos que não levam a solução. É usado em problemas de busca exaustiva: N-rainhas, Sudoku, geração de permutações, e problemas de satisfação de restrições. A ideia é: tente uma opção, se funciona continue recursivamente, se não funciona desfaça (backtrack) e tente outra opção. Backtracking pode ser otimizado com poda: descartar caminhos que certamente não levarão a solução. Apesar de ser exponencial no pior caso, backtracking com poda é prático para muitos problemas. É fundamental para resolver puzzles e problemas combinatórios.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Fibonacci com Memoização",
                    Code = @"// Recursão ingênua - O(2^n)
public int FibonacciLento(int n)
{
    if (n <= 1) return n;
    return FibonacciLento(n - 1) + FibonacciLento(n - 2);
}

// Com memoização - O(n)
private Dictionary<int, int> memo = new Dictionary<int, int>();

public int FibonacciRapido(int n)
{
    if (n <= 1) return n;
    
    if (memo.ContainsKey(n))
        return memo[n];
    
    int resultado = FibonacciRapido(n - 1) + FibonacciRapido(n - 2);
    memo[n] = resultado;
    return resultado;
}

// Teste
Console.WriteLine(FibonacciRapido(40)); // Rápido
// FibonacciLento(40) levaria muito tempo!",
                    Language = "csharp",
                    Explanation = "Memoização transforma Fibonacci de O(2^n) para O(n) armazenando resultados calculados. Isso evita recalcular os mesmos subproblemas repetidamente.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Gerar Permutações com Backtracking",
                    Code = @"public List<List<int>> GerarPermutacoes(int[] nums)
{
    var resultado = new List<List<int>>();
    var atual = new List<int>();
    var usados = new bool[nums.Length];
    
    Backtrack(nums, atual, usados, resultado);
    return resultado;
}

private void Backtrack(int[] nums, List<int> atual, bool[] usados, List<List<int>> resultado)
{
    if (atual.Count == nums.Length)
    {
        resultado.Add(new List<int>(atual));
        return;
    }
    
    for (int i = 0; i < nums.Length; i++)
    {
        if (usados[i]) continue;
        
        // Escolher
        atual.Add(nums[i]);
        usados[i] = true;
        
        // Explorar
        Backtrack(nums, atual, usados, resultado);
        
        // Desfazer (backtrack)
        atual.RemoveAt(atual.Count - 1);
        usados[i] = false;
    }
}

// Teste
var perms = GerarPermutacoes(new[] { 1, 2, 3 });
// Gera: [1,2,3], [1,3,2], [2,1,3], [2,3,1], [3,1,2], [3,2,1]",
                    Language = "csharp",
                    Explanation = "Backtracking explora todas as possibilidades: escolhe um elemento, explora recursivamente, depois desfaz a escolha para tentar outras opções.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Subconjuntos",
                    Description = "Gere todos os subconjuntos de um conjunto usando backtracking.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"public List<List<int>> GerarSubconjuntos(int[] nums)
{
    // Implemente aqui
}",
                    Hints = new List<string> 
                    { 
                        "Para cada elemento, você tem duas escolhas: incluir ou não",
                        "Use backtracking para explorar ambas as opções",
                        "Adicione o subconjunto atual ao resultado em cada chamada" 
                    }
                },
                new Exercise
                {
                    Title = "Problema da Mochila",
                    Description = "Dados itens com pesos e valores, e capacidade da mochila, maximize o valor. Use recursão com memoização.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"public int Mochila(int[] pesos, int[] valores, int capacidade)
{
    // Implemente aqui
}",
                    Hints = new List<string> 
                    { 
                        "Para cada item, escolha incluir ou não",
                        "Se incluir, reduza capacidade e adicione valor",
                        "Use memoização com (índice, capacidade restante) como chave" 
                    }
                },
                new Exercise
                {
                    Title = "N-Rainhas",
                    Description = "Coloque N rainhas em um tabuleiro NxN sem que se ataquem. Use backtracking.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"public List<List<string>> ResolverNRainhas(int n)
{
    // Implemente aqui
}",
                    Hints = new List<string> 
                    { 
                        "Coloque uma rainha por linha",
                        "Verifique se a posição é segura (sem ataques)",
                        "Use backtracking para tentar todas as posições" 
                    }
                }
            },
            Summary = "Nesta aula você dominou recursão, uma técnica fundamental onde funções chamam a si mesmas. Aprendeu memoização para otimizar recursões com subproblemas sobrepostos, e backtracking para explorar soluções. Recursão é essencial para muitos algoritmos e problemas complexos. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno. Grandes empresas como Microsoft, Google e Amazon aplicam estes conceitos em seus sistemas. Ao aprender estas técnicas, você está adquirindo conhecimento usado por desenvolvedores em todo o mundo. Considere como este conceito poderia ser aplicado em um projeto pessoal. Pensar em aplicações práticas ajuda a solidificar o entendimento e torna o aprendizado mais significativo. Iniciantes frequentemente cometem alguns erros ao aprender este tópico. Conhecer estes erros comuns ajuda a evitá-los e desenvolver código mais robusto desde o início. Um erro comum é não considerar casos extremos ou situações inesperadas. Sempre teste seu código com diferentes tipos de entrada, incluindo valores nulos, vazios ou inválidos. Outro erro frequente é otimizar prematuramente. Primeiro faça o código funcionar corretamente, depois otimize se necessário. Como diz o ditado: 'premature optimization is the root of all evil'. Quando encontrar problemas, use o debugger para entender o que está acontecendo. Coloque breakpoints, inspecione variáveis e execute o código passo a passo. Esta é uma habilidade essencial para todo desenvolvedor. Mensagens de erro podem parecer intimidadoras no início, mas elas são suas aliadas. Leia-as cuidadosamente - elas geralmente indicam exatamente onde e qual é o problema. Se algo não funcionar como esperado, não desanime. Debugging é uma parte normal do desenvolvimento. Até desenvolvedores experientes passam tempo significativo depurando código. Testar seu código é tão importante quanto escrevê-lo. Testes automatizados garantem que seu código funciona corretamente e continuará funcionando quando você fizer mudanças. Comece escrevendo testes simples para os casos mais comuns. Depois adicione testes para casos extremos e situações de erro. Uma boa cobertura de testes dá confiança para refatorar e melhorar o código. Considere a prática de TDD (Test-Driven Development), onde você escreve os testes antes do código. Isso ajuda a pensar claramente sobre o que o código deve fazer. Embora a correção seja sempre a prioridade, é importante estar ciente das implicações de performance de suas escolhas. Diferentes abordagens podem ter custos computacionais muito diferentes. Em muitos casos, a diferença de performance é negligenciável para aplicações pequenas. No entanto, à medida que seus programas crescem, escolhas eficientes se tornam cada vez mais importantes. Aprenda a usar ferramentas de profiling para identificar gargalos de performance. Otimize baseado em dados reais, não em suposições sobre onde o código é lento. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0003-000000000012"),
            CourseId = _courseId,
            Title = "Recursão e Backtracking",
            Duration = "60 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 60,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0003-000000000001" }),
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
                "Compreender programação dinâmica",
                "Identificar problemas que podem usar DP",
                "Implementar soluções top-down e bottom-up"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Programação Dinâmica",
                    Content = "Programação Dinâmica (DP) é uma técnica para resolver problemas de otimização dividindo-os em subproblemas sobrepostos e combinando suas soluções. Dois requisitos: subestrutura ótima (solução ótima contém soluções ótimas de subproblemas) e subproblemas sobrepostos (mesmos subproblemas aparecem múltiplas vezes). DP pode ser top-down (recursão com memoização) ou bottom-up (tabulação iterativa). Top-down é mais intuitivo, bottom-up é mais eficiente em espaço. DP transforma problemas exponenciais em polinomiais. Exemplos clássicos: Fibonacci, caminho mais curto, subsequência comum mais longa, problema da mochila, e corte de hastes. DP é uma das técnicas mais poderosas em algoritmos.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Identificando Problemas de DP",
                    Content = "Como identificar se um problema pode usar DP? Procure por: decisões em cada passo que afetam decisões futuras, múltiplas formas de resolver com diferentes custos, subproblemas que se repetem, e necessidade de encontrar solução ótima. Palavras-chave: máximo, mínimo, mais longo, mais curto, contar maneiras. Passos para resolver com DP: definir estado (o que representa um subproblema), definir transição (como combinar subproblemas), identificar casos base, e determinar ordem de computação. Praticar identificar padrões de DP é essencial. Problemas comuns incluem: sequências (LCS, LIS), strings (edit distance), árvores (diâmetro), e otimização (mochila, corte).",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Otimizações de DP",
                    Content = "DP pode ser otimizada em tempo e espaço. Redução de espaço: se dp[i] depende apenas de dp[i-1], use duas variáveis em vez de array. Para dp[i][j] dependendo de linha anterior, use duas linhas. Otimização de tempo: pré-computar valores, usar estruturas auxiliares (segment tree, BIT), ou aplicar truques específicos do problema. DP com bitmask representa estados como bits, útil para subconjuntos. DP em árvores usa DFS para computar estados. DP com otimização convexa (Convex Hull Trick) reduz O(n²) para O(n). Compreender essas otimizações permite resolver problemas mais complexos eficientemente. DP é fundamental em competições de programação e entrevistas técnicas.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Subsequência Comum Mais Longa (LCS)",
                    Code = @"using System;
using System.Collections.Generic;

public class LCSCalculator
{
    private Dictionary<(int, int), int> memo = new Dictionary<(int, int), int>();

    // Top-down com memoização
    public int LCS_TopDown(string s1, string s2, int i, int j)
    {
        if (i == s1.Length || j == s2.Length)
            return 0;
        
        if (memo.ContainsKey((i, j)))
            return memo[(i, j)];
        
        int resultado;
        if (s1[i] == s2[j])
        {
            resultado = 1 + LCS_TopDown(s1, s2, i + 1, j + 1);
        }
        else
        {
            resultado = Math.Max(
                LCS_TopDown(s1, s2, i + 1, j),
                LCS_TopDown(s1, s2, i, j + 1)
            );
        }
        
        memo[(i, j)] = resultado;
        return resultado;
    }

    // Bottom-up com tabulação
    public int LCS_BottomUp(string s1, string s2)
    {
        int m = s1.Length, n = s2.Length;
        int[,] dp = new int[m + 1, n + 1];
        
        for (int i = 1; i <= m; i++)
        {
            for (int j = 1; j <= n; j++)
            {
                if (s1[i - 1] == s2[j - 1])
                {
                    dp[i, j] = 1 + dp[i - 1, j - 1];
                }
                else
                {
                    dp[i, j] = Math.Max(dp[i - 1, j], dp[i, j - 1]);
                }
            }
        }
        
        return dp[m, n];
    }
}",
                    Language = "csharp",
                    Explanation = "LCS encontra a maior subsequência comum entre duas strings. Top-down usa recursão com memoização, bottom-up preenche tabela iterativamente. Ambos são O(m×n).",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Subida de Escadas",
                    Description = "Você pode subir 1 ou 2 degraus por vez. De quantas maneiras pode subir N degraus? Use DP.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"public int SubirEscadas(int n)
{
    // Implemente aqui
}",
                    Hints = new List<string> 
                    { 
                        "dp[i] = número de maneiras de chegar ao degrau i",
                        "dp[i] = dp[i-1] + dp[i-2]",
                        "Casos base: dp[0] = 1, dp[1] = 1" 
                    }
                },
                new Exercise
                {
                    Title = "Caminho Mínimo em Grade",
                    Description = "Dada uma grade com custos, encontre o caminho de custo mínimo do canto superior esquerdo ao inferior direito (só pode ir para direita ou baixo).",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"public int CaminhoMinimo(int[,] grade)
{
    // Implemente aqui
}",
                    Hints = new List<string> 
                    { 
                        "dp[i][j] = custo mínimo para chegar a (i,j)",
                        "dp[i][j] = grade[i][j] + min(dp[i-1][j], dp[i][j-1])",
                        "Inicialize primeira linha e coluna" 
                    }
                },
                new Exercise
                {
                    Title = "Partição de Subconjunto",
                    Description = "Determine se um array pode ser particionado em dois subconjuntos com soma igual. Use DP.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"public bool PodeParticionar(int[] nums)
{
    // Implemente aqui
}",
                    Hints = new List<string> 
                    { 
                        "Se soma total é ímpar, impossível",
                        "Problema se reduz a: existe subconjunto com soma = total/2?",
                        "Use DP de mochila: dp[i][j] = pode fazer soma j com primeiros i elementos" 
                    }
                }
            },
            Summary = "Nesta aula você dominou Programação Dinâmica, uma técnica poderosa para otimização. Aprendeu a identificar problemas com subestrutura ótima e subproblemas sobrepostos, e implementar soluções top-down e bottom-up. DP transforma problemas exponenciais em polinomiais. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno. Grandes empresas como Microsoft, Google e Amazon aplicam estes conceitos em seus sistemas. Ao aprender estas técnicas, você está adquirindo conhecimento usado por desenvolvedores em todo o mundo. Considere como este conceito poderia ser aplicado em um projeto pessoal. Pensar em aplicações práticas ajuda a solidificar o entendimento e torna o aprendizado mais significativo. Iniciantes frequentemente cometem alguns erros ao aprender este tópico. Conhecer estes erros comuns ajuda a evitá-los e desenvolver código mais robusto desde o início. Um erro comum é não considerar casos extremos ou situações inesperadas. Sempre teste seu código com diferentes tipos de entrada, incluindo valores nulos, vazios ou inválidos. Outro erro frequente é otimizar prematuramente. Primeiro faça o código funcionar corretamente, depois otimize se necessário. Como diz o ditado: 'premature optimization is the root of all evil'. Quando encontrar problemas, use o debugger para entender o que está acontecendo. Coloque breakpoints, inspecione variáveis e execute o código passo a passo. Esta é uma habilidade essencial para todo desenvolvedor. Mensagens de erro podem parecer intimidadoras no início, mas elas são suas aliadas. Leia-as cuidadosamente - elas geralmente indicam exatamente onde e qual é o problema. Se algo não funcionar como esperado, não desanime. Debugging é uma parte normal do desenvolvimento. Até desenvolvedores experientes passam tempo significativo depurando código. Testar seu código é tão importante quanto escrevê-lo. Testes automatizados garantem que seu código funciona corretamente e continuará funcionando quando você fizer mudanças. Comece escrevendo testes simples para os casos mais comuns. Depois adicione testes para casos extremos e situações de erro. Uma boa cobertura de testes dá confiança para refatorar e melhorar o código. Considere a prática de TDD (Test-Driven Development), onde você escreve os testes antes do código. Isso ajuda a pensar claramente sobre o que o código deve fazer. Embora a correção seja sempre a prioridade, é importante estar ciente das implicações de performance de suas escolhas. Diferentes abordagens podem ter custos computacionais muito diferentes. Em muitos casos, a diferença de performance é negligenciável para aplicações pequenas. No entanto, à medida que seus programas crescem, escolhas eficientes se tornam cada vez mais importantes. Aprenda a usar ferramentas de profiling para identificar gargalos de performance. Otimize baseado em dados reais, não em suposições sobre onde o código é lento. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0003-000000000013"),
            CourseId = _courseId,
            Title = "Programação Dinâmica",
            Duration = "60 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 60,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0003-000000000012" }),
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
                "Revisar todas as estruturas de dados e algoritmos aprendidos",
                "Comparar complexidades e casos de uso",
                "Aplicar conhecimento em problemas integrados",
                "Preparar-se para o próximo nível"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Revisão de Estruturas de Dados",
                    Content = "Neste curso você dominou estruturas de dados fundamentais. Arrays oferecem acesso O(1) mas tamanho fixo. Lists combinam acesso rápido com tamanho dinâmico. Stacks (LIFO) e Queues (FIFO) são especializadas para processamento sequencial. Linked Lists permitem inserções/remoções O(1) mas acesso O(n). HashSet e Dictionary oferecem operações O(1) médio para conjuntos e mapeamentos. Árvores organizam dados hierarquicamente, com BSTs permitindo busca O(log n). Grafos modelam relações complexas. Heaps implementam filas de prioridade eficientemente. Cada estrutura tem vantagens e desvantagens. Escolher a estrutura certa é fundamental para código eficiente. Compreender complexidades de tempo e espaço permite tomar decisões informadas.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Revisão de Algoritmos",
                    Content = "Você aprendeu algoritmos essenciais. Busca: linear O(n) para arrays não ordenados, binária O(log n) para ordenados. Ordenação: algoritmos simples O(n²) (Bubble, Selection, Insertion) para arrays pequenos, e eficientes O(n log n) (Merge, Quick, Heap) para grandes. Merge Sort é estável e previsível, Quick Sort é rápido na prática, Heap Sort é in-place com O(n log n) garantido. Grafos: DFS explora profundamente, BFS explora por níveis e encontra caminhos mais curtos. Recursão permite soluções elegantes, memoização otimiza subproblemas sobrepostos, backtracking explora todas as possibilidades, e programação dinâmica resolve problemas de otimização eficientemente. Dominar esses algoritmos é essencial para resolver problemas complexos.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Próximos Passos",
                    Content = "Você completou uma base sólida em estruturas de dados e algoritmos. Próximos passos incluem: praticar problemas em plataformas como LeetCode e HackerRank, estudar algoritmos avançados (algoritmos de grafos como Dijkstra e Floyd-Warshall, árvores balanceadas como AVL e Red-Black, estruturas avançadas como Segment Tree e Fenwick Tree), aprender sobre complexidade computacional e classes P vs NP, e aplicar conhecimento em projetos reais. Continue praticando: resolver problemas regularmente solidifica o conhecimento. Participe de competições de programação para desafiar-se. Leia código de outros para aprender diferentes abordagens. Ensine outros para consolidar seu entendimento. Estruturas de dados e algoritmos são fundamentais para engenharia de software e abrem portas para problemas mais complexos e interessantes.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Problema Integrado: Sistema de Recomendação",
                    Code = @"public class SistemaRecomendacao
{
    // Grafo de usuários e suas conexões
    private Dictionary<int, HashSet<int>> amigos = new Dictionary<int, HashSet<int>>();
    
    // Histórico de itens que cada usuário gostou
    private Dictionary<int, HashSet<string>> preferencias = new Dictionary<int, HashSet<string>>();
    
    public void AdicionarAmizade(int usuario1, int usuario2)
    {
        if (!amigos.ContainsKey(usuario1))
            amigos[usuario1] = new HashSet<int>();
        if (!amigos.ContainsKey(usuario2))
            amigos[usuario2] = new HashSet<int>();
        
        amigos[usuario1].Add(usuario2);
        amigos[usuario2].Add(usuario1);
    }
    
    public void AdicionarPreferencia(int usuario, string item)
    {
        if (!preferencias.ContainsKey(usuario))
            preferencias[usuario] = new HashSet<string>();
        
        preferencias[usuario].Add(item);
    }
    
    // Recomendar itens baseado em amigos (BFS)
    public List<string> Recomendar(int usuario, int maxRecomendacoes)
    {
        var recomendacoes = new Dictionary<string, int>(); // item -> contagem
        var visitados = new HashSet<int>();
        var fila = new Queue<int>();
        
        fila.Enqueue(usuario);
        visitados.Add(usuario);
        
        // BFS para encontrar amigos próximos
        while (fila.Count > 0)
        {
            int atual = fila.Dequeue();
            
            if (amigos.ContainsKey(atual))
            {
                foreach (int amigo in amigos[atual])
                {
                    if (!visitados.Contains(amigo))
                    {
                        visitados.Add(amigo);
                        fila.Enqueue(amigo);
                        
                        // Contar preferências dos amigos
                        if (preferencias.ContainsKey(amigo))
                        {
                            foreach (string item in preferencias[amigo])
                            {
                                // Não recomendar itens que usuário já tem
                                if (!preferencias.ContainsKey(usuario) || 
                                    !preferencias[usuario].Contains(item))
                                {
                                    if (!recomendacoes.ContainsKey(item))
                                        recomendacoes[item] = 0;
                                    recomendacoes[item]++;
                                }
                            }
                        }
                    }
                }
            }
        }
        
        // Ordenar por popularidade e retornar top N
        return recomendacoes
            .OrderByDescending(kvp => kvp.Value)
            .Take(maxRecomendacoes)
            .Select(kvp => kvp.Key)
            .ToList();
    }
}",
                    Language = "csharp",
                    Explanation = "Este sistema integra múltiplos conceitos: grafos para modelar amizades, HashSet para preferências únicas, Dictionary para mapeamentos, BFS para encontrar amigos próximos, e ordenação para ranquear recomendações.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Análise de Rede Social",
                    Description = "Implemente métodos para: encontrar usuário com mais amigos, detectar comunidades (componentes conectados), e calcular grau de separação entre dois usuários.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"public class AnalisadorRedeSocial
{
    public int UsuarioMaisPopular(Dictionary<int, List<int>> rede) { }
    public int GrauSeparacao(Dictionary<int, List<int>> rede, int u1, int u2) { }
    public List<List<int>> DetectarComunidades(Dictionary<int, List<int>> rede) { }
}",
                    Hints = new List<string> 
                    { 
                        "Mais popular: conte o tamanho de cada lista de amigos",
                        "Grau de separação: use BFS e conte níveis",
                        "Comunidades: use DFS para encontrar componentes conectados" 
                    }
                },
                new Exercise
                {
                    Title = "Cache Inteligente",
                    Description = "Implemente um cache LRU (Least Recently Used) que remove o item menos recentemente usado quando atinge capacidade máxima. Use Dictionary e LinkedList.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"public class LRUCache<TKey, TValue>
{
    public LRUCache(int capacidade) { }
    public TValue Get(TKey chave) { }
    public void Put(TKey chave, TValue valor) { }
}",
                    Hints = new List<string> 
                    { 
                        "Use Dictionary para acesso O(1) e LinkedList para ordem",
                        "Ao acessar, mova para o final da lista (mais recente)",
                        "Ao atingir capacidade, remova o primeiro da lista (menos recente)" 
                    }
                },
                new Exercise
                {
                    Title = "Otimizador de Rotas",
                    Description = "Dado um grafo ponderado representando cidades e distâncias, encontre a rota mais curta entre duas cidades. Implemente usando BFS modificado ou Dijkstra simplificado.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"public List<int> RotaMaisCurta(Dictionary<int, List<(int destino, int distancia)>> grafo, int origem, int destino)
{
    // Implemente aqui
}",
                    Hints = new List<string> 
                    { 
                        "Use fila de prioridade para processar cidades por distância",
                        "Mantenha distâncias mínimas e predecessores",
                        "Reconstrua o caminho seguindo predecessores" 
                    }
                }
            },
            Summary = "Parabéns por completar o curso de Estruturas de Dados e Algoritmos! Você dominou estruturas fundamentais (arrays, listas, pilhas, filas, árvores, grafos, heaps) e algoritmos essenciais (busca, ordenação, DFS, BFS, recursão, DP). Continue praticando e aplicando esse conhecimento em projetos reais. Você está preparado para desafios mais avançados! Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno. Grandes empresas como Microsoft, Google e Amazon aplicam estes conceitos em seus sistemas. Ao aprender estas técnicas, você está adquirindo conhecimento usado por desenvolvedores em todo o mundo. Considere como este conceito poderia ser aplicado em um projeto pessoal. Pensar em aplicações práticas ajuda a solidificar o entendimento e torna o aprendizado mais significativo. Iniciantes frequentemente cometem alguns erros ao aprender este tópico. Conhecer estes erros comuns ajuda a evitá-los e desenvolver código mais robusto desde o início. Um erro comum é não considerar casos extremos ou situações inesperadas. Sempre teste seu código com diferentes tipos de entrada, incluindo valores nulos, vazios ou inválidos. Outro erro frequente é otimizar prematuramente. Primeiro faça o código funcionar corretamente, depois otimize se necessário. Como diz o ditado: 'premature optimization is the root of all evil'. Quando encontrar problemas, use o debugger para entender o que está acontecendo. Coloque breakpoints, inspecione variáveis e execute o código passo a passo. Esta é uma habilidade essencial para todo desenvolvedor. Mensagens de erro podem parecer intimidadoras no início, mas elas são suas aliadas. Leia-as cuidadosamente - elas geralmente indicam exatamente onde e qual é o problema. Se algo não funcionar como esperado, não desanime. Debugging é uma parte normal do desenvolvimento. Até desenvolvedores experientes passam tempo significativo depurando código. Testar seu código é tão importante quanto escrevê-lo. Testes automatizados garantem que seu código funciona corretamente e continuará funcionando quando você fizer mudanças. Comece escrevendo testes simples para os casos mais comuns. Depois adicione testes para casos extremos e situações de erro. Uma boa cobertura de testes dá confiança para refatorar e melhorar o código. Considere a prática de TDD (Test-Driven Development), onde você escreve os testes antes do código. Isso ajuda a pensar claramente sobre o que o código deve fazer. Embora a correção seja sempre a prioridade, é importante estar ciente das implicações de performance de suas escolhas. Diferentes abordagens podem ter custos computacionais muito diferentes. Em muitos casos, a diferença de performance é negligenciável para aplicações pequenas. No entanto, à medida que seus programas crescem, escolhas eficientes se tornam cada vez mais importantes. Aprenda a usar ferramentas de profiling para identificar gargalos de performance. Otimize baseado em dados reais, não em suposições sobre onde o código é lento. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0003-000000000014"),
            CourseId = _courseId,
            Title = "Revisão e Integração de Conceitos",
            Duration = "60 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 60,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0003-000000000013" }),
            OrderIndex = 20,
            Content = "",
            StructuredContent = JsonSerializer.Serialize(content),
            Version = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }
}
