using System.Text.RegularExpressions;

namespace Germanium;

public enum TokenType
{
    Default,
    Keyword,
    String,
    Comment,
    Type,
    Number,
    Method,
    Operator,
    Punctuation,
}

public record Token(string Text, TokenType Type);

public static class SyntaxHighlighter
{
    private static readonly Dictionary<string, LanguageDefinition> Languages = new(StringComparer.OrdinalIgnoreCase);

    static SyntaxHighlighter()
    {
        RegisterCSharp();
        RegisterJavaScript();
        RegisterPython();
        RegisterRust();
        RegisterGo();
        RegisterJava();
        RegisterCpp();
        RegisterGeneric();
    }

    public static string DetectLanguage(string filePath)
    {
        var ext = Path.GetExtension(filePath).ToLowerInvariant();
        return ext switch
        {
            ".cs" => "csharp",
            ".js" or ".mjs" or ".cjs" => "javascript",
            ".ts" or ".tsx" => "javascript",
            ".jsx" => "javascript",
            ".py" or ".pyw" => "python",
            ".rs" => "rust",
            ".go" => "go",
            ".java" => "java",
            ".c" or ".h" => "cpp",
            ".cpp" or ".cc" or ".cxx" or ".hpp" => "cpp",
            _ => "generic",
        };
    }

    public static List<Token> Tokenize(string line, string language)
    {
        if (!Languages.TryGetValue(language, out var lang))
            lang = Languages["generic"];

        return lang.Tokenize(line);
    }

    private static void RegisterCSharp()
    {
        var keywords = new HashSet<string>
        {
            "abstract", "as", "base", "bool", "break", "byte", "case", "catch",
            "char", "checked", "class", "const", "continue", "decimal", "default",
            "delegate", "do", "double", "else", "enum", "event", "explicit", "extern",
            "false", "finally", "fixed", "float", "for", "foreach", "goto", "if",
            "implicit", "in", "int", "interface", "internal", "is", "lock", "long",
            "namespace", "new", "null", "object", "operator", "out", "override",
            "params", "private", "protected", "public", "readonly", "record", "ref",
            "return", "sbyte", "sealed", "short", "sizeof", "stackalloc", "static",
            "string", "struct", "switch", "this", "throw", "true", "try", "typeof",
            "uint", "ulong", "unchecked", "unsafe", "ushort", "using", "var",
            "virtual", "void", "volatile", "while", "yield", "async", "await",
            "when", "where", "init", "required", "global", "file", "scoped",
        };

        var types = new HashSet<string>
        {
            "String", "Int32", "Int64", "Boolean", "Double", "Float", "Decimal",
            "Object", "Byte", "Char", "DateTime", "TimeSpan", "Guid", "Task",
            "List", "Dictionary", "HashSet", "IEnumerable", "IList", "ICollection",
            "Action", "Func", "Console", "Math", "Environment", "File", "Path",
            "Array", "Span", "Memory", "ValueTask",
        };

        Languages["csharp"] = new LanguageDefinition(keywords, types, "//", ("/*", "*/"), '@');
    }

    private static void RegisterJavaScript()
    {
        var keywords = new HashSet<string>
        {
            "async", "await", "break", "case", "catch", "class", "const", "continue",
            "debugger", "default", "delete", "do", "else", "export", "extends",
            "false", "finally", "for", "from", "function", "if", "import", "in",
            "instanceof", "let", "new", "null", "of", "return", "static", "super",
            "switch", "this", "throw", "true", "try", "typeof", "undefined", "var",
            "void", "while", "with", "yield",
        };

        var types = new HashSet<string>
        {
            "Array", "Boolean", "Date", "Error", "Function", "JSON", "Map",
            "Math", "Number", "Object", "Promise", "Proxy", "RegExp", "Set",
            "String", "Symbol", "console", "window", "document", "process",
        };

        Languages["javascript"] = new LanguageDefinition(keywords, types, "//", ("/*", "*/"));
    }

