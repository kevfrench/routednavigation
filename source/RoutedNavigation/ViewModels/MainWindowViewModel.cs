﻿// This software may be modified and distributed under the terms of the MIT license.  
// See the LICENSE file in the project root for details.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;

using RoutedNavigation.Models;
using MahApps.Metro.IconPacks;
using System.Windows.Media;
using ModernWpf.Controls;

namespace RoutedNavigation.ViewModels
{
    public class MainWindowViewModel : ReactiveObject,
        IScreen, IEnableLogger
    {
        public MainWindowViewModel()
        {
            // Set up some menu items
            // - if this was dynamic we'd use an ObservableCollection
            // - odd Icon construction so as to use my preferred icon pack (https://materialdesignicons.com/)
            //   in my preferred (https://github.com/Kinnara/ModernWpf) NavigationView
            MenuItems = new List<MenuItemModel>
            {
                new MenuItemModel
                {
                    Title = "Home",
                    Icon = new PathIcon
                    {
                        Data = Geometry.Parse(
                            new PackIconMaterial { Kind = PackIconMaterialKind.HomeOutline }.Data
                        )
                    },
                    Url = "home",
                    ViewModel = typeof(HomeViewModel) },
                new MenuItemModel 
                { 
                    Title = "Collections", 
                    Url = "collections",
                    Selectable = false,
                    Icon = new PathIcon
                    {
                        Data = Geometry.Parse(
                            new PackIconMaterial { Kind = PackIconMaterialKind.RhombusSplit }.Data
                        )
                    },
                    ViewModel = typeof(CollectionsViewModel),
                    Children = new List<MenuItemModel>
                    {
                        new MenuItemModel
                        {
                            Title = "Notes",
                            Url = "notes",
                            Icon = new PathIcon
                            {
                                Data = Geometry.Parse(
                                    new PackIconMaterial { Kind = PackIconMaterialKind.NoteMultipleOutline }.Data
                                )
                            },
                            ViewModel= typeof(NotesViewModel)
                        },
                        new MenuItemModel
                        {
                            Title = "Mail",
                            Url = "mail",
                            Selectable = false,
                            Icon = new PathIcon
                            {
                                Data = Geometry.Parse(
                                    new PackIconMaterial { Kind = PackIconMaterialKind.EmailOutline }.Data
                                )
                            },
                            ViewModel= typeof(MailViewModel)
                        },
                    }
                },
            };

            // Keep a flat collection for lookup - probably a better way to do this
            this.allMenuItems = MenuItems
                .Union(MenuItems.SelectMany(item => item.Children))
                .ToList();

            // Whenever the URL changes, set the appropriate page active 
            _ = this.WhenAnyValue(vm => vm.SelectedItem)
                .WhereNotNull()
                .Select(item => Locator.Current.GetService(item.ViewModel) as IRoutableViewModel)
                .ToPropertyEx(this, vm => vm.ActivePage);

            // Navigate when the active page changes
            _ = this.WhenAnyValue(vm => vm.ActivePage)
                .WhereNotNull()
                .Subscribe(page =>
                {
                    // Don't double up the navigation stack
                    if (Router.NavigationStack.LastOrDefault() == page)
                    {
                        return;
                    }
                    // Reset if this is the first page on the stack
                    if (Router.NavigationStack.Count > 0)
                    {
                        Router.Navigate.Execute(page);
                    }
                    else
                    {
                        Router.NavigateAndReset.Execute(page);
                    }
                });

            // Handle the back navigation, we subscribe to the command to keep the UI in sync
            var canGoBack = this.WhenAnyValue(vm => vm.Router.NavigationStack.Count)
                .Select(count => count > 1);
            GoBack = ReactiveCommand.CreateFromObservable
                (() => Router.NavigateBack.Execute(Unit.Default), canGoBack);
            GoBack.Subscribe(vm =>
            {
                SelectedItem = allMenuItems
                    .FirstOrDefault(mi => mi.Url == (vm as PageViewModel)?.UrlPathSegment);
            });

            // Start with the first page
            SelectedItem = MenuItems[0];
        }
        readonly List<MenuItemModel> allMenuItems;

        [ObservableAsProperty]
        public IRoutableViewModel ActivePage { get; }

        [Reactive]
        public MenuItemModel SelectedItem { get; set; }

        public RoutingState Router { get; }
            = new RoutingState();

        public List<MenuItemModel> MenuItems { get; }
            = new List<MenuItemModel>();

        public ReactiveCommand<Unit, IRoutableViewModel> GoBack { get; }
    }
}
