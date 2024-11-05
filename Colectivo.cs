using System;

namespace TpTarjeta
{
    public class Colectivo
    {
        public const decimal tarifa = 1200;
        public virtual decimal Tarifa => tarifa;
        public virtual Boleto PagarCon(Tarjeta tarjeta, Tiempo tiempoActual, Fecha fechaActual)
        {
            if (tarjeta == null)
                throw new ArgumentNullException(nameof(tarjeta));

            if (!tarjeta.TieneSaldoSuficiente(tarifa))
                throw new InvalidOperationException("Saldo insuficiente en la tarjeta.");

            tarjeta.DebitarSaldo(tiempoActual, fechaActual);

            var boleto = new Boleto(
                fecha: fechaActual,
                tiempo: tiempoActual,
                tipoTarjeta: tarjeta.GetType().Name,
                lineaColectivo: "102 144",
                totalAbonado: tarjeta.ObtenerUltimoPago(),
                saldoRestante: tarjeta.ObtenerSaldo(),
                idTarjeta: tarjeta.ObtenerID(),
                cancelacionSaldoNegativo: tarjeta.SaldoNegativoCancelado()
                );

            boleto.MostrarBoleto();

            return boleto;
        }
    }

}