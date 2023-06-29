using System.Collections;
using System.Drawing;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utility;
using Color = UnityEngine.Color;

namespace Gallery
{
    public class ImageItem : MonoBehaviour
    {
        [SerializeField] [NotNull] private GameObject placeHolder;
        [SerializeField] [NotNull] private RawImage image;
        [SerializeField] private float requestCooldown = 5.0f;

        private string link;
        private bool imageScoped;
        private RectTransform rectTransform;

        private void OnEnable()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        public void Init(string link)
        {
            this.link = link;
            var cachedImage = ImageCache.GetImage(link);
            if (cachedImage != null)
            {
                imageScoped = true;
                SetImage(cachedImage);
            }
        }

        public void CheckVisibility()
        {
            if (!imageScoped)
            {
                var corners = new Vector3[4];
                rectTransform.GetWorldCorners(corners);
                var topLeft = corners[1];
                var screenPos = Camera.main.WorldToViewportPoint(topLeft);
                if (screenPos.y <= 1 && screenPos.y >= 0)
                {
                    imageScoped = true;
                    StartCoroutine(DownloadImage());
                }
            }
        }

        private IEnumerator DownloadImage()
        {
            var request = UnityWebRequestTexture.GetTexture(link);
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.Success)
            {
                var texture = ((DownloadHandlerTexture) request.downloadHandler).texture;
                SetImage(texture);
                ImageCache.CacheImage(texture, link);
            }
            else
            {
                yield return new WaitForSeconds(requestCooldown);
                StartCoroutine(DownloadImage());
            }
        }

        private void SetImage(Texture texture)
        {
            image.texture = texture;
            placeHolder.SetActive(false);
            image.gameObject.SetActive(true);
        }

        public void OnClick()
        {
            ImageCache.ChooseImage(link);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}