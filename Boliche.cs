using System;

namespace Boliche
{
    class Boliche
    {
        static void Main()
        {
            JogoDeBoliche jogoPerfeito = new();
            jogoPerfeito.Jogar(10);
            jogoPerfeito.Jogar(10);
            jogoPerfeito.Jogar(10);
            jogoPerfeito.Jogar(10);
            jogoPerfeito.Jogar(10);
            jogoPerfeito.Jogar(10);
            jogoPerfeito.Jogar(10);
            jogoPerfeito.Jogar(10);
            jogoPerfeito.Jogar(10);
            jogoPerfeito.Jogar(10);
            jogoPerfeito.Jogar(10);
            jogoPerfeito.Jogar(10);

            Console.WriteLine($"Jogo Perfeito: {jogoPerfeito.ObterPontuacao()}");

            JogoDeBoliche jogoExemplo = new();
            jogoExemplo.Jogar(1);
            jogoExemplo.Jogar(4);
            jogoExemplo.Jogar(4);
            jogoExemplo.Jogar(5);
            jogoExemplo.Jogar(6);
            jogoExemplo.Jogar(4);
            jogoExemplo.Jogar(5);
            jogoExemplo.Jogar(5);
            jogoExemplo.Jogar(10);
            jogoExemplo.Jogar(0);
            jogoExemplo.Jogar(1);
            jogoExemplo.Jogar(7);
            jogoExemplo.Jogar(3);
            jogoExemplo.Jogar(6);
            jogoExemplo.Jogar(4);
            jogoExemplo.Jogar(10);
            jogoExemplo.Jogar(2);
            jogoExemplo.Jogar(8);
            jogoExemplo.Jogar(6);

            Console.WriteLine($"Jogo Exemplo: {jogoExemplo.ObterPontuacao()}");

        }
    }
    // Enumeração para representar os possíveis bônus de um quadro
    enum Bonus
    {
        Strike,
        Spare,
        Nenhum
    }
    // Classe do jogo de boliche
    class JogoDeBoliche
    {
        readonly private IQuadro[] quadros = new IQuadro[]
        {
            new QuadroBasico(),
            new QuadroBasico(),
            new QuadroBasico(),
            new QuadroBasico(),
            new QuadroBasico(),
            new QuadroBasico(),
            new QuadroBasico(),
            new QuadroBasico(),
            new QuadroBasico(),
            new QuadroFinal()
        };

        public int Pontuacao { get; private set; }

        private int quadroAtual = 0; // quadro atual que está sendo "preenchido"

        public void Jogar(int pinos)
        {
            if (quadroAtual == quadros.Length)
                // É possível jogar até que o número de quadros se esgote
                return;
            IQuadro atual = quadros[quadroAtual];
            atual.AdicionarJogada(pinos);
            if (!atual.Disponivel()) 
                // Se o atual não está mais disponível após receber nova jogada, incrementamos o indice do atual
                ++quadroAtual;
        }

        // Contabiliza a pontuação de todos os quadros, usando um laço for pois o indice é usado em outra função
        public int ObterPontuacao()
        {
            for (int i = 0; i < quadros.Length; ++i)
            {
                IQuadro quadroAtual = quadros[i];
                Pontuacao += quadroAtual.Pontuacao();
                Pontuacao += PontuacaoProximasJogadas(i, quadroAtual.BonusDoQuadro());
            }
            return Pontuacao;
        }

        // Contabiliza os bônus
        // Como o quadro final nunca tem bônus e sempre possuí uma próxima jogada, isso garante que os acessos
        // nunca são usados acima do limite
        private int PontuacaoProximasJogadas(int i, Bonus b)
        {
            if (b == Bonus.Nenhum)
                return 0;
            int pont = 0;
            pont += quadros[i + 1].PontuacaoPrimeiraJogada();
            if (b == Bonus.Strike)
            {
                // Se o quadro possuir próxima jogada, usa-se a sua segunda jogada, se não, usa-se a primeira jogada do próximo quadro
                pont += quadros[i + 1].TemProximaJogada() ? quadros[i + 1].PontuacaoSegundaJogada() : quadros[i + 2].PontuacaoPrimeiraJogada();
            }
            return pont;
        }

    }

    // Interface que generaliza um quadro, abstraindo as particularidades do quadro final e do quadro normal
    interface IQuadro
    {
        int Pontuacao(); // Pontuação simples do quadro
        Bonus BonusDoQuadro(); // Bonus do quadro (Strike, Spare, Nenhum)
        bool Disponivel(); // Se o quadro ainda pode "receber" novas jogadas
        void AdicionarJogada(int pinos); // Adicionando uma jogada nova ao quadro
        int PontuacaoPrimeiraJogada(); // A pontuação da primeira jogada
        int PontuacaoSegundaJogada(); // A pontuação da segunda jogada
        bool TemProximaJogada(); // Se, após o resultado da primeira jogada, o quadro ainda possui mais jogadas para contabilizar
        // O conceito de próxima jogada é relacionado a contabilização dos bônus, como um strike contabiliza os pinos
        // das duas próximas jogadas, é recuperado a primeira jogada do próximo quadro, e então usa-se essa função
        // para descobrir se a próxima jogada está dentro do quadro, ou não, caso não esteja, é usado o quadro seguinte
        // Especialmente usado no caso de strikes seguidos
    }

