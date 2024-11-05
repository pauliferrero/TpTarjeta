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
        public override Boleto PagarCon(Tarjeta tarjeta, Tiempo tiempoActual)
        {
            if (tarjeta == null)
                throw new ArgumentNullException(nameof(tarjeta));

            if (!tarjeta.TieneSaldoSuficiente(Tarifa))
                throw new InvalidOperationException("Saldo insuficiente en la tarjeta.");

            Console.WriteLine($"Antes de debitar: Saldo: {tarjeta.ObtenerSaldo()}, Último Pago: {tarjeta.ObtenerUltimoPago()}");
            tarjeta.DebitarSaldo(tiempoActual);
            Console.WriteLine($"Después de debitar: Saldo: {tarjeta.ObtenerSaldo()}, Último Pago: {tarjeta.ObtenerUltimoPago()}");

            var boleto = new Boleto(
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
