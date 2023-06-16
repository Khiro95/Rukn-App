using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rukn.ViewModels
{
    public class AyaReferenceTextViewModel : TextViewModel
    {
        public AyaReferenceViewModel Reference { get; }

        public AyaReferenceTextViewModel(AyaReferenceViewModel ayaReference, AyaSelectorViewModel ayaSelector) : base(ayaSelector)
        {
            Reference = ayaReference;
            Reference.PropertyChanged += (s, e) => UpdateText();
            IsRTL = Reference.IsArabic;
            UpdateText();
        }

        protected override void UpdateText()
        {
            if (Reference?.Mode == ReferenceMode.Isolated)
            {
                Text = Reference.GenerateText(ayaSelector.SelectedSurah, ayaSelector.StartIndex, ayaSelector.EndIndex);
            }
            else
            {
                Text = "";
            }
        }
    }
}
