using Shared.Entities;
using Shared.Models;
using System.Text.Json;

namespace Shared.Data;

/// <summary>
/// Content seeder for Level 3: Banco de Dados e SQL
/// Creates 20 lessons covering database fundamentals, SQL, and C# database connectivity
/// </summary>
public partial class Level3ContentSeeder
{
    private readonly Guid _courseId = Guid.Parse("10000000-0000-0000-0000-000000000004");
    private readonly Guid _levelId = Guid.Parse("00000000-0000-0000-0000-000000000003");

    public Course CreateLevel3Course()
    {
        return new Course
        {
            Id = _courseId,
            LevelId = _levelId,
            Title = "Banco de Dados e SQL",
            Description = "Aprenda os fundamentos de bancos de dados relacionais, SQL e como integrar bancos de dados em aplicações C#. Este curso cobre desde conceitos básicos até tópicos avançados como transações, índices e stored procedures.",
            Level = Level.Intermediate,
            Duration = "4 semanas",
            LessonCount = 20,
            Topics = JsonSerializer.Serialize(new[]
            {
                "Fundamentos de Bancos de Dados",
                "SQL Básico",
                "SELECT e Consultas",
                "INSERT, UPDATE, DELETE",
                "JOINs e Relacionamentos",
                "Índices e Performance",
                "Transações e ACID",
                "Normalização",
                "Stored Procedures",
                "ADO.NET",
                "Dapper",
                "Segurança de Dados"
            }),
            OrderIndex = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    public List<Lesson> CreateLevel3Lessons()
    {
        return new List<Lesson>
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
    }


    private Lesson CreateLesson1()
    {
        var content = new LessonContent
        {
            Objectives = new List<string>
            {
                "Compreender o que são bancos de dados e por que são importantes",
                "Conhecer os diferentes tipos de bancos de dados",
                "Entender o modelo relacional e seus componentes",
                "Identificar quando usar bancos de dados em aplicações"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "O que são Bancos de Dados?",
                    Content = "Um banco de dados é um sistema organizado para armazenar, gerenciar e recuperar informações de forma eficiente. Imagine uma biblioteca gigante onde cada livro está catalogado e pode ser encontrado rapidamente. Bancos de dados fazem isso com dados digitais. Eles são fundamentais para praticamente todas as aplicações modernas, desde redes sociais até sistemas bancários. Sem bancos de dados, seria impossível manter informações de milhões de usuários, processar transações financeiras ou armazenar históricos de compras. Um banco de dados não é apenas um arquivo grande, mas um sistema sofisticado que garante integridade, segurança e acesso rápido aos dados. Ele permite que múltiplos usuários acessem as mesmas informações simultaneamente sem conflitos. Além disso, bancos de dados modernos oferecem recursos como backup automático, recuperação de desastres e replicação de dados. Aprender sobre bancos de dados é essencial para qualquer desenvolvedor que queira criar aplicações robustas e escaláveis.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Tipos de Bancos de Dados",
                    Content = "Existem diversos tipos de bancos de dados, cada um adequado para diferentes necessidades. Os bancos de dados relacionais (como SQL Server, MySQL, PostgreSQL) organizam dados em tabelas com linhas e colunas, similar a planilhas. Eles são ideais para dados estruturados e relacionamentos complexos entre entidades. Bancos de dados NoSQL (como MongoDB, Redis) são mais flexíveis e adequados para dados não estruturados ou semi-estruturados. Bancos de dados de grafos (como Neo4j) são especializados em relacionamentos complexos entre entidades. Bancos de dados em memória (como Redis) priorizam velocidade sobre persistência. Neste curso, focaremos em bancos de dados relacionais, que são os mais utilizados em aplicações empresariais. Eles oferecem garantias ACID (Atomicidade, Consistência, Isolamento, Durabilidade) que são cruciais para aplicações que lidam com dados críticos. A escolha do tipo de banco de dados depende dos requisitos específicos da aplicação, como volume de dados, padrões de acesso, necessidade de consistência e escalabilidade.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "O Modelo Relacional",
                    Content = "O modelo relacional, proposto por Edgar Codd em 1970, revolucionou o armazenamento de dados. Neste modelo, dados são organizados em tabelas (também chamadas de relações). Cada tabela representa uma entidade do mundo real, como Clientes, Produtos ou Pedidos. As linhas da tabela (tuplas) representam instâncias individuais da entidade, enquanto as colunas (atributos) representam as propriedades dessa entidade. Por exemplo, uma tabela Clientes pode ter colunas como ID, Nome, Email e Telefone. O poder do modelo relacional está na capacidade de estabelecer relacionamentos entre tabelas através de chaves. Uma chave primária identifica unicamente cada linha em uma tabela, enquanto chaves estrangeiras criam vínculos entre tabelas diferentes. Isso permite modelar relacionamentos complexos do mundo real, como 'um cliente pode fazer vários pedidos' ou 'um pedido contém vários produtos'. O modelo relacional também suporta operações matemáticas de conjuntos, permitindo consultas sofisticadas que combinam dados de múltiplas tabelas. Essa estrutura organizada facilita a manutenção da integridade dos dados e evita redundâncias desnecessárias.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Estrutura Básica de uma Tabela",
                    Code = @"-- Exemplo conceitual de uma tabela Clientes
-- Colunas: ID, Nome, Email, DataCadastro
-- Cada linha representa um cliente específico

/*
ID  | Nome           | Email                | DataCadastro
----|----------------|----------------------|-------------
1   | João Silva     | joao@email.com       | 2024-01-15
2   | Maria Santos   | maria@email.com      | 2024-01-16
3   | Pedro Costa    | pedro@email.com      | 2024-01-17
*/",
                    Language = "sql",
                    Explanation = "Esta representação mostra como dados são organizados em uma tabela relacional. Cada coluna tem um tipo de dado específico, e cada linha contém valores para todas as colunas.",
                    IsRunnable = false
                },
                new CodeExample
                {
                    Title = "Relacionamento Entre Tabelas",
                    Code = @"-- Tabela Clientes
/*
ClienteID | Nome
----------|-------------
1         | João Silva
2         | Maria Santos
*/

-- Tabela Pedidos (relacionada com Clientes)
/*
PedidoID | ClienteID | DataPedido | Valor
---------|-----------|------------|-------
101      | 1         | 2024-01-20 | 150.00
102      | 1         | 2024-01-22 | 200.00
103      | 2         | 2024-01-21 | 75.50
*/

-- ClienteID em Pedidos é uma chave estrangeira
-- que referencia ClienteID em Clientes",
                    Language = "sql",
                    Explanation = "Este exemplo mostra como duas tabelas se relacionam. A coluna ClienteID na tabela Pedidos referencia a chave primária da tabela Clientes, estabelecendo um relacionamento um-para-muitos.",
                    IsRunnable = false
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Identifique Entidades",
                    Description = "Para um sistema de biblioteca, identifique pelo menos 4 entidades (tabelas) que seriam necessárias e liste 3-4 atributos (colunas) para cada uma.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"// Exemplo:
// Entidade: Livros
// Atributos: ID, Titulo, Autor, ISBN, AnoPublicacao

// Sua vez - identifique mais 3 entidades:",
                    Hints = new List<string>
                    {
                        "Pense em quem usa a biblioteca (usuários/membros)",
                        "Considere o processo de empréstimo de livros",
                        "Não esqueça dos autores e editoras"
                    }
                },
                new Exercise
                {
                    Title = "Relacionamentos",
                    Description = "Usando as entidades do exercício anterior, descreva 3 relacionamentos entre elas. Por exemplo: 'Um autor pode escrever vários livros'.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Descreva os relacionamentos:
// 1. 
// 2. 
// 3. ",
                    Hints = new List<string>
                    {
                        "Pense em relacionamentos um-para-muitos",
                        "Considere como livros, autores e empréstimos se conectam",
                        "Um membro pode fazer vários empréstimos?"
                    }
                },
                new Exercise
                {
                    Title = "Escolha do Tipo de Banco",
                    Description = "Para cada cenário abaixo, justifique se um banco relacional ou NoSQL seria mais adequado: (a) Sistema bancário, (b) Rede social com posts e comentários, (c) Catálogo de produtos de e-commerce.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Analise cada cenário:
// (a) Sistema bancário: 
// (b) Rede social: 
// (c) E-commerce: ",
                    Hints = new List<string>
                    {
                        "Considere a necessidade de transações ACID",
                        "Pense na estrutura dos dados (estruturado vs não estruturado)",
                        "Avalie a importância de relacionamentos complexos"
                    }
                }
            },
            Summary = "Nesta aula você aprendeu o que são bancos de dados, conheceu os diferentes tipos disponíveis e compreendeu o modelo relacional. Você entendeu como dados são organizados em tabelas e como relacionamentos são estabelecidos através de chaves. Este conhecimento é a base para trabalhar com SQL e bancos de dados em suas aplicações. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno. Grandes empresas como Microsoft, Google e Amazon aplicam estes conceitos em seus sistemas. Ao aprender estas técnicas, você está adquirindo conhecimento usado por desenvolvedores em todo o mundo. Considere como este conceito poderia ser aplicado em um projeto pessoal. Pensar em aplicações práticas ajuda a solidificar o entendimento e torna o aprendizado mais significativo. Iniciantes frequentemente cometem alguns erros ao aprender este tópico. Conhecer estes erros comuns ajuda a evitá-los e desenvolver código mais robusto desde o início. Um erro comum é não considerar casos extremos ou situações inesperadas. Sempre teste seu código com diferentes tipos de entrada, incluindo valores nulos, vazios ou inválidos. Outro erro frequente é otimizar prematuramente. Primeiro faça o código funcionar corretamente, depois otimize se necessário. Como diz o ditado: 'premature optimization is the root of all evil'. Quando encontrar problemas, use o debugger para entender o que está acontecendo. Coloque breakpoints, inspecione variáveis e execute o código passo a passo. Esta é uma habilidade essencial para todo desenvolvedor. Mensagens de erro podem parecer intimidadoras no início, mas elas são suas aliadas. Leia-as cuidadosamente - elas geralmente indicam exatamente onde e qual é o problema. Se algo não funcionar como esperado, não desanime. Debugging é uma parte normal do desenvolvimento. Até desenvolvedores experientes passam tempo significativo depurando código. Testar seu código é tão importante quanto escrevê-lo. Testes automatizados garantem que seu código funciona corretamente e continuará funcionando quando você fizer mudanças. Comece escrevendo testes simples para os casos mais comuns. Depois adicione testes para casos extremos e situações de erro. Uma boa cobertura de testes dá confiança para refatorar e melhorar o código. Considere a prática de TDD (Test-Driven Development), onde você escreve os testes antes do código. Isso ajuda a pensar claramente sobre o que o código deve fazer. Embora a correção seja sempre a prioridade, é importante estar ciente das implicações de performance de suas escolhas. Diferentes abordagens podem ter custos computacionais muito diferentes. Em muitos casos, a diferença de performance é negligenciável para aplicações pequenas. No entanto, à medida que seus programas crescem, escolhas eficientes se tornam cada vez mais importantes. Aprenda a usar ferramentas de profiling para identificar gargalos de performance. Otimize baseado em dados reais, não em suposições sobre onde o código é lento. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0004-000000000001"),
            CourseId = _courseId,
            Title = "Introdução a Bancos de Dados",
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
                "Aprender a sintaxe básica do SQL",
                "Compreender os principais comandos DDL e DML",
                "Criar e executar consultas SQL simples",
                "Entender tipos de dados em SQL"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "O que é SQL?",
                    Content = "SQL (Structured Query Language) é a linguagem padrão para interagir com bancos de dados relacionais. Criada nos anos 1970, SQL permite criar, modificar, consultar e gerenciar dados de forma declarativa. Ao contrário de linguagens de programação procedurais como C#, onde você especifica como fazer algo passo a passo, em SQL você declara o que deseja obter e o banco de dados determina a melhor forma de executar. SQL é dividido em várias categorias de comandos: DDL (Data Definition Language) para definir estruturas como tabelas e índices, DML (Data Manipulation Language) para manipular dados, DCL (Data Control Language) para controlar permissões, e TCL (Transaction Control Language) para gerenciar transações. A beleza do SQL está em sua simplicidade e poder expressivo. Com poucas linhas de código, você pode realizar operações complexas em milhões de registros. SQL é uma habilidade essencial para desenvolvedores, analistas de dados e administradores de banco de dados. Embora existam variações entre diferentes sistemas de banco de dados, o SQL padrão ANSI é amplamente suportado, tornando o conhecimento transferível entre plataformas.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Comandos DDL - Definindo Estruturas",
                    Content = "DDL (Data Definition Language) é usado para definir e modificar a estrutura do banco de dados. O comando CREATE cria novos objetos como tabelas, índices ou views. Por exemplo, CREATE TABLE define uma nova tabela especificando suas colunas e tipos de dados. O comando ALTER modifica estruturas existentes, permitindo adicionar ou remover colunas, alterar tipos de dados ou modificar restrições. DROP remove objetos do banco de dados permanentemente. TRUNCATE remove todos os dados de uma tabela mas mantém sua estrutura. Ao criar tabelas, você define constraints (restrições) que garantem a integridade dos dados. PRIMARY KEY identifica unicamente cada registro. FOREIGN KEY estabelece relacionamentos entre tabelas. NOT NULL garante que uma coluna sempre tenha valor. UNIQUE impede valores duplicados. CHECK valida que os dados atendam a condições específicas. Essas restrições são fundamentais para manter a qualidade e consistência dos dados. Um bom design de banco de dados começa com comandos DDL bem planejados que refletem adequadamente as regras de negócio da aplicação.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Comandos DML - Manipulando Dados",
                    Content = "DML (Data Manipulation Language) é usado para manipular os dados dentro das tabelas. SELECT é o comando mais usado, permitindo consultar e recuperar dados. INSERT adiciona novos registros às tabelas. UPDATE modifica registros existentes. DELETE remove registros. Esses quatro comandos formam a base de praticamente todas as operações de dados em aplicações. O comando SELECT pode ser simples, recuperando todas as colunas de uma tabela, ou extremamente complexo, combinando dados de múltiplas tabelas com filtros, ordenação e agregações. INSERT pode adicionar um único registro ou múltiplos registros de uma vez. UPDATE pode modificar um único campo ou múltiplos campos, em um ou vários registros simultaneamente. DELETE pode remover registros específicos ou todos os registros de uma tabela. É crucial usar cláusulas WHERE com UPDATE e DELETE para evitar modificar ou excluir dados não intencionalmente. Comandos DML são frequentemente usados dentro de transações para garantir que múltiplas operações sejam executadas atomicamente, ou seja, todas com sucesso ou nenhuma.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Criando uma Tabela com DDL",
                    Code = @"-- Criar tabela de Produtos
CREATE TABLE Produtos (
    ProdutoID INT PRIMARY KEY IDENTITY(1,1),
    Nome NVARCHAR(100) NOT NULL,
    Descricao NVARCHAR(500),
    Preco DECIMAL(10,2) NOT NULL CHECK (Preco >= 0),
    Estoque INT NOT NULL DEFAULT 0,
    DataCadastro DATETIME NOT NULL DEFAULT GETDATE(),
    Ativo BIT NOT NULL DEFAULT 1
);",
                    Language = "sql",
                    Explanation = "Este comando cria uma tabela Produtos com várias colunas e restrições. IDENTITY gera IDs automaticamente, NOT NULL impede valores nulos, CHECK valida que o preço não seja negativo, e DEFAULT define valores padrão.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Inserindo Dados com DML",
                    Code = @"-- Inserir um único produto
INSERT INTO Produtos (Nome, Descricao, Preco, Estoque)
VALUES ('Notebook', 'Notebook Dell 15 polegadas', 2500.00, 10);

-- Inserir múltiplos produtos
INSERT INTO Produtos (Nome, Preco, Estoque)
VALUES 
    ('Mouse', 50.00, 100),
    ('Teclado', 150.00, 50),
    ('Monitor', 800.00, 25);",
                    Language = "sql",
                    Explanation = "O comando INSERT adiciona novos registros à tabela. Você pode inserir um registro por vez ou múltiplos registros em uma única operação. Colunas com DEFAULT ou IDENTITY podem ser omitidas.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Crie sua Primeira Tabela",
                    Description = "Crie uma tabela chamada Clientes com as colunas: ClienteID (chave primária auto-incremento), Nome (obrigatório), Email (obrigatório e único), Telefone, e DataCadastro (com valor padrão da data atual).",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"CREATE TABLE Clientes (
    -- Complete aqui
);",
                    Hints = new List<string>
                    {
                        "Use INT IDENTITY(1,1) para chave primária auto-incremento",
                        "Use NOT NULL para campos obrigatórios",
                        "Use UNIQUE para garantir emails únicos",
                        "Use DEFAULT GETDATE() para data atual"
                    }
                },
                new Exercise
                {
                    Title = "Insira Dados",
                    Description = "Usando a tabela Clientes criada anteriormente, insira 3 clientes com dados fictícios.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"INSERT INTO Clientes (Nome, Email, Telefone)
VALUES 
    -- Complete aqui com 3 clientes
    ();",
                    Hints = new List<string>
                    {
                        "Separe múltiplos registros com vírgula",
                        "Use aspas simples para strings",
                        "Não precisa especificar ClienteID nem DataCadastro"
                    }
                },
                new Exercise
                {
                    Title = "Modifique a Estrutura",
                    Description = "Adicione uma coluna Cidade (NVARCHAR(100)) à tabela Clientes usando o comando ALTER TABLE.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"ALTER TABLE Clientes
-- Complete aqui
;",
                    Hints = new List<string>
                    {
                        "Use ADD COLUMN para adicionar uma nova coluna",
                        "Especifique o tipo de dado apropriado",
                        "Considere se a coluna deve aceitar NULL"
                    }
                }
            },
            Summary = "Nesta aula você aprendeu a sintaxe básica do SQL e os principais comandos DDL e DML. Você viu como criar tabelas com CREATE TABLE, definir restrições de integridade, e inserir dados com INSERT. Esses comandos são a base para trabalhar com bancos de dados relacionais. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno. Grandes empresas como Microsoft, Google e Amazon aplicam estes conceitos em seus sistemas. Ao aprender estas técnicas, você está adquirindo conhecimento usado por desenvolvedores em todo o mundo. Considere como este conceito poderia ser aplicado em um projeto pessoal. Pensar em aplicações práticas ajuda a solidificar o entendimento e torna o aprendizado mais significativo. Iniciantes frequentemente cometem alguns erros ao aprender este tópico. Conhecer estes erros comuns ajuda a evitá-los e desenvolver código mais robusto desde o início. Um erro comum é não considerar casos extremos ou situações inesperadas. Sempre teste seu código com diferentes tipos de entrada, incluindo valores nulos, vazios ou inválidos. Outro erro frequente é otimizar prematuramente. Primeiro faça o código funcionar corretamente, depois otimize se necessário. Como diz o ditado: 'premature optimization is the root of all evil'. Quando encontrar problemas, use o debugger para entender o que está acontecendo. Coloque breakpoints, inspecione variáveis e execute o código passo a passo. Esta é uma habilidade essencial para todo desenvolvedor. Mensagens de erro podem parecer intimidadoras no início, mas elas são suas aliadas. Leia-as cuidadosamente - elas geralmente indicam exatamente onde e qual é o problema. Se algo não funcionar como esperado, não desanime. Debugging é uma parte normal do desenvolvimento. Até desenvolvedores experientes passam tempo significativo depurando código. Testar seu código é tão importante quanto escrevê-lo. Testes automatizados garantem que seu código funciona corretamente e continuará funcionando quando você fizer mudanças. Comece escrevendo testes simples para os casos mais comuns. Depois adicione testes para casos extremos e situações de erro. Uma boa cobertura de testes dá confiança para refatorar e melhorar o código. Considere a prática de TDD (Test-Driven Development), onde você escreve os testes antes do código. Isso ajuda a pensar claramente sobre o que o código deve fazer. Embora a correção seja sempre a prioridade, é importante estar ciente das implicações de performance de suas escolhas. Diferentes abordagens podem ter custos computacionais muito diferentes. Em muitos casos, a diferença de performance é negligenciável para aplicações pequenas. No entanto, à medida que seus programas crescem, escolhas eficientes se tornam cada vez mais importantes. Aprenda a usar ferramentas de profiling para identificar gargalos de performance. Otimize baseado em dados reais, não em suposições sobre onde o código é lento. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0004-000000000002"),
            CourseId = _courseId,
            Title = "SQL Básico - DDL e DML",
            Duration = "55 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 55,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0004-000000000001" }),
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
                "Dominar o comando SELECT para consultas básicas",
                "Usar cláusulas WHERE para filtrar dados",
                "Ordenar resultados com ORDER BY",
                "Limitar resultados com TOP e OFFSET"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "O Comando SELECT",
                    Content = "SELECT é o comando mais utilizado em SQL, responsável por consultar e recuperar dados de tabelas. Em sua forma mais simples, SELECT * FROM Tabela retorna todas as colunas e linhas de uma tabela. No entanto, o verdadeiro poder do SELECT está em sua capacidade de filtrar, ordenar, agrupar e transformar dados. Você pode selecionar colunas específicas em vez de todas, renomear colunas com aliases, realizar cálculos e aplicar funções. SELECT não modifica dados, apenas os recupera, tornando-o seguro para exploração e análise. A estrutura básica de uma consulta SELECT inclui: SELECT (colunas desejadas), FROM (tabela de origem), WHERE (filtros), ORDER BY (ordenação), e outras cláusulas opcionais. Dominar SELECT é essencial porque praticamente todas as operações de leitura de dados em aplicações usam este comando. Desde exibir uma lista de produtos até gerar relatórios complexos, SELECT é a ferramenta fundamental. Consultas SELECT bem escritas são eficientes e legíveis, facilitando manutenção e otimização futura.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Filtrando Dados com WHERE",
                    Content = "A cláusula WHERE permite filtrar registros baseado em condições específicas. Sem WHERE, SELECT retorna todos os registros da tabela, o que raramente é desejado em aplicações reais. WHERE usa operadores de comparação como = (igual), != ou <> (diferente), > (maior), < (menor), >= (maior ou igual), <= (menor ou igual). Você pode combinar múltiplas condições usando AND (todas devem ser verdadeiras) e OR (pelo menos uma deve ser verdadeira). O operador IN verifica se um valor está em uma lista. BETWEEN verifica se um valor está em um intervalo. LIKE permite buscar padrões em texto usando % (qualquer sequência de caracteres) e _ (um único caractere). IS NULL e IS NOT NULL verificam valores nulos. Parênteses podem agrupar condições para controlar a ordem de avaliação. Filtros eficientes são cruciais para performance, especialmente em tabelas grandes. Usar índices nas colunas filtradas pode acelerar drasticamente as consultas. WHERE também é usado em UPDATE e DELETE para especificar quais registros devem ser modificados ou removidos.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Ordenação e Limitação de Resultados",
                    Content = "ORDER BY ordena os resultados de uma consulta por uma ou mais colunas. Por padrão, a ordenação é ascendente (ASC), mas você pode especificar descendente (DESC). Ordenar por múltiplas colunas é útil quando há valores duplicados na primeira coluna de ordenação. Por exemplo, ORDER BY Categoria, Preco DESC ordena primeiro por categoria e depois por preço decrescente dentro de cada categoria. A ordenação acontece após a filtragem e antes da limitação de resultados. TOP (SQL Server) ou LIMIT (MySQL/PostgreSQL) limita o número de registros retornados. TOP 10 retorna os primeiros 10 registros. OFFSET e FETCH são usados para paginação, permitindo pular um número específico de registros e buscar os próximos. Por exemplo, OFFSET 20 ROWS FETCH NEXT 10 ROWS ONLY pula os primeiros 20 registros e retorna os próximos 10, útil para implementar paginação em aplicações web. Ordenação pode impactar performance em tabelas grandes, especialmente sem índices apropriados. Sempre considere se a ordenação é realmente necessária ou se pode ser feita na aplicação.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "SELECT com Filtros e Ordenação",
                    Code = @"-- Selecionar produtos específicos
SELECT Nome, Preco, Estoque
FROM Produtos
WHERE Preco > 100 AND Estoque > 0
ORDER BY Preco DESC;

-- Buscar produtos por nome (padrão)
SELECT *
FROM Produtos
WHERE Nome LIKE '%Notebook%'
ORDER BY Nome;

-- Produtos em uma faixa de preço
SELECT Nome, Preco
FROM Produtos
WHERE Preco BETWEEN 500 AND 2000
ORDER BY Preco;",
                    Language = "sql",
                    Explanation = "Estes exemplos mostram diferentes formas de filtrar dados: comparação numérica, busca por padrão com LIKE, e verificação de intervalo com BETWEEN. ORDER BY organiza os resultados.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Paginação de Resultados",
                    Code = @"-- Buscar os 10 produtos mais caros
SELECT TOP 10 Nome, Preco
FROM Produtos
ORDER BY Preco DESC;

-- Paginação: segunda página (registros 11-20)
SELECT Nome, Preco
FROM Produtos
ORDER BY ProdutoID
OFFSET 10 ROWS
FETCH NEXT 10 ROWS ONLY;

-- Produtos ativos com limite
SELECT TOP 5 Nome, Preco, Estoque
FROM Produtos
WHERE Ativo = 1
ORDER BY DataCadastro DESC;",
                    Language = "sql",
                    Explanation = "TOP retorna um número específico de registros. OFFSET/FETCH permite paginação, essencial para aplicações web que exibem resultados em páginas. A ordenação é importante para resultados consistentes.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Consulta Básica",
                    Description = "Escreva uma consulta que retorne o nome e email de todos os clientes cadastrados após 01/01/2024, ordenados por nome.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"SELECT 
