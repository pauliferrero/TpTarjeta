using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TpTarjeta
{
    public class Educativo : FranquiciaCompleta
    {    
        public Educativo(decimal saldoInicial, Tiempo tiempoInicial) : base(saldoInicial, tiempoInicial) { }
    }
}
