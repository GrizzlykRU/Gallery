using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using Utility;

namespace Gallery
{
    public class GalleryView : MonoBehaviour
    {
        [SerializeField] [NotNull] private ImageItem imageTemplate;
        [SerializeField] [NotNull] private Transform containter;
        [SerializeField] [NotNull] private UnityEvent onLoadFailed;
        [SerializeField] [NotNull] private UnityEvent onLoadSuccess;
        
        private const string ResourceLink = "http://data.ikppbb.com/test-task-unity-data/pics/";
        private const string BaseUrl = "http://data.ikppbb.com";
        private IReadOnlyList<string> imagesLinks;
        private List<ImageItem> images = new();

        private void Start()
        {
            if (!ImageCache.IsEmpty())
            {
                onLoadSuccess.Invoke();
            }
            StartCoroutine(SetupLinks());
        }

        private IEnumerator SetupLinks()
        {
            var page = string.Empty;
            yield return HtmlParser.GetText(ResourceLink, result => page = result);
            if (!string.IsNullOrEmpty(page))
            {
                onLoadSuccess.Invoke();
                imagesLinks = HtmlParser.GetLinks(page).Where(x => x.EndsWith(".jpg")).ToList();
                SetupView();
            }
            else
            {
                onLoadFailed.Invoke();
            }
        }

        private void SetupView()
        {
            foreach (var link in imagesLinks)
            {
                var image = Instantiate(imageTemplate, containter);
                image.Init(string.Concat(BaseUrl, link));
                image.gameObject.SetActive(true);
                images.Add(image);
            }
        }

        public void CheckImagesVisibility()
        {
            foreach (var image in images)
            {
                image.CheckVisibility();
            }
        }

        public void TryAgain()
        {
            StartCoroutine(SetupLinks());
        }
    }
}