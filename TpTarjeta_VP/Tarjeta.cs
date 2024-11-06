using System;

namespace TpTarjeta
{
    public class Tarjeta
    {
        protected Tiempo tiempoAcumulado;
        public decimal saldo;
        protected decimal saldoPendiente; 
        protected static readonly decimal LIMITE_SALDO = 36000m;
        protected static readonly decimal LIMITE_NEGATIVO = -480m;
        public decimal ultimoPago;
        protected string idTarjeta;
        protected bool saldoFueNegativo;
        public int contadorViajes;

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

        protected static bool EsMontoValido(decimal monto)
        {
            decimal[] montosAceptados = { 2000m, 3000m, 4000m, 5000m, 6000m, 7000m, 8000m, 9000m };
            return Array.Exists(montosAceptados, m => m == monto) || monto <= LIMITE_SALDO;
        }

        public virtual bool TieneSaldoSuficiente(decimal tarifa)
        {
            decimal tarifaAPagar = tarifa;
            return saldo >= tarifaAPagar || saldo - tarifaAPagar >= LIMITE_NEGATIVO;
        }

        public void DebitarSaldoPorViajes(int cantidadViajes, Tiempo tiempoPorViaje, Fecha fecha)
        {
            for (int i = 0; i < cantidadViajes; i++)
            {
                DebitarSaldo(tiempoPorViaje, fecha); 
                AcreditarSaldoPendiente(); 
            }
            tiempoAcumulado.Sumar(new Tiempo(0, cantidadViajes * tiempoPorViaje.ObtenerMinutos()));
        }

        public Tiempo ObtenerTiempoAcumulado() => tiempoAcumulado;

        public virtual void DebitarSaldo(Tiempo tiempoActual, Fecha fecha)
        {
            Console.WriteLine("Entrando a DebitarSaldo en Tarjeta."); // Debugging line
            contadorViajes++;

            decimal tarifaAPagar = CalcularTarifa();
            

            Console.WriteLine($"Saldo {ObtenerSaldo()}");
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

            
            saldo -= tarifaAPagar;
            Console.WriteLine($"Saldo {ObtenerSaldo()}");

            ultimoPago = tarifaAPagar;
            AcreditarSaldoPendiente();
            UpdateSaldoNegativoState();
            Console.WriteLine($"UltimoPago {ultimoPago}");
        }



        protected decimal CalcularTarifa()
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

        public virtual decimal ObtenerUltimoPago() => ultimoPago;

        public void RecargarSaldo(decimal monto)
        {
            if (monto <= 0)
                throw new ArgumentException("El monto de recarga debe ser positivo.");

            if (saldo + monto > LIMITE_SALDO)
            {
                decimal cargaPosible = LIMITE_SALDO - saldo;
                saldo += cargaPosible; 
                saldoPendiente += (monto - cargaPosible); 
                Console.WriteLine($"Carga parcial. Se acreditaron {cargaPosible} pesos, {monto - cargaPosible} quedan pendientes.");
            }
            else
            {
                saldo += monto;
                Console.WriteLine($"Se acreditaron {monto} pesos.");
            }

            UpdateSaldoNegativoState();
        }

        protected void AcreditarSaldoPendiente()
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

        public virtual void UpdateSaldoNegativoState()
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

        public virtual bool SaldoNegativoCancelado()
        {
            return saldo >= 0 && saldoFueNegativo;
        }

        protected static string GenerarIDNumerico(int longitud)
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