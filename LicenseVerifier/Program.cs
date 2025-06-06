using System.Security.Cryptography;
using System.Text;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();


app.MapGet("/verify/{cle}", (string cle) =>
{
    License? license = decryptKey(cle);
    if (license == null)
    {
        return Results.BadRequest("Clé invalide ou mal formée");
    }
    else if (license.IsValid())
    {
        return Results.Ok("Clé valide");
    }
    else
    {
        return Results.BadRequest("Clé invalide");
    }
});



License? decryptKey(string rawData)
{
    string decryptedData = "";
    using (SHA256 sha256Hash = SHA256.Create())
    {
        byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
        StringBuilder builder = new();
        foreach (byte b in bytes)
        {
            _ = builder.Append(b.ToString("x2"));
        }
        decryptedData = builder.ToString();
    }

    return System.Text.Json.JsonSerializer.Deserialize<License>(decryptedData);
}


app.Run();

public class License
{
    public int Version { get; set; }
    public required string Key { get; set; }

    private static readonly string[] clesValides = new[]
    {
        "toto", "toto1", "toto2"
    };

    public bool IsValid()
    {
        if (Version == 1)
        {
            if (clesValides.Contains(Key))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (Version == 2)
        {
            /// un truc plus complexe...
            return false;
        }
        else
        {
            return false;
        }
    }
}


public class CoteClient
{
    // fonction de chiffrage SHA256
    static string ComputeSha256Hash(string rawData)
    {
        using (SHA256 sha256Hash = SHA256.Create())
        {
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
            StringBuilder builder = new StringBuilder();
            foreach (byte b in bytes)
            {
                builder.Append(b.ToString("x2"));
            }
            return builder.ToString();
        }
    }
}
