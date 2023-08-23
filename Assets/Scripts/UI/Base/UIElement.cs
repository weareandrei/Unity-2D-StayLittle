using UnityEngine;
using UnityEngine.UIElements;

namespace UI.Base {
    public class UIElement : MonoBehaviour {
        protected VisualElement visualElement;
        
        private void Awake() {
            visualElement = GetComponent<UIDocument>().rootVisualElement;
        }

        public void Hide() {
            visualElement.style.display = DisplayStyle.None;
        }
        
        public void Display() {
            visualElement.style.display = DisplayStyle.Flex;
        }
        
    }
}