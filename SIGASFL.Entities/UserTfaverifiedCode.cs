﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace SIGASFL.Entities
{
    public partial class UserTfaverifiedCode
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public int Tfaid { get; set; }
        public string Token { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool Verified { get; set; }
        public DateTime? VerifiedDate { get; set; }

        public virtual TwoFactorAuthenticator Tfa { get; set; }
        public virtual Users User { get; set; }
    }
}