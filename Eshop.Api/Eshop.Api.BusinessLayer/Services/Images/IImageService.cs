using Eshop.Api.Models.Images;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Api.BusinessLayer.Services.Images
{
    public interface IImageService : IEshopService
    {
        IEnumerable<ImageGroup> GetImageGroups();
        ImageGroup GetImageGroup(int id);
        bool UpsertImageGroup(ImageGroup imageGroup);

        IEnumerable<Image> GetImages(int offset = 0, int limit = 0, int imageGroupId = 0);
        Image GetImage(int id);
        bool UpsertImage(Image image);
    }
}
