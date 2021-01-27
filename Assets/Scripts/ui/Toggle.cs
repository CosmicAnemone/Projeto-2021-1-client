using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ui {
    //Just a wrapper to join Unity's own 'button' and 'text' UI elements
    public class Toggle : MonoBehaviour {
        [SerializeField] private Text text;
        [SerializeField] private Button button;
        [SerializeField] private Graphic graphic;
        private bool _isOn;
        private UnityAction<bool> onToggle;

        public bool isOn {
            get => _isOn;
            set {
                _isOn = value;
                graphic.color = value ? Color.green : Color.red;
            }
        }

        private void Awake() {
            isOn = false;
            button.onClick.AddListener(() => {
                isOn = !isOn;
                onToggle?.Invoke(isOn);
            });
        }

        public Toggle setText(string newText) {
            text.text = newText;
            return this;
        }

        public Toggle setOnToggle(UnityAction<bool> onToggle) {
            this.onToggle = onToggle;
            return this;
        }
    }
}