FROM Clientes
WHERE 
ORDER BY ;",
                    Hints = new List<string>
                    {
                        "Use DataCadastro > '2024-01-01' para filtrar",
                        "Selecione apenas as colunas Nome e Email",
                        "ORDER BY Nome para ordenar alfabeticamente"
                    }
                },
                new Exercise
                {
                    Title = "Filtros Combinados",
                    Description = "Busque produtos que custam entre 100 e 500 reais, tenham estoque maior que 5, e estejam ativos. Ordene por preço crescente.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"SELECT Nome, Preco, Estoque
FROM Produtos
WHERE 
ORDER BY ;",
                    Hints = new List<string>
                    {
                        "Use BETWEEN para faixa de preço",
                        "Combine condições com AND",
                        "Ativo = 1 para produtos ativos",
                        "ORDER BY Preco ASC (ou apenas Preco)"
                    }
                },
                new Exercise
                {
                    Title = "Busca com Padrão",
                    Description = "Encontre todos os clientes cujo nome começa com 'Maria' ou cujo email contém 'gmail.com'. Retorne os 20 primeiros resultados.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"SELECT TOP 
FROM Clientes
WHERE 
;",
                    Hints = new List<string>
                    {
                        "Use LIKE 'Maria%' para nomes que começam com Maria",
                        "Use LIKE '%gmail.com%' para emails do Gmail",
                        "Combine condições com OR",
                        "TOP 20 limita os resultados"
                    }
                }
            },
            Summary = "Nesta aula você dominou o comando SELECT para consultar dados, aprendeu a filtrar registros com WHERE usando diversos operadores, ordenar resultados com ORDER BY, e limitar resultados com TOP e OFFSET/FETCH. Essas habilidades são fundamentais para recuperar exatamente os dados que você precisa. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno. Grandes empresas como Microsoft, Google e Amazon aplicam estes conceitos em seus sistemas. Ao aprender estas técnicas, você está adquirindo conhecimento usado por desenvolvedores em todo o mundo. Considere como este conceito poderia ser aplicado em um projeto pessoal. Pensar em aplicações práticas ajuda a solidificar o entendimento e torna o aprendizado mais significativo. Iniciantes frequentemente cometem alguns erros ao aprender este tópico. Conhecer estes erros comuns ajuda a evitá-los e desenvolver código mais robusto desde o início. Um erro comum é não considerar casos extremos ou situações inesperadas. Sempre teste seu código com diferentes tipos de entrada, incluindo valores nulos, vazios ou inválidos. Outro erro frequente é otimizar prematuramente. Primeiro faça o código funcionar corretamente, depois otimize se necessário. Como diz o ditado: 'premature optimization is the root of all evil'. Quando encontrar problemas, use o debugger para entender o que está acontecendo. Coloque breakpoints, inspecione variáveis e execute o código passo a passo. Esta é uma habilidade essencial para todo desenvolvedor. Mensagens de erro podem parecer intimidadoras no início, mas elas são suas aliadas. Leia-as cuidadosamente - elas geralmente indicam exatamente onde e qual é o problema. Se algo não funcionar como esperado, não desanime. Debugging é uma parte normal do desenvolvimento. Até desenvolvedores experientes passam tempo significativo depurando código. Testar seu código é tão importante quanto escrevê-lo. Testes automatizados garantem que seu código funciona corretamente e continuará funcionando quando você fizer mudanças. Comece escrevendo testes simples para os casos mais comuns. Depois adicione testes para casos extremos e situações de erro. Uma boa cobertura de testes dá confiança para refatorar e melhorar o código. Considere a prática de TDD (Test-Driven Development), onde você escreve os testes antes do código. Isso ajuda a pensar claramente sobre o que o código deve fazer. Embora a correção seja sempre a prioridade, é importante estar ciente das implicações de performance de suas escolhas. Diferentes abordagens podem ter custos computacionais muito diferentes. Em muitos casos, a diferença de performance é negligenciável para aplicações pequenas. No entanto, à medida que seus programas crescem, escolhas eficientes se tornam cada vez mais importantes. Aprenda a usar ferramentas de profiling para identificar gargalos de performance. Otimize baseado em dados reais, não em suposições sobre onde o código é lento. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0004-000000000003"),
            CourseId = _courseId,
            Title = "SELECT - Consultando Dados",
            Duration = "60 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 60,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0004-000000000002" }),
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
                "Aprender a inserir dados com INSERT",
                "Modificar dados existentes com UPDATE",
                "Remover dados com DELETE",
                "Entender as melhores práticas de manipulação de dados"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Inserindo Dados com INSERT",
                    Content = "O comando INSERT adiciona novos registros às tabelas. Existem várias formas de usar INSERT. A forma mais comum especifica as colunas e valores: INSERT INTO Tabela (Coluna1, Coluna2) VALUES (Valor1, Valor2). Você pode omitir colunas que têm valores DEFAULT ou permitem NULL. INSERT pode adicionar múltiplos registros em uma única operação, o que é mais eficiente que múltiplos INSERTs individuais. INSERT INTO SELECT permite copiar dados de uma tabela para outra baseado em uma consulta. Ao inserir dados, é importante considerar as restrições da tabela. Chaves primárias devem ser únicas, chaves estrangeiras devem referenciar registros existentes, e colunas NOT NULL devem ter valores. Violações dessas restrições resultam em erros. Em SQL Server, IDENTITY gera valores automaticamente para chaves primárias, então você não precisa especificá-las. Após um INSERT, você pode usar SCOPE_IDENTITY() para obter o ID gerado. Em aplicações, é comum inserir dados recebidos de formulários ou APIs, sempre validando e sanitizando os dados antes para evitar SQL injection e garantir integridade.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Atualizando Dados com UPDATE",
                    Content = "UPDATE modifica registros existentes em uma tabela. A sintaxe básica é: UPDATE Tabela SET Coluna1 = Valor1, Coluna2 = Valor2 WHERE Condição. A cláusula WHERE é crucial - sem ela, UPDATE modifica TODOS os registros da tabela, o que raramente é intencional. Sempre use WHERE para especificar quais registros devem ser atualizados. Você pode atualizar múltiplas colunas em uma única operação. UPDATE pode usar valores calculados, como incrementar um contador: SET Estoque = Estoque + 10. Você pode usar subconsultas em UPDATE para definir valores baseados em outras tabelas. UPDATE retorna o número de registros afetados, útil para verificar se a operação teve efeito. Em aplicações, UPDATE é usado para modificar dados de perfis de usuários, atualizar status de pedidos, ajustar inventário, entre outros. É importante implementar controle de concorrência para evitar que múltiplos usuários atualizem o mesmo registro simultaneamente, causando perda de dados. Técnicas como versionamento ou timestamps ajudam a detectar e resolver conflitos de atualização.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Removendo Dados com DELETE",
                    Content = "DELETE remove registros de uma tabela. A sintaxe é: DELETE FROM Tabela WHERE Condição. Assim como UPDATE, a cláusula WHERE é essencial. DELETE sem WHERE remove TODOS os registros da tabela, mantendo apenas a estrutura. Se você quer remover todos os dados e resetar a tabela, TRUNCATE é mais eficiente que DELETE, mas não pode ser usado se há chaves estrangeiras referenciando a tabela. DELETE respeita restrições de chave estrangeira - você não pode deletar um registro se outros registros dependem dele, a menos que use CASCADE DELETE. DELETE retorna o número de registros removidos. Em muitas aplicações, em vez de deletar fisicamente registros, usa-se 'soft delete' - marcar registros como inativos com uma flag ou data de exclusão. Isso preserva histórico e permite recuperação. DELETE é uma operação irreversível (sem transação), então sempre faça backup antes de deletar dados importantes. Em produção, é comum restringir permissões de DELETE a administradores e usar auditoria para rastrear exclusões.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "INSERT, UPDATE e DELETE Básicos",
                    Code = @"-- Inserir novo cliente
INSERT INTO Clientes (Nome, Email, Telefone)
VALUES ('Carlos Souza', 'carlos@email.com', '11999999999');

-- Atualizar telefone de um cliente
UPDATE Clientes
SET Telefone = '11988888888'
WHERE Email = 'carlos@email.com';

-- Deletar cliente específico
DELETE FROM Clientes
WHERE ClienteID = 5;

-- Atualizar múltiplas colunas
UPDATE Produtos
SET Preco = Preco * 1.1, -- Aumentar preço em 10%
    DataAtualizacao = GETDATE()
WHERE Categoria = 'Eletrônicos';",
                    Language = "sql",
                    Explanation = "Exemplos básicos de INSERT, UPDATE e DELETE. Note o uso de WHERE em UPDATE e DELETE para especificar registros específicos. UPDATE pode modificar múltiplas colunas e usar cálculos.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "INSERT Múltiplo e Soft Delete",
                    Code = @"-- Inserir múltiplos registros de uma vez
INSERT INTO Produtos (Nome, Preco, Estoque, Categoria)
VALUES 
    ('Mouse Gamer', 120.00, 50, 'Periféricos'),
    ('Teclado Mecânico', 350.00, 30, 'Periféricos'),
    ('Webcam HD', 200.00, 40, 'Periféricos');

-- Soft Delete: marcar como inativo em vez de deletar
UPDATE Clientes
SET Ativo = 0, DataExclusao = GETDATE()
WHERE ClienteID = 10;

-- Consultar apenas registros ativos
SELECT * FROM Clientes WHERE Ativo = 1;",
                    Language = "sql",
                    Explanation = "INSERT múltiplo é mais eficiente que múltiplos INSERTs. Soft delete preserva dados marcando como inativos, permitindo recuperação e mantendo histórico. Sempre filtre por Ativo = 1 em consultas.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Inserir Novos Produtos",
                    Description = "Insira 3 novos produtos na tabela Produtos com nome, preço e estoque de sua escolha.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"INSERT INTO Produtos (Nome, Preco, Estoque)
VALUES 
    -- Complete aqui
    ();",
                    Hints = new List<string>
                    {
                        "Separe múltiplos registros com vírgula",
                        "Use valores realistas para preço e estoque",
                        "Não esqueça as aspas simples para strings"
                    }
                },
                new Exercise
                {
                    Title = "Atualizar Preços",
                    Description = "Aumente em 15% o preço de todos os produtos da categoria 'Eletrônicos' que custam menos de 1000 reais.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"UPDATE Produtos
SET 
WHERE ;",
                    Hints = new List<string>
                    {
                        "Use Preco = Preco * 1.15 para aumentar 15%",
                        "Combine condições: Categoria = 'Eletrônicos' AND Preco < 1000",
                        "Teste com SELECT antes de executar UPDATE"
                    }
                },
                new Exercise
                {
                    Title = "Implementar Soft Delete",
                    Description = "Em vez de deletar, implemente soft delete para clientes inativos há mais de 2 anos. Adicione uma coluna DataExclusao e marque Ativo = 0.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"-- Primeiro, adicione a coluna se não existir
ALTER TABLE Clientes ADD DataExclusao DATETIME NULL;

-- Agora implemente o soft delete
UPDATE Clientes
SET 
WHERE ;",
                    Hints = new List<string>
                    {
                        "Use DATEADD para calcular data de 2 anos atrás",
                        "Defina Ativo = 0 e DataExclusao = GETDATE()",
                        "Filtre por DataUltimoAcesso < DATEADD(year, -2, GETDATE())"
                    }
                }
            },
            Summary = "Nesta aula você aprendeu a manipular dados com INSERT, UPDATE e DELETE. Você viu como inserir múltiplos registros, atualizar dados com cálculos, e implementar soft delete. Sempre use WHERE com UPDATE e DELETE para evitar modificar dados não intencionalmente. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno. Grandes empresas como Microsoft, Google e Amazon aplicam estes conceitos em seus sistemas. Ao aprender estas técnicas, você está adquirindo conhecimento usado por desenvolvedores em todo o mundo. Considere como este conceito poderia ser aplicado em um projeto pessoal. Pensar em aplicações práticas ajuda a solidificar o entendimento e torna o aprendizado mais significativo. Iniciantes frequentemente cometem alguns erros ao aprender este tópico. Conhecer estes erros comuns ajuda a evitá-los e desenvolver código mais robusto desde o início. Um erro comum é não considerar casos extremos ou situações inesperadas. Sempre teste seu código com diferentes tipos de entrada, incluindo valores nulos, vazios ou inválidos. Outro erro frequente é otimizar prematuramente. Primeiro faça o código funcionar corretamente, depois otimize se necessário. Como diz o ditado: 'premature optimization is the root of all evil'. Quando encontrar problemas, use o debugger para entender o que está acontecendo. Coloque breakpoints, inspecione variáveis e execute o código passo a passo. Esta é uma habilidade essencial para todo desenvolvedor. Mensagens de erro podem parecer intimidadoras no início, mas elas são suas aliadas. Leia-as cuidadosamente - elas geralmente indicam exatamente onde e qual é o problema. Se algo não funcionar como esperado, não desanime. Debugging é uma parte normal do desenvolvimento. Até desenvolvedores experientes passam tempo significativo depurando código. Testar seu código é tão importante quanto escrevê-lo. Testes automatizados garantem que seu código funciona corretamente e continuará funcionando quando você fizer mudanças. Comece escrevendo testes simples para os casos mais comuns. Depois adicione testes para casos extremos e situações de erro. Uma boa cobertura de testes dá confiança para refatorar e melhorar o código. Considere a prática de TDD (Test-Driven Development), onde você escreve os testes antes do código. Isso ajuda a pensar claramente sobre o que o código deve fazer. Embora a correção seja sempre a prioridade, é importante estar ciente das implicações de performance de suas escolhas. Diferentes abordagens podem ter custos computacionais muito diferentes. Em muitos casos, a diferença de performance é negligenciável para aplicações pequenas. No entanto, à medida que seus programas crescem, escolhas eficientes se tornam cada vez mais importantes. Aprenda a usar ferramentas de profiling para identificar gargalos de performance. Otimize baseado em dados reais, não em suposições sobre onde o código é lento. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0004-000000000004"),
            CourseId = _courseId,
            Title = "INSERT, UPDATE e DELETE",
            Duration = "55 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 55,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0004-000000000003" }),
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
                "Compreender funções de agregação em SQL",
                "Usar GROUP BY para agrupar dados",
                "Filtrar grupos com HAVING",
                "Aplicar agregações em cenários reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Funções de Agregação",
                    Content = "Funções de agregação realizam cálculos em conjuntos de valores e retornam um único resultado. COUNT() conta o número de registros. SUM() soma valores numéricos. AVG() calcula a média. MIN() e MAX() encontram o menor e maior valor, respectivamente. Essas funções são fundamentais para análise de dados e geração de relatórios. COUNT(*) conta todas as linhas, incluindo NULLs, enquanto COUNT(Coluna) conta apenas valores não-nulos. SUM() e AVG() ignoram valores NULL automaticamente. Você pode usar DISTINCT dentro de funções de agregação para contar ou somar apenas valores únicos: COUNT(DISTINCT Categoria) conta quantas categorias diferentes existem. Funções de agregação são frequentemente usadas com GROUP BY para calcular estatísticas por grupo. Por exemplo, calcular o total de vendas por mês, a média de preços por categoria, ou o número de pedidos por cliente. Sem GROUP BY, funções de agregação operam em todos os registros retornados pela consulta. Dominar agregações é essencial para transformar dados brutos em informações úteis para tomada de decisões.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Agrupando Dados com GROUP BY",
                    Content = "GROUP BY agrupa registros que têm valores iguais em colunas especificadas, permitindo aplicar funções de agregação a cada grupo. Por exemplo, GROUP BY Categoria agrupa produtos por categoria, permitindo calcular o preço médio de cada categoria. Todas as colunas no SELECT que não são funções de agregação devem estar no GROUP BY. Você pode agrupar por múltiplas colunas: GROUP BY Categoria, Marca agrupa por combinações únicas de categoria e marca. A ordem das colunas no GROUP BY não afeta o resultado, mas pode afetar a performance. GROUP BY é processado após WHERE mas antes de ORDER BY. Isso significa que você filtra registros individuais com WHERE, depois agrupa, e finalmente ordena os grupos. GROUP BY é extremamente útil para relatórios: vendas por região, produtos mais vendidos por categoria, clientes com mais pedidos, etc. Em aplicações de business intelligence e dashboards, praticamente todos os gráficos e métricas usam GROUP BY de alguma forma. Entender quando e como agrupar dados é uma habilidade crucial para análise de dados.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Filtrando Grupos com HAVING",
                    Content = "HAVING filtra grupos após a agregação, enquanto WHERE filtra registros individuais antes da agregação. Use WHERE para filtrar linhas e HAVING para filtrar grupos. Por exemplo, WHERE Preco > 100 filtra produtos caros antes de agrupar, enquanto HAVING SUM(Quantidade) > 50 filtra grupos cuja soma de quantidade excede 50. HAVING só pode ser usado com GROUP BY e geralmente contém funções de agregação. Você pode combinar WHERE e HAVING na mesma consulta: WHERE filtra os dados de entrada, GROUP BY agrupa, HAVING filtra os grupos, e ORDER BY ordena o resultado final. HAVING é essencial para responder perguntas como 'quais clientes fizeram mais de 10 pedidos?' ou 'quais categorias têm valor total de vendas acima de 10000?'. A ordem de execução é: FROM, WHERE, GROUP BY, HAVING, SELECT, ORDER BY. Entender essa ordem ajuda a escrever consultas corretas e eficientes. HAVING pode usar aliases definidos no SELECT em alguns bancos de dados, mas é mais portável usar a expressão completa.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Funções de Agregação Básicas",
                    Code = @"-- Estatísticas gerais de produtos
SELECT 
    COUNT(*) AS TotalProdutos,
    AVG(Preco) AS PrecoMedio,
    MIN(Preco) AS PrecoMinimo,
    MAX(Preco) AS PrecoMaximo,
    SUM(Estoque) AS EstoqueTotal
FROM Produtos;

-- Contar produtos por categoria
SELECT 
    Categoria,
    COUNT(*) AS QuantidadeProdutos,
    AVG(Preco) AS PrecoMedio
FROM Produtos
GROUP BY Categoria
ORDER BY QuantidadeProdutos DESC;",
                    Language = "sql",
                    Explanation = "O primeiro exemplo calcula estatísticas gerais sem agrupamento. O segundo agrupa por categoria, mostrando quantos produtos e o preço médio de cada categoria. AS cria aliases para colunas calculadas.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "GROUP BY com HAVING",
                    Code = @"-- Categorias com mais de 5 produtos e preço médio > 200
SELECT 
    Categoria,
    COUNT(*) AS QuantidadeProdutos,
    AVG(Preco) AS PrecoMedio,
    SUM(Estoque) AS EstoqueTotal
FROM Produtos
WHERE Ativo = 1
GROUP BY Categoria
HAVING COUNT(*) > 5 AND AVG(Preco) > 200
ORDER BY PrecoMedio DESC;

-- Clientes com mais de 3 pedidos
SELECT 
    ClienteID,
    COUNT(*) AS TotalPedidos,
    SUM(ValorTotal) AS ValorTotalGasto
FROM Pedidos
GROUP BY ClienteID
HAVING COUNT(*) > 3
ORDER BY ValorTotalGasto DESC;",
                    Language = "sql",
                    Explanation = "WHERE filtra produtos ativos antes de agrupar. HAVING filtra grupos após agregação, mantendo apenas categorias com mais de 5 produtos e preço médio acima de 200. O segundo exemplo identifica clientes frequentes.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Estatísticas de Vendas",
                    Description = "Calcule o total de vendas, número de pedidos e ticket médio (valor médio por pedido) da tabela Pedidos.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"SELECT 
    -- Complete aqui
FROM Pedidos;",
                    Hints = new List<string>
                    {
                        "Use SUM(ValorTotal) para total de vendas",
                        "Use COUNT(*) para número de pedidos",
                        "Use AVG(ValorTotal) para ticket médio",
                        "Dê aliases descritivos com AS"
                    }
                },
                new Exercise
                {
                    Title = "Produtos por Categoria",
                    Description = "Liste cada categoria com a quantidade de produtos, preço médio, e estoque total. Ordene por quantidade de produtos decrescente.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"SELECT 
    Categoria,
    -- Complete aqui
FROM Produtos
GROUP BY 
ORDER BY ;",
                    Hints = new List<string>
                    {
                        "Use COUNT(*), AVG(Preco), SUM(Estoque)",
                        "GROUP BY Categoria",
                        "ORDER BY QuantidadeProdutos DESC"
                    }
                },
                new Exercise
                {
                    Title = "Clientes VIP",
                    Description = "Identifique clientes que gastaram mais de 5000 reais no total e fizeram pelo menos 5 pedidos. Mostre ClienteID, total gasto e número de pedidos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"SELECT 
    ClienteID,
    -- Complete aqui
FROM Pedidos
GROUP BY 
HAVING ;",
                    Hints = new List<string>
                    {
                        "Use SUM(ValorTotal) e COUNT(*)",
                        "HAVING SUM(ValorTotal) > 5000 AND COUNT(*) >= 5",
                        "Ordene por total gasto decrescente"
                    }
                }
            },
            Summary = "Nesta aula você aprendeu funções de agregação (COUNT, SUM, AVG, MIN, MAX), como agrupar dados com GROUP BY, e filtrar grupos com HAVING. Essas ferramentas são essenciais para análise de dados e geração de relatórios em aplicações. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno. Grandes empresas como Microsoft, Google e Amazon aplicam estes conceitos em seus sistemas. Ao aprender estas técnicas, você está adquirindo conhecimento usado por desenvolvedores em todo o mundo. Considere como este conceito poderia ser aplicado em um projeto pessoal. Pensar em aplicações práticas ajuda a solidificar o entendimento e torna o aprendizado mais significativo. Iniciantes frequentemente cometem alguns erros ao aprender este tópico. Conhecer estes erros comuns ajuda a evitá-los e desenvolver código mais robusto desde o início. Um erro comum é não considerar casos extremos ou situações inesperadas. Sempre teste seu código com diferentes tipos de entrada, incluindo valores nulos, vazios ou inválidos. Outro erro frequente é otimizar prematuramente. Primeiro faça o código funcionar corretamente, depois otimize se necessário. Como diz o ditado: 'premature optimization is the root of all evil'. Quando encontrar problemas, use o debugger para entender o que está acontecendo. Coloque breakpoints, inspecione variáveis e execute o código passo a passo. Esta é uma habilidade essencial para todo desenvolvedor. Mensagens de erro podem parecer intimidadoras no início, mas elas são suas aliadas. Leia-as cuidadosamente - elas geralmente indicam exatamente onde e qual é o problema. Se algo não funcionar como esperado, não desanime. Debugging é uma parte normal do desenvolvimento. Até desenvolvedores experientes passam tempo significativo depurando código. Testar seu código é tão importante quanto escrevê-lo. Testes automatizados garantem que seu código funciona corretamente e continuará funcionando quando você fizer mudanças. Comece escrevendo testes simples para os casos mais comuns. Depois adicione testes para casos extremos e situações de erro. Uma boa cobertura de testes dá confiança para refatorar e melhorar o código. Considere a prática de TDD (Test-Driven Development), onde você escreve os testes antes do código. Isso ajuda a pensar claramente sobre o que o código deve fazer. Embora a correção seja sempre a prioridade, é importante estar ciente das implicações de performance de suas escolhas. Diferentes abordagens podem ter custos computacionais muito diferentes. Em muitos casos, a diferença de performance é negligenciável para aplicações pequenas. No entanto, à medida que seus programas crescem, escolhas eficientes se tornam cada vez mais importantes. Aprenda a usar ferramentas de profiling para identificar gargalos de performance. Otimize baseado em dados reais, não em suposições sobre onde o código é lento. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0004-000000000005"),
            CourseId = _courseId,
            Title = "Funções de Agregação e GROUP BY",
            Duration = "60 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 60,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0004-000000000004" }),
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
                "Compreender relacionamentos entre tabelas",
                "Dominar INNER JOIN para combinar dados",
                "Usar aliases de tabelas para simplificar consultas",
                "Aplicar JOINs em múltiplas tabelas"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Relacionamentos Entre Tabelas",
                    Content = "Em bancos de dados relacionais, dados são normalizados em múltiplas tabelas para evitar redundância. Relacionamentos conectam essas tabelas através de chaves. Um relacionamento um-para-muitos é o mais comum: um cliente pode ter muitos pedidos, mas cada pedido pertence a um único cliente. Isso é implementado com uma chave estrangeira na tabela 'muitos' (Pedidos.ClienteID) referenciando a chave primária da tabela 'um' (Clientes.ClienteID). Relacionamentos muitos-para-muitos requerem uma tabela intermediária: produtos e pedidos têm relacionamento muitos-para-muitos através de PedidoItens. Relacionamentos um-para-um são raros, usados quando dados são separados por questões de performance ou segurança. Entender relacionamentos é crucial para modelar dados corretamente. Chaves estrangeiras garantem integridade referencial, impedindo que você referencie registros inexistentes. Ao projetar um banco de dados, identifique as entidades, seus atributos e os relacionamentos entre elas. Isso forma a base do modelo de dados que suportará sua aplicação.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "INNER JOIN - Combinando Tabelas",
                    Content = "INNER JOIN combina linhas de duas tabelas baseado em uma condição de junção, geralmente igualdade entre chaves. Apenas linhas que têm correspondência em ambas as tabelas são retornadas. A sintaxe é: SELECT colunas FROM Tabela1 INNER JOIN Tabela2 ON Tabela1.Chave = Tabela2.Chave. INNER JOIN é o tipo mais comum de junção, usado quando você quer dados que existem em ambas as tabelas. Por exemplo, listar pedidos com informações do cliente: SELECT Pedidos.*, Clientes.Nome FROM Pedidos INNER JOIN Clientes ON Pedidos.ClienteID = Clientes.ClienteID. Sem JOIN, você teria apenas o ClienteID no pedido, não o nome do cliente. INNER JOIN permite desnormalizar dados temporariamente para consultas, mantendo a normalização no armazenamento. Você pode fazer JOIN em múltiplas tabelas encadeando JOINs. A ordem dos JOINs geralmente não afeta o resultado, mas pode afetar a performance. O otimizador de consultas do banco de dados determina a melhor ordem de execução. INNER JOIN é fundamental para trabalhar com dados relacionais, permitindo consultas que atravessam relacionamentos.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Aliases e Múltiplos JOINs",
                    Content = "Aliases de tabelas tornam consultas mais legíveis e são essenciais quando você faz JOIN da mesma tabela múltiplas vezes. Use AS ou simplesmente um espaço: FROM Clientes AS C ou FROM Clientes C. Com aliases, você pode escrever C.Nome em vez de Clientes.Nome, tornando consultas mais concisas. Aliases são obrigatórios em self-joins, onde uma tabela é juntada consigo mesma. Por exemplo, uma tabela Funcionarios com ManagerID pode usar self-join para listar funcionários com seus gerentes. Múltiplos JOINs permitem combinar três ou mais tabelas. Por exemplo, para listar pedidos com cliente e produtos: Pedidos JOIN Clientes JOIN PedidoItens JOIN Produtos. A ordem importa: você deve fazer JOIN em tabelas que já estão na consulta. Comece com a tabela principal, depois adicione JOINs para tabelas relacionadas. Cada JOIN adiciona complexidade e pode impactar performance, então use índices nas colunas de junção. Consultas com múltiplos JOINs são comuns em aplicações reais, onde dados estão distribuídos em muitas tabelas normalizadas.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "INNER JOIN Básico",
                    Code = @"-- Listar pedidos com nome do cliente
SELECT 
    P.PedidoID,
    P.DataPedido,
    P.ValorTotal,
    C.Nome AS NomeCliente,
    C.Email
FROM Pedidos P
INNER JOIN Clientes C ON P.ClienteID = C.ClienteID
ORDER BY P.DataPedido DESC;

-- Produtos com nome da categoria
SELECT 
    Prod.Nome AS Produto,
    Prod.Preco,
    Cat.Nome AS Categoria
FROM Produtos Prod
INNER JOIN Categorias Cat ON Prod.CategoriaID = Cat.CategoriaID
WHERE Prod.Ativo = 1;",
                    Language = "sql",
                    Explanation = "INNER JOIN combina Pedidos com Clientes usando ClienteID. Aliases (P, C, Prod, Cat) tornam a consulta mais legível. Apenas pedidos com clientes existentes são retornados.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Múltiplos JOINs",
                    Code = @"-- Listar itens de pedidos com todas as informações
SELECT 
    P.PedidoID,
    C.Nome AS Cliente,
    Prod.Nome AS Produto,
    PI.Quantidade,
    PI.PrecoUnitario,
    (PI.Quantidade * PI.PrecoUnitario) AS Subtotal
FROM Pedidos P
INNER JOIN Clientes C ON P.ClienteID = C.ClienteID
INNER JOIN PedidoItens PI ON P.PedidoID = PI.PedidoID
INNER JOIN Produtos Prod ON PI.ProdutoID = Prod.ProdutoID
WHERE P.DataPedido >= '2024-01-01'
ORDER BY P.PedidoID, Prod.Nome;",
                    Language = "sql",
                    Explanation = "Esta consulta combina 4 tabelas para mostrar detalhes completos de itens de pedidos. Cada JOIN conecta tabelas relacionadas. O cálculo de Subtotal usa colunas das tabelas juntadas.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "JOIN Simples",
                    Description = "Liste todos os pedidos mostrando PedidoID, DataPedido, ValorTotal e o Nome do cliente. Ordene por data decrescente.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"SELECT 
    -- Complete aqui
