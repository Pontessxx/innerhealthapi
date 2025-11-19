using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using InnerHealth.Api.Dtos;
using InnerHealth.Api.Services;

namespace InnerHealth.Api.Controllers;

/// <summary>
/// Fornece endpoints para gerenciar sessões de exposição ao sol,
/// acompanhar totais diários e resumir hábitos semanais relacionados à vitamina D.
/// </summary>
/// <remarks>
/// Este controlador permite registrar sessões de exposição solar,
/// consultar totais diários e semanais, atualizar registros e excluir sessões.
/// </remarks>
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
    /// Retorna as sessões de exposição ao sol registradas no dia atual,
    /// incluindo o total de minutos e a quantidade recomendada para o dia.
    /// </summary>
    /// <remarks>
    /// <b>Exemplo de requisição:</b>
    ///
    ///     GET /api/v1/sunlight/today
    ///
    /// <b>Exemplo de resposta:</b>
    /// ```json
    /// {
    ///   "date": "2025-01-10",
    ///   "totalMinutes": 15,
    ///   "recommendedMinutes": 20,
    ///   "entries": [
    ///     { "id": 1, "minutes": 10, "createdAt": "2025-01-10T09:00:00Z" },
    ///     { "id": 2, "minutes": 5,  "createdAt": "2025-01-10T16:30:00Z" }
    ///   ]
    /// }
    /// ```
    /// </remarks>
    /// <returns>Resumo diário de exposição ao sol.</returns>
    /// <response code="200">Retorna o resumo de exposição solar do dia.</response>
    [HttpGet("today")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
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
    /// Retorna os totais de exposição ao sol da semana atual (segunda–domingo).
    /// </summary>
    /// <remarks>
    /// As semanas sempre começam na <b>segunda-feira</b>.
    ///
    /// <b>Exemplo de resposta:</b>
    /// ```json
    /// {
    ///   "monday": 10,
    ///   "tuesday": 5,
    ///   "wednesday": 0,
    ///   "thursday": 8,
    ///   "friday": 12,
    ///   "saturday": 0,
    ///   "sunday": 20
    /// }
    /// ```
    /// </remarks>
    /// <returns>Dicionário com minutos totais para cada dia da semana.</returns>
    /// <response code="200">Retorna o resumo semanal de exposição ao sol.</response>
    [HttpGet("week")]
    [ProducesResponseType(typeof(Dictionary<string, int>), StatusCodes.Status200OK)]
    [MapToApiVersion("1.0")]
    [MapToApiVersion("2.0")]
    public async Task<IActionResult> GetWeekly()
    {
        var today = DateOnly.FromDateTime(DateTime.Now);

        // Calcula quantos dias voltar para chegar na segunda-feira
        int diff = ((int)today.DayOfWeek + 6) % 7;
        var monday = today.AddDays(-diff);

        var totals = await _sunlightService.GetWeeklyTotalsAsync(monday);

        return Ok(totals);
    }

    /// <summary>
    /// Registra uma nova sessão de exposição ao sol para o dia atual.
    /// </summary>
    /// <remarks>
    /// <b>Exemplo de requisição:</b>
    ///
    ///     POST /api/v1/sunlight
    ///     {
    ///         "minutes": 12
    ///     }
    ///
    /// <b>Exemplo de resposta:</b>
    /// ```json
    /// {
    ///   "id": 4,
    ///   "minutes": 12,
    ///   "createdAt": "2025-01-10T13:00:00Z"
    /// }
    /// ```
    /// </remarks>
    /// <param name="dto">Quantidade de minutos de exposição solar.</param>
    /// <returns>A sessão registrada.</returns>
    /// <response code="201">Sessão criada com sucesso.</response>
    /// <response code="400">Payload enviado é inválido.</response>
    [HttpPost]
    [ProducesResponseType(typeof(SunlightSessionDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [MapToApiVersion("1.0")]
    [MapToApiVersion("2.0")]
    public async Task<IActionResult> Post([FromBody] CreateSunlightSessionDto dto)
    {
        var session = await _sunlightService.AddSessionAsync(dto.Minutes);
        var resultDto = _mapper.Map<SunlightSessionDto>(session);

        return CreatedAtAction(nameof(GetToday), new { id = resultDto.Id }, resultDto);
    }

    /// <summary>
    /// Atualiza uma sessão de exposição ao sol existente pelo ID.
    /// </summary>
    /// <remarks>
    /// <b>Exemplo de requisição:</b>
    ///
    ///     PUT /api/v1/sunlight/7
    ///     {
    ///         "minutes": 20
    ///     }
    /// </remarks>
    /// <param name="id">ID da sessão.</param>
    /// <param name="dto">Quantidade de minutos atualizada.</param>
    /// <returns>A sessão atualizada.</returns>
    /// <response code="200">Sessão atualizada com sucesso.</response>
    /// <response code="404">Sessão não encontrada.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(SunlightSessionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [MapToApiVersion("1.0")]
    [MapToApiVersion("2.0")]
    public async Task<IActionResult> Put(int id, [FromBody] UpdateSunlightSessionDto dto)
    {
        var updated = await _sunlightService.UpdateSessionAsync(id, dto.Minutes);
        if (updated == null)
            return NotFound();

        return Ok(_mapper.Map<SunlightSessionDto>(updated));
    }

    /// <summary>
    /// Exclui uma sessão de exposição ao sol pelo ID.
    /// </summary>
    /// <remarks>
    /// <b>Exemplo de requisição:</b>
    ///
    ///     DELETE /api/v1/sunlight/2
    /// </remarks>
    /// <param name="id">ID da sessão.</param>
    /// <response code="204">Sessão excluída com sucesso.</response>
    /// <response code="404">Sessão não encontrada.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [MapToApiVersion("1.0")]
    [MapToApiVersion("2.0")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _sunlightService.DeleteSessionAsync(id);
        if (!deleted)
            return NotFound();

        return NoContent();
    }
}
