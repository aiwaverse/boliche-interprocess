using System;

namespace Boliche
{
    class Boliche
    {
        static void Main(string[] args)
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
    enum Bonus
    {
        Strike = 2,
        Spare = 1,
        Nenhum = 0
    }
    class JogoDeBoliche
    {
        private IQuadro[] quadros = new IQuadro[]
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

        private int quadroAtual = 0;

        public void Jogar(int pinos)
        {
            if (quadroAtual == 10)
                return;
            IQuadro atual = quadros[quadroAtual];
            atual.AdicionarJogada(pinos);
            if (!atual.Disponivel())
                ++quadroAtual;
        }

        public int ObterPontuacao()
        {
            for (int i = 0; i < quadros.Length; ++i)
            {
                IQuadro quadroAtual = quadros[i];
                Pontuacao += quadroAtual.Pontuacao();
                if (quadroAtual.BonusDoQuadro() == Bonus.Nenhum)
                    continue;
                Pontuacao += PontuacaoProximasJogadas(i, quadroAtual.BonusDoQuadro());
            }
            return Pontuacao;
        }

        private int PontuacaoProximasJogadas(int i, Bonus b)
        {
            if (i == 8)
            {
                if (b == Bonus.Strike)
                    return quadros[9].PontuacaoPrimeiraJogada() + quadros[9].PontuacaoSegundaJogada();
                else
                    return quadros[9].PontuacaoPrimeiraJogada();
            }
            else
            {
                if (b == Bonus.Strike)
                {
                    int pontuacao1 = quadros[i + 1].PontuacaoPrimeiraJogada();
                    if (pontuacao1 == 10)
                        return pontuacao1 + quadros[i + 2].PontuacaoPrimeiraJogada();
                    return pontuacao1 + quadros[i + 1].PontuacaoSegundaJogada();
                }
                else
                {
                    return quadros[i + 1].PontuacaoPrimeiraJogada();
                }
            }
        }

    }

    interface IQuadro
    {
        int Pontuacao();

        Bonus BonusDoQuadro();

        bool Disponivel();

        void AdicionarJogada(int pinos);

        int PontuacaoPrimeiraJogada();
        int PontuacaoSegundaJogada();
    }

    class QuadroBasico : IQuadro
    {
        private int? _primeiraJogada = null;
        public int? PrimeiraJogada
        {
            get => _primeiraJogada;
            set
            {
                if (value > 10)
                {
                    _primeiraJogada = 10;
                }
                else
                {
                    _primeiraJogada = value;
                }
            }
        }
        private int? _segundaJogada = null;
        public int? SegundaJogada
        {
            get => _segundaJogada;
            set
            {
                if (value > 10)
                {
                    _segundaJogada = 10;
                }
                else
                {
                    _segundaJogada = value;
                }
            }
        }

        public Bonus BonusDoQuadro()
        {
            if (PrimeiraJogada == 10)
                return Bonus.Strike;
            if (PrimeiraJogada + SegundaJogada == 10)
                return Bonus.Spare;
            return Bonus.Nenhum;
        }

        public int Pontuacao()
        {
            return (PrimeiraJogada ?? 0) + (SegundaJogada ?? 0);
        }

        public bool Disponivel()
        {
            if (PrimeiraJogada == 10 || (PrimeiraJogada is not null && SegundaJogada is not null))
                return false;
            return true;
        }

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

        public int PontuacaoPrimeiraJogada() => PrimeiraJogada ?? 0;
        public int PontuacaoSegundaJogada() => SegundaJogada ?? 0;
    }

    class QuadroFinal : IQuadro
    {
        private int? _primeiraJogada = null;
        public int? PrimeiraJogada
        {
            get => _primeiraJogada;
            set
            {
                if (value > 10)
                {
                    _primeiraJogada = 10;
                }
                else
                {
                    _primeiraJogada = value;
                }
            }
        }
        private int? _segundaJogada = null;
        public int? SegundaJogada
        {
            get => _segundaJogada;
            set
            {
                if (value > 10)
                {
                    _segundaJogada = 10;
                }
                else
                {
                    _segundaJogada = value;
                }
            }
        }
        private int? _terceiraJogada = null;
        public int? TerceiraJogada
        {
            get => _terceiraJogada;
            set
            {
                if (value > 10)
                {
                    _terceiraJogada = 10;
                }
                else
                {
                    _terceiraJogada = value;
                }
            }
        }
        public Bonus BonusDoQuadro()
        {
            return Bonus.Nenhum;
        }

        public int Pontuacao()
        {
            return (PrimeiraJogada ?? 0) + (SegundaJogada ?? 0) + (TerceiraJogada ?? 0);
        }

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

        public void AdicionarJogada(int pinos)
        {
            if (PrimeiraJogada is null)
                PrimeiraJogada = pinos;
            else if (SegundaJogada is null)
                SegundaJogada = pinos;
            else
                TerceiraJogada = pinos;
        }
        public int PontuacaoPrimeiraJogada() => PrimeiraJogada ?? 0;
        public int PontuacaoSegundaJogada() => SegundaJogada ?? 0;
    }
}
