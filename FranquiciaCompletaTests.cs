using NUnit.Framework;
using TpTarjeta;

namespace TpTarjeta.Tests
{
    [TestFixture]
    public class FranquiciaCompletaTests
    {
        Tiempo tiempo;
        Colectivo colectivo;

        [SetUp]
        public void Setup()
        {
            tiempo = new Tiempo(10, 0);
            colectivo = new Colectivo();
        }

        [Test]
        public void PuedePagarBoleto()
        {
            var tarjeta = new FranquiciaCompleta(0, tiempo);

            Assert.That(() => colectivo.PagarCon(tarjeta, tiempo), Throws.Nothing, "La tarjeta de Franquicia Completa no pudo pagar el boleto.");
        }

        [Test]
        public void CobraViajeCuandoSeSuperaLimiteGratuito()
        {
            var tarjeta = new FranquiciaCompleta(1200, tiempo); // Suficiente saldo para cubrir tarifas

            colectivo.PagarCon(tarjeta, tiempo); // 1er viaje gratuito
            tiempo.SumarMinutos(5); 
            colectivo.PagarCon(tarjeta, tiempo); // 2do viaje gratuito
            tiempo.SumarMinutos(5); 

            Assert.DoesNotThrow(() => colectivo.PagarCon(tarjeta, tiempo), "Se esperaba que no se lanzara una excepción al intentar realizar un tercer viaje.");
            Assert.That(tarjeta.ObtenerSaldo(), Is.EqualTo(1200 - 1200), "El saldo no se debió debitar correctamente después del tercer viaje.");
        }

        [Test]
        public void NoMasDeDosViajesGratuitosPorDia()
        {
            var tarjeta = new FranquiciaCompleta(10, tiempo); 
            tarjeta.DebitarSaldo(tiempo);
            tiempo.SumarMinutos(5); 
            tarjeta.DebitarSaldo(tiempo);
            tiempo.SumarMinutos(5); 

            Assert.Throws<InvalidOperationException>(() => tarjeta.DebitarSaldo(tiempo), "Se esperaba una excepción al intentar realizar un tercer viaje gratuito.");
        }

        [Test]
        public void ViajeFueraDeHorario()
        {
            var tarjeta = new FranquiciaCompleta(1000m, tiempo); 

            Assert.Throws<InvalidOperationException>(() => tarjeta.DebitarSaldo(new Tiempo(5, 0))); // 05:00
            Assert.Throws<InvalidOperationException>(() => tarjeta.DebitarSaldo(new Tiempo(22, 30))); // 22:30
        }
    }
}
