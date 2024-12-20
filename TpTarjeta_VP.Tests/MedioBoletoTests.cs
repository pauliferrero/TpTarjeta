﻿using NUnit.Framework;
using TpTarjeta;

namespace TpTarjeta.Tests
{
    public class MedioBoletoTests
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
        public void MontoEsMitadDelNormal()
        {
            var tarjeta = new MedioBoleto(1200, tiempo); 

            tarjeta.DebitarSaldo(tiempo, fecha); 
            var ultimoPagoMedioBoleto = tarjeta.ObtenerUltimoPago();

            Assert.That(ultimoPagoMedioBoleto, Is.EqualTo(600m), "El monto del medio boleto debería ser 600.");
            Console.WriteLine("El monto del medio boleto es correctamente 600.");
        }

        // Iteración 3
        [Test]
        public void ViajeAntesDe5Minutos_LanzaExcepcion()
        {
            var tarjeta = new MedioBoleto(1000, tiempo); 
            tarjeta.DebitarSaldo(tiempo, fecha); 

            var tiempoAntesDe5Min = new Tiempo(10, 3); 

            Assert.That(() => tarjeta.DebitarSaldo(tiempoAntesDe5Min, fecha), Throws.InvalidOperationException, "Se esperaba una excepción al intentar realizar otro viaje antes de 5 minutos.");
            Console.WriteLine("No se puede realizar otro viaje en menos de 5 minutos con la misma tarjeta medio boleto.");
        }

        [Test]
        public void MasDeCuatroViajes_LanzaExcepcion()
        {
            var tarjeta = new MedioBoleto(3000, tiempo); 

            tarjeta.DebitarSaldo(new Tiempo(tiempo.ObtenerHoras(), tiempo.ObtenerMinutos()), fecha); // 1er viaje
            tiempo.SumarMinutos(5); 

            tarjeta.DebitarSaldo(new Tiempo(tiempo.ObtenerHoras(), tiempo.ObtenerMinutos()), fecha); // 2do viaje
            tiempo.SumarMinutos(5); 

            tarjeta.DebitarSaldo(new Tiempo(tiempo.ObtenerHoras(), tiempo.ObtenerMinutos()), fecha); // 3er viaje
            tiempo.SumarMinutos(5); 

            tarjeta.DebitarSaldo(new Tiempo(tiempo.ObtenerHoras(), tiempo.ObtenerMinutos()), fecha); // 4to viaje

            tiempo.SumarMinutos(5); 
            Assert.That(() => tarjeta.DebitarSaldo(new Tiempo(tiempo.ObtenerHoras(), tiempo.ObtenerMinutos()), fecha), Throws.InvalidOperationException, "Se esperaba una excepción al intentar realizar un quinto viaje con medio boleto.");
            Console.WriteLine("No se puede realizar más de cuatro viajes en un día con la tarjeta medio boleto.");
        }

        // Iteración 4
        [Test]
        public void ViajeFueraDeHorario()
        {
            var tarjeta = new MedioBoleto(1000m, tiempo);

            Assert.Throws<InvalidOperationException>(() => tarjeta.DebitarSaldo(new Tiempo(5, 59), fecha)); // 23:00
            Assert.Throws<InvalidOperationException>(() => tarjeta.DebitarSaldo(new Tiempo(22, 30), fecha)); // 22:30
        }

        [Test]
        public void ViajeFueraDeDiaValido()
        {
            var fechaSabado = new Fecha(DiaDeLaSemana.Sabado, 11, 2024 ); 
            var tarjeta = new MedioBoleto(1000m, tiempo);

            Assert.Throws<InvalidOperationException>(() => tarjeta.DebitarSaldo(new Tiempo(10, 0), fechaSabado)); 
        }
    }
}
