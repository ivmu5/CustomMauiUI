using MauiUiComponents.Resources.Localization;
using MauiUiSettings;
using SQLiteStorage;

namespace MauiUiComponents;

public class ThemeToggle<TView> : ToggleGroup<FlexLayout>
    where TView : View, ITextComponent, new()
{
    private readonly UiServiceStore _uiServices;
    private readonly ComponentStore _componentStore;

    private readonly ToggleGridView<TView> _viewSystemTheme;
    private readonly ToggleGridView<TView> _viewDarkTheme;
    private readonly ToggleGridView<TView> _viewLightTheme;

    private readonly Dictionary<ToggleGridView<TView>, ThemeType> _themeMap = new();



    public ThemeToggle(
        UiServiceStore uiServices,
        ComponentStore componentStore)
        : base(componentStore)
    {
        _uiServices = uiServices;
        _componentStore = componentStore;

        UseCaption = true;
        CaptionLabel.TextBind(
            _componentStore.ResourcesStore.SettingsLocalization,
            nameof(Settings.ThemeSetting));

        _viewSystemTheme = CreateThemeView(nameof(Settings.ThemeSystem), ThemeType.System);
        _viewDarkTheme = CreateThemeView(nameof(Settings.ThemeDark), ThemeType.Dark);
        _viewLightTheme = CreateThemeView(nameof(Settings.ThemeLight), ThemeType.Light);

        SelectItem(GetCurrentThemeView());
        SelectionChanged += ToggleSelectionChanged;

        _uiServices.ThemeService.PropertyChanged += OnThemeChanged;

        ToggleLayout.FlexEqualGrow();
    }

    private ToggleGridView<TView> GetCurrentThemeView()
    {
        return _uiServices.ThemeService.CurrentTheme switch
        {
            ThemeType.System => _viewSystemTheme,
            ThemeType.Dark => _viewDarkTheme,
            ThemeType.Light => _viewLightTheme,
            _ => throw new NotImplementedException()
        };
    }

    private void OnThemeChanged(object? sender, EventArgs e)
    {
        var currentView = GetCurrentThemeView();
        if (currentView == SelectedItem)
            return;

        SelectItem(currentView);
    }

    private ToggleGridView<TView> CreateThemeView(
        string key,
        ThemeType theme)
    {
        var toggleGrid = _componentStore.Custom.ToggleGroup
            .BaseToggleGridTextView<TView>(
                _componentStore.ResourcesStore.SettingsLocalization, 
                key);

        _themeMap[toggleGrid] = theme;
        AddItem(toggleGrid);

        return toggleGrid;
    }

    private void ToggleSelectionChanged(object? sender, ValueChangedEventArgs<ToggleGrid> e)
    {
        if (_themeMap.TryGetValue((ToggleGridView<TView>)e.NewValue!, out var theme))
        {
            _uiServices.ThemeService.SetTheme(theme);
        }
    }

    protected override void OnParentSet()
    {
        base.OnParentSet();

        if (Parent == null)
        {
            _uiServices.ThemeService.PropertyChanged -= OnThemeChanged;
            SelectionChanged -= ToggleSelectionChanged;
        }
    }
}