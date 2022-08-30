using Protecc.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Credentials;
using Windows.UI;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.Security.Cryptography;
using Windows.Storage.Streams;
using Protecc.Helpers;
using System.Diagnostics;
using OtpNet;

namespace Protecc.Services
{
    ///     Credentials are stored with three parameters. Name, Key and Resource
    ///     The Name contains the account name
    ///     The Key contains the 2FA key string
    ///     The resource contains a 8 digit identifier string with format: 
    ///     #Color in HEX format, Time in seconds (max 2 digits), Number of code digits (max 1 digit), Index representing encryptionmode enum
    ///     Encryption enums: 0 = Sha1, 1 = Sha256, 2 = Sha512
    ///     Example: Color white, 30 seconds, 6 digits, Sha512 will be FFFFFF3062
    ///     Example: Color black, 60 seconds, 8 digits, Sha1 will be 0000006080
    ///     Full Example: Name "Twitter", Color blue, 30 seconds, 6 digits, Sha1 will be "Twitter0000ff3060"

    public class CredentialService
    {
        private static PasswordVault Vault = new PasswordVault();
        public static ObservableCollection<VaultItem> CredentialList = new ObservableCollection<VaultItem>();

        protected internal static void StoreNewCredential(string Name, string Key, Color Color, int TimeIndex, int DigitsIndex, int Encryptionindex)
        {
            string Resource = DataHelper.Encode(Color, TimeIndex, DigitsIndex, Encryptionindex);
            Vault.Add(new PasswordCredential(Resource, Name, Key));
            CredentialList.Add(new VaultItem(Name, Resource));
        }

        protected internal static byte[] GetKey(VaultItem vaultItem)
        {
            byte[] Key;
            Key = Base32Encoding.ToBytes(Vault.Retrieve(vaultItem.Resource, vaultItem.Name).Password);
            return Key;
        }

        protected internal static void RemoveItem(VaultItem vaultItem)
        {
            Vault.Remove(Vault.Retrieve(vaultItem.Resource, vaultItem.Name));
            CredentialList.Remove(vaultItem);
        }

        protected internal async static Task RefreshListAsync()
        {
            CredentialList.Clear();
            await Task.Run(async() =>
            {
                foreach (var i in Vault.RetrieveAll())
                {
                    await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        CredentialList.Add(new VaultItem(i.UserName, i.Resource));
                    });
                }
            });
        }

        protected internal async static Task<List<Account>> ExportAccountsAsync()
        {
            List<Account> Accounts = new();
            await Task.Run(async () =>
            {
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    CredentialList.Clear();
                    foreach (var i in Vault.RetrieveAll())
                    {
                        Accounts.Add(new Account() { 
                            Name = i.UserName,
                            Resource = i.Resource,
                            Key = i.Password
                        });
                    }
                });
            });
            return Accounts;
        }
    }
}
