using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TpTarjeta
{
    public class ColectivoInterurbano : Colectivo
    {
        public const decimal tarifaInterurbana = 2500;
        public override decimal Tarifa => tarifaInterurbana;
        public override Boleto PagarCon(Tarjeta tarjeta, Tiempo tiempoActual, Fecha fechaActual)
        {
            if (tarjeta == null)
                throw new ArgumentNullException(nameof(tarjeta));

            if (!tarjeta.TieneSaldoSuficiente(Tarifa))
                throw new InvalidOperationException("Saldo insuficiente en la tarjeta.");

            tarjeta.DebitarSaldo(tiempoActual, fechaActual);

            var boleto = new Boleto(
                fecha: fechaActual,
                tiempo: tiempoActual,
                tipoTarjeta: tarjeta.GetType().Name,
                lineaColectivo: "Expresso",
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
