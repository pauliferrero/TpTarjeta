using NUnit.Framework;
using TpTarjeta;

namespace TpTarjeta.Tests
{
    [TestFixture]
    internal class FranquiciaCompletaTests
    {
        [Test]
        public void FranquiciaCompleta_PuedePagarBoleto()
        {
            // Arrange
            var tiempo = new Tiempo(10, 0);
            var tarjeta = new FranquiciaCompleta(0, tiempo);
            var colectivo = new Colectivo();

            // Act
            try
            {
                colectivo.PagarCon(tarjeta, tiempo);
                Console.WriteLine("La tarjeta de Franquicia Completa pudo pagar el boleto.");
            }
            catch (Exception ex)
            {
                Assert.Fail($"La tarjeta de Franquicia Completa no pudo pagar el boleto: {ex.Message}");
            }

            // Assert (opcional)
            Assert.IsTrue(true); // Siempre pasa para indicar que no hubo excepciones
        }

        

        [Test]
        public void FranquiciaCompleta_CobraViajeCuandoSeSuperaLimiteGratuito()
        {
            // Arrange
            var tiempo = new Tiempo(10, 0);
            var tarjeta = new FranquiciaCompleta(1000, tiempo); // Suficiente saldo para cubrir tarifas
            var colectivo = new Colectivo();

            // Act
            colectivo.PagarCon(tarjeta, tiempo); // 1er viaje gratuito
            tiempo.SumarMinutos(5); // 5 minutos después
            colectivo.PagarCon(tarjeta, tiempo); // 2do viaje gratuito

            // Intentar un tercer viaje, que debería cobrarse
            tiempo.SumarMinutos(5); // 5 minutos después para intentar un tercer viaje

            Assert.DoesNotThrow(() => colectivo.PagarCon(tarjeta, tiempo), "Se esperaba que no se lanzara una excepción al intentar realizar un tercer viaje.");

            // Verifica que el saldo se haya debitado correctamente después del tercer viaje
            Assert.That(tarjeta.ObtenerSaldo(), Is.EqualTo(1000 - 940), "El saldo no se debió debitar correctamente después del tercer viaje.");
        }

        [Test]
        public void FranquiciaCompleta_NoPermiteMasDeDosViajesGratuitosPorDia()
        {
            // Arrange
            var tiempo = new Tiempo(10, 0);
            var tarjeta = new FranquiciaCompleta(10, tiempo); // Suficiente saldo para cubrir un viaje
            var colectivo = new Colectivo();

            // Act
            Console.WriteLine("Iniciando test de límite de dos viajes gratuitos por día.");
            Console.WriteLine($"Tiempo actual: {tiempo.ObtenerTiempoActual()}");

            // Intentar el primer viaje gratuito
            Console.WriteLine("Intentando el primer viaje gratuito...");
            tarjeta.RealizarViaje(tiempo);
            Console.WriteLine($"Saldo después del primer viaje: {tarjeta.ObtenerSaldo()}");

            // Intentar el segundo viaje gratuito
            tiempo.SumarMinutos(5); // 5 minutos después
            Console.WriteLine($"Tiempo actual: {tiempo.ObtenerTiempoActual()}");
            Console.WriteLine("Intentando el segundo viaje gratuito...");
            tarjeta.RealizarViaje(tiempo);
            Console.WriteLine($"Saldo después del segundo viaje: {tarjeta.ObtenerSaldo()}");

            // Intentar un tercer viaje, que debería lanzar una excepción
            tiempo.SumarMinutos(5); // 5 minutos después para intentar un tercer viaje
            Console.WriteLine($"Tiempo actual: {tiempo.ObtenerTiempoActual()}");
            Console.WriteLine("Intentando el tercer viaje, que debería lanzar una excepción...");

            Assert.Throws<InvalidOperationException>(() => tarjeta.RealizarViaje(tiempo),
                "Se esperaba una excepción al intentar realizar un tercer viaje gratuito.");
        }




    }

}
