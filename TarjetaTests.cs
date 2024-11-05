using NUnit.Framework;
using TpTarjeta;

namespace TpTarjeta.Tests
{
    public class TarjetaTests
    {
        Colectivo colectivo;
        Tiempo tiempo;

        [SetUp]
        public void Setup()
        {
            colectivo = new Colectivo();
            tiempo = new Tiempo(10, 0);
        }

        [Test]
        public void SaldoSuficiente()
        {
            var tarjeta = new Tarjeta(2000); // Tarjeta con saldo suficiente

            var boleto = colectivo.PagarCon(tarjeta, tiempo);

            Assert.That(boleto, Is.Not.Null, "El boleto debería haberse generado.");
            Assert.That(tarjeta.ObtenerSaldo(), Is.EqualTo(2000 - 1200), "El saldo no se descontó correctamente.");

            Console.WriteLine("El boleto fue generado correctamente y el saldo se actualizó.");
        }

        [Test]
        public void SaldoInsuficiente_LanzaExcepcion()
        {
            var tarjeta = new Tarjeta(50); // Tarjeta con saldo insuficiente

            // Act & Assert
            Assert.That(() => colectivo.PagarCon(tarjeta, tiempo), Throws.InvalidOperationException, "Se esperaba una excepción cuando el saldo es insuficiente.");

            // Output
            Console.WriteLine("Se lanzó una excepción como se esperaba debido al saldo insuficiente.");
        }

        [Test]
        public void NoPermiteSaldoMenorA480_LanzaExcepcion()
        {
            var tarjeta = new Tarjeta(-490);

            Assert.That(() => colectivo.PagarCon(tarjeta, tiempo), Throws.InvalidOperationException, "Se esperaba una excepción cuando el saldo es menor a -480.");

            Console.WriteLine("Se lanzó una excepción como se esperaba debido a un saldo menor a -480.");
        }

        
        [Test]
        public void CargaExcedeLimite()
        {
            var tarjeta = new Tarjeta(9000m);
            tarjeta.RecargarSaldo(50000m);

            Assert.That(tarjeta.ObtenerSaldo(), Is.EqualTo(36000m), "El saldo acreditado no llegó al límite máximo permitido.");
            Assert.That(tarjeta.ObtenerSaldoPendiente(), Is.EqualTo(50000m - (36000m - 9000m)), "El saldo pendiente no se calculó correctamente.");
        }

        [Test]
        public void AcreditarSaldoPendienteProgresivamente()
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
        public void RecargaSaldo()
        {
            var tarjeta = new Tarjeta(500m);
            tarjeta.RecargarSaldo(2000m); 
            Assert.That(tarjeta.ObtenerSaldo(), Is.EqualTo(2500m));
        }

        [Test]
        public void RecargaSaldoNegativoCancelado()
        {
            var tarjeta = new Tarjeta(-400m); 
            tarjeta.RecargarSaldo(480m); 
            Assert.That(tarjeta.ObtenerSaldo(), Is.EqualTo(80m)); 
        }

        [Test]
        public void UsoFrecuente_TarifaNormalHastaViaje29()
        {
            var tarjeta = new Tarjeta(36000m);

            tarjeta.DebitarSaldoPorViajes(29, tiempo); // Realizar 29 viajes

            Assert.That(tarjeta.ObtenerSaldo(), Is.EqualTo(36000m - (29 * 1200m)), "La tarifa normal debería aplicarse hasta el viaje 29.");
        }

        [Test]
        public void UsoFrecuente_AplicarDescuento20()
        {
            var tarjeta = new Tarjeta(36000m);

            tarjeta.DebitarSaldoPorViajes(29, tiempo); // 29 viajes tarifa normal

            tarjeta.DebitarSaldo(tiempo); // 30º viaje con 20% descuento

            Assert.That(tarjeta.ObtenerSaldo(), Is.EqualTo(36000m - (29 * 1200m) - (1200m * 0.80m)), "El 20% de descuento debería aplicarse a partir del viaje 30 y hasta el 79.");
        }

        [Test]
        public void UsoFrecuente_AplicarDescuento25()
        {
            var tarjeta = new Tarjeta(36000m);

            tarjeta.DebitarSaldoPorViajes(29, tiempo); // 29 viajes tarifa normal
            tarjeta.DebitarSaldo(tiempo); // 30º viaje, tarifa normal
            tarjeta.RecargarSaldo(36000m);
            tarjeta.DebitarSaldoPorViajes(30, tiempo); // 30 viajes (del 31 al 60)
            tarjeta.RecargarSaldo(36000m);
            tarjeta.DebitarSaldoPorViajes(20, tiempo); // 20 viajes (del 61 al 80)
            tarjeta.DebitarSaldo(tiempo); // 80º viaje con 25% descuento

            var saldoEsperado = 108000m - (30 * 1200m) - (50 * (1200m * 0.80m)) - (1200m * 0.75m);
            Assert.That(tarjeta.ObtenerSaldo(), Is.EqualTo(saldoEsperado), "El 25% de descuento debería aplicarse en el viaje 80.");
        }

        [Test]
        public void UsoFrecuente_TarifaNormalViaje81EnAdelante()
        {
            var tarjeta = new Tarjeta(36000m);

            tarjeta.DebitarSaldoPorViajes(29, tiempo); // 29 viajes tarifa normal
            tarjeta.DebitarSaldo(tiempo); // 30º viaje
            tarjeta.RecargarSaldo(36000m);
            tarjeta.DebitarSaldoPorViajes(30, tiempo); // 30 viajes (del 31 al 60)
            tarjeta.RecargarSaldo(36000m);
            tarjeta.DebitarSaldoPorViajes(20, tiempo); // 20 viajes (del 61 al 80)
            tarjeta.DebitarSaldo(tiempo); // 81º viaje sin descuento

            var saldoEsperado = 108000m - (29 * 1200m) - (50 * (1200m * 0.80m)) - (1200m * 0.75m) - 1200m;
            Assert.That(tarjeta.ObtenerSaldo(), Is.EqualTo(saldoEsperado), "La tarifa normal debería aplicarse a partir del viaje 81.");
        }
    }
}
