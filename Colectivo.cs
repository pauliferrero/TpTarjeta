using System;

namespace TpTarjeta
{
    public class Colectivo
    {
        public Boleto PagarCon(Tarjeta tarjeta, Tiempo tiempoActual)
        {
            if (tarjeta == null)
                throw new ArgumentNullException(nameof(tarjeta));

            if (!tarjeta.TieneSaldoSuficiente())
                throw new InvalidOperationException("Saldo insuficiente en la tarjeta.");



            tarjeta.DebitarSaldo(tiempoActual);

            // Creación del boleto con la información proporcionada por la tarjeta
            return new Boleto(
                tipoTarjeta: tarjeta.GetType().Name,
                lineaColectivo: "102 144",
                totalAbonado: tarjeta.ObtenerUltimoPago(),
                saldoRestante: tarjeta.ObtenerSaldo(),
                idTarjeta: tarjeta.ObtenerID(),
                cancelacionSaldoNegativo: tarjeta.SaldoNegativoCancelado()
            );
        }
    }

}
