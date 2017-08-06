﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Services.Services.Base;
using MoneyChest.Data.Context;
using MoneyChest.Data.Entities;
using System.Linq.Expressions;

namespace MoneyChest.Services.Services
{
    public interface IUserService : IBaseHistoricizedService<User>
    {
    }

    public class UserService : BaseHistoricizedService<User>, IUserService
    {
        public UserService(ApplicationDbContext context) : base(context)
        {
        }

        protected override int UserId(User entity) => entity.Id;

        protected override Expression<Func<User, bool>> LimitByUser(int userId) => item => item.Id == userId;
    }
}
