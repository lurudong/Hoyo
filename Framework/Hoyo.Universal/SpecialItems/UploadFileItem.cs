namespace Hoyo.Universal;

public class UploadFileItem
{
    public string FileId { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public long Length { get; set; } = 0;
    public string ContentType { get; set; } = string.Empty;
}