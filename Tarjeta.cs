using System;

namespace TpTarjeta
{
    public class Tarjeta
    {
        protected decimal saldo;
        protected static readonly decimal TARIFA = 940m;
        protected static readonly decimal LIMITE_SALDO = 9900m;
        protected static readonly decimal LIMITE_NEGATIVO = -480m;
        private decimal ultimoPago;
        private string idTarjeta;

        public Tarjeta(decimal saldoInicial)
        {
            if (!EsMontoValido(saldoInicial))
                throw new ArgumentException("Saldo inicial no válido.");

            saldo = saldoInicial;
            idTarjeta = GenerarIDNumerico(16); // Generamos un ID numérico de 16 dígitos.
        }

        private bool EsMontoValido(decimal monto)
        {
            decimal[] montosAceptados = { 2000m, 3000m, 4000m, 5000m, 6000m, 7000m, 8000m, 9000m };
            return Array.Exists(montosAceptados, m => m == monto) || monto <= LIMITE_SALDO;
        }

        // Método para verificar si tiene saldo suficiente (marcado como virtual)
        public virtual bool TieneSaldoSuficiente()
        {
            return saldo >= TARIFA || saldo - TARIFA >= LIMITE_NEGATIVO;
        }

        public virtual void DebitarSaldo(Tiempo tiempoActual)
        {
            if (!TieneSaldoSuficiente())
                throw new InvalidOperationException("Saldo insuficiente.");

            // Registra el último pago realizado
            ultimoPago = TARIFA; // Guardamos el monto debito
            saldo -= TARIFA; // Se debita el saldo de la tarjeta
        }

        public decimal ObtenerSaldo() => saldo;

        // Método para obtener el ID de la tarjeta
        public string ObtenerID() => idTarjeta;

        // Método para obtener el último pago realizado
        public decimal ObtenerUltimoPago() => ultimoPago;

        // Método para recargar saldo
        public void RecargarSaldo(decimal monto)
        {
            if (monto <= 0)
                throw new ArgumentException("El monto de recarga debe ser positivo.");

            bool saldoNegativoCancelado = saldo < 0;

            if (saldoNegativoCancelado)
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

            // Calcula el nuevo saldo si se carga el monto
            if (saldo + monto > 36000m)
            {
                // Solo se puede acreditar hasta el límite permitido
                decimal cargaPosible = 36000m - saldo;
                saldo += cargaPosible; // Acredita hasta el límite
                decimal excedente = monto - cargaPosible; // Lo que queda pendiente
                Console.WriteLine($"Carga parcial. Se acreditaron {cargaPosible} pesos, {excedente} quedan pendientes.");
            }
            else
            {
                saldo += monto; // Acredita el monto total si no se excede el límite
                Console.WriteLine($"Se acreditaron {monto} pesos.");
            }
        }

        // Método para verificar si se canceló el saldo negativo con la recarga
        public bool SaldoNegativoCancelado() => saldo >= 0;

        private string GenerarIDNumerico(int longitud)
        {
            Random random = new Random();
            string id = "";

            for (int i = 0; i < longitud; i++)
            {
                id += random.Next(0, 10).ToString(); // Genera un número entre 0 y 9
            }

            return id;
        }
    }
}
