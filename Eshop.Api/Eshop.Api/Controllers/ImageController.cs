using Eshop.Api.DataAccess.Repository.Interfaces;
using Eshop.Api.Models.Images;
using Eshop.Api.Models.Products;
using Microsoft.AspNetCore.Mvc;

namespace Eshop.Api.Controllers
{
	[ApiController]
	public class ImageController : EshopApiControllerBase
	{
		public IRepository<ImageGroup> _imageGroupRepository;
		public IRepository<Image> _imageRepository;

		public ImageController(IRepository<ImageGroup> imageGroupRepository, IRepository<Image> imageRepository)
		{
			_imageGroupRepository = imageGroupRepository;
			_imageRepository = imageRepository;
		}

		[HttpGet]
		[Route("api/[controller]/listImageGroups")]
		public IActionResult ListImageGroups()
		{
			var imageGroups = _imageGroupRepository.GetAll();
			return Json(new { imageGroups });
		}

		[HttpGet]
		[Route("api/[controller]/listImages")]
		public IActionResult ListImages(int offset = 0, int limit = 0)
		{
			IEnumerable<Image> images = null;

			if (limit > 0)
			{
				images = _imageRepository.GetAll(offset: offset, limit: limit);
			}
			else
			{
				images = _imageRepository.GetAll();
			}

			return Json(new { images });
		}

		[HttpPost]
		[Route("api/[controller]/upsertImageGroup")]
		public IActionResult UpsertImageGroup([FromBody] ImageGroup imageGroup)
		{
			return UpsertEntity(imageGroup, _imageGroupRepository, true);
		}

		[HttpPost]
		[Route("api/[controller]/upsertImage")]
		public IActionResult UpsertImage([FromBody] Image image)
		{
			return UpsertEntity(image, _imageRepository, true);
		}
	}
}
