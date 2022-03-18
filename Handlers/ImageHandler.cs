using ImageMagick;

namespace ImageHandlerExample.Handlers
{
    public class ImageHandler
    {
        public RequestDelegate Handler(string filePath)
        {
            return async context =>
            {
                FileInfo fileInfo = new FileInfo($"{filePath}\\{context.Request.RouteValues["imageName"].ToString()}"); // filepath and image parameter
                using MagickImage magick = new MagickImage(fileInfo); // image size library

                int width = magick.Width, height = magick.Height; // Original image size request

                if (!string.IsNullOrEmpty(context.Request.Query["w"].ToString()))
                    width = int.Parse(context.Request.Query["w"].ToString());
                if (!string.IsNullOrEmpty(context.Request.Query["h"].ToString()))
                    height = int.Parse(context.Request.Query["h"].ToString());

                magick.Resize(width, height);

                var buffer = magick.ToByteArray(); //  convert content type for response
                context.Response.Clear();
                context.Response.ContentType = string.Concat("image/", fileInfo.Extension.Replace(".", ""));

                await context.Response.Body.WriteAsync(buffer, 0, buffer.Length);
                await context.Response.WriteAsync(filePath);
            };
        }
    }
}