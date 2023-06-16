using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rukn.ViewModels
{
    public class AyaTextViewModel : TextViewModel
    {
        private bool includeNumbers = true;

        protected StringBuilder builder = new();

        public AyaReferenceViewModel Reference { get; }
        public bool IncludeNumbers
        {
            get => includeNumbers;
            set
            {
                if (SetProperty(ref includeNumbers, value))
                {
                    UpdateText();
                }
            }
        }

        public AyaTextViewModel(AyaSelectorViewModel ayaSelector, bool isArabic = false) : base(ayaSelector)
        {
            Reference = new AyaReferenceViewModel() { IsArabic = isArabic };
            Reference.PropertyChanged += (s, e) => UpdateText();
            IsRTL = isArabic;
        }

        protected override void UpdateText()
        {
            bool needSpace = false;
            foreach (Aya aya in ayaSelector.SelectedAyas.OrderBy(a => a.ID))
            {
                if (needSpace)
                {
                    builder.Append(' ');
                }

                builder.Append(aya.EnglishText);
                if (IncludeNumbers || ayaSelector.EndIndex > ayaSelector.StartIndex)
                {
                    builder
                        // non-breaking-space (nbsp)
                        .Append('\u00a0')
                        .Append('(')
                        .Append(aya.ID)
                        .Append(')');
                }

                needSpace = true;
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
