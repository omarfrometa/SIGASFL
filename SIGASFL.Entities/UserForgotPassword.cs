﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace SIGASFL.Entities
{
    public partial class UserForgotPassword
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Link { get; set; }
        public string Code { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool Verified { get; set; }
        public DateTime? VerifiedDate { get; set; }

        public virtual Users User { get; set; }
    }
}