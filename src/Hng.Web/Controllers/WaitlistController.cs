﻿using Hng.Application.Features.Notifications.Dtos;
using Hng.Application.Features.Products.Dtos;
using Hng.Application.Features.WaitLists.Commands;
using Hng.Application.Features.WaitLists.Dtos;
using Hng.Application.Shared.Dtos;
using Hng.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Hng.Application.Features.WaitLists.Queries;

namespace Hng.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/waitlists")]
    public class WaitlistController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WaitlistController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Create Waitlist
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(SuccessResponseDto<NotificationDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FailureResponseDto<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(FailureResponseDto<object>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Create([FromBody] WaitListDto command)
        {
            try
            {
                var createCommand = new CreateWaitlistCommand(command);
                var response = await _mediator.Send(createCommand);
                return response != null
                      ? CreatedAtAction(nameof(Create), new SuccessResponseDto<Waitlist> { Data = response })
                      : BadRequest(new FailureResponseDto<object> { Error = "Conflict", Message = "Email already exists in the waitlist", Data = false });
            }
            catch (Exception ex)
            {
                return BadRequest(new FailureResponseDto<object>
                {
                    Error = "Bad Request",
                    Message = ex.Message,
                    Data = false
                });
            }
        }
        /// <summary>
        /// Get All Waitlist
        /// </summary>
        [HttpGet()]
        [ProducesResponseType(typeof(SuccessResponseDto<GetNotificationsResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FailureResponseDto<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(FailureResponseDto<object>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var query = new GetAllWaitlistQuery();
                var response = await _mediator.Send(query);
                return Ok(new SuccessResponseDto<List<Waitlist>>
                {
                    Message = "WaitLists retrieved successfully",
                    Data = response
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new FailureResponseDto<object>
                {
                    Error = "Bad Request",
                    Message = ex.Message,
                    Data = false
                });
            }
        }
        /// <summary>
        /// Waitlist Deletion - deletes waitlist by id
        /// </summary>
        [HttpDelete("{id:guid}")]
        [Authorize]
        [ProducesResponseType(typeof(SuccessResponseDto<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FailureResponseDto<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(FailureResponseDto<object>), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> DeleteWaitlistById(Guid id)
        {
            try
            {
                var command = new DeleteWaitlistByIdCommand(id);
                var response = await _mediator.Send(command);
                return response != null
                    ? Ok(new SuccessResponseDto<object> { Message = "Waitlist deleted successfully", Data = true })
                    : NotFound(new FailureResponseDto<object> { Error = "Not Found", Message = $"Waitlist with ID {id} not found.", Data = false });
            }
            catch (Exception ex)
            {
                return BadRequest(new FailureResponseDto<object>
                {
                    Error = "Bad Request",
                    Message = ex.Message,
                    Data = false
                });
            }
        }
    }
}
