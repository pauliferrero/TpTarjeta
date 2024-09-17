using System;
using System.Collections.Generic;

namespace Tp2AAT
{
    public class Tarjeta
    {
        private decimal saldo;
        private static readonly decimal TARIFA = 940m;
        private static readonly decimal LIMITE_SALDO = 9900m;

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

        public bool TieneSaldoSuficiente()
        {
            return saldo >= TARIFA;
        }

        public void DebitarSaldo()
        {
            if (!TieneSaldoSuficiente())
                throw new InvalidOperationException("Saldo insuficiente para realizar el pago.");

            saldo -= TARIFA;
        }

        public decimal ObtenerSaldo()
        {
            return saldo;
        }
    }
}
