using SQLite;

namespace DesenMobileDatabase.Models;

[Table("SavedUsers")]
public class SavedUserModel : BaseModel
{
    [MaxLength(250)]
    public string Description { get; set; }

    [Ignore]
    public string Desc => Description?.Length >= 2 ? Description.Substring(0, 2) : Description;

    [MaxLength(250)]
    public string Username { get; set; }

    [MaxLength(250)]
    public string Password { get; set; }

    public SavedUserModel()
    {
    }

    public SavedUserModel(string uname,string upass)
    {
        this.Username = uname;
        this.Password = upass.ToString();
    }
}

