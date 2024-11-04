using System;

namespace TpTarjeta
{
    public class Tarjeta
    {
        protected decimal saldo;
        protected decimal saldoPendiente; // Saldo pendiente de acreditación
        protected static readonly decimal TARIFA_NORMAL = 1200m;
        protected static readonly decimal TARIFA_DESCUENTO_20 = 1200m * 0.80m; // 20% de descuento
        protected static readonly decimal TARIFA_DESCUENTO_25 = 1200m * 0.75m; // 25% de descuento
        protected static readonly decimal LIMITE_SALDO = 36000m;
        protected static readonly decimal LIMITE_NEGATIVO = -480m;
        private decimal ultimoPago;
        private string idTarjeta;
        private Tiempo tiempoAcumulado;


        // Mantener un contador de viajes
        private int contadorViajes;

        public Tarjeta(decimal saldoInicial)
        {
            if (!EsMontoValido(saldoInicial))
                throw new ArgumentException("Saldo inicial no válido.");

            saldo = saldoInicial;
            idTarjeta = GenerarIDNumerico(16);
            contadorViajes = 0; // Inicializa el contador de viajes
            tiempoAcumulado = new Tiempo(0, 0);
            saldoPendiente = 0; // Inicializa el saldo pendiente
        }

        private bool EsMontoValido(decimal monto)
        {
            decimal[] montosAceptados = { 2000m, 3000m, 4000m, 5000m, 6000m, 7000m, 8000m, 9000m };
            return Array.Exists(montosAceptados, m => m == monto) || monto <= LIMITE_SALDO;
        }

        // Método para verificar si tiene saldo suficiente (sin parámetros)
        public virtual bool TieneSaldoSuficiente()
        {
            decimal tarifaAPagar = CalcularTarifa();
            return saldo >= tarifaAPagar || saldo - tarifaAPagar >= LIMITE_NEGATIVO;
        }

        public void DebitarSaldoPorViajes(int cantidadViajes, Tiempo tiempoPorViaje)
        {
            // Calcular el total de viajes
            for (int i = 0; i < cantidadViajes; i++)
            {
                DebitarSaldo(tiempoPorViaje); // Realiza el débito para cada viaje
                AcreditarSaldoPendiente(); // Intenta acreditar saldo pendiente
            }
            // Sumar el tiempo total al tiempo acumulado
            tiempoAcumulado.Sumar(new Tiempo(0, cantidadViajes * tiempoPorViaje.ObtenerMinutos()));
        }

        public Tiempo ObtenerTiempoAcumulado() => tiempoAcumulado;

        public virtual void DebitarSaldo(Tiempo tiempoActual)
        {
            // Incrementar el contador de viajes al inicio
            contadorViajes++;

            decimal tarifaAPagar = CalcularTarifa();

            // Verificar si hay suficiente saldo
            if (saldo < tarifaAPagar)
            {
                // Verificar si se puede cubrir con saldo pendiente
                if (saldoPendiente + saldo < tarifaAPagar)
                {
                    Console.WriteLine($"Saldo: {saldo}, Saldo Pendiente: {saldoPendiente}, Tarifa a Pagar: {tarifaAPagar}");
                    throw new InvalidOperationException("No hay suficiente saldo o saldo pendiente para cubrir la tarifa.");
                }

                // Calcular cuánto saldo pendiente se necesita usar
                decimal saldoNecesario = tarifaAPagar - saldo;

                // Si hay saldo pendiente suficiente, usarlo
                if (saldoPendiente >= saldoNecesario)
                {
                    saldoPendiente -= saldoNecesario; // Usar saldo pendiente
                    saldo = 0; // El saldo actual queda en 0
                }
                else
                {
                    // Usar todo el saldo y restar lo que queda del saldo pendiente
                    saldoPendiente -= (saldoNecesario - saldo);
                    saldo = 0;
                }
            }
            else
            {
                saldo -= tarifaAPagar; // Se debita el saldo de la tarjeta
            }

            // Registra el último pago realizado
            ultimoPago = tarifaAPagar;

            // Acreditar cualquier saldo pendiente que pueda ser transferido al saldo actual
            AcreditarSaldoPendiente();
        }



        private decimal CalcularTarifa()
        {
            if (contadorViajes >= 1 && contadorViajes <= 29)
                return TARIFA_NORMAL; // Tarifa normal
            else if (contadorViajes >= 30 && contadorViajes <= 79)
                return TARIFA_DESCUENTO_20; // 20% de descuento
            else if (contadorViajes == 80)
                return TARIFA_DESCUENTO_25; // 25% de descuento para el 80º viaje
            else
                return TARIFA_NORMAL; // Para 81 o más viajes, tarifa normal
        }

        public void ReiniciarContadorViajes()
        {
            contadorViajes = 0; // Reinicia el contador de viajes al inicio de un nuevo mes
        }

        public decimal ObtenerSaldo() => saldo;

        public decimal ObtenerSaldoPendiente() => saldoPendiente;

        // Método para obtener el ID de la tarjeta
        public string ObtenerID() => idTarjeta;

        // Método para obtener el último pago realizado
        public decimal ObtenerUltimoPago() => ultimoPago;

        // Método para recargar saldo
        public void RecargarSaldo(decimal monto)
        {
            if (monto <= 0)
                throw new ArgumentException("El monto de recarga debe ser positivo.");

            // Lógica existente para pagar deudas

            // Cálculo de saldo
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
        }

        // Método para verificar y acreditar saldo pendiente
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
                        saldoPendiente = 0; // Se acredita todo el saldo pendiente
                    }
                    else
                    {
                        saldo += cargaPosible;
                        saldoPendiente -= cargaPosible; // Reduce el saldo pendiente
                    }
                }
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
