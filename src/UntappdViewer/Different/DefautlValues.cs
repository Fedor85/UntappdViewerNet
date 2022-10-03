namespace UntappdViewer
{
    public static class DefautlValues
    {
        public const string NoImageIconResources = @"pack://application:,,,/UntappdViewer;component/Resources/no-image-icon.png";

        public const string DefaultUrl = "http://schemas.microsoft.com/winfx/2006/xaml";

        public const string UntappdtUrl = "https://untappd.com/";

        public const string DeveloperProfileUrl = UntappdtUrl + @"user/Feador";

        public const string EmptyImage = @"..\Resources\empty_1x1.png";

        public const string DefaultBeerLabelName = "badge-beer-default";

        public const string DefaultBreweryLabelName = "badge-brewery-default";

        public const string Separator = " | ";

        public static readonly string[] SeparatorsBeerTypeName = {"/", "-"};

        public const int ChartRatingScoreYInterval = 250;
    }
}