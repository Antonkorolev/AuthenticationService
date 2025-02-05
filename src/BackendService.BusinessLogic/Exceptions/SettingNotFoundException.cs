using BackendService.BusinessLogic.Exceptions.Abstraction;

namespace BackendService.BusinessLogic.Exceptions;

public sealed class SettingNotFoundException (string param) : AuthenticationException($"Setting by key = '{param}' not found");