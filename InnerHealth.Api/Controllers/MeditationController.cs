using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using InnerHealth.Api.Dtos;
using InnerHealth.Api.Services;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authorization;

namespace InnerHealth.Api.Controllers;

/// <summary>
/// Fornece endpoints para gerenciar sessões de meditação, acompanhar totais diários e semanais
/// e registrar hábitos de atenção plena do usuário.
/// </summary>
/// <remarks>
/// Este controlador permite ao usuário registrar sessões de meditação,
/// consultar o total diário, acompanhar o progresso semanal e alterar sessões existentes.
/// </remarks>
[ApiController]
[Route("api/v{version:apiVersion}/meditation")]
public class MeditationController : ControllerBase
{
    private readonly IMeditationService _meditationService;
    private readonly IMapper _mapper;

    public MeditationController(IMeditationService meditationService, IMapper mapper)
    {
        _meditationService = meditationService;
        _mapper = mapper;
    }

    /// <summary>
    /// Retorna as sessões de meditação registradas no dia atual, incluindo total de minutos
    /// e o tempo recomendado de meditação diária.
    /// </summary>
    /// <remarks>
    /// <b>Exemplo de requisição:</b>
    ///
    ///     GET /api/v1/meditation/today
    ///
    /// <b>Exemplo de resposta:</b>
    ///
    /// ```json
    /// {
    ///   "date": "2025-01-10",
    ///   "totalMinutes": 15,
    ///   "recommendedMinutes": 20,
    ///   "entries": [
    ///     { "id": 1, "minutes": 10, "createdAt": "2025-01-10T10:00:00Z" },
    ///     { "id": 2, "minutes": 5,  "createdAt": "2025-01-10T16:30:00Z" }
    ///   ]
    /// }
    /// ```
    /// </remarks>
    /// <returns>Resumo de meditação do dia atual.</returns>
    /// <response code="200">Retorna as sessões registradas hoje e os totais agregados.</response>
     [HttpGet("today")]
        [SwaggerOperation(
            Summary = "Retorna o resumo diário de meditação.",
            Description = "Inclui as sessões registradas hoje, total de minutos e a recomendação diária."
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "Resumo diário retornado com sucesso.")]
        [MapToApiVersion("1.0")]
        [MapToApiVersion("2.0")]
    public async Task<IActionResult> GetToday()
    {
        var date = DateOnly.FromDateTime(DateTime.Now);
        var sessions = await _meditationService.GetSessionsAsync(date);
        var total = await _meditationService.GetDailyTotalAsync(date);
        var recommended = _meditationService.GetRecommendedDailyMinutes();
        var dtoList = _mapper.Map<IEnumerable<MeditationSessionDto>>(sessions);

        return Ok(new
        {
            date,
            totalMinutes = total,
            recommendedMinutes = recommended,
            entries = dtoList
        });
    }

    /// <summary>
    /// Retorna o total de minutos de meditação acumulados na semana atual (segunda a domingo).
    /// </summary>
    /// <remarks>
    /// <b>Exemplo de requisição:</b>
    ///
    ///     GET /api/v1/meditation/week
    ///
    /// <b>Exemplo de resposta:</b>
    ///
    /// ```json
    /// {
    ///   "monday": "2025-01-06",
    ///   "totals": {
    ///     "monday": 10,
    ///     "tuesday": 0,
    ///     "wednesday": 15,
    ///     "thursday": 5,
    ///     "friday": 0,
    ///     "saturday": 20,
    ///     "sunday": 0
    ///   }
    /// }
    /// ```
    /// </remarks>
    /// <returns>Totais semanais de meditação.</returns>
    /// <response code="200">Retorna os totais agregados da semana atual.</response>
     [HttpGet("week")]
        [SwaggerOperation(
            Summary = "Retorna o total semanal de meditação.",
            Description = "Agrupa os minutos de segunda a domingo, mostrando o progresso semanal."
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "Totais semanais retornados com sucesso.")]
        [MapToApiVersion("1.0")]
        [MapToApiVersion("2.0")]
    public async Task<IActionResult> GetWeekly()
    {
        var today = DateOnly.FromDateTime(DateTime.Now);
        int diff = ((int)today.DayOfWeek + 6) % 7;
        var monday = today.AddDays(-diff);
        var totals = await _meditationService.GetWeeklyTotalsAsync(monday);

        return Ok(totals);
    }