FROM Pedidos P
INNER JOIN 
ORDER BY ;",
                    Hints = new List<string>
                    {
                        "INNER JOIN Clientes C ON P.ClienteID = C.ClienteID",
                        "Selecione P.PedidoID, P.DataPedido, P.ValorTotal, C.Nome",
                        "ORDER BY P.DataPedido DESC"
                    }
                },
                new Exercise
                {
                    Title = "Produtos e Categorias",
                    Description = "Liste produtos ativos com nome da categoria, preço e estoque. Mostre apenas produtos com estoque maior que 10.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"SELECT 
    -- Complete aqui
FROM Produtos P
INNER JOIN 
WHERE ;",
                    Hints = new List<string>
                    {
                        "JOIN Categorias C ON P.CategoriaID = C.CategoriaID",
                        "WHERE P.Ativo = 1 AND P.Estoque > 10",
                        "Selecione nome do produto, categoria, preço e estoque"
                    }
                },
                new Exercise
                {
                    Title = "Relatório Completo de Vendas",
                    Description = "Crie um relatório mostrando: PedidoID, NomeCliente, NomeProduto, Quantidade, PrecoUnitario e Subtotal para pedidos de 2024.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"SELECT 
    -- Complete aqui
FROM Pedidos P
INNER JOIN 
INNER JOIN 
INNER JOIN 
WHERE ;",
                    Hints = new List<string>
                    {
                        "Você precisa de 3 JOINs: Clientes, PedidoItens, Produtos",
                        "Subtotal = PI.Quantidade * PI.PrecoUnitario",
                        "WHERE YEAR(P.DataPedido) = 2024"
                    }
                }
            },
            Summary = "Nesta aula você aprendeu sobre relacionamentos entre tabelas e como usar INNER JOIN para combinar dados. Você viu como usar aliases para simplificar consultas e como fazer múltiplos JOINs para combinar três ou mais tabelas. JOINs são essenciais para trabalhar com dados relacionais. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno. Grandes empresas como Microsoft, Google e Amazon aplicam estes conceitos em seus sistemas. Ao aprender estas técnicas, você está adquirindo conhecimento usado por desenvolvedores em todo o mundo. Considere como este conceito poderia ser aplicado em um projeto pessoal. Pensar em aplicações práticas ajuda a solidificar o entendimento e torna o aprendizado mais significativo. Iniciantes frequentemente cometem alguns erros ao aprender este tópico. Conhecer estes erros comuns ajuda a evitá-los e desenvolver código mais robusto desde o início. Um erro comum é não considerar casos extremos ou situações inesperadas. Sempre teste seu código com diferentes tipos de entrada, incluindo valores nulos, vazios ou inválidos. Outro erro frequente é otimizar prematuramente. Primeiro faça o código funcionar corretamente, depois otimize se necessário. Como diz o ditado: 'premature optimization is the root of all evil'. Quando encontrar problemas, use o debugger para entender o que está acontecendo. Coloque breakpoints, inspecione variáveis e execute o código passo a passo. Esta é uma habilidade essencial para todo desenvolvedor. Mensagens de erro podem parecer intimidadoras no início, mas elas são suas aliadas. Leia-as cuidadosamente - elas geralmente indicam exatamente onde e qual é o problema. Se algo não funcionar como esperado, não desanime. Debugging é uma parte normal do desenvolvimento. Até desenvolvedores experientes passam tempo significativo depurando código. Testar seu código é tão importante quanto escrevê-lo. Testes automatizados garantem que seu código funciona corretamente e continuará funcionando quando você fizer mudanças. Comece escrevendo testes simples para os casos mais comuns. Depois adicione testes para casos extremos e situações de erro. Uma boa cobertura de testes dá confiança para refatorar e melhorar o código. Considere a prática de TDD (Test-Driven Development), onde você escreve os testes antes do código. Isso ajuda a pensar claramente sobre o que o código deve fazer. Embora a correção seja sempre a prioridade, é importante estar ciente das implicações de performance de suas escolhas. Diferentes abordagens podem ter custos computacionais muito diferentes. Em muitos casos, a diferença de performance é negligenciável para aplicações pequenas. No entanto, à medida que seus programas crescem, escolhas eficientes se tornam cada vez mais importantes. Aprenda a usar ferramentas de profiling para identificar gargalos de performance. Otimize baseado em dados reais, não em suposições sobre onde o código é lento. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0004-000000000006"),
            CourseId = _courseId,
            Title = "INNER JOIN - Relacionamentos",
            Duration = "60 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 60,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0004-000000000005" }),
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
                "Entender LEFT JOIN e RIGHT JOIN",
                "Usar FULL OUTER JOIN",
                "Identificar quando usar cada tipo de JOIN",
                "Trabalhar com valores NULL em JOINs"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "LEFT JOIN - Incluindo Registros Sem Correspondência",
                    Content = "LEFT JOIN (ou LEFT OUTER JOIN) retorna todas as linhas da tabela esquerda e as linhas correspondentes da tabela direita. Se não houver correspondência, as colunas da tabela direita terão valores NULL. Isso é útil quando você quer ver todos os registros de uma tabela, independentemente de terem relacionamentos. Por exemplo, listar todos os clientes, incluindo aqueles que nunca fizeram pedidos. LEFT JOIN Clientes com Pedidos mostrará todos os clientes, com NULL nos campos de pedido para clientes sem pedidos. Você pode usar IS NULL para filtrar registros sem correspondência: WHERE Pedidos.PedidoID IS NULL encontra clientes sem pedidos. LEFT JOIN é mais comum que RIGHT JOIN porque é mais intuitivo colocar a tabela principal à esquerda. Use LEFT JOIN quando a tabela esquerda é a entidade principal e você quer incluir registros mesmo sem relacionamentos. LEFT JOIN é essencial para relatórios que precisam mostrar ausência de dados, como produtos nunca vendidos ou clientes inativos.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "RIGHT JOIN e FULL OUTER JOIN",
                    Content = "RIGHT JOIN é o oposto de LEFT JOIN: retorna todas as linhas da tabela direita e correspondências da esquerda. RIGHT JOIN é menos usado porque você pode reescrever como LEFT JOIN invertendo a ordem das tabelas. FULL OUTER JOIN retorna todas as linhas de ambas as tabelas, com NULL onde não há correspondência. FULL OUTER JOIN é raro, usado quando você quer ver todos os registros de ambas as tabelas, independentemente de relacionamentos. Por exemplo, comparar duas listas de produtos de diferentes fornecedores, mostrando produtos exclusivos de cada um e produtos comuns. A escolha entre INNER, LEFT, RIGHT e FULL JOIN depende da pergunta que você quer responder. INNER JOIN: 'mostre apenas registros relacionados'. LEFT JOIN: 'mostre todos da esquerda, relacionados ou não'. RIGHT JOIN: 'mostre todos da direita, relacionados ou não'. FULL JOIN: 'mostre todos de ambas, relacionados ou não'. Entender essas diferenças é crucial para escrever consultas corretas.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Trabalhando com NULL em JOINs",
                    Content = "Quando não há correspondência em um OUTER JOIN, as colunas da tabela sem correspondência contêm NULL. Você pode usar IS NULL ou IS NOT NULL para filtrar esses casos. COALESCE() substitui NULL por um valor padrão: COALESCE(Pedidos.ValorTotal, 0) retorna 0 se ValorTotal for NULL. ISNULL() (SQL Server) ou IFNULL() (MySQL) fazem o mesmo. Ao fazer cálculos com colunas que podem ser NULL, use COALESCE para evitar resultados NULL. Por exemplo, SUM(COALESCE(Quantidade, 0)) garante que NULL seja tratado como 0. LEFT JOIN com IS NULL é uma técnica comum para encontrar registros órfãos: clientes sem pedidos, produtos sem vendas, etc. Isso é útil para limpeza de dados e análise. Ao usar múltiplos OUTER JOINs, a ordem importa e pode afetar quais registros são incluídos. Teste suas consultas cuidadosamente para garantir que retornam os dados esperados. NULL em SQL representa ausência de valor, não zero ou string vazia, então requer tratamento especial.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "LEFT JOIN - Clientes com e sem Pedidos",
                    Code = @"-- Todos os clientes, incluindo sem pedidos
SELECT 
    C.ClienteID,
    C.Nome,
    COUNT(P.PedidoID) AS TotalPedidos,
    COALESCE(SUM(P.ValorTotal), 0) AS ValorTotalGasto
FROM Clientes C
LEFT JOIN Pedidos P ON C.ClienteID = P.ClienteID
GROUP BY C.ClienteID, C.Nome
ORDER BY TotalPedidos DESC;

-- Encontrar clientes que nunca fizeram pedidos
SELECT C.ClienteID, C.Nome, C.Email
FROM Clientes C
LEFT JOIN Pedidos P ON C.ClienteID = P.ClienteID
WHERE P.PedidoID IS NULL;",
                    Language = "sql",
                    Explanation = "LEFT JOIN garante que todos os clientes apareçam. COUNT e SUM com COALESCE tratam NULL corretamente. A segunda consulta usa WHERE IS NULL para encontrar clientes sem pedidos.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "FULL OUTER JOIN - Comparação de Listas",
                    Code = @"-- Comparar produtos de dois fornecedores
SELECT 
    COALESCE(F1.ProdutoNome, F2.ProdutoNome) AS Produto,
    F1.Preco AS PrecoFornecedor1,
    F2.Preco AS PrecoFornecedor2,
    CASE 
        WHEN F1.ProdutoID IS NULL THEN 'Apenas Fornecedor 2'
        WHEN F2.ProdutoID IS NULL THEN 'Apenas Fornecedor 1'
        ELSE 'Ambos'
    END AS Disponibilidade
FROM Fornecedor1Produtos F1
FULL OUTER JOIN Fornecedor2Produtos F2 
    ON F1.ProdutoNome = F2.ProdutoNome
ORDER BY Produto;",
                    Language = "sql",
                    Explanation = "FULL OUTER JOIN mostra produtos de ambos os fornecedores. COALESCE pega o nome de qualquer fornecedor que tenha o produto. CASE identifica disponibilidade.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Produtos Nunca Vendidos",
                    Description = "Liste todos os produtos que nunca foram vendidos (não aparecem em PedidoItens). Mostre ProdutoID, Nome e Preco.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"SELECT 
    -- Complete aqui
FROM Produtos P
LEFT JOIN 
WHERE ;",
                    Hints = new List<string>
                    {
                        "LEFT JOIN PedidoItens PI ON P.ProdutoID = PI.ProdutoID",
                        "WHERE PI.ProdutoID IS NULL",
                        "Isso encontra produtos sem correspondência em PedidoItens"
                    }
                },
                new Exercise
                {
                    Title = "Relatório de Atividade de Clientes",
                    Description = "Liste todos os clientes com total de pedidos e valor gasto. Inclua clientes sem pedidos (mostrar 0). Ordene por valor gasto decrescente.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"SELECT 
    C.Nome,
    -- Complete aqui
FROM Clientes C
LEFT JOIN 
GROUP BY 
ORDER BY ;",
                    Hints = new List<string>
                    {
                        "Use COUNT(P.PedidoID) para contar pedidos",
                        "Use COALESCE(SUM(P.ValorTotal), 0) para valor total",
                        "GROUP BY C.ClienteID, C.Nome",
                        "ORDER BY ValorTotalGasto DESC"
                    }
                },
                new Exercise
                {
                    Title = "Análise de Categorias",
                    Description = "Mostre todas as categorias com quantidade de produtos e valor total em estoque. Inclua categorias sem produtos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"SELECT 
    Cat.Nome AS Categoria,
    -- Complete aqui
FROM Categorias Cat
LEFT JOIN 
GROUP BY 
ORDER BY ;",
                    Hints = new List<string>
                    {
                        "COUNT(P.ProdutoID) conta produtos",
                        "SUM(P.Preco * P.Estoque) calcula valor em estoque",
                        "Use COALESCE para tratar NULL",
                        "GROUP BY Cat.CategoriaID, Cat.Nome"
                    }
                }
            },
            Summary = "Nesta aula você aprendeu sobre LEFT JOIN, RIGHT JOIN e FULL OUTER JOIN. Você viu como incluir registros sem correspondência e trabalhar com valores NULL. Esses tipos de JOIN são essenciais para análises que precisam mostrar ausência de dados. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno. Grandes empresas como Microsoft, Google e Amazon aplicam estes conceitos em seus sistemas. Ao aprender estas técnicas, você está adquirindo conhecimento usado por desenvolvedores em todo o mundo. Considere como este conceito poderia ser aplicado em um projeto pessoal. Pensar em aplicações práticas ajuda a solidificar o entendimento e torna o aprendizado mais significativo. Iniciantes frequentemente cometem alguns erros ao aprender este tópico. Conhecer estes erros comuns ajuda a evitá-los e desenvolver código mais robusto desde o início. Um erro comum é não considerar casos extremos ou situações inesperadas. Sempre teste seu código com diferentes tipos de entrada, incluindo valores nulos, vazios ou inválidos. Outro erro frequente é otimizar prematuramente. Primeiro faça o código funcionar corretamente, depois otimize se necessário. Como diz o ditado: 'premature optimization is the root of all evil'. Quando encontrar problemas, use o debugger para entender o que está acontecendo. Coloque breakpoints, inspecione variáveis e execute o código passo a passo. Esta é uma habilidade essencial para todo desenvolvedor. Mensagens de erro podem parecer intimidadoras no início, mas elas são suas aliadas. Leia-as cuidadosamente - elas geralmente indicam exatamente onde e qual é o problema. Se algo não funcionar como esperado, não desanime. Debugging é uma parte normal do desenvolvimento. Até desenvolvedores experientes passam tempo significativo depurando código. Testar seu código é tão importante quanto escrevê-lo. Testes automatizados garantem que seu código funciona corretamente e continuará funcionando quando você fizer mudanças. Comece escrevendo testes simples para os casos mais comuns. Depois adicione testes para casos extremos e situações de erro. Uma boa cobertura de testes dá confiança para refatorar e melhorar o código. Considere a prática de TDD (Test-Driven Development), onde você escreve os testes antes do código. Isso ajuda a pensar claramente sobre o que o código deve fazer. Embora a correção seja sempre a prioridade, é importante estar ciente das implicações de performance de suas escolhas. Diferentes abordagens podem ter custos computacionais muito diferentes. Em muitos casos, a diferença de performance é negligenciável para aplicações pequenas. No entanto, à medida que seus programas crescem, escolhas eficientes se tornam cada vez mais importantes. Aprenda a usar ferramentas de profiling para identificar gargalos de performance. Otimize baseado em dados reais, não em suposições sobre onde o código é lento. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0004-000000000007"),
            CourseId = _courseId,
            Title = "LEFT, RIGHT e FULL OUTER JOIN",
            Duration = "55 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 55,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0004-000000000006" }),
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
                "Compreender o que são índices e por que são importantes",
                "Criar índices para melhorar performance",
                "Identificar quando usar índices",
                "Entender o impacto de índices em INSERT/UPDATE/DELETE"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "O que são Índices?",
                    Content = "Índices são estruturas de dados que melhoram a velocidade de recuperação de dados em tabelas. Pense em um índice de banco de dados como o índice de um livro: em vez de ler o livro inteiro para encontrar um tópico, você consulta o índice que aponta diretamente para a página correta. Sem índices, o banco de dados precisa fazer um table scan, lendo cada linha para encontrar os dados desejados. Com índices, o banco pode localizar dados rapidamente, especialmente em tabelas grandes. Índices são criados em colunas frequentemente usadas em cláusulas WHERE, JOIN, e ORDER BY. Chaves primárias automaticamente têm índices. Índices usam estruturas como B-trees que permitem busca logarítmica, muito mais rápida que busca linear. Por exemplo, em uma tabela com 1 milhão de registros, um table scan pode examinar 1 milhão de linhas, enquanto um índice pode encontrar o registro em cerca de 20 comparações. Índices são essenciais para performance em aplicações com grandes volumes de dados. Sem índices apropriados, consultas podem levar segundos ou minutos em vez de milissegundos.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Tipos de Índices e Quando Usar",
                    Content = "Existem vários tipos de índices. Índices clustered determinam a ordem física dos dados na tabela. Cada tabela pode ter apenas um índice clustered, geralmente na chave primária. Índices non-clustered são estruturas separadas que apontam para os dados. Uma tabela pode ter múltiplos índices non-clustered. Índices únicos garantem que não haja valores duplicados. Índices compostos incluem múltiplas colunas, úteis quando consultas filtram por várias colunas simultaneamente. A ordem das colunas em índices compostos importa: coloque colunas mais seletivas primeiro. Crie índices em colunas usadas frequentemente em WHERE, JOIN, e ORDER BY. Colunas com alta cardinalidade (muitos valores únicos) beneficiam mais de índices. Evite índices em colunas raramente consultadas ou com baixa cardinalidade. Índices têm custo: ocupam espaço em disco e tornam INSERT, UPDATE e DELETE mais lentos porque o índice precisa ser atualizado. O equilíbrio é criar índices suficientes para consultas rápidas sem sobrecarregar operações de escrita.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Criando e Gerenciando Índices",
                    Content = "CREATE INDEX cria um novo índice: CREATE INDEX IX_Produtos_Nome ON Produtos(Nome). Prefixos como IX_ indicam índices. CREATE UNIQUE INDEX cria índice único. Para índices compostos, liste múltiplas colunas: CREATE INDEX IX_Produtos_Categoria_Preco ON Produtos(Categoria, Preco). DROP INDEX remove índices. ALTER INDEX REBUILD reconstrói índices fragmentados, melhorando performance. Índices fragmentam com o tempo devido a INSERT/UPDATE/DELETE. Manutenção regular de índices é importante em ambientes de produção. Use INCLUDE para adicionar colunas não-chave ao índice, criando covering indexes que contêm todos os dados necessários para uma consulta, eliminando a necessidade de acessar a tabela. Filtered indexes incluem apenas linhas que atendem a uma condição, úteis para consultas que sempre filtram da mesma forma. Monitore o uso de índices com DMVs (Dynamic Management Views) para identificar índices não utilizados que podem ser removidos. Ferramentas como Database Engine Tuning Advisor sugerem índices baseados em workload real.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Criando Índices",
                    Code = @"-- Índice simples em coluna frequentemente consultada
CREATE INDEX IX_Clientes_Email 
ON Clientes(Email);

-- Índice único para garantir emails únicos
CREATE UNIQUE INDEX UX_Clientes_Email 
ON Clientes(Email);

-- Índice composto para consultas que filtram por categoria e preço
CREATE INDEX IX_Produtos_Categoria_Preco 
ON Produtos(Categoria, Preco);

-- Covering index com colunas incluídas
CREATE INDEX IX_Produtos_Categoria_INCLUDE 
ON Produtos(Categoria) 
INCLUDE (Nome, Preco, Estoque);

-- Índice filtrado para produtos ativos
CREATE INDEX IX_Produtos_Ativos 
ON Produtos(Categoria, Preco) 
WHERE Ativo = 1;",
                    Language = "sql",
                    Explanation = "Exemplos de diferentes tipos de índices. Índices simples, únicos, compostos, covering e filtrados. Cada um otimiza diferentes padrões de consulta.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Impacto de Índices em Consultas",
                    Code = @"-- Sem índice: table scan (lento em tabelas grandes)
SELECT * FROM Clientes WHERE Email = 'joao@email.com';
-- Tempo: ~500ms em 1M registros

-- Com índice em Email: index seek (rápido)
CREATE INDEX IX_Clientes_Email ON Clientes(Email);
SELECT * FROM Clientes WHERE Email = 'joao@email.com';
-- Tempo: ~5ms em 1M registros

-- Consulta que beneficia de índice composto
SELECT Nome, Preco FROM Produtos 
WHERE Categoria = 'Eletrônicos' AND Preco > 1000
ORDER BY Preco;
-- Índice ideal: IX_Produtos_Categoria_Preco

-- Verificar uso de índices
SELECT * FROM sys.dm_db_index_usage_stats 
WHERE database_id = DB_ID();",
                    Language = "sql",
                    Explanation = "Demonstra o impacto dramático de índices na performance. A mesma consulta pode ser 100x mais rápida com índice apropriado. A última consulta mostra como monitorar uso de índices.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Criar Índice Simples",
                    Description = "Crie um índice na coluna DataCadastro da tabela Clientes para acelerar consultas que filtram por data.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"CREATE INDEX 
ON ;",
                    Hints = new List<string>
                    {
                        "Use um nome descritivo como IX_Clientes_DataCadastro",
                        "Sintaxe: CREATE INDEX nome ON tabela(coluna)",
                        "Índices em datas são úteis para consultas de intervalo"
                    }
                },
                new Exercise
                {
                    Title = "Índice Composto",
                    Description = "Crie um índice composto em Produtos para otimizar consultas que filtram por Categoria e ordenam por Preco.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"CREATE INDEX 
ON Produtos();",
                    Hints = new List<string>
                    {
                        "Liste múltiplas colunas: (Categoria, Preco)",
                        "A ordem importa: coluna de filtro primeiro",
                        "Nome sugerido: IX_Produtos_Categoria_Preco"
                    }
                },
                new Exercise
                {
                    Title = "Covering Index",
                    Description = "Crie um covering index em Pedidos(ClienteID) que inclua DataPedido e ValorTotal, otimizando consultas de histórico de clientes.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"CREATE INDEX 
ON Pedidos()
INCLUDE ();",
                    Hints = new List<string>
                    {
                        "Coluna principal: ClienteID",
                        "INCLUDE (DataPedido, ValorTotal)",
                        "Covering indexes eliminam acesso à tabela"
                    }
                }
            },
            Summary = "Nesta aula você aprendeu o que são índices, por que são cruciais para performance, e como criar diferentes tipos de índices. Você viu que índices aceleram consultas mas têm custo em operações de escrita. Escolher índices apropriados é essencial para aplicações performáticas. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno. Grandes empresas como Microsoft, Google e Amazon aplicam estes conceitos em seus sistemas. Ao aprender estas técnicas, você está adquirindo conhecimento usado por desenvolvedores em todo o mundo. Considere como este conceito poderia ser aplicado em um projeto pessoal. Pensar em aplicações práticas ajuda a solidificar o entendimento e torna o aprendizado mais significativo. Iniciantes frequentemente cometem alguns erros ao aprender este tópico. Conhecer estes erros comuns ajuda a evitá-los e desenvolver código mais robusto desde o início. Um erro comum é não considerar casos extremos ou situações inesperadas. Sempre teste seu código com diferentes tipos de entrada, incluindo valores nulos, vazios ou inválidos. Outro erro frequente é otimizar prematuramente. Primeiro faça o código funcionar corretamente, depois otimize se necessário. Como diz o ditado: 'premature optimization is the root of all evil'. Quando encontrar problemas, use o debugger para entender o que está acontecendo. Coloque breakpoints, inspecione variáveis e execute o código passo a passo. Esta é uma habilidade essencial para todo desenvolvedor. Mensagens de erro podem parecer intimidadoras no início, mas elas são suas aliadas. Leia-as cuidadosamente - elas geralmente indicam exatamente onde e qual é o problema. Se algo não funcionar como esperado, não desanime. Debugging é uma parte normal do desenvolvimento. Até desenvolvedores experientes passam tempo significativo depurando código. Testar seu código é tão importante quanto escrevê-lo. Testes automatizados garantem que seu código funciona corretamente e continuará funcionando quando você fizer mudanças. Comece escrevendo testes simples para os casos mais comuns. Depois adicione testes para casos extremos e situações de erro. Uma boa cobertura de testes dá confiança para refatorar e melhorar o código. Considere a prática de TDD (Test-Driven Development), onde você escreve os testes antes do código. Isso ajuda a pensar claramente sobre o que o código deve fazer. Embora a correção seja sempre a prioridade, é importante estar ciente das implicações de performance de suas escolhas. Diferentes abordagens podem ter custos computacionais muito diferentes. Em muitos casos, a diferença de performance é negligenciável para aplicações pequenas. No entanto, à medida que seus programas crescem, escolhas eficientes se tornam cada vez mais importantes. Aprenda a usar ferramentas de profiling para identificar gargalos de performance. Otimize baseado em dados reais, não em suposições sobre onde o código é lento. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0004-000000000008"),
            CourseId = _courseId,
            Title = "Índices e Performance",
            Duration = "60 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 60,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0004-000000000007" }),
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
                "Compreender o conceito de transações",
                "Aprender as propriedades ACID",
                "Usar BEGIN TRANSACTION, COMMIT e ROLLBACK",
                "Implementar transações em cenários reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "O que são Transações?",
                    Content = "Uma transação é uma unidade lógica de trabalho que agrupa múltiplas operações de banco de dados em uma única operação atômica. Todas as operações na transação devem ser concluídas com sucesso, ou nenhuma delas é aplicada. Imagine transferir dinheiro entre contas bancárias: você precisa debitar de uma conta e creditar em outra. Se o débito funciona mas o crédito falha, você perde dinheiro. Transações garantem que ambas as operações aconteçam ou nenhuma aconteça. BEGIN TRANSACTION inicia uma transação. COMMIT confirma as mudanças permanentemente. ROLLBACK desfaz todas as mudanças desde o BEGIN. Transações são essenciais para manter consistência de dados em operações complexas. Sem transações, falhas parciais podem deixar o banco de dados em estado inconsistente. Transações também fornecem isolamento, garantindo que operações concorrentes não interfiram umas com as outras. Em aplicações multi-usuário, transações previnem condições de corrida e garantem que cada usuário veja uma visão consistente dos dados. Transações são fundamentais para qualquer aplicação que requer integridade de dados.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Propriedades ACID",
                    Content = "ACID é um acrônimo que define as propriedades que garantem confiabilidade de transações. Atomicidade: transações são tudo-ou-nada. Ou todas as operações são concluídas, ou nenhuma é. Não há estados parciais. Consistência: transações levam o banco de dados de um estado válido para outro estado válido, respeitando todas as restrições e regras. Isolamento: transações concorrentes não interferem umas com as outras. Cada transação vê o banco como se fosse a única executando. Durabilidade: uma vez que uma transação é confirmada com COMMIT, as mudanças são permanentes, mesmo se o sistema falhar imediatamente após. Essas propriedades são implementadas pelo sistema de gerenciamento de banco de dados através de logs de transação, locks e outros mecanismos. ACID é o que diferencia bancos de dados relacionais de sistemas de armazenamento mais simples. Aplicações críticas como sistemas bancários, e-commerce e sistemas de saúde dependem de ACID para garantir integridade de dados. Entender ACID ajuda a projetar aplicações robustas que lidam corretamente com falhas e concorrência.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Implementando Transações em SQL",
                    Content = "Para usar transações em SQL, comece com BEGIN TRANSACTION (ou BEGIN TRAN). Execute suas operações DML (INSERT, UPDATE, DELETE). Se tudo funcionar, execute COMMIT para confirmar as mudanças. Se algo der errado, execute ROLLBACK para desfazer tudo. Use TRY...CATCH para capturar erros e fazer ROLLBACK automaticamente. Transações implícitas acontecem automaticamente para cada comando individual. Transações explícitas você controla com BEGIN/COMMIT/ROLLBACK. Transações longas podem causar bloqueios, impedindo outros usuários de acessar dados. Mantenha transações curtas e focadas. SAVEPOINT permite criar pontos de salvamento dentro de uma transação, permitindo ROLLBACK parcial. Níveis de isolamento (READ UNCOMMITTED, READ COMMITTED, REPEATABLE READ, SERIALIZABLE) controlam como transações concorrentes interagem. Níveis mais altos fornecem mais isolamento mas podem reduzir concorrência. Em aplicações, use transações para operações que modificam múltiplas tabelas relacionadas, garantindo que todas as mudanças sejam aplicadas atomicamente.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Transação Básica",
                    Code = @"-- Transferência bancária com transação
