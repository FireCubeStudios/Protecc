using Protecc.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;
using ZXing;

namespace Protecc.Helpers
{
    public class TOTPUriHelper
    {
        public static async Task<TOTPClass> GetFromClipboard()
        {
            string res = null;
            try
            {
                DataPackageView dataPackageView = Clipboard.GetContent();
                if (dataPackageView.Contains(StandardDataFormats.Bitmap))
                {
                    IRandomAccessStreamReference imageReceived = null;
                    try
                    {
                        imageReceived = await dataPackageView.GetBitmapAsync();
                    }
                    catch { }
                    finally
                    {
                        try
                        {
                            if (imageReceived != null)
                            {
                                using IRandomAccessStreamWithContentType imageStream = await imageReceived.OpenReadAsync();
                                BitmapDecoder bitmapDecoder = await BitmapDecoder.CreateAsync(imageStream);
                                SoftwareBitmap softwareBitmap = await bitmapDecoder.GetSoftwareBitmapAsync();

                                res = DecodeBitmap(softwareBitmap);
                            }
                        }
                        catch { }
                    }
                }
            }
            catch { }
            return new TOTPClass(res);
        }

        public static string DecodeBitmap(SoftwareBitmap bitmap)
        {
            var reader = new BarcodeReader();
            var decoded = reader.Decode(bitmap);
            if (decoded != null)
            {
                return decoded.Text;
            }
            else
            {
                return null;
            }
        }
    }
}
