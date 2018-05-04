using System;

namespace WebViewSamples
{
    public static class Constants
    {
        public const string DefaultNavigationTarget = "https://www.bing.com";
        public static readonly Uri DefaultNavigationTargetUri = new Uri(DefaultNavigationTarget, UriKind.Absolute);
    }
}