BEGIN TRANSACTION;

    -- Debitar da conta origem
    UPDATE Contas 
    SET Saldo = Saldo - 1000 
    WHERE ContaID = 1;

    -- Creditar na conta destino
    UPDATE Contas 
    SET Saldo = Saldo + 1000 
    WHERE ContaID = 2;

    -- Registrar a transferência
    INSERT INTO Transferencias (ContaOrigem, ContaDestino, Valor, Data)
    VALUES (1, 2, 1000, GETDATE());

COMMIT TRANSACTION;
-- Todas as operações são confirmadas juntas",
                    Language = "sql",
                    Explanation = "Esta transação garante que o débito, crédito e registro aconteçam atomicamente. Se qualquer operação falhar, ROLLBACK desfaz tudo, mantendo consistência.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Transação com Tratamento de Erros",
                    Code = @"BEGIN TRY
    BEGIN TRANSACTION;

        -- Criar pedido
        INSERT INTO Pedidos (ClienteID, DataPedido, ValorTotal)
        VALUES (1, GETDATE(), 500.00);

        DECLARE @PedidoID INT = SCOPE_IDENTITY();

        -- Adicionar itens do pedido
        INSERT INTO PedidoItens (PedidoID, ProdutoID, Quantidade, PrecoUnitario)
        VALUES (@PedidoID, 10, 2, 250.00);

        -- Atualizar estoque
        UPDATE Produtos 
        SET Estoque = Estoque - 2 
        WHERE ProdutoID = 10;

    COMMIT TRANSACTION;
    PRINT 'Pedido criado com sucesso';
END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION;
    PRINT 'Erro: ' + ERROR_MESSAGE();
END CATCH;",
                    Language = "sql",
                    Explanation = "TRY...CATCH captura erros automaticamente. Se qualquer operação falhar, ROLLBACK é executado no bloco CATCH, desfazendo todas as mudanças. Isso garante atomicidade mesmo com erros.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Transação Simples",
                    Description = "Crie uma transação que insira um novo cliente e seu primeiro pedido. Use COMMIT para confirmar.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"BEGIN TRANSACTION;

    -- Inserir cliente
    INSERT INTO Clientes (Nome, Email)
    VALUES ('Ana Silva', 'ana@email.com');

    -- Inserir pedido
    -- Complete aqui

COMMIT TRANSACTION;",
                    Hints = new List<string>
                    {
                        "Use SCOPE_IDENTITY() para obter o ClienteID inserido",
                        "INSERT INTO Pedidos com o ClienteID obtido",
                        "Todas as operações devem estar entre BEGIN e COMMIT"
                    }
                },
                new Exercise
                {
                    Title = "Transação com Rollback",
                    Description = "Implemente uma transação que atualiza o estoque de um produto e registra a movimentação. Se o estoque ficar negativo, faça ROLLBACK.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"BEGIN TRY
    BEGIN TRANSACTION;

        -- Atualizar estoque
        -- Registrar movimentação
        -- Verificar se estoque é válido

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    -- Complete aqui
END CATCH;",
                    Hints = new List<string>
                    {
                        "UPDATE Produtos SET Estoque = Estoque - Quantidade",
                        "Use IF para verificar estoque negativo",
                        "ROLLBACK TRANSACTION no CATCH",
                        "RAISERROR para forçar erro se estoque negativo"
                    }
                },
                new Exercise
                {
                    Title = "Transação Complexa",
                    Description = "Crie uma transação para processar um pedido: criar pedido, adicionar itens, atualizar estoque de múltiplos produtos, e calcular total. Use tratamento de erros.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"BEGIN TRY
    BEGIN TRANSACTION;

        -- Complete aqui

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION;
    THROW;
END CATCH;",
                    Hints = new List<string>
                    {
                        "Insira em Pedidos primeiro, obtenha PedidoID",
                        "Insira múltiplos itens em PedidoItens",
                        "Atualize estoque para cada produto",
                        "Calcule e atualize ValorTotal do pedido"
                    }
                }
            },
            Summary = "Nesta aula você aprendeu sobre transações e propriedades ACID. Você viu como usar BEGIN TRANSACTION, COMMIT e ROLLBACK para garantir atomicidade de operações. Transações são essenciais para manter integridade de dados em operações complexas. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno. Grandes empresas como Microsoft, Google e Amazon aplicam estes conceitos em seus sistemas. Ao aprender estas técnicas, você está adquirindo conhecimento usado por desenvolvedores em todo o mundo. Considere como este conceito poderia ser aplicado em um projeto pessoal. Pensar em aplicações práticas ajuda a solidificar o entendimento e torna o aprendizado mais significativo. Iniciantes frequentemente cometem alguns erros ao aprender este tópico. Conhecer estes erros comuns ajuda a evitá-los e desenvolver código mais robusto desde o início. Um erro comum é não considerar casos extremos ou situações inesperadas. Sempre teste seu código com diferentes tipos de entrada, incluindo valores nulos, vazios ou inválidos. Outro erro frequente é otimizar prematuramente. Primeiro faça o código funcionar corretamente, depois otimize se necessário. Como diz o ditado: 'premature optimization is the root of all evil'. Quando encontrar problemas, use o debugger para entender o que está acontecendo. Coloque breakpoints, inspecione variáveis e execute o código passo a passo. Esta é uma habilidade essencial para todo desenvolvedor. Mensagens de erro podem parecer intimidadoras no início, mas elas são suas aliadas. Leia-as cuidadosamente - elas geralmente indicam exatamente onde e qual é o problema. Se algo não funcionar como esperado, não desanime. Debugging é uma parte normal do desenvolvimento. Até desenvolvedores experientes passam tempo significativo depurando código. Testar seu código é tão importante quanto escrevê-lo. Testes automatizados garantem que seu código funciona corretamente e continuará funcionando quando você fizer mudanças. Comece escrevendo testes simples para os casos mais comuns. Depois adicione testes para casos extremos e situações de erro. Uma boa cobertura de testes dá confiança para refatorar e melhorar o código. Considere a prática de TDD (Test-Driven Development), onde você escreve os testes antes do código. Isso ajuda a pensar claramente sobre o que o código deve fazer. Embora a correção seja sempre a prioridade, é importante estar ciente das implicações de performance de suas escolhas. Diferentes abordagens podem ter custos computacionais muito diferentes. Em muitos casos, a diferença de performance é negligenciável para aplicações pequenas. No entanto, à medida que seus programas crescem, escolhas eficientes se tornam cada vez mais importantes. Aprenda a usar ferramentas de profiling para identificar gargalos de performance. Otimize baseado em dados reais, não em suposições sobre onde o código é lento. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0004-000000000009"),
            CourseId = _courseId,
            Title = "Transações e ACID",
            Duration = "60 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 60,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0004-000000000008" }),
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
                "Entender o conceito de normalização de dados",
                "Conhecer as formas normais (1NF, 2NF, 3NF)",
                "Identificar problemas de dados desnormalizados",
                "Aplicar normalização em design de banco de dados"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "O que é Normalização?",
                    Content = "Normalização é o processo de organizar dados em um banco de dados para reduzir redundância e melhorar integridade. Dados desnormalizados contêm informações repetidas em múltiplos lugares, causando problemas de atualização, inserção e exclusão. Por exemplo, se você armazena o endereço do cliente em cada pedido, quando o cliente muda de endereço, você precisa atualizar todos os pedidos. Normalização divide dados em tabelas relacionadas, armazenando cada informação em um único lugar. Isso elimina redundância e garante consistência. A normalização segue regras chamadas formas normais, cada uma resolvendo tipos específicos de problemas. Primeira Forma Normal (1NF) elimina grupos repetidos. Segunda Forma Normal (2NF) elimina dependências parciais. Terceira Forma Normal (3NF) elimina dependências transitivas. Bancos de dados bem normalizados são mais fáceis de manter, têm menos erros de dados e ocupam menos espaço. No entanto, normalização excessiva pode impactar performance devido a múltiplos JOINs. O equilíbrio entre normalização e performance depende dos requisitos da aplicação. Em sistemas OLTP (transacionais), normalização é preferida. Em sistemas OLAP (analíticos), desnormalização controlada pode melhorar performance de consultas.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Formas Normais",
                    Content = "Primeira Forma Normal (1NF) requer que cada coluna contenha valores atômicos (indivisíveis) e que não haja grupos repetidos. Por exemplo, uma coluna Telefones contendo '11999999999, 11988888888' viola 1NF. A solução é criar uma tabela separada ClienteTelefones. Segunda Forma Normal (2NF) requer 1NF e que todos os atributos não-chave dependam completamente da chave primária inteira, não apenas de parte dela. Isso se aplica a chaves primárias compostas. Por exemplo, em PedidoItens(PedidoID, ProdutoID, NomeProduto, Preco), NomeProduto depende apenas de ProdutoID, não da chave completa. A solução é mover NomeProduto para a tabela Produtos. Terceira Forma Normal (3NF) requer 2NF e que não haja dependências transitivas. Um atributo não-chave não deve depender de outro atributo não-chave. Por exemplo, em Pedidos(PedidoID, ClienteID, NomeCliente, CidadeCliente), CidadeCliente depende de ClienteID, não de PedidoID. A solução é mover dados do cliente para a tabela Clientes. Existem formas normais mais avançadas (BCNF, 4NF, 5NF), mas 3NF é suficiente para a maioria das aplicações. Normalizar até 3NF elimina a maioria dos problemas de redundância e anomalias de atualização.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Desnormalização Estratégica",
                    Content = "Embora normalização seja importante, às vezes desnormalização controlada melhora performance. Desnormalização adiciona redundância intencional para evitar JOINs custosos. Por exemplo, armazenar ValorTotal em Pedidos em vez de calcular sempre a partir de PedidoItens. Isso duplica dados mas acelera consultas. Use desnormalização quando: consultas são muito mais frequentes que atualizações, JOINs impactam significativamente a performance, ou você precisa de dados históricos que não devem mudar. Ao desnormalizar, implemente mecanismos para manter consistência. Triggers podem atualizar dados redundantes automaticamente. Stored procedures podem encapsular lógica de atualização. Caching pode reduzir necessidade de desnormalização. Tabelas de relatório ou data warehouses frequentemente são desnormalizadas para otimizar consultas analíticas. Em aplicações modernas, padrões como CQRS (Command Query Responsibility Segregation) usam modelos normalizados para escrita e desnormalizados para leitura. A chave é desnormalizar conscientemente, documentando decisões e mantendo consistência. Nunca desnormalize por preguiça ou falta de entendimento de normalização. Sempre normalize primeiro, depois desnormalize apenas onde necessário baseado em métricas de performance reais.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Problema de Desnormalização",
                    Code = @"-- Tabela desnormalizada (problemática)
CREATE TABLE PedidosDesnormalizado (
    PedidoID INT PRIMARY KEY,
    ClienteNome NVARCHAR(100),
    ClienteEmail NVARCHAR(100),
    ClienteEndereco NVARCHAR(200),
    ProdutoNome NVARCHAR(100),
    ProdutoPreco DECIMAL(10,2),
    Quantidade INT
);
-- Problemas: 
-- 1. Dados do cliente repetidos em cada pedido
-- 2. Múltiplos produtos requerem múltiplas linhas
-- 3. Atualizar email do cliente requer atualizar todos os pedidos",
                    Language = "sql",
                    Explanation = "Esta estrutura desnormalizada causa redundância massiva. Dados do cliente são duplicados em cada pedido. Mudanças no cliente requerem atualizar múltiplos registros, arriscando inconsistência.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Solução Normalizada (3NF)",
                    Code = @"-- Tabelas normalizadas
CREATE TABLE Clientes (
    ClienteID INT PRIMARY KEY IDENTITY,
    Nome NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) UNIQUE NOT NULL,
    Endereco NVARCHAR(200)
);

CREATE TABLE Produtos (
    ProdutoID INT PRIMARY KEY IDENTITY,
    Nome NVARCHAR(100) NOT NULL,
    Preco DECIMAL(10,2) NOT NULL
);

CREATE TABLE Pedidos (
    PedidoID INT PRIMARY KEY IDENTITY,
    ClienteID INT NOT NULL,
    DataPedido DATETIME NOT NULL,
    FOREIGN KEY (ClienteID) REFERENCES Clientes(ClienteID)
);

CREATE TABLE PedidoItens (
    PedidoItemID INT PRIMARY KEY IDENTITY,
    PedidoID INT NOT NULL,
    ProdutoID INT NOT NULL,
    Quantidade INT NOT NULL,
    PrecoUnitario DECIMAL(10,2) NOT NULL,
    FOREIGN KEY (PedidoID) REFERENCES Pedidos(PedidoID),
    FOREIGN KEY (ProdutoID) REFERENCES Produtos(ProdutoID)
);
-- Benefícios:
-- 1. Cada informação armazenada uma vez
-- 2. Atualizar cliente afeta apenas tabela Clientes
-- 3. Integridade garantida por chaves estrangeiras",
                    Language = "sql",
                    Explanation = "Estrutura normalizada elimina redundância. Dados do cliente, produto e pedido estão em tabelas separadas. Relacionamentos são mantidos através de chaves estrangeiras. Mudanças são localizadas e consistentes.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Identificar Violações de 1NF",
                    Description = "Analise esta tabela e identifique violações de 1NF: Funcionarios(ID, Nome, Telefones, Habilidades). Telefones contém '11999999999, 11988888888' e Habilidades contém 'C#, SQL, JavaScript'.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"-- Problema: colunas com múltiplos valores
-- Solução: criar tabelas separadas
-- Complete o design normalizado",
                    Hints = new List<string>
                    {
                        "Crie tabela FuncionarioTelefones(FuncionarioID, Telefone)",
                        "Crie tabela FuncionarioHabilidades(FuncionarioID, Habilidade)",
                        "Cada valor deve estar em sua própria linha"
                    }
                },
                new Exercise
                {
                    Title = "Normalizar para 3NF",
                    Description = "Normalize esta tabela para 3NF: Pedidos(PedidoID, ClienteID, NomeCliente, EmailCliente, CidadeCliente, EstadoCliente, ProdutoID, NomeProduto, PrecoProduto, Quantidade).",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"-- Identifique as dependências
-- Crie tabelas normalizadas
-- Defina chaves primárias e estrangeiras",
                    Hints = new List<string>
                    {
                        "Dados do cliente dependem de ClienteID",
                        "Dados do produto dependem de ProdutoID",
                        "Crie tabelas: Clientes, Produtos, Pedidos, PedidoItens",
                        "Use chaves estrangeiras para relacionamentos"
                    }
                },
                new Exercise
                {
                    Title = "Desnormalização Justificada",
                    Description = "Você tem Pedidos e PedidoItens normalizados. Justifique adicionar ValorTotal em Pedidos (desnormalização) e implemente um trigger para mantê-lo atualizado.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"-- Adicionar coluna
ALTER TABLE Pedidos ADD ValorTotal DECIMAL(10,2);

-- Criar trigger para manter consistência
CREATE TRIGGER trg_AtualizarValorTotal
-- Complete aqui",
                    Hints = new List<string>
                    {
                        "Justificativa: evitar SUM em cada consulta",
                        "Trigger em PedidoItens após INSERT/UPDATE/DELETE",
                        "Calcular SUM(Quantidade * PrecoUnitario)",
                        "UPDATE Pedidos SET ValorTotal = ..."
                    }
                }
            },
            Summary = "Nesta aula você aprendeu sobre normalização de dados e as formas normais 1NF, 2NF e 3NF. Você viu como normalização elimina redundância e melhora integridade, e quando desnormalização estratégica pode ser benéfica. Normalização é fundamental para design de banco de dados robusto. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno. Grandes empresas como Microsoft, Google e Amazon aplicam estes conceitos em seus sistemas. Ao aprender estas técnicas, você está adquirindo conhecimento usado por desenvolvedores em todo o mundo. Considere como este conceito poderia ser aplicado em um projeto pessoal. Pensar em aplicações práticas ajuda a solidificar o entendimento e torna o aprendizado mais significativo. Iniciantes frequentemente cometem alguns erros ao aprender este tópico. Conhecer estes erros comuns ajuda a evitá-los e desenvolver código mais robusto desde o início. Um erro comum é não considerar casos extremos ou situações inesperadas. Sempre teste seu código com diferentes tipos de entrada, incluindo valores nulos, vazios ou inválidos. Outro erro frequente é otimizar prematuramente. Primeiro faça o código funcionar corretamente, depois otimize se necessário. Como diz o ditado: 'premature optimization is the root of all evil'. Quando encontrar problemas, use o debugger para entender o que está acontecendo. Coloque breakpoints, inspecione variáveis e execute o código passo a passo. Esta é uma habilidade essencial para todo desenvolvedor. Mensagens de erro podem parecer intimidadoras no início, mas elas são suas aliadas. Leia-as cuidadosamente - elas geralmente indicam exatamente onde e qual é o problema. Se algo não funcionar como esperado, não desanime. Debugging é uma parte normal do desenvolvimento. Até desenvolvedores experientes passam tempo significativo depurando código. Testar seu código é tão importante quanto escrevê-lo. Testes automatizados garantem que seu código funciona corretamente e continuará funcionando quando você fizer mudanças. Comece escrevendo testes simples para os casos mais comuns. Depois adicione testes para casos extremos e situações de erro. Uma boa cobertura de testes dá confiança para refatorar e melhorar o código. Considere a prática de TDD (Test-Driven Development), onde você escreve os testes antes do código. Isso ajuda a pensar claramente sobre o que o código deve fazer. Embora a correção seja sempre a prioridade, é importante estar ciente das implicações de performance de suas escolhas. Diferentes abordagens podem ter custos computacionais muito diferentes. Em muitos casos, a diferença de performance é negligenciável para aplicações pequenas. No entanto, à medida que seus programas crescem, escolhas eficientes se tornam cada vez mais importantes. Aprenda a usar ferramentas de profiling para identificar gargalos de performance. Otimize baseado em dados reais, não em suposições sobre onde o código é lento. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0004-00000000000a"),
            CourseId = _courseId,
            Title = "Normalização de Dados",
            Duration = "60 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 60,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0004-000000000009" }),
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
                "Compreender o que são stored procedures",
                "Criar e executar stored procedures",
                "Usar parâmetros de entrada e saída",
                "Entender vantagens e desvantagens de stored procedures"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "O que são Stored Procedures?",
                    Content = "Stored procedures são blocos de código SQL armazenados no banco de dados que podem ser executados repetidamente. Pense neles como funções ou métodos em programação, mas executados no servidor de banco de dados. Stored procedures encapsulam lógica de negócio complexa, permitindo reutilização e manutenção centralizada. Em vez de enviar múltiplos comandos SQL da aplicação, você chama um stored procedure que executa toda a lógica no servidor. Isso reduz tráfego de rede e melhora performance. Stored procedures podem aceitar parâmetros de entrada, executar lógica condicional, loops, tratamento de erros, e retornar resultados ou parâmetros de saída. Eles são compilados e otimizados pelo banco de dados, geralmente executando mais rápido que SQL ad-hoc. Stored procedures também melhoram segurança: você pode dar permissão para executar procedures sem dar acesso direto às tabelas. Isso previne SQL injection e limita o que usuários podem fazer. Em aplicações empresariais, stored procedures são comuns para operações críticas como processamento de pedidos, cálculos financeiros e relatórios complexos. Eles centralizam lógica de negócio no banco de dados, garantindo que todas as aplicações usem as mesmas regras.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Criando e Executando Stored Procedures",
                    Content = "Para criar um stored procedure, use CREATE PROCEDURE seguido do nome e corpo. O corpo contém comandos SQL e lógica de controle. Parâmetros são declarados após o nome: @NomeParametro TipoDado. Parâmetros de entrada recebem valores quando o procedure é chamado. Parâmetros de saída (OUTPUT) retornam valores para o chamador. Para executar um stored procedure, use EXEC ou EXECUTE seguido do nome e valores dos parâmetros. Stored procedures podem retornar conjuntos de resultados com SELECT, valores escalares com RETURN, ou parâmetros de saída. Você pode usar variáveis, IF/ELSE, WHILE, TRY/CATCH e outras estruturas de controle dentro de procedures. ALTER PROCEDURE modifica procedures existentes. DROP PROCEDURE remove procedures. Stored procedures podem chamar outros procedures, criando hierarquias de lógica. Eles podem usar transações para garantir atomicidade de operações complexas. Ao projetar stored procedures, mantenha-os focados em uma responsabilidade específica. Procedures muito grandes e complexos são difíceis de manter. Use nomes descritivos que indicam o que o procedure faz, como sp_CriarPedido ou sp_CalcularTotalVendas.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Vantagens e Desvantagens",
                    Content = "Vantagens de stored procedures incluem: performance melhorada através de compilação e planos de execução cacheados, redução de tráfego de rede enviando apenas chamadas de procedure, segurança aprimorada através de permissões granulares, reutilização de código evitando duplicação, e manutenção centralizada onde mudanças em um procedure afetam todas as aplicações. Stored procedures também facilitam migração de lógica de negócio sem alterar código da aplicação. Desvantagens incluem: acoplamento ao banco de dados específico (procedures SQL Server não funcionam em PostgreSQL), dificuldade de teste e debug comparado a código de aplicação, versionamento e controle de código-fonte mais complexo, e potencial de lógica de negócio fragmentada entre aplicação e banco de dados. Em arquiteturas modernas, há debate sobre onde colocar lógica de negócio. Alguns preferem manter lógica na aplicação para flexibilidade e testabilidade. Outros usam procedures para operações críticas de performance. A melhor abordagem depende dos requisitos. Para operações simples de CRUD, ORMs como Entity Framework podem ser suficientes. Para lógica complexa de dados, procedures podem ser mais eficientes. O importante é ser consistente e documentar decisões arquiteturais.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Stored Procedure Básico",
                    Code = @"-- Criar procedure para buscar clientes por cidade
CREATE PROCEDURE sp_BuscarClientesPorCidade
    @Cidade NVARCHAR(100)
AS
BEGIN
    SELECT ClienteID, Nome, Email, Telefone
    FROM Clientes
    WHERE Cidade = @Cidade
    ORDER BY Nome;
END;

-- Executar o procedure
EXEC sp_BuscarClientesPorCidade @Cidade = 'São Paulo';

-- Ou simplesmente
EXEC sp_BuscarClientesPorCidade 'São Paulo';",
                    Language = "sql",
                    Explanation = "Este procedure aceita um parâmetro de entrada (Cidade) e retorna clientes filtrados. EXEC executa o procedure passando o valor do parâmetro. O procedure encapsula a lógica de consulta.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Procedure com Parâmetros de Saída e Transação",
                    Code = @"-- Procedure para criar pedido e retornar ID
CREATE PROCEDURE sp_CriarPedido
    @ClienteID INT,
    @ProdutoID INT,
    @Quantidade INT,
    @PedidoID INT OUTPUT
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;

        -- Criar pedido
        INSERT INTO Pedidos (ClienteID, DataPedido, Status)
        VALUES (@ClienteID, GETDATE(), 'Pendente');

        SET @PedidoID = SCOPE_IDENTITY();

        -- Adicionar item
        DECLARE @Preco DECIMAL(10,2);
        SELECT @Preco = Preco FROM Produtos WHERE ProdutoID = @ProdutoID;

        INSERT INTO PedidoItens (PedidoID, ProdutoID, Quantidade, PrecoUnitario)
        VALUES (@PedidoID, @ProdutoID, @Quantidade, @Preco);

        -- Atualizar estoque
        UPDATE Produtos 
        SET Estoque = Estoque - @Quantidade 
        WHERE ProdutoID = @ProdutoID;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;

-- Executar com parâmetro de saída
DECLARE @NovoPedidoID INT;
EXEC sp_CriarPedido 
    @ClienteID = 1, 
    @ProdutoID = 10, 
    @Quantidade = 2,
    @PedidoID = @NovoPedidoID OUTPUT;
PRINT 'Pedido criado: ' + CAST(@NovoPedidoID AS VARCHAR);",
                    Language = "sql",
                    Explanation = "Este procedure complexo cria um pedido completo em uma transação. Usa parâmetro OUTPUT para retornar o ID do pedido criado. TRY/CATCH garante rollback em caso de erro.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Procedure Simples",
                    Description = "Crie um stored procedure sp_ListarProdutosPorCategoria que aceita uma categoria e retorna produtos dessa categoria ordenados por preço.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"CREATE PROCEDURE sp_ListarProdutosPorCategoria
    @Categoria NVARCHAR(50)
AS
BEGIN
    -- Complete aqui
