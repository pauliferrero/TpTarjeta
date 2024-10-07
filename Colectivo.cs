using System;

namespace Tp2AAT
{
    public class Colectivo
    {
        public Boleto PagarCon(Tarjeta tarjeta)
        {
            if (tarjeta == null)
                throw new ArgumentNullException(nameof(tarjeta));

            if (!tarjeta.TieneSaldoSuficiente())
                throw new InvalidOperationException("Saldo insuficiente en la tarjeta.");

            tarjeta.DebitarSaldo();
            return new Boleto();
        }
    }
}

