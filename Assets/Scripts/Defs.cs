using UnityEngine;

public static class Defs {
    public static readonly Vector2 messageBorder = new Vector2(20, 20);

    private const int PORT = 00000;
    public static readonly string base_url = $"http://localhost:{PORT}/registro";
}