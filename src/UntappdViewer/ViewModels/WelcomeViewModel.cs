﻿using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Prism.Commands;
using UntappdViewer.Interfaces.Services;
using UntappdViewer.Properties;

namespace UntappdViewer.ViewModels
{
    public class WelcomeViewModel
    {
        private IDialogService dialogService;

        public ICommand OpenFileCommand { get; }

        public ICommand DropFileCommand { get; }

        public WelcomeViewModel(IDialogService dialogService)
        {
            this.dialogService = dialogService;
            OpenFileCommand = new DelegateCommand(OpenFile);
            DropFileCommand = new DelegateCommand<DragEventArgs>(DropFile);
        }

        private void OpenFile()
        {
            string saveOpenFilePath = Settings.Default.LastOpenedFilePath;
            string openFilePath = dialogService.OpenFile(String.IsNullOrEmpty(saveOpenFilePath) ? String.Empty : Path.GetDirectoryName(saveOpenFilePath), Extensions.GetExtensions());
            if (String.IsNullOrEmpty(openFilePath))
                return;

            SaveSettings(openFilePath);
        }

        private void DropFile(DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
                return;

            string[] filesPaths = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (filesPaths.Length == 0)
                return;

            string openFilePath = filesPaths[0];
            if (String.IsNullOrEmpty(openFilePath))
                return;

            if (!Extensions.GetExtensions().Contains(GetExtensionWihtoutPoint(openFilePath)))
                return;

            SaveSettings(openFilePath);
        }

        public static string GetExtensionWihtoutPoint(string path)
        {
            string extension = Path.GetExtension(path);
            if (String.IsNullOrEmpty(extension))
                return extension;

            return extension.Replace(".", String.Empty).Trim().ToLower();
        }

        private void SaveSettings(string filePath)
        {
            Settings.Default.LastOpenedFilePath = filePath;
            Settings.Default.Save();
        }
    }
}