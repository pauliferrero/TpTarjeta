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
            var tarjeta = new Tarjeta(2000); // Tarjeta con saldo suficiente
            var colectivo = new Colectivo();

            // Act
            var boleto = colectivo.PagarCon(tarjeta, tiempo);

            // Assert
            Assert.That(boleto, Is.Not.Null, "El boleto debería haberse generado.");
            Assert.That(tarjeta.ObtenerSaldo(), Is.EqualTo(2000 - 1200), "El saldo no se descontó correctamente.");

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
        public void TestCalculoTarifaSinDescuento()
        {
            var tarjeta = new Tarjeta(5000m);
            tarjeta.DebitarSaldo(new Tiempo(10, 0)); // 1er viaje
            Assert.That(tarjeta.ObtenerSaldo(), Is.EqualTo(3800m)); // 5000 - 1200
        }

        [Test]
        public void Tarjeta_Frecuente_TarifaNormalHastaViaje29()
        {
            // Arrange
            var tarjeta = new Tarjeta(36000m);
            var tiempo = new Tiempo(10, 0);

            // Act - Realizar 29 viajes
            tarjeta.DebitarSaldoPorViajes(29, tiempo);

            // Assert - El saldo debe reducirse en tarifa normal (1200 * 29)
            Assert.That(tarjeta.ObtenerSaldo(), Is.EqualTo(36000m - (29 * 1200m)),
                "La tarifa normal debería aplicarse hasta el viaje 29.");
        }

        [Test]
        public void Tarjeta_Frecuente_AplicarDescuento20PorCientoViajes30_79()
        {
            // Arrange
            var tarjeta = new Tarjeta(36000m);
            var tiempo = new Tiempo(10, 0);

            // Act - Realizar 30 viajes (primer viaje con descuento del 20%)
            tarjeta.DebitarSaldoPorViajes(29, tiempo); // 29 viajes tarifa normal

            tarjeta.DebitarSaldo(tiempo); // 30º viaje con 20% descuento

            // Assert
            var saldoEsperado = 36000m - (29 * 1200m) - (1200m * 0.80m);
            Assert.That(tarjeta.ObtenerSaldo(), Is.EqualTo(saldoEsperado),
                "El 20% de descuento debería aplicarse a partir del viaje 30 y hasta el 79.");
        }

        [Test]
        public void TestCargaExcedeLimite()
        {
            var tarjeta = new Tarjeta(9000m);
            tarjeta.RecargarSaldo(50000m); // Intentar cargar 50,000

            Assert.That(tarjeta.ObtenerSaldo(), Is.EqualTo(36000m), "El saldo acreditado no llegó al límite máximo permitido.");
            Assert.That(tarjeta.ObtenerSaldoPendiente(), Is.EqualTo(50000m - (36000m - 9000m)), "El saldo pendiente no se calculó correctamente.");
        }

        [Test]
        public void TestAcreditarSaldoPendienteProgresivamente()
        {
            // Inicializar la tarjeta con el saldo máximo de 36000
            var tarjeta = new Tarjeta(36000m);

            // Recargar la tarjeta con un monto adicional de 6000 (deja 6000 como saldo pendiente)
            tarjeta.RecargarSaldo(6000m);

            // Verificar que el saldo sigue en 36000 y el saldo pendiente es 6000
            Assert.That(tarjeta.ObtenerSaldo(), Is.EqualTo(36000m), "El saldo inicial no es correcto.");
            Assert.That(tarjeta.ObtenerSaldoPendiente(), Is.EqualTo(6000m), "El saldo pendiente inicial no es correcto.");

            // Realizar un viaje que reduce el saldo a 34800
            tarjeta.DebitarSaldo(new Tiempo(10, 0));

            // Verificar que el saldo se actualizó a 36000 y que el saldo pendiente se redujo a 4800
            Assert.That(tarjeta.ObtenerSaldo(), Is.EqualTo(36000m), "El saldo no se acreditó correctamente tras el viaje.");
            Assert.That(tarjeta.ObtenerSaldoPendiente(), Is.EqualTo(4800m), "El saldo pendiente no se actualizó correctamente tras el viaje.");
        }


        [Test]
        public void Tarjeta_Frecuente_AplicarDescuento25PorCientoViaje80()
        {
            // Arrange
            var tarjeta = new Tarjeta(36000m);
            var tiempo = new Tiempo(10, 0);

            // Act - Realizar 30 viajes (tarifa normal)
            tarjeta.DebitarSaldoPorViajes(29, tiempo); // 29 viajes tarifa normal
            tarjeta.DebitarSaldo(tiempo); // 30º viaje, tarifa normal

            // Recargar la tarjeta después de 30 viajes
            tarjeta.RecargarSaldo(36000m);

            // Realizar 30 viajes más (viajes 31 a 60, con 20% de descuento)
            tarjeta.DebitarSaldoPorViajes(30, tiempo); // 30 viajes (del 31 al 60)

            // Recargar la tarjeta después de 60 viajes
            tarjeta.RecargarSaldo(36000m);

            // Realizar hasta el 80º viaje (viaje 61 a 80, con 25% de descuento en el 80º viaje)
            tarjeta.DebitarSaldoPorViajes(20, tiempo); // 20 viajes (del 61 al 80)
            tarjeta.DebitarSaldo(tiempo); // 80º viaje con 25% descuento

            // Assert
            var saldoEsperado = 108000m - (30 * 1200m) - (50 * (1200m * 0.80m)) - (1200m * 0.75m);
            Assert.That(tarjeta.ObtenerSaldo(), Is.EqualTo(saldoEsperado),
                "El 25% de descuento debería aplicarse en el viaje 80.");
        }

        [Test]
        public void Tarjeta_Frecuente_TarifaNormalViaje81EnAdelante()
        {
            // Arrange
            var tarjeta = new Tarjeta(36000m);
            var tiempo = new Tiempo(10, 0);

            // Act - Realizar 30 viajes (tarifa normal)
            tarjeta.DebitarSaldoPorViajes(29, tiempo); // 29 viajes tarifa normal
            tarjeta.DebitarSaldo(tiempo); // 30º viaje

            // Recargar la tarjeta después de 30 viajes
            tarjeta.RecargarSaldo(36000m);

            // Realizar 30 viajes más (viajes 31 a 60, con 20% de descuento)
            tarjeta.DebitarSaldoPorViajes(30, tiempo); // 30 viajes (del 31 al 60)

            // Recargar la tarjeta después de 60 viajes
            tarjeta.RecargarSaldo(36000m);

            // Realizar hasta el 80º viaje (viaje 61 a 80, con 25% de descuento en el 80º viaje)
            tarjeta.DebitarSaldoPorViajes(20, tiempo); // 20 viajes (del 61 al 80)

            // Realizar el 81º viaje (tarifa normal)
            tarjeta.DebitarSaldo(tiempo); // 81º viaje sin descuento
            Console.WriteLine("Viaje 81, tarifa normal."); // Mensaje de depuración

            // Assert
            var saldoEsperado = 108000m - (29 * 1200m) - (50 * (1200m * 0.80m)) - (1200m * 0.75m) - 1200m;
            Assert.That(tarjeta.ObtenerSaldo(), Is.EqualTo(saldoEsperado),
                "La tarifa normal debería aplicarse a partir del viaje 81.");
        }


        [Test]
        public void TestSaldoSuficiente()
        {
            // Arrange
            var tarjeta = new Tarjeta(1200m); // Saldo suficiente para el primer viaje
            var tiempo = new Tiempo(10, 0);

            // Act - Realizar el primer viaje
            tarjeta.DebitarSaldo(tiempo); // 1er viaje

            // Assert - No debe lanzar excepción
            Assert.That(tarjeta.ObtenerSaldo(), Is.EqualTo(0)); // El saldo debería ser 0 después del viaje
        }


        [Test]
        public void TestRecargaSaldo()
        {
            var tarjeta = new Tarjeta(500m); // Saldo inicial
            tarjeta.RecargarSaldo(2000m); // Cargar saldo
            Assert.That(tarjeta.ObtenerSaldo(), Is.EqualTo(2500m)); // 500 + 2000
        }

        [Test]
        public void TestRecargaSaldoNegativoCancelado()
        {
            var tarjeta = new Tarjeta(-400m); // Saldo negativo inicial
            tarjeta.RecargarSaldo(480m); // Cargar saldo
            Assert.That(tarjeta.ObtenerSaldo(), Is.EqualTo(80m)); // Saldo debería ser positivo
        }
    }
}
