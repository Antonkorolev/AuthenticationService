using BackendService.BusinessLogic.Exceptions.Abstraction;

namespace BackendService.BusinessLogic.Exceptions;

public class UserNotFoundException(string message) : AuthenticationException(message);