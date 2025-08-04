using Shared;

namespace Backend;
public interface IImgurService
{
  Task<ImgurUploadResultDto> UploadImageAsync(string imageBase64, string title);
  Task DeleteImageAsync(string deletehash);
}