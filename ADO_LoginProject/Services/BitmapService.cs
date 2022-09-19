using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using ADO_LoginProject.MVVM.ViewModels;

namespace ADO_LoginProject.Services
{
    public abstract class BitmapService
    {
        public static BitmapImage GetBitmapImageFromUrl(string path)
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(path, UriKind.Absolute);
            bitmap.EndInit();

            return bitmap;
        }

    }
}
