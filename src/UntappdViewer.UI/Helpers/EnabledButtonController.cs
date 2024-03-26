using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using Microsoft.Xaml.Behaviors.Core;
using UntappdViewer.UI.Interfaces;

namespace UntappdViewer.UI.Helpers
{
    public class EnabledButtonController(Button button)
    {
        private readonly List<TextBox> textBoxControls = new();

        private readonly List<ITextBox> iTextBoxControls = new();

        public void RegisterTextControl(TextBox textControl)
        {
            textControl.TextChanged += TextChanged;
            textBoxControls.Add(textControl);
        }

        public void RegisterTextControl(ITextBox textControl)
        {
            textControl.TextChanged = new ActionCommand(TextChanged);
            iTextBoxControls.Add(textControl);
        }

        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            TextChanged();
        }

        private void TextChanged()
        {
            button.IsEnabled = textBoxControls.All(item => !String.IsNullOrEmpty(item.Text)) && 
                               iTextBoxControls.All(item => !String.IsNullOrEmpty(item.Text));
        }
    }
}