using CubeKit.UI.Services;
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
                var userKey = keyCreationResult.Credential;
                var publicKey = userKey.RetrievePublicKey();
                var keyAttestationResult = await userKey.GetAttestationAsync();
                IBuffer keyAttestation = null;
                IBuffer certificateChain = null;
                bool keyAttestationIncluded = false;
                bool keyAttestationCanBeRetrievedLater = false;

                keyAttestationResult = await userKey.GetAttestationAsync();
                KeyCredentialAttestationStatus keyAttestationRetryType = 0;

                if (keyAttestationResult.Status == KeyCredentialAttestationStatus.Success)
                {
                    keyAttestationIncluded = true;
                    keyAttestation = keyAttestationResult.AttestationBuffer;
                    certificateChain = keyAttestationResult.CertificateChainBuffer;
                    keyAttestationRetryType = KeyCredentialAttestationStatus.Success;
                    Frame rootFrame = Window.Current.Content as Frame;
                    rootFrame.Navigate(typeof(MainPage));
                }
                else if (keyAttestationResult.Status == KeyCredentialAttestationStatus.TemporaryFailure)
                {
                    keyAttestationRetryType = KeyCredentialAttestationStatus.TemporaryFailure;
                    keyAttestationCanBeRetrievedLater = true;
                    Title.Text = "Authentication failed";
                    Title.Foreground = RedLinearGradientBrush;
                    Ring.Visibility = Visibility.Collapsed;
                    AuthButton.Visibility = Visibility.Visible;
                    Content.Background = new SolidColorBrush(Colors.Black);
                    Aurora.Visibility = Visibility.Collapsed;
                    RSOD.Visibility = Visibility.Visible;
                }
                else if (keyAttestationResult.Status == KeyCredentialAttestationStatus.NotSupported)
                {
                    keyAttestationRetryType = KeyCredentialAttestationStatus.NotSupported;
                    keyAttestationCanBeRetrievedLater = true;
                    Title.Text = "Authentication not supported";
                    Title.Foreground = RedLinearGradientBrush;
                    Ring.Visibility = Visibility.Collapsed;
                    AuthButton.Visibility = Visibility.Visible;
                    Content.Background = new SolidColorBrush(Colors.Black);
                    Aurora.Visibility = Visibility.Collapsed;
                    RSOD.Visibility = Visibility.Visible;
                }
            }
            else if (keyCreationResult.Status == KeyCredentialStatus.UserCanceled)
            {
                // Show error message to the user to get confirmation that user
                // does not want to enroll.
                Title.Text = "Authentication cancelled";
                Title.Foreground = RedLinearGradientBrush;
                Ring.Visibility = Visibility.Collapsed;
                AuthButton.Visibility = Visibility.Visible;
                Content.Background = new SolidColorBrush(Colors.Black);
                Aurora.Visibility = Visibility.Collapsed;
                RSOD.Visibility = Visibility.Visible;
            }
        }

        private void Authenticate_Click(object sender, RoutedEventArgs e) => Authenticate();
    }
}
