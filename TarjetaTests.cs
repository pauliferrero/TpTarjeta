using NUnit.Framework;
using TpTarjeta;

namespace TpTarjeta.Tests
{
    public class TarjetaTests
    {
        [SetUp]
        public void Setup()
        {
            // Inicializa objetos comunes para todas las pruebas aquí, si lo necesitas.
        }

        [Test]
        public void PagarConSaldo_SaldoSuficiente_DebitaCorrectamente()
        {
            // Arrange
            var tiempo = new Tiempo(10, 0);
            var tarjeta = new Tarjeta(1000); // Tarjeta con saldo suficiente
            var colectivo = new Colectivo();

            // Act
            var boleto = colectivo.PagarCon(tarjeta, tiempo);

            // Assert
            Assert.That(boleto, Is.Not.Null, "El boleto debería haberse generado.");
            Assert.That(tarjeta.ObtenerSaldo(), Is.EqualTo(1000 - 940), "El saldo no se descontó correctamente.");

            // Output
            Console.WriteLine("El boleto fue generado correctamente y el saldo se actualizó.");
        }

        [Test]
        public void PagarSinSaldo_SaldoInsuficiente_LanzaExcepcion()
        {
            // Arrange
            var tiempo = new Tiempo(10, 0);
            var tarjeta = new Tarjeta(50); // Tarjeta con saldo insuficiente
            var colectivo = new Colectivo();

            // Act & Assert
            Assert.That(() => colectivo.PagarCon(tarjeta, tiempo), Throws.InvalidOperationException, "Se esperaba una excepción cuando el saldo es insuficiente.");

            // Output
            Console.WriteLine("Se lanzó una excepción como se esperaba debido al saldo insuficiente.");
        }

        [Test]
        public void PagarConSaldoInsuficiente_NoPermiteSaldoMenorA480_LanzaExcepcion()
        {
            // Arrange
            var tiempo = new Tiempo(10, 0);
            var tarjeta = new Tarjeta(-490); // Saldo inicial menor a -480
            var colectivo = new Colectivo();

            // Act & Assert
            Assert.That(() => colectivo.PagarCon(tarjeta, tiempo), Throws.InvalidOperationException, "Se esperaba una excepción cuando el saldo es menor a -480.");

            // Output
            Console.WriteLine("Se lanzó una excepción como se esperaba debido a un saldo menor a -480.");
        }

        [Test]
        public void RecargarTarjeta_DescuentoAdecuado_SaldoAdeudado()
        {
            // Arrange
            var tiempo = new Tiempo(10, 0);
            var tarjeta = new Tarjeta(500); // Saldo positivo para permitir el pago
            var colectivo = new Colectivo();
            var saldoRecarga = 1000; // Monto de la recarga

            // Act
            colectivo.PagarCon(tarjeta, tiempo); // Simulamos el pago que decrementa el saldo
            tarjeta.RecargarSaldo(saldoRecarga); // Recargamos la tarjeta

            // Assert
            Assert.That(tarjeta.ObtenerSaldo(), Is.EqualTo(500 - 940 + saldoRecarga), "El saldo no se actualizó correctamente después de la recarga y el descuento del saldo adeudado.");

            // Output
            Console.WriteLine("La recarga se realizó correctamente y el saldo se actualizó.");
        }
    }
}
