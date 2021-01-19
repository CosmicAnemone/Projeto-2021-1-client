using UnityEngine.UI;

namespace data {
    public static class PlaceholderData {
        public static readonly Beneficio[] beneficios = {
            new Beneficio("Plano de Saúde Norte Europa")
                .setCampo("Nome", InputField.ContentType.Name)
                .setCampo("Data Admissão", InputField.ContentType.Alphanumeric)
                .setCampo("Email", InputField.ContentType.EmailAddress),
            new Beneficio("Plano de Saúde Pampulha Intermedica")
                .setCampo("Nome", InputField.ContentType.Name)
                .setCampo("Data Admissão", InputField.ContentType.Alphanumeric)
                .setCampo("Endereço", InputField.ContentType.Alphanumeric),
            new Beneficio("Plano Odontológico Dental Sorriso")
                .setCampo("Nome", InputField.ContentType.Name)
                .setCampo("Peso(kg)", InputField.ContentType.DecimalNumber)
                .setCampo("Altura(cm)", InputField.ContentType.DecimalNumber),
            new Beneficio("Plano de Saúde Mental Mente Sã, Corpo São")
                .setCampo("Horas meditadas nos últimos 7 dias", InputField.ContentType.IntegerNumber)
        };

        public static readonly Empresa acmeCo =
            new Empresa("Acme Co", beneficios[0], beneficios[2]);
        public static readonly Empresa tioPatinhasBank =
            new Empresa("Tio Patinhas Bank", beneficios[1], beneficios[2], beneficios[3]);
        
        public static readonly Empresa[] empresas = {acmeCo, tioPatinhasBank};
    }
}