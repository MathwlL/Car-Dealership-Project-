using Microsoft.EntityFrameworkCore;

namespace CarMark.Data;

public static class DatabaseInitializer
{
    public static async Task InitializeAsync(CarMarkDbContext database)
    {
        await database.Database.EnsureCreatedAsync();
        await using var connection = database.Database.GetDbConnection();
        await connection.OpenAsync();
        await using var command = connection.CreateCommand();
        command.CommandText = "PRAGMA table_info('Cars');";
        await using var reader = await command.ExecuteReaderAsync();
        var hasCurrency = false;
        while (await reader.ReadAsync())
        {
            if (string.Equals(reader.GetString(1), "Currency", StringComparison.OrdinalIgnoreCase)) { hasCurrency = true; break; }
        }
        await reader.CloseAsync();
        if (!hasCurrency)
        {
            command.CommandText = "ALTER TABLE \"Cars\" ADD COLUMN \"Currency\" TEXT NOT NULL DEFAULT 'BRL';";
            await command.ExecuteNonQueryAsync();
        }

        command.CommandText = "PRAGMA table_info('Users');";
        await using var userReader = await command.ExecuteReaderAsync();
        var hasProfileImage = false;
        while (await userReader.ReadAsync())
        {
            if (string.Equals(userReader.GetString(1), "ProfileImage", StringComparison.OrdinalIgnoreCase)) { hasProfileImage = true; break; }
        }
        await userReader.CloseAsync();
        if (!hasProfileImage)
        {
            command.CommandText = "ALTER TABLE \"Users\" ADD COLUMN \"ProfileImage\" TEXT NULL;";
            await command.ExecuteNonQueryAsync();
        }

        foreach (var column in new[] { "ShowProfileImage", "ShowEmail", "ShowPhone", "ShowBirthDate" })
        {
            command.CommandText = "PRAGMA table_info('Users');";
            await using var privacyReader = await command.ExecuteReaderAsync();
            var exists = false;
            while (await privacyReader.ReadAsync())
            {
                if (string.Equals(privacyReader.GetString(1), column, StringComparison.OrdinalIgnoreCase)) { exists = true; break; }
            }
            await privacyReader.CloseAsync();
            if (!exists)
            {
                command.CommandText = $"ALTER TABLE \"Users\" ADD COLUMN \"{column}\" INTEGER NOT NULL DEFAULT 0;";
                await command.ExecuteNonQueryAsync();
            }
        }
    }
}
