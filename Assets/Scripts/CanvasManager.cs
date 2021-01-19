// Para builds focando numa empresa específica,
// descomente a diretiva em questão e comente a diretiva de 'empresa indeterminada'.
//
// #define ACME_CO
// #define TIO_PATINHAS_BANK

#define EMPRESA_INDETERMINADA

using System;
using System.Collections.Generic;
using System.Linq;
using data;
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
        Debug.Log(long.TryParse("12822305730", out choices.CPF));
#if ACME_CO
        choices.empresa = PlaceholderData.acmeCo;
        escolherCPF();
#elif TIO_PATINHAS_BANK
        choices.empresa = PlaceholderData.tioPatinhasBank;
        escolherCPF();
#else
        escolherEmpresa();
#endif
    }

#if EMPRESA_INDETERMINADA
    private void escolherEmpresa() {
        foreach (Empresa empresa in PlaceholderData.empresas) {
            createUI(basic_button)
                .setText(empresa.nome)
                .setOnClick(() => {
                    clearUI();
                    choices.empresa = empresa;
                    escolherCPF();
                });
        }
    }
#endif

    private void escolherCPF() {
        createUI(basic_label).setText("CPF do funcionário");

        InputField inputField = createUI(basic_input_field);
        inputField.contentType = InputField.ContentType.IntegerNumber;
        inputField.characterLimit = 11;

        createUI(basic_button)
            .setText("Confirmar")
            .setOnClick(() => {
                clearUI();
                string input = inputField.text;
                if (input.Length != 11) {
                    mensagem("O CPF deve conter 11 caracteres.", escolherCPF);
                } else if (!long.TryParse(input, out choices.CPF)) {
                    Debug.Log(input);
                    mensagem("CPF deve ser um número.", escolherCPF);
                } else {
                    escolherAcao();
                }
            });

        createUI(basic_button)
            .setText("Voltar")
            .setOnClick(() => {
                clearUI();
                choices.empresa = null;
                escolherEmpresa();
            });
    }

    private void escolherAcao() {
        createUI(basic_label).setText("O que eseja fazer?");
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
            .setText("Voltar")
            .setOnClick(() => {
                clearUI();
                choices.CPF = 0;
                escolherCPF();
            });
    }

    private void escolherBeneficios() {
        createUI(basic_label).setText($"Qual benefício deseja {choices.actionVerb}?");
        foreach (Beneficio beneficio in choices.empresa.beneficios) {
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
                        mensagem(choices.beneficios.Aggregate(
                                     "Beneficios descadastrados:\n",
                                     (current, beneficio) => current + $"\n\t- {beneficio.nome}"),
                                 escolherBeneficios);
                        choices.beneficios.Clear();
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
        foreach (Beneficio beneficio in choices.beneficios) {
            foreach (KeyValuePair<string, InputField.ContentType> pair in beneficio.campos) {
                if (!choices.campos.ContainsKey(pair.Key)) {
                    choices.campos.Add(pair.Key, pair.Value);
                }
            }
        }
        Dictionary<string, InputField> inputFields = new Dictionary<string, InputField>();
        foreach (KeyValuePair<string,InputField.ContentType> pair in choices.campos) {
            createUI(basic_label).setText(pair.Key);
            InputField inputField = createUI(basic_input_field);
            inputField.contentType = pair.Value;
            inputFields.Add(pair.Key, inputField);
        }
        createUI(basic_button)
            .setText("Confirmar")
            .setOnClick(() => {
                clearUI();
                mensagem(inputFields.Aggregate(
                             $"Campos {choices.resultVerb}os:\n",
                             (current, pair)=>
                                 $"{current}\n{pair.Key} -> {pair.Value.text}"),
                         preencherCampos);
            });
        createUI(basic_button)
            .setText("Voltar")
            .setOnClick(() => {
                clearUI();
                choices.beneficios.Clear();
                escolherBeneficios();
            });
    }

    private void mensagem(string mensagem, UnityAction returnTo) {
        createUI(basic_label).flexible.setText(mensagem);
        createUI(basic_button)
            .setText("Voltar")
            .setOnClick(() => {
                clearUI();
                returnTo.Invoke();
            });
    }

    private T createUI<T>(MyAssetLoader<T> asset) where T : MonoBehaviour {
        return registerUI(Instantiate(asset.Asset, verticalLayout));
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