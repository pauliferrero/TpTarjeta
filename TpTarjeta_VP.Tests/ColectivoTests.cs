using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TpTarjeta.Tests
{
    public class ColectivoTests
    {
        Colectivo colectivo;
        Tarjeta tarjeta;
        Tiempo tiempo;
        MedioBoleto medioBoleto;
        FranquiciaCompleta franquiciaCompleta;
        Fecha fecha;

        [SetUp]
        public void Setup()
        {
            colectivo = new Colectivo();
            tarjeta = new Tarjeta(3000);
            tiempo = new Tiempo(10, 0);
            medioBoleto = new MedioBoleto(3000, tiempo);
            franquiciaCompleta = new FranquiciaCompleta(3000, tiempo);
            colectivo = new Colectivo();
            fecha = new Fecha(DiaDeLaSemana.Lunes, 11, 2024);
        }

        // Iteración 4
        [Test]
        public void PagarCon_TarifaNormal()
        {
            var boleto = colectivo.PagarCon(tarjeta, tiempo, fecha);

            Assert.Multiple(() =>
            {
                Assert.That(boleto, Is.Not.Null, "El boleto no debería ser nulo.");
                Assert.That(boleto.Fecha.Dia, Is.EqualTo(fecha.Dia), $"Se esperaba el día '{fecha.Dia}', pero se obtuvo '{boleto.Fecha.Dia}'.");
                Assert.That(boleto.Fecha.Mes, Is.EqualTo(fecha.Mes), $"Se esperaba el mes '{fecha.Mes}', pero se obtuvo '{boleto.Fecha.Mes}'.");
                Assert.That(boleto.Fecha.Año, Is.EqualTo(fecha.Año), $"Se esperaba el año '{fecha.Año}', pero se obtuvo '{boleto.Fecha.Año}'.");
                Assert.That(boleto.Hora.ObtenerHoras(), Is.EqualTo(tiempo.ObtenerHoras()), $"Se esperaba la hora '{tiempo.ObtenerHoras()}', pero se obtuvo '{boleto.Hora.ObtenerHoras()}'.");
                Assert.That(boleto.Hora.ObtenerMinutos(), Is.EqualTo(tiempo.ObtenerMinutos()), $"Se esperaba los minutos '{tiempo.ObtenerMinutos()}', pero se obtuvo '{boleto.Hora.ObtenerMinutos()}'.");
                Assert.That(boleto.LineaColectivo, Is.EqualTo("102 144"), $"Se esperaba la línea de colectivo 'Expresso', pero se obtuvo '{boleto.LineaColectivo}'.");
                Assert.That(boleto.TotalAbonado, Is.EqualTo(tarjeta.ObtenerUltimoPago()), $"Se esperaba un total abonado de '{tarjeta.ObtenerUltimoPago()}', pero se obtuvo '{boleto.TotalAbonado}'.");
                Assert.That(boleto.SaldoRestante, Is.EqualTo(tarjeta.ObtenerSaldo()), $"Se esperaba un saldo restante de '{tarjeta.ObtenerSaldo()}', pero se obtuvo '{boleto.SaldoRestante}'.");
                Assert.That(boleto.IdTarjeta, Is.EqualTo(tarjeta.ObtenerID()), $"Se esperaba ID de tarjeta '{tarjeta.ObtenerID()}', pero se obtuvo '{boleto.IdTarjeta}'.");
                Assert.That(boleto.CancelacionSaldoNegativo, Is.False, "Se esperaba que no se cancelara el saldo negativo.");
            });
        }

        [Test]
        public void PagarCon_MedioBoleto()
        {
            var boleto = colectivo.PagarCon(medioBoleto, tiempo, fecha);

            Assert.Multiple(() =>
            {
                Assert.That(boleto, Is.Not.Null, "El boleto no debería ser nulo.");
                Assert.That(boleto.Fecha.Dia, Is.EqualTo(fecha.Dia), $"Se esperaba el día '{fecha.Dia}', pero se obtuvo '{boleto.Fecha.Dia}'.");
                Assert.That(boleto.Fecha.Mes, Is.EqualTo(fecha.Mes), $"Se esperaba el mes '{fecha.Mes}', pero se obtuvo '{boleto.Fecha.Mes}'.");
                Assert.That(boleto.Fecha.Año, Is.EqualTo(fecha.Año), $"Se esperaba el año '{fecha.Año}', pero se obtuvo '{boleto.Fecha.Año}'.");
                Assert.That(boleto.Hora.ObtenerHoras(), Is.EqualTo(tiempo.ObtenerHoras()), $"Se esperaba la hora '{tiempo.ObtenerHoras()}', pero se obtuvo '{boleto.Hora.ObtenerHoras()}'.");
                Assert.That(boleto.Hora.ObtenerMinutos(), Is.EqualTo(tiempo.ObtenerMinutos()), $"Se esperaba los minutos '{tiempo.ObtenerMinutos()}', pero se obtuvo '{boleto.Hora.ObtenerMinutos()}'.");
                Assert.That(boleto.LineaColectivo, Is.EqualTo("102 144"), $"Se esperaba la línea de colectivo 'Expresso', pero se obtuvo '{boleto.LineaColectivo}'.");
                Assert.That(boleto.TotalAbonado, Is.EqualTo(medioBoleto.ObtenerUltimoPago()), $"Se esperaba un total abonado de '600', pero se obtuvo '{boleto.TotalAbonado}'.");
                Assert.That(boleto.SaldoRestante, Is.EqualTo(medioBoleto.ObtenerSaldo()), $"Se esperaba un saldo restante de '{medioBoleto.ObtenerSaldo()}', pero se obtuvo '{boleto.SaldoRestante}'.");
                Assert.That(boleto.IdTarjeta, Is.EqualTo(medioBoleto.ObtenerID()), $"Se esperaba ID de tarjeta '{medioBoleto.ObtenerID()}', pero se obtuvo '{boleto.IdTarjeta}'.");
                Assert.That(boleto.CancelacionSaldoNegativo, Is.False, "Se esperaba que no se cancelara el saldo negativo.");
            });
        }

        [Test]
        public void PagarCon_FranquiciaCompleta()
        {
            var boleto = colectivo.PagarCon(franquiciaCompleta, tiempo, fecha);

            Assert.Multiple(() =>
            {
                Assert.That(boleto, Is.Not.Null, "El boleto no debería ser nulo.");
                Assert.That(boleto.Fecha.Dia, Is.EqualTo(fecha.Dia), $"Se esperaba el día '{fecha.Dia}', pero se obtuvo '{boleto.Fecha.Dia}'.");
                Assert.That(boleto.Fecha.Mes, Is.EqualTo(fecha.Mes), $"Se esperaba el mes '{fecha.Mes}', pero se obtuvo '{boleto.Fecha.Mes}'.");
                Assert.That(boleto.Fecha.Año, Is.EqualTo(fecha.Año), $"Se esperaba el año '{fecha.Año}', pero se obtuvo '{boleto.Fecha.Año}'.");
                Assert.That(boleto.Hora.ObtenerHoras(), Is.EqualTo(tiempo.ObtenerHoras()), $"Se esperaba la hora '{tiempo.ObtenerHoras()}', pero se obtuvo '{boleto.Hora.ObtenerHoras()}'.");
                Assert.That(boleto.Hora.ObtenerMinutos(), Is.EqualTo(tiempo.ObtenerMinutos()), $"Se esperaba los minutos '{tiempo.ObtenerMinutos()}', pero se obtuvo '{boleto.Hora.ObtenerMinutos()}'.");
                Assert.That(boleto.LineaColectivo, Is.EqualTo("102 144"), $"Se esperaba la línea de colectivo 'Expresso', pero se obtuvo '{boleto.LineaColectivo}'.");
                Assert.That(boleto.TotalAbonado, Is.EqualTo(0m), $"Se esperaba un total abonado de '0', pero se obtuvo '{boleto.TotalAbonado}'.");
                Assert.That(boleto.SaldoRestante, Is.EqualTo(franquiciaCompleta.ObtenerSaldo()), $"Se esperaba un saldo restante de '{franquiciaCompleta.ObtenerSaldo()}', pero se obtuvo '{boleto.SaldoRestante}'.");
                Assert.That(boleto.IdTarjeta, Is.EqualTo(franquiciaCompleta.ObtenerID()), $"Se esperaba ID de tarjeta '{franquiciaCompleta.ObtenerID()}', pero se obtuvo '{boleto.IdTarjeta}'.");
                Assert.That(boleto.CancelacionSaldoNegativo, Is.False, "Se esperaba que no se cancelara el saldo negativo.");
            });
        }
    }
}
