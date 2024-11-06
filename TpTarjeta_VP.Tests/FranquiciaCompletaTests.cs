using NUnit.Framework;
using TpTarjeta;

namespace TpTarjeta.Tests
{
    [TestFixture]
    public class FranquiciaCompletaTests
    {
        Tiempo tiempo;
        Colectivo colectivo;
        Fecha fecha;

        [SetUp]
        public void Setup()
        {
            tiempo = new Tiempo(10, 0);
            colectivo = new Colectivo();
            fecha = new Fecha(DiaDeLaSemana.Lunes, 11, 2024);
        }

        // Iteración 2
        [Test]
        public void PuedePagarBoleto()
        {
            var tarjeta = new FranquiciaCompleta(0, tiempo);

            Assert.That(() => colectivo.PagarCon(tarjeta, tiempo, fecha), Throws.Nothing, "La tarjeta de Franquicia Completa no pudo pagar el boleto.");
        }


        // Iteración 4
        [Test]
        public void ViajeFueraDeHorario()
        {
            var tarjeta = new FranquiciaCompleta(1000m, tiempo); 

            Assert.Throws<InvalidOperationException>(() => tarjeta.DebitarSaldo(new Tiempo(5, 0), fecha)); // 05:00
            Assert.Throws<InvalidOperationException>(() => tarjeta.DebitarSaldo(new Tiempo(22, 30), fecha)); // 22:30
        }

        [Test]
        public void ViajeFueraDeDiaValido()
        {
            var fechaSabado = new Fecha(DiaDeLaSemana.Sabado, 11, 2024);
            var tarjeta = new FranquiciaCompleta(1000m, tiempo);

            Assert.Throws<InvalidOperationException>(() => tarjeta.DebitarSaldo(new Tiempo(10, 0), fechaSabado)); 
        }
    }
}
