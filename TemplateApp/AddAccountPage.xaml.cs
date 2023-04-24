using CubeKit.UI.Services;
using OtpNet;
using Protecc.Classes;
using Protecc.Services;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Protecc
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddAccountPage : Page
    {
        public AddAccountPage()
        {
            this.InitializeComponent();
            WindowService.Initialize(AppTitleBar, AppTitle);
        }

        private async void Submit_Click(object sender, RoutedEventArgs e)
        {
            Ring.Visibility = Visibility.Visible;
            Content.Opacity = 0;
            await Task.Delay(100);
            if (NameBox.Text == "")
            {
                NameBox.Foreground = RedLinearGradientBrush;
                NameBox.Focus(FocusState.Programmatic);
                NameLoadAnimation.Start();
            }
            else if (String.IsNullOrEmpty(KeyBox.Password) || String.IsNullOrWhiteSpace(KeyBox.Password) || KeyBox.Password.Length < 2)
            {
                KeyBox.Foreground = RedLinearGradientBrush;
                KeyBox.Focus(FocusState.Programmatic);
                PasswordLoadAnimation.Start();
            }
            else
            {
                try
                {
                    var baseKey = Base32Encoding.ToBytes(KeyBox.Password); // Validate key
                    Totp OTP = new Totp(baseKey); // Validate safety
                    OTP = null;
                }
                catch
                {
                    KeyBox.Foreground = RedLinearGradientBrush;
                    KeyBox.Focus(FocusState.Programmatic);
                    PasswordLoadAnimation.Start();
                    Ring.Visibility = Visibility.Collapsed;
                    Content.Opacity = 1;
                    return;
                }
                CredentialService.StoreNewCredential(NameBox.Text, KeyBox.Password, ColorPicker.Color, TimeOptions.SelectedIndex, DigitOptions.SelectedIndex, EncryptionMode.SelectedIndex);
                Frame rootFrame = Window.Current.Content as Frame;
                rootFrame.Navigate(typeof(MainPage));
                return;
            }
            Ring.Visibility = Visibility.Collapsed;
            Content.Opacity = 1;
        }

        private async void Close_Click(object sender, RoutedEventArgs e)
        {
            Ring.Visibility = Visibility.Visible;
            Content.Opacity = 0;
            await Task.Delay(100);
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(MainPage));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter.GetType() == typeof(string))
            {
                KeyBox.Password = e.Parameter as string;
            }
            else if (e.Parameter.GetType() == typeof(TOTPClass))
            {
                TOTPClass OTP = e.Parameter as TOTPClass;
                NameBox.Text = OTP.Issuer ?? OTP.Account;
                KeyBox.Password = OTP.Secret;
                EncryptionMode.SelectedIndex = (int)OTP.Algorithm;
                DigitOptions.SelectedIndex = OTP.Secret == "6" ? 0 : 1;
                TimeOptions.SelectedIndex = OTP.Secret == "30" ? 0 : 1;
            }
        }
    }
}
