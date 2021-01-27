using System;
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

        public Beneficio setCampo(string campo, string tipo) {
            switch (tipo) {
                case Field.campo_string:
                    return setCampo(campo, ContentType.Alphanumeric);
                case Field.campo_inteiro:
                    return setCampo(campo, ContentType.IntegerNumber);
                case Field.campo_decimal:
                    return setCampo(campo, ContentType.DecimalNumber);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private Beneficio setCampo(string campo, ContentType tipo) {
            campos[campo] = tipo;
            return this;
        }
    }
}