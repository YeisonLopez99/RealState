namespace Domain.Entities;

public class Owner
{
    public Guid IdOwner { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; } = default!;
    public string Email { get; private set; } = default!;
    public string PasswordHash { get; private set; } = default!; // stored PBKDF2 (salt+hash) base64
    public string Phone { get; private set; } = default!;
    public string Address { get; private set; } = default!;
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    private Owner() { }

    public Owner(string name, string email, string passwordHash, string phone, string address)
    {
        Name = name;
        Email = email;
        PasswordHash = passwordHash;
        Phone = phone;
        Address = address;
    }
}