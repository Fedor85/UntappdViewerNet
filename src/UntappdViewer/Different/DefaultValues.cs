﻿using System.Windows.Media;

namespace UntappdViewer
{
    public static class DefaultValues
    {
        public const string NoImageIconResources = @"pack://application:,,,/UntappdViewer;component/Resources/no-image-icon.png";

        public const string DefaultUrl = "http://schemas.microsoft.com/winfx/2006/xaml";

        public const string UntappdtUrl = "https://untappd.com/";

        public const string DeveloperProfileUrl = UntappdtUrl + @"user/Feador";

        public const string EmptyImage = @"..\Resources\empty_1x1.png";

        public const string DefaultFlag = @"..\Resources\earth_planet.png";

        public const string DefaultBeerLabelName = "badge-beer-default";

        public const string DefaultBreweryLabelName = "badge-brewery-default";

        public const string Separator = " | ";

        public const string DefaultServingType = "NoName";

        public const int ChartRatingScoreYInterval = 250;

        public const int MinCheckinRatingByLikeBeer = 4;

        public static readonly Color MainColorLight = Color.FromRgb(255, 193, 0);

        public static readonly Color MainColorDark = Color.FromRgb(253, 149, 50);

        public static readonly GradientStopCollection MainGradient3 = GetMainGradient3();

        private static GradientStopCollection GetMainGradient3()
        {
            GradientStopCollection gradient = new GradientStopCollection();
            gradient.Add(new GradientStop(Color.FromRgb(255, 255, 255), 0));
            gradient.Add(new GradientStop(MainColorLight, 0.5));
            gradient.Add(new GradientStop(MainColorDark, 1));
            return gradient;
        }
    }
}