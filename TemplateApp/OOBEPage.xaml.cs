using CubeKit.UI.Services;
using Protecc.Classes;
using Protecc.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Security.Credentials;
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
    public sealed partial class OOBEPage : Page
    {
        public SettingsClass Settings = new();
        public OOBEPage()
        {
            this.InitializeComponent();
            WindowService.Initialize(AppTitleBar, AppTitle);
            bar1.Value = (FlappyBird.SelectedIndex + 1) * 10;
            LoadingAnimation2.Stop();
            bar1.Foreground = ShineBrush;
            bar2.Foreground = new SolidColorBrush((Color)Application.Current.Resources["SystemAccentColor"]);
            LoadingAnimation.Begin();
            bar2.Value = 0;
            SetupSettings();
        }

        public async void SetupSettings()
        {
            if (!await KeyCredentialManager.IsSupportedAsync())
            {
                WindowsHelloText.Text = "*Windows Hello not setup";
                WindowsHelloSwitch.IsEnabled = false;
            }
        }

        private void CompletedOOBE_Click(object sender, RoutedEventArgs e)
        {
            OpenRing.Visibility = Visibility.Visible;
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(MainPage));
        }

        private void FlappyBird_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Steps.Text = "Step " + (FlappyBird.SelectedIndex + 1) + "/4";
                if (FlappyBird.SelectedIndex <= 1)
                {
                    // Section.Text = "Feature showcase";
                    bar1.Value = (FlappyBird.SelectedIndex + 1) * 10;
                    LoadingAnimation2.Stop();
                    bar1.Foreground = ShineBrush;
                    bar2.Foreground = new SolidColorBrush((Color)Application.Current.Resources["SystemAccentColor"]);
                    LoadingAnimation.Begin();
                    bar2.Value = 0;
                }
                else
                {
                    LoadingAnimation.Stop();
                    bar2.Foreground = ShineBrush;
                    bar1.Foreground = new SolidColorBrush((Color)Application.Current.Resources["SystemAccentColor"]);
                    LoadingAnimation2.Begin();
                    //  Section.Text = "Setup preferences";
                    bar2.Value = (FlappyBird.SelectedIndex - 1) * 10;
                    if (FlappyBird.SelectedIndex == 3)
                    {
                        LoadingAnimation2.Stop();
                        bar2.Foreground = new SolidColorBrush((Color)Application.Current.Resources["SystemAccentColor"]);
                    }
                }
            }
            catch
            {

            }

        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (Window.Current.Bounds.Height > 700)
            {
                Stepper.Visibility = Visibility.Visible;
                Skipper.Visibility = Visibility.Visible;
            }
            else
            {
                Stepper.Visibility = Visibility.Collapsed;
                Skipper.Visibility = Visibility.Collapsed;
            }
        }
    }
}
