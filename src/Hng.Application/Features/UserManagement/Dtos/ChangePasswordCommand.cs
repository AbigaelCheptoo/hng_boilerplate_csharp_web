﻿using CSharpFunctionalExtensions;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Hng.Application.Features.UserManagement.Dtos
{
    public record ChangePasswordCommand : IRequest<Result<ChangePasswordResponse>>
    {
        [JsonPropertyName("old_password")]
        public string OldPassword { get; set; } //relaxed because of people that signed in via google

        [Required(ErrorMessage = "New Password is required")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
        [RegularExpression(@"^(?=.*[a-zA-Z])(?=.*\d).{8,}$",
            ErrorMessage = "Password must contain at least one letter and one number")]
        [JsonPropertyName("new_password")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [Compare("NewPassword")]
        [JsonPropertyName("confirm_new_password")]
        public string ConfirmNewPassword { get; set; }
    }

    public record ChangePasswordResponse
    {
        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("status_code")]
        public int StatusCode { get; set; }

        [JsonPropertyName("data")]
        public ChangePasswordData Data { get; set; }
    }

    public record ChangePasswordData
    {
        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
}