namespace data {
    public class Empresa {
        public readonly string nome;
        public readonly Beneficio[] beneficios;
        
        public Empresa(string nome, params Beneficio[] beneficios) {
            this.nome = nome;
            this.beneficios = beneficios;
        }
    }
}