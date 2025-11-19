using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using InnerHealth.Api.Dtos;
using InnerHealth.Api.Services;
using InnerHealth.Api.Models;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authorization;

namespace InnerHealth.Api.Controllers;

/// <summary>
/// Fornece endpoints para gerenciar registros de atividade física,
/// acompanhar treinos diários e semanais e registrar sessões de exercício.
/// </summary>
/// <remarks>
/// Este controlador permite ao usuário registrar atividades físicas,
/// consultar histórico diário e semanal, atualizar registros e excluir atividades.
/// </remarks>
[ApiController]
[Route("api/v{version:apiVersion}/physical-activity")]
public class PhysicalActivityController : ControllerBase
{
    private readonly IPhysicalActivityService _activityService;
    private readonly IMapper _mapper;

    public PhysicalActivityController(IPhysicalActivityService activityService, IMapper mapper)
    {
        _activityService = activityService;
        _mapper = mapper;
    }

    /// <summary>
    /// Retorna todas as atividades físicas registradas no dia atual.
    /// </summary>
    /// <remarks>
    /// <b>Exemplo de requisição:</b>
    ///
    ///     GET /api/v1/physical-activity/today
    ///
    /// <b>Exemplo de resposta:</b>
    ///
    /// ```json
    /// {
    ///   "date": "2025-01-10",
    ///   "entries": [
    ///     { "id": 1, "modality": "Running", "durationMinutes": 30, "createdAt": "2025-01-10T09:00:00Z" },
    ///     { "id": 2, "modality": "Cycling", "durationMinutes": 45, "createdAt": "2025-01-10T18:00:00Z" }
    ///   ]
    /// }
    /// ```
    /// </remarks>
    /// <returns>Lista de atividades registradas hoje.</returns>
    /// <response code="200">Retorna as atividades do dia atual.</response>
    [HttpGet("today")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [MapToApiVersion("1.0")]
    [MapToApiVersion("2.0")]
    [SwaggerOperation(
        Summary = "Retorna as atividades físicas registradas no dia atual.",
        Description = "Inclui todas as atividades registradas hoje, com modalidade, duração e horário."
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Atividades do dia retornadas com sucesso.")]

    public async Task<IActionResult> GetToday()
    {
        var date = DateOnly.FromDateTime(DateTime.Now);
        var activities = await _activityService.GetActivitiesAsync(date);
        var dtoList = _mapper.Map<IEnumerable<PhysicalActivityDto>>(activities);

        return Ok(new { date, entries = dtoList });
    }

    /// <summary>
    /// Retorna todas as atividades físicas registradas na semana atual (segunda–domingo).
    /// </summary>
    /// <remarks>
    /// As semanas sempre começam na <b>segunda-feira</b>.
    ///
    /// <b>Exemplo de resposta:</b>
    ///
    /// ```json
    /// {
    ///   "monday": [
    ///     { "id": 10, "modality": "Walking", "durationMinutes": 20 }
    ///   ],
    ///   "tuesday": [],
    ///   "wednesday": [
    ///     { "id": 12, "modality": "Football", "durationMinutes": 60 }
    ///   ],
    ///   "thursday": [],
    ///   "friday": [],
    ///   "saturday": [
    ///     { "id": 14, "modality": "Gym", "durationMinutes": 45 }
    ///   ],
    ///   "sunday": []
    /// }
    /// ```
    /// </remarks>
    /// <returns>Lista de atividades agrupadas por dia da semana.</returns>
    /// <response code="200">Retorna o resumo semanal das atividades físicas.</response>
    [HttpGet("week")]
    [ProducesResponseType(typeof(Dictionary<string, IEnumerable<PhysicalActivityDto>>), StatusCodes.Status200OK)]
    [MapToApiVersion("1.0")]
    [MapToApiVersion("2.0")]
    [SwaggerOperation(
        Summary = "Retorna todas as atividades físicas da semana atual.",
        Description = "Lista atividades por dia útil, de segunda a domingo."
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Resumo semanal retornado com sucesso.")]

    public async Task<IActionResult> GetWeekly()
    {
        var today = DateOnly.FromDateTime(DateTime.Now);
        int diff = ((int)today.DayOfWeek + 6) % 7;
        var monday = today.AddDays(-diff);

        var activities = await _activityService.GetWeeklyActivitiesAsync(monday);
        var mapped = activities.ToDictionary(
            kvp => kvp.Key,
            kvp => _mapper.Map<IEnumerable<PhysicalActivityDto>>(kvp.Value)
        );

        return Ok(mapped);
    }

    /// <summary>
    /// Registra uma nova sessão de atividade física no dia atual.
    /// </summary>
    /// <remarks>
    /// <b>Exemplo de requisição:</b>
    ///
    ///     POST /api/v1/physical-activity
    ///     {
    ///         "modality": "Running",
    ///         "durationMinutes": 25
    ///     }
    ///
    /// <b>Exemplo de resposta:</b>
    ///
    /// ```json
    /// {
    ///   "id": 3,
    ///   "modality": "Running",
    ///   "durationMinutes": 25,
    ///   "createdAt": "2025-01-10T14:21:00Z"
    /// }
    /// ```
    /// </remarks>
    /// <param name="dto">Dados da atividade a ser registrada.</param>
    /// <returns>A atividade criada.</returns>
    /// <response code="201">Atividade criada com sucesso.</response>
    /// <response code="400">Dados inválidos fornecidos.</response>
    [HttpPost]
    [ProducesResponseType(typeof(PhysicalActivityDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [MapToApiVersion("1.0")]
    [MapToApiVersion("2.0")]
    [SwaggerOperation(
        Summary = "Registra uma nova sessão de atividade física.",
        Description = "Cria uma nova atividade no dia atual. Modalidade e duração são obrigatórias."
    )]
    [SwaggerResponse(StatusCodes.Status201Created, "Atividade criada com sucesso.", typeof(PhysicalActivityDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Dados inválidos fornecidos.")]

    public async Task<IActionResult> Post([FromBody] CreatePhysicalActivityDto dto)
    {
        var activity = await _activityService.AddActivityAsync(dto.Modality, dto.DurationMinutes);
        var resultDto = _mapper.Map<PhysicalActivityDto>(activity);

        return CreatedAtAction(nameof(GetToday), new { id = resultDto.Id }, resultDto);
    }

    /// <summary>
    /// Atualiza uma sessão de atividade física existente pelo ID.
    /// </summary>
    /// <remarks>
    /// <b>Exemplo de requisição:</b>
    ///
    ///     PUT /api/v1/physical-activity/5
    ///     {
    ///         "modality": "Cycling",
    ///         "durationMinutes": 40
    ///     }
    /// </remarks>
    /// <param name="id">ID da atividade.</param>
    /// <param name="dto">Novos dados da atividade.</param>
    /// <returns>A atividade atualizada.</returns>
    /// <response code="200">Retorna a atividade atualizada.</response>
    /// <response code="404">Atividade não encontrada.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(PhysicalActivityDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [MapToApiVersion("1.0")]
    [MapToApiVersion("2.0")]
    [SwaggerOperation(
        Summary = "Atualiza um registro de atividade física.",
        Description = "Permite alterar modalidade e duração de uma atividade existente."
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Atividade atualizada com sucesso.", typeof(PhysicalActivityDto))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Atividade não encontrada.")]

    public async Task<IActionResult> Put(int id, [FromBody] UpdatePhysicalActivityDto dto)
    {
        var updated = await _activityService.UpdateActivityAsync(id, dto.Modality, dto.DurationMinutes);
        if (updated == null)
            return NotFound();

        return Ok(_mapper.Map<PhysicalActivityDto>(updated));
    }

    /// <summary>
    /// Exclui uma sessão de atividade física pelo ID.
    /// </summary>
    /// <remarks>
    /// <b>Exemplo de requisição:</b>
    ///
    ///     DELETE /api/v1/physical-activity/7
    /// </remarks>
    /// <param name="id">ID da atividade.</param>
    /// <response code="204">Atividade excluída com sucesso.</response>
    /// <response code="404">Atividade não encontrada.</response>
    [Authorize]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [MapToApiVersion("1.0")]
    [MapToApiVersion("2.0")]
    [SwaggerOperation(
        Summary = "Exclui um registro de atividade física.",
        Description = "Remove uma atividade existente pelo ID."
    )]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Atividade excluída com sucesso.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Atividade não encontrada.")]

    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _activityService.DeleteActivityAsync(id);
        if (!deleted)
            return NotFound();

        return NoContent();
    }
}
