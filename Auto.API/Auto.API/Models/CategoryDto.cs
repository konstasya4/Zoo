using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Auto.API.Models;

public class CategoryDto {

    [HiddenInput(DisplayValue = false)]
    public string AnimalCode { get; set; }

    private string code;

    private static string NormalizeCode(string cd) {
        return cd == null ? cd : Regex.Replace(cd.ToUpperInvariant(), "[^A-Z0-9]", "");
    }

    [Required]
    [DisplayName("Code number")]
    public string Code
    {
        get => NormalizeCode(code);
        set => code = value;
    }


    [Required]
    [DisplayName("Title")]
    public string Title { get; set; }
}