using System.Collections.Generic;
using SimpleJSON;

namespace data {
    public class Funcionario {
        public readonly long CPF;
        public readonly HashSet<string> beneficios;
        public readonly JSONObject campos;
        public JSONObject novosCampos;
        
        public Funcionario(long CPF, HashSet<string> beneficios, JSONObject campos) {
            this.CPF = CPF;
            this.beneficios = beneficios;
            this.campos = campos;
        }
    }
}