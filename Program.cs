using System;

namespace Tp2AAT
{
    public class Program
    {
        public static void Main()
        {
            // Crea instancias de las tarjetas y el colectivo
            FranquiciaCompleta tarjetaFC = new FranquiciaCompleta(0); // Saldo inicial 0
            MedioBoleto tarjetaMB = new MedioBoleto(2000); // Saldo inicial 2000
            Colectivo colectivo = new Colectivo();

            // Prueba con Franquicia Completa
            Console.WriteLine("\n--- Prueba de Franquicia Completa ---");
            try
            {
                // Realiza primer viaje gratuito
                tarjetaFC.RealizarViaje();
                Console.WriteLine("Primer viaje gratuito realizado con Franquicia Completa.");

                // Realiza segundo viaje gratuito
                tarjetaFC.RealizarViaje();
                Console.WriteLine("Segundo viaje gratuito realizado con Franquicia Completa.");

                // Intenta tercer viaje (debe fallar, si no, tiene saldo)
                tarjetaFC.RealizarViaje();
                Console.WriteLine("Tercer viaje realizado (no debería ser gratuito).");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error en la prueba de Franquicia Completa: " + ex.Message);
            }

            // Verifica el saldo después del pago
            Console.WriteLine($"Saldo Franquicia Completa: {tarjetaFC.ObtenerSaldo()}");

            // Crea boleto para Franquicia Completa
            Boleto boletoFC = new Boleto(tarjetaFC.GetType().Name, "102 144", tarjetaFC.ObtenerUltimoPago(), tarjetaFC.ObtenerSaldo(), tarjetaFC.ObtenerID(), false);
            boletoFC.MostrarBoleto();

            //Prueba el límite de la tarjeta
            try
            {
                tarjetaFC.RecargarSaldo(50000); // Intentamos recargar más del límite
                Console.WriteLine($"Recarga realizada. Nuevo saldo Medio Boleto: {tarjetaMB.ObtenerSaldo()}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al recargar saldo: " + ex.Message);
            }

            // Prueba con Medio Boleto
            Console.WriteLine("\n--- Prueba de Medio Boleto ---");
            try
            {
                decimal saldoAntes = tarjetaMB.ObtenerSaldo();
                Boleto boletoMB = colectivo.PagarCon(tarjetaMB); // Realizamos un pago y obtenemos el boleto
                decimal saldoDespues = tarjetaMB.ObtenerSaldo();
                Console.WriteLine($"Pago realizado con Medio Boleto. Saldo antes: {saldoAntes}, saldo después: {saldoDespues}");
                boletoMB.MostrarBoleto(); // Mostramos el boleto después de pagar
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al pagar con Medio Boleto: " + ex.Message);
            }

            // Prueba la recarga de saldo en Medio Boleto
            try
            {
                tarjetaMB.RecargarSaldo(3000); // Recargamos saldo
                Console.WriteLine($"Saldo actual: {tarjetaMB.ObtenerSaldo()}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al recargar saldo: " + ex.Message);
            }

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

            // Espera 5 minutos (en lugar de 5 minutos esperamos 5 segundos para que no sea tan largo)
            Console.WriteLine("Esperando 5 segundos para probar otro pago...");
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(5));

            // Intenta pagar nuevamente después de 5 minutos
            try
            {
                Console.WriteLine("Intentando realizar otro pago con Medio Boleto después de 5 segundos...");
                Boleto boletoMB = colectivo.PagarCon(tarjetaMB); // Usamos el método para pagar
                Console.WriteLine("Pago realizado con éxito después de 5 segundos.");
                boletoMB.MostrarBoleto(); // Mostramos el boleto después de pagar
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al intentar pagar con Medio Boleto después de 5 segundos: " + ex.Message);
            }

            
        }
    }
}
