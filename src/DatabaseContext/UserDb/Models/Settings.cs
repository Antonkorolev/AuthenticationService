using System.ComponentModel.DataAnnotations;

namespace DatabaseContext.UserDb.Models;

public sealed class Settings
{
    [Key]
    public int SettingId { get; set; }

    public string Key { get; set; } = default!;

    public string Value { get; set; } = default!;
}