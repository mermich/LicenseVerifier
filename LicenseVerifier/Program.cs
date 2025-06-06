WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

string[] clesValides = new[]
{
    "toto", "toto1", "toto2"
};


app.MapGet("/verify/{cle}", (string cle) =>
{
    if (clesValides.Contains(decryptKey(cle)))
    {
        return Results.Ok("Clé valide");
    }
    return Results.BadRequest("Clé invalide");
});



string decryptKey(string encryptedKey)
{
    // Simulate decryption logic here
    // In a real application, you would implement actual decryption logic
    return encryptedKey; // For simplicity, returning the same key
}

app.Run();