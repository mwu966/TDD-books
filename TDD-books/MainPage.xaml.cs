using System;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

// 空白ページの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x411 を参照してください

namespace TDD_books
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        BitmapImage[] images = new BitmapImage[8];
        DispatcherTimer timer;
        int counter = 0;
        int speed = 0;
        Rect stickR, boxR;

        public MainPage()
        {
            this.InitializeComponent();

            for (int i = 0; i < 8; i++)
            {
                images[i] = new BitmapImage(
                    new Uri("ms-appx:///Images/stickman"+i+".png"));
            }
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(30);
            timer.Tick += Tick;
            timer.Start();

            DataContext = this;
            stickR = new Rect(20,200,100,150);
            boxR = new Rect(600, 300, 50, 200);

            Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;
        }

        private void CoreWindow_KeyDown(CoreWindow sender, KeyEventArgs args)
        {
            if (args.VirtualKey == Windows.System.VirtualKey.Space && stickR.Y == 200)
            {
                speed = -17;
            }
        }

        private void Tick(object sender, object e)
        {
            counter = (counter + 1)%8;
            Stickman.Source = images[counter];
            
            speed += 1;
            stickR.Y = Math.Min(200, stickR.Y + speed);
            Stickman.SetValue(Canvas.TopProperty,stickR.Y);

            boxR.X = (boxR.X > -100) ? boxR.X -10:600;
            Box.SetValue(Canvas.LeftProperty,boxR.X);

            if (RectHelper.Intersect(boxR,stickR) != Rect.Empty)
            {
                GameOver.Visibility = Visibility.Visible;
                timer.Stop();
            }
        }
    }
}