    private static void RegisterPython()
    {
        var keywords = new HashSet<string>
        {
            "False", "None", "True", "and", "as", "assert", "async", "await",
            "break", "class", "continue", "def", "del", "elif", "else", "except",
            "finally", "for", "from", "global", "if", "import", "in", "is",
            "lambda", "nonlocal", "not", "or", "pass", "raise", "return", "try",
            "while", "with", "yield", "self",
        };

        var types = new HashSet<string>
        {
            "int", "float", "str", "bool", "list", "dict", "set", "tuple",
            "type", "object", "range", "print", "len", "enumerate", "zip",
            "map", "filter", "sorted", "reversed", "open", "super",
        };

        Languages["python"] = new LanguageDefinition(keywords, types, "#", null);
    }

    private static void RegisterRust()
    {
        var keywords = new HashSet<string>
        {
            "as", "async", "await", "break", "const", "continue", "crate", "dyn",
            "else", "enum", "extern", "false", "fn", "for", "if", "impl", "in",
            "let", "loop", "match", "mod", "move", "mut", "pub", "ref", "return",
            "self", "Self", "static", "struct", "super", "trait", "true", "type",
            "unsafe", "use", "where", "while", "yield",
        };

        var types = new HashSet<string>
        {
            "i8", "i16", "i32", "i64", "i128", "isize", "u8", "u16", "u32",
            "u64", "u128", "usize", "f32", "f64", "bool", "char", "str",
            "String", "Vec", "Option", "Result", "Box", "Rc", "Arc",
            "HashMap", "HashSet", "BTreeMap", "BTreeSet",
        };

        Languages["rust"] = new LanguageDefinition(keywords, types, "//", ("/*", "*/"));
    }

    private static void RegisterGo()
    {
        var keywords = new HashSet<string>
        {
            "break", "case", "chan", "const", "continue", "default", "defer",
            "else", "fallthrough", "for", "func", "go", "goto", "if", "import",
            "interface", "map", "package", "range", "return", "select", "struct",
            "switch", "type", "var", "nil", "true", "false",
        };

        var types = new HashSet<string>
        {
            "bool", "byte", "complex64", "complex128", "error", "float32",
            "float64", "int", "int8", "int16", "int32", "int64", "rune",
            "string", "uint", "uint8", "uint16", "uint32", "uint64", "uintptr",
            "fmt", "os", "io", "log", "http", "context",
        };

        Languages["go"] = new LanguageDefinition(keywords, types, "//", ("/*", "*/"));
    }

    private static void RegisterJava()
    {
        var keywords = new HashSet<string>
        {
            "abstract", "assert", "boolean", "break", "byte", "case", "catch",
            "char", "class", "const", "continue", "default", "do", "double",
            "else", "enum", "extends", "false", "final", "finally", "float",
            "for", "goto", "if", "implements", "import", "instanceof", "int",
            "interface", "long", "native", "new", "null", "package", "private",
            "protected", "public", "return", "short", "static", "strictfp",
            "super", "switch", "synchronized", "this", "throw", "throws",
            "transient", "true", "try", "void", "volatile", "while", "var",
            "record", "sealed", "permits", "yield",
        };

        var types = new HashSet<string>
        {
            "String", "Integer", "Long", "Double", "Float", "Boolean", "Character",
            "Byte", "Short", "Object", "Class", "System", "Math", "List",
            "ArrayList", "HashMap", "Map", "Set", "HashSet", "Optional",
            "Stream", "Collections", "Arrays",
        };

        Languages["java"] = new LanguageDefinition(keywords, types, "//", ("/*", "*/"));
    }

    private static void RegisterCpp()
    {
        var keywords = new HashSet<string>
        {
            "alignas", "alignof", "and", "asm", "auto", "bitand", "bitor", "bool",
            "break", "case", "catch", "char", "class", "const", "constexpr",
            "continue", "decltype", "default", "delete", "do", "double", "else",
            "enum", "explicit", "export", "extern", "false", "float", "for",
            "friend", "goto", "if", "inline", "int", "long", "mutable",
            "namespace", "new", "noexcept", "not", "nullptr", "operator", "or",
            "private", "protected", "public", "register", "return", "short",
            "signed", "sizeof", "static", "struct", "switch", "template", "this",
            "throw", "true", "try", "typedef", "typeid", "typename", "union",
            "unsigned", "using", "virtual", "void", "volatile", "while",
            "#include", "#define", "#ifdef", "#ifndef", "#endif", "#pragma",
        };

        var types = new HashSet<string>
        {
            "std", "string", "vector", "map", "set", "pair", "tuple",
            "unique_ptr", "shared_ptr", "weak_ptr", "optional", "variant",
            "cout", "cin", "endl", "printf", "scanf", "malloc", "free",
            "size_t", "int8_t", "int16_t", "int32_t", "int64_t",
            "uint8_t", "uint16_t", "uint32_t", "uint64_t",
        };

        Languages["cpp"] = new LanguageDefinition(keywords, types, "//", ("/*", "*/"));
    }

