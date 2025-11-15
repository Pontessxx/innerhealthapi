using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using InnerHealth.Api.Dtos;
using InnerHealth.Api.Models;
using InnerHealth.Api.Services;

namespace InnerHealth.Api.Controllers;

/// <summary>
/// API endpoints for managing water intake.
/// Provides both version 1 and version 2 endpoints under the same controller.
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/water")]
public class WaterController : ControllerBase
{
    private readonly IWaterService _waterService;
    private readonly IMapper _mapper;
    public WaterController(IWaterService waterService, IMapper mapper)
    {
        _waterService = waterService;
        _mapper = mapper;
    }

    /// <summary>
    /// Gets the water consumption for today, including individual records and totals.
    /// </summary>
    [HttpGet("today")]
    [MapToApiVersion("1.0")]
    [MapToApiVersion("2.0")]
    public async Task<IActionResult> GetToday()
    {
        var date = DateOnly.FromDateTime(DateTime.Now);
        var intakes = await _waterService.GetIntakesAsync(date);
        var total = await _waterService.GetDailyTotalAsync(date);
        var recommended = await _waterService.GetRecommendedDailyAmountAsync();
        var dtoList = _mapper.Map<IEnumerable<WaterIntakeDto>>(intakes);
        return Ok(new
        {
            date,
            totalMl = total,
            recommendedMl = recommended,
            entries = dtoList
        });
    }

    /// <summary>
    /// Gets the daily totals for the current week (Monday through Sunday).
    /// </summary>
    [HttpGet("week")]
    [MapToApiVersion("1.0")]
    [MapToApiVersion("2.0")]
    public async Task<IActionResult> GetWeekly()
    {
        var today = DateOnly.FromDateTime(DateTime.Now);
        // Determine Monday of the current week (ISO 8601: Monday=1, Sunday=7)
        int diff = ((int)today.DayOfWeek + 6) % 7; // convert Sunday=0 to 6, Monday=1 to 0
        var monday = today.AddDays(-diff);
        var totals = await _waterService.GetWeeklyTotalsAsync(monday);
        return Ok(totals);
    }

    /// <summary>
    /// Adds a new water intake entry for today.
    /// </summary>
    [HttpPost]
    [MapToApiVersion("1.0")]
    [MapToApiVersion("2.0")]
    public async Task<IActionResult> Post([FromBody] CreateWaterIntakeDto dto)
    {
        var intake = await _waterService.AddIntakeAsync(dto.AmountMl);
        var resultDto = _mapper.Map<WaterIntakeDto>(intake);
        return CreatedAtAction(nameof(GetToday), new { id = resultDto.Id }, resultDto);
    }

    /// <summary>
    /// Updates an existing water intake entry.
    /// </summary>
    [HttpPut("{id}")]
    [MapToApiVersion("1.0")]
    [MapToApiVersion("2.0")]
    public async Task<IActionResult> Put(int id, [FromBody] UpdateWaterIntakeDto dto)
    {
        var updated = await _waterService.UpdateIntakeAsync(id, dto.AmountMl);
        if (updated == null)
        {
            return NotFound();
        }
        return Ok(_mapper.Map<WaterIntakeDto>(updated));
    }

    /// <summary>
    /// Deletes a water intake entry.
    /// </summary>
    [HttpDelete("{id}")]
    [MapToApiVersion("1.0")]
    [MapToApiVersion("2.0")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _waterService.DeleteIntakeAsync(id);
        if (!deleted)
        {
            return NotFound();
        }
        return NoContent();
    }
}