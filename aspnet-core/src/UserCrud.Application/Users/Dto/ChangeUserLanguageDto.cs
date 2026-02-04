using System.ComponentModel.DataAnnotations;

namespace UserCrud.Users.Dto;

public class ChangeUserLanguageDto
{
    [Required]
    public string LanguageName { get; set; }
}