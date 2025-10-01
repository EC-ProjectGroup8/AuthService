using System;
using AuthServices.Data.Context;
using AuthServices.Data.Dto;
using AuthServices.Data.Entities;
using AuthServices.Data.Interfaces;
using AuthServices.Models;
using AuthServices.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using static System.Net.WebRequestMethods;

namespace AuthServices.Services;

public class UserService(IUserRepository repository, UserManager<UsersEntity> userManager, IEmailSender emailSender) : IUserService
{
    private readonly IUserRepository _repository = repository;
    private readonly UserManager<UsersEntity> _userManager = userManager;
    private readonly IEmailSender _emailSender = emailSender;

    public async Task<bool> CreateUser(RegisterDto model)
    {
        if (model == null) return false;
        var existing = await _userManager.FindByEmailAsync(model.Email);
        if (existing != null) return false;

        var user = new UsersEntity
        {
            UserName = model.Email,
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName
        };
        var result = await _userManager.CreateAsync(user, model.Password);
        return result.Succeeded;

    }

    public async Task<UserReturnData?> GetUserById(string id)
    {
        if (string.IsNullOrEmpty(id)) return null;
        var existing = await _userManager.FindByIdAsync(id);
        if (existing == null) return null;
        var result = new UserReturnData
        {
            Id = existing.Id,
            Email = existing.Email!,
            FirstName = existing.FirstName,
            LastName = existing.LastName
        };
        return result;

    }

    public async Task<bool> UserExistsByEmailAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email)) return false;
        var user = await _userManager.FindByEmailAsync(email.Trim());
        return user != null;
    }

    public async Task<UsersEntity?> GetUserByEmail(string email)
    {
        if (string.IsNullOrEmpty(email)) return null;

        var user = await _userManager.FindByEmailAsync(email);
        return user;
    }


    public async Task<IEnumerable<UserReturnData>> GetAllUsers()
    {
        return await _userManager.Users
            .Select(user => new UserReturnData
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email!
            })
            .ToListAsync();
    }


    public async Task<bool> UpdateUser(UsersEntity entity)
    {
        if (entity == null) return false;
        var existing = await _userManager.FindByIdAsync(entity.Id);
        if (existing == null) return false;
        existing.FirstName = entity.FirstName;
        existing.LastName = entity.LastName;
        existing.Email = entity.Email;
        existing.UserName = entity.Email;
        var result = await _userManager.UpdateAsync(existing);
        if (!result.Succeeded) return false;
        else return true;
    }

    public async Task<bool> DeleteUser(string id)
    {
        if (string.IsNullOrEmpty(id)) return false;
        var existing = await _userManager.FindByIdAsync(id);
        if (existing == null) return false;
        var result = await _userManager.DeleteAsync(existing);
        return result.Succeeded;
    }

    public async Task RequestPasswordReset(string email)
    {
        try
        {
            if (string.IsNullOrEmpty(email))
                throw new ArgumentException("Email cannot be null or empty.", nameof(email));

            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var baseUrl = "https://gentle-sky-07e989710.2.azurestaticapps.net";
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var url = $"{baseUrl}/glomt-losenord?email={Uri.EscapeDataString(email)}&token={Uri.EscapeDataString(token)}";
                await _emailSender.SendResetLinkAsync(email, url);
                return;
            }
            else
                throw new ArgumentException("User was not found");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in PasswordReset: {ex.Message}");
            return;
        }
    }

    public async Task<bool> ChangeForgottenPassword(ResetPasswordRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
            return false;

        var decodedToken = Uri.UnescapeDataString(request.ResetCode);
        var result = await _userManager.ResetPasswordAsync(user, decodedToken, request.NewPassword);

        if (result.Succeeded)
        {
            await _emailSender.SendResetSuccessAsync(request.Email);
            return true;
        } else
            return false;
    }
}
