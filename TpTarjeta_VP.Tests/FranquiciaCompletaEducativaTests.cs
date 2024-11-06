using System;
using NUnit.Framework;

namespace TpTarjeta.Tests
{
    [TestFixture]
    public class FranquiciaCompletaEducativaTests
    {
        Colectivo colectivo;
        Tiempo tiempo;
        Fecha fecha;
        Tarjeta tarjetaEducativa;

        [SetUp]
        public void Setup()
        {
            tiempo = new Tiempo(10, 0); 
            
            colectivo = new Colectivo();
            fecha = new Fecha(DiaDeLaSemana.Lunes, 11, 2024);
        }

        // Iteración 3
        [Test]
        public void CobraViajeCuandoSeSuperaLimiteGratuito()
        {
            tarjetaEducativa = new Educativo(1200, tiempo);

            colectivo.PagarCon(tarjetaEducativa, tiempo, fecha); // 1er viaje gratuito
            tiempo.SumarMinutos(5);
            colectivo.PagarCon(tarjetaEducativa, tiempo, fecha); // 2do viaje gratuito
            tiempo.SumarMinutos(5);

            Assert.DoesNotThrow(() => colectivo.PagarCon(tarjetaEducativa, tiempo, fecha), "Se esperaba que no se lanzara una excepción al intentar realizar un tercer viaje.");
            Assert.That(tarjetaEducativa.ObtenerSaldo(), Is.EqualTo(1200 - 1200), "El saldo no se debió debitar correctamente después del tercer viaje.");
        }

        [Test]
        public void NoMasDeDosViajesGratuitosPorDia()
        {
            tarjetaEducativa = new Educativo(10, tiempo);
            tarjetaEducativa.DebitarSaldo(tiempo, fecha);
            tiempo.SumarMinutos(5);
            tarjetaEducativa.DebitarSaldo(tiempo, fecha);
            tiempo.SumarMinutos(5);

            Assert.Throws<InvalidOperationException>(() => tarjetaEducativa.DebitarSaldo(tiempo, fecha), "Se esperaba una excepción al intentar realizar un tercer viaje gratuito.");
        }

    }
}
