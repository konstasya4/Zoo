using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Auto.API.Models;

public class ProductDto {

    [HiddenInput(DisplayValue = false)]
    public string CategoryCode { get; set; }

    private string serial;

    private static string NormalizeSerial(string ser) {
        return ser == null ? ser : Regex.Replace(ser.ToUpperInvariant(), "[^A-Z0-9]", "");
    }

    [Required]
    [DisplayName("Serial number")]
    public string Serial{
        get => NormalizeSerial(serial);
        set => serial = value;
    }

    [Required]
    [DisplayName("Price of product")]
    public string Price { get; set; }

    [Required]
    [DisplayName("Title")]
    public string Title { get; set; }
}