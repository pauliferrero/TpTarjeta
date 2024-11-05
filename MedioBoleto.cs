using System;

namespace TpTarjeta
{
    public class MedioBoleto : Tarjeta
    {
        private Tiempo? ultimoViaje; 
        new private decimal ultimoPago;
        private const decimal tarifaMedioBoleto = 600m; 
        private int contadorViajes;
        private bool saldoFueNegativo;

        public MedioBoleto(decimal saldoInicial, Tiempo tiempoInicial) : base(saldoInicial)
        {
            ultimoViaje = null;
            contadorViajes = 0;
            saldoFueNegativo = saldo < 0;
        }

        public override void DebitarSaldo(Tiempo tiempoActual, Fecha fecha)
        {
            if (!EsHorarioValido(tiempoActual, fecha))
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
            UpdateSaldoNegativoState();
            contadorViajes++;
            
        }

        public override decimal ObtenerUltimoPago() => ultimoPago;

        public override void UpdateSaldoNegativoState()
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

        public override bool SaldoNegativoCancelado()
        {
            return saldo >= 0 && saldoFueNegativo;
        }

        private static bool EsHorarioValido(Tiempo tiempo, Fecha fecha)
        {
            int hora = tiempo.ObtenerHoras();
            int minutos = tiempo.ObtenerMinutos();
            var dia = fecha.Dia;

            // Verificar si es sábado o domingo
            if (dia == DiaDeLaSemana.Sabado || dia == DiaDeLaSemana.Domingo)
            {
                return false; // No se permite viajar los fines de semana
            }

            // Verificar si la hora está dentro del rango permitido (lunes a viernes de 6 a 22 hs)
            if (hora < 6 || (hora == 22 && minutos > 0))
            {
                return false;
            }

            return true;
        }

    }
}