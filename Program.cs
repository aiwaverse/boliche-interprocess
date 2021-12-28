using System;

namespace Boliche
{
    class Boliche
    {
        static void Main(string[] args)
        {
            JogoDeBoliche jogo = new();
            jogo.Jogar(1);
            jogo.Jogar(4);
            jogo.Jogar(4);
            jogo.Jogar(5);
            jogo.Jogar(6);
            jogo.Jogar(4);
            jogo.Jogar(5);
            jogo.Jogar(5);
            jogo.Jogar(10);
            jogo.Jogar(0);
            jogo.Jogar(1);
            jogo.Jogar(7);
            jogo.Jogar(3);
            jogo.Jogar(6);
            jogo.Jogar(4);
            jogo.Jogar(10);
            jogo.Jogar(2);
            jogo.Jogar(8);
            jogo.Jogar(6);
            Console.WriteLine(jogo.ObterPontuacao());
        }
    }
    enum Bonus
    {
        Strike,
        Spare,
        Nenhum
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
            Bonus bonusAContabilizar = Bonus.Nenhum;
            for (int i = 0; i < quadros.Length; ++i)
            {
                IQuadro quadro = quadros[i];
                Pontuacao += quadro.Pontuacao();
                if (bonusAContabilizar == Bonus.Strike)
                {
                    Pontuacao += quadro.PontuacaoPrimeiraJogada();
                    Pontuacao += quadro.PontuacaoSegundaJogada();
                }
                if (bonusAContabilizar == Bonus.Spare)
                    Pontuacao += quadro.PontuacaoPrimeiraJogada();

                bonusAContabilizar = quadro.BonusDoQuadro();
            }
            return Pontuacao;
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
            if (PrimeiraJogada == 10 && TerceiraJogada is not null)
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
