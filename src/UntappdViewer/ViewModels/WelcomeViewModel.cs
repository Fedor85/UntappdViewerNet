﻿using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Prism.Commands;
using Prism.Events;
using Prism.Modularity;
using Prism.Regions;
using UntappdViewer.Domain;
using UntappdViewer.Events;
using UntappdViewer.Helpers;
using UntappdViewer.Infrastructure;
using UntappdViewer.Interfaces.Services;
using UntappdViewer.Modules;
using UntappdViewer.Views;

namespace UntappdViewer.ViewModels
{
    public class WelcomeViewModel: RegionManagerBaseModel
    {
        private IUntappdService untappdService;

        private IInteractionRequestService interactionRequestService;

        private ISettingService settingService;

        private IModuleManager moduleManager;

        private IEventAggregator eventAggregator;

        public ICommand OpenFileCommand { get; }

        public ICommand DropFileCommand { get; }

        public ICommand CreateProjectCommand { get; }

        public string EmailUrl
        {
            get { return "mailto:" + Properties.Resources.Email; }
        }

        public string MyProfileUrl
        {
            get { return Properties.Resources.DeveloperProfileUrl; }
        }

        public WelcomeViewModel(IUntappdService untappdService, IInteractionRequestService interactionRequestService,
                                                                ISettingService settingService,
                                                                IModuleManager moduleManager,
                                                                IRegionManager regionManager,
                                                                IEventAggregator eventAggregator) : base(regionManager)
        {
            this.untappdService = untappdService;
            this.interactionRequestService = interactionRequestService;
            this.settingService = settingService;
            this.moduleManager = moduleManager;
            this.settingService = settingService;
            this.eventAggregator = eventAggregator;

            OpenFileCommand = new DelegateCommand(OpenFile);
            DropFileCommand = new DelegateCommand<DragEventArgs>(DropFile);
            CreateProjectCommand = new DelegateCommand(CreateProject);
        }

        protected override void Activate()
        {
            base.Activate();
            untappdService.CleanUpUntappd();
            eventAggregator.GetEvent<OpenFileEvent>().Subscribe(RunUntappd);
            moduleManager.LoadModule(typeof(RecentFilesModule).Name);
            ActivateView(RegionNames.RecentFilesRegion, typeof(RecentFiles));
        }

        protected override void DeActivate()
        {
            base.DeActivate();
            eventAggregator.GetEvent<OpenFileEvent>().Unsubscribe(RunUntappd);
            DeActivateAllViews(RegionNames.RecentFilesRegion);
        }

        private void OpenFile()
        {
            string lastOpenFilePath = FileHelper.GetFirstFileItemPath(settingService.GetRecentFilePaths());
            string filePath = interactionRequestService.OpenFile(String.IsNullOrEmpty(lastOpenFilePath) ? String.Empty : Path.GetDirectoryName(lastOpenFilePath), Extensions.GetSupportExtensions());
            if (String.IsNullOrEmpty(filePath))
                return;

            RunUntappd(filePath);
        }

        private void DropFile(DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
                return;

            string[] filesPaths = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (filesPaths.Length == 0)
                return;

            RunUntappd(filesPaths[0]);
        }

        private void CreateProject()
        {
            string untappdUserName = interactionRequestService.AskReplaceText(Properties.Resources.UntappdUserNameCaption, String.Empty);
            untappdService.Create(untappdUserName);
            moduleManager.LoadModule(typeof(MainModule).Name);
            ActivateView(RegionNames.RootRegion, typeof(Main));
            interactionRequestService.ClearMessageOnStatusBar();
        }

        private void RunUntappd(string filePath)
        {
            FileStatus fileStatus = FileHelper.Check(filePath, Extensions.GetSupportExtensions());
            if (!EnumsHelper.IsValidFileStatus(fileStatus))
            {
                interactionRequestService.ShowMessage(Properties.Resources.Warning, CommunicationHelper.GetFileStatusMessage(fileStatus, filePath));
                return;
            }

            try
            {
                string untappdUserName = String.Empty;
                if (FileHelper.GetExtensionWihtoutPoint(filePath).Equals(Extensions.CSV))
                    untappdUserName = interactionRequestService.AskReplaceText(Properties.Resources.UntappdUserNameCaption, String.Empty);

                untappdService.Initialize(filePath, untappdUserName);
            }
            catch (ArgumentException ex)
            {
                interactionRequestService.ShowError(Properties.Resources.Error, ex.Message);
                return;
            }

            settingService.SetRecentFilePaths(FileHelper.AddFilePath(settingService.GetRecentFilePaths(), filePath, settingService.GetMaxRecentFilePaths()));
            moduleManager.LoadModule(typeof(MainModule).Name);
            ActivateView(RegionNames.RootRegion, typeof(Main));
            interactionRequestService.ShowMessageOnStatusBar(filePath);
        }
    }
}