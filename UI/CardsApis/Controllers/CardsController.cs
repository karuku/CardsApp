using Contracts.Extensions;
using Contracts.Mapper;
using Contracts.ReqModels;
using Contracts.ResModels;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CardsApis.Controllers
{
	[Route("api/v{version:apiVersion}/cards")]
	//[Route("api/v1/cards")]
	public class CardsController : ApiBaseController
	{
		private readonly IServiceManager _serviceManager;
		private readonly ILogger<CardsController> _logger;

		public CardsController(IServiceManager serviceManager,ILogger<CardsController> logger)
		{
			_serviceManager = serviceManager;
			_logger = logger;
		}

		/// <summary>
		/// Get request to query for range of cards.
		/// </summary>
		/// <param name="page">pagination page requested.</param>
		/// <param name="size">pagination page size requested.</param>
		/// <param name="searchTerm">string with values to filter from name, description and color.</param>
		/// <param name="dateCreated">date for the requested data. Format should be yyyyMMdd.</param>
		/// <param name="cardStatus">filter data by status.</param>
		/// <param name="orderBy">valid values include name, color, status, datecreated, or all.</param>
		/// <returns></returns>
		[HttpGet]
		public IActionResult GetAll(int? page=null, int? size=null, string? searchTerm = null,string? dateCreated=null,CardStatusTypes? cardStatus=null, string? orderBy = null)
		{
			var res = _serviceManager
				.CardService
				.GetCards(new CardsReq(page??1, size??10)
				{
					SearchTerm=searchTerm,
					CreatedDate=dateCreated,
					Status=cardStatus,
					OrderBy=orderBy
				});

			if (res.Datas != null) return Ok(res);

			return NotFound(res);
		}
		/// <summary>
		/// Get request to query for a card by id.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet("{id:long}")]
		public IActionResult Get(long id)
		{
			var res = _serviceManager
				.CardService
				.GetCardById(id);

			if (res.Data != null) return Ok(res);

			return NotFound(res);
		}
		/// <summary>
		/// Post request to create card.
		/// </summary>
		/// <param name="req"></param>
		/// <returns></returns>
		[HttpPost]
		public IActionResult Post(AddCardReq req)
		{
			if ((req.Color.Length > 0 && !req.Color.StartsWith("#")) || (req.Color.Length > 0 && req.Color.Length < 6))
			{
				return BadRequest(ResBase.InvalidRes("Invalid color, color should have 6 alphanumeric characters prefixed with a #."));
			}

			var editReq = AppObjMapper.Mapper.Map<EditCardReq>(req);
			editReq.IsEdit = false;
			editReq.Id = 0;
			var res = _serviceManager
				.CardService
				.EditCard(editReq);

			if (res.IsSuccessful()) return Ok(res);

			return BadRequest(res);
		}
		/// <summary>
		/// Put request to update a card identified by its id.
		/// </summary>
		/// <param name="id">card id to update.</param>
		/// <param name="req"></param>
		/// <returns></returns>
		[HttpPut("{id:long}")]
		public IActionResult Put(long id,UpdateCardReq req)
		{
			if ((int)req.CardStatus > 3 || (int)req.CardStatus < 0)
			{
				return BadRequest(ResBase.InvalidRes("Invalid card status, the valid range from 0 to 3"));
			}
			if ((req.Color.Length > 0 && !req.Color.StartsWith("#")) || (req.Color.Length>0 && req.Color.Length<6))
			{
				return BadRequest(ResBase.InvalidRes("Invalid color, color should have 6 alphanumeric characters prefixed with a #."));
			}
			var editReq = AppObjMapper.Mapper.Map<EditCardReq>(req);
			editReq.IsEdit = true;
			editReq.Id = id;
			var res = _serviceManager
				.CardService
				.EditCard(editReq);

			if (res.IsSuccessful()) return Ok(res);

			return BadRequest(res);
		}
		/// <summary>
		/// Delete request to remove a card, identified by its id.
		/// </summary>
		/// <param name="id">Card unique id to delete.</param>
		/// <returns></returns>
		[HttpDelete("{id:long}")]
		public IActionResult Delete(long id)
		{
			var res = _serviceManager
				.CardService
				.DeleteCard(id);

			if (res.IsSuccessful()) return Ok(res);

			return BadRequest(res);
		}

	}
}