END;",
                    Hints = new List<string>
                    {
                        "SELECT colunas relevantes FROM Produtos",
                        "WHERE Categoria = @Categoria",
                        "ORDER BY Preco"
                    }
                },
                new Exercise
                {
                    Title = "Procedure com Cálculo",
                    Description = "Crie sp_CalcularTotalVendasCliente que aceita ClienteID e retorna o total gasto pelo cliente usando parâmetro OUTPUT.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"CREATE PROCEDURE sp_CalcularTotalVendasCliente
    @ClienteID INT,
    @TotalGasto DECIMAL(10,2) OUTPUT
AS
BEGIN
    -- Complete aqui
END;",
                    Hints = new List<string>
                    {
                        "SELECT @TotalGasto = SUM(ValorTotal)",
                        "FROM Pedidos WHERE ClienteID = @ClienteID",
                        "Use COALESCE para tratar NULL"
                    }
                },
                new Exercise
                {
                    Title = "Procedure com Transação",
                    Description = "Crie sp_TransferirEstoque que transfere quantidade de um produto entre dois depósitos, usando transação para garantir atomicidade.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"CREATE PROCEDURE sp_TransferirEstoque
    @ProdutoID INT,
    @DepositoOrigem INT,
    @DepositoDestino INT,
    @Quantidade INT
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;
        -- Complete aqui
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;",
                    Hints = new List<string>
                    {
                        "UPDATE Estoque SET Quantidade = Quantidade - @Quantidade WHERE...",
                        "UPDATE Estoque SET Quantidade = Quantidade + @Quantidade WHERE...",
                        "Verifique se há estoque suficiente antes de transferir"
                    }
                }
            },
            Summary = "Nesta aula você aprendeu sobre stored procedures, como criá-los e executá-los, e usar parâmetros de entrada e saída. Você viu vantagens como performance e segurança, e desvantagens como acoplamento ao banco de dados. Stored procedures são ferramentas poderosas para encapsular lógica de dados complexa. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno. Grandes empresas como Microsoft, Google e Amazon aplicam estes conceitos em seus sistemas. Ao aprender estas técnicas, você está adquirindo conhecimento usado por desenvolvedores em todo o mundo. Considere como este conceito poderia ser aplicado em um projeto pessoal. Pensar em aplicações práticas ajuda a solidificar o entendimento e torna o aprendizado mais significativo. Iniciantes frequentemente cometem alguns erros ao aprender este tópico. Conhecer estes erros comuns ajuda a evitá-los e desenvolver código mais robusto desde o início. Um erro comum é não considerar casos extremos ou situações inesperadas. Sempre teste seu código com diferentes tipos de entrada, incluindo valores nulos, vazios ou inválidos. Outro erro frequente é otimizar prematuramente. Primeiro faça o código funcionar corretamente, depois otimize se necessário. Como diz o ditado: 'premature optimization is the root of all evil'. Quando encontrar problemas, use o debugger para entender o que está acontecendo. Coloque breakpoints, inspecione variáveis e execute o código passo a passo. Esta é uma habilidade essencial para todo desenvolvedor. Mensagens de erro podem parecer intimidadoras no início, mas elas são suas aliadas. Leia-as cuidadosamente - elas geralmente indicam exatamente onde e qual é o problema. Se algo não funcionar como esperado, não desanime. Debugging é uma parte normal do desenvolvimento. Até desenvolvedores experientes passam tempo significativo depurando código. Testar seu código é tão importante quanto escrevê-lo. Testes automatizados garantem que seu código funciona corretamente e continuará funcionando quando você fizer mudanças. Comece escrevendo testes simples para os casos mais comuns. Depois adicione testes para casos extremos e situações de erro. Uma boa cobertura de testes dá confiança para refatorar e melhorar o código. Considere a prática de TDD (Test-Driven Development), onde você escreve os testes antes do código. Isso ajuda a pensar claramente sobre o que o código deve fazer. Embora a correção seja sempre a prioridade, é importante estar ciente das implicações de performance de suas escolhas. Diferentes abordagens podem ter custos computacionais muito diferentes. Em muitos casos, a diferença de performance é negligenciável para aplicações pequenas. No entanto, à medida que seus programas crescem, escolhas eficientes se tornam cada vez mais importantes. Aprenda a usar ferramentas de profiling para identificar gargalos de performance. Otimize baseado em dados reais, não em suposições sobre onde o código é lento. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0004-00000000000b"),
            CourseId = _courseId,
            Title = "Stored Procedures",
            Duration = "60 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 60,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0004-00000000000a" }),
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
                "Compreender ADO.NET e sua arquitetura",
                "Conectar a bancos de dados usando SqlConnection",
                "Executar comandos SQL com SqlCommand",
                "Ler dados com SqlDataReader"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Introdução ao ADO.NET",
                    Content = "ADO.NET é a tecnologia de acesso a dados do .NET Framework, fornecendo classes para conectar a bancos de dados, executar comandos e recuperar resultados. ADO.NET é uma API de baixo nível que oferece controle total sobre operações de banco de dados. Diferente de ORMs como Entity Framework que abstraem SQL, ADO.NET requer que você escreva SQL explicitamente. Isso oferece máximo controle e performance, mas requer mais código. A arquitetura ADO.NET inclui providers específicos para cada banco de dados: System.Data.SqlClient para SQL Server, Npgsql para PostgreSQL, MySql.Data para MySQL. Cada provider implementa classes como Connection, Command, DataReader e DataAdapter. As classes principais são: SqlConnection para estabelecer conexão, SqlCommand para executar comandos SQL, SqlDataReader para ler resultados de forma forward-only e read-only (mais eficiente), e SqlDataAdapter para preencher DataSets (estruturas de dados desconectadas). ADO.NET é ideal quando você precisa de máxima performance, controle fino sobre SQL, ou trabalha com bancos de dados legados. Embora ORMs sejam mais produtivos para CRUD simples, ADO.NET brilha em operações complexas, bulk inserts, e cenários de alta performance.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Conectando e Executando Comandos",
                    Content = "Para conectar a um banco de dados, crie um SqlConnection com uma connection string que especifica servidor, banco de dados e credenciais. Connection strings podem usar autenticação Windows (Integrated Security=true) ou SQL Server (User ID e Password). Sempre use using para garantir que conexões sejam fechadas, mesmo se ocorrerem exceções. Conexões são recursos caros e limitados, então abra tarde e feche cedo. SqlCommand representa um comando SQL ou stored procedure. Defina CommandText com o SQL e CommandType (Text para SQL direto, StoredProcedure para procedures). Use parâmetros (@Nome) em vez de concatenar strings para prevenir SQL injection. SqlParameter adiciona parâmetros ao comando. ExecuteNonQuery executa comandos que não retornam dados (INSERT, UPDATE, DELETE) e retorna o número de linhas afetadas. ExecuteScalar retorna um único valor (útil para COUNT, SUM, ou SELECT de um campo). ExecuteReader retorna SqlDataReader para ler múltiplas linhas. Connection pooling é automático no ADO.NET, reutilizando conexões para melhorar performance. Não crie novas connection strings para cada operação, use a mesma string para aproveitar pooling.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Lendo Dados com SqlDataReader",
                    Content = "SqlDataReader é a forma mais eficiente de ler dados em ADO.NET. Ele lê dados de forma forward-only e read-only, consumindo mínima memória. Após ExecuteReader, use Read() para avançar para a próxima linha. Read() retorna true se há mais linhas, false quando termina. Acesse colunas por índice (reader[0]) ou nome (reader[\"Nome\"]). Use métodos tipados como GetInt32, GetString, GetDateTime para melhor performance e type safety. Sempre verifique IsDBNull antes de ler valores que podem ser NULL. SqlDataReader mantém a conexão aberta enquanto está lendo, então processe dados rapidamente ou copie para uma estrutura em memória. Use using com SqlDataReader para garantir que seja fechado. Para múltiplos result sets (múltiplos SELECTs em um comando), use NextResult() para avançar para o próximo result set. SqlDataReader é ideal para grandes volumes de dados onde você processa linha por linha. Para pequenos conjuntos de dados que você precisa manipular, considere carregar em uma List<T>. ADO.NET também oferece DataSet e DataTable para cenários desconectados, mas são menos usados em aplicações modernas devido ao overhead de memória.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Conectar e Executar Comando Simples",
                    Code = @"using System;
using System.Data;
using System.Data.SqlClient;

// Connection string (use configuração em produção)
string connectionString = ""Server=localhost;Database=MeuBanco;Integrated Security=true;"";

// Inserir um cliente
using (SqlConnection connection = new SqlConnection(connectionString))
{
    connection.Open();

    string sql = ""INSERT INTO Clientes (Nome, Email) VALUES (@Nome, @Email)"";
    
    using (SqlCommand command = new SqlCommand(sql, connection))
    {
        // Usar parâmetros previne SQL injection
        command.Parameters.AddWithValue(""@Nome"", ""João Silva"");
        command.Parameters.AddWithValue(""@Email"", ""joao@email.com"");

        int rowsAffected = command.ExecuteNonQuery();
        Console.WriteLine($""{rowsAffected} linha(s) inserida(s)"");
    }
}",
                    Language = "csharp",
                    Explanation = "Este código conecta ao banco de dados, cria um comando SQL com parâmetros, e executa com ExecuteNonQuery. Using garante que recursos sejam liberados. Parâmetros previnem SQL injection.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Ler Dados com SqlDataReader",
                    Code = @"using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

public class Cliente
{
    public int ClienteID { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

string connectionString = ""Server=localhost;Database=MeuBanco;Integrated Security=true;"";

using (SqlConnection connection = new SqlConnection(connectionString))
{
    connection.Open();

    string sql = ""SELECT ClienteID, Nome, Email FROM Clientes WHERE Ativo = 1"";
    
    using (SqlCommand command = new SqlCommand(sql, connection))
    using (SqlDataReader reader = command.ExecuteReader())
    {
        while (reader.Read())
        {
            int id = reader.GetInt32(0); // Por índice
            string nome = reader.GetString(1);
            string email = reader[""Email""].ToString(); // Por nome

            Console.WriteLine($""ID: {id}, Nome: {nome}, Email: {email}"");
        }
    }
}

// Ou carregar em uma lista
var clientes = new List<Cliente>();
using (SqlConnection connection = new SqlConnection(connectionString))
{
    connection.Open();
    using (SqlCommand command = new SqlCommand(""SELECT * FROM Clientes"", connection))
    using (SqlDataReader reader = command.ExecuteReader())
    {
        while (reader.Read())
        {
            clientes.Add(new Cliente
            {
                ClienteID = reader.GetInt32(0),
                Nome = reader.GetString(1),
                Email = reader.GetString(2)
            });
        }
    }
}",
                    Language = "csharp",
                    Explanation = "SqlDataReader lê dados eficientemente. Read() avança para próxima linha. GetInt32, GetString são métodos tipados. Você pode acessar por índice ou nome. O segundo exemplo mostra como carregar dados em objetos.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Inserir Produto",
                    Description = "Escreva código C# para inserir um produto no banco de dados usando ADO.NET. Use parâmetros para Nome, Preco e Estoque.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using System.Data.SqlClient;

string connectionString = ""..."";

using (SqlConnection connection = new SqlConnection(connectionString))
{
    connection.Open();
    
    // Complete aqui
}",
                    Hints = new List<string>
                    {
                        "CREATE SqlCommand com INSERT INTO Produtos",
                        "AddWithValue para cada parâmetro",
                        "ExecuteNonQuery para executar",
                        "Sempre use using para liberar recursos"
                    }
                },
                new Exercise
                {
                    Title = "Buscar e Exibir Clientes",
                    Description = "Crie um método que busca todos os clientes de uma cidade específica e retorna uma List<Cliente>.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"public List<Cliente> BuscarClientesPorCidade(string cidade)
{
    var clientes = new List<Cliente>();
    
    // Complete aqui
    
    return clientes;
}",
                    Hints = new List<string>
                    {
                        "Use SqlConnection e SqlCommand",
                        "SELECT com WHERE Cidade = @Cidade",
                        "SqlDataReader para ler resultados",
                        "while (reader.Read()) para processar linhas"
                    }
                },
                new Exercise
                {
                    Title = "Executar Stored Procedure",
                    Description = "Execute o stored procedure sp_CriarPedido usando ADO.NET, passando parâmetros de entrada e capturando o parâmetro de saída PedidoID.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"using (SqlConnection connection = new SqlConnection(connectionString))
{
    connection.Open();
    
    using (SqlCommand command = new SqlCommand(""sp_CriarPedido"", connection))
    {
        command.CommandType = CommandType.StoredProcedure;
        
        // Complete aqui
    }
}",
                    Hints = new List<string>
                    {
                        "CommandType = CommandType.StoredProcedure",
                        "AddWithValue para parâmetros de entrada",
                        "Add com Direction = ParameterDirection.Output para saída",
                        "Leia valor de saída após ExecuteNonQuery"
                    }
                }
            },
            Summary = "Nesta aula você aprendeu sobre ADO.NET, a API de acesso a dados do .NET. Você viu como conectar a bancos de dados com SqlConnection, executar comandos com SqlCommand, e ler dados com SqlDataReader. ADO.NET oferece controle total e máxima performance para operações de banco de dados. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno. Grandes empresas como Microsoft, Google e Amazon aplicam estes conceitos em seus sistemas. Ao aprender estas técnicas, você está adquirindo conhecimento usado por desenvolvedores em todo o mundo. Considere como este conceito poderia ser aplicado em um projeto pessoal. Pensar em aplicações práticas ajuda a solidificar o entendimento e torna o aprendizado mais significativo. Iniciantes frequentemente cometem alguns erros ao aprender este tópico. Conhecer estes erros comuns ajuda a evitá-los e desenvolver código mais robusto desde o início. Um erro comum é não considerar casos extremos ou situações inesperadas. Sempre teste seu código com diferentes tipos de entrada, incluindo valores nulos, vazios ou inválidos. Outro erro frequente é otimizar prematuramente. Primeiro faça o código funcionar corretamente, depois otimize se necessário. Como diz o ditado: 'premature optimization is the root of all evil'. Quando encontrar problemas, use o debugger para entender o que está acontecendo. Coloque breakpoints, inspecione variáveis e execute o código passo a passo. Esta é uma habilidade essencial para todo desenvolvedor. Mensagens de erro podem parecer intimidadoras no início, mas elas são suas aliadas. Leia-as cuidadosamente - elas geralmente indicam exatamente onde e qual é o problema. Se algo não funcionar como esperado, não desanime. Debugging é uma parte normal do desenvolvimento. Até desenvolvedores experientes passam tempo significativo depurando código. Testar seu código é tão importante quanto escrevê-lo. Testes automatizados garantem que seu código funciona corretamente e continuará funcionando quando você fizer mudanças. Comece escrevendo testes simples para os casos mais comuns. Depois adicione testes para casos extremos e situações de erro. Uma boa cobertura de testes dá confiança para refatorar e melhorar o código. Considere a prática de TDD (Test-Driven Development), onde você escreve os testes antes do código. Isso ajuda a pensar claramente sobre o que o código deve fazer. Embora a correção seja sempre a prioridade, é importante estar ciente das implicações de performance de suas escolhas. Diferentes abordagens podem ter custos computacionais muito diferentes. Em muitos casos, a diferença de performance é negligenciável para aplicações pequenas. No entanto, à medida que seus programas crescem, escolhas eficientes se tornam cada vez mais importantes. Aprenda a usar ferramentas de profiling para identificar gargalos de performance. Otimize baseado em dados reais, não em suposições sobre onde o código é lento. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0004-00000000000c"),
            CourseId = _courseId,
            Title = "ADO.NET - Fundamentos",
            Duration = "65 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 65,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0004-00000000000b" }),
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
                "Compreender o que é Dapper e suas vantagens",
                "Configurar Dapper em projetos C#",
                "Executar consultas e mapeamento automático",
                "Usar Dapper para operações CRUD"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Introdução ao Dapper",
                    Content = "Dapper é um micro-ORM (Object-Relational Mapper) criado pela equipe do Stack Overflow para combinar a simplicidade do ADO.NET com a conveniência de mapeamento automático de objetos. Diferente de ORMs completos como Entity Framework, Dapper não gera SQL automaticamente - você escreve SQL manualmente. Isso oferece controle total sobre consultas enquanto elimina o código boilerplate de ADO.NET para mapear resultados em objetos. Dapper é extremamente rápido, quase tão rápido quanto ADO.NET puro, porque usa geração de código IL em tempo de execução para mapeamento eficiente. Ele é apenas uma biblioteca de extensão sobre IDbConnection, então funciona com qualquer provider ADO.NET (SQL Server, PostgreSQL, MySQL, SQLite). Dapper é ideal quando você quer escrever SQL otimizado mas não quer o overhead de Entity Framework ou o código repetitivo de ADO.NET. É perfeito para aplicações de alta performance, consultas complexas, e cenários onde você precisa de controle fino sobre SQL. Dapper é usado por grandes empresas como Stack Overflow, que processa milhões de requisições diariamente. Sua simplicidade e performance o tornam uma escolha popular para APIs e microserviços.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Configurando e Usando Dapper",
                    Content = "Para usar Dapper, instale o pacote NuGet Dapper. Dapper adiciona métodos de extensão a IDbConnection, então você continua usando SqlConnection normalmente. Os principais métodos são: Query<T> para consultas que retornam múltiplas linhas, QuerySingle<T> para uma única linha, QueryFirst<T> para a primeira linha, Execute para comandos sem retorno, e ExecuteScalar para valores únicos. Dapper mapeia automaticamente colunas do resultado para propriedades do objeto baseado no nome. Se os nomes não correspondem exatamente, use aliases no SQL (AS). Dapper suporta tipos anônimos, classes, structs, e tipos primitivos. Para parâmetros, passe um objeto anônimo ou a própria entidade. Dapper converte propriedades em parâmetros SQL automaticamente. Você pode executar múltiplas consultas e mapear para diferentes tipos com QueryMultiple. Dapper suporta stored procedures definindo commandType: CommandType.StoredProcedure. Para operações em lote, use Execute com uma coleção - Dapper executa eficientemente. Dapper não rastreia mudanças como Entity Framework, então você controla explicitamente quando salvar. Isso oferece previsibilidade e performance.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Operações CRUD com Dapper",
                    Content = "Implementar CRUD com Dapper é direto. Para Create (INSERT), use Execute com SQL INSERT e passe o objeto. Para obter o ID gerado, adicione SELECT SCOPE_IDENTITY() ao SQL e use ExecuteScalar. Para Read (SELECT), use Query<T> para listas ou QuerySingle<T> para um registro. Dapper mapeia automaticamente resultados para objetos. Para Update, use Execute com SQL UPDATE e passe o objeto com ID. Para Delete, use Execute com SQL DELETE e passe o ID. Dapper não gera SQL automaticamente, então você escreve exatamente o SQL que precisa. Isso é uma vantagem para performance e controle. Para relacionamentos, você pode usar QueryMultiple para buscar entidade principal e relacionadas em uma única viagem ao banco, ou usar Query com tipos dinâmicos para mapear joins. Dapper.Contrib é uma extensão que adiciona métodos como Insert, Update, Delete, Get que geram SQL automaticamente, aproximando-se de um ORM completo. Para aplicações que precisam de máxima performance com código limpo, Dapper é uma excelente escolha. Ele oferece 80% da conveniência de ORMs com 95% da performance de ADO.NET puro.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Consultas Básicas com Dapper",
                    Code = @"using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;

public class Cliente
{
    public int ClienteID { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Cidade { get; set; } = string.Empty;
    public bool Ativo { get; set; }
}

string connectionString = ""Server=localhost;Database=MeuBanco;Integrated Security=true;"";

// Buscar todos os clientes
using (var connection = new SqlConnection(connectionString))
{
    var clientes = connection.Query<Cliente>(
        ""SELECT ClienteID, Nome, Email FROM Clientes WHERE Ativo = 1""
    ).ToList();

    foreach (var cliente in clientes)
    {
        Console.WriteLine($""{cliente.Nome} - {cliente.Email}"");
    }
}

// Buscar cliente por ID
using (var connection = new SqlConnection(connectionString))
{
    var cliente = connection.QuerySingle<Cliente>(
        ""SELECT * FROM Clientes WHERE ClienteID = @Id"",
        new { Id = 1 }
    );
}

// Buscar com filtros
using (var connection = new SqlConnection(connectionString))
{
    var clientes = connection.Query<Cliente>(
        ""SELECT * FROM Clientes WHERE Cidade = @Cidade AND Ativo = @Ativo"",
        new { Cidade = ""São Paulo"", Ativo = true }
    ).ToList();
}",
                    Language = "csharp",
                    Explanation = "Dapper adiciona métodos de extensão a SqlConnection. Query<T> executa SQL e mapeia resultados automaticamente. Parâmetros são passados como objetos anônimos. Muito mais simples que ADO.NET puro.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Operações CRUD Completas",
                    Code = @"using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;

public class Cliente
{
    public int ClienteID { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public string Cidade { get; set; } = string.Empty;
    public bool Ativo { get; set; }
}

public class ClienteRepository
{
    private readonly string _connectionString;

    public ClienteRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    // CREATE
    public int Inserir(Cliente cliente)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var sql = @""INSERT INTO Clientes (Nome, Email, Telefone, Cidade) 
                        VALUES (@Nome, @Email, @Telefone, @Cidade);
                        SELECT CAST(SCOPE_IDENTITY() as int)"";
            
            return connection.ExecuteScalar<int>(sql, cliente);
        }
    }

    // READ
    public Cliente BuscarPorId(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            return connection.QuerySingleOrDefault<Cliente>(
                ""SELECT * FROM Clientes WHERE ClienteID = @Id"",
                new { Id = id }
            );
        }
    }

    public List<Cliente> BuscarTodos()
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            return connection.Query<Cliente>(
                ""SELECT * FROM Clientes WHERE Ativo = 1""
            ).ToList();
        }
    }

    // UPDATE
    public void Atualizar(Cliente cliente)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var sql = @""UPDATE Clientes 
                        SET Nome = @Nome, Email = @Email, 
                            Telefone = @Telefone, Cidade = @Cidade
                        WHERE ClienteID = @ClienteID"";
            
            connection.Execute(sql, cliente);
        }
    }

    // DELETE
    public void Deletar(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Execute(
                ""DELETE FROM Clientes WHERE ClienteID = @Id"",
                new { Id = id }
            );
        }
    }
}",
                    Language = "csharp",
                    Explanation = "Este repositório implementa CRUD completo com Dapper. Execute para INSERT/UPDATE/DELETE, Query para SELECT múltiplos, QuerySingle para um registro. Dapper mapeia objetos automaticamente, eliminando código boilerplate.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Consulta Simples",
                    Description = "Use Dapper para buscar todos os produtos de uma categoria específica e retornar List<Produto>.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Dapper;

public List<Produto> BuscarPorCategoria(string categoria)
{
    using (var connection = new SqlConnection(connectionString))
    {
        // Complete aqui
    }
}",
                    Hints = new List<string>
                    {
                        "Use connection.Query<Produto>",
                        "SQL: SELECT * FROM Produtos WHERE Categoria = @Categoria",
                        "Passe new { Categoria = categoria } como parâmetro",
                        "Converta para List com .ToList()"
                    }
                },
                new Exercise
                {
                    Title = "Inserir com Retorno de ID",
                    Description = "Implemente um método para inserir um pedido e retornar o ID gerado usando Dapper.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"using System.Data.SqlClient;

public int InserirPedido(Pedido pedido)
{
    using (var connection = new SqlConnection(connectionString))
    {
        // Complete aqui
    }
}",
                    Hints = new List<string>
                    {
                        "SQL: INSERT INTO Pedidos (...) VALUES (...); SELECT CAST(SCOPE_IDENTITY() as int)",
                        "Use connection.ExecuteScalar<int>",
                        "Passe o objeto pedido como parâmetro",
                        "Dapper mapeia propriedades automaticamente"
                    }
                },
                new Exercise
                {
                    Title = "Repository Completo",
                    Description = "Crie uma classe ProdutoRepository com métodos CRUD completos usando Dapper: Inserir, BuscarPorId, BuscarTodos, Atualizar, Deletar.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"public class ProdutoRepository
{
    private readonly string _connectionString;

    public ProdutoRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    // Complete os métodos CRUD aqui
}",
                    Hints = new List<string>
                    {
                        "Inserir: Execute com INSERT + SCOPE_IDENTITY",
                        "BuscarPorId: QuerySingleOrDefault",
                        "BuscarTodos: Query<Produto>",
                        "Atualizar: Execute com UPDATE",
                        "Deletar: Execute com DELETE"
                    }
                }
            },
            Summary = "Nesta aula você aprendeu sobre Dapper, um micro-ORM que combina performance de ADO.NET com conveniência de mapeamento automático. Você viu como executar consultas, mapear resultados, e implementar operações CRUD. Dapper é ideal para aplicações que precisam de performance sem abrir mão de código limpo. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno. Grandes empresas como Microsoft, Google e Amazon aplicam estes conceitos em seus sistemas. Ao aprender estas técnicas, você está adquirindo conhecimento usado por desenvolvedores em todo o mundo. Considere como este conceito poderia ser aplicado em um projeto pessoal. Pensar em aplicações práticas ajuda a solidificar o entendimento e torna o aprendizado mais significativo. Iniciantes frequentemente cometem alguns erros ao aprender este tópico. Conhecer estes erros comuns ajuda a evitá-los e desenvolver código mais robusto desde o início. Um erro comum é não considerar casos extremos ou situações inesperadas. Sempre teste seu código com diferentes tipos de entrada, incluindo valores nulos, vazios ou inválidos. Outro erro frequente é otimizar prematuramente. Primeiro faça o código funcionar corretamente, depois otimize se necessário. Como diz o ditado: 'premature optimization is the root of all evil'. Quando encontrar problemas, use o debugger para entender o que está acontecendo. Coloque breakpoints, inspecione variáveis e execute o código passo a passo. Esta é uma habilidade essencial para todo desenvolvedor. Mensagens de erro podem parecer intimidadoras no início, mas elas são suas aliadas. Leia-as cuidadosamente - elas geralmente indicam exatamente onde e qual é o problema. Se algo não funcionar como esperado, não desanime. Debugging é uma parte normal do desenvolvimento. Até desenvolvedores experientes passam tempo significativo depurando código. Testar seu código é tão importante quanto escrevê-lo. Testes automatizados garantem que seu código funciona corretamente e continuará funcionando quando você fizer mudanças. Comece escrevendo testes simples para os casos mais comuns. Depois adicione testes para casos extremos e situações de erro. Uma boa cobertura de testes dá confiança para refatorar e melhorar o código. Considere a prática de TDD (Test-Driven Development), onde você escreve os testes antes do código. Isso ajuda a pensar claramente sobre o que o código deve fazer. Embora a correção seja sempre a prioridade, é importante estar ciente das implicações de performance de suas escolhas. Diferentes abordagens podem ter custos computacionais muito diferentes. Em muitos casos, a diferença de performance é negligenciável para aplicações pequenas. No entanto, à medida que seus programas crescem, escolhas eficientes se tornam cada vez mais importantes. Aprenda a usar ferramentas de profiling para identificar gargalos de performance. Otimize baseado em dados reais, não em suposições sobre onde o código é lento. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0004-00000000000d"),
            CourseId = _courseId,
            Title = "Dapper - Micro-ORM",
            Duration = "60 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 60,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0004-00000000000c" }),
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
                "Compreender conceitos de segurança de banco de dados",
                "Prevenir SQL Injection",
                "Implementar autenticação e autorização",
                "Aplicar princípio do menor privilégio"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "SQL Injection e Como Prevenir",
                    Content = "SQL Injection é uma das vulnerabilidades mais perigosas em aplicações web. Ocorre quando entrada do usuário é concatenada diretamente em comandos SQL, permitindo que atacantes executem SQL arbitrário. Por exemplo, se você concatena: SELECT * FROM Users WHERE Username = ' + username + ', um atacante pode inserir ' OR '1'='1 como username, resultando em SELECT * FROM Users WHERE Username = '' OR '1'='1', que retorna todos os usuários. Pior ainda, atacantes podem usar ; para executar múltiplos comandos, potencialmente deletando tabelas inteiras com DROP TABLE. A prevenção é simples mas crucial: SEMPRE use parâmetros. Em ADO.NET, use SqlParameter. Em Dapper, passe objetos como parâmetros. ORMs como Entity Framework parametrizam automaticamente. Parâmetros garantem que entrada do usuário seja tratada como dados, não como código SQL. Nunca confie em validação do lado do cliente - sempre valide no servidor. Use stored procedures com parâmetros para camada adicional de segurança. Ferramentas de análise estática podem detectar SQL injection em código. Testes de penetração devem incluir tentativas de SQL injection. SQL Injection pode levar a roubo de dados, modificação não autorizada, e comprometimento completo do sistema. Prevenir SQL injection deve ser prioridade máxima em qualquer aplicação que usa banco de dados.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Autenticação e Autorização",
                    Content = "Autenticação verifica quem você é, autorização determina o que você pode fazer. Em bancos de dados, autenticação pode usar Windows Authentication (integrada com Active Directory) ou SQL Server Authentication (usuário e senha no banco). Windows Authentication é mais segura porque usa Kerberos e não requer armazenar senhas. SQL Authentication é necessária para aplicações web ou quando Windows Authentication não está disponível. Sempre use conexões criptografadas (Encrypt=true) para proteger credenciais em trânsito. Nunca armazene connection strings com senhas em código-fonte - use configuração externa ou Azure Key Vault. Autorização em SQL Server usa logins (nível de servidor) e users (nível de banco de dados). Logins podem ser mapeados para users em múltiplos bancos. Roles agrupam permissões: db_datareader pode ler todas as tabelas, db_datawriter pode modificar. Crie roles customizadas para necessidades específicas. Princípio do menor privilégio: dê apenas as permissões mínimas necessárias. Aplicações não devem usar conta sa ou db_owner. Crie users específicos com permissões limitadas. Para aplicações web, considere usar um único user de aplicação e implementar autorização na camada de aplicação. Audite acessos com SQL Server Audit para rastrear quem fez o quê e quando.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Criptografia e Proteção de Dados Sensíveis",
                    Content = "Dados sensíveis como senhas, números de cartão de crédito e informações pessoais devem ser protegidos. Nunca armazene senhas em texto plano - use hashing com salt (bcrypt, Argon2). Hashing é unidirecional, então mesmo se o banco for comprometido, senhas não podem ser recuperadas. Para dados que precisam ser recuperados (como números de cartão), use criptografia. SQL Server oferece Transparent Data Encryption (TDE) que criptografa todo o banco de dados em disco. Always Encrypted criptografa colunas específicas, mantendo dados criptografados mesmo em memória do servidor. Column-level encryption permite criptografar colunas individuais com chaves gerenciadas pela aplicação. Para dados em trânsito, use SSL/TLS (Encrypt=true na connection string). Implemente row-level security para garantir que usuários vejam apenas seus próprios dados. Dynamic data masking pode ocultar dados sensíveis de usuários não autorizados. Backups devem ser criptografados para proteger dados em repouso. Considere requisitos de compliance como LGPD, GDPR, PCI-DSS que exigem proteção de dados pessoais e financeiros. Implemente políticas de retenção de dados, deletando dados antigos que não são mais necessários. Logs de auditoria devem registrar acessos a dados sensíveis. Treine desenvolvedores em práticas de segurança e realize revisões de código focadas em segurança.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "SQL Injection - Vulnerável vs Seguro",
                    Code = @"using System;
