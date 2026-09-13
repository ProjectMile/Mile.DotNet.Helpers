using System;
using System.IO;
using System.Text;

namespace Mile.DotNet.Helpers
{
    internal static class Text
    {
        public static Encoding CreateStrictEncoding(
            int CodePage)
        {
            return Encoding.GetEncoding(
                CodePage,
                EncoderFallback.ExceptionFallback,
                DecoderFallback.ExceptionFallback);
        }

        private static readonly Lazy<Encoding> m_StrictAscii = new Lazy<Encoding>(
            () => CreateStrictEncoding(20127));

        public static Encoding StrictAscii
        {
            get => m_StrictAscii.Value;
        }

        private static readonly Lazy<Encoding> m_Utf8WithoutBom =
            new Lazy<Encoding>(() => new UTF8Encoding(false, true));

        public static Encoding Utf8WithoutBom
        {
            get => m_Utf8WithoutBom.Value;
        }

        private static readonly Lazy<Encoding> m_Utf8WithBom =
            new Lazy<Encoding>(() => new UTF8Encoding(true, true));

        public static Encoding Utf8WithBom
        {
            get => m_Utf8WithBom.Value;
        }

        public static void TranscodeFile(
            string FilePath,
            Encoding InputEncoding,
            Encoding OutputEncoding)
        {
            File.WriteAllText(
                FilePath,
                File.ReadAllText(FilePath, InputEncoding),
                OutputEncoding);
        }


        public static void ConvertFileToUtf8(
            string FilePath,
            Encoding InputEncoding)
        {
            TranscodeFile(FilePath, InputEncoding, Utf8WithoutBom);
        }

        public static void ConvertFileToUtf8(
            string FilePath)
        {
            TranscodeFile(FilePath, Utf8WithBom, Utf8WithoutBom);
        }

        public static void ConvertFileToUtf8WithBom(
            string FilePath,
            Encoding InputEncoding)
        {
            TranscodeFile(FilePath, InputEncoding, Utf8WithBom);
        }

        public static void ConvertFileToUtf8WithBom(
            string FilePath)
        {
            TranscodeFile(FilePath, Utf8WithoutBom, Utf8WithBom);
        }

        public static string NormalizeLineEndings(
            string Text,
            string LineEnding = "\r\n")
        {
            string Normalized = Text.Replace("\r\n", "\n").Replace("\r", "\n");
            return Normalized.Replace("\n", LineEnding);
        }

        public static void NormalizeTextFile(
            string FilePath,
            Encoding InputEncoding,
            Encoding OutputEncoding,
            string OutputLineEnding = "\r\n")
        {
            File.WriteAllText(
                FilePath,
                NormalizeLineEndings(
                    File.ReadAllText(FilePath, InputEncoding),
                    OutputLineEnding),
                OutputEncoding);
        }

        public static void NormalizeTextFileToUtf8(
            string FilePath,
            Encoding InputEncoding,
            string OutputLineEnding = "\r\n")
        {
            NormalizeTextFile(
                FilePath,
                InputEncoding,
                Utf8WithoutBom,
                OutputLineEnding);
        }

        public static void NormalizeTextFileToUtf8WithBom(
            string FilePath,
            Encoding InputEncoding,
            string OutputLineEnding = "\r\n")
        {
            NormalizeTextFile(
                FilePath,
                InputEncoding,
                Utf8WithBom,
                OutputLineEnding);
        }

        public static void SaveTextToFileAsUtf8(
            string FilePath,
            string Contents)
        {
            File.WriteAllText(FilePath, Contents, Utf8WithoutBom);
        }

        public static void SaveTextToFileAsUtf8WithBom(
            string FilePath,
            string Contents)
        {
            File.WriteAllText(FilePath, Contents, Utf8WithBom);
        }

        private sealed class Utf8StringWriter : StringWriter
        {
            public override Encoding Encoding => Utf8WithoutBom;
        }

        public static StringWriter CreateUtf8StringWriter()
        {
            return new Utf8StringWriter();
        }
    }
}
