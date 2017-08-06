using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Services.Services.Base;
using MoneyChest.Data.Context;
using MoneyChest.Data.Entities;

namespace MoneyChest.Services.Services
{
    public interface ICategoryService : IBaseHistoricizedService<Category>
    {
    }

    public class CategoryService : BaseHistoricizedService<Category>, ICategoryService
    {
        public CategoryService(ApplicationDbContext context) : base(context)
        {
        }

        protected override int UserId(Category entity) => entity.UserId;
    }
}
