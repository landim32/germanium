using SkiaSharp;

namespace Germanium;

public record TokenColor(SKColor Color, bool Bold = false, bool Italic = false);

public class Theme
{
    public string Name { get; init; } = "";
    public SKColor Background { get; init; }
    public SKColor WindowBackground { get; init; }
    public SKColor TitleBarColor { get; init; }
    public SKColor LineNumberColor { get; init; }
    public SKColor DefaultText { get; init; }
    public TokenColor Keyword { get; init; } = new(SKColors.White);
    public TokenColor String { get; init; } = new(SKColors.White);
    public TokenColor Comment { get; init; } = new(SKColors.White);
    public TokenColor Type { get; init; } = new(SKColors.White);
    public TokenColor Number { get; init; } = new(SKColors.White);
    public TokenColor Method { get; init; } = new(SKColors.White);
    public TokenColor Operator { get; init; } = new(SKColors.White);
    public TokenColor Punctuation { get; init; } = new(SKColors.White);

    public static Theme Dracula => new()
    {
        Name = "Dracula",
        Background = SKColor.Parse("#282a36"),
        WindowBackground = SKColor.Parse("#282a36"),
        TitleBarColor = SKColor.Parse("#21222c"),
        LineNumberColor = SKColor.Parse("#6272a4"),
        DefaultText = SKColor.Parse("#f8f8f2"),
        Keyword = new(SKColor.Parse("#ff79c6"), Bold: true),
        String = new(SKColor.Parse("#f1fa8c")),
        Comment = new(SKColor.Parse("#6272a4"), Italic: true),
        Type = new(SKColor.Parse("#8be9fd"), Italic: true),
        Number = new(SKColor.Parse("#bd93f9")),
        Method = new(SKColor.Parse("#50fa7b")),
        Operator = new(SKColor.Parse("#ff79c6")),
        Punctuation = new(SKColor.Parse("#f8f8f2")),
    };

    public static Theme Monokai => new()
    {
        Name = "Monokai",
        Background = SKColor.Parse("#272822"),
        WindowBackground = SKColor.Parse("#272822"),
        TitleBarColor = SKColor.Parse("#1e1f1c"),
        LineNumberColor = SKColor.Parse("#90908a"),
        DefaultText = SKColor.Parse("#f8f8f2"),
        Keyword = new(SKColor.Parse("#f92672")),
        String = new(SKColor.Parse("#e6db74")),
        Comment = new(SKColor.Parse("#75715e"), Italic: true),
        Type = new(SKColor.Parse("#66d9ef"), Italic: true),
        Number = new(SKColor.Parse("#ae81ff")),
        Method = new(SKColor.Parse("#a6e22e")),
        Operator = new(SKColor.Parse("#f92672")),
        Punctuation = new(SKColor.Parse("#f8f8f2")),
    };

    public static Theme OneDark => new()
    {
        Name = "OneDark",
        Background = SKColor.Parse("#282c34"),
        WindowBackground = SKColor.Parse("#282c34"),
        TitleBarColor = SKColor.Parse("#21252b"),
        LineNumberColor = SKColor.Parse("#495162"),
        DefaultText = SKColor.Parse("#abb2bf"),
        Keyword = new(SKColor.Parse("#c678dd")),
        String = new(SKColor.Parse("#98c379")),
        Comment = new(SKColor.Parse("#5c6370"), Italic: true),
        Type = new(SKColor.Parse("#e5c07b")),
        Number = new(SKColor.Parse("#d19a66")),
        Method = new(SKColor.Parse("#61afef")),
        Operator = new(SKColor.Parse("#56b6c2")),
        Punctuation = new(SKColor.Parse("#abb2bf")),
    };

    public static Theme Nord => new()
    {
        Name = "Nord",
        Background = SKColor.Parse("#2e3440"),
        WindowBackground = SKColor.Parse("#2e3440"),
        TitleBarColor = SKColor.Parse("#272c36"),
        LineNumberColor = SKColor.Parse("#4c566a"),
        DefaultText = SKColor.Parse("#d8dee9"),
        Keyword = new(SKColor.Parse("#81a1c1"), Bold: true),
        String = new(SKColor.Parse("#a3be8c")),
        Comment = new(SKColor.Parse("#616e88"), Italic: true),
        Type = new(SKColor.Parse("#8fbcbb")),
        Number = new(SKColor.Parse("#b48ead")),
        Method = new(SKColor.Parse("#88c0d0")),
        Operator = new(SKColor.Parse("#81a1c1")),
        Punctuation = new(SKColor.Parse("#eceff4")),
    };

    public static Theme SolarizedDark => new()
    {
        Name = "SolarizedDark",
        Background = SKColor.Parse("#002b36"),
        WindowBackground = SKColor.Parse("#002b36"),
        TitleBarColor = SKColor.Parse("#00252f"),
        LineNumberColor = SKColor.Parse("#586e75"),
        DefaultText = SKColor.Parse("#839496"),
        Keyword = new(SKColor.Parse("#859900"), Bold: true),
        String = new(SKColor.Parse("#2aa198")),
        Comment = new(SKColor.Parse("#586e75"), Italic: true),
        Type = new(SKColor.Parse("#b58900")),
        Number = new(SKColor.Parse("#d33682")),
        Method = new(SKColor.Parse("#268bd2")),
        Operator = new(SKColor.Parse("#859900")),
        Punctuation = new(SKColor.Parse("#93a1a1")),
    };

    public static readonly Dictionary<string, Theme> All = new(StringComparer.OrdinalIgnoreCase)
    {
        ["dracula"] = Dracula,
        ["monokai"] = Monokai,
        ["onedark"] = OneDark,
        ["nord"] = Nord,
        ["solarized-dark"] = SolarizedDark,
    };
}
