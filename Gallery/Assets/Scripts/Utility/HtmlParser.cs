using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine.Networking;

namespace Utility
{
    public static class HtmlParser
    {
        public static IEnumerator GetText(string url, Action<string> onSucces)
        {
            var request = UnityWebRequest.Get(url);
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.Success)
            {
                onSucces.Invoke(request.downloadHandler.text);
            }
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