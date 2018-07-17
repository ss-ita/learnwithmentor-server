using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnWithMentorDAL.EF
{
    class ImageReader
    {
        public const char Separator = ' ';
        public string Name { get; set; }
        public string ImageEncoded { get; set; }

        public ImageReader(string name, string imageEncoded)
        {
            this.Name = name;
            this.ImageEncoded = imageEncoded;
        }

        public static ImageReader Parse(string text)
        {
            string[] parts = text.Split(Separator);
            return new ImageReader(parts[0], parts[1]);
        } 
    }
}
