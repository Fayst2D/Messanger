﻿namespace Messenger.BusinessLogic.Models;

public class Contact
{
    public Guid ContactId { get; set; }
    public string Username { get; init; } = "";
    public string Email { get; init; } = "";
}