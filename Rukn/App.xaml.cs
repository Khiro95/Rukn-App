using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Rukn.ViewModels;

namespace Rukn
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static readonly Dictionary<string, Typeface> Typefaces = new Dictionary<string, Typeface>(2);

        static App()
        {
            EventManager.RegisterClassHandler(typeof(TextBox), TextBox.KeyUpEvent, new RoutedEventHandler((sender, e) =>
            {
                if (sender is TextBox textBox && e is KeyEventArgs ke)
                {
                    if (ke.Key == Key.Enter && !textBox.AcceptsReturn)
                    {
                        Keyboard.ClearFocus();
                        FocusManager.SetFocusedElement(FocusManager.GetFocusScope(textBox), null);
                    }
                }
            }));
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            FontFamily fontFamily = (FontFamily)Current.Resources[MainViewModel.ArabicFont];
            Typefaces.Add(MainViewModel.ArabicFont, new Typeface(fontFamily, FontStyles.Normal, FontWeights.Normal, FontStretches.Normal));

            fontFamily = (FontFamily)Current.Resources[MainViewModel.LatinFont];
            Typefaces.Add(MainViewModel.LatinFont, new Typeface(fontFamily, FontStyles.Normal, FontWeights.Normal, FontStretches.Normal));

            QuranReader.InitQuran("quran_hafs_and_english.xml");
        }
    }
}
