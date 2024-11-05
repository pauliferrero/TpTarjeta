using System;

namespace TpTarjeta
{
    public class FranquiciaCompleta : Tarjeta
    {
        private const int MaxViajesGratuitosPorDia = 2; 
        private int viajesRealizadosHoy; 
        private Tiempo tiempoUltimoViaje; 
        private Tiempo tiempoActual; 
        public const decimal tarifaFC = 0;

        public FranquiciaCompleta(decimal saldoInicial, Tiempo tiempoInicial) : base(saldoInicial)
        {
            viajesRealizadosHoy = 0;
            tiempoUltimoViaje = tiempoInicial; 
            tiempoActual = tiempoInicial; 
        }

        public override bool TieneSaldoSuficiente(decimal tarifa)
        {
            tarifa = tarifaFC;
            return viajesRealizadosHoy < MaxViajesGratuitosPorDia || base.TieneSaldoSuficiente(tarifa);
        }

        public override void DebitarSaldo(Tiempo tiempo)
        {
            if (!EsHorarioValido(tiempo))
            {
                throw new InvalidOperationException("No se puede realizar el viaje fuera de la franja horaria permitida.");
            }

            if (viajesRealizadosHoy >= MaxViajesGratuitosPorDia)
            {
                base.DebitarSaldo(tiempo);
            }
            else
            {
                viajesRealizadosHoy++;
            }

            tiempoUltimoViaje = tiempo;
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

        public void RealizarViaje(Tiempo tiempo)
        {
            tiempoActual = tiempo; 

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

                DebitarSaldo(tiempo);
            }
            else
            {
                viajesRealizadosHoy++;
            }

            tiempoUltimoViaje = tiempoActual;
        }


        private bool EsNuevoDia()
        {
            return tiempoUltimoViaje.ObtenerHoras() != tiempoActual.ObtenerHoras();
        }
    }
}