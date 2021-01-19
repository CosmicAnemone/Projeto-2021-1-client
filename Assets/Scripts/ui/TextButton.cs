using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ui {
    //Just a wrapper to join Unity's own 'button' and 'text' UI elements
    public class TextButton : MonoBehaviour {
        [SerializeField] private Text text;
        [SerializeField] private Button button;

        public TextButton setText(string newText) {
            text.text = newText;
            return this;
        }

        public TextButton setOnClick(UnityAction action) {
            button.onClick.AddListener(action);
            return this;
        }
    }
}