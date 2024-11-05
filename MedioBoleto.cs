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