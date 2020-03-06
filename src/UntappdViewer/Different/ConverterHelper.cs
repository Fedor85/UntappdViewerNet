namespace UntappdViewer
{
    public static class ConverterHelper
    {
        public static string GetServingTypeImagePath(string servingTypeName)
        {
            switch (servingTypeName.ToLower())
            {
                case "draft":
                    return @"..\Resources\draft@3x.png";
                case "bottle":
                    return @"..\Resources\bottle@3x.png";
                case "can":
                    return @"..\Resources\can@3x.png";
                case "taster":
                    return @"..\Resources\taster@3x.png";
                case "cask":
                    return @"..\Resources\cask@3x.png";
                case "crowler":
                    return @"..\Resources\crowler.png";
                case "growler":
                    return @"..\Resources\growler.png";
                default:
                    return DefautlValues.EmptyImage;
            }
        }
    }
}
