﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace SIGASFL.Entities
{
    public partial class Users
    {
        public Users()
        {
            UserForgotPassword = new HashSet<UserForgotPassword>();
            UserProfile = new HashSet<UserProfile>();
            UserTfabackupCode = new HashSet<UserTfabackupCode>();
            UserTfacross = new HashSet<UserTfacross>();
            UserTfaverifiedCode = new HashSet<UserTfaverifiedCode>();
        }

        public string Id { get; set; }
        public int Seq { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public DateTime? EmailConfirmedDate { get; set; }
        public byte[] PasswordSalt { get; set; }
        public byte[] PasswordHash { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public DateTime? PhoneNumberConfirmedDate { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public string TwoFactorSecretKey { get; set; }
        public bool AllowAccessFailed { get; set; }
        public short? AccessFailedCount { get; set; }
        public string Picture { get; set; }
        public bool ChangePwdNextLogin { get; set; }
        public DateTime? LastPwdChangedDate { get; set; }
        public string DefaultLanguage { get; set; }
        public string ParentId { get; set; }
        public string DisplayName { get; set; }
        public bool Locked { get; set; }
        public DateTime? LockedDate { get; set; }
        public DateTime? LastAccessDate { get; set; }
        public string LastIpAddress { get; set; }
        public bool ProfileCompleted { get; set; }
        public DateTime? ProfileCompletedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual ICollection<UserForgotPassword> UserForgotPassword { get; set; }
        public virtual ICollection<UserProfile> UserProfile { get; set; }
        public virtual ICollection<UserTfabackupCode> UserTfabackupCode { get; set; }
        public virtual ICollection<UserTfacross> UserTfacross { get; set; }
        public virtual ICollection<UserTfaverifiedCode> UserTfaverifiedCode { get; set; }
    }
}