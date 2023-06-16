using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Rukn
{
    public static class QuranReader
    {
        public static Quran Quran { get; private set; }
        
        public static void InitQuran(string xmlPath)
        {
            XmlSerializer serializer = new(typeof(Quran));
            Quran = (Quran)serializer.Deserialize(File.OpenRead(xmlPath));
        }
    }

    public class Quran
    {
        public List<Sura> Suras { get; set; }
    }

    public class Sura
    {
        [XmlAttribute]
        public byte ID { get; set; }
        [XmlAttribute]
        public string Name { get; set; }
        [XmlAttribute]
        public string EnglishName { get; set; }
        public List<Aya> Ayas { get; set; }
    }

    public class Aya
    {
        [XmlAttribute]
        public ushort ID { get; set; }
        public string Text { get; set; }
        public string EnglishText { get; set; }
    }
}
