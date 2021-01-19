using ui;
using UnityEngine.UI;
using Toggle = ui.Toggle;

public static class Assets {
    public static readonly MyAssetLoader<TextButton> basic_button =
        new MyAssetLoader<TextButton>("basic button");
    public static readonly MyAssetLoader<Toggle> basic_toggle =
        new MyAssetLoader<Toggle>("basic toggle");
    public static readonly MyAssetLoader<TextLabel> basic_label =
        new MyAssetLoader<TextLabel>("basic label");
    public static readonly MyAssetLoader<InputField> basic_input_field =
        new MyAssetLoader<InputField>("basic input field");
}