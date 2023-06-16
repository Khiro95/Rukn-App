using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rukn.ViewModels
{
    public class AyaSelectorViewModel : ViewModelBase
    {
        private Sura selectedSurah;
        private ushort startIndex, endIndex;
        private bool isRange;

        public bool IsRange
        {
            get => isRange;
            set
            {
                if (SetProperty(ref isRange, value))
                {
                    if (!value)
                    {
                        EndIndex = StartIndex;
                    }
                }
            }
        }
        public Sura SelectedSurah
        {
            get => selectedSurah;
            set
            {
                if (SetProperty(ref selectedSurah, value))
                {
                    StartIndex = 0; // Combobox doesn't update selected item if previous value = new value, so I set it to 0 to force refresh
                    EndIndex = 0; // Combobox doesn't update selected item if previous value = new value, so I set it to 0 to force refresh
                    StartIndex = 1; // the setter of StartIndex will update EndIndex as well
                }
            }
        }
        public ushort StartIndex
        {
            get => startIndex;
            set
            {
                if (SetProperty(ref startIndex, value) && value > 0 && value <= selectedSurah.Ayas.Count)
                {
                    if (!isRange || value > endIndex)
                    {
                        EndIndex = value;
                    }
                    else
                    {
                        SelectionUpdated?.Invoke(this, (selectedSurah, SelectedAyas));
                    }
                }
            }
        }
        public ushort EndIndex
        {
            get => endIndex;
            set
            {
                if (SetProperty(ref endIndex, value) && value > 0 && value <= selectedSurah.Ayas.Count)
                {
                    if (isRange && value < startIndex)
                    {
                        StartIndex = value;
                    }
                    else
                    {
                        SelectionUpdated?.Invoke(this, (selectedSurah, SelectedAyas));
                    }
                }
            }
        }
        public IEnumerable<Aya> SelectedAyas => selectedSurah.Ayas.Skip(startIndex - 1).Take(endIndex - startIndex + 1);

        public event EventHandler<(Sura, IEnumerable<Aya>)> SelectionUpdated;
    }
}
