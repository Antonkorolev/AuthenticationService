using System.Security.Authentication;

namespace BackendService.BusinessLogic.Exceptions;

public sealed class UserVerifiedException(string message) : AuthenticationException(message);