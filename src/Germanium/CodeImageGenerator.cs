using SkiaSharp;

namespace Germanium;

public class CodeImageOptions
{
    public int? Width { get; set; }
    public int? Height { get; set; }
    public string ThemeName { get; set; } = "dracula";
    public string? Language { get; set; }
    public string FontFamily { get; set; } = "Consolas";
    public float FontSize { get; set; } = 14f;
    public bool ShowLineNumbers { get; set; } = true;
    public bool ShowWindowControls { get; set; } = true;
    public string? WindowTitle { get; set; }
    public int PaddingX { get; set; } = 60;
    public int PaddingY { get; set; } = 40;
    public float WindowCornerRadius { get; set; } = 10f;
    public float ShadowRadius { get; set; } = 20f;
    public bool ShowShadow { get; set; } = true;
}

public class CodeImageGenerator
{
    private const float LineSpacing = 1.4f;
    private const int TitleBarHeight = 36;
    private const int WindowPadding = 20;
    private const int LineNumberPadding = 15;

    public static void Generate(string code, string outputPath, CodeImageOptions options)
    {
        var theme = Theme.All.GetValueOrDefault(options.ThemeName) ?? Theme.Dracula;
        var language = options.Language ?? "generic";
        var lines = code.ReplaceLineEndings("\n").Split('\n');

        using var font = new SKFont(SKTypeface.FromFamilyName(options.FontFamily, SKFontStyle.Normal), options.FontSize);
        using var boldFont = new SKFont(SKTypeface.FromFamilyName(options.FontFamily, SKFontStyle.Bold), options.FontSize);
        using var italicFont = new SKFont(SKTypeface.FromFamilyName(options.FontFamily, SKFontStyle.Italic), options.FontSize);

        float charWidth = font.MeasureText("M");
        float lineHeight = options.FontSize * LineSpacing;

        // Calculate line number width
        int lineNumberDigits = lines.Length.ToString().Length;
        float lineNumberWidth = options.ShowLineNumbers
            ? charWidth * (lineNumberDigits + 1) + LineNumberPadding
            : 0;

        // Measure max line width
        float maxLineWidth = 0;
        foreach (var line in lines)
        {
            float w = font.MeasureText(line.TrimEnd());
            if (w > maxLineWidth) maxLineWidth = w;
        }

        // Calculate dimensions
        float codeWidth = lineNumberWidth + maxLineWidth + WindowPadding * 2;
        float codeHeight = lines.Length * lineHeight + WindowPadding * 2;
        float titleBar = options.ShowWindowControls ? TitleBarHeight : 0;
        float windowWidth = codeWidth;
        float windowHeight = codeHeight + titleBar;

        int imageWidth = options.Width ?? (int)(windowWidth + options.PaddingX * 2);
        int imageHeight = options.Height ?? (int)(windowHeight + options.PaddingY * 2);

        // Recalculate window to fit if explicit size is given
        if (options.Width.HasValue)
            windowWidth = imageWidth - options.PaddingX * 2;
        if (options.Height.HasValue)
            windowHeight = imageHeight - options.PaddingY * 2;

        using var surface = SKSurface.Create(new SKImageInfo(imageWidth, imageHeight));
        var canvas = surface.Canvas;

        // Background
        canvas.Clear(theme.Background);

        float windowX = (imageWidth - windowWidth) / 2f;
        float windowY = (imageHeight - windowHeight) / 2f;
        var windowRect = new SKRect(windowX, windowY, windowX + windowWidth, windowY + windowHeight);

        // Shadow
        if (options.ShowShadow)
        {
            using var shadowPaint = new SKPaint
            {
                Color = new SKColor(0, 0, 0, 80),
                ImageFilter = SKImageFilter.CreateBlur(options.ShadowRadius, options.ShadowRadius),
                IsAntialias = true,
            };
            canvas.DrawRoundRect(windowRect, options.WindowCornerRadius, options.WindowCornerRadius, shadowPaint);
        }

        // Window background
        using var windowPaint = new SKPaint
        {
            Color = theme.WindowBackground,
            IsAntialias = true,
        };
        canvas.DrawRoundRect(windowRect, options.WindowCornerRadius, options.WindowCornerRadius, windowPaint);

        // Title bar
        if (options.ShowWindowControls)
        {
            var titleBarRect = new SKRect(windowX, windowY, windowX + windowWidth, windowY + TitleBarHeight);

            using var titleBarPaint = new SKPaint
            {
                Color = theme.TitleBarColor,
                IsAntialias = true,
            };

            // Draw title bar with rounded top corners using clipping
            canvas.Save();
            using var titleBarPath = new SKPath();
            titleBarPath.AddRoundRect(
                new SKRect(windowX, windowY, windowX + windowWidth, windowY + TitleBarHeight + options.WindowCornerRadius),
                options.WindowCornerRadius, options.WindowCornerRadius);

            var clipRect = new SKRect(windowX, windowY, windowX + windowWidth, windowY + TitleBarHeight);
            using var clipPath = new SKPath();
            clipPath.AddRoundRect(windowRect, options.WindowCornerRadius, options.WindowCornerRadius);
            canvas.ClipPath(clipPath);
            canvas.DrawRect(clipRect, titleBarPaint);
            canvas.Restore();

            // Traffic light buttons
            float buttonY = windowY + TitleBarHeight / 2f;
            float buttonX = windowX + 18f;
            float buttonRadius = 6f;
            float buttonSpacing = 20f;

            SKColor[] buttonColors = [
                SKColor.Parse("#ff5f57"), // red
                SKColor.Parse("#febc2e"), // yellow
                SKColor.Parse("#28c840"), // green
            ];

            foreach (var color in buttonColors)
            {
                using var btnPaint = new SKPaint { Color = color, IsAntialias = true };
                canvas.DrawCircle(buttonX, buttonY, buttonRadius, btnPaint);
                buttonX += buttonSpacing;
            }

            // Window title
            if (!string.IsNullOrEmpty(options.WindowTitle))
            {
                using var titlePaint = new SKPaint
                {
                    Color = theme.LineNumberColor,
                    IsAntialias = true,
                };
                using var titleFont = new SKFont(
                    SKTypeface.FromFamilyName("Segoe UI", SKFontStyle.Normal),
                    options.FontSize);

                float titleWidth = titleFont.MeasureText(options.WindowTitle);
                float titleX = windowX + (windowWidth - titleWidth) / 2f;
                float titleY = buttonY + options.FontSize / 3f;
                canvas.DrawText(options.WindowTitle, titleX, titleY, titleFont, titlePaint);
            }
        }

        // Code area
        float codeStartX = windowX + WindowPadding;
        float codeStartY = windowY + titleBar + WindowPadding + options.FontSize;

        // Clip to window
        canvas.Save();
        using var codePath = new SKPath();
        codePath.AddRoundRect(windowRect, options.WindowCornerRadius, options.WindowCornerRadius);
        canvas.ClipPath(codePath);

        for (int i = 0; i < lines.Length; i++)
        {
            float y = codeStartY + i * lineHeight;

            // Line numbers
            if (options.ShowLineNumbers)
            {
                using var lineNumPaint = new SKPaint
                {
                    Color = theme.LineNumberColor,
                    IsAntialias = true,
                };

                string lineNum = (i + 1).ToString().PadLeft(lineNumberDigits);
                float numWidth = font.MeasureText(lineNum);
                float numX = codeStartX + lineNumberWidth - numWidth - LineNumberPadding;
                canvas.DrawText(lineNum, numX, y, font, lineNumPaint);
            }

            // Syntax highlighted tokens
            float x = codeStartX + lineNumberWidth;
            var tokens = SyntaxHighlighter.Tokenize(lines[i], language);

            foreach (var token in tokens)
            {
                var (color, bold, italic) = GetTokenStyle(token.Type, theme);

                using var paint = new SKPaint
                {
                    Color = color,
                    IsAntialias = true,
                };

                var tokenFont = bold ? boldFont : italic ? italicFont : font;
                canvas.DrawText(token.Text, x, y, tokenFont, paint);
                x += tokenFont.MeasureText(token.Text);
            }
        }

        canvas.Restore();

        // Save to PNG
        using var image = surface.Snapshot();
        using var data = image.Encode(SKEncodedImageFormat.Png, 100);
        using var stream = File.OpenWrite(outputPath);
        data.SaveTo(stream);
    }

    private static (SKColor Color, bool Bold, bool Italic) GetTokenStyle(TokenType type, Theme theme)
    {
        var tc = type switch
        {
            TokenType.Keyword => theme.Keyword,
            TokenType.String => theme.String,
            TokenType.Comment => theme.Comment,
            TokenType.Type => theme.Type,
            TokenType.Number => theme.Number,
            TokenType.Method => theme.Method,
            TokenType.Operator => theme.Operator,
            TokenType.Punctuation => theme.Punctuation,
            _ => new TokenColor(theme.DefaultText),
        };
        return (tc.Color, tc.Bold, tc.Italic);
    }
}
