namespace UntappdViewer.UI.Controls.ViewModel
{
    public class RatingImageViewModel: ImageViewModel
    {
        public double? RatingScore { get; set; }

        public bool VisibilityRatingScore { get { return RatingScore.HasValue; } }
    }
}