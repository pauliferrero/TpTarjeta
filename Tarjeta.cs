<<<<<<< HEAD
using System;

namespace TpTarjeta
{
    public class Tarjeta
    {
        public decimal saldo;
        protected decimal saldoPendiente; 
        protected static readonly decimal LIMITE_SALDO = 36000m;
        protected static readonly decimal LIMITE_NEGATIVO = -480m;
        public decimal ultimoPago;
        private string idTarjeta;
        private Tiempo tiempoAcumulado;
        private bool saldoFueNegativo;
        private int contadorViajes;

        public Tarjeta(decimal saldoInicial)
        {

            if (!EsMontoValido(saldoInicial))
                throw new ArgumentException("Saldo inicial no válido.");

            saldo = saldoInicial;
            idTarjeta = GenerarIDNumerico(16);
            contadorViajes = 0; 
            tiempoAcumulado = new Tiempo(0, 0);
            saldoPendiente = 0; 
            saldoFueNegativo = saldo < 0;
        }

        private bool EsMontoValido(decimal monto)
        {
            decimal[] montosAceptados = { 2000m, 3000m, 4000m, 5000m, 6000m, 7000m, 8000m, 9000m };
            return Array.Exists(montosAceptados, m => m == monto) || monto <= LIMITE_SALDO;
        }

        public virtual bool TieneSaldoSuficiente(decimal tarifa)
        {
            decimal tarifaAPagar = tarifa;

            return saldo >= tarifaAPagar || saldo - tarifaAPagar >= LIMITE_NEGATIVO;
        }

        public void DebitarSaldoPorViajes(int cantidadViajes, Tiempo tiempoPorViaje)
        {
            for (int i = 0; i < cantidadViajes; i++)
            {
                DebitarSaldo(tiempoPorViaje); 
                AcreditarSaldoPendiente(); 
            }
            tiempoAcumulado.Sumar(new Tiempo(0, cantidadViajes * tiempoPorViaje.ObtenerMinutos()));
        }

        public Tiempo ObtenerTiempoAcumulado() => tiempoAcumulado;

        public virtual void DebitarSaldo(Tiempo tiempoActual)
        {
            contadorViajes++;

            decimal tarifaAPagar = CalcularTarifa();

            Console.WriteLine($"Tarifa a pagar: {tarifaAPagar}");

            if (saldo < tarifaAPagar)
            {
                if (saldoPendiente + saldo < tarifaAPagar)
                {
                    Console.WriteLine($"Saldo: {saldo}, Saldo Pendiente: {saldoPendiente}, Tarifa a Pagar: {tarifaAPagar}");
                    throw new InvalidOperationException("No hay suficiente saldo o saldo pendiente para cubrir la tarifa.");
                }

                decimal saldoNecesario = tarifaAPagar - saldo;

                if (saldoPendiente >= saldoNecesario)
                {
                    saldoPendiente -= saldoNecesario; 
                    saldo = 0; 
                }
                else
                {
                    saldoPendiente -= (saldoNecesario - saldo);
                    saldo = 0;
                }
            }
            else
            {
                saldo -= tarifaAPagar; 
            }

            ultimoPago = tarifaAPagar;

            Console.WriteLine($"Antes de debitar: últimoPago: {ultimoPago}, saldo: {saldo}");

            AcreditarSaldoPendiente();
            UpdateSaldoNegativoState();
        }



        private decimal CalcularTarifa()
        {
            if (contadorViajes >= 1 && contadorViajes <= 29)
                return Colectivo.tarifa; 
            else if (contadorViajes >= 30 && contadorViajes <= 79)
                return Colectivo.tarifa * 0.80m; // 20% de descuento
            else if (contadorViajes == 80)
                return Colectivo.tarifa * 0.75m; // 25% de descuento para el 80º viaje
            else
                return Colectivo.tarifa; // Para 81 o más viajes, tarifa normal
        }

        public decimal ObtenerSaldo() => saldo;

        public decimal ObtenerSaldoPendiente() => saldoPendiente;

        public string ObtenerID() => idTarjeta;

        public decimal ObtenerUltimoPago() => ultimoPago;

        public void RecargarSaldo(decimal monto)
        {
            if (monto <= 0)
                throw new ArgumentException("El monto de recarga debe ser positivo.");

            if (saldo + monto > LIMITE_SALDO)
            {
                decimal cargaPosible = LIMITE_SALDO - saldo;
                saldo += cargaPosible; // Acredita hasta el límite
                saldoPendiente += (monto - cargaPosible); // Aumenta el saldo pendiente
                Console.WriteLine($"Carga parcial. Se acreditaron {cargaPosible} pesos, {monto - cargaPosible} quedan pendientes.");
            }
            else
            {
                saldo += monto; // Acredita el monto total si no se excede el límite
                Console.WriteLine($"Se acreditaron {monto} pesos.");
            }

            UpdateSaldoNegativoState();
        }

        private void AcreditarSaldoPendiente()
        {
            if (saldoPendiente > 0)
            {
                decimal cargaPosible = LIMITE_SALDO - saldo;
                if (cargaPosible > 0)
                {
                    if (saldoPendiente <= cargaPosible)
                    {
                        saldo += saldoPendiente;
                        saldoPendiente = 0; 
                    }
                    else
                    {
                        saldo += cargaPosible;
                        saldoPendiente -= cargaPosible; 
                    }
                }
            }
        }

        private void UpdateSaldoNegativoState()
        {
            if (saldo >= 0)
            {
                if (saldoFueNegativo)
                {
                    saldoFueNegativo = false; 
                }
            }
            else
            {
                saldoFueNegativo = true; 
            }
        }

        public bool SaldoNegativoCancelado()
        {
            return saldo >= 0 && saldoFueNegativo;
        }

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
=======
using System;

namespace Tp2AAT
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

        public virtual void DebitarSaldo()
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
>>>>>>> saldo_tarjeta
