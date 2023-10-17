namespace ConfigComparer
{
    public abstract record CompareLineResult
    {
        public record Same(KeyValuePair<string, string> Left, KeyValuePair<string, string> Right) : CompareLineResult;
        public record LeftOnly(KeyValuePair<string, string> Left) : CompareLineResult;
        public record RightOnly(KeyValuePair<string, string> Right) : CompareLineResult;
        public record Different(KeyValuePair<string, string> Left, KeyValuePair<string, string> Right) : CompareLineResult;
    }
}