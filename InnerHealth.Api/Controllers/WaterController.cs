using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using InnerHealth.Api.Dtos;
using InnerHealth.Api.Models;
using InnerHealth.Api.Services;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authorization;

namespace InnerHealth.Api.Controllers;

/// <summary>
/// Fornece endpoints para gerenciar registros de ingestão de água,
/// permitindo o acompanhamento diário e o resumo semanal de hidratação.
/// Suporta as versões de API 1 e 2 utilizando os mesmos métodos.
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
    /// Retorna as ingestões de água registradas hoje, incluindo todas as entradas,
    /// total consumido (em mL) e a quantidade recomendada.
    /// </summary>
    /// <remarks>
    /// <b>Exemplo de requisição:</b>
    ///
    ///     GET /api/v1/water/today
    ///
    /// <b>Exemplo de resposta:</b>
    /// ```json
    /// {
    ///   "date": "2025-01-10",
    ///   "totalMl": 1200,
    ///   "recommendedMl": 2000,
    ///   "entries": [
    ///       { "id": 1, "amountMl": 300, "createdAt": "2025-01-10T09:00:00Z" },
    ///       { "id": 2, "amountMl": 500, "createdAt": "2025-01-10T12:30:00Z" },
    ///       { "id": 3, "amountMl": 400, "createdAt": "2025-01-10T17:45:00Z" }
    ///   ]
    /// }
    /// ```
    /// </remarks>
    /// <returns>Resumo da ingestão de água do dia.</returns>
    /// <response code="200">Retorna as ingestões de água de hoje e os totais calculados.</response>
    [HttpGet("today")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [MapToApiVersion("1.0")]
    [MapToApiVersion("2.0")]
    [SwaggerOperation(
        Summary = "Retorna a ingestão de água registrada no dia atual.",
        Description = "Inclui todas as entradas do dia, o total consumido em mL e o valor recomendado."
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Resumo diário de ingestão retornado com sucesso.")]

    public async Task<IActionResult> GetToday()
    {
        var date = DateOnly.FromDateTime(DateTime.Now);

        var intakes     = await _waterService.GetIntakesAsync(date);
        var total       = await _waterService.GetDailyTotalAsync(date);
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
    /// Retorna o total de água consumida na semana atual (segunda a domingo).
    /// </summary>
    /// <remarks>
    /// A semana segue o padrão ISO-8601, iniciando sempre na **segunda-feira**.
    ///
    /// <b>Exemplo de resposta:</b>
    /// ```json
    /// {
    ///   "monday": 800,
    ///   "tuesday": 1200,
    ///   "wednesday": 1500,
    ///   "thursday": 900,
    ///   "friday": 2000,
    ///   "saturday": 0,
    ///   "sunday": 500
    /// }
    /// ```
    /// </remarks>
    /// <returns>Dicionário contendo o total ingerido em cada dia da semana.</returns>
    /// <response code="200">Retorna os totais semanais de hidratação.</response>
    [HttpGet("week")]
    [ProducesResponseType(typeof(Dictionary<string, int>), StatusCodes.Status200OK)]
    [MapToApiVersion("1.0")]
    [MapToApiVersion("2.0")]
    [SwaggerOperation(
         Summary = "Retorna o total de ingestão de água na semana atual.",
        Description = "A semana segue o padrão ISO-8601 (segunda a domingo). Retorna um dicionário com o total por dia."
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Totais semanais retornados com sucesso.", typeof(Dictionary<string, int>))]

    public async Task<IActionResult> GetWeekly()
    {
        var today = DateOnly.FromDateTime(DateTime.Now);

        // Converte Sunday=0 para 6 e Monday=1 para 0
        int diff = ((int)today.DayOfWeek + 6) % 7;

        var monday = today.AddDays(-diff);
        var totals = await _waterService.GetWeeklyTotalsAsync(monday);

        return Ok(totals);
    }

    /// <summary>
    /// Registra uma nova ingestão de água para o dia atual.
    /// </summary>
    /// <remarks>
    /// <b>Exemplo de requisição:</b>
    ///
    ///     POST /api/v1/water
    ///     {
    ///         "amountMl": 250
    ///     }
    ///
    /// <b>Exemplo de resposta:</b>
    /// ```json
    /// {
    ///   "id": 10,
    ///   "amountMl": 250,
    ///   "createdAt": "2025-01-10T14:10:00Z"
    /// }
    /// ```
    /// </remarks>
    /// <param name="dto">Informações da ingestão de água.</param>
    /// <returns>Registro criado da ingestão de água.</returns>
    /// <response code="201">Registro criado com sucesso.</response>
    /// <response code="400">Payload inválido.</response>
    [HttpPost]
    [ProducesResponseType(typeof(WaterIntakeDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [MapToApiVersion("1.0")]
    [MapToApiVersion("2.0")]
    [SwaggerOperation(
        Summary = "Registra uma nova ingestão de água.",
        Description = "Cria um novo registro de ingestão em mL associado automaticamente à data atual."
    )]
    [SwaggerResponse(StatusCodes.Status201Created, "Ingestão registrada com sucesso.", typeof(WaterIntakeDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Payload inválido — verifique os campos obrigatórios.")]

    public async Task<IActionResult> Post([FromBody] CreateWaterIntakeDto dto)
    {
        var intake = await _waterService.AddIntakeAsync(dto.AmountMl);
        var resultDto = _mapper.Map<WaterIntakeDto>(intake);

        return CreatedAtAction(nameof(GetToday), new { id = resultDto.Id }, resultDto);
    }

    /// <summary>
    /// Atualiza um registro de ingestão de água existente.
    /// </summary>
    /// <remarks>
    /// <b>Exemplo de requisição:</b>
    ///
    ///     PUT /api/v1/water/5
    ///     {
    ///         "amountMl": 400
    ///     }
    /// </remarks>
    /// <param name="id">ID do registro de ingestão.</param>
    /// <param name="dto">Novo valor atualizado.</param>
    /// <returns>Registro atualizado.</returns>
    /// <response code="200">Atualizado com sucesso.</response>
    /// <response code="404">Registro não encontrado.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(WaterIntakeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [MapToApiVersion("1.0")]
    [MapToApiVersion("2.0")]
    [SwaggerOperation(
        Summary = "Atualiza um registro de ingestão de água.",
        Description = "Permite atualizar a quantidade ingerida em mL para um registro já existente."
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Registro de ingestão atualizado com sucesso.", typeof(WaterIntakeDto))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Registro não encontrado.")]

    public async Task<IActionResult> Put(int id, [FromBody] UpdateWaterIntakeDto dto)
    {
        var updated = await _waterService.UpdateIntakeAsync(id, dto.AmountMl);

        if (updated == null)
            return NotFound();

        return Ok(_mapper.Map<WaterIntakeDto>(updated));
    }

    /// <summary>
    /// Exclui um registro de ingestão de água pelo ID.
    /// </summary>
    /// <remarks>
    /// <b>Exemplo de requisição:</b>
    ///
    ///     DELETE /api/v1/water/8
    /// </remarks>
    /// <param name="id">ID do registro.</param>
    /// <response code="204">Registro excluído com sucesso.</response>
    /// <response code="404">Registro não encontrado.</response>
    [Authorize]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [MapToApiVersion("1.0")]
    [MapToApiVersion("2.0")]
    [SwaggerOperation(
        Summary = "Exclui um registro de ingestão de água pelo ID.",
        Description = "Remove permanentemente o registro especificado, caso exista."
    )]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Registro excluído com sucesso.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Registro não encontrado.")]

    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _waterService.DeleteIntakeAsync(id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}
