using System.ComponentModel.DataAnnotations;

namespace HttpApi;

public class ControllerOptions
{
    public const string Section = "ControllerOptions";

    [Required]
    public int? ConfiguredValue { get; set; }
}