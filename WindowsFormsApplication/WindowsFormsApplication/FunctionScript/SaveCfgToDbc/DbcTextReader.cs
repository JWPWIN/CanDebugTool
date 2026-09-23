using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

/// <summary>
/// 客户 OEM DBC 文本读取与分词：编码回退、按分号切语句、引号感知 Tokenize。
/// </summary>
public static class DbcTextReader
{
    private static readonly UTF8Encoding Utf8Strict = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true);
    private static readonly UTF8Encoding Utf8Lenient = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: false);

    /// <summary>
    /// 读取 DBC 文件文本。优先 UTF-8（含 BOM），失败或替换符过多则回退 GB18030。
    /// </summary>
    public static string ReadDbcFile(string filePath)
    {
        if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            return null;

        byte[] bytes = File.ReadAllBytes(filePath);
        if (bytes.Length == 0)
            return string.Empty;

        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        // UTF-8 BOM
        if (bytes.Length >= 3 && bytes[0] == 0xEF && bytes[1] == 0xBB && bytes[2] == 0xBF)
            return Utf8Lenient.GetString(bytes, 3, bytes.Length - 3);

        try
        {
            string utf8 = Utf8Strict.GetString(bytes);
            if (!LooksHeavilyCorrupted(utf8))
                return utf8;
        }
        catch (DecoderFallbackException)
        {
            // fall through to GB18030
        }

        try
        {
            return Encoding.GetEncoding("GB18030").GetString(bytes);
        }
        catch
        {
            return Utf8Lenient.GetString(bytes);
        }
    }

    private static bool LooksHeavilyCorrupted(string text)
    {
        if (string.IsNullOrEmpty(text))
            return false;

        int replacement = 0;
        int sample = Math.Min(text.Length, 8000);
        for (int i = 0; i < sample; i++)
        {
            if (text[i] == '\uFFFD')
                replacement++;
        }
        return replacement >= 8;
    }

    /// <summary>
    /// 按行切分（BO_/SG_ 无分号）；同时把以分号结束的属性语句（CM_/BA_/VAL_）
    /// 在跨行时拼成完整语句。返回用于解析的逻辑行列表。
    /// </summary>
    public static List<string> EnumerateLogicalLines(string dbcText)
    {
        var result = new List<string>();
        if (string.IsNullOrWhiteSpace(dbcText))
            return result;

        string[] rawLines = dbcText.Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.None);
        var pending = new StringBuilder();
        bool pendingOpenQuote = false;

        for (int i = 0; i < rawLines.Length; i++)
        {
            string line = rawLines[i];
            string trimmed = line.Trim();
            if (trimmed.Length == 0)
                continue;
            if (!pendingOpenQuote && trimmed.StartsWith("//", StringComparison.Ordinal))
                continue;

            if (pending.Length > 0)
            {
                // 若拼接到独立报文/节点行，说明之前误把 NS_ 符号当属性：丢弃 pending，改解析本行
                if (!pendingOpenQuote && IsStructuralLine(trimmed))
                {
                    pending.Clear();
                    pendingOpenQuote = false;
                    // fall through to normal handling
                }
                else
                {
                    pending.Append(' ').Append(trimmed);
                    pendingOpenQuote = CountUnescapedQuotes(pending.ToString()) % 2 != 0;
                    if (!pendingOpenQuote && pending.ToString().TrimEnd().EndsWith(";", StringComparison.Ordinal))
                    {
                        result.Add(NormalizeWhitespace(StripTrailingSemicolon(pending.ToString())));
                        pending.Clear();
                    }
                    continue;
                }
            }

            bool openQuote = CountUnescapedQuotes(trimmed) % 2 != 0;
            // 仅把「带参数的属性语句」当作跨行拼接；NS_ 列表里的裸关键字（如 BA_DEF_）不能触发
            bool isAttrStmt = IsAttributeStatement(trimmed);
            bool needsContinue = openQuote ||
                (isAttrStmt && !trimmed.EndsWith(";", StringComparison.Ordinal));

            if (needsContinue)
            {
                pending.Append(trimmed);
                pendingOpenQuote = openQuote;
                if (!pendingOpenQuote && pending.ToString().TrimEnd().EndsWith(";", StringComparison.Ordinal))
                {
                    result.Add(NormalizeWhitespace(StripTrailingSemicolon(pending.ToString())));
                    pending.Clear();
                }
                continue;
            }

            result.Add(NormalizeWhitespace(StripTrailingSemicolon(trimmed)));
        }

        if (pending.Length > 0)
            result.Add(NormalizeWhitespace(StripTrailingSemicolon(pending.ToString())));

        return result;
    }

    /// <summary>兼容旧名：等价于 EnumerateLogicalLines。</summary>
    public static List<string> EnumerateStatements(string dbcText) => EnumerateLogicalLines(dbcText);

    private static bool StartsWithKeyword(string line, params string[] keys)
    {
        foreach (string k in keys)
        {
            if (line.StartsWith(k, StringComparison.Ordinal))
                return true;
        }
        return false;
    }

    /// <summary>
    /// NS_ 段中的符号名常为单独一行的 BA_DEF_ / CM_ 等；只有带空格参数的才是真正属性语句。
    /// </summary>
    private static bool IsAttributeStatement(string trimmed)
    {
        if (string.IsNullOrEmpty(trimmed))
            return false;

        // 必须是关键字 + 空白 + 后续内容，避免把 NS_ 列表项当成跨行属性
        if (!trimmed.Contains(' '))
            return false;

        return StartsWithKeyword(trimmed,
            "CM_", "BA_", "BA_DEF_", "BA_DEF_DEF_", "VAL_", "SIG_GROUP_", "SIG_VALTYPE_", "BO_TX_BU_");
    }

    /// <summary>报文/信号/节点等结构行，遇到时应中断错误的属性拼接。</summary>
    private static bool IsStructuralLine(string trimmed)
    {
        return StartsWithKeyword(trimmed, "BO_", "SG_", "BU_:", "BU_", "BS_", "VERSION", "NS_");
    }

    private static int CountUnescapedQuotes(string text)
    {
        int n = 0;
        for (int i = 0; i < text.Length; i++)
        {
            if (text[i] == '"')
                n++;
        }
        return n;
    }

    private static string StripTrailingSemicolon(string text)
    {
        string t = text.TrimEnd();
        if (t.EndsWith(";", StringComparison.Ordinal))
            return t.Substring(0, t.Length - 1).TrimEnd();
        return t;
    }

    private static string NormalizeWhitespace(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return string.Empty;

        var sb = new StringBuilder(text.Length);
        bool prevSpace = false;
        bool inQuote = false;
        for (int i = 0; i < text.Length; i++)
        {
            char c = text[i];
            if (c == '"')
            {
                inQuote = !inQuote;
                sb.Append(c);
                prevSpace = false;
                continue;
            }

            if (!inQuote && (c == '\r' || c == '\n' || c == '\t'))
                c = ' ';

            if (!inQuote && c == ' ')
            {
                if (prevSpace) continue;
                prevSpace = true;
                sb.Append(' ');
                continue;
            }

            prevSpace = false;
            sb.Append(c);
        }
        return sb.ToString().Trim();
    }

    /// <summary>
    /// 空白分词；引号字符串为一个 token（去掉外层引号）。
    /// </summary>
    public static List<string> Tokenize(string statement)
    {
        var tokens = new List<string>();
        if (string.IsNullOrWhiteSpace(statement))
            return tokens;

        var sb = new StringBuilder();
        bool inQuote = false;

        for (int i = 0; i < statement.Length; i++)
        {
            char c = statement[i];
            if (c == '"')
            {
                if (inQuote)
                {
                    // 结束引号：提交字符串 token
                    tokens.Add(sb.ToString());
                    sb.Clear();
                    inQuote = false;
                }
                else
                {
                    // 开始引号：先刷出前面积累
                    if (sb.Length > 0)
                    {
                        tokens.Add(sb.ToString());
                        sb.Clear();
                    }
                    inQuote = true;
                }
                continue;
            }

            if (inQuote)
            {
                sb.Append(c);
                continue;
            }

            if (char.IsWhiteSpace(c))
            {
                if (sb.Length > 0)
                {
                    tokens.Add(sb.ToString());
                    sb.Clear();
                }
                continue;
            }

            sb.Append(c);
        }

        if (sb.Length > 0)
            tokens.Add(sb.ToString());

        return tokens;
    }
}
