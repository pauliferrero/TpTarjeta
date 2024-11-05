using NUnit.Framework;
using TpTarjeta;

namespace TpTarjeta.Tests
{
    public class BoletoTests
    {
        Tiempo tiempo;
        Fecha fecha;

        [SetUp]
        public void Setup()
        {
            tiempo = new Tiempo(10, 0);
            fecha = new Fecha(DiaDeLaSemana.Lunes, 11, 2024);
        }

        // Iteración 2
        [Test]
        public void CrearBoleto_FranciquiaCompleta()
        {
            var tipoTarjeta = "Franquicia Completa";
            var lineaColectivo = "102 144";
            var totalAbonado = 940m; // Tarifa completa (antes de actualizar la tarifa)
            var saldoRestante = 100m;
            var idTarjeta = "12345";
            var cancelacionSaldoNegativo = false;

            var boleto = new Boleto(fecha, tiempo, tipoTarjeta, lineaColectivo, totalAbonado, saldoRestante, idTarjeta, cancelacionSaldoNegativo);

            Assert.Multiple(() =>
            {
                Assert.That(boleto.Fecha.Dia, Is.EqualTo(fecha.Dia), "El día del boleto no es correcto.");
                Assert.That(boleto.Fecha.Mes, Is.EqualTo(fecha.Mes), "El mes del boleto no es correcto.");
                Assert.That(boleto.Fecha.Año, Is.EqualTo(fecha.Año), "El año del boleto no es correcto.");
                Assert.That(boleto.Hora.ObtenerHoras(), Is.EqualTo(tiempo.ObtenerHoras()), "Las horas del boleto no son correctas.");
                Assert.That(boleto.Hora.ObtenerMinutos(), Is.EqualTo(tiempo.ObtenerMinutos()), "Los minutos del boleto no son correctos.");
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

            var boleto = new Boleto(fecha, tiempo, tipoTarjeta, lineaColectivo, totalAbonado, saldoRestante, idTarjeta, cancelacionSaldoNegativo);

            Assert.Multiple(() =>
            {
                Assert.That(boleto.Fecha.Dia, Is.EqualTo(fecha.Dia), "El día del boleto no es correcto.");
                Assert.That(boleto.Fecha.Mes, Is.EqualTo(fecha.Mes), "El mes del boleto no es correcto.");
                Assert.That(boleto.Fecha.Año, Is.EqualTo(fecha.Año), "El año del boleto no es correcto.");
                Assert.That(boleto.Hora.ObtenerHoras(), Is.EqualTo(tiempo.ObtenerHoras()), "Las horas del boleto no son correctas.");
                Assert.That(boleto.Hora.ObtenerMinutos(), Is.EqualTo(tiempo.ObtenerMinutos()), "Los minutos del boleto no son correctos.");
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