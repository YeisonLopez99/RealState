﻿namespace Domain.Entities;

public class RefreshToken
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public string TokenHash { get; set; } = default!;
    public DateTime Expires { get; set; }
    public bool Revoked { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}