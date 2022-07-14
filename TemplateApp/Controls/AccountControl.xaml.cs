using Protecc.Classes;
using Protecc.Helpers;
using Protecc.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using TextBlockFX;
using TextBlockFX.Win2D.UWP.Effects;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Security.Credentials;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Protecc.Controls
{
    public sealed partial class AccountControl : UserControl
    {
        private TOTPHelper TOTP;
        private bool WasChecked = false; //Used when Window deactivated to determine if filter was already on

        public VaultItem AccountVaultItem
        {
            get { return (VaultItem)GetValue(AccountVaultItemProperty); }
            set { 
                SetValue(AccountVaultItemProperty, value);
                TOTP = new TOTPHelper(AccountVaultItem);
            }
        }
        public static readonly DependencyProperty AccountVaultItemProperty =
                   DependencyProperty.Register("AccountVaultItem", typeof(VaultItem), typeof(AccountControl), null);

        public AccountControl()
        {
            this.InitializeComponent();
            Bindings.Update();
            Window.Current.Activated += Current_Activated;
        }

        private void Current_Activated(object sender, WindowActivatedEventArgs e)
        {
            if (new SettingsClass().FocusBlur)
            {
                if (e.WindowActivationState == CoreWindowActivationState.Deactivated)
                {
                    WasChecked = (bool)PrivacyButton.IsChecked;
                    PrivacyButton.IsChecked = true;
                }
                else
                {
                    PrivacyButton.IsChecked = WasChecked;
                    WasChecked = false;
                }
            }
            Bindings.Update();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            TOTP.Dispose();
            CredentialService.RemoveItem(AccountVaultItem);
        }

        private async void Copy_Click(object sender, RoutedEventArgs e)
        {
            CopyIcon.Symbol = Fluent.Icons.FluentSymbol.Copy20;
            try
            {
                DataPackage dataPackage = new DataPackage();
                dataPackage.RequestedOperation = DataPackageOperation.Copy;
                dataPackage.SetText(CodeBlock.Text.Replace(" ", ""));
                Clipboard.SetContent(dataPackage);
                CopyIcon.Symbol = Fluent.Icons.FluentSymbol.Checkmark20;
            }
            catch
            {
                CopyIcon.Symbol = Fluent.Icons.FluentSymbol.ErrorCircle20;
            }
            await Task.Delay(2000);
            CopyIcon.Symbol = Fluent.Icons.FluentSymbol.Copy20;
        }

        private void Content_Unloaded(object sender, RoutedEventArgs e) => TOTP.Dispose();

        //private void Content_Loaded(object sender, RoutedEventArgs e) => TOTP = new TOTPHelper(CodeBlock, Progress, AccountVaultItem);
    }
}
