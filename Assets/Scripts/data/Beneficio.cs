using System.Collections.Generic;
using static UnityEngine.UI.InputField;

namespace data {
    public class Beneficio {
        public readonly string nome;
        public readonly Dictionary<string, ContentType> campos;
        
        public Beneficio(string nome) {
            this.nome = nome;
            campos = new Dictionary<string, ContentType>();
        }

        public Beneficio setCampo(string campo, ContentType tipo) {
            campos[campo] = tipo;
            return this;
        }
    }
}