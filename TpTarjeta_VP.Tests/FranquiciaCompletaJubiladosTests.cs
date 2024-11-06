using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TpTarjeta.Tests
{
    public class FranquiciaCompletaJubiladosTests
    {
        Jubilado tarjetaJubilado;
        Tiempo tiempo;
        Fecha fecha;

        [SetUp]
        public void Setup()
        { 
            tiempo = new Tiempo(10, 30); 
            fecha = new Fecha(DiaDeLaSemana.Lunes, 11, 2024);
            tarjetaJubilado = new Jubilado(2000, tiempo);
        }

        [Test]
        public void TresViajesGratuitosEnUnDia()
        {
            tarjetaJubilado.DebitarSaldo(tiempo, fecha); 
            Assert.That(tarjetaJubilado.ObtenerSaldo(), Is.EqualTo(2000), "El saldo no debería haber cambiado después del primer viaje gratuito.");

            tiempo.SumarMinutos(5); 
            tarjetaJubilado.DebitarSaldo(tiempo, fecha); 
            Assert.That(tarjetaJubilado.ObtenerSaldo(), Is.EqualTo(2000), "El saldo no debería haber cambiado después del segundo viaje gratuito.");

            tiempo.SumarMinutos(5);
            tarjetaJubilado.DebitarSaldo(tiempo, fecha);
            Assert.That(tarjetaJubilado.ObtenerSaldo(), Is.EqualTo(2000), "El saldo no debería haber cambiado después del tercer viaje gratuito.");
        }

    }

}
