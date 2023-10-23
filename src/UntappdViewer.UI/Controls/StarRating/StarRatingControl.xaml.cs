using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace UntappdViewer.UI.Controls.StarRating
{
    /// <summary>
    /// Interaction logic for StarRatingControl.xaml
    /// </summary>
    public partial class StarRatingControl : UserControl
    {
        public static readonly DependencyProperty RatingProperty = DependencyProperty.Register("Rating", typeof(double?), typeof(StarRatingControl), new PropertyMetadata(InitializeStarRating));

        private static readonly DependencyProperty VisibilityRatingTextProperty = DependencyProperty.Register("VisibilityRatingText", typeof(Visibility), typeof(StarRatingControl));

        public static readonly DependencyProperty StarCountProperty = DependencyProperty.Register("StarCount", typeof(int?), typeof(StarRatingControl), new PropertyMetadata(InitializeStarRating));

        public static readonly DependencyProperty StarSizeProperty = DependencyProperty.Register("StarSize", typeof(double), typeof(StarRatingControl), new PropertyMetadata(20d));

        private static readonly DependencyProperty ViewStarsColorProperty = DependencyProperty.Register("ViewStarsColor", typeof(Brush), typeof(StarRatingControl), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(255, 255, 255))));

        private static readonly DependencyProperty ViewStarsOpacityProperty = DependencyProperty.Register("ViewStarsOpacity", typeof(double), typeof(StarRatingControl), new PropertyMetadata(0d));

        private static readonly DependencyProperty BackgroundStarColorProperty = DependencyProperty.Register("BackgroundStarColor", typeof(Brush), typeof(StarRatingControl), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(255, 255, 255))));

        private static readonly DependencyProperty BackgroundStarOpacityProperty = DependencyProperty.Register("BackgroundStarOpacity", typeof(double), typeof(StarRatingControl), new PropertyMetadata(1d));

        private static readonly DependencyProperty ForegroundStarColorProperty = DependencyProperty.Register("ForegroundStarColor", typeof(Brush), typeof(StarRatingControl), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(255, 193, 0))));

        private static readonly DependencyProperty ForegroundStarOpacityProperty = DependencyProperty.Register("ForegroundStarOpacity", typeof(double), typeof(StarRatingControl), new PropertyMetadata(1d));

        public static readonly DependencyProperty BorderStarThicknessProperty = DependencyProperty.Register("BorderStarThickness", typeof(double), typeof(StarRatingControl), new PropertyMetadata(5d));

        private static readonly DependencyProperty BorderStarColorProperty = DependencyProperty.Register("BorderStarColor", typeof(Brush), typeof(StarRatingControl), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(0, 0, 0))));

        private static readonly DependencyProperty BorderStarOpacityProperty = DependencyProperty.Register("BorderStarOpacity", typeof(double), typeof(StarRatingControl), new PropertyMetadata(1d));

        public double? Rating
        {
            get { return (double?)GetValue(RatingProperty); }
            set { SetValue(RatingProperty, value); }
        }

        public Visibility VisibilityRatingText
        {
            get { return (Visibility)GetValue(VisibilityRatingTextProperty); }
            set { SetValue(VisibilityRatingTextProperty, value); }
        }

        public int? StarCount
        {
            get { return (int?)GetValue(StarCountProperty); }
            set { SetValue(StarCountProperty, value); }
        }

        public double StarSize
        {
            get { return (double)GetValue(StarSizeProperty); }
            set { SetValue(StarSizeProperty, value); }
        }

        public Brush ViewStarsColor
        {
            get { return (Brush)GetValue(ViewStarsColorProperty); }
            set { SetValue(ViewStarsColorProperty, value); }
        }

        public double ViewStarsOpacity
        {
            get { return (double)GetValue(ViewStarsOpacityProperty); }
            set { SetValue(ViewStarsOpacityProperty, value); }
        }

        public Brush BackgroundStarColor
        {
            get { return (Brush)GetValue(BackgroundStarColorProperty); }
            set { SetValue(BackgroundStarColorProperty, value); }
        }

        public double BackgroundStarOpacity
        {
            get { return (double)GetValue(BackgroundStarOpacityProperty); }
            set { SetValue(BackgroundStarOpacityProperty, value); }
        }


        public Brush ForegroundStarColor
        {
            get { return (Brush)GetValue(ForegroundStarColorProperty); }
            set { SetValue(ForegroundStarColorProperty, value); }
        }

        public double ForegroundStarOpacity
        {
            get { return (double)GetValue(ForegroundStarOpacityProperty); }
            set { SetValue(ForegroundStarOpacityProperty, value); }
        }

        public double BorderStarThickness
        {
            get { return (double)GetValue(BorderStarThicknessProperty); }
            set { SetValue(BorderStarThicknessProperty, value); }
        }

        public Brush BorderStarColor
        {
            get { return (Brush)GetValue(BorderStarColorProperty); }
            set { SetValue(BorderStarColorProperty, value); }
        }

        public double BorderStarOpacity
        {
            get { return (double)GetValue(BorderStarOpacityProperty); }
            set { SetValue(BorderStarOpacityProperty, value); }
        }


        public StarRatingControl()
        {
            InitializeComponent();
            ViewStars.SetBinding(Viewbox.HeightProperty, new Binding { Path = new PropertyPath(StarSizeProperty), Source = this });

            BackgroundPanel.SetBinding(DockPanel.BackgroundProperty, new Binding { Path = new PropertyPath(ViewStarsColorProperty), Source = this });
            BackgroundPanel.SetBinding(DockPanel.OpacityProperty, new Binding { Path = new PropertyPath(ViewStarsOpacityProperty), Source = this });

            RatingText.SetBinding(Grid.VisibilityProperty, new Binding { Path = new PropertyPath(VisibilityRatingTextProperty), Source = this });
        }

        private void InitializeStarRating()
        {
            if (!StarCount.HasValue || !Rating.HasValue)
                return;

            SetStarCount(StarCount.Value);
            SetRating(Rating.Value);
        }

        private void SetRating(double rating)
        {
            int truncateRating = Convert.ToInt32(Math.Truncate(rating));
            List<StarProgressBar> starProgress = GetStars();
            foreach (StarProgressBar starProgres in starProgress)
                starProgres.Value = 0;

            for (int i = 0; i < truncateRating; i++)
            {
                if (i < starProgress.Count)
                    starProgress[i].Value = 1;
            }

            double module = rating - truncateRating;
            if (truncateRating < starProgress.Count)
                starProgress[truncateRating].Value = module;
        }

        private void SetStarCount(int count)
        {
            StarratingPanel.Children.Clear();
            for (int i = 0; i < count; i++)
            {
                StarProgressBar starProgress = GetStarProgressBar();
                StarratingPanel.Children.Add(starProgress);

                if (i != 0)
                    UpdateMargin(starProgress);
            }
        }

        private StarProgressBar GetStarProgressBar()
        {
            StarProgressBar starProgress = new StarProgressBar();

            starProgress.SetBinding(StarProgressBar.BackgroundStarColorProperty, new Binding { Path = new PropertyPath(BackgroundStarColorProperty), Source = this });
            starProgress.SetBinding(StarProgressBar.BackgroundStarOpacityProperty, new Binding { Path = new PropertyPath(BackgroundStarOpacityProperty), Source = this });

            starProgress.SetBinding(StarProgressBar.ForegroundStarColorProperty, new Binding { Path = new PropertyPath(ForegroundStarColorProperty), Source = this });
            starProgress.SetBinding(StarProgressBar.ForegroundStarOpacityProperty, new Binding { Path = new PropertyPath(ForegroundStarOpacityProperty), Source = this });

            starProgress.SetBinding(StarProgressBar.BorderStarThicknessProperty, new Binding { Path = new PropertyPath(BorderStarThicknessProperty), Source = this });
            starProgress.SetBinding(StarProgressBar.BorderStarColorProperty, new Binding { Path = new PropertyPath(BorderStarColorProperty), Source = this });
            starProgress.SetBinding(StarProgressBar.BorderStarOpacityProperty, new Binding { Path = new PropertyPath(BorderStarOpacityProperty), Source = this });

            return starProgress;
        }

        private void UpdateMargin(StarProgressBar starProgress)
        {
            Thickness thickness = starProgress.Margin;
            thickness.Left = 10;
            starProgress.Margin = thickness;
        }


        private List<StarProgressBar> GetStars()
        {
            return StarratingPanel.Children.Cast<StarProgressBar>().ToList();
        }

        private static void InitializeStarRating(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            StarRatingControl listControl = d as StarRatingControl;
            listControl.InitializeStarRating();
        }
    }
}