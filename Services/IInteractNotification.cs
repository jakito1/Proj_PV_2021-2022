﻿using NutriFitWeb.Models;

namespace NutriFitWeb.Services
***REMOVED***
    public interface IInteractNotification
    ***REMOVED***
        Task Create(string? notificationMessage, UserAccountModel notificationReceiver);
        Task<List<Notification>> GetLastThree(string? userName);

        Task<bool> NotificationsExist(string? userName);
***REMOVED***
***REMOVED***
