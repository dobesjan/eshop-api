using Eshop.Api.DataAccess.Repository;
using Eshop.Api.DataAccess.UnitOfWork;
using Eshop.Api.Models.Images;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Api.BusinessLayer.Services.Images
{
    public class ImageService : EshopService, IImageService
	{
		private IUnitOfWork _unitOfWork;

		public ImageService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public Image GetImage(int id)
		{
			if (id <= 0)
			{
				throw new InvalidDataException("Image not found in db!");
			}

			var entity = _unitOfWork.ImageRepository.Get(c => c.Id == id, includeProperties: "ImageGroup");
			if (entity == null)
			{
				throw new InvalidDataException("Image not found in db!");
			}

			return entity;
		}

		public ImageGroup GetImageGroup(int id)
		{
			if (id <= 0)
			{
				throw new InvalidDataException("Image group not found in db!");
			}

			var entity = _unitOfWork.ImageGroupRepository.Get(c => c.Id == id);
			if (entity == null)
			{
				throw new InvalidDataException("Image group not found in db!");
			}

			return entity;
		}

		public IEnumerable<ImageGroup> GetImageGroups()
		{
			return _unitOfWork.ImageGroupRepository.GetAll();
		}

		public IEnumerable<Image> GetImages(int offset = 0, int limit = 0, int imageGroupId = 0)
		{
			IEnumerable<Image> images = null;

			if (limit > 0)
			{
				images = _unitOfWork.ImageRepository.GetAll(offset: offset, limit: limit, includeProperties: "ImageGroup");
			}
			else
			{
				images = _unitOfWork.ImageRepository.GetAll(includeProperties: "ImageGroup");
			}

			if (imageGroupId > 0)
			{
				images = images.Where(i => i.ImageGroupId == imageGroupId);
			}

			return images;
		}

		public bool UpsertImage(Image image)
		{
			return UpsertEntity(image, _unitOfWork.ImageRepository) != null;
		}

		public bool UpsertImageGroup(ImageGroup imageGroup)
		{
			return UpsertEntity(imageGroup, _unitOfWork.ImageGroupRepository) != null;
		}
	}
}
