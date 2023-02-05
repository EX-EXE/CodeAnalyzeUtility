using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CodeAnalyzeUtility
{
    public static class OutputCodeExtensions
    {
        public static char IndentChar { get; set; } = '\t';

        public static string[] ToLines(this string lines)
        {
            return lines.Replace("\r\n", "\n").Split('\n').ToArray();
        }

        public static string OutputLine(this IEnumerable<string> lines, int indentCount = 0)
        {
            var separator = Environment.NewLine + new string(IndentChar, indentCount);
            return string.Join(Environment.NewLine, OutputLines(lines, indentCount));
        }

        public static string[] OutputLines(this IEnumerable<string> lines, int indentCount = 0)
        {
            return lines.Select(x => $"{new string(IndentChar, indentCount)}{x}").ToArray();
        }

        public static string[] OutputIfStatement(this string ifStatement, IEnumerable<string> thenSection, IEnumerable<string> elseSection)
        {
            var thenArray = thenSection.ToArray();
            var elseArray = elseSection.ToArray();
            if (thenArray.Length <= 0 && elseArray.Length <= 0)
            {
                return Array.Empty<string>();
            }

            var builder = new List<string>();
            // if
            builder.Add($"if({ifStatement})");
            builder.Add("{");
            if (0 < thenArray.Length)
            {
                builder.AddRange(thenSection.Select(line => $"{IndentChar}{line}"));
            }
            else
            {
                builder.Add($"{IndentChar}// nothing");
            }
            builder.Add("}");
            // else
            if (0 < elseArray.Length)
            {
                builder.Add("{");
                builder.AddRange(elseArray.Select(line => $"{IndentChar}{line}"));
                builder.Add("}");
            }
            return builder.ToArray();
        }
        public static string[] OutputStatement(this IEnumerable<string> lines)
        {
            var builder = new List<string>();
            builder.Add("{");
            {
                builder.AddRange(lines.Select(line => $"{IndentChar}{line}"));
            }
            builder.Add("}");
            return builder.ToArray();
        }

        public static string[] OutputForStatement(this string variableName, int start, int end, Func<string, string> sectionLineFunc)
        {
            return OutputForStatement(variableName, start, end, (x) => new[] { sectionLineFunc(x) });
        }

        public static string[] OutputForStatement(this string variableName, int start, int end, Func<string, IEnumerable<string>> sectionLineFunc)
        {
            var builder = new List<string>();
            // for
            builder.Add($"for(int {variableName} = {start}; {variableName} < {end} ; ++{variableName})");
            builder.Add("{");
            {
                var lines = sectionLineFunc(variableName);
                builder.AddRange(lines.Select(line => $"{IndentChar}{line}"));
            }
            builder.Add("}");
            return builder.ToArray();
        }

        public static string[] OutputForEachStatement(this string enumerableVariableName, Func<string, string> sectionLineFunc)
        {
            return OutputForEachStatement(enumerableVariableName, (x) => new[] { sectionLineFunc(x) });
        }

        public static string[] OutputForEachStatement(this string enumerableVariableName, Func<string, IEnumerable<string>> sectionLineFunc)
        {
            var itemVariableName = $"item{enumerableVariableName}";

            // for
            var builder = new List<string>();
            builder.Add($"foreach(var {itemVariableName} in {enumerableVariableName})");
            builder.Add("{");
            {
                var lines = sectionLineFunc(itemVariableName);
                builder.AddRange(lines.Select(line => $"{IndentChar}{line}"));
            }
            builder.Add("}");
            return builder.ToArray();
        }

        public static string[] ForEachLines<T>(this IEnumerable<T> values, Func<T, string> sectionLineFunc)
        {
            return ForEachLines<T>(values, (x) => new[] { sectionLineFunc(x) });
        }

        public static string[] ForEachLines<T>(this IEnumerable<T> values, Func<T, IEnumerable<string>> sectionLineFunc)
        {
            var builder = new List<string>();
            foreach (var value in values)
            {
                var lines = sectionLineFunc(value);
                builder.AddRange(lines);
            }
            return builder.ToArray();
        }

        public static string[] ForEachIndexLines<T>(this IEnumerable<T> values, Func<int, T, string> sectionLineFunc)
        {
            return ForEachIndexLines<T>(values, (i,x) => new[] { sectionLineFunc(i,x) });
        }

        public static string[] ForEachIndexLines<T>(this IEnumerable<T> values, Func<int, T, IEnumerable<string>> sectionLineFunc)
        {
            var builder = new List<string>();
            foreach (var (index, value) in values.Select((x, i) => (i, x)))
            {
                var lines = sectionLineFunc(index, value);
                builder.AddRange(lines);
            }
            return builder.ToArray();
        }
    }
}
