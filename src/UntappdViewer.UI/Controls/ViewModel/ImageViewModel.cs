using System;

namespace UntappdViewer.UI.Controls.ViewModel
{
    public class ImageViewModel: BaseImageViewModel
    {
        public string Caption { get; set; }

        public bool VisibilityCaption { get { return !String.IsNullOrEmpty(Caption); } }

        public string Signature { get; set; }

        public bool VisibilitySignature { get { return !String.IsNullOrEmpty(Signature); } }

        public string ToolTip { get; set; }
    }
}