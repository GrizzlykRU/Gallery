using Gallery;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace View
{
    public class View : MonoBehaviour
    {
        [SerializeField] [NotNull] private Image image;


        private void Start()
        {
            var texture = ImageCache.ChosenImage;
            image.sprite = Sprite.Create((Texture2D) texture, new Rect(0.0f, 0.0f, texture.width, texture.height),
                new Vector2(0.5f, 0.5f), 100.0f);
            image.preserveAspect = true;
            Screen.orientation = ScreenOrientation.AutoRotation;
        }

        public void Close()
        {
            Screen.orientation = ScreenOrientation.Portrait;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                Close();
            }
        }
    }
}