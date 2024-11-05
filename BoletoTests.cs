using NUnit.Framework;
using TpTarjeta;

namespace TpTarjeta.Tests
{
    public class BoletoTests
    {

        [Test]
        public void CrearBoleto_FranciquiaCompleta()
        {
            var tipoTarjeta = "Franquicia Completa";
            var lineaColectivo = "102 144";
            var totalAbonado = 940m; // Tarifa completa (antes de actualizar la tarifa)
            var saldoRestante = 100m; 
            var idTarjeta = "12345";
            var cancelacionSaldoNegativo = false;

            var boleto = new Boleto(tipoTarjeta, lineaColectivo, totalAbonado, saldoRestante, idTarjeta, cancelacionSaldoNegativo);

            Assert.Multiple(() =>
            {
                Assert.That(boleto.TipoTarjeta, Is.EqualTo(tipoTarjeta), "El tipo de tarjeta del boleto no es correcto.");
                Assert.That(boleto.LineaColectivo, Is.EqualTo(lineaColectivo), "La línea de colectivo del boleto no es correcta.");
                Assert.That(boleto.TotalAbonado, Is.EqualTo(totalAbonado), "El total abonado del boleto no es correcto.");
                Assert.That(boleto.SaldoRestante, Is.EqualTo(saldoRestante), "El saldo restante del boleto no es correcto.");
                Assert.That(boleto.IdTarjeta, Is.EqualTo(idTarjeta), "El ID de tarjeta del boleto no es correcto.");
                Assert.That(boleto.CancelacionSaldoNegativo, Is.EqualTo(cancelacionSaldoNegativo), "La cancelación de saldo negativo no es correcta.");
            });

            boleto.MostrarBoleto();
        }

        [Test]
        public void CrearBoleto_MedioBoleto()
        {
            var tipoTarjeta = "Medio Boleto";
            var lineaColectivo = "101 Negro";
            var totalAbonado = 470m; // Tarifa medio boleto (antes de actualizar la tarifa)
            var saldoRestante = 530m; 
            var idTarjeta = "67890";
            var cancelacionSaldoNegativo = false;

            var boleto = new Boleto(tipoTarjeta, lineaColectivo, totalAbonado, saldoRestante, idTarjeta, cancelacionSaldoNegativo);

            Assert.Multiple(() =>
            {
                Assert.That(boleto.TipoTarjeta, Is.EqualTo(tipoTarjeta), "El tipo de tarjeta del boleto no es correcto.");
                Assert.That(boleto.LineaColectivo, Is.EqualTo(lineaColectivo), "La línea de colectivo del boleto no es correcta.");
                Assert.That(boleto.TotalAbonado, Is.EqualTo(totalAbonado), "El total abonado del boleto no es correcto.");
                Assert.That(boleto.SaldoRestante, Is.EqualTo(saldoRestante), "El saldo restante del boleto no es correcto.");
                Assert.That(boleto.IdTarjeta, Is.EqualTo(idTarjeta), "El ID de tarjeta del boleto no es correcto.");
                Assert.That(boleto.CancelacionSaldoNegativo, Is.EqualTo(cancelacionSaldoNegativo), "La cancelación de saldo negativo no es correcta.");
            });

            boleto.MostrarBoleto();
        }
    }
}