using System.Data.SqlClient;
using Dapper;

// ❌ VULNERÁVEL - NUNCA FAÇA ISSO!
string username = ""usuario_input"";
string sql = ""SELECT * FROM Users WHERE Username = '"" + username + ""'"";
// Atacante pode inserir: ' OR '1'='1' --

// ✅ SEGURO - Use parâmetros com Dapper
using (var connection = new SqlConnection(connectionString))
{
    var sqlSeguro = ""SELECT * FROM Users WHERE Username = @Username"";
    var user = connection.QuerySingleOrDefault<User>(sqlSeguro, new { Username = username });
}

// ✅ SEGURO - ADO.NET com parâmetros
using (var connection = new SqlConnection(connectionString))
{
    connection.Open();
    var command = new SqlCommand(""SELECT * FROM Users WHERE Username = @Username"", connection);
    command.Parameters.AddWithValue(""@Username"", username);
    using (var reader = command.ExecuteReader())
    {
        // Processar resultados
    }
}",
                    Language = "csharp",
                    Explanation = "O primeiro exemplo é vulnerável a SQL injection. Os exemplos seguros usam parâmetros, que tratam entrada como dados, não código. Sempre use parâmetros, nunca concatene strings.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Implementar Permissões Granulares",
                    Code = @"-- Criar login e user para aplicação
CREATE LOGIN AppUser WITH PASSWORD = 'SenhaForte123!';
USE MeuBanco;
CREATE USER AppUser FOR LOGIN AppUser;

-- Criar role customizada com permissões específicas
CREATE ROLE AppRole;

-- Dar permissões apenas nas tabelas necessárias
GRANT SELECT, INSERT, UPDATE ON Clientes TO AppRole;
GRANT SELECT, INSERT, UPDATE ON Pedidos TO AppRole;
GRANT SELECT ON Produtos TO AppRole; -- Apenas leitura

-- Negar operações perigosas
DENY DELETE ON Clientes TO AppRole;
DENY ALTER, DROP ON SCHEMA::dbo TO AppRole;

-- Adicionar user à role
ALTER ROLE AppRole ADD MEMBER AppUser;

-- Dar permissão para executar stored procedures específicos
GRANT EXECUTE ON sp_CriarPedido TO AppRole;
GRANT EXECUTE ON sp_BuscarClientes TO AppRole;

-- Connection string da aplicação
-- Server=localhost;Database=MeuBanco;User ID=AppUser;Password=SenhaForte123!;",
                    Language = "sql",
                    Explanation = "Este código implementa princípio do menor privilégio. Cria user específico para aplicação com permissões limitadas. Usa roles para agrupar permissões. Nega operações perigosas explicitamente.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Corrigir SQL Injection",
                    Description = "Reescreva este código vulnerável usando parâmetros: string sql = \"SELECT * FROM Produtos WHERE Categoria = '\" + categoria + \"' AND Preco < \" + precoMaximo;",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Dapper;

// Código vulnerável
string sql = ""SELECT * FROM Produtos WHERE Categoria = '"" + categoria + ""' AND Preco < "" + precoMaximo;

// Reescreva usando parâmetros (Dapper ou ADO.NET)
",
                    Hints = new List<string>
                    {
                        "Use @Categoria e @PrecoMaximo como parâmetros",
                        "Passe new { Categoria = categoria, PrecoMaximo = precoMaximo }",
                        "Nunca concatene entrada do usuário em SQL"
                    }
                },
                new Exercise
                {
                    Title = "Criar User com Permissões Limitadas",
                    Description = "Crie um login e user para uma aplicação de relatórios que precisa apenas ler dados de Clientes e Pedidos, sem permissão para modificar.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"-- Criar login
CREATE LOGIN ReportUser WITH PASSWORD = '...';

-- Criar user e dar permissões
-- Complete aqui",
                    Hints = new List<string>
                    {
                        "CREATE USER ReportUser FOR LOGIN ReportUser",
                        "GRANT SELECT ON Clientes TO ReportUser",
                        "GRANT SELECT ON Pedidos TO ReportUser",
                        "Não dê permissões INSERT, UPDATE, DELETE"
                    }
                },
                new Exercise
                {
                    Title = "Implementar Hashing de Senhas",
                    Description = "Implemente um método para hash de senhas usando BCrypt.Net-Next (instale o pacote NuGet). Nunca armazene senhas em texto plano.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"using BCrypt.Net;

public class PasswordService
{
    public string HashPassword(string password)
    {
        // Complete aqui
    }

    public bool VerifyPassword(string password, string hash)
    {
        // Complete aqui
    }
}",
                    Hints = new List<string>
                    {
                        "Use BCrypt.HashPassword(password) para criar hash",
                        "Use BCrypt.Verify(password, hash) para verificar",
                        "BCrypt adiciona salt automaticamente",
                        "Armazene o hash no banco, não a senha"
                    }
                }
            },
            Summary = "Nesta aula você aprendeu sobre segurança de banco de dados, incluindo prevenção de SQL injection, autenticação e autorização, e proteção de dados sensíveis. Segurança deve ser prioridade em qualquer aplicação que lida com dados importantes. Sempre use parâmetros, implemente menor privilégio, e proteja dados sensíveis. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno. Grandes empresas como Microsoft, Google e Amazon aplicam estes conceitos em seus sistemas. Ao aprender estas técnicas, você está adquirindo conhecimento usado por desenvolvedores em todo o mundo. Considere como este conceito poderia ser aplicado em um projeto pessoal. Pensar em aplicações práticas ajuda a solidificar o entendimento e torna o aprendizado mais significativo. Iniciantes frequentemente cometem alguns erros ao aprender este tópico. Conhecer estes erros comuns ajuda a evitá-los e desenvolver código mais robusto desde o início. Um erro comum é não considerar casos extremos ou situações inesperadas. Sempre teste seu código com diferentes tipos de entrada, incluindo valores nulos, vazios ou inválidos. Outro erro frequente é otimizar prematuramente. Primeiro faça o código funcionar corretamente, depois otimize se necessário. Como diz o ditado: 'premature optimization is the root of all evil'. Quando encontrar problemas, use o debugger para entender o que está acontecendo. Coloque breakpoints, inspecione variáveis e execute o código passo a passo. Esta é uma habilidade essencial para todo desenvolvedor. Mensagens de erro podem parecer intimidadoras no início, mas elas são suas aliadas. Leia-as cuidadosamente - elas geralmente indicam exatamente onde e qual é o problema. Se algo não funcionar como esperado, não desanime. Debugging é uma parte normal do desenvolvimento. Até desenvolvedores experientes passam tempo significativo depurando código. Testar seu código é tão importante quanto escrevê-lo. Testes automatizados garantem que seu código funciona corretamente e continuará funcionando quando você fizer mudanças. Comece escrevendo testes simples para os casos mais comuns. Depois adicione testes para casos extremos e situações de erro. Uma boa cobertura de testes dá confiança para refatorar e melhorar o código. Considere a prática de TDD (Test-Driven Development), onde você escreve os testes antes do código. Isso ajuda a pensar claramente sobre o que o código deve fazer. Embora a correção seja sempre a prioridade, é importante estar ciente das implicações de performance de suas escolhas. Diferentes abordagens podem ter custos computacionais muito diferentes. Em muitos casos, a diferença de performance é negligenciável para aplicações pequenas. No entanto, à medida que seus programas crescem, escolhas eficientes se tornam cada vez mais importantes. Aprenda a usar ferramentas de profiling para identificar gargalos de performance. Otimize baseado em dados reais, não em suposições sobre onde o código é lento. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0004-00000000000e"),
            CourseId = _courseId,
            Title = "Segurança de Banco de Dados",
            Duration = "60 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 60,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0004-00000000000d" }),
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
                "Entender views e suas aplicações",
                "Criar e usar views para simplificar consultas",
                "Compreender views materializadas",
                "Aplicar views em cenários reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "O que são Views?",
                    Content = "Views são consultas SQL salvas que funcionam como tabelas virtuais. Uma view não armazena dados, apenas a definição da consulta. Quando você consulta uma view, o banco de dados executa a consulta subjacente. Views simplificam consultas complexas, encapsulam lógica de negócio, e fornecem camada de abstração sobre tabelas. Por exemplo, se você frequentemente junta Pedidos com Clientes e Produtos, pode criar uma view que faz esses joins, permitindo consultas simples como SELECT * FROM vw_PedidosCompletos. Views melhoram segurança permitindo acesso a subconjuntos de dados sem expor tabelas completas. Você pode dar permissão para uma view que mostra apenas colunas não-sensíveis, ocultando dados confidenciais. Views facilitam manutenção: se a estrutura de tabelas muda, você atualiza a view sem modificar código da aplicação. Views podem incluir cálculos, agregações, e filtros. Elas são especialmente úteis para relatórios e dashboards. Views podem referenciar outras views, criando hierarquias de abstração. No entanto, views complexas com múltiplos joins podem impactar performance. Use índices nas tabelas base e considere views indexadas para melhor performance.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Criando e Usando Views",
                    Content = "Para criar uma view, use CREATE VIEW nome AS SELECT.... A consulta SELECT define o que a view retorna. Você pode usar WHERE, JOIN, GROUP BY, e outras cláusulas. Após criar, consulte a view como se fosse uma tabela: SELECT * FROM MinhaView. ALTER VIEW modifica views existentes. DROP VIEW remove views. Views podem ter colunas calculadas: CREATE VIEW vw_ProdutosComDesconto AS SELECT Nome, Preco, Preco * 0.9 AS PrecoComDesconto FROM Produtos. Você pode filtrar dados em views: CREATE VIEW vw_ProdutosAtivos AS SELECT * FROM Produtos WHERE Ativo = 1. Isso garante que consultas sempre vejam apenas produtos ativos. Views podem agregar dados: CREATE VIEW vw_VendasPorCategoria AS SELECT Categoria, SUM(Quantidade) AS TotalVendido FROM... GROUP BY Categoria. WITH CHECK OPTION garante que INSERT/UPDATE através da view respeitem o filtro WHERE da view. Por exemplo, uma view de produtos ativos com CHECK OPTION impede inserir produtos inativos. Views são read-only por padrão, mas views simples (baseadas em uma tabela sem agregações) podem ser atualizáveis. Para views complexas, use INSTEAD OF triggers para permitir modificações.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Views Indexadas e Materializadas",
                    Content = "Views indexadas (também chamadas materialized views em outros bancos) armazenam fisicamente os resultados da consulta, melhorando drasticamente performance de consultas complexas. Em SQL Server, crie uma view indexada criando primeiro um índice clustered único na view: CREATE UNIQUE CLUSTERED INDEX idx ON MinhaView(Coluna). Isso materializa os dados da view. Views indexadas são atualizadas automaticamente quando dados base mudam, mas isso adiciona overhead em INSERT/UPDATE/DELETE. Use views indexadas para consultas frequentes e caras em dados que mudam raramente. Views indexadas têm restrições: não podem usar OUTER JOIN, subconsultas, funções não-determinísticas, ou DISTINCT. PostgreSQL e Oracle suportam materialized views que devem ser atualizadas manualmente com REFRESH MATERIALIZED VIEW. Isso é útil para relatórios que não precisam de dados em tempo real. Views indexadas são excelentes para data warehouses e sistemas de relatórios onde consultas complexas são executadas frequentemente. Elas trocam espaço em disco e overhead de atualização por velocidade de consulta. Monitore o impacto de views indexadas em operações de escrita. Se INSERT/UPDATE ficam muito lentos, considere atualizar views indexadas em horários de baixo uso ou usar materialized views com refresh manual.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Criando Views Úteis",
                    Code = @"-- View para pedidos completos com informações do cliente
CREATE VIEW vw_PedidosCompletos AS
SELECT 
    P.PedidoID,
    P.DataPedido,
    P.Status,
    C.Nome AS NomeCliente,
    C.Email AS EmailCliente,
    C.Cidade,
    P.ValorTotal
FROM Pedidos P
INNER JOIN Clientes C ON P.ClienteID = C.ClienteID;

-- Usar a view
SELECT * FROM vw_PedidosCompletos 
WHERE Cidade = 'São Paulo' 
ORDER BY DataPedido DESC;

-- View com cálculos
CREATE VIEW vw_ProdutosComMargemLucro AS
SELECT 
    ProdutoID,
    Nome,
    CustoUnitario,
    PrecoVenda,
    (PrecoVenda - CustoUnitario) AS Lucro,
    ((PrecoVenda - CustoUnitario) / PrecoVenda * 100) AS MargemLucroPercentual
FROM Produtos;

-- View agregada para relatórios
CREATE VIEW vw_VendasMensais AS
SELECT 
    YEAR(DataPedido) AS Ano,
    MONTH(DataPedido) AS Mes,
    COUNT(*) AS TotalPedidos,
    SUM(ValorTotal) AS ValorTotalVendas,
    AVG(ValorTotal) AS TicketMedio
FROM Pedidos
GROUP BY YEAR(DataPedido), MONTH(DataPedido);",
                    Language = "sql",
                    Explanation = "Views encapsulam consultas complexas. A primeira view junta pedidos com clientes. A segunda calcula margem de lucro. A terceira agrega vendas mensais. Todas podem ser consultadas como tabelas simples.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "View Indexada para Performance",
                    Code = @"-- Criar view para relatório de vendas por produto
CREATE VIEW vw_VendasPorProduto
WITH SCHEMABINDING -- Necessário para view indexada
AS
SELECT 
    P.ProdutoID,
    P.Nome,
    P.Categoria,
    SUM(PI.Quantidade) AS TotalVendido,
    SUM(PI.Quantidade * PI.PrecoUnitario) AS ValorTotalVendas,
    COUNT_BIG(*) AS NumeroVendas -- COUNT_BIG necessário
FROM dbo.Produtos P
INNER JOIN dbo.PedidoItens PI ON P.ProdutoID = PI.ProdutoID
GROUP BY P.ProdutoID, P.Nome, P.Categoria;

-- Criar índice clustered para materializar a view
CREATE UNIQUE CLUSTERED INDEX idx_VendasPorProduto 
ON vw_VendasPorProduto(ProdutoID);

-- Agora consultas nesta view são extremamente rápidas
SELECT * FROM vw_VendasPorProduto 
WHERE Categoria = 'Eletrônicos'
ORDER BY ValorTotalVendas DESC;

-- A view é atualizada automaticamente quando dados mudam",
                    Language = "sql",
                    Explanation = "View indexada materializa resultados para performance. SCHEMABINDING vincula view às tabelas. Índice clustered único torna a view física. Consultas são muito mais rápidas, mas INSERT/UPDATE nas tabelas base têm overhead.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Criar View Simples",
                    Description = "Crie uma view vw_ClientesAtivos que mostra apenas clientes ativos com colunas ClienteID, Nome, Email e DataCadastro.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"CREATE VIEW vw_ClientesAtivos AS
-- Complete aqui
;",
                    Hints = new List<string>
                    {
                        "SELECT ClienteID, Nome, Email, DataCadastro",
                        "FROM Clientes",
                        "WHERE Ativo = 1"
                    }
                },
                new Exercise
                {
                    Title = "View com Joins",
                    Description = "Crie vw_ProdutosComCategoria que junta Produtos com Categorias, mostrando ProdutoID, NomeProduto, NomeCategoria, Preco e Estoque.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"CREATE VIEW vw_ProdutosComCategoria AS
SELECT 
    -- Complete aqui
FROM Produtos P
INNER JOIN 
;",
                    Hints = new List<string>
                    {
                        "INNER JOIN Categorias C ON P.CategoriaID = C.CategoriaID",
                        "Use aliases para nomes de colunas claros",
                        "P.Nome AS NomeProduto, C.Nome AS NomeCategoria"
                    }
                },
                new Exercise
                {
                    Title = "View Agregada",
                    Description = "Crie vw_TopClientesPorGasto que mostra os 100 clientes que mais gastaram, com ClienteID, Nome, TotalPedidos e ValorTotalGasto.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"CREATE VIEW vw_TopClientesPorGasto AS
SELECT TOP 100
    -- Complete aqui
FROM Clientes C
INNER JOIN 
GROUP BY 
ORDER BY ;",
                    Hints = new List<string>
                    {
                        "JOIN Pedidos P ON C.ClienteID = P.ClienteID",
                        "COUNT(P.PedidoID) AS TotalPedidos",
                        "SUM(P.ValorTotal) AS ValorTotalGasto",
                        "GROUP BY C.ClienteID, C.Nome",
                        "ORDER BY ValorTotalGasto DESC"
                    }
                }
            },
            Summary = "Nesta aula você aprendeu sobre views, consultas salvas que funcionam como tabelas virtuais. Você viu como criar views para simplificar consultas complexas, melhorar segurança, e facilitar manutenção. Views indexadas materializam resultados para máxima performance. Views são ferramentas essenciais para organizar e otimizar acesso a dados. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno. Grandes empresas como Microsoft, Google e Amazon aplicam estes conceitos em seus sistemas. Ao aprender estas técnicas, você está adquirindo conhecimento usado por desenvolvedores em todo o mundo. Considere como este conceito poderia ser aplicado em um projeto pessoal. Pensar em aplicações práticas ajuda a solidificar o entendimento e torna o aprendizado mais significativo. Iniciantes frequentemente cometem alguns erros ao aprender este tópico. Conhecer estes erros comuns ajuda a evitá-los e desenvolver código mais robusto desde o início. Um erro comum é não considerar casos extremos ou situações inesperadas. Sempre teste seu código com diferentes tipos de entrada, incluindo valores nulos, vazios ou inválidos. Outro erro frequente é otimizar prematuramente. Primeiro faça o código funcionar corretamente, depois otimize se necessário. Como diz o ditado: 'premature optimization is the root of all evil'. Quando encontrar problemas, use o debugger para entender o que está acontecendo. Coloque breakpoints, inspecione variáveis e execute o código passo a passo. Esta é uma habilidade essencial para todo desenvolvedor. Mensagens de erro podem parecer intimidadoras no início, mas elas são suas aliadas. Leia-as cuidadosamente - elas geralmente indicam exatamente onde e qual é o problema. Se algo não funcionar como esperado, não desanime. Debugging é uma parte normal do desenvolvimento. Até desenvolvedores experientes passam tempo significativo depurando código. Testar seu código é tão importante quanto escrevê-lo. Testes automatizados garantem que seu código funciona corretamente e continuará funcionando quando você fizer mudanças. Comece escrevendo testes simples para os casos mais comuns. Depois adicione testes para casos extremos e situações de erro. Uma boa cobertura de testes dá confiança para refatorar e melhorar o código. Considere a prática de TDD (Test-Driven Development), onde você escreve os testes antes do código. Isso ajuda a pensar claramente sobre o que o código deve fazer. Embora a correção seja sempre a prioridade, é importante estar ciente das implicações de performance de suas escolhas. Diferentes abordagens podem ter custos computacionais muito diferentes. Em muitos casos, a diferença de performance é negligenciável para aplicações pequenas. No entanto, à medida que seus programas crescem, escolhas eficientes se tornam cada vez mais importantes. Aprenda a usar ferramentas de profiling para identificar gargalos de performance. Otimize baseado em dados reais, não em suposições sobre onde o código é lento. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0004-00000000000f"),
            CourseId = _courseId,
            Title = "Views e Consultas Salvas",
            Duration = "55 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 55,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0004-00000000000e" }),
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
                "Compreender triggers e seus usos",
                "Criar triggers para auditoria e validação",
                "Entender INSTEAD OF triggers",
                "Aplicar triggers com responsabilidade"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "O que são Triggers?",
                    Content = "Triggers são procedimentos especiais que executam automaticamente em resposta a eventos de banco de dados como INSERT, UPDATE ou DELETE. Triggers permitem implementar lógica de negócio, validações, auditoria e manutenção de integridade referencial customizada. Existem dois tipos principais: AFTER triggers executam após a operação ser concluída, e INSTEAD OF triggers substituem a operação original. Triggers têm acesso a tabelas especiais: INSERTED contém linhas novas (INSERT) ou atualizadas (UPDATE), DELETED contém linhas antigas (DELETE) ou antes da atualização (UPDATE). Triggers são úteis para: auditoria automática registrando quem mudou o quê e quando, validações complexas que não podem ser expressas com constraints, cálculos derivados como atualizar totais quando itens mudam, e sincronização de dados entre tabelas. No entanto, triggers têm desvantagens: podem impactar performance significativamente, tornam o comportamento do banco menos previsível, dificultam debug, e podem criar dependências ocultas. Use triggers com moderação e documente-os claramente. Considere alternativas como constraints, computed columns, ou lógica na aplicação antes de usar triggers.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Criando Triggers para Auditoria",
                    Content = "Auditoria é um dos usos mais comuns de triggers. Um trigger de auditoria registra mudanças em uma tabela de log sempre que dados são modificados. Por exemplo, quando um produto é atualizado, o trigger registra o produto ID, valores antigos e novos, usuário que fez a mudança, e timestamp. Isso cria trilha de auditoria completa para compliance e troubleshooting. Para implementar auditoria, crie uma tabela de log com colunas para tabela afetada, operação (INSERT/UPDATE/DELETE), valores antigos e novos (pode usar JSON ou XML), usuário (SYSTEM_USER), e data/hora. Crie triggers AFTER INSERT, AFTER UPDATE, e AFTER DELETE na tabela auditada. No trigger, insira registro na tabela de log com informações de INSERTED e DELETED. Para UPDATE, DELETED contém valores antigos e INSERTED contém novos. Auditoria com triggers é transparente para aplicação - não requer mudanças de código. No entanto, pode gerar grande volume de dados de log. Implemente políticas de retenção para arquivar ou deletar logs antigos. Considere impacto de performance - triggers de auditoria adicionam overhead a cada operação de escrita.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "INSTEAD OF Triggers e Validações",
                    Content = "INSTEAD OF triggers substituem completamente a operação original, dando controle total sobre o que acontece. São úteis para views complexas que normalmente não seriam atualizáveis, permitindo definir como INSERT/UPDATE/DELETE devem ser tratados. INSTEAD OF triggers também permitem validações complexas que rejeitam operações inválidas. Por exemplo, um INSTEAD OF DELETE trigger pode implementar soft delete, marcando registros como inativos em vez de deletá-los fisicamente. INSTEAD OF UPDATE pode validar regras de negócio complexas e rejeitar atualizações inválidas com RAISERROR. Ao usar INSTEAD OF triggers, você é responsável por executar a operação desejada - o trigger substitui completamente o comportamento padrão. Isso oferece flexibilidade máxima mas requer cuidado. Triggers podem chamar stored procedures para lógica complexa, mantendo o trigger simples. Evite lógica de negócio complexa diretamente em triggers - use-os como pontos de integração que chamam código bem testado. Triggers não devem fazer operações longas ou acessar recursos externos - isso bloqueia transações. Mantenha triggers rápidos e focados.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Trigger de Auditoria",
                    Code = @"-- Criar tabela de auditoria
CREATE TABLE ProdutosAuditoria (
    AuditoriaID INT PRIMARY KEY IDENTITY,
    ProdutoID INT,
    Operacao VARCHAR(10),
    ValorAntigo NVARCHAR(MAX),
    ValorNovo NVARCHAR(MAX),
    Usuario NVARCHAR(100),
    DataHora DATETIME DEFAULT GETDATE()
);

