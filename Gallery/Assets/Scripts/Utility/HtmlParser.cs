using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine.Networking;

namespace Utility
{
    public class HtmlParser
    {
        public static IEnumerator GetText(string url, Action<string> onComplete)
        {
            var request = UnityWebRequest.Get(url);
            yield return request.Send();
            onComplete?.Invoke(request.downloadHandler.text);
        }

        public static IReadOnlyList<string> GetLinks(string html)
        {
            var pattern = @"href\s*=\s*(?:[""'](?<1>[^""']*)[""']|(?<1>[^>\s]+))";
            var macthes = Regex.Matches(html, pattern,
                RegexOptions.IgnoreCase | RegexOptions.Compiled);
            return macthes.Select(x => x.Groups[1].Value).ToList();
        }
    }
}