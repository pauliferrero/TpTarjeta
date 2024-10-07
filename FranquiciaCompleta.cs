namespace Tp2AAT
{
    public class FranquiciaCompleta : Tarjeta
    {
        public FranquiciaCompleta(decimal saldoInicial) : base(saldoInicial) { }

        public override bool TieneSaldoSuficiente() => true; // Siempre puede pagar

        public override void DebitarSaldo() { /* No se debita nada */ }
    }
}
