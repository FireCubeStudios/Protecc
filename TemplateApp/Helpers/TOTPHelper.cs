using Microsoft.UI.Xaml.Controls;
using OtpNet;
using Protecc.Classes;
using Protecc.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextBlockFX;
using TextBlockFX.Win2D.UWP;
using TextBlockFX.Win2D.UWP.Effects;
using Windows.ApplicationModel.Core;
using Windows.System.Threading;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using FX = TextBlockFX.Win2D.UWP;
using ProgressRing = Microsoft.UI.Xaml.Controls.ProgressRing;

namespace Protecc.Helpers
{
    public class TOTPHelper : INotifyPropertyChanged, IDisposable
    {
        private ThreadPoolTimer PeriodicTimer;
        private Totp OTP;
        private int Time;
        private string _Code;
        public string Code
        {
            get
            {
                return _Code;
            }
            set
            {
                SetField(ref _Code, value, "Code");
            }
        }
        private double _Maximum;
        public double Maximum
        {
            get
            {
                return _Maximum;
            }
            set
            {
                SetField(ref _Maximum, value, "Maximum");
            }
        }
        private double _TimeRemaining;
        public double TimeRemaining
        {
            get 
            {
                return _TimeRemaining; 
            }
            set
            {
                SetField(ref _TimeRemaining, value, "TimeRemaining");
            }
        }
        private int Digits;
        public TOTPHelper(VaultItem vault)
        {
            Time = DataHelper.DecodeTime(vault.Resource);
            Maximum = Time;
            Digits = DataHelper.DecodeDigits(vault.Resource);
            Maximum = Time - 1;
            OTP = new Totp(CredentialService.GetKey(vault), step:Time, DataHelper.DecodeEncryption(vault.Resource), totpSize:Digits);
            Code = FormatCode(OTP.ComputeTotp(DateTime.UtcNow));
            PeriodicTimer = ThreadPoolTimer.CreatePeriodicTimer(TimerElapsed, TimeSpan.FromMilliseconds(1000));
        }

        private async void TimerElapsed(ThreadPoolTimer timer)
        {      
            if (OTP.RemainingSeconds() == 1)
            {
                TimeRemaining = 0;
                TimeRemaining = Time - 1;
                await Task.Delay(1000);
                Code = FormatCode(OTP.ComputeTotp(DateTime.UtcNow));
            }
            else if (OTP.RemainingSeconds() <= Time - 1)
            {
                TimeRemaining = OTP.RemainingSeconds();
            }
        }

        private string FormatCode(string code)
        {
            if (Digits == 6)
            {
                return code.Substring(0, 2) + " " + code.Substring(2, 2) + " " + code.Substring(4, 2);
            }
            else // Assume 8 digits
            {
                return code.Substring(0, 2) + " " + code.Substring(2, 2) + " " + code.Substring(4, 2) + " " + code.Substring(6, 2);
            }
        }

        public void Dispose() => PeriodicTimer.Cancel();

        public event PropertyChangedEventHandler PropertyChanged;
        protected async virtual void OnPropertyChanged(string propertyName)
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                PropertyChangedEventHandler handler = PropertyChanged;
                if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
            });
        }
        protected bool SetField<T>(ref T field, T value, string propertyName)
        {
                if (EqualityComparer<T>.Default.Equals(field, value)) return false;
                field = value;
                OnPropertyChanged(propertyName);
                return true;
        }
    }
}
