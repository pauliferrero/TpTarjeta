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

            // Esperar 5 minutos (en realidad espera 5 segundos para no alargar el tiempo de ejecución)
            Console.WriteLine("Esperando 5 minutos para probar otro pago...");
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(5));

            // Intentar pagar nuevamente después de 5 minutos
            try
            {
                Console.WriteLine("Intentando realizar otro pago con Medio Boleto después de 5 minutos...");
                colectivo.PagarCon(tarjetaMB); // Intentamos otro pago después de esperar 5 minutos
                Console.WriteLine("Pago realizado con éxito después de 5 minutos.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al intentar pagar con Medio Boleto después de 5 minutos: " + ex.Message);
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
