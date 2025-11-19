using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using InnerHealth.Api.Dtos;
using InnerHealth.Api.Services;
using InnerHealth.Api.Models;

namespace InnerHealth.Api.Controllers;

/// <summary>
/// Fornece endpoints para gerenciar registros de sono, acompanhar a qualidade diária do sono
/// e resumir hábitos semanais relacionados ao descanso.
/// </summary>
/// <remarks>
/// Este controlador permite registrar, consultar, atualizar e excluir registros de sono,
/// além de gerar resumos diários e semanais.
/// </remarks>
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
    /// Retorna o registro de sono do dia atual, se existir.
    /// </summary>
    /// <remarks>
    /// <b>Exemplo de requisição:</b>
    ///
    ///     GET /api/v1/sleep/today
    ///
    /// <b>Exemplo de resposta (sem registro):</b>
    /// ```json
    /// {
    ///   "date": "2025-01-10",
    ///   "record": null
    /// }
    /// ```
    ///
    /// <b>Exemplo de resposta (com registro):</b>
    /// ```json
    /// {
    ///   "date": "2025-01-10",
    ///   "record": {
    ///     "id": 4,
    ///     "hours": 7.5,
    ///     "quality": "Good",
    ///     "createdAt": "2025-01-10T08:30:00Z"
    ///   }
    /// }
    /// ```
    /// </remarks>
    /// <returns>O registro de sono de hoje, ou null caso ainda não exista.</returns>
    /// <response code="200">Retorna o resumo de sono do dia atual.</response>
    [HttpGet("today")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
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
    /// Retorna os registros de sono da semana atual (segunda–domingo).
    /// </summary>
    /// <remarks>
    /// As semanas sempre começam na <b>segunda-feira</b>.
    ///
    /// <b>Exemplo de resposta:</b>
    /// ```json
    /// {
    ///   "monday": { "hours": 7, "quality": "Good" },
    ///   "tuesday": null,
    ///   "wednesday": { "hours": 6.5, "quality": "Average" },
    ///   "thursday": null,
    ///   "friday": { "hours": 8, "quality": "Excellent" },
    ///   "saturday": null,
    ///   "sunday": null
    /// }
    /// ```
    /// </remarks>
    /// <returns>Dicionário contendo cada dia da semana e seu registro correspondente.</returns>
    /// <response code="200">Retorna o resumo semanal do sono.</response>
    [HttpGet("week")]
    [ProducesResponseType(typeof(Dictionary<string, SleepRecordDto?>), StatusCodes.Status200OK)]
    [MapToApiVersion("1.0")]
    [MapToApiVersion("2.0")]
    public async Task<IActionResult> GetWeekly()
    {
        var today = DateOnly.FromDateTime(DateTime.Now);

        // Calcula quantos dias voltar para chegar na segunda-feira
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
    /// Cria um novo registro de sono para o dia atual.
    /// </summary>
    /// <remarks>
    /// <b>Exemplo de requisição:</b>
    ///
    ///     POST /api/v1/sleep
    ///     {
    ///        "hours": 7.5,
    ///        "quality": "Good"
    ///     }
    ///
    /// <b>Exemplo de resposta:</b>
    /// ```json
    /// {
    ///   "id": 8,
    ///   "hours": 7.5,
    ///   "quality": "Good",
    ///   "createdAt": "2025-01-10T08:30:00Z"
    /// }
    /// ```
    /// </remarks>
    /// <param name="dto">Dados do registro de sono.</param>
    /// <returns>O registro recém-criado.</returns>
    /// <response code="201">Registro criado com sucesso.</response>
    /// <response code="400">Payload inválido.</response>
    [HttpPost]
    [ProducesResponseType(typeof(SleepRecordDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [MapToApiVersion("1.0")]
    [MapToApiVersion("2.0")]
    public async Task<IActionResult> Post([FromBody] CreateSleepRecordDto dto)
    {
        var record = await _sleepService.AddRecordAsync(dto.Hours, dto.Quality);
        var resultDto = _mapper.Map<SleepRecordDto>(record);

        return CreatedAtAction(nameof(GetToday), new { id = resultDto.Id }, resultDto);
    }

    /// <summary>
    /// Atualiza um registro de sono existente pelo ID.
    /// </summary>
    /// <remarks>
    /// <b>Exemplo de requisição:</b>
    ///
    ///     PUT /api/v1/sleep/10
    ///     {
    ///         "hours": 6.0,
    ///         "quality": "Poor"
    ///     }
    /// </remarks>
    /// <param name="id">ID do registro de sono.</param>
    /// <param name="dto">Dados atualizados.</param>
    /// <returns>O registro atualizado.</returns>
    /// <response code="200">Registro atualizado com sucesso.</response>
    /// <response code="404">Registro não encontrado.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(SleepRecordDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [MapToApiVersion("1.0")]
    [MapToApiVersion("2.0")]
    public async Task<IActionResult> Put(int id, [FromBody] UpdateSleepRecordDto dto)
    {
        var updated = await _sleepService.UpdateRecordAsync(id, dto.Hours, dto.Quality);
        if (updated == null)
            return NotFound();

        return Ok(_mapper.Map<SleepRecordDto>(updated));
    }

    /// <summary>
    /// Exclui um registro de sono pelo ID.
    /// </summary>
    /// <remarks>
    /// <b>Exemplo de requisição:</b>
    ///
    ///     DELETE /api/v1/sleep/6
    /// </remarks>
    /// <param name="id">ID do registro.</param>
    /// <response code="204">Registro excluído com sucesso.</response>
    /// <response code="404">Registro não encontrado.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [MapToApiVersion("1.0")]
    [MapToApiVersion("2.0")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _sleepService.DeleteRecordAsync(id);
        if (!deleted)
            return NotFound();

        return NoContent();
    }
}
