using System;

namespace TpTarjeta
{
    public class FranquiciaCompleta : Tarjeta
    {
        protected Tiempo tiempoUltimoViaje;
        protected Tiempo tiempoActual;
        public const decimal tarifaFC = 0;
        protected const int MaxViajesGratuitosPorDia = 2;
        protected int viajesRealizadosHoy;
        new protected decimal ultimoPago;
        new protected bool saldoFueNegativo;

        public FranquiciaCompleta(decimal saldoInicial, Tiempo tiempoInicial) : base(saldoInicial)
        {
            viajesRealizadosHoy = 0;
            tiempoUltimoViaje = tiempoInicial;
            tiempoActual = tiempoInicial;
            ultimoPago = 0;
            saldoFueNegativo = saldo < 0;
        }

        public override bool TieneSaldoSuficiente(decimal tarifa)
        {
            tarifa = tarifaFC;
            return viajesRealizadosHoy < MaxViajesGratuitosPorDia || base.TieneSaldoSuficiente(tarifa);
        }

        public override void DebitarSaldo(Tiempo tiempo, Fecha fecha)
        {
            if (!EsHorarioValido(tiempo, fecha))
            {
                throw new InvalidOperationException("No se puede realizar el viaje fuera de la franja horaria permitida.");
            }

            if (EsNuevoDia())
            {
                viajesRealizadosHoy = 0;
            }

            if (viajesRealizadosHoy >= MaxViajesGratuitosPorDia)
            {
                if (!TieneSaldoSuficiente(tarifaFC))
                {
                    throw new InvalidOperationException("No tienes saldo suficiente para realizar el viaje.");
                }

                base.DebitarSaldo(tiempo, fecha);
                ultimoPago = tarifaFC;
            }
            else
            {
                viajesRealizadosHoy++;
            }

            tiempoUltimoViaje = tiempo;
            tiempoActual = tiempo;
            UpdateSaldoNegativoState();
        }

        public override decimal ObtenerUltimoPago() => ultimoPago;

        public static bool EsHorarioValido(Tiempo tiempo, Fecha fecha)
        {
            int hora = tiempo.ObtenerHoras();
            int minutos = tiempo.ObtenerMinutos();
            var dia = fecha.Dia;

            if (dia == DiaDeLaSemana.Sabado || dia == DiaDeLaSemana.Domingo)
            {
                return false;
            }

            if (hora < 6 || (hora == 22 && minutos > 0))
            {
                return false;
            }

            return true;
        }

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

        public bool EsNuevoDia()
        {
            return tiempoUltimoViaje.ObtenerHoras() != tiempoActual.ObtenerHoras();
        }
    }
}
