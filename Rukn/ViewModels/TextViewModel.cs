using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace Rukn.ViewModels
{
    public class TextViewModel : ViewModelBase
    {
        private string text, alignment = "Center", fontName, foreground = "#000000";
        private double fontSize, x, y, width, height;

        protected AyaSelectorViewModel ayaSelector;

        private const double MinFontSize = 12; // 12 px

        public double FontSize { get => fontSize; set => SetProperty(ref fontSize, value >= MinFontSize ? value : MinFontSize); }
        public string Alignment { get => alignment; set => SetProperty(ref alignment, value); }
        public string FontName { get => fontName; set => SetProperty(ref fontName, value); }
        public string Foreground { get => foreground; set => SetProperty(ref foreground, value); }
        public string Text { get => text; set => SetProperty(ref text, value); }
        public bool IsRTL { get; init; }
        public double X { get => x; set => SetProperty(ref x, value); }
        public double Y { get => y; set => SetProperty(ref y, value); }
        public double Width { get => width; set => SetProperty(ref width, value); }
        public double Height { get => height; set => SetProperty(ref height, value); }
        public string Label { get; init; }
        public ICommand ResetPosition { get; }

        public TextViewModel() : base() { }
        public TextViewModel(AyaSelectorViewModel ayaSelector) : base()
        {
            this.ayaSelector = ayaSelector;
            ayaSelector.SelectionUpdated += (s, e) => UpdateText();
            ResetPosition = new RelayCommand(o => X = Y = 0);
            UpdateText();
        }

        protected virtual void UpdateText()
        {
            throw new NotImplementedException();
        }
    }
}
