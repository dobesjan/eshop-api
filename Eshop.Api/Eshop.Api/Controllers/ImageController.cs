using Eshop.Api.BusinessLayer.Services.Images;
using Eshop.Api.Controllers;
using Eshop.Api.Models.Images;
using Eshop.Api.Models.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class ImageController : EshopApiControllerBase
{
	private readonly IImageService _imageService;

	public ImageController(IImageService imageService, ILogger<ImageController> logger) : base(logger)
	{
		_imageService = imageService;
	}

	[HttpGet("listImageGroups")]
	public IActionResult ListImageGroups()
	{
		return HandleResponse(() =>
		{
			var imageGroups = _imageService.GetImageGroups();
			return CreateResult(success: true, data: imageGroups);
		}, "Problem while retrieving image groups!");
	}

	[HttpGet("listImages")]
	public IActionResult ListImages(int offset = 0, int limit = 0, int imageGroupId = 0)
	{
		return HandleResponse(() =>
		{
			var images = _imageService.GetImages(offset, limit, imageGroupId);
			return CreateResult(success: true, data: images);
		}, "Problem while retrieving images!");
	}

	[HttpGet("get")]
	public IActionResult GetImage(int id = 0)
	{
		return HandleResponse(() =>
		{
			var image = _imageService.GetImage(id);
			return CreateResult(success: true, data: image);
		}, "Image not found!");
	}

	[HttpGet("getImageGroup")]
	public IActionResult GetImageGroup(int id = 0)
	{
		return HandleResponse(() =>
		{
			var group = _imageService.GetImageGroup(id);
			return CreateResult(success: true, data: group);
		}, "Image group not found!");
	}

	[HttpPost("upsertImageGroup")]
	public IActionResult UpsertImageGroup([FromBody] ImageGroup imageGroup)
	{
		return HandleResponse(() =>
		{
			var success = _imageService.UpsertImageGroup(imageGroup);
			return CreateResult(success: success, successMessage: "Image group saved successfully!", errorMessage: "Problem saving image group!");
		}, "Problem saving image group!");
	}

	[HttpPost("upsertImage")]
	public IActionResult UpsertImage([FromBody] Image image)
	{
		return HandleResponse(() =>
		{
			var success = _imageService.UpsertImage(image);
			return CreateResult(success: success, successMessage: "Image saved successfully!", errorMessage: "Problem saving image!");
		}, "Problem saving image!");
	}
}
