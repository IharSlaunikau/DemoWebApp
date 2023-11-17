namespace DemoWebApp.WebSite.Extensions;

public static class ByteArrayExtensions
{
    public static string ToBase64Image(this byte[] image)
    {
        if (image == null || image.Length == 0)
        {
            return string.Empty;
        }

        return $"data:image/jpeg;base64,{Convert.ToBase64String(image)}";
    }
}
