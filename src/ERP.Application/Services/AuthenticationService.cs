using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Ardalis.Result;
using Ardalis.Result.FluentValidation;
using ERP.Application.Interfaces;
using ERP.Application.Requests.AuthenticationRequests;
using ERP.Application.Requests.UsersRequest;
using ERP.Application.Responses;
using ERP.Domain.Entities;
using ERP.Domain.Repositories;
using ERP.Shared.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace ERP.Application.Services;

public class AuthenticationService : IAuthenticationService
{

    #region Constructor

    public AuthenticationService
        (
        IDateTimeService dateTimeService,
        ITokenClaimsService tokenClaimsService,
        IUserRepository repository,
        IUnitOfWork uow,
        IHashService hashService
        )
    {
        _dateTimeService = dateTimeService;
        _tokenClaimsService = tokenClaimsService;
        _repository = repository;
        _uow = uow;
        _hashService = hashService;
    }

    #endregion

    #region Fields

    private readonly IDateTimeService _dateTimeService;
    private readonly ITokenClaimsService _tokenClaimsService;
    private readonly IUserRepository _repository;
    private readonly IUnitOfWork _uow;
    private readonly IHashService _hashService;

    #endregion


    #region Methods

    public async Task<Result<TokenResponse>> AuthenticateAsync(LogInRequest request)
    {
        await request.ValidateAsync();
        if (!request.IsValid)
            return Result.Invalid(request.ValidationResult.AsErrors());

        var user = await _repository.GetUserByEmail(request.Email);


        if (user == null)
        {
            return Result.NotFound("Nenhum usuário encontrado");
        }

        if (_hashService.Compare(request.Password, user.Password))
        {
            var claims = GenerateClaims(user);

            var (accessToken, createdAt, expiresAt) = _tokenClaimsService.GenerateAccessToken(claims);

            var refreshToken = _tokenClaimsService.GenerateRefreshToken();

            user.AddToken(accessToken, refreshToken, expiresAt);
            _repository.Update(user);
            await _uow.CommitAsync();

            return Result.Success(new TokenResponse(accessToken, createdAt, expiresAt, refreshToken));
        }

        return Result.Unauthorized();
    }
    public async Task<Result<UserResponse>> CreateUser(SignupRequest request)
    {
        await request.ValidateAsync();

        if (!request.IsValid)
            return Result.Invalid(request.ValidationResult.AsErrors());

        var newUser = new User(request.Email, request.Name, _hashService.Hash(request.Password), request.ProfileId);

        _repository.Add(newUser);

        try
        {

            await _uow.CommitAsync();

            var userResponse = new UserResponse(newUser.Id, newUser.Email, newUser.Username);

            return Result.Success(userResponse);

        }
        catch (DbUpdateException ex)
        {
            var innerException = ex.InnerException;

            // Verifique se a exceção interna é do tipo Npgsql.PostgresException e se é uma violação de chave única
            if (innerException is Npgsql.PostgresException postgresException && postgresException.SqlState == "23505")
            {
                return Result.Error(postgresException.MessageText);
            }

            // Se não for uma violação de chave única, relançar a exceção
            throw;
        }

    }

    private static Claim[] GenerateClaims(User user) => new[]
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
        new Claim(JwtRegisteredClaimNames.UniqueName, user.Id.ToString()),
        new Claim(JwtRegisteredClaimNames.Sub, user.Username, ClaimValueTypes.String),
        new Claim(JwtRegisteredClaimNames.Email, user.Email.ToString(), ClaimValueTypes.Email)
    };

    #endregion

}
