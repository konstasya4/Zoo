using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Auto.API.Models;

public class AnimalDto {

    private string code;

    private static string NormalizeCode(string cod) {
        return cod == null ? cod : Regex.Replace(cod.ToUpperInvariant(), "[^A-Z0-9]", "");
    }

    [Required]
    [DisplayName("Code")]
    public string Code{
        get => NormalizeCode(code);
        set => code = value;
    }


    [Required]
    [DisplayName("Animal")]
    public string Title { get; set; }
}