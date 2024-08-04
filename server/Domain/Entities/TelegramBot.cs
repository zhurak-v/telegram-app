namespace Domain.Entities;

public class TelegramBot
{
    public Guid Id { get; set; }
    public long BotId { get; set; }
    public DateTime CreatedAt { get; set; }
    public string EncryptedToken { get; set; } = null!;
}
