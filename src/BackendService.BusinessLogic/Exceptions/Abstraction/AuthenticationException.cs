namespace BackendService.BusinessLogic.Exceptions.Abstraction;

public abstract class AuthenticationException(string message) : Exception(message);