-- Trigger para auditar atualizações em Produtos
CREATE TRIGGER trg_Produtos_Auditoria
ON Produtos
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO ProdutosAuditoria (ProdutoID, Operacao, ValorAntigo, ValorNovo, Usuario)
    SELECT 
        i.ProdutoID,
        'UPDATE',
        (SELECT d.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
        (SELECT i.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
        SYSTEM_USER
    FROM INSERTED i
    INNER JOIN DELETED d ON i.ProdutoID = d.ProdutoID;
END;

-- Agora toda atualização em Produtos é automaticamente auditada
UPDATE Produtos SET Preco = 150.00 WHERE ProdutoID = 1;

-- Consultar auditoria
SELECT * FROM ProdutosAuditoria WHERE ProdutoID = 1;",
                    Language = "sql",
                    Explanation = "Este trigger registra automaticamente todas as atualizações em Produtos. Usa JSON para armazenar valores antigos e novos. INSERTED contém novos valores, DELETED contém antigos. SYSTEM_USER captura quem fez a mudança.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "INSTEAD OF Trigger para Soft Delete",
                    Code = @"-- Implementar soft delete com trigger
CREATE TRIGGER trg_Clientes_SoftDelete
ON Clientes
INSTEAD OF DELETE
AS
BEGIN
    SET NOCOUNT ON;

    -- Em vez de deletar, marcar como inativo
    UPDATE Clientes
    SET Ativo = 0, DataExclusao = GETDATE()
    WHERE ClienteID IN (SELECT ClienteID FROM DELETED);

    -- Registrar na auditoria
    INSERT INTO ClientesAuditoria (ClienteID, Operacao, Usuario)
    SELECT ClienteID, 'SOFT_DELETE', SYSTEM_USER
    FROM DELETED;
END;

-- Agora DELETE não remove fisicamente
DELETE FROM Clientes WHERE ClienteID = 10;
-- Cliente 10 ainda existe mas Ativo = 0

-- Trigger de validação
CREATE TRIGGER trg_Produtos_ValidarPreco
ON Produtos
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM INSERTED WHERE Preco < 0)
    BEGIN
        RAISERROR('Preço não pode ser negativo', 16, 1);
        ROLLBACK TRANSACTION;
    END;

    IF EXISTS (SELECT 1 FROM INSERTED WHERE Preco > 1000000)
    BEGIN
        RAISERROR('Preço excede limite máximo', 16, 1);
        ROLLBACK TRANSACTION;
    END;
END;",
                    Language = "sql",
                    Explanation = "INSTEAD OF trigger substitui DELETE por soft delete. Marca registros como inativos em vez de deletar. O segundo trigger valida preços, rejeitando valores inválidos com RAISERROR e ROLLBACK.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Trigger de Auditoria Simples",
                    Description = "Crie um trigger que registra em uma tabela ClientesLog toda vez que um cliente é inserido, armazenando ClienteID, Nome, Usuario e DataHora.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"CREATE TABLE ClientesLog (
    LogID INT PRIMARY KEY IDENTITY,
    ClienteID INT,
    Nome NVARCHAR(100),
    Usuario NVARCHAR(100),
    DataHora DATETIME DEFAULT GETDATE()
);

CREATE TRIGGER trg_Clientes_LogInsert
ON Clientes
AFTER INSERT
AS
BEGIN
    -- Complete aqui
END;",
                    Hints = new List<string>
                    {
                        "INSERT INTO ClientesLog (ClienteID, Nome, Usuario)",
                        "SELECT ClienteID, Nome, SYSTEM_USER FROM INSERTED",
                        "INSERTED contém as linhas inseridas"
                    }
                },
                new Exercise
                {
                    Title = "Trigger para Atualizar Total",
                    Description = "Crie um trigger em PedidoItens que atualiza automaticamente ValorTotal em Pedidos sempre que itens são inseridos, atualizados ou deletados.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"CREATE TRIGGER trg_PedidoItens_AtualizarTotal
ON PedidoItens
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    -- Complete aqui
END;",
                    Hints = new List<string>
                    {
                        "UPDATE Pedidos SET ValorTotal = (SELECT SUM(...))",
                        "WHERE PedidoID IN (SELECT PedidoID FROM INSERTED UNION SELECT PedidoID FROM DELETED)",
                        "Calcule SUM(Quantidade * PrecoUnitario) de PedidoItens",
                        "Use COALESCE para tratar NULL"
                    }
                },
                new Exercise
                {
                    Title = "INSTEAD OF Trigger para Validação",
                    Description = "Crie um INSTEAD OF UPDATE trigger em Produtos que impede atualizar o preço em mais de 50% de uma vez, rejeitando a operação se a mudança for muito grande.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"CREATE TRIGGER trg_Produtos_ValidarMudancaPreco
ON Produtos
INSTEAD OF UPDATE
AS
BEGIN
    -- Validar mudança de preço
    -- Se válido, executar UPDATE
    -- Se inválido, RAISERROR e ROLLBACK
END;",
                    Hints = new List<string>
                    {
                        "Compare i.Preco com d.Preco (INSERTED vs DELETED)",
                        "Calcule percentual de mudança: ABS(i.Preco - d.Preco) / d.Preco",
                        "IF percentual > 0.5 THEN RAISERROR",
                        "ELSE executar UPDATE normalmente"
                    }
                }
            },
            Summary = "Nesta aula você aprendeu sobre triggers, procedimentos que executam automaticamente em resposta a eventos. Você viu triggers AFTER para auditoria e INSTEAD OF para controle total. Triggers são poderosos mas devem ser usados com moderação devido ao impacto em performance e complexidade. Use-os para auditoria, validações complexas e manutenção de integridade. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno. Grandes empresas como Microsoft, Google e Amazon aplicam estes conceitos em seus sistemas. Ao aprender estas técnicas, você está adquirindo conhecimento usado por desenvolvedores em todo o mundo. Considere como este conceito poderia ser aplicado em um projeto pessoal. Pensar em aplicações práticas ajuda a solidificar o entendimento e torna o aprendizado mais significativo. Iniciantes frequentemente cometem alguns erros ao aprender este tópico. Conhecer estes erros comuns ajuda a evitá-los e desenvolver código mais robusto desde o início. Um erro comum é não considerar casos extremos ou situações inesperadas. Sempre teste seu código com diferentes tipos de entrada, incluindo valores nulos, vazios ou inválidos. Outro erro frequente é otimizar prematuramente. Primeiro faça o código funcionar corretamente, depois otimize se necessário. Como diz o ditado: 'premature optimization is the root of all evil'. Quando encontrar problemas, use o debugger para entender o que está acontecendo. Coloque breakpoints, inspecione variáveis e execute o código passo a passo. Esta é uma habilidade essencial para todo desenvolvedor. Mensagens de erro podem parecer intimidadoras no início, mas elas são suas aliadas. Leia-as cuidadosamente - elas geralmente indicam exatamente onde e qual é o problema. Se algo não funcionar como esperado, não desanime. Debugging é uma parte normal do desenvolvimento. Até desenvolvedores experientes passam tempo significativo depurando código. Testar seu código é tão importante quanto escrevê-lo. Testes automatizados garantem que seu código funciona corretamente e continuará funcionando quando você fizer mudanças. Comece escrevendo testes simples para os casos mais comuns. Depois adicione testes para casos extremos e situações de erro. Uma boa cobertura de testes dá confiança para refatorar e melhorar o código. Considere a prática de TDD (Test-Driven Development), onde você escreve os testes antes do código. Isso ajuda a pensar claramente sobre o que o código deve fazer. Embora a correção seja sempre a prioridade, é importante estar ciente das implicações de performance de suas escolhas. Diferentes abordagens podem ter custos computacionais muito diferentes. Em muitos casos, a diferença de performance é negligenciável para aplicações pequenas. No entanto, à medida que seus programas crescem, escolhas eficientes se tornam cada vez mais importantes. Aprenda a usar ferramentas de profiling para identificar gargalos de performance. Otimize baseado em dados reais, não em suposições sobre onde o código é lento. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0004-000000000010"),
            CourseId = _courseId,
            Title = "Triggers e Automação",
            Duration = "60 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 60,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0004-00000000000f" }),
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
                "Compreender otimização de consultas",
                "Analisar planos de execução",
                "Identificar e resolver gargalos de performance",
                "Aplicar técnicas de otimização"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Fundamentos de Otimização",
                    Content = "Otimização de consultas é crucial para aplicações performáticas. Uma consulta mal escrita pode levar segundos ou minutos quando deveria levar milissegundos. O otimizador de consultas do SQL Server analisa consultas e escolhe o melhor plano de execução, mas você pode ajudá-lo escrevendo SQL eficiente. Princípios básicos: use índices apropriados, evite SELECT *, filtre dados o mais cedo possível com WHERE, use EXISTS em vez de IN para subconsultas, evite funções em colunas de WHERE (impede uso de índices), e minimize operações em loops. Table scans (ler toda a tabela) são lentos em tabelas grandes - índices permitem index seeks (busca direta). Joins devem usar colunas indexadas. ORDER BY e GROUP BY beneficiam de índices. Estatísticas mantêm o otimizador informado sobre distribuição de dados - atualize-as regularmente. Fragmentação de índices degrada performance - reconstrua índices periodicamente. Use paginação para grandes result sets em vez de retornar milhares de linhas. Considere caching na aplicação para dados consultados frequentemente. Performance é sobre fazer menos trabalho: menos linhas lidas, menos joins, menos ordenação. Meça antes de otimizar - use ferramentas de profiling para identificar consultas lentas.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Analisando Planos de Execução",
                    Content = "Planos de execução mostram como o SQL Server executa uma consulta, revelando gargalos de performance. No SQL Server Management Studio, ative 'Include Actual Execution Plan' antes de executar consultas. O plano mostra operações como Table Scan, Index Seek, Index Scan, Nested Loops, Hash Match, e Sort. Table Scan é geralmente ruim em tabelas grandes - indica falta de índice. Index Seek é bom - busca direta usando índice. Index Scan lê todo o índice, melhor que Table Scan mas pior que Seek. Nested Loops é eficiente para pequenos conjuntos de dados. Hash Match é usado para joins grandes. Sort é custoso - evite quando possível usando índices. Percentuais no plano indicam custo relativo de cada operação - foque em operações com maior custo. Warnings (triângulos amarelos) indicam problemas como falta de estatísticas ou conversões implícitas. Conversões implícitas (CONVERT_IMPLICIT) impedem uso de índices. Missing Index suggestions sugerem índices que melhorariam a consulta. Analise planos de consultas lentas para identificar problemas. Compare planos antes e depois de otimizações para verificar melhoria. Planos de execução são essenciais para otimização baseada em dados, não suposições.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Técnicas Avançadas de Otimização",
                    Content = "Técnicas avançadas incluem: query hints para forçar comportamentos específicos (use com cuidado), particionamento de tabelas grandes para melhorar manutenção e consultas, covering indexes que incluem todas as colunas necessárias eliminando acesso à tabela, filtered indexes para subconjuntos de dados, e columnstore indexes para consultas analíticas. Desnormalização controlada pode melhorar leitura às custas de complexidade de escrita. Materialized views (views indexadas) pré-calculam resultados caros. Batch operations são mais eficientes que operações individuais - use Table-Valued Parameters para passar múltiplas linhas. Evite cursores - operações baseadas em conjuntos são muito mais rápidas. Use CTEs (Common Table Expressions) e window functions para consultas complexas legíveis e eficientes. Considere read-only replicas para separar carga de leitura de escrita. Implemente caching na aplicação com Redis ou MemoryCache para dados consultados frequentemente. Use async/await em C# para não bloquear threads durante operações de banco. Connection pooling reutiliza conexões eficientemente. Monitore performance continuamente com Extended Events ou Query Store. Estabeleça baselines de performance e alertas para degradação. Otimização é processo contínuo - dados crescem, padrões de uso mudam, e consultas precisam ser revisitadas.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Consultas Otimizadas vs Não Otimizadas",
                    Code = @"-- ❌ NÃO OTIMIZADO: SELECT *, função em WHERE, sem índice
SELECT * FROM Pedidos 
WHERE YEAR(DataPedido) = 2024;
-- Problema: função em coluna impede uso de índice, SELECT * traz dados desnecessários

-- ✅ OTIMIZADO: colunas específicas, filtro sem função
SELECT PedidoID, ClienteID, DataPedido, ValorTotal 
FROM Pedidos 
WHERE DataPedido >= '2024-01-01' AND DataPedido < '2025-01-01';
-- Benefício: pode usar índice em DataPedido, retorna apenas colunas necessárias

-- ❌ NÃO OTIMIZADO: IN com subconsulta
SELECT * FROM Produtos 
WHERE ProdutoID IN (SELECT ProdutoID FROM PedidoItens WHERE Quantidade > 10);

-- ✅ OTIMIZADO: EXISTS é mais eficiente
SELECT P.* FROM Produtos P
WHERE EXISTS (SELECT 1 FROM PedidoItens PI 
              WHERE PI.ProdutoID = P.ProdutoID AND PI.Quantidade > 10);

-- ❌ NÃO OTIMIZADO: múltiplas consultas em loop (N+1 problem)
-- foreach (var pedido in pedidos) {
--     var cliente = db.Query(""SELECT * FROM Clientes WHERE ClienteID = @Id"", pedido.ClienteID);
-- }

-- ✅ OTIMIZADO: uma consulta com JOIN
SELECT P.*, C.Nome, C.Email 
FROM Pedidos P
INNER JOIN Clientes C ON P.ClienteID = C.ClienteID;",
                    Language = "sql",
                    Explanation = "Exemplos mostram consultas não otimizadas e suas versões otimizadas. Evite funções em WHERE, use EXISTS em vez de IN, selecione apenas colunas necessárias, e evite N+1 queries com JOINs.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Analisando e Otimizando com Plano de Execução",
                    Code = @"-- Habilitar plano de execução
SET STATISTICS IO ON;
SET STATISTICS TIME ON;

-- Consulta para análise
SELECT C.Nome, COUNT(P.PedidoID) AS TotalPedidos, SUM(P.ValorTotal) AS TotalGasto
FROM Clientes C
LEFT JOIN Pedidos P ON C.ClienteID = P.ClienteID
WHERE C.Cidade = 'São Paulo'
GROUP BY C.ClienteID, C.Nome
HAVING SUM(P.ValorTotal) > 1000
ORDER BY TotalGasto DESC;

-- Verificar estatísticas
-- Logical reads: quantas páginas lidas (menor é melhor)
-- CPU time: tempo de CPU (menor é melhor)

-- Criar índice sugerido pelo plano
CREATE INDEX IX_Clientes_Cidade ON Clientes(Cidade) INCLUDE (ClienteID, Nome);
CREATE INDEX IX_Pedidos_ClienteID_ValorTotal ON Pedidos(ClienteID) INCLUDE (ValorTotal);

-- Re-executar e comparar
-- Deve ver redução dramática em logical reads e tempo

-- Verificar fragmentação de índices
SELECT 
    OBJECT_NAME(ips.object_id) AS TableName,
    i.name AS IndexName,
    ips.avg_fragmentation_in_percent
FROM sys.dm_db_index_physical_stats(DB_ID(), NULL, NULL, NULL, 'LIMITED') ips
INNER JOIN sys.indexes i ON ips.object_id = i.object_id AND ips.index_id = i.index_id
WHERE ips.avg_fragmentation_in_percent > 30
ORDER BY ips.avg_fragmentation_in_percent DESC;

-- Reconstruir índices fragmentados
ALTER INDEX ALL ON Pedidos REBUILD;",
                    Language = "sql",
                    Explanation = "STATISTICS IO/TIME mostram métricas de performance. Plano de execução revela gargalos. Criar índices apropriados melhora drasticamente performance. Monitorar e reconstruir índices fragmentados mantém performance.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Otimizar Consulta",
                    Description = "Otimize esta consulta: SELECT * FROM Produtos WHERE UPPER(Nome) LIKE '%NOTEBOOK%' ORDER BY Preco. Remova função em WHERE e SELECT *.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"-- Consulta original (lenta)
SELECT * FROM Produtos 
WHERE UPPER(Nome) LIKE '%NOTEBOOK%' 
ORDER BY Preco;

-- Versão otimizada
-- Complete aqui",
                    Hints = new List<string>
                    {
                        "Remova UPPER() - use coluna diretamente",
                        "Selecione apenas colunas necessárias",
                        "Considere criar índice em Nome",
                        "LIKE '%texto%' não pode usar índice, mas é melhor que função"
                    }
                },
                new Exercise
                {
                    Title = "Substituir IN por EXISTS",
                    Description = "Reescreva usando EXISTS: SELECT * FROM Clientes WHERE ClienteID IN (SELECT ClienteID FROM Pedidos WHERE ValorTotal > 1000).",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"-- Versão com IN
SELECT * FROM Clientes 
WHERE ClienteID IN (SELECT ClienteID FROM Pedidos WHERE ValorTotal > 1000);

-- Reescreva com EXISTS
-- Complete aqui",
                    Hints = new List<string>
                    {
                        "WHERE EXISTS (SELECT 1 FROM Pedidos P WHERE...)",
                        "P.ClienteID = C.ClienteID AND P.ValorTotal > 1000",
                        "EXISTS para assim que encontra correspondência",
                        "Mais eficiente que IN para grandes conjuntos"
                    }
                },
                new Exercise
                {
                    Title = "Criar Covering Index",
                    Description = "Analise esta consulta e crie um covering index que elimine acesso à tabela: SELECT Nome, Preco, Estoque FROM Produtos WHERE Categoria = 'Eletrônicos' ORDER BY Preco.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"-- Consulta que precisa de otimização
SELECT Nome, Preco, Estoque 
FROM Produtos 
WHERE Categoria = 'Eletrônicos' 
ORDER BY Preco;

-- Criar covering index
CREATE INDEX 
-- Complete aqui",
                    Hints = new List<string>
                    {
                        "Coluna de filtro (Categoria) deve estar no índice",
                        "Coluna de ordenação (Preco) deve estar no índice",
                        "Outras colunas (Nome, Estoque) em INCLUDE",
                        "CREATE INDEX IX_Produtos_Categoria_Preco ON Produtos(Categoria, Preco) INCLUDE (Nome, Estoque)"
                    }
                }
            },
            Summary = "Nesta aula você aprendeu técnicas de otimização de consultas, como analisar planos de execução para identificar gargalos, e aplicar otimizações como índices apropriados, evitar funções em WHERE, e usar EXISTS em vez de IN. Otimização é essencial para aplicações performáticas com grandes volumes de dados. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno. Grandes empresas como Microsoft, Google e Amazon aplicam estes conceitos em seus sistemas. Ao aprender estas técnicas, você está adquirindo conhecimento usado por desenvolvedores em todo o mundo. Considere como este conceito poderia ser aplicado em um projeto pessoal. Pensar em aplicações práticas ajuda a solidificar o entendimento e torna o aprendizado mais significativo. Iniciantes frequentemente cometem alguns erros ao aprender este tópico. Conhecer estes erros comuns ajuda a evitá-los e desenvolver código mais robusto desde o início. Um erro comum é não considerar casos extremos ou situações inesperadas. Sempre teste seu código com diferentes tipos de entrada, incluindo valores nulos, vazios ou inválidos. Outro erro frequente é otimizar prematuramente. Primeiro faça o código funcionar corretamente, depois otimize se necessário. Como diz o ditado: 'premature optimization is the root of all evil'. Quando encontrar problemas, use o debugger para entender o que está acontecendo. Coloque breakpoints, inspecione variáveis e execute o código passo a passo. Esta é uma habilidade essencial para todo desenvolvedor. Mensagens de erro podem parecer intimidadoras no início, mas elas são suas aliadas. Leia-as cuidadosamente - elas geralmente indicam exatamente onde e qual é o problema. Se algo não funcionar como esperado, não desanime. Debugging é uma parte normal do desenvolvimento. Até desenvolvedores experientes passam tempo significativo depurando código. Testar seu código é tão importante quanto escrevê-lo. Testes automatizados garantem que seu código funciona corretamente e continuará funcionando quando você fizer mudanças. Comece escrevendo testes simples para os casos mais comuns. Depois adicione testes para casos extremos e situações de erro. Uma boa cobertura de testes dá confiança para refatorar e melhorar o código. Considere a prática de TDD (Test-Driven Development), onde você escreve os testes antes do código. Isso ajuda a pensar claramente sobre o que o código deve fazer. Embora a correção seja sempre a prioridade, é importante estar ciente das implicações de performance de suas escolhas. Diferentes abordagens podem ter custos computacionais muito diferentes. Em muitos casos, a diferença de performance é negligenciável para aplicações pequenas. No entanto, à medida que seus programas crescem, escolhas eficientes se tornam cada vez mais importantes. Aprenda a usar ferramentas de profiling para identificar gargalos de performance. Otimize baseado em dados reais, não em suposições sobre onde o código é lento. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0004-000000000011"),
            CourseId = _courseId,
            Title = "Otimização de Consultas",
            Duration = "65 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 65,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0004-000000000010" }),
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
                "Entender conceitos de concorrência em bancos de dados",
                "Compreender níveis de isolamento de transações",
                "Identificar e resolver deadlocks",
                "Aplicar controle de concorrência otimista e pessimista"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Concorrência e Problemas Comuns",
                    Content = "Concorrência ocorre quando múltiplos usuários ou processos acessam o mesmo banco de dados simultaneamente. Sem controle adequado, isso pode causar problemas sérios. Dirty Read acontece quando uma transação lê dados modificados por outra transação que ainda não foi confirmada. Se a segunda transação for revertida, a primeira leu dados inválidos. Non-Repeatable Read ocorre quando uma transação lê o mesmo registro duas vezes e obtém valores diferentes porque outra transação modificou o registro entre as leituras. Phantom Read acontece quando uma transação executa a mesma consulta duas vezes e obtém conjuntos de resultados diferentes porque outra transação inseriu ou deletou registros. Lost Update ocorre quando duas transações leem o mesmo valor, ambas o modificam, e a última gravação sobrescreve a primeira, perdendo uma atualização. Esses problemas podem levar a inconsistências de dados, relatórios incorretos e bugs difíceis de reproduzir. Bancos de dados relacionais oferecem mecanismos de controle de concorrência para prevenir esses problemas, principalmente através de níveis de isolamento de transações e locks.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Níveis de Isolamento de Transações",
                    Content = "SQL Server oferece quatro níveis de isolamento que balanceiam consistência e performance. Read Uncommitted é o menos restritivo, permite dirty reads mas oferece máxima concorrência. Útil apenas para relatórios aproximados onde precisão não é crítica. Read Committed (padrão) previne dirty reads mas permite non-repeatable reads e phantom reads. Adequado para maioria das aplicações. Repeatable Read previne dirty reads e non-repeatable reads mas permite phantom reads. Usa locks de leitura até o fim da transação. Serializable é o mais restritivo, previne todos os problemas de concorrência mas reduz significativamente a concorrência. Transações são executadas como se fossem seriais. Snapshot Isolation usa versionamento de linhas em vez de locks, permitindo alta concorrência sem dirty reads, non-repeatable reads ou phantom reads. Leituras veem uma versão consistente dos dados no início da transação. A escolha do nível de isolamento depende dos requisitos de consistência da aplicação versus necessidade de performance e concorrência.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Deadlocks e Como Evitá-los",
                    Content = "Deadlock ocorre quando duas ou mais transações esperam indefinidamente umas pelas outras para liberar locks. Por exemplo, Transação A trava Tabela 1 e espera Tabela 2, enquanto Transação B trava Tabela 2 e espera Tabela 1. SQL Server detecta deadlocks automaticamente e escolhe uma transação como vítima, revertendo-a para quebrar o impasse. Para minimizar deadlocks: acesse objetos sempre na mesma ordem em todas as transações, mantenha transações curtas e rápidas, use o menor nível de isolamento possível, considere usar NOLOCK hints para leituras que toleram dados ligeiramente desatualizados, e implemente retry logic para transações que podem ser vítimas de deadlock. Monitore deadlocks usando SQL Server Profiler ou Extended Events. Analise deadlock graphs para identificar padrões. Em aplicações de alta concorrência, deadlocks ocasionais são normais, mas deadlocks frequentes indicam problemas de design que devem ser corrigidos. Considere redesenhar transações ou usar controle de concorrência otimista.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Definindo Nível de Isolamento",
                    Code = @"-- Read Committed (padrão)
SET TRANSACTION ISOLATION LEVEL READ COMMITTED;
BEGIN TRANSACTION;
SELECT * FROM Produtos WHERE ProdutoID = 1;
-- Outras operações
COMMIT;

-- Snapshot Isolation (requer habilitação no banco)
ALTER DATABASE MeuBanco SET ALLOW_SNAPSHOT_ISOLATION ON;

SET TRANSACTION ISOLATION LEVEL SNAPSHOT;
BEGIN TRANSACTION;
SELECT * FROM Produtos WHERE ProdutoID = 1;
UPDATE Produtos SET Estoque = Estoque - 1 WHERE ProdutoID = 1;
COMMIT;",
                    Language = "sql",
                    Explanation = "Diferentes níveis de isolamento oferecem diferentes garantias de consistência. Snapshot Isolation é ideal para aplicações com alta concorrência de leitura.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Controle de Concorrência Otimista em C#",
                    Code = @"using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;

public class Produto
{
    public int ProdutoID { get; set; }
    public string Nome { get; set; } = string.Empty;
    public int Estoque { get; set; }
    public byte[] RowVersion { get; set; } = Array.Empty<byte>(); // Timestamp para controle de concorrência
}

public class ProdutoService
{
    private readonly string connectionString;

    public ProdutoService(string connString)
    {
        connectionString = connString;
    }

    public async Task<bool> AtualizarEstoqueOtimista(int produtoId, int quantidade)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            var produto = await connection.QuerySingleAsync<Produto>(
                ""SELECT * FROM Produtos WHERE ProdutoID = @Id"", 
                new { Id = produtoId });
            
            var linhasAfetadas = await connection.ExecuteAsync(@""
                UPDATE Produtos 
                SET Estoque = @NovoEstoque 
                WHERE ProdutoID = @Id AND RowVersion = @RowVersion"",
                new { 
                    NovoEstoque = produto.Estoque - quantidade,
                    Id = produtoId,
                    RowVersion = produto.RowVersion 
                });
            
            return linhasAfetadas > 0; // False se houve conflito
        }
    }
}",
                    Language = "csharp",
                    Explanation = "Controle otimista usa uma coluna de versão para detectar conflitos. Se RowVersion mudou entre leitura e atualização, outro usuário modificou o registro.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Identifique o Problema de Concorrência",
                    Description = "Para cada cenário, identifique qual problema de concorrência pode ocorrer: (a) Duas transações leem saldo de conta, ambas adicionam 100, ambas gravam. (b) Transação A lê registro, Transação B modifica e confirma, Transação A lê novamente. (c) Transação A lê registros com WHERE Preco > 100, Transação B insere novo registro com Preco = 150, Transação A lê novamente.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Analise cada cenário:
