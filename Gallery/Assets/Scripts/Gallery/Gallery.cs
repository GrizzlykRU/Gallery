using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace Gallery
{
    public class Gallery : MonoBehaviour
    {
        [SerializeField] private ImageItem imageTemplate;
        [SerializeField] private Transform containter;
        private const string ResourceLink = "http://data.ikppbb.com/test-task-unity-data/pics/";
        private const string BaseUrl = "http://data.ikppbb.com";
        private IReadOnlyList<string> imagesLinks;
        private List<ImageItem> images = new();
        private ScrollRect scroll;

        private void OnEnable()
        {
            StartCoroutine(SetupLinks());
            scroll = GetComponent<ScrollRect>();
        }

        private IEnumerator SetupLinks()
        {
            var page = string.Empty;
            yield return HtmlParser.GetText(ResourceLink, (html) => page = html);
            if (!string.IsNullOrEmpty(page))
            {
                imagesLinks = HtmlParser.GetLinks(page).Where(x => x.EndsWith(".jpg")).ToList();
                SetupView();
            }
        }

        private void SetupView()
        {
            foreach (var link in imagesLinks)
            {
                var image = Instantiate(imageTemplate, containter);
                image.Init(string.Concat(BaseUrl, link));
                scroll.onValueChanged.AddListener((position) => image.CheckVisibility());
                image.gameObject.SetActive(true);
                images.Add(image);
            }
        }
    }
}