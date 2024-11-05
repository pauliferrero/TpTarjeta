using NUnit.Framework;
using System;

namespace TpTarjeta.Tests
{
    [TestFixture]
    public class ColectivoTests
    {
        ColectivoInterurbano colectivoInterurbano;
        Tarjeta tarjeta;
        Tiempo tiempo;
        MedioBoleto medioBoleto;
        FranquiciaCompleta franquiciaCompleta;

        [SetUp]
        public void Setup()
        {
            colectivoInterurbano = new ColectivoInterurbano();
            tarjeta = new Tarjeta(3000); 
            tiempo = new Tiempo(10,0); 
            medioBoleto = new MedioBoleto(3000, tiempo);
            franquiciaCompleta = new FranquiciaCompleta(3000, tiempo);
        }

        [Test]
        public void PagarCon_TarifaNormal()
        {
            var boleto = colectivoInterurbano.PagarCon(tarjeta, tiempo);

            Assert.Multiple(() =>
            {
                Assert.That(boleto, Is.Not.Null, "El boleto no debería ser nulo.");
                Assert.That(boleto.Fecha.ObtenerHoras(), Is.EqualTo(tiempo.ObtenerHoras()), $"Se esperaba la hora '{tiempo.ObtenerHoras()}', pero se obtuvo '{boleto.Fecha.ObtenerHoras()}'.");
                Assert.That(boleto.Fecha.ObtenerMinutos(), Is.EqualTo(tiempo.ObtenerMinutos()), $"Se esperaba los minutos '{tiempo.ObtenerMinutos()}', pero se obtuvo '{boleto.Fecha.ObtenerMinutos()}'.");
                Assert.That(boleto.LineaColectivo, Is.EqualTo("Expresso"), $"Se esperaba la línea de colectivo 'Expresso', pero se obtuvo '{boleto.LineaColectivo}'.");
                Assert.That(boleto.TotalAbonado, Is.EqualTo(tarjeta.ObtenerUltimoPago()), $"Se esperaba un total abonado de '{tarjeta.ObtenerUltimoPago()}', pero se obtuvo '{boleto.TotalAbonado}'.");
                Assert.That(boleto.SaldoRestante, Is.EqualTo(tarjeta.ObtenerSaldo()), $"Se esperaba un saldo restante de '{tarjeta.ObtenerSaldo()}', pero se obtuvo '{boleto.SaldoRestante}'.");
                Assert.That(boleto.IdTarjeta, Is.EqualTo(tarjeta.ObtenerID()), $"Se esperaba ID de tarjeta '{tarjeta.ObtenerID()}', pero se obtuvo '{boleto.IdTarjeta}'.");
                Assert.That(boleto.CancelacionSaldoNegativo, Is.False, "Se esperaba que no se cancelara el saldo negativo.");
            });
        }

        [Test]
        public void PagarCon_MedioBoleto()
        {
            var boleto = colectivoInterurbano.PagarCon(medioBoleto, tiempo);
            decimal TotalAbonado = tarjeta.ObtenerUltimoPago();

            Assert.Multiple(() =>
            {
                Assert.That(boleto, Is.Not.Null, "El boleto no debería ser nulo.");
                Assert.That(boleto.Fecha.ObtenerHoras(), Is.EqualTo(tiempo.ObtenerHoras()), $"Se esperaba la hora '{tiempo.ObtenerHoras()}', pero se obtuvo '{boleto.Fecha.ObtenerHoras()}'.");
                Assert.That(boleto.Fecha.ObtenerMinutos(), Is.EqualTo(tiempo.ObtenerMinutos()), $"Se esperaba los minutos '{tiempo.ObtenerMinutos()}', pero se obtuvo '{boleto.Fecha.ObtenerMinutos()}'.");
                Assert.That(boleto.LineaColectivo, Is.EqualTo("Expresso"), $"Se esperaba la línea de colectivo 'Expresso', pero se obtuvo '{boleto.LineaColectivo}'.");
                Assert.That(TotalAbonado, Is.EqualTo(600m), $"Se esperaba un total abonado de '600', pero se obtuvo '{boleto.TotalAbonado}'.");
                Assert.That(boleto.SaldoRestante, Is.EqualTo(medioBoleto.ObtenerSaldo()), $"Se esperaba un saldo restante de '{medioBoleto.ObtenerSaldo()}', pero se obtuvo '{boleto.SaldoRestante}'.");
                Assert.That(boleto.IdTarjeta, Is.EqualTo(medioBoleto.ObtenerID()), $"Se esperaba ID de tarjeta '{medioBoleto.ObtenerID()}', pero se obtuvo '{boleto.IdTarjeta}'.");
                Assert.That(boleto.CancelacionSaldoNegativo, Is.False, "Se esperaba que no se cancelara el saldo negativo.");
            });
        }

        [Test]
        public void PagarCon_FranquiciaCompleta()
        {
            var boleto = colectivoInterurbano.PagarCon(franquiciaCompleta, tiempo);

            Assert.Multiple(() =>
            {
                Assert.That(boleto, Is.Not.Null, "El boleto no debería ser nulo.");
                Assert.That(boleto.Fecha.ObtenerHoras(), Is.EqualTo(tiempo.ObtenerHoras()), $"Se esperaba la hora '{tiempo.ObtenerHoras()}', pero se obtuvo '{boleto.Fecha.ObtenerHoras()}'.");
                Assert.That(boleto.Fecha.ObtenerMinutos(), Is.EqualTo(tiempo.ObtenerMinutos()), $"Se esperaba los minutos '{tiempo.ObtenerMinutos()}', pero se obtuvo '{boleto.Fecha.ObtenerMinutos()}'.");
                Assert.That(boleto.LineaColectivo, Is.EqualTo("Expresso"), $"Se esperaba la línea de colectivo 'Expresso', pero se obtuvo '{boleto.LineaColectivo}'.");
                Assert.That(boleto.TotalAbonado, Is.EqualTo(0m), $"Se esperaba un total abonado de '0', pero se obtuvo '{boleto.TotalAbonado}'.");
                Assert.That(boleto.SaldoRestante, Is.EqualTo(franquiciaCompleta.ObtenerSaldo()), $"Se esperaba un saldo restante de '{franquiciaCompleta.ObtenerSaldo()}', pero se obtuvo '{boleto.SaldoRestante}'.");
                Assert.That(boleto.IdTarjeta, Is.EqualTo(franquiciaCompleta.ObtenerID()), $"Se esperaba ID de tarjeta '{franquiciaCompleta.ObtenerID()}', pero se obtuvo '{boleto.IdTarjeta}'.");
                Assert.That(boleto.CancelacionSaldoNegativo, Is.False, "Se esperaba que no se cancelara el saldo negativo.");
            });
        }

    }
}
