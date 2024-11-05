using System;

namespace TpTarjeta
{
    public class Program
    {
        public static void Main()
        {
            // Crea instancias de las tarjetas, colectivo y tiempo
            Tiempo tiempo = new Tiempo(10, 0); // Inicializa el tiempo a las 10:00
            FranquiciaCompleta tarjetaFC = new FranquiciaCompleta(0, tiempo); // Saldo inicial 0
            MedioBoleto tarjetaMB = new MedioBoleto(2000, tiempo); // Saldo inicial 2000 y tiempo inicial
            Colectivo colectivo = new Colectivo();

            // Prueba con Franquicia Completa
            Console.WriteLine("\n--- Prueba de Franquicia Completa ---");
            for (int i = 1; i <= 3; i++)
            {
                try
                {
                    Boleto boletoFC = colectivo.PagarCon(tarjetaFC, tiempo); // Usamos PagarCon para generar el boleto
                    Console.WriteLine($"Viaje {i} realizado con Franquicia Completa.");
                    boletoFC.MostrarBoleto(); // Mostramos el boleto correspondiente al viaje
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error en el viaje {i} con Franquicia Completa: " + ex.Message);
                }
            }

            // Verifica el saldo después del pago
            Console.WriteLine($"Saldo Franquicia Completa: {tarjetaFC.ObtenerSaldo()}");

            // Prueba con Medio Boleto
            Console.WriteLine("\n--- Prueba de Medio Boleto ---");
            try
            {
                decimal saldoAntes = tarjetaMB.ObtenerSaldo();
                Boleto boletoMB = colectivo.PagarCon(tarjetaMB, tiempo); // Realizamos un pago y obtenemos el boleto
                decimal saldoDespues = tarjetaMB.ObtenerSaldo();
                Console.WriteLine($"Pago realizado con Medio Boleto. Saldo antes: {saldoAntes}, saldo después: {saldoDespues}");
                boletoMB.MostrarBoleto(); // Mostramos el boleto después de pagar
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al pagar con Medio Boleto: " + ex.Message);
            }

            // Intentar pagar nuevamente antes de 5 minutos (esto debería fallar)
            try
            {
                Console.WriteLine("Intentando realizar otro pago con Medio Boleto antes de 5 minutos...");
                colectivo.PagarCon(tarjetaMB, tiempo); // Intentamos otro pago antes de que pasen 5 minutos
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al intentar pagar nuevamente con Medio Boleto: " + ex.Message);
            }

            // Avanza 5 minutos en el tiempo
            Console.WriteLine("Avanzando 5 minutos en el tiempo...");
            tiempo.SumarMinutos(5); // Asegúrate de que este método sume 5 minutos correctamente

            // Intenta pagar nuevamente después de 5 minutos
            try
            {
                Console.WriteLine("Intentando realizar otro pago con Medio Boleto después de 5 minutos...");
                Boleto boletoMB = colectivo.PagarCon(tarjetaMB, tiempo); // Usamos el método para pagar
                Console.WriteLine("Pago realizado con éxito después de 5 minutos.");
                boletoMB.MostrarBoleto(); // Mostramos el boleto después de pagar
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al intentar pagar con Medio Boleto después de 5 minutos: " + ex.Message);
            }
        }
    }
}
