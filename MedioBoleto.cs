<<<<<<< HEAD
using System;

namespace TpTarjeta
{
    public class MedioBoleto : Tarjeta
    {
        private Tiempo? ultimoViaje; 
        new private decimal ultimoPago;
        private const decimal tarifaMedioBoleto = 600m; 
        private int contadorViajes;

        public MedioBoleto(decimal saldoInicial, Tiempo tiempoInicial) : base(saldoInicial)
        {
            ultimoViaje = null;
            contadorViajes = 0;
        }

        public override void DebitarSaldo(Tiempo tiempoActual)
        {
            if (!EsHorarioValido(tiempoActual))
            {
                throw new InvalidOperationException("No se puede realizar el viaje fuera del horario permitido.");
            }

            if (!TieneSaldoSuficiente(tarifaMedioBoleto))
            {
                throw new InvalidOperationException("Saldo insuficiente.");
            }

            if (contadorViajes >= 4)
            {
                throw new InvalidOperationException("No se pueden realizar más de cuatro viajes en un día con la tarjeta medio boleto.");
            }

            if (ultimoViaje != null && !tiempoActual.HaPasadoSuficienteTiempo(ultimoViaje, 5))
            {
                throw new InvalidOperationException("No ha pasado suficiente tiempo desde el último viaje.");
            }

            saldo -= tarifaMedioBoleto;
            ultimoViaje = tiempoActual;
            ultimoPago = tarifaMedioBoleto;
            contadorViajes++;

            Console.WriteLine($"Pago registrado. Saldo restante: {saldo}, total de viajes: {contadorViajes}, ultimo pago: {ultimoPago}");
        }

        public new decimal ObtenerUltimoPago()
        {
            Console.WriteLine($"Último Pago: {ultimoPago}");
            return ultimoPago;
        }

        private bool EsHorarioValido(Tiempo tiempo)
        {
            int hora = tiempo.ObtenerHoras();
            int minutos = tiempo.ObtenerMinutos();

            if (hora < 6 || (hora == 22 && minutos > 0))
            {
                return false;
            }

            return true;
        }
        
    }
}
=======
using System;

namespace Tp2AAT
{
    public class MedioBoleto : Tarjeta
    {
        private static readonly decimal TARIFA_MEDIO = TARIFA / 2;
        private static readonly TimeSpan TIEMPO_MINIMO_ENTRE_VIAJES = TimeSpan.FromSeconds(5); // Usamos la funcion TimeSpan.FromSeconds en vez de FromMinutes para no alargar la ejecución
        private DateTime ultimoViaje;  // Almacena la fecha y hora del último viaje.

        public MedioBoleto(decimal saldoInicial) : base(saldoInicial)
        {
            ultimoViaje = DateTime.MinValue;  // Inicializamos con una fecha por defecto muy antigua.
        }

        public override bool TieneSaldoSuficiente()
        {
            return saldo >= TARIFA_MEDIO || saldo - TARIFA_MEDIO >= LIMITE_NEGATIVO;
        }

        public override void DebitarSaldo()
        {
            // Verificar si han pasado 5 minutos desde el último viaje.
            if (DateTime.Now - ultimoViaje < TIEMPO_MINIMO_ENTRE_VIAJES)
            {
                throw new InvalidOperationException("Debe esperar 5 minutos antes de realizar otro viaje con medio boleto.");
            }

            if (!TieneSaldoSuficiente())
                throw new InvalidOperationException("Saldo insuficiente para realizar el pago con medio boleto.");

            // Actualizamos el saldo y la fecha del último viaje.
            saldo -= TARIFA_MEDIO;
            ultimoViaje = DateTime.Now;  // Registramos el momento en que se realiza el viaje.
        }
    }
}
>>>>>>> limitacion_MB
