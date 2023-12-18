﻿namespace PestKit.Interfaces
{
    public interface IEmailService
    {
        Task SendMailAsync(string emailTo, string subject, string body, bool IsHtml = false);
    }
}
