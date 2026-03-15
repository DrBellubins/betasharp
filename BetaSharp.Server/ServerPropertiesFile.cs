using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace BetaSharp.Server
{
    internal sealed class ServerPropertiesFile
    {
        private readonly Dictionary<string, string> _values;
        private readonly List<string> _headerLines;

        public ServerPropertiesFile()
        {
            _values = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            _headerLines = new List<string>();
        }

        public static ServerPropertiesFile Load(string path)
        {
            var props = new ServerPropertiesFile();

            if (!File.Exists(path))
            {
                return props;
            }

            using var stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            using var reader = new StreamReader(stream);

            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                if (line.StartsWith("#", StringComparison.Ordinal))
                {
                    props._headerLines.Add(line);
                    continue;
                }

                int equalsIndex = line.IndexOf('=');
                if (equalsIndex <= 0)
                {
                    continue;
                }

                string key = line[..equalsIndex].Trim();
                string value = line[(equalsIndex + 1)..].Trim();

                if (key.Length == 0)
                {
                    continue;
                }

                props._values[key] = value;
            }

            return props;
        }

        public void Save(string path, string? titleComment = "BetaSharp server properties")
        {
            Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(path))!);

            using var stream = File.Open(path, FileMode.Create, FileAccess.Write, FileShare.None);
            using var writer = new StreamWriter(stream);

            if (!string.IsNullOrWhiteSpace(titleComment))
            {
                writer.Write('#');
                writer.WriteLine(titleComment);
            }

            writer.Write('#');
            writer.WriteLine(DateTime.Now.ToString("ddd MMM dd HH:mm:ss yyyy", CultureInfo.InvariantCulture));

            if (_headerLines.Count > 0)
            {
                foreach (string header in _headerLines)
                {
                    if (header.StartsWith("#BetaSharp server properties", StringComparison.Ordinal))
                    {
                        continue;
                    }

                    if (header.StartsWith("#Sun ", StringComparison.Ordinal) ||
                        header.StartsWith("#Mon ", StringComparison.Ordinal) ||
                        header.StartsWith("#Tue ", StringComparison.Ordinal) ||
                        header.StartsWith("#Wed ", StringComparison.Ordinal) ||
                        header.StartsWith("#Thu ", StringComparison.Ordinal) ||
                        header.StartsWith("#Fri ", StringComparison.Ordinal) ||
                        header.StartsWith("#Sat ", StringComparison.Ordinal))
                    {
                        continue;
                    }

                    writer.WriteLine(header);
                }
            }

            foreach (var pair in _values)
            {
                writer.Write(pair.Key);
                writer.Write('=');
                writer.WriteLine(pair.Value ?? string.Empty);
            }
        }

        public bool ContainsKey(string key)
        {
            return _values.ContainsKey(key);
        }

        public string GetString(string key, string fallback)
        {
            if (_values.TryGetValue(key, out string? value) && value != null)
            {
                return value;
            }

            _values[key] = fallback;
            return fallback;
        }

        public int GetInt(string key, int fallback)
        {
            string raw = GetString(key, fallback.ToString(CultureInfo.InvariantCulture));

            if (int.TryParse(raw, NumberStyles.Integer, CultureInfo.InvariantCulture, out int parsed))
            {
                return parsed;
            }

            _values[key] = fallback.ToString(CultureInfo.InvariantCulture);
            return fallback;
        }

        public bool GetBool(string key, bool fallback)
        {
            string raw = GetString(key, fallback.ToString());

            if (bool.TryParse(raw, out bool parsedBool))
            {
                return parsedBool;
            }

            if (raw == "1")
            {
                return true;
            }

            if (raw == "0")
            {
                return false;
            }

            _values[key] = fallback.ToString();
            return fallback;
        }

        public void SetString(string key, string value)
        {
            _values[key] = value ?? string.Empty;
        }

        public void SetInt(string key, int value)
        {
            _values[key] = value.ToString(CultureInfo.InvariantCulture);
        }

        public void SetBool(string key, bool value)
        {
            _values[key] = value.ToString();
        }
    }
}
