namespace Common.Service.ExtentionMethod
{
    public static class EConvert
    {
        public static int? ToNullableInt(this string s)
        {
            int i;
            if (int.TryParse(s, out i)) return i;
            return null;
        }
    }
}