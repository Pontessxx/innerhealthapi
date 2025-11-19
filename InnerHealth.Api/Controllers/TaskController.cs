using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using InnerHealth.Api.Dtos;
using InnerHealth.Api.Services;

namespace InnerHealth.Api.Controllers;

/// <summary>
/// Fornece endpoints para gerenciar tarefas diárias, incluindo criação,
/// atualização, marcação de conclusão e listagem.
/// </summary>
/// <remarks>
/// Este controlador permite criar, consultar, atualizar e excluir tarefas,
/// além de consultar as tarefas do dia.
/// </remarks>
[ApiController]
[Route("api/v{version:apiVersion}/tasks")]
public class TaskController : ControllerBase
{
    private readonly ITaskService _taskService;
    private readonly IMapper _mapper;

    public TaskController(ITaskService taskService, IMapper mapper)
    {
        _taskService = taskService;
        _mapper = mapper;
    }

    /// <summary>
    /// Retorna todas as tarefas agendadas para o dia atual.
    /// </summary>
    /// <remarks>
    /// <b>Exemplo de requisição:</b>
    ///
    ///     GET /api/v1/tasks/today
    ///
    /// <b>Exemplo de resposta:</b>
    /// ```json
    /// [
    ///   {
    ///     "id": 1,
    ///     "title": "Morning workout",
    ///     "description": "30 minutes of cardio",
    ///     "date": "2025-01-10",
    ///     "priority": "High",
    ///     "isComplete": false
    ///   }
    /// ]
    /// ```
    /// </remarks>
    /// <returns>Lista de tarefas do dia.</returns>
    /// <response code="200">Retorna as tarefas do dia atual.</response>
    [HttpGet("today")]
    [ProducesResponseType(typeof(IEnumerable<TaskItemDto>), StatusCodes.Status200OK)]
    [MapToApiVersion("1.0")]
    [MapToApiVersion("2.0")]
    public async Task<IActionResult> GetToday()
    {
        var date = DateOnly.FromDateTime(DateTime.Now);
        var tasks = await _taskService.GetTasksAsync(date);

        return Ok(_mapper.Map<IEnumerable<TaskItemDto>>(tasks));
    }

    /// <summary>
    /// Retorna todas as tarefas cadastradas no sistema.
    /// </summary>
    /// <remarks>
    /// <b>Exemplo de resposta:</b>
    /// ```json
    /// [
    ///   {
    ///     "id": 1,
    ///     "title": "Drink water",
    ///     "description": "Finish 2L today",
    ///     "date": "2025-01-10",
    ///     "priority": "Medium",
    ///     "isComplete": true
    ///   },
    ///   {
    ///     "id": 2,
    ///     "title": "Study React Native",
    ///     "description": null,
    ///     "date": "2025-01-11",
    ///     "priority": "High",
    ///     "isComplete": false
    ///   }
    /// ]
    /// ```
    /// </remarks>
    /// <returns>Lista de todas as tarefas.</returns>
    /// <response code="200">Retorna todas as tarefas cadastradas.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<TaskItemDto>), StatusCodes.Status200OK)]
    [MapToApiVersion("1.0")]
    [MapToApiVersion("2.0")]
    public async Task<IActionResult> GetAll()
    {
        var tasks = await _taskService.GetAllTasksAsync();

        return Ok(_mapper.Map<IEnumerable<TaskItemDto>>(tasks));
    }

    /// <summary>
    /// Cria uma nova tarefa.
    /// </summary>
    /// <remarks>
    /// <b>Exemplo de requisição:</b>
    ///
    ///     POST /api/v1/tasks
    ///     {
    ///         "title": "Buy groceries",
    ///         "description": "Eggs, rice, fruits",
    ///         "date": "2025-01-10",
    ///         "priority": "Low"
    ///     }
    ///
    /// <b>Exemplo de resposta:</b>
    /// ```json
    /// {
    ///   "id": 3,
    ///   "title": "Buy groceries",
    ///   "description": "Eggs, rice, fruits",
    ///   "date": "2025-01-10",
    ///   "priority": "Low",
    ///   "isComplete": false
    /// }
    /// ```
    /// </remarks>
    /// <param name="dto">Dados para criação da tarefa.</param>
    /// <returns>A tarefa criada.</returns>
    /// <response code="201">Tarefa criada com sucesso.</response>
    /// <response code="400">Dados inválidos fornecidos.</response>
    [HttpPost]
    [ProducesResponseType(typeof(TaskItemDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [MapToApiVersion("1.0")]
    [MapToApiVersion("2.0")]
    public async Task<IActionResult> Post([FromBody] CreateTaskItemDto dto)
    {
        var task = await _taskService.AddTaskAsync(dto.Title!, dto.Description, dto.Date, dto.Priority);

        var resultDto = _mapper.Map<TaskItemDto>(task);

        return CreatedAtAction(nameof(GetAll), new { id = resultDto.Id }, resultDto);
    }

    /// <summary>
    /// Atualiza uma tarefa existente.
    /// </summary>
    /// <remarks>
    /// <b>Exemplo de requisição:</b>
    ///
    ///     PUT /api/v1/tasks/5
    ///     {
    ///         "title": "Study React Native",
    ///         "description": "Finish layout refactor",
    ///         "date": "2025-01-11",
    ///         "isComplete": true,
    ///         "priority": "High"
    ///     }
    /// </remarks>
    /// <param name="id">ID da tarefa a ser atualizada.</param>
    /// <param name="dto">Dados atualizados da tarefa.</param>
    /// <returns>A tarefa atualizada.</returns>
    /// <response code="200">Tarefa atualizada com sucesso.</response>
    /// <response code="404">Tarefa não encontrada.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(TaskItemDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [MapToApiVersion("1.0")]
    [MapToApiVersion("2.0")]
    public async Task<IActionResult> Put(int id, [FromBody] UpdateTaskItemDto dto)
    {
        var updated = await _taskService.UpdateTaskAsync(
            id,
            dto.Title!,
            dto.Description,
            dto.Date,
            dto.IsComplete,
            dto.Priority
        );

        if (updated == null)
            return NotFound();

        return Ok(_mapper.Map<TaskItemDto>(updated));
    }

    /// <summary>
    /// Exclui uma tarefa pelo ID.
    /// </summary>
    /// <remarks>
    /// <b>Exemplo de requisição:</b>
    ///
    ///     DELETE /api/v1/tasks/7
    /// </remarks>
    /// <param name="id">ID da tarefa a ser excluída.</param>
    /// <response code="204">Tarefa excluída com sucesso.</response>
    /// <response code="404">Tarefa não encontrada.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [MapToApiVersion("1.0")]
    [MapToApiVersion("2.0")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _taskService.DeleteTaskAsync(id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}
