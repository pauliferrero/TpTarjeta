using System;

namespace Tp2AAT
{
    public class FranquiciaCompleta : Tarjeta
    {
        private const int MaxViajesGratuitosPorDia = 2; // Máximo de viajes gratuitos por día
        private int viajesRealizadosHoy; // Contador de viajes realizados hoy
        private DateTime fechaUltimoViaje; // Fecha del último viaje realizado

        public FranquiciaCompleta(decimal saldoInicial) : base(saldoInicial) 
        {
            viajesRealizadosHoy = 0;
            fechaUltimoViaje = DateTime.MinValue; // Inicializa en la fecha mínima
        }

        public override bool TieneSaldoSuficiente()
        {
            // Permite pagar siempre, pero verifica el límite de viajes
            return viajesRealizadosHoy < MaxViajesGratuitosPorDia;
        }

        public override void DebitarSaldo()
        {
            // No se debita nada si se realizan viajes gratuitos
            // Se debe manejar el caso de cobrar si se superan los viajes gratuitos
            if (viajesRealizadosHoy >= MaxViajesGratuitosPorDia)
            {
                // Aquí se podría lanzar una excepción o manejar el cobro normal
                throw new InvalidOperationException("Se cobrará el precio completo por este viaje.");
            }
            // Si no se han superado los viajes gratuitos, no se hace nada
        }

        public void RealizarViaje()
        {
            // Verificar si es un nuevo día
            if (fechaUltimoViaje.Date != DateTime.Now.Date)
            {
                // Reiniciar el contador si es un nuevo día
                viajesRealizadosHoy = 0;
                fechaUltimoViaje = DateTime.Now; // Actualizar la fecha del último viaje
            }

            if (viajesRealizadosHoy < MaxViajesGratuitosPorDia)
            {
                viajesRealizadosHoy++; // Incrementar el contador de viajes gratuitos
            }
            else
            {
                // Si se superan los viajes gratuitos, lanzar excepción
                throw new InvalidOperationException("Se cobrará el precio completo por este viaje.");
            }
        }
    }
}
