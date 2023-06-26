using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Gallery
{
    public class ImageItem : MonoBehaviour
    {
        [SerializeField] private GameObject placeHolder;
        [SerializeField] private RawImage image;
        private string link;
        private bool seen;
        private RectTransform rectTransform;

        private void OnEnable()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        public void Init(string link)
        {
            this.link = link;
        }

        public void CheckVisibility()
        {
            if (!seen)
            {
                var screenPos = Camera.main.WorldToViewportPoint(transform.position);
                if (screenPos.y <= 1 && screenPos.y >= 0)
                {
                    seen = true;
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
                image.texture = texture;
                placeHolder.SetActive(false);
                image.gameObject.SetActive(true);
            }
        }
    }
}