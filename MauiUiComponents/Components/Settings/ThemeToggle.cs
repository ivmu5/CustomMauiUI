using MauiUiComponents.Resources.Localization;
using MauiUiSettings;

namespace MauiUiComponents;

public class ThemeToggle<TView> : ToggleGroup<FlexLayout>
    where TView : View, ITextComponent, new()
{
    private readonly UiServiceStore _uiServices;
    private readonly ComponentStore _componentStore;

    private readonly ToggleGrid _viewSystemTheme;
    private readonly ToggleGrid _viewDarkTheme;
    private readonly ToggleGrid _viewLightTheme;

    private readonly Dictionary<ToggleGrid, ThemeType> _themeMap = new();



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

        _viewSystemTheme = CreateThemeView(ThemeType.System, nameof(Settings.ThemeSystem));
        _viewDarkTheme = CreateThemeView(ThemeType.Dark, nameof(Settings.ThemeDark));
        _viewLightTheme = CreateThemeView(ThemeType.Light, nameof(Settings.ThemeLight));

        SelectItem(GetCurrentThemeView());

        _uiServices.ThemeService.PropertyChanged += OnThemeChanged;

        ToggleLayout.FlexEqualGrow();
    }

    private ToggleGrid GetCurrentThemeView()
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

    private ToggleGrid CreateThemeView(
        ThemeType theme,
        string localizationKey)
    {
        var toggleGrid = _componentStore.Custom.ToggleGroup
            .BaseTextToggleGrid(
                _componentStore.ResourcesStore.SettingsLocalization,
                localizationKey,
                _componentStore.Custom.ToggleGroup.ToggleBackgroundColorAction<TView>(),
                new(
                    v => _uiServices.ThemeService.SetTheme(theme),
                    v => { }));

        _themeMap[toggleGrid] = theme;
        AddItem(toggleGrid);

        return toggleGrid;
    }

    protected override void OnParentSet()
    {
        base.OnParentSet();

        if (Parent == null)
        {
            _uiServices.ThemeService.PropertyChanged -= OnThemeChanged;
        }
    }
}