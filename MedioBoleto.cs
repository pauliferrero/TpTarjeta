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
