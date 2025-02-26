using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace StockFlow.Domain.Entities;

public class Administrators
{
    private const string PasswordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{9,}$";
    private string _password;

    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }

    [RegularExpression(PasswordPattern, 
        ErrorMessage = "{0} must contain at least 9 characters, an uppercase letter, a lowercase letter, a number, and a special character.")]
    public string Password
    {
        get => _password;
        set
        {
            if (string.IsNullOrWhiteSpace(value) || !Regex.IsMatch(value, PasswordPattern))
            {
                throw new ArgumentException($"{nameof(Password)} does not meet complexity requirements.");
            }
            _password = value;
        }
    }
}
