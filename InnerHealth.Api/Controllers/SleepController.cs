using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using InnerHealth.Api.Dtos;
using InnerHealth.Api.Services;
using InnerHealth.Api.Models;

namespace InnerHealth.Api.Controllers;

/// <summary>
/// Endpoints para registrar e acompanhar registros de sono.
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/sleep")]
public class SleepController : ControllerBase
{
    private readonly ISleepService _sleepService;
    private readonly IMapper _mapper;

    public SleepController(ISleepService sleepService, IMapper mapper)
    {
        _sleepService = sleepService;
        _mapper = mapper;
    }

    /// <summary>
    /// Traz o registro de sono de hoje (se houver).
    /// </summary>
    [HttpGet("today")]
    [MapToApiVersion("1.0")]
    [MapToApiVersion("2.0")]
    public async Task<IActionResult> GetToday()
    {
        var date = DateOnly.FromDateTime(DateTime.Now);
        var record = await _sleepService.GetRecordAsync(date);

        if (record == null)
        {
            return Ok(new { date, record = (SleepRecordDto?)null });
        }

        return Ok(new
        {
            date,
            record = _mapper.Map<SleepRecordDto>(record)
        });
    }

    /// <summary>
    /// Retorna os registros de sono da semana, organizados por dia.
    /// </summary>
    [HttpGet("week")]
    [MapToApiVersion("1.0")]
    [MapToApiVersion("2.0")]
    public async Task<IActionResult> GetWeekly()
    {
        var today = DateOnly.FromDateTime(DateTime.Now);
        int diff = ((int)today.DayOfWeek + 6) % 7;
        var monday = today.AddDays(-diff);

        var records = await _sleepService.GetWeeklyRecordsAsync(monday);

        var mapped = records.ToDictionary(
            kvp => kvp.Key,
            kvp => kvp.Value != null ? _mapper.Map<SleepRecordDto>(kvp.Value) : null
        );

        return Ok(mapped);
    }

    /// <summary>
    /// Salva um novo registro de sono.
    /// </summary>
    [HttpPost]
    [MapToApiVersion("1.0")]
    [MapToApiVersion("2.0")]
    public async Task<IActionResult> Post([FromBody] CreateSleepRecordDto dto)
    {
        var record = await _sleepService.AddRecordAsync(dto.Hours, dto.Quality);
        var resultDto = _mapper.Map<SleepRecordDto>(record);

        return CreatedAtAction(nameof(GetToday), new { id = resultDto.Id }, resultDto);
    }

    /// <summary>
    /// Atualiza um registro de sono existente.
    /// </summary>
    [HttpPut("{id}")]
    [MapToApiVersion("1.0")]
    [MapToApiVersion("2.0")]
    public async Task<IActionResult> Put(int id, [FromBody] UpdateSleepRecordDto dto)
    {
        var updated = await _sleepService.UpdateRecordAsync(id, dto.Hours, dto.Quality);
        if (updated == null) return NotFound();

        return Ok(_mapper.Map<SleepRecordDto>(updated));
    }

    /// <summary>
    /// Remove um registro de sono.
    /// </summary>
    [HttpDelete("{id}")]
    [MapToApiVersion("1.0")]
    [MapToApiVersion("2.0")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _sleepService.DeleteRecordAsync(id);
        if (!deleted) return NotFound();

        return NoContent();
    }
}