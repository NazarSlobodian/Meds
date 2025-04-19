using Humanizer.Bytes;
using Meds.Server.Models.DbModels;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Data;
using System.Text;

public class ExportService
{
   
    private readonly Wv1Context _context;
    private readonly ActivityLoggerService _activityLoggerService;
    public ExportService(Wv1Context context, ActivityLoggerService activityLoggerService)
    {
        _context = context;
        _activityLoggerService = activityLoggerService;
    }

    public async Task<byte[]> GetBatchesJson()
    {
        Wv1Context newContext = new Wv1Context();
        using var connection = newContext.Database.GetDbConnection();
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = "CALL exportMostBatchesDataAsJSON()";
        command.CommandType = CommandType.Text;

        using var reader = await command.ExecuteReaderAsync();

        string json = null;

        if (await reader.ReadAsync())
        {
            json = reader.GetString(0); // Assuming your SP returns a single column of JSON
        }
        else
            throw new Exception("Couldn't read");

        byte[] bytes = Encoding.UTF8.GetBytes(json);
        await _activityLoggerService.Log("Requesting batches json", null, null, "success");
        return bytes;

    }
}