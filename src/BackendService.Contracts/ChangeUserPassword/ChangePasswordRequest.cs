namespace BackendService.Contracts.ChangeUserPassword;

public sealed class ChangePasswordRequest(string login, string oldPassword, string newPassword)
{
    public string Login { get; set; } = login;

    public string OldPassword { get; set; } = oldPassword;

    public string NewPassword { get; set; } = newPassword;
}