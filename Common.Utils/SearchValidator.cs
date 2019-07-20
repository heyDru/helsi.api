namespace Common.Utils
{
    public static class SearchValidator
    {
        public static bool ValidateSearchRequest(string searchString)
        {
            var minLength = 3;
            var maxLength = 256;

            return searchString.Length >= minLength && searchString.Length <= maxLength;
        }
    }
}