using System;
using System.Collections.Generic;

namespace data {
    public class Choices {
        public Empresa empresa;
        public Funcionario funcionario;
        public Action action = Action.none;
        public readonly HashSet<Beneficio> beneficios = new HashSet<Beneficio>();

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

        public string acaoInfinitivo => $"{verboParcial}r";
        public string acao {
            get {
                switch (action) {
                    case Action.add:
                        return "cadastro";
                    case Action.edit:
                        return "modificação";
                    case Action.remove:
                        return "descadastro";
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
        public string verboParcial {
            get {
                switch (action) {
                    case Action.add:
                        return "cadastra";
                    case Action.edit:
                        return "modifica";
                    case Action.remove:
                        return "descadastra";
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