using System;
using System.Windows.Media.Imaging;

namespace Heroes.Icons.Models
{
    public class IconBase
    {
        protected BitmapImage CreateBitmapImage(Uri uri)
        {
            BitmapImage image = new BitmapImage(uri);
            image.Freeze();

            return image;
        }
    }
}
