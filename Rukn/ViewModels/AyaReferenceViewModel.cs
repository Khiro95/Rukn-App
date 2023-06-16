using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rukn.ViewModels
{
    [TypeConverter(typeof(Converters.EnumTypeConverter))]
    public enum ReferenceMode
    {
        None,
        Inline,
        NewLine,
        Isolated
    }

    public class AyaReferenceViewModel : ViewModelBase
    {
        private ReferenceMode mode;
        private bool includeBrackets;

        public bool IsArabic { get; init; }
        public ReferenceMode Mode
        {
            get => mode;
            set
            {
                if (SetProperty(ref mode, value))
                {
                    ModeChanged?.Invoke(this, value);
                }
            }
        }
        public bool IncludeBrackets
        {
            get => includeBrackets;
            set
            {
                if (SetProperty(ref includeBrackets, value))
                {
                    IncludeBracketsChanged?.Invoke(this, value);
                }
            }
        }

        public event EventHandler<ReferenceMode> ModeChanged;
        public event EventHandler<bool> IncludeBracketsChanged;

        public string GenerateText(Sura selectedSura, int startIndex, int endIndex)
        {
            if (mode == ReferenceMode.None)
            {
                return "";
            }

            string range;

            if (startIndex == endIndex)
            {
                range = IsArabic ? Helper.ConvertNumbers(startIndex) : startIndex.ToString();
            }
            else
            {
                range = IsArabic ? $"{Helper.ConvertNumbers(startIndex)} - {Helper.ConvertNumbers(endIndex)}" : $"{startIndex} - {endIndex}";
            }

            string result = $"{(IsArabic ? selectedSura.Name : selectedSura.EnglishName)}: {range}";

            if (includeBrackets)
            {
                result = '[' + result + ']';
            }

            return result;
        }
    }
}
