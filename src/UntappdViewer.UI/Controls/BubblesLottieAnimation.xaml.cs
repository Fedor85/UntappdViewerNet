using LottieSharp.WPF;
using System;
using System.Windows.Controls;
using UntappdViewer.UI.Helpers;

namespace UntappdViewer.UI.Controls
{
    /// <summary>
    /// Interaction logic for BubblesLottieAnimation.xaml
    /// </summary>
    public partial class BubblesLottieAnimation : UserControl
    {
        public BubblesLottieAnimation()
        {
            InitializeComponent();
            SizeChanged += BubblesLottieAnimationSizeChanged;
        }

        private void BubblesLottieAnimationSizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
        {
            double maxSize = Math.Max(ActualHeight, ActualWidth);
            foreach (LottieAnimationView lottieAnimation in UIHelper.FindVisualChildren<LottieAnimationView>(MainGrid))
            {
                lottieAnimation.Width = maxSize;
                lottieAnimation.Height = maxSize;
            }
        }
    }
}
