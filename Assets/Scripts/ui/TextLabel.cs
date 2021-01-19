using UnityEngine;
using UnityEngine.UI;

namespace ui {
    //Again, just a wrapper to facilitate access to Unity's own Text UI (which would be in a child GameObject)
    public class TextLabel : MonoBehaviour {
        [SerializeField] private Text text;
        private bool _flexible;
        private RectTransform rectTransform => (RectTransform) transform;
        public TextLabel flexible {
            get {
                _flexible = true;
                return this;
            }
        }

        public TextLabel setText(string newText) {
            text.text = newText;
            if (_flexible) {
                rectTransform.SetSizeWithCurrentAnchors(
                    RectTransform.Axis.Horizontal,
                    text.preferredWidth + Defs.messageBorder.x);
                rectTransform.SetSizeWithCurrentAnchors(
                    RectTransform.Axis.Vertical,
                    text.preferredHeight + Defs.messageBorder.y);
            }
            return this;
        }
    }
}