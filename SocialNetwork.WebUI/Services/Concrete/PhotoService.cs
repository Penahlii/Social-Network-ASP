using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using SocialNetwork.WebUI.Services.Abstract;
using SocialNetwork.WebUI.Settings;

namespace SocialNetwork.WebUI.Services.Concrete;

public class PhotoService : IPhotoService
{
    private IConfiguration _configuration;
    private CloudinarySettings _cloudinarySettings;
    private Cloudinary _cloudinary;

    public PhotoService(IConfiguration configuration)
    {
        _configuration = configuration;
        _cloudinarySettings = _configuration.GetSection("CloudinarySettings").Get<CloudinarySettings>();
        Account account = new Account(_cloudinarySettings.CloudName,
            _cloudinarySettings.ApiKey, _cloudinarySettings.ApiSecret);
        _cloudinary = new Cloudinary(account); 
    }

    public async Task<string> UploadImageAsync(IFormFile uploadFile)
    {
        var uploadedResult = new ImageUploadResult();
        var file = uploadFile;
        if (file?.Length > 0)
        {
            using (var stream = file.OpenReadStream())
            {
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.Name, stream)
                };
                uploadedResult = await _cloudinary.UploadAsync(uploadParams);

                if (uploadedResult != null)
                {
                    return uploadedResult.Url.ToString();
                }
            }
        }
        return "";
    }
}
