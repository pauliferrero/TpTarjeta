using System;

namespace Tp2AAT
{
    public class Tarjeta
    {
        protected decimal saldo;
        protected static readonly decimal TARIFA = 940m;
        protected static readonly decimal LIMITE_SALDO = 9900m;
        protected static readonly decimal LIMITE_NEGATIVO = -480m;

        public Tarjeta(decimal saldoInicial)
        {
            if (!EsMontoValido(saldoInicial))
                throw new ArgumentException("Saldo inicial no válido.");

            saldo = saldoInicial;
        }

        private bool EsMontoValido(decimal monto)
        {
            decimal[] montosAceptados = { 2000m, 3000m, 4000m, 5000m, 6000m, 7000m, 8000m, 9000m };
            return Array.Exists(montosAceptados, m => m == monto) || monto <= LIMITE_SALDO;
        }

        public virtual bool TieneSaldoSuficiente()
        {
            return saldo >= TARIFA || saldo - TARIFA >= LIMITE_NEGATIVO;
        }

        public virtual void DebitarSaldo()
        {
            if (!TieneSaldoSuficiente())
                throw new InvalidOperationException("Saldo insuficiente.");

            saldo -= TARIFA;
        }

        public decimal ObtenerSaldo() => saldo;

        public void RecargarSaldo(decimal monto)
        {
            if (monto <= 0)
                throw new ArgumentException("El monto de recarga debe ser positivo.");

            if (saldo < 0)
            {
                decimal deuda = Math.Abs(saldo);
                if (monto >= deuda)
                {
                    monto -= deuda;
                    saldo = 0;
                }
                else
                {
                    saldo += monto;
                    monto = 0;
                }
            }

            saldo += monto;
        }
    }
}
