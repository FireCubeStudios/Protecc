using CubeKit.UI.Services;
using Protecc.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Security.Credentials;
using Windows.Security.Credentials.UI;
using Windows.Storage.Streams;
using Windows.UI;
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
    public sealed partial class WindowsHelloPage : Page
    {
        public WindowsHelloPage()
        {
            this.InitializeComponent();
            WindowService.Initialize(AppTitleBar, AppTitle);
            Authenticate();
        }

        private async void Authenticate()
        {
            Ring.Visibility = Visibility.Visible;
            AuthButton.Visibility = Visibility.Collapsed;
            Title.Text = "Authenticating using Windows Hello...";
            Title.Foreground = AccentLinearGradientBrush;
            Content.Background = new SolidColorBrush(Colors.Transparent);
            Aurora.Visibility = Visibility.Visible;
            RSOD.Visibility = Visibility.Collapsed;
            var keyCredentialAvailable = await KeyCredentialManager.IsSupportedAsync();
            if (!keyCredentialAvailable)
            {
                // The user didn't set up a PIN yet
                // In this page this code wouldn't be hit
                return;
            }
            var keyCreationResult = await KeyCredentialManager.RequestCreateAsync("Protecc", KeyCredentialCreationOption.ReplaceExisting);
            if (keyCreationResult.Status == KeyCredentialStatus.Success)
            {
                Frame rootFrame = Window.Current.Content as Frame;
                rootFrame.Navigate(typeof(MainPage));
               // await CredentialService.RefreshListAsync();
            }
            else if (keyCreationResult.Status == KeyCredentialStatus.UserCanceled)
            {
                Error("Authentication cancelled");
            }
            else if(keyCreationResult.Status == KeyCredentialStatus.UnknownError)
            {
                Error("Unknown Error occurred");
            }
            else
            {
                Error("Something went wrong");
            }
        }

        private void Error(string message)
        {
            Title.Text = message;
            Title.Foreground = RedLinearGradientBrush;
            Ring.Visibility = Visibility.Collapsed;
            AuthButton.Visibility = Visibility.Visible;
            Content.Background = new SolidColorBrush(Colors.Black);
            Aurora.Visibility = Visibility.Collapsed;
            RSOD.Visibility = Visibility.Visible;
        }
        private void Authenticate_Click(object sender, RoutedEventArgs e) => Authenticate();
    }
}
