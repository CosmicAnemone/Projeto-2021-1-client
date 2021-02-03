using UnityEngine;

public static class Defs {
    public static readonly Vector2 messageBorder = new Vector2(20, 20);
    // Normalmente usaríamos uma porta fixa, mas para permitir testes alheios,
    // será permitido ao cliente escolher a porta, nesse protótipo.
    public static int PORT;
    public static readonly string base_url = $"http://localhost:{PORT}/registro";
}