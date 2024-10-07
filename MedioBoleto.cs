using System;

namespace Tp2AAT
{
    public class MedioBoleto : Tarjeta
    {
        private static readonly decimal TARIFA_MEDIO = TARIFA / 2;

        public MedioBoleto(decimal saldoInicial) : base(saldoInicial) { }

        public override bool TieneSaldoSuficiente()
        {
            return saldo >= TARIFA_MEDIO || saldo - TARIFA_MEDIO >= LIMITE_NEGATIVO;
        }

        public override void DebitarSaldo()
        {
            if (!TieneSaldoSuficiente())
                throw new InvalidOperationException("Saldo insuficiente para realizar el pago con medio boleto.");

            saldo -= TARIFA_MEDIO;
        }
    }
}
