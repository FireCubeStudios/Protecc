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
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using FX = TextBlockFX.Win2D.UWP;
using ProgressRing = Microsoft.UI.Xaml.Controls.ProgressRing;

namespace Protecc.Helpers
{
    public class TOTPHelper : IDisposable
    {
       // private FX.TextBlockFX CodeBlock;
        private FX.TextBlockFX CodeBlock;
        private ProgressRing Progress;
        private DispatcherTimer dispatcherTimer;
        private Totp OTP;
        private int Time;
        private int Digits;
        public TOTPHelper(FX.TextBlockFX codeBlock, ProgressRing ring, VaultItem vault)
        {
            CodeBlock = codeBlock;
            CodeBlock.RedrawStateChanged += CodeBlock_RedrawStateChanged;
            CodeBlock.TextEffect = new MotionBlur();
            Progress = ring;
            Progress.Maximum = Time;
            Time = DataHelper.DecodeTime(vault.Resource);
            Digits = DataHelper.DecodeDigits(vault.Resource);
            Progress.Maximum = Time - 1;
            OTP = new Totp(CredentialService.GetKey(vault), step:Time, DataHelper.DecodeEncryption(vault.Resource), totpSize:Digits);
            CodeBlock.Text = FormatCode(OTP.ComputeTotp(DateTime.UtcNow));
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += Timer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
     
        }

        private void CodeBlock_RedrawStateChanged(object sender, RedrawState e)
        {
            if (e == RedrawState.Idle)
            {
                dispatcherTimer.Start();
            }
        }

        private async void Timer_Tick(object sender, object e)
        {
            await Task.Run(async () =>
            {
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                {
                    if (OTP.RemainingSeconds() == 1)
                    {
                        Progress.Value = 0;
                        Progress.Value = Time - 1;
                        await Task.Delay(1000);
                        CodeBlock.Text = FormatCode(OTP.ComputeTotp(DateTime.UtcNow));
                    }
                    else if(OTP.RemainingSeconds() <= Time - 1)
                    {
                        Progress.Value = OTP.RemainingSeconds();
                    }
                });
            });
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

        public void Dispose()
        {
            dispatcherTimer.Stop();
            dispatcherTimer.Tick -= Timer_Tick;
            OTP = null;
            CodeBlock = null;
            Progress = null;
        }
    }
}
