using System;
using System.Collections.Generic;

namespace PureClarity.Helpers
{
    internal static class HostKeyFingerprint
    {
        public static bool Matches(string actualFingerprint, IReadOnlyList<string> expectedFingerprints)
        {
            if (expectedFingerprints == null)
            {
                return false;
            }

            var actual = Normalise(actualFingerprint);
            if (actual.Length == 0)
            {
                return false;
            }

            for (var i = 0; i < expectedFingerprints.Count; i++)
            {
                var expected = Normalise(expectedFingerprints[i]);
                if (expected.Length != 0 && string.Equals(expected, actual, StringComparison.Ordinal))
                {
                    return true;
                }
            }

            return false;
        }

        // Accepts fingerprints with or without the "SHA256:" prefix and base64 padding.
        public static string Normalise(string fingerprint)
        {
            if (string.IsNullOrWhiteSpace(fingerprint))
            {
                return string.Empty;
            }

            var value = fingerprint.Trim();
            if (value.StartsWith("SHA256:", StringComparison.OrdinalIgnoreCase))
            {
                value = value.Substring("SHA256:".Length);
            }

            return value.TrimEnd('=');
        }
    }
}
