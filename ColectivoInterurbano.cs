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
        public new Boleto PagarCon(Tarjeta tarjeta, Tiempo tiempoActual)
        {
            if (tarjeta == null)
                throw new ArgumentNullException(nameof(tarjeta));

            if (!tarjeta.TieneSaldoSuficiente(Tarifa))
                throw new InvalidOperationException("Saldo insuficiente en la tarjeta.");

            Console.WriteLine($"Antes de debitar: Saldo: {tarjeta.ObtenerSaldo()}, Último Pago: {tarjeta.ObtenerUltimoPago()}");
            tarjeta.DebitarSaldo(tiempoActual);
            Console.WriteLine($"Después de debitar: Saldo: {tarjeta.ObtenerSaldo()}, Último Pago: {tarjeta.ObtenerUltimoPago()}");

            // Obtén el valor de ultimoPago directamente después de DebitarSaldo
            decimal totalAbonado = tarjeta.ultimoPago;  // Accede directamente a la variable
            Console.WriteLine($"Total Abonado (directo): {totalAbonado}");

            var boleto = new Boleto(
                tipoTarjeta: tarjeta.GetType().Name,
                lineaColectivo: "Expresso",
                totalAbonado: totalAbonado,
                saldoRestante: tarjeta.ObtenerSaldo(),
                idTarjeta: tarjeta.ObtenerID(),
                cancelacionSaldoNegativo: tarjeta.SaldoNegativoCancelado()
            );

            // Mostrar el boleto por pantalla
            boleto.MostrarBoleto();

            return boleto;
        }

    }
}
