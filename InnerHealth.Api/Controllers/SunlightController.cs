using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using InnerHealth.Api.Dtos;
using InnerHealth.Api.Services;

namespace InnerHealth.Api.Controllers;

/// <summary>
/// Endpoints para registrar e acompanhar exposição ao sol.
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/sunlight")]
public class SunlightController : ControllerBase
{
    private readonly ISunlightService _sunlightService;
    private readonly IMapper _mapper;

    public SunlightController(ISunlightService sunlightService, IMapper mapper)
    {
        _sunlightService = sunlightService;
        _mapper = mapper;
    }

    /// <summary>
    /// Retorna os registros de exposição ao sol de hoje, o total acumulado
    /// e a recomendação diária.
    /// </summary>
    [HttpGet("today")]
    [MapToApiVersion("1.0")]
    [MapToApiVersion("2.0")]
    public async Task<IActionResult> GetToday()
    {
        var date = DateOnly.FromDateTime(DateTime.Now);
        var sessions = await _sunlightService.GetSessionsAsync(date);
        var total = await _sunlightService.GetDailyTotalAsync(date);
        var recommended = _sunlightService.GetRecommendedDailyMinutes();
        var dtoList = _mapper.Map<IEnumerable<SunlightSessionDto>>(sessions);

        return Ok(new
        {
            date,
            totalMinutes = total,
            recommendedMinutes = recommended,
            entries = dtoList
        });
    }

    /// <summary>
    /// Retorna o resumo semanal da exposição ao sol.
    /// </summary>
    [HttpGet("week")]
    [MapToApiVersion("1.0")]
    [MapToApiVersion("2.0")]
    public async Task<IActionResult> GetWeekly()
    {
        var today = DateOnly.FromDateTime(DateTime.Now);
        int diff = ((int)today.DayOfWeek + 6) % 7;
        var monday = today.AddDays(-diff);

        var totals = await _sunlightService.GetWeeklyTotalsAsync(monday);
        return Ok(totals);
    }

    /// <summary>
    /// Adiciona um novo registro de exposição ao sol.
    /// </summary>
    [HttpPost]
    [MapToApiVersion("1.0")]
    [MapToApiVersion("2.0")]
    public async Task<IActionResult> Post([FromBody] CreateSunlightSessionDto dto)
    {
        var session = await _sunlightService.AddSessionAsync(dto.Minutes);
        var resultDto = _mapper.Map<SunlightSessionDto>(session);

        return CreatedAtAction(nameof(GetToday), new { id = resultDto.Id }, resultDto);
    }

    /// <summary>
    /// Atualiza um registro de exposição ao sol.
    /// </summary>
    [HttpPut("{id}")]
    [MapToApiVersion("1.0")]
    [MapToApiVersion("2.0")]
    public async Task<IActionResult> Put(int id, [FromBody] UpdateSunlightSessionDto dto)
    {
        var updated = await _sunlightService.UpdateSessionAsync(id, dto.Minutes);
        if (updated == null) return NotFound();

        return Ok(_mapper.Map<SunlightSessionDto>(updated));
    }

    /// <summary>
    /// Remove um registro de exposição ao sol.
    /// </summary>
    [HttpDelete("{id}")]
    [MapToApiVersion("1.0")]
    [MapToApiVersion("2.0")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _sunlightService.DeleteSessionAsync(id);
        if (!deleted) return NotFound();

        return NoContent();
    }
}