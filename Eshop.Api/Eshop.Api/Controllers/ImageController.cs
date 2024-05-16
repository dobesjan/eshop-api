using Eshop.Api.BusinessLayer.Services.Images;
using Eshop.Api.BusinessLayer.Services.Interfaces.Images;
using Eshop.Api.DataAccess.Repository.Interfaces;
using Eshop.Api.Models.Images;
using Eshop.Api.Models.Products;
using Microsoft.AspNetCore.Mvc;

namespace Eshop.Api.Controllers
{
	[ApiController]
	public class ImageController : EshopApiControllerBase
	{
		private readonly IImageService _imageService;
		private readonly ILogger<ImageController> _logger;

		public ImageController(IImageService imageService, ILogger<ImageController> logger)
		{
			_imageService = imageService;
			_logger = logger;
		}

		[HttpGet]
		[Route("api/[controller]/listImageGroups")]
		public IActionResult ListImageGroups()
		{
			var imageGroups = _imageService.GetImageGroups();
			return Json(new { imageGroups });
		}

		[HttpGet]
		[Route("api/[controller]/listImages")]
		public IActionResult ListImages(int offset = 0, int limit = 0, int imageGroupId = 0)
		{
			IEnumerable<Image> images = _imageService.GetImages(offset, limit, imageGroupId);
			return Json(new { images });
		}

		[HttpGet]
		[Route("api/[controller]/get")]
		public IActionResult GetImage(int id = 0)
		{
			try
			{
				var image = _imageService.GetImage(id);
				return Json(new { data = image });
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return Json(new { success = false, message = "Image not found!" });
			}
		}

		[HttpGet]
		[Route("api/[controller]/getImageGroup")]
		public IActionResult GetImageGroup(int id = 0)
		{
			try
			{
				var group = _imageService.GetImageGroup(id);
				return Json(new { data = group });
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return Json(new { success = false, message = "Image group not found!" });
			}
		}

		[HttpPost]
		[Route("api/[controller]/upsertImageGroup")]
		public IActionResult UpsertImageGroup([FromBody] ImageGroup imageGroup)
		{
			var error = ValidateModel();
			if (error != null)
			{
				return error;
			}

			try
			{
				var success = _imageService.UpsertImageGroup(imageGroup);
				if (!success)
				{
					_logger.LogError("Problem saving image group!");
					return Json(new { success = false, message = "Problem saving image group!" });
				}

				return Json(new { success = true, message = "Problem saving image group!" });
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return Json(new { success = false, message = "Problem saving image group!" });
			}
		}

		[HttpPost]
		[Route("api/[controller]/upsertImage")]
		public IActionResult UpsertImage([FromBody] Image image)
		{
			var error = ValidateModel();
			if (error != null)
			{
				return error;
			}

			try
			{
				var success = _imageService.UpsertImage(image);
				if (!success)
				{
					_logger.LogError("Problem saving image group!");
					return Json(new { success = false, message = "Problem saving image group!" });
				}

				return Json(new { success = true, message = "Problem saving image group!" });
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return Json(new { success = false, message = "Problem saving image group!" });
			}
		}
	}
}
