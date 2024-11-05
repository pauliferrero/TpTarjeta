using System;

namespace TpTarjeta
{
    public class Boleto
    {
        public DateTime Fecha { get; private set; }
        public string TipoTarjeta { get; private set; }
        public string LineaColectivo { get; private set; }
        public decimal TotalAbonado { get; private set; }
        public decimal SaldoRestante { get; private set; }
        public string IdTarjeta { get; private set; }
        public bool CancelacionSaldoNegativo { get; private set; }

        public Boleto(string tipoTarjeta, string lineaColectivo, decimal totalAbonado, decimal saldoRestante, string idTarjeta, bool cancelacionSaldoNegativo)
        {
            Fecha = DateTime.Now;
            TipoTarjeta = tipoTarjeta;
            LineaColectivo = lineaColectivo;
            TotalAbonado = totalAbonado;
            SaldoRestante = saldoRestante;
            IdTarjeta = idTarjeta;
            CancelacionSaldoNegativo = cancelacionSaldoNegativo;
        }

        public void MostrarBoleto()
        {
            Console.WriteLine("----- BOLETO -----");
            Console.WriteLine($"Fecha: {Fecha}");
            Console.WriteLine($"Tipo de Tarjeta: {TipoTarjeta}");
            Console.WriteLine($"Línea de Colectivo: {LineaColectivo}");
            Console.WriteLine($"Total Abonado: {TotalAbonado}");
            Console.WriteLine($"Saldo Restante: {SaldoRestante}");
            Console.WriteLine($"ID de Tarjeta: {IdTarjeta}");
            if (CancelacionSaldoNegativo)
            {
                Console.WriteLine("Se canceló el saldo negativo con este pago.");
            }
            Console.WriteLine("------------------");
        }
    }
}