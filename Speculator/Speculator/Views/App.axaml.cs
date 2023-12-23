// Anyone is free to copy, modify, use, compile, or distribute this software,
// either in source code form or as a compiled binary, for any non-commercial
// purpose.
//
// If modified, please retain this copyright header, and consider telling us
// about your changes.  We're always glad to see how people use our code!
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND.
// We do not accept any liability for damage caused by executing
// or modifying this code.

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Speculator.ViewModels;

namespace Speculator.Views;

// ReSharper disable once PartialTypeWithSinglePart
public partial class App : Application
{
    public App()
    {
        DataContext = new AppViewModel();
    }
    
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var viewModel = new MainWindowViewModel();
            desktop.MainWindow = new MainWindow
            {
                DataContext = viewModel
            };

            desktop.MainWindow.Activated += (sender, args) => viewModel.Speccy.PortHandler.HandleKeyEvents = true;
            desktop.MainWindow.Deactivated += (sender, args) => viewModel.Speccy.PortHandler.HandleKeyEvents = false;

            if (!Design.IsDesignMode)
            {
                viewModel.Speccy.PortHandler.StartKeyboardHook();
                viewModel.Speccy.PowerOnAsync();
            }
        }

        base.OnFrameworkInitializationCompleted();
    }
}