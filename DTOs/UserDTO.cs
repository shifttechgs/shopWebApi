namespace ShopWebApi.DTOs;

public class UserDTO
{
   public string firstname { get; set; }

    public string lastname { get; set; }
    
    
    public string Email { get; set; }
    
    public string Password { get; set; }
   
    public string ConfirmPassword { get; set; }
}