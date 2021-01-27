using System;
using System.Collections.Generic;

namespace data {
    public class Choices {
        public Empresa empresa;
        public Funcionario funcionario;
        public Action action = Action.none;
        public readonly HashSet<Beneficio> beneficios = new HashSet<Beneficio>();

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

        public string actionType {
            get {
                switch (action) {
                    case Action.add:
                        return "add";
                    case Action.edit:
                        return "edit";
                    case Action.remove:
                        return "remove";
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