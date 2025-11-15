using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using InnerHealth.Api.Dtos;
using InnerHealth.Api.Services;
using InnerHealth.Api.Models;

namespace InnerHealth.Api.Controllers;

/// <summary>
/// API endpoints for managing the user profile.
/// There is only one user in the system, but the API supports multiple users for future extensibility.
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/profile")]
public class UserProfileController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IMapper _mapper;
    public UserProfileController(IUserService userService, IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
    }
    [HttpGet]
    [MapToApiVersion("1.0")]
    [MapToApiVersion("2.0")]
    public async Task<IActionResult> Get()
    {
        var user = await _userService.GetUserAsync();
        if (user == null)
        {
            return Ok((UserProfileDto?)null);
        }
        return Ok(_mapper.Map<UserProfileDto>(user));
    }
    [HttpPut]
    [MapToApiVersion("1.0")]
    [MapToApiVersion("2.0")]
    public async Task<IActionResult> Put([FromBody] UpdateUserProfileDto dto)
    {
        var user = await _userService.UpdateUserAsync(dto);
        return Ok(_mapper.Map<UserProfileDto>(user));
    }
}