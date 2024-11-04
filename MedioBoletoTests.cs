using NUnit.Framework;
using TpTarjeta;

namespace TpTarjeta.Tests
{
    public class MedioBoletoTests
    {
        [SetUp]
        public void Setup()
        {
            // Inicializa objetos comunes para todas las pruebas aquí, si lo necesitas.
        }

        [Test]
        public void PagoConMedioBoleto_MontoEsMitadDelNormal()
        {
            // Arrange
            var tiempoInicial = new Tiempo(10, 0); // Tiempo inicial para el viaje
            var tarjeta = new MedioBoleto(1200, tiempoInicial); // Tarjeta medio boleto con saldo suficiente

            // Act
            tarjeta.DebitarSaldo(tiempoInicial); // Debitamos el monto de medio boleto
            var ultimoPagoMedioBoleto = tarjeta.ObtenerUltimoPago(); // Obtenemos el último pago

            // Assert
            Assert.That(ultimoPagoMedioBoleto, Is.EqualTo(600m), "El monto del medio boleto debería ser 470.");
            Console.WriteLine("El monto del medio boleto es correctamente 470.");
        }

        [Test]
        public void PagoConMedioBoleto_SiRealizaViajeAntesDe5Minutos_LanzaExcepcion()
        {
            var tiempoInicial = new Tiempo(10, 0); // Tiempo inicial para el viaje
            var tarjeta = new MedioBoleto(1000, tiempoInicial); // Tarjeta medio boleto
            tarjeta.DebitarSaldo(tiempoInicial); // Realiza el primer viaje

            var tiempoAntesDe5Min = new Tiempo(10, 3); // Tiempo 3 minutos después

            Assert.That(() => tarjeta.DebitarSaldo(tiempoAntesDe5Min), Throws.InvalidOperationException, "Se esperaba una excepción al intentar realizar otro viaje antes de 5 minutos.");
            Console.WriteLine("No se puede realizar otro viaje en menos de 5 minutos con la misma tarjeta medio boleto.");
        }


        [Test]
        public void PagoConMedioBoleto_MasDeCuatroViajes_LanzaExcepcion()
        {
            var tiempo = new Tiempo(10, 0); // Tiempo inicial para el primer viaje
            var tarjeta = new MedioBoleto(3000, tiempo); // Tarjeta medio boleto

            tarjeta.DebitarSaldo(new Tiempo(tiempo.ObtenerHoras(), tiempo.ObtenerMinutos())); // 1er viaje
            tiempo.SumarMinutos(5); // 5 minutos después

            tarjeta.DebitarSaldo(new Tiempo(tiempo.ObtenerHoras(), tiempo.ObtenerMinutos())); // 2do viaje
            tiempo.SumarMinutos(5); // 5 minutos después

            tarjeta.DebitarSaldo(new Tiempo(tiempo.ObtenerHoras(), tiempo.ObtenerMinutos())); // 3er viaje
            tiempo.SumarMinutos(5); // 5 minutos después

            tarjeta.DebitarSaldo(new Tiempo(tiempo.ObtenerHoras(), tiempo.ObtenerMinutos())); // 4to viaje

            tiempo.SumarMinutos(5); // 5 minutos después para intentar un quinto viaje
            Assert.That(() => tarjeta.DebitarSaldo(new Tiempo(tiempo.ObtenerHoras(), tiempo.ObtenerMinutos())), Throws.InvalidOperationException, "Se esperaba una excepción al intentar realizar un quinto viaje con medio boleto.");
            Console.WriteLine("No se puede realizar más de cuatro viajes en un día con la tarjeta medio boleto.");
        }




    }
}
