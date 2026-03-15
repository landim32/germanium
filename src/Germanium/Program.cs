using System.CommandLine;
using System.CommandLine.Binding;
using Germanium;

var fileArgument = new Argument<FileInfo>(
    "file",
    "Caminho para o arquivo de código-fonte");

var outputOption = new Option<string?>(
    ["--output", "-o"],
    "Caminho do arquivo PNG de saída (padrão: <nome-do-arquivo>.png)");

var widthOption = new Option<int?>(
    ["--width", "-w"],
    "Largura da imagem em pixels");

var heightOption = new Option<int?>(
    ["--height", "-h"],
    "Altura da imagem em pixels");

var themeOption = new Option<string>(
    ["--theme", "-t"],
    () => "dracula",
    $"Tema de cores ({string.Join(", ", Theme.All.Keys)})");

var languageOption = new Option<string?>(
    ["--language", "-l"],
    "Linguagem para syntax highlighting (auto-detecta se não especificado)");

var fontSizeOption = new Option<float>(
    "--font-size",
    () => 14f,
    "Tamanho da fonte");

var fontOption = new Option<string>(
    "--font",
    () => "Consolas",
    "Família da fonte");

var noLineNumbersOption = new Option<bool>(
    "--no-line-numbers",
    "Ocultar números de linha");

var noWindowOption = new Option<bool>(
    "--no-window",
    "Ocultar controles da janela (botões)");

var titleOption = new Option<string?>(
    "--title",
    "Título da janela");

var paddingOption = new Option<int>(
    "--padding",
    () => 60,
    "Padding externo em pixels");

var noShadowOption = new Option<bool>(
    "--no-shadow",
    "Desabilitar sombra");

var rootCommand = new RootCommand("Germanium - Gera imagens PNG bonitas de código-fonte")
{
    fileArgument,
    outputOption,
    widthOption,
    heightOption,
    themeOption,
    languageOption,
    fontSizeOption,
    fontOption,
    noLineNumbersOption,
    noWindowOption,
    titleOption,
    paddingOption,
    noShadowOption,
};

rootCommand.SetHandler(context =>
{
    var file = context.ParseResult.GetValueForArgument(fileArgument);
    var output = context.ParseResult.GetValueForOption(outputOption);
    var width = context.ParseResult.GetValueForOption(widthOption);
    var height = context.ParseResult.GetValueForOption(heightOption);
    var theme = context.ParseResult.GetValueForOption(themeOption) ?? "dracula";
    var language = context.ParseResult.GetValueForOption(languageOption);
    var fontSize = context.ParseResult.GetValueForOption(fontSizeOption);
    var font = context.ParseResult.GetValueForOption(fontOption) ?? "Consolas";
    var noLineNumbers = context.ParseResult.GetValueForOption(noLineNumbersOption);
    var noWindow = context.ParseResult.GetValueForOption(noWindowOption);
    var title = context.ParseResult.GetValueForOption(titleOption);
    var padding = context.ParseResult.GetValueForOption(paddingOption);
    var noShadow = context.ParseResult.GetValueForOption(noShadowOption);

    if (!file.Exists)
    {
        Console.Error.WriteLine($"Erro: Arquivo não encontrado: {file.FullName}");
        context.ExitCode = 1;
        return;
    }

    var code = File.ReadAllText(file.FullName);
    var lang = language ?? SyntaxHighlighter.DetectLanguage(file.FullName);
    var outputPath = output ?? Path.ChangeExtension(file.Name, ".png");

    var options = new CodeImageOptions
    {
        Width = width,
        Height = height,
        ThemeName = theme,
        Language = lang,
        FontFamily = font,
        FontSize = fontSize,
        ShowLineNumbers = !noLineNumbers,
        ShowWindowControls = !noWindow,
        WindowTitle = title ?? file.Name,
        PaddingX = padding,
        PaddingY = padding > 0 ? (int)(padding * 0.67f) : 0,
        ShowShadow = !noShadow,
    };

    try
    {
        CodeImageGenerator.Generate(code, outputPath, options);
        Console.WriteLine($"Imagem gerada: {Path.GetFullPath(outputPath)}");
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine($"Erro ao gerar imagem: {ex.Message}");
        context.ExitCode = 1;
    }
});

return await rootCommand.InvokeAsync(args);
