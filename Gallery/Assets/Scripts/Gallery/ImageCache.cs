using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gallery
{
    public class ImageCache : MonoBehaviour
    {
        private static Dictionary<string, Texture> images = new();
        private static Texture chosenImage;
        public static Texture ChosenImage => chosenImage;
        private static ImageCache instance;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        public static void ChooseImage(string link)
        {
            chosenImage = images[link];
        }

        public static void CacheImage(Texture texture, string link)
        {
            if (!images.ContainsKey(link))
            {
                images.Add(link, texture);
            }
        }

        public static Texture GetImage(string link) => images.ContainsKey(link) ? images[link] : null;

        public static bool IsEmpty() => !images.Any();
    }
}