using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Toolkit.Win32.UI.Controls;
using Newtonsoft.Json;

namespace WebViewSamples.Forms.Favicons
{

    public static partial class WebViewExtensions
    {
        private const string EmptyFavIcon = "R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7";
        private static readonly byte[] EmptyFavIconBytes = Convert.FromBase64String(EmptyFavIcon);

        private static bool ConvertToIcon(Stream input, Stream output, int size = 32, bool preserveAspectRatio = false)
        {
            Bitmap inputBitmap = null;

            try
            {
                inputBitmap = (Bitmap)Image.FromStream(input);
            }
            catch (ArgumentException)
            {
            }

            if (inputBitmap == null)
            {
                return false;
            }

            float width = size, height = size;
            if (preserveAspectRatio)
            {
                if (inputBitmap.Width > inputBitmap.Height)
                {
                    height = ((float)inputBitmap.Height / inputBitmap.Width) * size;
                }
                else
                {
                    width = ((float)inputBitmap.Width / inputBitmap.Height) * size;
                }
            }

            var newBitmap = new Bitmap(inputBitmap, new Size((int)width, (int)height));

            // save the resized png into a memory stream for future use
            using (var memoryStream = new MemoryStream())
            {
                newBitmap.Save(memoryStream, ImageFormat.Png);

                var iconWriter = new BinaryWriter(output);

                // 0-1 reserved, 0
                iconWriter.Write((byte)0);
                iconWriter.Write((byte)0);

                // 2-3 image type, 1 = icon, 2 = cursor
                iconWriter.Write((short)1);

                // 4-5 number of images
                iconWriter.Write((short)1);

                // image entry 1
                // 0 image width
                iconWriter.Write((byte)width);
                // 1 image height
                iconWriter.Write((byte)height);

                // 2 number of colors
                iconWriter.Write((byte)0);

                // 3 reserved
                iconWriter.Write((byte)0);

                // 4-5 color planes
                iconWriter.Write((short)0);

                // 6-7 bits per pixel
                iconWriter.Write((short)32);

                // 8-11 size of image data
                iconWriter.Write((int)memoryStream.Length);

                // 12-15 offset of image data
                iconWriter.Write((int)(6 + 16));

                // write image data
                // png data must contain the whole png data file
                iconWriter.Write(memoryStream.ToArray());

                iconWriter.Flush();
            }

            return true;
        }

        private static async Task<IEnumerable<Stream>> GetFavIconAsync(this IWebView webView)
        {
            var streams = new List<Stream>();

            // Download the ico
            foreach (var iconUri in await GetFavIconUriAsync(webView))
            {
                var request = (HttpWebRequest)WebRequest.Create(iconUri);
                request.AllowAutoRedirect = true;
                request.Timeout = 500;

                Stream stream = null;
                string mime = null;

                try
                {
                    var response = request.GetResponse();
                    stream = response.GetResponseStream();
                    mime = response.ContentType;
                }
                catch (WebException)
                {
                }

                // Not really an image, so skip
                if ("image/svg+xml".Equals(mime, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (stream != null)
                {
                    var conversionRequired = true;
                    if (!string.IsNullOrEmpty(mime))
                    {
                        // Image is actually a real ICO file
                        conversionRequired &= !"image/vnd.microsoft.icon".Equals(mime, StringComparison.OrdinalIgnoreCase);

                        // These tend to be some kind of icon or image (GIF, PNG, BMP, etc.)
                        conversionRequired &= !"image/x-icon".Equals(mime, StringComparison.OrdinalIgnoreCase);
                    }

                    if (conversionRequired)
                    {
                        var iconStream = new MemoryStream();
                        if (ConvertToIcon(stream, iconStream))
                        {
                            stream = iconStream;
                        }
                    }

                    streams.Add(stream);
                }
            }

            return streams;
        }

        private static async Task<List<string>> GetFavIconUriAsync(this IWebView webView)
        {
            // Asynchronously check for favicon in the web page markup
            const string ReadLinkJavaScript = @"
JSON.stringify(Array.from(document.getElementsByTagName('link'))
    .filter(link => link.rel.includes('icon'))
    .map(link => link.href))
";
            var result = await webView.InvokeScriptAsync("eval", ReadLinkJavaScript);

            // Parse result
            // NOTE: Uses Newtonsoft JSON as it is a popular library for parsing JSON
            return JsonConvert.DeserializeObject<List<string>>(result);
        }
    }
}
