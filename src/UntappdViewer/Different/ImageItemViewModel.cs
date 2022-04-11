namespace UntappdViewer.Different
{
    public class ImageItemViewModel
    {
        public string ImagePath { get; private set; }

        public string ImageDescription { get; private set; }

        public ImageItemViewModel(string imagePath, string imageDescription)
        {
            ImagePath = imagePath;
            ImageDescription = imageDescription;
        }
    }
}