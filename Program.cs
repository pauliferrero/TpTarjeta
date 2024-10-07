using System;

namespace Tp2AAT
{
    public class Program
    {
        public static void Main()
        {
            // Crear instancias de las tarjetas y el colectivo
            FranquiciaCompleta tarjetaFC = new FranquiciaCompleta(0); // Saldo inicial 0
            MedioBoleto tarjetaMB = new MedioBoleto(2000); // Saldo inicial 2000
            Colectivo colectivo = new Colectivo();

            // Prueba con Franquicia Completa
            Console.WriteLine("\n--- Prueba de Franquicia Completa ---");
            try
            {
                // Realizar primer viaje gratuito
                tarjetaFC.RealizarViaje();
                Console.WriteLine("Primer viaje gratuito realizado con Franquicia Completa.");

                // Realizar segundo viaje gratuito
                tarjetaFC.RealizarViaje();
                Console.WriteLine("Segundo viaje gratuito realizado con Franquicia Completa.");

                // Intentar tercer viaje (debe fallar)
                tarjetaFC.RealizarViaje();
                Console.WriteLine("Tercer viaje realizado (no debería ser gratuito).");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error en la prueba de Franquicia Completa: " + ex.Message);
            }

            // Verificar saldo después del pago
            Console.WriteLine($"Saldo Franquicia Completa: {tarjetaFC.ObtenerSaldo()}");

            // Intentar pagar con Medio Boleto
            Console.WriteLine("\n--- Prueba de Medio Boleto ---");
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

            // Intentar pagar nuevamente antes de 5 minutos (esto debería fallar)
            try
            {
                Console.WriteLine("Intentando realizar otro pago con Medio Boleto antes de 5 minutos...");
                colectivo.PagarCon(tarjetaMB); // Intentamos otro pago antes de que pasen 5 minutos
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al intentar pagar nuevamente con Medio Boleto: " + ex.Message);
            }

            // Esperar 5 segundos (en lugar de 5 minutos)
            Console.WriteLine("Esperando 5 segundos para probar otro pago...");
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(5));

            // Intentar pagar nuevamente después de 5 segundos
            try
            {
                Console.WriteLine("Intentando realizar otro pago con Medio Boleto después de 5 segundos...");
                colectivo.PagarCon(tarjetaMB); // Intentamos otro pago después de esperar
                Console.WriteLine("Pago realizado con éxito después de 5 segundos.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al intentar pagar con Medio Boleto después de 5 segundos: " + ex.Message);
            }

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
