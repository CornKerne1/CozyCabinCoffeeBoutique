using UnityEngine;
using UnityEngine.UI;

// VRUI Button Icon Toggle Component
namespace SpaceBear.VRUI
{
    [ExecuteAlways]
    public class VRUIButtonIconToggle : MonoBehaviour
    {
        public Texture offImage;
        public Texture onImage;
        public bool isOn = false;

        UnityEngine.UI.Button button;
        RawImage image;

        void Start()
        {
            button = gameObject.GetComponentInChildren<UnityEngine.UI.Button>();
            image = gameObject.GetComponentInChildren<RawImage>();

            button.onClick.AddListener(() => { isOn = !isOn; });
        }

        private void Update()
        {
            image.texture = isOn ? onImage : offImage;
        }

    }
}
