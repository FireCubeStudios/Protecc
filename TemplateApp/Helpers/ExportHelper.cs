using Protecc.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Provider;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Protecc.Helpers
{
    public class ExportHelper
    {
        public static async Task<bool> Export()
        {
            var savePicker = new FileSavePicker();
            savePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            // Dropdown of file types the user can save the file as
            savePicker.FileTypeChoices.Add("Protecc 2FA Export", new List<string>() { ".yaml" });
            // Default file name if the user does not type one in or select a file to replace
            savePicker.SuggestedFileName = "Protecc 2FA Export";
            StorageFile file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                var serializer = new SerializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance).Build();
                CachedFileManager.DeferUpdates(file);
                await FileIO.WriteTextAsync(file, serializer.Serialize(await CredentialService.ExportAccountsAsync()));
                FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(file);
                return status == FileUpdateStatus.Complete ? true : false;
            }
            else
            {
                return false;
            }
        }
    }
}
