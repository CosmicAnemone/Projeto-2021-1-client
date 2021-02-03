// Para builds focando numa empresa específica,
// descomente a diretiva em questão e comente a diretiva de 'empresa indeterminada'.

// #define ACME_CO
// #define TIO_PATINHAS_BANK

#define EMPRESA_INDETERMINADA

using System;
using System.Collections.Generic;
using System.ComponentModel;
using data;
using network;
using SimpleJSON;
using ui;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static Assets;

/*
 Para um cliente mais simples, decidi criar a maioria da interface programaticamente.
 Cada elemento tem um asset básico que eu criei pelo Unity (ver Assets.cs)
 que é então instanciado, configurado e colocado num 'vertical layout'
 (que simplesmente os organiza verticalmente).
 */
public class CanvasManager : MonoBehaviour {
    public RectTransform verticalLayout;
    private readonly List<GameObject> generatedUI = new List<GameObject>();
    private readonly Choices choices = new Choices();

    private void Start() {
        escolherPorta();
    }

    private void nextAction() {
        choices.action = Choices.Action.none;
        choices.beneficios.Clear();
        escolherAcao();
    }

    private void escolherPorta() {
        (TextLabel textLabel, InputField inputField) = createUI(basic_label, basic_input_field);
        textLabel.setText("Porta a ser usada na conexão com o servidor");
        inputField.contentType = InputField.ContentType.IntegerNumber;
        inputField.characterLimit = 5;

        createUI(basic_button)
            .setText("Confirmar")
            .setOnClick(() => {
                clearUI();
                string input = inputField.text;
                if (!int.TryParse(input, out Defs.PORT)) {
                    mensagem("A porta deve ser um número.", escolherPorta);
                } else {
                    escolherEmpresa();
                }
            });
    }

    private void escolherEmpresa() {
#if ACME_CO
        NetworkingManager.loadEmpresa(choices,
                                      PlaceholderData.acmeCo,
                                      s => mensagem(s, escolherEmpresa));
        escolherCPF();
        return;
#elif TIO_PATINHAS_BANK
        NetworkingManager.loadEmpresa(choices,
                                      PlaceholderData.tioPatinhasBank,
                                      s => mensagem(s, escolherEmpresa));
        escolherCPF();
        return;
#else
        foreach (string nomeEmpresa in PlaceholderData.nomeEmpresas) {
            createUI(basic_button)
                .setText(nomeEmpresa)
                .setOnClick(() => {
                    clearUI();
                    loading("Carregando informações da empresa...");
                    if (NetworkingManager.loadEmpresa(
                        choices, nomeEmpresa, s => mensagem(s, escolherEmpresa, doClearUI: true))) {
                        clearUI();
                        escolherCPF();
                    }
                });
        }
#endif
    }

    private void escolherCPF() {
        (TextLabel textLabel, InputField inputField) = createUI(basic_label, basic_input_field);
        textLabel.setText("CPF do funcionário");
        inputField.contentType = InputField.ContentType.IntegerNumber;
        inputField.characterLimit = 11;

        createUI(basic_button)
            .setText("Confirmar")
            .setOnClick(() => {
                clearUI();
                string input = inputField.text;
                if (input.Length != 11) {
                    mensagem("O CPF deve conter 11 caracteres.", escolherCPF);
                } else if (!long.TryParse(input, out long CPF)) {
                    mensagem("CPF deve ser um número.", escolherCPF);
                } else {
                    loading("Carregando informações do funcionário...");
                    if (NetworkingManager.loadFuncionario(
                        choices, CPF, s => mensagem(s, escolherCPF, doClearUI: true))) {
                        clearUI();
                        escolherAcao();
                    }
                }
            });

#if EMPRESA_INDETERMINADA
        createUI(basic_button)
            .setText("Voltar")
            .setOnClick(() => {
                clearUI();
                choices.empresa = null;
                escolherEmpresa();
            });
#endif
    }

    private void escolherAcao() {
        createUI(basic_label).setText("O que deseja fazer?");
        createUI(basic_button)
            .setText("Adicionar benefício(s)")
            .setOnClick(() => {
                clearUI();
                choices.action = Choices.Action.add;
                escolherBeneficios();
            });
        createUI(basic_button)
            .setText("Modificar benefício(s)")
            .setOnClick(() => {
                clearUI();
                choices.action = Choices.Action.edit;
                escolherBeneficios();
            });
        createUI(basic_button)
            .setText("Remover benefício(s)")
            .setOnClick(() => {
                clearUI();
                choices.action = Choices.Action.remove;
                escolherBeneficios();
            });
        createUI(basic_button)
            .setText("Selecionar outro funcionário")
            .setOnClick(() => {
                clearUI();
                choices.funcionario = null;
                escolherCPF();
            });
    }

