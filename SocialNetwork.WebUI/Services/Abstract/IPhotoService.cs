namespace SocialNetwork.WebUI.Services.Abstract;

public interface IPhotoService
{
    Task<string> UploadImageAsync(IFormFile uploadFile);
}
