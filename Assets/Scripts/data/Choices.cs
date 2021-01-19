using System;
using System.Collections.Generic;
using static UnityEngine.UI.InputField;

namespace data {
    public class Choices {
        public Empresa empresa;
        public long CPF;
        public Action action;
        public readonly HashSet<Beneficio> beneficios = new HashSet<Beneficio>();
        public readonly Dictionary<string, ContentType> campos = new Dictionary<string, ContentType>();

        public string actionVerb {
            get {
                switch (action) {
                    case Action.add:
                        return "cadastrar";
                    case Action.edit:
                        return "modificar";
                    case Action.remove:
                        return "descadastrar";
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
        
        public string resultVerb {
            get {
                switch (action) {
                    case Action.add:
                        return "cadastrad";
                    case Action.edit:
                        return "modificad";
                    case Action.remove:
                        return "descadastrad";
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public enum Action {
            none, add, edit, remove
        }
    }
}