using System.Linq;

namespace Rukn.ViewModels
{
    public class ArabicAyaTextViewModel : AyaTextViewModel
    {
        private bool includeParenthesis, includeSpaces;
        private string parenthesis;
        private char leftParenthesis, rightParenthesis;

        public static readonly string[] DecoratedParenthesis =
        {
            "\ufd5e\ufd5f",
            "\ufd60\ufd61",
            "\ufd62\ufd63",
            "\ufd64\ufd65",
            "\ufd66\ufd67",
            "\ufd68\ufd69",
            "\ufd6a\ufd6b",
            "\ufd6c\ufd6d",
            "\ufd6e\ufd6f",
            "\ufd70\ufd71",
            "\ufd72\ufd73",
            "\ufd74\ufd75",
            "\ufd76\ufd77",
            "\ufd78\ufd79",
            "\ufd7a\ufd7b",
            "\ufd7c\ufd7d",
        };

        public bool IncludeParenthesis
        {
            get => includeParenthesis;
            set
            {
                if (SetProperty(ref includeParenthesis, value))
                {
                    UpdateText();
                }
            }
        }
        public string Parenthesis
        {
            get => parenthesis;
            set
            {
                if (SetProperty(ref parenthesis, value) && value != null)
                {
                    leftParenthesis = value[0];
                    rightParenthesis = value[1];
                    UpdateText();
                }
            }
        }
        public bool IncludeSpaces
        {
            get => includeSpaces;
            set
            {
                if (SetProperty(ref includeSpaces, value))
                {
                    UpdateText();
                }
            }
        }

        public ArabicAyaTextViewModel(AyaSelectorViewModel ayaSelector) : base(ayaSelector, true)
        {
            Parenthesis = DecoratedParenthesis[0];
        }

        protected override void UpdateText()
        {
            if (includeParenthesis)
            {
                builder.Append(rightParenthesis);
                if (includeSpaces)
                {
                    builder.Append(' ');
                }
            }

            bool needSpace = false;
            foreach (Aya aya in ayaSelector.SelectedAyas.OrderBy(a => a.ID))
            {
                if (needSpace)
                {
                    builder.Append(' ');
                }

                builder.Append(aya.Text);
                if (IncludeNumbers || ayaSelector.EndIndex > ayaSelector.StartIndex)
                {
                    builder
                        // non-breaking-space (nbsp)
                        .Append('\u00a0')
                        // Ideally WPF can handle numbers and convert them to eastern arabic numbers when using
                        // arabic culture such as "ar-sa", but it doesn't seem to work properly, so I use english
                        // culture and convert numbers manually
                        .Append(Helper.ConvertNumbers(aya.ID, '\u0660'));
                }

                needSpace = true;
            }

            if (includeParenthesis)
            {
                if (includeSpaces)
                {
                    builder.Append(' ');
                }
                builder.Append(leftParenthesis);
            }

            if (Reference is not null)
            {
                if (Reference.Mode == ReferenceMode.Inline)
                {
                    builder
                        .Append(' ')
                        .Append(Reference.GenerateText(ayaSelector.SelectedSurah, ayaSelector.StartIndex, ayaSelector.EndIndex));
                }
                else if (Reference.Mode == ReferenceMode.NewLine)
                {
                    builder
                        .AppendLine()
                        .Append(Reference.GenerateText(ayaSelector.SelectedSurah, ayaSelector.StartIndex, ayaSelector.EndIndex));
                }
            }

            Text = builder.ToString();
            builder.Clear();
        }
    }
}
