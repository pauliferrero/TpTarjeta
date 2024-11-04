using System;

namespace TpTarjeta
{
    public class FranquiciaCompleta : Tarjeta
    {
        private const int MaxViajesGratuitosPorDia = 2; // Máximo de viajes gratuitos por día
        private int viajesRealizadosHoy; // Contador de viajes realizados hoy
        private Tiempo tiempoUltimoViaje; // Tiempo del último viaje
        private Tiempo tiempoActual; // Tiempo actual que se recibe al momento del viaje

        // Constructor
        public FranquiciaCompleta(decimal saldoInicial, Tiempo tiempoInicial) : base(saldoInicial)
        {
            viajesRealizadosHoy = 0;
            tiempoUltimoViaje = tiempoInicial; // Inicializa en el tiempo proporcionado
            tiempoActual = tiempoInicial; // Al principio el tiempo actual es igual al tiempo del último viaje
        }

        public override bool TieneSaldoSuficiente()
        {
            // Permite pagar siempre que haya viajes gratuitos disponibles o saldo suficiente
            return viajesRealizadosHoy < MaxViajesGratuitosPorDia || base.TieneSaldoSuficiente();
        }

        public override void DebitarSaldo(Tiempo tiempo)
        {
            if (!EsHorarioValido(tiempo))
            {
                throw new InvalidOperationException("No se puede realizar el viaje fuera de la franja horaria permitida.");
            }

            if (viajesRealizadosHoy >= MaxViajesGratuitosPorDia)
            {
                // Si se han superado los viajes gratuitos, se debe debitar el saldo como una tarjeta normal
                base.DebitarSaldo(tiempo);
            }
            else
            {
                // Incrementar el contador de viajes gratuitos
                viajesRealizadosHoy++;
            }

            // Actualizamos el tiempo del último viaje
            tiempoUltimoViaje = tiempo;
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


        // Método para realizar un viaje
        public void RealizarViaje(Tiempo tiempo)
        {
            tiempoActual = tiempo; // Actualizamos el tiempo actual con el nuevo tiempo proporcionado

            // Simular el nuevo día: si la hora del último viaje es diferente, reiniciar el contador
            if (EsNuevoDia())
            {
                viajesRealizadosHoy = 0; // Reseteamos el contador de viajes gratuitos
            }

            // Si ya se han realizado los viajes gratuitos del día, verificar el saldo
            if (viajesRealizadosHoy >= MaxViajesGratuitosPorDia)
            {
                if (!TieneSaldoSuficiente())
                {
                    throw new InvalidOperationException("No tienes saldo suficiente para realizar el viaje.");
                }

                // Si se han superado los viajes gratuitos, se debe debitar el saldo como una tarjeta normal
                DebitarSaldo(tiempo); // Debitar saldo si no quedan viajes gratuitos
            }
            else
            {
                // Incrementar el contador de viajes gratuitos
                viajesRealizadosHoy++;
            }

            // Actualizamos la hora del último viaje al tiempo actual después de realizar el viaje
            tiempoUltimoViaje = tiempoActual;
        }


        private bool EsNuevoDia()
        {
            // Lógica para determinar si es un nuevo día basado en el objeto tiempo
            // Comparamos si ha pasado un cambio de día entre el último viaje y el actual
            return tiempoUltimoViaje.ObtenerHoras() != tiempoActual.ObtenerHoras();
        }
    }
}
