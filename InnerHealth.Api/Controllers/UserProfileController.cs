using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using InnerHealth.Api.Dtos;
using InnerHealth.Api.Services;
using InnerHealth.Api.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace InnerHealth.Api.Controllers;

/// <summary>
/// Fornece endpoints para obter e atualizar o perfil do usuário.
/// Embora a aplicação atual utilize apenas um perfil, a API foi projetada
/// para dar suporte a múltiplos usuários no futuro.
/// </summary>
/// <remarks>
/// Este controlador permite recuperar o perfil atual e atualizar seus dados,
/// incluindo informações de saúde, objetivos e métricas pessoais.
/// </remarks>
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

    /// <summary>
    /// Retorna o perfil atual do usuário.
    /// </summary>
    /// <remarks>
    /// <b>Exemplo de requisição:</b>
    ///
    ///     GET /api/v1/profile
    ///
    /// <b>Exemplo de resposta (perfil existente):</b>
    /// ```json
    /// {
    ///   "id": 1,
    ///   "name": "Henrique Pontes",
    ///   "age": 21,
    ///   "height": 1.78,
    ///   "weight": 75.2,
    ///   "goal": "Gain muscle"
    /// }
    /// ```
    ///
    /// <b>Exemplo de resposta (sem perfil ainda):</b>
    /// ```json
    /// null
    /// ```
    /// </remarks>
    /// <returns>O perfil do usuário, ou null caso não exista.</returns>
    /// <response code="200">Retorna o perfil do usuário ou null.</response>
    [HttpGet]
    [ProducesResponseType(typeof(UserProfileDto), StatusCodes.Status200OK)]
    [MapToApiVersion("1.0")]
    [MapToApiVersion("2.0")]
    [SwaggerOperation(
        Summary = "Retorna o perfil atual do usuário.",
        Description = "Retorna todas as informações do perfil do usuário, incluindo nome, idade, altura, peso e objetivo. Caso nenhum perfil exista ainda, retorna null."
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Perfil retornado com sucesso ou null caso não exista.", typeof(UserProfileDto))]

    public async Task<IActionResult> Get()
    {
        var user = await _userService.GetUserAsync();

        if (user == null)
            return Ok((UserProfileDto?)null);

        return Ok(_mapper.Map<UserProfileDto>(user));
    }

    /// <summary>
    /// Atualiza o perfil do usuário com novas informações.
    /// </summary>
    /// <remarks>
    /// <b>Exemplo de requisição:</b>
    ///
    ///     PUT /api/v1/profile
    ///     {
    ///       "name": "Henrique Pontes",
    ///       "age": 21,
    ///       "height": 1.78,
    ///       "weight": 75.2,
    ///       "goal": "Improve sleep"
    ///     }
    ///
    /// <b>Exemplo de resposta:</b>
    /// ```json
    /// {
    ///   "id": 1,
    ///   "name": "Henrique Pontes",
    ///   "age": 21,
    ///   "height": 1.78,
    ///   "weight": 75.2,
    ///   "goal": "Improve sleep"
    /// }
    /// ```
    /// </remarks>
    /// <param name="dto">Dados atualizados do perfil do usuário.</param>
    /// <returns>O perfil atualizado.</returns>
    /// <response code="200">Perfil atualizado com sucesso.</response>
    /// <response code="400">Payload inválido.</response>
    [HttpPut]
    [ProducesResponseType(typeof(UserProfileDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [MapToApiVersion("1.0")]
    [MapToApiVersion("2.0")]
    [SwaggerOperation(
        Summary = "Atualiza o perfil do usuário.",
        Description = "Atualiza os dados do perfil, como nome, idade, altura, peso e objetivo. O perfil é criado caso ainda não exista."
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Perfil atualizado com sucesso.", typeof(UserProfileDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Payload inválido — verifique campos obrigatórios.")]

    public async Task<IActionResult> Put([FromBody] UpdateUserProfileDto dto)
    {
        var user = await _userService.UpdateUserAsync(dto);

        return Ok(_mapper.Map<UserProfileDto>(user));
    }
}