    class QuadroBasico : IQuadro
    {
        private int? _primeiraJogada = null;

        // Uma jogada possuí no máximo 10 pontos, e no mínimo 0 pontos
        // Valores maiores ou menores são "saturados"
        public int? PrimeiraJogada
        {
            get => _primeiraJogada;
            set
            {
                if (value > 10)
                    _primeiraJogada = 10;
                else if (value < 0)
                    _primeiraJogada = 0;
                else
                    _primeiraJogada = value;
            }
        }
        private int? _segundaJogada = null;
        public int? SegundaJogada
        {
            get => _segundaJogada;
            set
            {
                if (value > 10)
                    _segundaJogada = 10;
                else if (value < 0)
                    _segundaJogada = 0;
                else
                    _segundaJogada = value;
            }
        }

        // Para o quadro básico, os bônus são calculados normalmente
        public Bonus BonusDoQuadro()
        {
            if (PrimeiraJogada == 10)
                return Bonus.Strike;
            if (PrimeiraJogada + SegundaJogada == 10)
                return Bonus.Spare;
            return Bonus.Nenhum;
        }

        // Uma jogada não feita possuí pontuação zero, é a simples soma das pontuações
        public int Pontuacao()
        {
            return (PrimeiraJogada ?? 0) + (SegundaJogada ?? 0);
        }

        // O quadro está disponível se ambas as jogadas não são nulas, e não há um strike
        public bool Disponivel()
        {
            if (PrimeiraJogada == 10 || (PrimeiraJogada is not null && SegundaJogada is not null))
                return false;
            return true;
        }

        // Pré-condição: O quadro está disponível
        public void AdicionarJogada(int pinos)
        {
            // Não consegui usar o operador ??= aqui por causa do else
            if (PrimeiraJogada is null)
            {
                PrimeiraJogada = pinos;
            }
            else
            {
                SegundaJogada = pinos;
            }
        }

        // Funções simples nesse caso
        public int PontuacaoPrimeiraJogada() => PrimeiraJogada ?? 0;
        public int PontuacaoSegundaJogada() => SegundaJogada ?? 0;

        // O quadro "tem uma próxima jogada" se ambas não forem nulas
        public bool TemProximaJogada()
        {
            return PrimeiraJogada is not null && SegundaJogada is not null;
        }

    }

    class QuadroFinal : IQuadro
    {
        private int? _primeiraJogada = null;

        // Uma jogada possuí no máximo 10 pontos, e no mínimo 0 pontos
        // Valores maiores ou menores são "saturados"
        public int? PrimeiraJogada
        {
            get => _primeiraJogada;
            set
            {
                if (value > 10)
                    _primeiraJogada = 10;
                else if (value < 0)
                    _primeiraJogada = 0;
                else
                    _primeiraJogada = value;
            }
        }
        private int? _segundaJogada = null;
        public int? SegundaJogada
        {
            get => _segundaJogada;
            set
            {
                if (value > 10)
                    _segundaJogada = 10;
                else if (value < 0)
                    _segundaJogada = 0;
                else
                    _segundaJogada = value;
            }
        }
        private int? _terceiraJogada = null;
        public int? TerceiraJogada
        {
            get => _terceiraJogada;
            set
            {
                if (value > 10)
                    _terceiraJogada = 10;
                else if (value < 0)
                    _terceiraJogada = 0;
                else
                    _terceiraJogada = value;
            }
        }
        // O quadro final nunca possuí bônus
        public Bonus BonusDoQuadro()
        {
            return Bonus.Nenhum;
        }

        // A pontuação do quadro final é a soma das três jogadas (Zero se null)
        public int Pontuacao()
        {
            return (PrimeiraJogada ?? 0) + (SegundaJogada ?? 0) + (TerceiraJogada ?? 0);
        }

        // O quadro final está disponível sse a primeira ou segunda jogada forem nulls, ou
        // caso exista um strike na primeira jogada, ou um spare na primeira e segunda (nesses casos, a terceira deve ser null)
        public bool Disponivel()
        {
            if (PrimeiraJogada is null)
                return true;
            if (SegundaJogada is null)
                return true;
            if (PrimeiraJogada == 10 && TerceiraJogada is null)
                return true;
            if ((PrimeiraJogada + SegundaJogada == 10) && TerceiraJogada is null)
                return true;
            return false;
        }
        // Adicionar uma jogada é trivial no quadro (se nulo, adiciona)
        // Pré-condição: O quadro está disponível
        public void AdicionarJogada(int pinos)
        {
            if (PrimeiraJogada is null)
                PrimeiraJogada = pinos;
            else if (SegundaJogada is null)
                SegundaJogada = pinos;
            else
                TerceiraJogada = pinos;
        }
        // Funções simples para retornar a pontuação
        public int PontuacaoPrimeiraJogada() => PrimeiraJogada ?? 0;
        public int PontuacaoSegundaJogada() => SegundaJogada ?? 0;
        // O quadro final sempre possuí "próxima jogada"
        public bool TemProximaJogada() => true;
    }
}
