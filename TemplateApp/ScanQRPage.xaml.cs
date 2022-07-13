using CubeKit.UI.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ZXing.Mobile;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Protecc
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ScanQRPage : Page
    {
		MobileBarcodeScanner scanner;
		Frame rootFrame = Window.Current.Content as Frame;

		public ScanQRPage()
        {
            this.InitializeComponent();
			//Create a new instance of our scanner
			scanner = new MobileBarcodeScanner(this.Dispatcher);
			scanner.RootFrame = this.Frame;
			scanner.Dispatcher = this.Dispatcher;
			scanner.OnCameraError += Scanner_OnCameraError;
			scanner.OnCameraInitialized += Scanner_OnCameraInitialized;

			UIElement CustomOverlayElement = CameraOverlay.Children[0];
			CameraOverlay.Children.RemoveAt(0);
			scanner.CustomOverlay = CustomOverlayElement;
			scanner.UseCustomOverlay = true;

			//Start scanning
			scanner.Scan(new MobileBarcodeScanningOptions { AutoRotate = true }).ContinueWith(t =>
			{
				if (t.Result != null)
					HandleScanResult(t.Result);
					t.Dispose();
			});
		}
		void Scanner_OnCameraInitialized()
		{
			//handle initialization
		}

		void Scanner_OnCameraError(IEnumerable<string> errors)
		{
			if (errors != null)
			{
				rootFrame.Navigate(typeof(ErrorPage), errors.ToArray()[0]);
			}
		}


		public void HandleScanResult(ZXing.Result result)
		{
			if (result != null && !string.IsNullOrEmpty(result.Text))
				Create(result.Text);
		}

		public async void Create(string key)
        {
			await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
			{
				rootFrame.Navigate(typeof(AddAccountPage), key);
			});
		}

		private void Close_Click(object sender, RoutedEventArgs e)
		{
			rootFrame.Navigate(typeof(MainPage));
		}
	}
}
