using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Rukn.ViewModels;

namespace Rukn.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private OpenFileDialog ofd;
        private SaveFileDialog sfd;

        private static BrushConverter brushConverter = new BrushConverter();

        private MainViewModel ViewModel => DataContext as MainViewModel;

        static MainWindow()
        {
            string lang = Properties.Settings.Default.DisplayLanguage;
            if (string.IsNullOrEmpty(lang))
            {
                lang = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
            }
            Rukn.Resources.Culture = new System.Globalization.CultureInfo(lang);
            LanguageProperty.OverrideMetadata(typeof(MainWindow), new FrameworkPropertyMetadata(System.Windows.Markup.XmlLanguage.GetLanguage(lang), OnUILanguageChanged));
        }

        private static void OnUILanguageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)d;
            if (!mainWindow.IsLoaded)
            {
                return;
            }
            string lang = e.NewValue as string ?? (e.NewValue as System.Windows.Markup.XmlLanguage)?.IetfLanguageTag;
            if (string.IsNullOrEmpty(lang))
            {
                return;
            }
            Properties.Settings.Default.DisplayLanguage = lang;
            Properties.Settings.Default.Save();
            var culture = new System.Globalization.CultureInfo(lang);
            Rukn.Resources.Culture = culture;

            mainWindow.Hide();
            new MainWindow()
            {
                DataContext = mainWindow.ViewModel,
                Language = System.Windows.Markup.XmlLanguage.GetLanguage(lang),
                WindowState = mainWindow.WindowState,
                WindowStartupLocation = WindowStartupLocation.Manual,
                Left = mainWindow.Left,
                Top = mainWindow.Top
            }.Show();
            mainWindow.Close();
        }

        public MainWindow()
        {
            InitializeComponent();

            MainGrid.FlowDirection = Rukn.Resources.Culture.TextInfo.IsRightToLeft ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;

            if (ViewModel?.AyaSelector is not null)
            {
                ViewModel.AyaSelector.SelectedSurah = QuranReader.Quran.Suras[0];
                ViewModel.AyaSelector.StartIndex = 1;
            }
        }

        private void LoadClick(object sender, RoutedEventArgs e)
        {
            if (ViewModel is null)
            {
                return;
            }
            if (ofd is null)
            {
                ofd = new OpenFileDialog();
                ofd.Filter = "Image Files(*.png;*.jpg;*.jpeg;*.bmp)|*.png;*.jpg;*.jpeg;*.bmp;";
            }
            if (ofd.ShowDialog() == true)
            {
                Uri uri = new Uri(ofd.FileName);
                var image = new BitmapImage(uri);
                if (image.Height < ViewModel.MinHeight || image.Width < ViewModel.MinWidth)
                {
                    MessageBox.Show(string.Format(Rukn.Resources.Error_InvalidImageSize_Body, ViewModel.MinWidth, ViewModel.MinHeight), Rukn.Resources.Error_InvalidImageSize_Title, MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                ViewModel.Width = (int)image.Width;
                ViewModel.Height = (int)image.Height;
                ViewModel.ImagePath = uri;
            }
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            if (sfd is null)
            {
                sfd = new SaveFileDialog();
                sfd.Filter = "Portable Network Graphic (*.png)|*.png";
            }
            sfd.FileName = null;
            if (sfd.ShowDialog() == true)
            {
                try
                {
                    var image = RenderResult();
                    using (var fileStream = new FileStream(sfd.FileName, FileMode.Create))
                    {
                        BitmapEncoder encoder = new PngBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create(image));
                        encoder.Save(fileStream);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(Rukn.Resources.Error + "\n" + ex.Message, Rukn.Resources.Save, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void CopyClick(object sender, RoutedEventArgs e)
        {
            var image = RenderResult();
            Clipboard.SetImage(image);
        }

        private BitmapSource RenderResult()
        {
            (int width, int height) size = (ViewModel.Width, ViewModel.Height);

            // just in case
            if (size == (0, 0))
            {
                return null;
            }

            DrawingVisual drawingVisual = new DrawingVisual();
            using (DrawingContext dc = drawingVisual.RenderOpen())
            {
                // draw opaque background
                dc.DrawRectangle((SolidColorBrush)brushConverter.ConvertFromString(ViewModel.BackgroundColor), null, new Rect(0, 0, size.width, size.height));

                // if a background image is used then draw it
                if (ViewModel.ImagePath is not null)
                {
                    var image = new BitmapImage(ViewModel.ImagePath);
                    dc.DrawImage(image, new Rect(0, 0, image.Width, image.Height));
                }

                foreach (var txt in ViewModel.Texts)
                {
                    if (!string.IsNullOrEmpty(txt.Text))
                    {
                        DrawText(dc, txt, App.Typefaces[txt.FontName]);
                    }
                }
            }
            RenderTargetBitmap bitmap = new RenderTargetBitmap(size.width, size.height, 96, 96, PixelFormats.Default);
            bitmap.Render(drawingVisual);
            return bitmap;
        }

        private static void DrawText(DrawingContext dc, TextViewModel textVM, Typeface typeface)
        {
            if (textVM is null || textVM.Width < 1 || textVM.Height < 1)
            {
                return;
            }

            SolidColorBrush brush = (SolidColorBrush)brushConverter.ConvertFromString(textVM.Foreground);
            Rect txtRect = new Rect(textVM.X, textVM.Y, textVM.Width, textVM.Height);

            FormattedText formattedText = new FormattedText(
                textVM.Text,
                System.Globalization.CultureInfo.GetCultureInfo("en"),
                textVM.IsRTL ? FlowDirection.RightToLeft : FlowDirection.LeftToRight,
                typeface,
                textVM.FontSize,
                brush, /*null, TextFormattingMode.Display,*/
                96);

            formattedText.MaxTextWidth = textVM.Width;
            // ¯\_(ツ)_/¯
            formattedText.TextAlignment = Controls.TextBlockUtils.GetAdjustedTextAlignment(textVM.IsRTL, textVM.Alignment);
            formattedText.Trimming = TextTrimming.None;
            dc.PushClip(new RectangleGeometry(txtRect));
            var geo = formattedText.BuildGeometry(txtRect.Location);
            dc.DrawGeometry(brush, null, geo);
            //dc.DrawText(formattedText, textVM.Rect.Location);
            dc.Pop();
        }
    }
}
