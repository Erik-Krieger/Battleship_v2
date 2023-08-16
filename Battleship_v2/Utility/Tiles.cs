using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Battleship_v2.Utility
{
    public static class Tiles
    {
        public static ImageSource Water { get; } = new BitmapImage(new Uri( "C:\\Users\\erik.krieger\\OneDrive - LPKF\\Code\\Playground\\Battleship_v2\\Battleship_v2\\Resources\\dev_art\\blue.png" ) );
        /*public static Image Miss { get; } = Image.FromFile( "Resources/dev_art/light_gray.png" );
        public static Image Hit { get; } = Image.FromFile( "Resources/dev_art/red.png" );
        public static Image Carrier { get; } = Image.FromFile( "Resources/dev_art/gray_c.png" );
        public static Image Battleship { get; } = Image.FromFile( "Resources/dev_art/gray_b.png" );
        public static Image Submarine { get; } = Image.FromFile( "Resources/dev_art/gray_s.png" );
        public static Image Destroyer { get; } = Image.FromFile( "Resources/dev_art/gray_d.png" );
        public static Image PatrolBoat { get; } = Image.FromFile( "Resources/dev_art/gray_p.png" );*/

        public static BitmapImage BitmapToImageSource(Bitmap theBitmap)
        {
            using ( MemoryStream memory = new MemoryStream() )
            {
                theBitmap.Save( memory, System.Drawing.Imaging.ImageFormat.Bmp );
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }
    }
}
