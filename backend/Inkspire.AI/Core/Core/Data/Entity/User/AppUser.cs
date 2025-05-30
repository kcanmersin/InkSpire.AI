﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core.Data.Entity.User
{
    public class AppUser : IdentityUser<Guid>
    {
        public override Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public override string PasswordHash { get; set; } = string.Empty;
        public bool IsConfirmed { get; set; } = false;
        public bool IsTwoFactorEnabled { get; set; } = false;
        public string TwoFactorCode { get; set; } = string.Empty;
        public DateTime? TwoFactorExpiryTime { get; set; }
        public string NativeLanguage { get; set; } = string.Empty;
        public string TargetLanguage { get; set; } = string.Empty;
        public List<Word> Words { get; set; } = new List<Word>();
        public string? ProfileImageUrl { get; set; }
        public int Point { get; set; } = 0;

        public string? ActiveRoomId { get; set; }

    }


}