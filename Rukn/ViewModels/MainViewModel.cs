using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Rukn.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private bool includeArabic = true, includeTranslation;
        private int width, height;
        private ArabicAyaTextViewModel verses;
        private AyaTextViewModel translation;
        private AyaReferenceTextViewModel reference, en_reference;
        private string backgroundColor = "#FFFFFF";
        private Uri imagePath;

        public const string ArabicFont = "UthmanicHafs";
        public const string LatinFont = "Bitter";

        public AyaSelectorViewModel AyaSelector { get; } = new();
        public ArabicAyaTextViewModel Verses { get => verses; private set => SetProperty(ref verses, value); }
        public AyaTextViewModel Translation { get => translation; private set => SetProperty(ref translation, value); }
        public AyaReferenceTextViewModel Reference { get => reference; private set => SetProperty(ref reference, value); }
        public AyaReferenceTextViewModel EnglishReference { get => en_reference; private set => SetProperty(ref en_reference, value); }
        public string BackgroundColor { get => backgroundColor; set => SetProperty(ref backgroundColor, value); }
        public bool IncludeTranslation
        {
            get => includeTranslation;
            set
            {
                if (SetProperty(ref includeTranslation, value))
                {
                    if (value)
                    {
                        if (translation is null)
                        {
                            Translation = new AyaTextViewModel(AyaSelector)
                            {
                                Label = "EnglishTranslation",
                                FontName = LatinFont,
                                FontSize = 16,
                                X = 10,
                                Y = 10,
                                Width = this.Width - 20,
                                Height = this.Height - 20
                            };
                            translation.Reference.ModeChanged += EnglishReference_ModeChanged;
                        }
                        Texts.Add(translation);
                    }
                    else
                    {
                        if (en_reference?.Reference?.Mode == ReferenceMode.Isolated)
                        {
                            en_reference.Reference.Mode = ReferenceMode.None;
                        }
                        Texts.Remove(translation);
                    }
                }
            }
        }

        public bool IncludeArabic
        {
            get => includeArabic;
            set
            {
                if (SetProperty(ref includeArabic, value))
                {
                    if (value)
                    {
                        Texts.Add(verses);
                    }
                    else
                    {
                        if (reference?.Reference?.Mode == ReferenceMode.Isolated)
                        {
                            reference.Reference.Mode = ReferenceMode.None;
                        }
                        Texts.Remove(verses);
                    }
                }
            }
        }

        public Uri ImagePath
        {
            get => imagePath;
            set
            {
                if (SetProperty(ref imagePath, value))
                {
                    OnPropertyChanged(nameof(IsImageAbsent));
                }
            }
        }
        public bool IsImageAbsent => ImagePath is null;
        public int Width { get => width; set => SetProperty(ref width, value >= MinWidth ? value : MinWidth); }
        public int Height { get => height; set => SetProperty(ref height, value >= MinHeight ? value : MinHeight); }
        public int MinWidth { get; set; }
        public int MinHeight { get; set; }
        public ObservableCollection<TextViewModel> Texts { get; } = new ObservableCollection<TextViewModel>();
        public ICommand RemoveImage { get; }

        public MainViewModel()
        {
            RemoveImage = new RelayCommand(o => ImagePath = null, o => !IsImageAbsent);
            width = 400;
            height = 300;
            AyaSelector.SelectionUpdated += (s, e) =>
            {
                if (verses is null)
                {
                    Verses = new ArabicAyaTextViewModel(AyaSelector)
                    {
                        Label = "ArabicText",
                        FontName = ArabicFont,
                        FontSize = 24,
                        Foreground = "#9C3F16",
                        X = 10,
                        Y = 10,
                        Width = this.Width - 20,
                        Height = this.Height - 20
                    };
                    Texts.Add(verses);
                    verses.Reference.ModeChanged += ArabicReference_ModeChanged;
                }
            };
        }

        private void ArabicReference_ModeChanged(object sender, ReferenceMode mode)
        {
            if (mode == ReferenceMode.Isolated)
            {
                if (reference is null)
                {
                    Reference = new AyaReferenceTextViewModel(verses.Reference, AyaSelector)
                    {
                        Label = "ArabicReference",
                        FontName = ArabicFont,
                        FontSize = 24,
                        X = 10,
                        Y = 10,
                        Width = this.Width - 20,
                        Height = this.Height - 20
                    };
                }
                Texts.Add(reference);
            }
            else
            {
                Texts.Remove(reference);
            }
        }

        private void EnglishReference_ModeChanged(object sender, ReferenceMode mode)
        {
            if (mode == ReferenceMode.Isolated)
            {
                if (en_reference is null)
                {
                    EnglishReference = new AyaReferenceTextViewModel(translation.Reference, AyaSelector)
                    {
                        Label = "EnglishReference",
                        FontName = LatinFont,
                        FontSize = 16,
                        X = 10,
                        Y = 10,
                        Width = this.Width - 20,
                        Height = this.Height - 20
                    };
                }
                Texts.Add(en_reference);
            }
            else
            {
                Texts.Remove(en_reference);
            }
        }
    }
}
