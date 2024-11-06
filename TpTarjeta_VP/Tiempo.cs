using System;

namespace TpTarjeta
{
    public class Tiempo
    {
        private int horas;
        private int minutos;

        public Tiempo(int horasIniciales, int minutosIniciales)
        {
            horas = horasIniciales % 24; // Asegura que las horas estén entre 0 y 23
            minutos = minutosIniciales % 60; // Asegura que los minutos estén entre 0 y 59
        }

        public int ObtenerHoras()
        {
            return horas;
        }

        public int ObtenerMinutos()
        {
            return minutos;
        }

        public bool HaPasadoSuficienteTiempo(Tiempo otroTiempo, int minutosRequeridos)
        {
            // Convierte ambos tiempos a minutos totales
            int totalMinutosActual = this.horas * 60 + this.minutos;
            int totalMinutosOtro = otroTiempo.horas * 60 + otroTiempo.minutos;

            // Devuelve true si han pasado suficientes minutos
            return Math.Abs(totalMinutosActual - totalMinutosOtro) >= minutosRequeridos;
        }

        public string ObtenerTiempoActual()
        {
            return $"{horas:D2}:{minutos:D2}";
        }

        public void SumarMinutos(int cantidadMinutos)
        {
            minutos += cantidadMinutos;
            while (minutos >= 60)
            {
                minutos -= 60;
                horas++;
            }
            horas %= 24; 
        }
        public void SumarHoras(int cantidadHoras)
        {
            horas = (horas + cantidadHoras) % 24;
        }

        public void Sumar(Tiempo otroTiempo)
        {
            this.horas += otroTiempo.horas;
            this.minutos += otroTiempo.minutos;

            // Manejo del desbordamiento de minutos
            if (this.minutos >= 60)
            {
                this.horas += this.minutos / 60;
                this.minutos %= 60;
            }
        }

        public void SumarCincoMinutos()
        {
            SumarMinutos(5);
        }
    }
}
