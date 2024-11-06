using System;

namespace TpTarjeta
{
    public class Jubilado : FranquiciaCompleta
    {
        public Jubilado(decimal saldoInicial, Tiempo tiempoInicial) : base(saldoInicial, tiempoInicial) { }

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

            if (!TieneSaldoSuficiente(tarifaFC))
            {
                throw new InvalidOperationException("No tienes saldo suficiente para realizar el viaje.");
            }

            ultimoPago = tarifaFC; 
            tiempoUltimoViaje = tiempo;
            tiempoActual = tiempo;
            UpdateSaldoNegativoState();
        }
    }
}
