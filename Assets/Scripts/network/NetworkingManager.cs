using System;
using System.Collections.Generic;
using data;
using SimpleJSON;

namespace network {
    public static class NetworkingManager {
        public static bool loadEmpresa(Choices choices, string nomeEmpresa, Action<string> onError) {
            JSONObject json = NetworkingClient.tryGet(
                Defs.base_url + "?empresa=" + Uri.EscapeDataString(nomeEmpresa))?.AsObject;
            if (json == null) {
                onError("Erro ao carregar dados");
                return false;
            } else if (isError(json, onError)) {
                return false;
            } else {
                List<Beneficio> beneficios = new List<Beneficio>();
                foreach (KeyValuePair<string, JSONNode> pair in json) {
                    Beneficio beneficio = new Beneficio(pair.Key);
                    if (pair.Value is JSONObject camposJson) {
                        foreach (KeyValuePair<string, JSONNode> campo in camposJson) {
                            if (!pair.Value.IsString ||
                                beneficio.setCampo(campo.Key, campo.Value.Value) == null) {
                                onError("Json inválido");
                                return false;
                            }
                        }
                    } else {
                        onError("Json inválido");
                        return false;
                    }
                    beneficios.Add(beneficio);
                }
                choices.empresa = new Empresa(nomeEmpresa, beneficios.ToArray());
                return true;
            }
        }

        public static bool loadFuncionario(Choices choices, long CPF, Action<string> onError) {
            JSONObject json = NetworkingClient.tryPost(
                Defs.base_url,
                new JSONObject {
                    [Field.type] = "info_funcionario",
                    [Field.empresa] = choices.empresa.nome,
                    [Field.funcionario] = CPF
                }
            )?.AsObject;
            if (json == null) {
                onError("Erro ao carregar dados");
                return false;
            } else if (isError(json, onError)) {
                return false;
            } else {
                //TODO validar beneficios e campos
                if (json.tryFetch(Field.beneficios, out JSONArray beneficios) &&
                    json.tryFetch(Field.campos, out JSONObject campos)) {
                    HashSet<string> beneficiosSet = new HashSet<string>();
                    foreach (JSONNode beneficio in beneficios) {
                        if (beneficio.IsString) {
                            beneficiosSet.Add(beneficio.Value);
                        } else {
                            onError("Json inválido");
                            return false;
                        }
                    }
                    choices.funcionario = new Funcionario(CPF, beneficiosSet, campos);
                    return true;
                } else {
                    onError("Json inválido");
                    return false;
                }
            }
        }

        public static bool modificarCadastros(Choices choices, Action<string> onError) {
            foreach (Beneficio beneficio in choices.beneficios) {
                if (!modificarCadastro(choices, beneficio)) {
                    onError("Erro ao cadastrar o beneficio " + beneficio.nome);
                    return false;
                }
            }
            return true;
        }

        private static bool modificarCadastro(Choices choices, Beneficio beneficio) {
            JSONObject requestJson = new JSONObject {
                [Field.type] = choices.actionType,
                [Field.empresa] = choices.empresa.nome,
                [Field.funcionario] = choices.funcionario.CPF,
                [Field.beneficio] = beneficio.nome
            };
            //TODO validar campos
            if (choices.action != Choices.Action.remove) {
                JSONObject campos = new JSONObject();
                foreach (string nome in beneficio.campos.Keys) {
                    if (campos[nome] == null)
                        campos[nome] = choices.funcionario.novosCampos[nome];
                }
                requestJson[Field.campos] = campos;
            }
            JSONObject json = NetworkingClient.tryPost(Defs.base_url, requestJson)?.AsObject;
            return json != null && !isError(json);
        }

        private static bool isError(JSONNode node, Action<string> onError = null) {
            string type = null;
            if (!node.tryFetch(Field.type, ref type) || type != "error")
                return false;

            string errorMessage = null;
            onError?.Invoke(node.tryFetch(Field.data, ref errorMessage)
                        ? $"Erro:\n{errorMessage}"
                        : "Erro desconhecido");

            return true;
        }
    }
}