    /// <summary>
    /// Registra uma nova sessão de meditação para o dia atual.
    /// </summary>
    /// <remarks>
    /// <b>Exemplo de requisição:</b>
    ///
    ///     POST /api/v1/meditation
    ///     {
    ///        "minutes": 12
    ///     }
    ///
    /// <b>Observação:</b> A data da sessão é automaticamente atribuída ao dia atual.
    /// </remarks>
    /// <param name="dto">Duração da sessão em minutos.</param>
    /// <returns>A sessão de meditação criada.</returns>
    /// <response code="201">Sessão criada com sucesso.</response>
    /// <response code="400">Dados inválidos.</response>
    [HttpPost]
        [SwaggerOperation(
            Summary = "Registra uma nova sessão de meditação.",
            Description = "Cria uma sessão no dia atual. Apenas os minutos precisam ser informados."
        )]
        [SwaggerResponse(StatusCodes.Status201Created, "Sessão criada com sucesso.", typeof(MeditationSessionDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Dados inválidos.")]
        [MapToApiVersion("1.0")]
        [MapToApiVersion("2.0")]
    public async Task<IActionResult> Post([FromBody] CreateMeditationSessionDto dto)
    {
        var session = await _meditationService.AddSessionAsync(dto.Minutes);
        var resultDto = _mapper.Map<MeditationSessionDto>(session);

        return CreatedAtAction(nameof(GetToday), new { id = resultDto.Id }, resultDto);
    }

    /// <summary>
    /// Atualiza uma sessão de meditação existente pelo ID.
    /// </summary>
    /// <remarks>
    /// <b>Exemplo de requisição:</b>
    ///
    ///     PUT /api/v1/meditation/3
    ///     {
    ///         "minutes": 18
    ///     }
    /// </remarks>
    /// <param name="id">ID da sessão.</param>
    /// <param name="dto">Novos minutos atualizados.</param>
    /// <returns>A sessão atualizada.</returns>
    /// <response code="200">Retorna a sessão atualizada.</response>
    /// <response code="404">Sessão não encontrada.</response>
    [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Atualiza uma sessão de meditação.",
            Description = "Permite alterar a duração da sessão existente."
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "Sessão atualizada com sucesso.", typeof(MeditationSessionDto))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Sessão não encontrada.")]
        [MapToApiVersion("1.0")]
        [MapToApiVersion("2.0")]
    public async Task<IActionResult> Put(int id, [FromBody] UpdateMeditationSessionDto dto)
    {
        var updated = await _meditationService.UpdateSessionAsync(id, dto.Minutes);
        if (updated == null) return NotFound();

        return Ok(_mapper.Map<MeditationSessionDto>(updated));
    }

    /// <summary>
    /// Exclui uma sessão de meditação pelo ID.
    /// </summary>
    /// <remarks>
    /// <b>Exemplo de requisição:</b>
    ///
    ///     DELETE /api/v1/meditation/4
    /// </remarks>
    /// <param name="id">ID da sessão a ser removida.</param>
    /// <response code="204">Sessão excluída com sucesso.</response>
    /// <response code="404">Sessão não encontrada.</response>
    [Authorize]
    [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Exclui uma sessão de meditação.",
            Description = "Remove permanentemente a sessão pelo ID informado."
        )]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Sessão excluída com sucesso.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Sessão não encontrada.")]
        [MapToApiVersion("1.0")]
        [MapToApiVersion("2.0")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _meditationService.DeleteSessionAsync(id);
        if (!deleted) return NotFound();

        return NoContent();
    }
}
