using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TpTarjeta
{
    public enum DiaDeLaSemana
    {
        Domingo,
        Lunes,
        Martes,
        Miercoles,
        Jueves,
        Viernes,
        Sabado
    }

    public class Fecha
    {
        public DiaDeLaSemana Dia { get; set; }
        public int Mes { get; set; }
        public int Año { get; set; }

        public Fecha(DiaDeLaSemana dia, int mes, int año)
        {
            Dia = dia;
            Mes = mes;
            Año = año;

            if (mes < 1 || mes > 12)
            {
                throw new ArgumentException("El mes debe estar entre 1 y 12.");
            }

            if (dia < DiaDeLaSemana.Domingo || dia > DiaDeLaSemana.Sabado)
            {
                throw new ArgumentException("El día de la semana no es válido.");
            }

            if (año < 1)
            {
                throw new ArgumentException("El año debe ser mayor que 0.");
            }
        }

        public override string ToString()
        {
            return $"{Dia} {Mes}/{Año}";
        }
    }
}
