using System;
using System.Collections.Generic;
using System.Linq;
using TextBlockFX;
using TextBlockFX.Win2D.UWP;
using TextBlockFX.Win2D.UWP.Effects;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Pivot = TextBlockFX.Win2D.UWP.Effects.Pivot;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Protecc
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TestPage : Page
    {
        public TestPage()
        {
            this.InitializeComponent();
            _timer.Interval = TimeSpan.FromMilliseconds(1000);
            _timer.Tick += _timer_Tick;
            _sampleTexts = _inOtherWords;
            TBFX.TextEffect = new MotionBlur();
        }
        private readonly DispatcherTimer _timer = new DispatcherTimer();

        private int _index = -1;

        private string[] _sampleTexts;

        private readonly string[] _inOtherWords = new[]
        {
            "Fly me to the 🌕moon",
            "And let me play among the 🌟stars",
            "Let me see what spring is like on",
            "Jupiter and Mars",
            "In other words, hold my hand",
            "In other words, darling, kiss me",
            "Fill my heart with 🎶song",
            "And let me sing forevermore",
            "You are all I long for",
            "All I worship and adore",
            "In other words, please be true",
            "In other words, I ❤️love you",
        };

        private ITextEffect _selectedEffect;
        private int _selectedSampleTextIndex;

        public List<BuiltInEffect> BuiltInEffects => new List<BuiltInEffect>()
        {
            new BuiltInEffect("Default", new Default()),
            new BuiltInEffect("Motion Blur", new MotionBlur()),
            new BuiltInEffect("Blur", new Blur()),
            new BuiltInEffect("Elastic", new Elastic()),
            new BuiltInEffect("Zoom", new Zoom()),
            new BuiltInEffect("Pivot", new Pivot())
        };

        public ITextEffect SelectedEffect;


        public int SelectedSampleTextIndex = 0;


        private void _timer_Tick(object sender, object e)
        {
            SetSampleText();
            _timer.Stop();
        }

        private void AutoPlayButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (AutoPlayButton.IsChecked == true)
            {
                _index = -1;
                SetSampleText();
            }
            else
            {
                _timer.Stop();
            }
        }

        private void SetSampleText()
        {
            _index = (_index + 1) % _sampleTexts.Length;
            string text = _sampleTexts[_index];
            TBFX.Text = text;
        }

        private void TBFX_OnRedrawStateChanged(object sender, RedrawState e)
        {
            if (AutoPlayButton.IsChecked == true && e == RedrawState.Idle)
            {
                _timer.Start();
            }
        }

        private void EffectComboBox_OnLoaded(object sender, RoutedEventArgs e)
        {
            EffectComboBox.SelectedIndex = 0;
        }

        private void TextComboBox_OnLoaded(object sender, RoutedEventArgs e)
        {
            //TextComboBox.SelectedIndex = 0;
        }

        private static List<ComboWrapper<T>> GetEnumAsList<T>()
        {
            var names = Enum.GetNames(typeof(T)).ToList();
            var values = Enum.GetValues(typeof(T)).Cast<T>().ToList();
            return names.Zip(values, (k, v) => new ComboWrapper<T>(k, v)).ToList();
        }
    }

    public class ComboWrapper<T>
    {
        public string Name { get; }

        public T Value { get; }

        public ComboWrapper(string name, T value)
        {
            Name = name;
            Value = value;
        }
    }

    public class BuiltInEffect
    {
        public string Name { get; }

        public ITextEffect Effect { get; }

        public BuiltInEffect(string name, ITextEffect effect)
        {
            Name = name;
            Effect = effect;
        }
    }
}
