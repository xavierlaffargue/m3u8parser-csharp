namespace M3U8Parser
{
    using System;
    using System.Collections.Generic;

    public static class M3U8AttributeParser
    {
        [ThreadStatic]
        private static Dictionary<string, string> currentAttributes;

        public static IDisposable EnterContext(string content)
        {
            var previous = currentAttributes;
            currentAttributes = Parse(content);
            return new ContextScope(previous);
        }

        public static bool TryGetValue(string name, out string value)
        {
            if (currentAttributes != null && currentAttributes.TryGetValue(name, out value))
            {
                return true;
            }

            value = null;
            return false;
        }

        public static Dictionary<string, string> Parse(string content)
        {
            var dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            if (string.IsNullOrEmpty(content))
            {
                return dict;
            }

            int colonIndex = content.IndexOf(':');
            int firstEquals = content.IndexOf('=');
            if (firstEquals >= 0 && colonIndex > firstEquals)
            {
                colonIndex = -1;
            }

            string attributesPart = colonIndex >= 0 ? content.Substring(colonIndex + 1) : content;

            int i = 0;
            int length = attributesPart.Length;
            while (i < length)
            {
                while (i < length && (char.IsWhiteSpace(attributesPart[i]) || attributesPart[i] == ','))
                {
                    i++;
                }

                if (i >= length)
                {
                    break;
                }

                int keyStart = i;
                while (i < length && attributesPart[i] != '=' && !char.IsWhiteSpace(attributesPart[i]))
                {
                    i++;
                }

                if (i >= length || attributesPart[i] != '=')
                {
                    break;
                }

                string key = attributesPart.Substring(keyStart, i - keyStart).Trim();
                i++; // Skip '='

                while (i < length && char.IsWhiteSpace(attributesPart[i]))
                {
                    i++;
                }

                if (i >= length)
                {
                    dict[key] = string.Empty;
                    break;
                }

                string value;
                if (attributesPart[i] == '"')
                {
                    i++; // Skip leading quote
                    int valStart = i;
                    while (i < length && attributesPart[i] != '"')
                    {
                        if (attributesPart[i] == '\\' && i + 1 < length && attributesPart[i + 1] == '"')
                        {
                            i += 2;
                        }
                        else
                        {
                            i++;
                        }
                    }

                    value = attributesPart.Substring(valStart, i - valStart);
                    if (i < length)
                    {
                        i++; // Skip trailing quote
                    }
                }
                else
                {
                    int valStart = i;
                    while (i < length && attributesPart[i] != ',' && !char.IsWhiteSpace(attributesPart[i]))
                    {
                        i++;
                    }

                    value = attributesPart.Substring(valStart, i - valStart);
                }

                dict[key] = value;
            }

            return dict;
        }

        private sealed class ContextScope : IDisposable
        {
            private readonly Dictionary<string, string> previous;

            public ContextScope(Dictionary<string, string> previous)
            {
                this.previous = previous;
            }

            public void Dispose()
            {
                currentAttributes = this.previous;
            }
        }
    }
}
