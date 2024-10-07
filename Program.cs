using System;

namespace Tp2AAT
{
    public class Program
    {
        public static void Main()
        {
            // Crear instancias de las tarjetas y el colectivo
            Tarjeta tarjetaFC = new FranquiciaCompleta(0); // Saldo inicial 0
            Tarjeta tarjetaMB = new MedioBoleto(2000); // Saldo inicial 2000
            Colectivo colectivo = new Colectivo();

            // Intentar pagar con Franquicia Completa
            try
            {
                colectivo.PagarCon(tarjetaFC);
                Console.WriteLine("Pago realizado con Franquicia Completa.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al pagar con Franquicia Completa: " + ex.Message);
            }

            // Verificar saldo después del pago
            Console.WriteLine($"Saldo Franquicia Completa: {tarjetaFC.ObtenerSaldo()}");

            // Intentar pagar con Medio Boleto
            try
            {
                decimal saldoAntes = tarjetaMB.ObtenerSaldo();
                colectivo.PagarCon(tarjetaMB); // Realizamos un pago
                decimal saldoDespues = tarjetaMB.ObtenerSaldo();
                Console.WriteLine($"Pago realizado con Medio Boleto. Saldo antes: {saldoAntes}, saldo después: {saldoDespues}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al pagar con Medio Boleto: " + ex.Message);
            }

            // Verificar saldo después del pago
            Console.WriteLine($"Saldo Medio Boleto: {tarjetaMB.ObtenerSaldo()}");

            // Probar la recarga de saldo en Medio Boleto
            try
            {
                tarjetaMB.RecargarSaldo(3000); // Recargamos saldo
                Console.WriteLine($"Recarga realizada. Nuevo saldo Medio Boleto: {tarjetaMB.ObtenerSaldo()}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al recargar saldo: " + ex.Message);
            }
        }
    }
}