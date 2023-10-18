using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace UntappdViewer.UI.Controls.StarRating
{
    /// <summary>
    /// Interaction logic for StarRatingControl.xaml
    /// </summary>
    public partial class StarRatingControl : UserControl
    {
        public static readonly DependencyProperty DependencyProperty = DependencyProperty.Register("Rating", typeof(double), typeof(StarRatingControl), new PropertyMetadata(UpdateRating));

        private static void UpdateRating(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            StarRatingControl listControl = d as StarRatingControl;
            listControl.UpdateRating(e.NewValue == null ? 0 : Convert.ToDouble(e.NewValue));
        }

        public double Rating
        {
            get { return (double)GetValue(DependencyProperty); }
            set { SetValue(DependencyProperty, value); }
        }

        public Color ViewStarsColor
        {
            set
            {
                ((SolidColorBrush)BackgroundPanel.Background).Color = value;
            }
        }

        public double ViewStarsOpacity
        {
            set { ((SolidColorBrush)BackgroundPanel.Background).Opacity = value; }
        }

        public Color BackgroundStarColor
        {
            set
            {
                foreach (StarProgressBar star in GetStars())
                    star.BackgroundStarColor = value;
            }
        }

        public double BackgroundStarOpacity
        {
            set
            {
                foreach (StarProgressBar star in GetStars())
                    star.BackgroundStarOpacity = value;
            }
        }

        public Color ForegroundStarColor
        {
            set
            {
                foreach (StarProgressBar star in GetStars())
                    star.ForegroundStarColor = value;
            }
        }

        public double ForegroundStarOpacity
        {
            set
            {
                foreach (StarProgressBar star in GetStars())
                    star.ForegroundStarOpacity = value;
            }
        }

        public Color BorderStarColor
        {
            set
            {
                foreach (StarProgressBar star in GetStars())
                    star.BorderStarColor = value;
            }
        }

        public double BorderStarOpacity
        {
            set
            {
                foreach (StarProgressBar star in GetStars())
                    star.BorderStarOpacity = value;
            }
        }

        public double StarSize
        {
            set
            {
                ViewStars.Height = value;
                ViewStars.Width = GetStars().Count * value;
            }
        }

        public double BorderStarThickness
        {
            set
            {
                foreach (StarProgressBar star in GetStars())
                    star.BorderStarThickness = value;
            }
        }

        public int StarCount
        {
            set
            {
                StarratingPanel.Children.Clear();
                for (int i = 0; i < value; i++)
                {
                    StarratingPanel.Children.Add(new StarProgressBar());
                    if (i != value - 1)
                        StarratingPanel.Children.Add(new Label { Content = " " });
                }
            }
        }

        public StarRatingControl()
        {
            InitializeComponent();
            BackgroundPanel.Background = new SolidColorBrush(Colors.Transparent);
        }

        private void UpdateRating(double rating)
        {
            int truncateRating = Convert.ToInt32(Math.Truncate(rating));
            List<StarProgressBar> stars = GetStars();
            foreach (StarProgressBar star in stars)
                star.Value = 0;

            for (int i = 0; i < truncateRating; i++)
            {
                if (i < stars.Count)
                    stars[i].Value = 1;
            }
            double module = rating - truncateRating;
            if (truncateRating < stars.Count)
                stars[truncateRating].Value = module;
        }

        private List<StarProgressBar> GetStars()
        {
            List<StarProgressBar> stars = new List<StarProgressBar>();
            foreach (object child in StarratingPanel.Children)
            {
                StarProgressBar star = child as StarProgressBar;
                if (star != null)
                    stars.Add(star);
            }
            return stars;
        }
    }
}