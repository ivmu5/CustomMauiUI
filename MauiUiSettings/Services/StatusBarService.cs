using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Core.Platform;
using System.ComponentModel;

namespace MauiUiSettings;

public class StatusBarService : IDisposable
{
    public const bool DefaultIsVisible = true;

    private readonly ThemeService _themeService;
    private readonly ColorService _colorService;


    public bool IsVisible
    {
        get;
        set
        {
            field = value;
#if ANDROID
            var window = Platform.CurrentActivity?.Window;

            if (window == null)
                return;

            if (field)
            {
                window.ClearFlags(Android.Views.WindowManagerFlags.Fullscreen);
            }
            else
            {
                window.SetFlags(
                    Android.Views.WindowManagerFlags.Fullscreen,
                    Android.Views.WindowManagerFlags.Fullscreen);
            }
#elif IOS

            //MainThread.BeginInvokeOnMainThread(() =>
            //{
            //    var window = UIApplication.SharedApplication
            //        .ConnectedScenes
            //        .OfType<UIWindowScene>()
            //        .FirstOrDefault()?
            //        .Windows
            //        .FirstOrDefault();

            //    var controller = window?.RootViewController;

            //    controller?.SetNeedsStatusBarAppearanceUpdate();
            //});
    
#endif
        } 
    } = DefaultIsVisible;

    public StatusBarService(
        ThemeService themeService,
        ColorService colorService)
    {
        _themeService = themeService;
        _colorService = colorService;

        SetStatusBarTheme();

        _themeService.PropertyChanged += OnThemeChanged;
        _colorService.PropertyChanged += OnColorChanged;
    }

    private void SetStatusBarTheme()
    {
#if IOS15_0_OR_GREATER || ANDROID23_0_OR_GREATER
        StatusBar.SetStyle(
            _themeService.IsDark
            ? StatusBarStyle.LightContent
            : StatusBarStyle.DarkContent);
#endif
    }

    private void OnThemeChanged(object? sender, PropertyChangedEventArgs e)
    {
        SetStatusBarTheme();
    }

    private void OnColorChanged(object? sender, PropertyChangedEventArgs e)
    {
        SetStatusBarTheme();
    }

    public void Dispose()
    {
        _themeService.PropertyChanged -= OnThemeChanged;
        _colorService.PropertyChanged -= OnColorChanged;
    }
}
