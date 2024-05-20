using Eshop.Api.BusinessLayer.Services.Interfaces.Images;
using Eshop.Api.DataAccess.Repository.Interfaces;
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
		private readonly IRepository<ImageGroup> _imageGroupRepository;
		private readonly IRepository<Image> _imageRepository;

		public ImageService(IRepository<ImageGroup> imageGroupRepository, IRepository<Image> imageRepository)
		{
			_imageGroupRepository = imageGroupRepository;
			_imageRepository = imageRepository;
		}

		public Image GetImage(int id)
		{
			if (id <= 0)
			{
				throw new InvalidDataException("Image not found in db!");
			}

			var entity = _imageRepository.Get(c => c.Id == id, includeProperties: "ImageGroup");
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

			var entity = _imageGroupRepository.Get(c => c.Id == id);
			if (entity == null)
			{
				throw new InvalidDataException("Image group not found in db!");
			}

			return entity;
		}

		public IEnumerable<ImageGroup> GetImageGroups()
		{
			return _imageGroupRepository.GetAll();
		}

		public IEnumerable<Image> GetImages(int offset = 0, int limit = 0, int imageGroupId = 0)
		{
			IEnumerable<Image> images = null;

			if (limit > 0)
			{
				images = _imageRepository.GetAll(offset: offset, limit: limit, includeProperties: "ImageGroup");
			}
			else
			{
				images = _imageRepository.GetAll(includeProperties: "ImageGroup");
			}

			if (imageGroupId > 0)
			{
				images = images.Where(i => i.ImageGroupId == imageGroupId);
			}

			return images;
		}

		public bool UpsertImage(Image image)
		{
			return UpsertEntity(image, _imageRepository) != null;
		}

		public bool UpsertImageGroup(ImageGroup imageGroup)
		{
			return UpsertEntity(imageGroup, _imageGroupRepository) != null;
		}
	}
}
