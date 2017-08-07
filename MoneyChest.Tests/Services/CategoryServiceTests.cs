﻿using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyChest.Data.Entities;
using MoneyChest.Data.Entities.History;
using MoneyChest.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Tests.Services
{
    [TestClass]
    public class CategoryServiceTests : IdManageableUserableHistoricizedServiceTestBase<Category, CategoryService, CategoryHistory>
    {
        #region Overrides 

        protected override void ChangeEntity(Category entity) => entity.Name = "Some other name";
        protected override void SetUserId(Category entity, int userId) => entity.UserId = userId;

        #endregion
    }
}