// (a) Problema: 
// (b) Problema: 
// (c) Problema: ",
                    Hints = new List<string>
                    {
                        "(a) é Lost Update - última gravação sobrescreve a primeira",
                        "(b) é Non-Repeatable Read - mesma leitura retorna valores diferentes",
                        "(c) é Phantom Read - mesma consulta retorna registros diferentes"
                    }
                },
                new Exercise
                {
                    Title = "Escolha o Nível de Isolamento",
                    Description = "Para uma aplicação bancária que processa transferências entre contas, qual nível de isolamento você recomendaria e por quê?",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Nível recomendado: 
// Justificativa: ",
                    Hints = new List<string>
                    {
                        "Transferências bancárias exigem alta consistência",
                        "Não pode haver lost updates ou dirty reads",
                        "Considere Serializable ou Snapshot Isolation",
                        "Snapshot oferece melhor performance com mesma consistência"
                    }
                },
                new Exercise
                {
                    Title = "Implemente Retry Logic para Deadlock",
                    Description = "Escreva código C# que tenta executar uma transação e, se encontrar deadlock (erro 1205), tenta novamente até 3 vezes com delay exponencial.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"public async Task ExecutarComRetry(Func<Task> operacao)
{
    int tentativas = 0;
    int maxTentativas = 3;
    
    while (tentativas < maxTentativas)
    {
        try
        {
            // Complete aqui
        }
        catch (SqlException ex) when (ex.Number == 1205)
        {
            // Complete aqui
        }
    }
}",
                    Hints = new List<string>
                    {
                        "Use await operacao() para executar a operação",
                        "Incremente tentativas no catch",
                        "Use await Task.Delay(Math.Pow(2, tentativas) * 100) para delay exponencial",
                        "Se atingir maxTentativas, relance a exceção"
                    }
                }
            },
            Summary = "Nesta aula você aprendeu sobre concorrência em bancos de dados, os problemas que podem ocorrer (dirty read, non-repeatable read, phantom read, lost update), os níveis de isolamento de transações disponíveis, e como identificar e prevenir deadlocks. Você também viu controle de concorrência otimista usando versionamento de linhas. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno. Grandes empresas como Microsoft, Google e Amazon aplicam estes conceitos em seus sistemas. Ao aprender estas técnicas, você está adquirindo conhecimento usado por desenvolvedores em todo o mundo. Considere como este conceito poderia ser aplicado em um projeto pessoal. Pensar em aplicações práticas ajuda a solidificar o entendimento e torna o aprendizado mais significativo. Iniciantes frequentemente cometem alguns erros ao aprender este tópico. Conhecer estes erros comuns ajuda a evitá-los e desenvolver código mais robusto desde o início. Um erro comum é não considerar casos extremos ou situações inesperadas. Sempre teste seu código com diferentes tipos de entrada, incluindo valores nulos, vazios ou inválidos. Outro erro frequente é otimizar prematuramente. Primeiro faça o código funcionar corretamente, depois otimize se necessário. Como diz o ditado: 'premature optimization is the root of all evil'. Quando encontrar problemas, use o debugger para entender o que está acontecendo. Coloque breakpoints, inspecione variáveis e execute o código passo a passo. Esta é uma habilidade essencial para todo desenvolvedor. Mensagens de erro podem parecer intimidadoras no início, mas elas são suas aliadas. Leia-as cuidadosamente - elas geralmente indicam exatamente onde e qual é o problema. Se algo não funcionar como esperado, não desanime. Debugging é uma parte normal do desenvolvimento. Até desenvolvedores experientes passam tempo significativo depurando código. Testar seu código é tão importante quanto escrevê-lo. Testes automatizados garantem que seu código funciona corretamente e continuará funcionando quando você fizer mudanças. Comece escrevendo testes simples para os casos mais comuns. Depois adicione testes para casos extremos e situações de erro. Uma boa cobertura de testes dá confiança para refatorar e melhorar o código. Considere a prática de TDD (Test-Driven Development), onde você escreve os testes antes do código. Isso ajuda a pensar claramente sobre o que o código deve fazer. Embora a correção seja sempre a prioridade, é importante estar ciente das implicações de performance de suas escolhas. Diferentes abordagens podem ter custos computacionais muito diferentes. Em muitos casos, a diferença de performance é negligenciável para aplicações pequenas. No entanto, à medida que seus programas crescem, escolhas eficientes se tornam cada vez mais importantes. Aprenda a usar ferramentas de profiling para identificar gargalos de performance. Otimize baseado em dados reais, não em suposições sobre onde o código é lento. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0004-000000000012"),
            CourseId = _courseId,
            Title = "Concorrência e Isolamento de Transações",
            Duration = "60 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 60,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0004-000000000011" }),
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
                "Compreender o que são stored procedures e suas vantagens",
                "Criar e executar stored procedures",
                "Usar parâmetros de entrada e saída",
                "Chamar stored procedures de C#"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "O que são Stored Procedures?",
                    Content = "Stored Procedures são blocos de código SQL armazenados no banco de dados que podem ser executados repetidamente. Pense nelas como funções ou métodos, mas no banco de dados. Elas oferecem várias vantagens: performance melhorada porque o plano de execução é compilado e cacheado, segurança aprimorada porque você pode dar permissão para executar a procedure sem dar acesso direto às tabelas, redução de tráfego de rede porque múltiplos comandos SQL são executados com uma única chamada, e manutenção centralizada porque lógica de negócio no banco pode ser atualizada sem modificar código da aplicação. Stored procedures podem aceitar parâmetros de entrada, retornar valores através de parâmetros de saída, e retornar conjuntos de resultados. Elas suportam lógica condicional (IF/ELSE), loops (WHILE), tratamento de erros (TRY/CATCH), e podem chamar outras procedures. No entanto, há desvantagens: lógica de negócio no banco é mais difícil de testar e versionar, procedures complexas podem ser difíceis de debugar, e migrar para outro banco de dados requer reescrever procedures devido a diferenças de sintaxe.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Criando e Executando Stored Procedures",
                    Content = "Para criar uma stored procedure, use CREATE PROCEDURE seguido do nome e corpo da procedure. Parâmetros são declarados com @ seguido do nome e tipo. Parâmetros de entrada são padrão, parâmetros de saída usam OUTPUT. Dentro da procedure, você pode usar qualquer comando SQL válido. Para executar, use EXEC ou EXECUTE seguido do nome e valores dos parâmetros. Procedures podem retornar valores de três formas: através de SELECT (retorna conjunto de resultados), através de parâmetros OUTPUT (retorna valores específicos), ou através de RETURN (retorna um código de status inteiro). Use ALTER PROCEDURE para modificar procedures existentes e DROP PROCEDURE para removê-las. Procedures podem incluir transações, garantindo que múltiplas operações sejam atômicas. Use BEGIN TRANSACTION, COMMIT e ROLLBACK dentro de procedures. TRY/CATCH permite tratamento de erros robusto. RAISERROR ou THROW podem sinalizar erros customizados. Procedures podem ser encadeadas, com uma procedure chamando outra, mas cuidado com profundidade excessiva que pode causar stack overflow.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Chamando Stored Procedures de C#",
                    Content = "Em C#, você pode chamar stored procedures usando ADO.NET ou Dapper. Com ADO.NET, crie um SqlCommand com CommandType.StoredProcedure e adicione parâmetros usando Parameters.Add. Para parâmetros de saída, defina Direction como ParameterDirection.Output. Execute com ExecuteNonQuery (sem retorno), ExecuteScalar (valor único), ou ExecuteReader (conjunto de resultados). Com Dapper, use connection.Execute para procedures sem retorno, connection.QuerySingle/QueryFirst para valor único, ou connection.Query para múltiplos resultados. Dapper simplifica o código mas ADO.NET oferece mais controle. Para parâmetros de saída com Dapper, use DynamicParameters. Sempre use parâmetros, nunca concatene valores em nomes de procedures. Procedures são excelentes para operações complexas que envolvem múltiplas tabelas ou lógica condicional. No entanto, para operações simples de CRUD, queries diretas ou ORMs como Entity Framework podem ser mais apropriados. A escolha entre procedures e queries depende dos requisitos de performance, segurança e manutenibilidade da aplicação.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Criando Stored Procedure com Parâmetros",
                    Code = @"-- Procedure para inserir produto e retornar ID
CREATE PROCEDURE sp_InserirProduto
    @Nome NVARCHAR(100),
    @Preco DECIMAL(10,2),
    @Estoque INT,
    @ProdutoID INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        INSERT INTO Produtos (Nome, Preco, Estoque, DataCadastro)
        VALUES (@Nome, @Preco, @Estoque, GETDATE());
        
        SET @ProdutoID = SCOPE_IDENTITY();
        
        COMMIT TRANSACTION;
        RETURN 0; -- Sucesso
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        
        RETURN -1; -- Erro
    END CATCH
END;

-- Executar a procedure
DECLARE @NovoID INT;
EXEC sp_InserirProduto 
    @Nome = 'Notebook', 
    @Preco = 2500.00, 
    @Estoque = 10,
    @ProdutoID = @NovoID OUTPUT;
SELECT @NovoID AS ProdutoInserido;",
                    Language = "sql",
                    Explanation = "Esta procedure insere um produto e retorna o ID gerado através de um parâmetro OUTPUT. Usa transação e tratamento de erros para garantir consistência.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Chamando Stored Procedure com ADO.NET",
                    Code = @"using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

public class ProdutoRepository
{
    private readonly string connectionString;

    public ProdutoRepository(string connString)
    {
        connectionString = connString;
    }

    public async Task<int> InserirProdutoComProcedure(string nome, decimal preco, int estoque)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            await connection.OpenAsync();
            
            using (var command = new SqlCommand(""sp_InserirProduto"", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                
                command.Parameters.AddWithValue(""@Nome"", nome);
                command.Parameters.AddWithValue(""@Preco"", preco);
                command.Parameters.AddWithValue(""@Estoque"", estoque);
                
                var outputParam = new SqlParameter(""@ProdutoID"", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(outputParam);
                
                await command.ExecuteNonQueryAsync();
                
                return (int)outputParam.Value;
            }
        }
    }
}",
                    Language = "csharp",
                    Explanation = "Este código chama a stored procedure usando ADO.NET, passando parâmetros de entrada e capturando o parâmetro de saída que contém o ID do produto inserido.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Chamando Stored Procedure com Dapper",
                    Code = @"using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;

public class ProdutoService
{
    private readonly string connectionString;

    public ProdutoService(string connString)
    {
        connectionString = connString;
    }

    public async Task<int> InserirProdutoComDapper(string nome, decimal preco, int estoque)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            var parameters = new DynamicParameters();
            parameters.Add(""@Nome"", nome);
            parameters.Add(""@Preco"", preco);
            parameters.Add(""@Estoque"", estoque);
            parameters.Add(""@ProdutoID"", dbType: DbType.Int32, direction: ParameterDirection.Output);
            
            await connection.ExecuteAsync(
                ""sp_InserirProduto"", 
                parameters, 
                commandType: CommandType.StoredProcedure);
            
            return parameters.Get<int>(""@ProdutoID"");
        }
    }

    // Procedure que retorna conjunto de resultados
    public async Task<IEnumerable<Produto>> BuscarProdutosPorCategoria(string categoria)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            return await connection.QueryAsync<Produto>(
                ""sp_BuscarProdutosPorCategoria"",
                new { Categoria = categoria },
                commandType: CommandType.StoredProcedure);
        }
    }
}

public class Produto
{
    public int ProdutoID { get; set; }
    public string Nome { get; set; } = string.Empty;
    public decimal Preco { get; set; }
    public int Estoque { get; set; }
}",
                    Language = "csharp",
                    Explanation = "Dapper simplifica chamadas a stored procedures. DynamicParameters permite trabalhar com parâmetros de saída. Query/QueryAsync retornam objetos tipados automaticamente.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Crie uma Stored Procedure Simples",
                    Description = "Crie uma stored procedure chamada sp_ContarProdutos que retorna o número total de produtos ativos no banco.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"CREATE PROCEDURE sp_ContarProdutos
AS
BEGIN
    -- Complete aqui
END;",
                    Hints = new List<string>
                    {
                        "Use SELECT COUNT(*) FROM Produtos",
                        "Adicione WHERE Ativo = 1 para contar apenas ativos",
                        "SET NOCOUNT ON no início é boa prática"
                    }
                },
                new Exercise
                {
                    Title = "Procedure com Parâmetros de Entrada e Saída",
                    Description = "Crie uma stored procedure sp_AtualizarEstoque que recebe ProdutoID e Quantidade, atualiza o estoque, e retorna o novo estoque através de um parâmetro OUTPUT.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"CREATE PROCEDURE sp_AtualizarEstoque
    @ProdutoID INT,
    @Quantidade INT,
    @NovoEstoque INT OUTPUT
AS
BEGIN
    -- Complete aqui
END;",
                    Hints = new List<string>
                    {
                        "UPDATE Produtos SET Estoque = Estoque + @Quantidade WHERE ProdutoID = @ProdutoID",
                        "Use SELECT @NovoEstoque = Estoque FROM Produtos WHERE ProdutoID = @ProdutoID",
                        "Considere usar transação para garantir consistência"
                    }
                },
                new Exercise
                {
                    Title = "Chame a Procedure de C#",
                    Description = "Escreva código C# usando Dapper para chamar sp_AtualizarEstoque, passando ProdutoID=1 e Quantidade=5, e exiba o novo estoque.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"using System.Data.SqlClient;

public async Task<int> AtualizarEstoque(int produtoId, int quantidade)
{
    using (var connection = new SqlConnection(connectionString))
    {
        var parameters = new DynamicParameters();
        // Complete aqui
        
        await connection.ExecuteAsync(/* ... */);
        
        return parameters.Get<int>(""@NovoEstoque"");
    }
}",
                    Hints = new List<string>
                    {
                        "parameters.Add(\"@ProdutoID\", produtoId)",
                        "parameters.Add(\"@Quantidade\", quantidade)",
                        "parameters.Add(\"@NovoEstoque\", dbType: DbType.Int32, direction: ParameterDirection.Output)",
                        "commandType: CommandType.StoredProcedure"
                    }
                }
            },
            Summary = "Nesta aula você aprendeu o que são stored procedures, suas vantagens e desvantagens, como criá-las com parâmetros de entrada e saída, e como chamá-las de C# usando ADO.NET e Dapper. Stored procedures são ferramentas poderosas para encapsular lógica de banco de dados e melhorar performance e segurança. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno. Grandes empresas como Microsoft, Google e Amazon aplicam estes conceitos em seus sistemas. Ao aprender estas técnicas, você está adquirindo conhecimento usado por desenvolvedores em todo o mundo. Considere como este conceito poderia ser aplicado em um projeto pessoal. Pensar em aplicações práticas ajuda a solidificar o entendimento e torna o aprendizado mais significativo. Iniciantes frequentemente cometem alguns erros ao aprender este tópico. Conhecer estes erros comuns ajuda a evitá-los e desenvolver código mais robusto desde o início. Um erro comum é não considerar casos extremos ou situações inesperadas. Sempre teste seu código com diferentes tipos de entrada, incluindo valores nulos, vazios ou inválidos. Outro erro frequente é otimizar prematuramente. Primeiro faça o código funcionar corretamente, depois otimize se necessário. Como diz o ditado: 'premature optimization is the root of all evil'. Quando encontrar problemas, use o debugger para entender o que está acontecendo. Coloque breakpoints, inspecione variáveis e execute o código passo a passo. Esta é uma habilidade essencial para todo desenvolvedor. Mensagens de erro podem parecer intimidadoras no início, mas elas são suas aliadas. Leia-as cuidadosamente - elas geralmente indicam exatamente onde e qual é o problema. Se algo não funcionar como esperado, não desanime. Debugging é uma parte normal do desenvolvimento. Até desenvolvedores experientes passam tempo significativo depurando código. Testar seu código é tão importante quanto escrevê-lo. Testes automatizados garantem que seu código funciona corretamente e continuará funcionando quando você fizer mudanças. Comece escrevendo testes simples para os casos mais comuns. Depois adicione testes para casos extremos e situações de erro. Uma boa cobertura de testes dá confiança para refatorar e melhorar o código. Considere a prática de TDD (Test-Driven Development), onde você escreve os testes antes do código. Isso ajuda a pensar claramente sobre o que o código deve fazer. Embora a correção seja sempre a prioridade, é importante estar ciente das implicações de performance de suas escolhas. Diferentes abordagens podem ter custos computacionais muito diferentes. Em muitos casos, a diferença de performance é negligenciável para aplicações pequenas. No entanto, à medida que seus programas crescem, escolhas eficientes se tornam cada vez mais importantes. Aprenda a usar ferramentas de profiling para identificar gargalos de performance. Otimize baseado em dados reais, não em suposições sobre onde o código é lento. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0004-000000000013"),
            CourseId = _courseId,
            Title = "Stored Procedures e Functions",
            Duration = "60 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 60,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0004-000000000012" }),
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
                "Compreender princípios de segurança de banco de dados",
                "Implementar autenticação e autorização adequadas",
                "Proteger dados sensíveis com criptografia",
                "Aplicar melhores práticas de segurança"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Princípios de Segurança de Banco de Dados",
                    Content = "Segurança de banco de dados é crítica porque bancos armazenam os ativos mais valiosos de uma organização: dados. O princípio do menor privilégio determina que usuários e aplicações devem ter apenas as permissões mínimas necessárias. Nunca use contas administrativas (sa, root) em aplicações. Defesa em profundidade significa múltiplas camadas de segurança: firewall de rede, autenticação forte, criptografia de dados, auditoria de acessos. Separação de responsabilidades garante que nenhuma pessoa tenha controle completo sobre processos críticos. Auditoria e monitoramento permitem detectar e responder a atividades suspeitas. Backup e recuperação garantem que dados possam ser restaurados após incidentes. Patches e atualizações regulares corrigem vulnerabilidades conhecidas. Testes de penetração identificam fraquezas antes que atacantes as explorem. Treinamento de equipe reduz erros humanos, a maior causa de violações de segurança. Conformidade com regulamentações como LGPD, GDPR, PCI-DSS não é opcional. Violações de dados podem resultar em multas massivas, perda de confiança de clientes, e danos irreparáveis à reputação.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Autenticação, Autorização e Auditoria",
                    Content = "Autenticação verifica identidade (quem você é), autorização determina permissões (o que você pode fazer), e auditoria registra ações (o que você fez). Use Windows Authentication quando possível por ser mais segura que SQL Authentication. Para SQL Authentication, exija senhas fortes e rotação regular. Considere autenticação multifator para acessos administrativos. Implemente bloqueio de conta após tentativas de login falhadas. Use roles para agrupar permissões em vez de atribuir permissões individuais a cada usuário. Crie roles customizadas para necessidades específicas da aplicação. Revise permissões regularmente e remova acessos desnecessários. Use schemas para organizar objetos e controlar acesso em nível de schema. Implemente Row-Level Security para filtrar linhas baseado no usuário. Dynamic Data Masking oculta dados sensíveis de usuários não autorizados. SQL Server Audit rastreia quem acessou o quê e quando. Configure alertas para atividades suspeitas como múltiplas tentativas de login falhadas, acessos fora do horário, ou queries que retornam grandes volumes de dados. Armazene logs de auditoria em local seguro e separado do banco de dados principal.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Criptografia e Proteção de Dados Sensíveis",
                    Content = "Criptografia protege dados em repouso (armazenados) e em trânsito (sendo transmitidos). Transparent Data Encryption (TDE) criptografa arquivos de banco de dados inteiros, protegendo contra roubo de backups ou arquivos de dados. Always Encrypted criptografa colunas específicas, mantendo dados criptografados mesmo durante queries. Útil para dados extremamente sensíveis como números de cartão de crédito. Column-level encryption permite criptografar colunas individuais com chaves gerenciadas pela aplicação. Use conexões criptografadas (Encrypt=true em connection strings) para proteger dados em trânsito. Nunca armazene senhas em texto claro - use hashing com salt (bcrypt, PBKDF2). Gerencie chaves de criptografia adequadamente - considere Azure Key Vault ou AWS KMS. Rotacione chaves regularmente. Implemente tokenização para dados de pagamento, substituindo números reais por tokens. Mascare dados em ambientes de desenvolvimento e teste - nunca use dados de produção reais. Implemente data loss prevention (DLP) para detectar e prevenir exfiltração de dados sensíveis. Classifique dados por sensibilidade e aplique controles apropriados a cada categoria.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Criando Usuário e Atribuindo Permissões",
                    Code = @"-- Criar login no nível de servidor
CREATE LOGIN AppUser WITH PASSWORD = 'SenhaForte123!';

-- Criar usuário no banco de dados
USE MeuBanco;
CREATE USER AppUser FOR LOGIN AppUser;

-- Criar role customizada
CREATE ROLE AppRole;

-- Atribuir permissões à role
GRANT SELECT, INSERT, UPDATE ON dbo.Produtos TO AppRole;
GRANT SELECT ON dbo.Categorias TO AppRole;
GRANT EXECUTE ON dbo.sp_InserirProduto TO AppRole;

-- Adicionar usuário à role
ALTER ROLE AppRole ADD MEMBER AppUser;

-- Verificar permissões
SELECT * FROM fn_my_permissions(NULL, 'DATABASE');",
                    Language = "sql",
                    Explanation = "Este exemplo mostra como criar um usuário com permissões limitadas usando roles. O usuário pode apenas ler/modificar Produtos, ler Categorias, e executar uma stored procedure específica.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Habilitando TDE (Transparent Data Encryption)",
                    Code = @"-- Criar master key no banco master
USE master;
CREATE MASTER KEY ENCRYPTION BY PASSWORD = 'SenhaMasterKey123!';

-- Criar certificado
CREATE CERTIFICATE TDECert WITH SUBJECT = 'TDE Certificate';

-- Criar encryption key no banco de dados
USE MeuBanco;
CREATE DATABASE ENCRYPTION KEY
WITH ALGORITHM = AES_256
ENCRYPTION BY SERVER CERTIFICATE TDECert;

-- Habilitar TDE
ALTER DATABASE MeuBanco SET ENCRYPTION ON;

-- Verificar status
SELECT 
    db_name(database_id) AS DatabaseName,
    encryption_state,
    CASE encryption_state
        WHEN 0 THEN 'No encryption'
        WHEN 1 THEN 'Unencrypted'
        WHEN 2 THEN 'Encryption in progress'
        WHEN 3 THEN 'Encrypted'
        WHEN 4 THEN 'Key change in progress'
        WHEN 5 THEN 'Decryption in progress'
    END AS EncryptionState
FROM sys.dm_database_encryption_keys;",
                    Language = "sql",
                    Explanation = "TDE criptografa todo o banco de dados, protegendo contra acesso não autorizado aos arquivos físicos. Importante fazer backup do certificado!",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Connection String Segura em C#",
                    Code = @"using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

// ❌ INSEGURO - senha hardcoded
// string connectionString = ""Server=localhost;Database=MeuBanco;User Id=sa;Password=123456;"";

// ✅ SEGURO - usando configuração
public class DatabaseConfig
{
    private readonly IConfiguration _configuration;
    
    public DatabaseConfig(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public string GetConnectionString()
    {
        // Connection string em appsettings.json ou variáveis de ambiente
        return _configuration.GetConnectionString(""DefaultConnection"") ?? string.Empty;
    }
}

// ✅ AINDA MELHOR - usando Azure Key Vault
public class SecureConnectionString
{
    public async Task<string> GetConnectionStringFromKeyVault()
    {
        var client = new SecretClient(
            new Uri(""https://myvault.vault.azure.net/""),
            new DefaultAzureCredential());
        
        var secret = await client.GetSecretAsync(""DatabaseConnectionString"");
        return secret.Value.Value;
    }
}

// Sempre use conexões criptografadas
public class ConnectionStringBuilder
{
    public string BuildSecureConnectionString()
    {
        return ""Server=myserver.database.windows.net;Database=MeuBanco;"" +
               ""User Id=appuser;Password=SenhaForte123!;"" +
               ""Encrypt=true;TrustServerCertificate=false;"";
    }
}",
                    Language = "csharp",
                    Explanation = "Nunca armazene credenciais em código. Use configuração externa, variáveis de ambiente, ou serviços de gerenciamento de segredos como Azure Key Vault. Sempre criptografe conexões.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Aplique o Princípio do Menor Privilégio",
                    Description = "Você tem uma aplicação web que precisa ler produtos, inserir pedidos, e atualizar estoque. Crie um usuário e role com apenas essas permissões específicas.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"-- Criar login e usuário
CREATE LOGIN WebAppUser WITH PASSWORD = 'SenhaForte123!';
USE MeuBanco;
CREATE USER WebAppUser FOR LOGIN WebAppUser;

-- Criar role e atribuir permissões
CREATE ROLE WebAppRole;
-- Complete aqui com as permissões mínimas necessárias

ALTER ROLE WebAppRole ADD MEMBER WebAppUser;",
                    Hints = new List<string>
                    {
                        "GRANT SELECT ON Produtos TO WebAppRole",
                        "GRANT INSERT ON Pedidos TO WebAppRole",
                        "GRANT UPDATE (Estoque) ON Produtos TO WebAppRole",
                        "Não dê permissões de DELETE ou DROP"
                    }
                },
                new Exercise
                {
                    Title = "Identifique Vulnerabilidades",
                    Description = "Analise este código e identifique pelo menos 3 problemas de segurança: string sql = \"SELECT * FROM Users WHERE Username = '\" + username + \"' AND Password = '\" + password + \"'\"; SqlCommand cmd = new SqlCommand(sql, connection); var user = cmd.ExecuteScalar();",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"// Problemas identificados:
// 1. 
// 2. 
// 3. ",
                    Hints = new List<string>
                    {
                        "SQL Injection - concatenação de strings",
                        "Senha em texto claro - deveria ser hash",
                        "Sem criptografia de conexão",
                        "Sem tratamento de erros"
                    }
                },
                new Exercise
                {
                    Title = "Implemente Auditoria",
                    Description = "Crie uma tabela de auditoria e um trigger que registra todas as modificações (INSERT, UPDATE, DELETE) na tabela Produtos, incluindo usuário, data/hora, e operação realizada.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"-- Criar tabela de auditoria
CREATE TABLE ProdutosAuditoria (
    AuditoriaID INT IDENTITY(1,1) PRIMARY KEY,
    -- Complete aqui
);

-- Criar trigger
CREATE TRIGGER trg_Produtos_Auditoria
ON Produtos
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    -- Complete aqui
END;",
                    Hints = new List<string>
                    {
                        "Tabela deve ter: ProdutoID, Operacao, Usuario, DataHora, ValoresAntigos, ValoresNovos",
                        "Use SYSTEM_USER para capturar usuário",
                        "Use GETDATE() para timestamp",
                        "Verifique tabelas INSERTED e DELETED para determinar operação",
                        "IF EXISTS(SELECT * FROM INSERTED) AND EXISTS(SELECT * FROM DELETED) = UPDATE"
                    }
                }
            },
            Summary = "Nesta aula você aprendeu princípios fundamentais de segurança de banco de dados, incluindo autenticação e autorização adequadas, criptografia de dados em repouso e em trânsito, auditoria de acessos, e melhores práticas como princípio do menor privilégio e defesa em profundidade. Segurança deve ser prioridade em qualquer aplicação que lida com dados sensíveis. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor. Não se preocupe se não entender tudo perfeitamente na primeira vez. A programação é uma habilidade que se desenvolve com prática constante. Cada linha de código que você escreve é um passo no seu aprendizado. Recomendamos que você experimente modificar os exemplos de código fornecidos. Tente prever o que acontecerá antes de executar, depois execute e veja se estava correto. Esta é uma excelente forma de aprender. Este conceito é usado em aplicações que você provavelmente usa todos os dias. Desde redes sociais até sistemas bancários, os princípios que você está aprendendo são fundamentais para o software moderno. Grandes empresas como Microsoft, Google e Amazon aplicam estes conceitos em seus sistemas. Ao aprender estas técnicas, você está adquirindo conhecimento usado por desenvolvedores em todo o mundo. Considere como este conceito poderia ser aplicado em um projeto pessoal. Pensar em aplicações práticas ajuda a solidificar o entendimento e torna o aprendizado mais significativo. Iniciantes frequentemente cometem alguns erros ao aprender este tópico. Conhecer estes erros comuns ajuda a evitá-los e desenvolver código mais robusto desde o início. Um erro comum é não considerar casos extremos ou situações inesperadas. Sempre teste seu código com diferentes tipos de entrada, incluindo valores nulos, vazios ou inválidos. Outro erro frequente é otimizar prematuramente. Primeiro faça o código funcionar corretamente, depois otimize se necessário. Como diz o ditado: 'premature optimization is the root of all evil'. Quando encontrar problemas, use o debugger para entender o que está acontecendo. Coloque breakpoints, inspecione variáveis e execute o código passo a passo. Esta é uma habilidade essencial para todo desenvolvedor. Mensagens de erro podem parecer intimidadoras no início, mas elas são suas aliadas. Leia-as cuidadosamente - elas geralmente indicam exatamente onde e qual é o problema. Se algo não funcionar como esperado, não desanime. Debugging é uma parte normal do desenvolvimento. Até desenvolvedores experientes passam tempo significativo depurando código. Testar seu código é tão importante quanto escrevê-lo. Testes automatizados garantem que seu código funciona corretamente e continuará funcionando quando você fizer mudanças. Comece escrevendo testes simples para os casos mais comuns. Depois adicione testes para casos extremos e situações de erro. Uma boa cobertura de testes dá confiança para refatorar e melhorar o código. Considere a prática de TDD (Test-Driven Development), onde você escreve os testes antes do código. Isso ajuda a pensar claramente sobre o que o código deve fazer. Embora a correção seja sempre a prioridade, é importante estar ciente das implicações de performance de suas escolhas. Diferentes abordagens podem ter custos computacionais muito diferentes. Em muitos casos, a diferença de performance é negligenciável para aplicações pequenas. No entanto, à medida que seus programas crescem, escolhas eficientes se tornam cada vez mais importantes. Aprenda a usar ferramentas de profiling para identificar gargalos de performance. Otimize baseado em dados reais, não em suposições sobre onde o código é lento. Na prática profissional, este conceito é aplicado diariamente por desenvolvedores em projetos de todos os tamanhos. Empresas de tecnologia utilizam estas técnicas para construir sistemas escaláveis e manuteníveis. Compreender profundamente este tópico permitirá que você escreva código mais eficiente, legível e fácil de manter. Estas habilidades são altamente valorizadas no mercado de trabalho. Ao dominar este conceito, você estará preparado para enfrentar desafios reais do desenvolvimento de software. Muitos problemas complexos podem ser resolvidos aplicando estes princípios fundamentais. É importante seguir as melhores práticas da indústria ao trabalhar com este conceito. A comunidade de desenvolvedores C# estabeleceu convenções que facilitam a colaboração e manutenção do código. Desenvolvedores experientes recomendam sempre considerar a legibilidade do código. Um código bem escrito é aquele que outros desenvolvedores (e você mesmo no futuro) conseguem entender facilmente. Ao escrever código, pense não apenas em fazer funcionar, mas em fazer funcionar bem. Considere aspectos como performance, manutenibilidade e escalabilidade desde o início. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos da programação. Cada conceito que você aprende se torna uma ferramenta em sua caixa de ferramentas de desenvolvedor."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0004-000000000014"),
            CourseId = _courseId,
            Title = "Segurança de Banco de Dados",
            Duration = "65 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 65,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0004-000000000013" }),
            OrderIndex = 20,
            Content = "",
            StructuredContent = JsonSerializer.Serialize(content),
            Version = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }
}