    private static void RegisterGeneric()
    {
        var keywords = new HashSet<string>
        {
            "if", "else", "for", "while", "do", "switch", "case", "break",
            "continue", "return", "class", "struct", "enum", "interface",
            "function", "var", "let", "const", "true", "false", "null",
            "new", "this", "import", "export", "from", "try", "catch",
            "throw", "finally", "async", "await", "yield", "public",
            "private", "protected", "static", "void", "int", "string",
            "bool", "float", "double",
        };

        Languages["generic"] = new LanguageDefinition(keywords, new HashSet<string>(), "//", ("/*", "*/"));
    }
}

internal partial class LanguageDefinition
{
    private readonly HashSet<string> _keywords;
    private readonly HashSet<string> _types;
    private readonly string _lineComment;
    private readonly (string Start, string End)? _blockComment;
    private readonly char? _verbatimStringPrefix;

    public LanguageDefinition(
        HashSet<string> keywords,
        HashSet<string> types,
        string lineComment,
        (string Start, string End)? blockComment,
        char? verbatimStringPrefix = null)
    {
        _keywords = keywords;
        _types = types;
        _lineComment = lineComment;
        _blockComment = blockComment;
        _verbatimStringPrefix = verbatimStringPrefix;
    }

    public List<Token> Tokenize(string line)
    {
        var tokens = new List<Token>();
        int i = 0;

        while (i < line.Length)
        {
            // Whitespace
            if (char.IsWhiteSpace(line[i]))
            {
                int start = i;
                while (i < line.Length && char.IsWhiteSpace(line[i])) i++;
                tokens.Add(new Token(line[start..i], TokenType.Default));
                continue;
            }

            // Line comment
            if (i + _lineComment.Length <= line.Length &&
                line[i..].StartsWith(_lineComment))
            {
                tokens.Add(new Token(line[i..], TokenType.Comment));
                break;
            }

            // Block comment start
            if (_blockComment.HasValue &&
                i + _blockComment.Value.Start.Length <= line.Length &&
                line[i..].StartsWith(_blockComment.Value.Start))
            {
                int end = line.IndexOf(_blockComment.Value.End, i + _blockComment.Value.Start.Length, StringComparison.Ordinal);
                if (end >= 0)
                {
                    end += _blockComment.Value.End.Length;
                    tokens.Add(new Token(line[i..end], TokenType.Comment));
                    i = end;
                }
                else
                {
                    tokens.Add(new Token(line[i..], TokenType.Comment));
                    break;
                }
                continue;
            }

            // Preprocessor directives (lines starting with #)
            if (line[i] == '#' && tokens.All(t => t.Type == TokenType.Default && string.IsNullOrWhiteSpace(t.Text)))
            {
                tokens.Add(new Token(line[i..], TokenType.Keyword));
                break;
            }

            // Strings
            if (line[i] == '"' || line[i] == '\'' || line[i] == '`')
            {
                char quote = line[i];
                int start = i;
                i++;
                while (i < line.Length)
                {
                    if (line[i] == '\\' && i + 1 < line.Length)
                    {
                        i += 2;
                        continue;
                    }
                    if (line[i] == quote)
                    {
                        i++;
                        break;
                    }
                    i++;
                }
                tokens.Add(new Token(line[start..i], TokenType.String));
                continue;
            }

            // Verbatim string prefix
            if (_verbatimStringPrefix.HasValue && line[i] == _verbatimStringPrefix.Value &&
                i + 1 < line.Length && line[i + 1] == '"')
            {
                int start = i;
                i += 2;
                while (i < line.Length)
                {
                    if (line[i] == '"')
                    {
                        if (i + 1 < line.Length && line[i + 1] == '"')
                        {
                            i += 2;
                            continue;
                        }
                        i++;
                        break;
                    }
                    i++;
                }
                tokens.Add(new Token(line[start..i], TokenType.String));
                continue;
            }

            // Dollar string interpolation
            if (line[i] == '$' && i + 1 < line.Length && line[i + 1] == '"')
            {
                int start = i;
                i += 2;
                while (i < line.Length)
                {
                    if (line[i] == '\\' && i + 1 < line.Length)
                    {
                        i += 2;
                        continue;
                    }
                    if (line[i] == '"')
                    {
                        i++;
                        break;
                    }
                    i++;
                }
                tokens.Add(new Token(line[start..i], TokenType.String));
                continue;
            }

            // Numbers
            if (char.IsDigit(line[i]) || (line[i] == '.' && i + 1 < line.Length && char.IsDigit(line[i + 1])))
            {
                int start = i;
                if (line[i] == '0' && i + 1 < line.Length && (line[i + 1] == 'x' || line[i + 1] == 'X'))
                {
                    i += 2;
                    while (i < line.Length && IsHexDigit(line[i])) i++;
                }
                else
                {
                    while (i < line.Length && (char.IsDigit(line[i]) || line[i] == '.' || line[i] == '_')) i++;
                    if (i < line.Length && (line[i] == 'e' || line[i] == 'E'))
                    {
                        i++;
                        if (i < line.Length && (line[i] == '+' || line[i] == '-')) i++;
                        while (i < line.Length && char.IsDigit(line[i])) i++;
                    }
                }
                // Type suffixes
                while (i < line.Length && (line[i] == 'f' || line[i] == 'F' || line[i] == 'd' || line[i] == 'D' ||
                       line[i] == 'l' || line[i] == 'L' || line[i] == 'u' || line[i] == 'U' ||
                       line[i] == 'm' || line[i] == 'M'))
                    i++;
                tokens.Add(new Token(line[start..i], TokenType.Number));
                continue;
            }

            // Identifiers and keywords
            if (char.IsLetter(line[i]) || line[i] == '_')
            {
                int start = i;
                while (i < line.Length && (char.IsLetterOrDigit(line[i]) || line[i] == '_')) i++;
                string word = line[start..i];

                // Check if it's followed by ( to detect methods
                bool isMethodCall = i < line.Length && line[i] == '(';
                // Check if followed by < for generic types
                bool isGenericType = i < line.Length && line[i] == '<';

                if (_keywords.Contains(word))
                    tokens.Add(new Token(word, TokenType.Keyword));
                else if (_types.Contains(word) || (char.IsUpper(word[0]) && isGenericType))
                    tokens.Add(new Token(word, TokenType.Type));
                else if (isMethodCall)
                    tokens.Add(new Token(word, TokenType.Method));
                else if (char.IsUpper(word[0]) && word.Length > 1)
                    tokens.Add(new Token(word, TokenType.Type));
                else
                    tokens.Add(new Token(word, TokenType.Default));
                continue;
            }

            // Operators
            if ("=+-*/<>!&|^~%?:".Contains(line[i]))
            {
                int start = i;
                i++;
                // Multi-char operators
                if (i < line.Length && "=+-*/<>!&|^~%?:".Contains(line[i])) i++;
                if (i < line.Length && line[i] == '=') i++;
                tokens.Add(new Token(line[start..i], TokenType.Operator));
                continue;
            }

            // Punctuation
            if ("(){}[];,.@".Contains(line[i]))
            {
                tokens.Add(new Token(line[i].ToString(), TokenType.Punctuation));
                i++;
                continue;
            }

            // Anything else
            tokens.Add(new Token(line[i].ToString(), TokenType.Default));
            i++;
        }

        return tokens;
    }

    private static bool IsHexDigit(char c) =>
        char.IsDigit(c) || (c >= 'a' && c <= 'f') || (c >= 'A' && c <= 'F');
}
