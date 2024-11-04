using System;

namespace TpTarjeta
{
    public class MedioBoleto : Tarjeta
    {
        private Tiempo? ultimoViaje; // Guarda el tiempo del último viaje
        private decimal ultimoPago;
        private const decimal TARIFA_MEDIO_BOLETO = 600m; // Tarifa específica para medio boleto

        // Constructor
        public MedioBoleto(decimal saldoInicial, Tiempo tiempoInicial) : base(saldoInicial)
        {
            ultimoViaje = null; // Inicializa como null
        }

        public override void DebitarSaldo(Tiempo tiempoActual)
        {
            if (!EsHorarioValido(tiempoActual))
                throw new InvalidOperationException("No se puede realizar el viaje fuera del horario permitido.");

            if (!TieneSaldoSuficiente())
                throw new InvalidOperationException("Saldo insuficiente.");

            // Si no se ha realizado ningún viaje, simplemente registra el viaje actual
            if (ultimoViaje == null)
            {
                ultimoViaje = tiempoActual; // Registra el tiempo del primer viaje
                ultimoPago = TARIFA_MEDIO_BOLETO; // Registra el pago
                saldo -= TARIFA_MEDIO_BOLETO; // Debita el saldo
                return; // Termina el método aquí
            }

            // Si hay un viaje previo, comprueba si ha pasado el tiempo requerido
            if (ultimoViaje != null && !tiempoActual.HaPasadoSuficienteTiempo((Tiempo)ultimoViaje, 5)) // 5 minutos requeridos
            {
                throw new InvalidOperationException("No ha pasado suficiente tiempo desde el último viaje.");
            }

            // Registra el último pago realizado con la tarifa del medio boleto
            ultimoPago = TARIFA_MEDIO_BOLETO; // Guardamos el monto debito
            saldo -= TARIFA_MEDIO_BOLETO; // Se debita el saldo de la tarjeta
            ultimoViaje = tiempoActual; // Actualiza el último viaje al tiempo actual
        }

        private bool EsHorarioValido(Tiempo tiempo)
        {
            int hora = tiempo.ObtenerHoras();
            int minutos = tiempo.ObtenerMinutos();

            // Se acepta desde las 6:00 hasta las 22:00 (22:00 incluido)
            if (hora < 6 || (hora == 22 && minutos > 0))
            {
                return false;
            }

            return true; // Dentro de horario permitido
        }


        // Método para obtener el último pago realizado
        public new decimal ObtenerUltimoPago() => ultimoPago;
    }
}