    private void escolherBeneficios() {
        choices.beneficios.Clear();
        createUI(basic_label).setText($"Qual benefício deseja {choices.acaoInfinitivo}?");
        List<Beneficio> beneficios = new List<Beneficio>();
        foreach (Beneficio beneficio in choices.empresa.beneficios) {
            switch (choices.action) {
                case Choices.Action.add:
                    if (!choices.funcionario.beneficios.Contains(beneficio.nome)) {
                        beneficios.Add(beneficio);
                    }
                    break;
                case Choices.Action.edit:
                case Choices.Action.remove:
                    if (choices.funcionario.beneficios.Contains(beneficio.nome)) {
                        beneficios.Add(beneficio);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        foreach (Beneficio beneficio in beneficios) {
            createUI(basic_toggle)
                .setText(beneficio.nome)
                .setOnToggle(isOn => {
                    if (isOn) {
                        choices.beneficios.Add(beneficio);
                    } else {
                        choices.beneficios.Remove(beneficio);
                    }
                });
        }
        createUI(basic_button)
            .setText("Confirmar")
            .setOnClick(() => {
                clearUI();
                switch (choices.action) {
                    case Choices.Action.add:
                    case Choices.Action.edit:
                        preencherCampos();
                        break;
                    case Choices.Action.remove:
                        loading("Solicitando descadastro de benefícios...");
                        if (NetworkingManager.modificarCadastros(
                            choices, s => mensagem(s, escolherBeneficios, doClearUI: true))) {
                            clearUI();
                            mensagem("Beneficios descadastrados com sucesso!", nextAction);
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            });
        createUI(basic_button)
            .setText("Voltar")
            .setOnClick(() => {
                clearUI();
                choices.action = Choices.Action.none;
                escolherAcao();
            });
    }

    private void preencherCampos() {
        choices.funcionario.novosCampos = new JSONObject();
        Dictionary<string, InputField.ContentType> campos = new Dictionary<string, InputField.ContentType>();
        foreach (Beneficio beneficio in choices.beneficios) {
            foreach (KeyValuePair<string, InputField.ContentType> pair in beneficio.campos) {
                campos[pair.Key] = pair.Value;
            }
        }
        foreach (KeyValuePair<string, InputField.ContentType> pair in campos) {
            (TextLabel textLabel, InputField inputField) = createUI(basic_label, basic_input_field);
            textLabel.setText(pair.Key);
            inputField.contentType = pair.Value;
            inputField.onEndEdit.AddListener(input => {
                switch (pair.Value) {
                    case InputField.ContentType.Alphanumeric:
                        choices.funcionario.novosCampos[pair.Key] = input;
                        break;
                    case InputField.ContentType.IntegerNumber:
                        try {
                            choices.funcionario.novosCampos[pair.Key] = int.Parse(input);
                        } catch (ArithmeticException) {
                            inputField.text = "";
                        }
                        break;
                    case InputField.ContentType.DecimalNumber:
                        try {
                            choices.funcionario.novosCampos[pair.Key] = double.Parse(input);
                        } catch (ArithmeticException) {
                            inputField.text = "";
                        }
                        break;
                    default:
                        throw new InvalidEnumArgumentException();
                }
            });
        }
        createUI(basic_button)
            .setText("Confirmar")
            .setOnClick(() => {
                clearUI();
                loading($"Solicitando {choices.acao} de benefícios...");
                if (NetworkingManager.modificarCadastros(
                    choices, s => mensagem(s, preencherCampos, doClearUI: true))) {
                    clearUI();
                    mensagem($"Beneficios {choices.verboParcial}os com sucesso!", nextAction);
                }
            });
        createUI(basic_button)
            .setText("Voltar")
            .setOnClick(() => {
                clearUI();
                choices.beneficios.Clear();
                escolherBeneficios();
            });
    }

    private void loading(string mensagem = "Carregando...") {
        createUI(basic_label).flexible.setText(mensagem);
    }

    private void mensagem(string mensagem, UnityAction returnTo,
                          string voltar = "Voltar", bool doClearUI = false) {
        if (doClearUI) clearUI();
        createUI(basic_label).flexible.setText(mensagem);
        createUI(basic_button)
            .setText(voltar)
            .setOnClick(() => {
                clearUI();
                returnTo.Invoke();
            });
    }

    private T createUI<T>(MyAssetLoader<T> asset) where T : MonoBehaviour {
        return registerUI(Instantiate(asset.Asset, verticalLayout));
    }

    private (T1, T2) createUI<T1, T2>(MyAssetLoader<T1> left, MyAssetLoader<T2> right)
        where T1 : MonoBehaviour
        where T2 : MonoBehaviour {
        HorizontalLayoutGroup layout = createUI(horizontal_layout);
        return (Instantiate(left.Asset, layout.transform),
                Instantiate(right.Asset, layout.transform));
    }

    private T registerUI<T>(T targetUI) where T : MonoBehaviour {
        generatedUI.Add(targetUI.gameObject);
        return targetUI;
    }

    private void clearUI() {
        foreach (GameObject b in generatedUI) {
            Destroy(b);
        }
        generatedUI.Clear();
    }
}