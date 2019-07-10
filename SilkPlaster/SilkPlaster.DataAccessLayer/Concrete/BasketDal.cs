﻿using SilkPlaster.DataAccessLayer.Abstract;
using SilkPlaster.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilkPlaster.DataAccessLayer.Concrete
{
    public class BasketDal : EntityRepository<Basket>, IBasketDal
    {
        public void DeleteRange(List<int> IdList)
        {
            foreach (var item in IdList)
            {
                Basket basket = Find(i => i.Id == item);
                Delete(basket);
            }
        }

        public List<Basket> GetBasketItemsByMemberId(int memberId)
        {
            return ListQueryable()
                .Include("Product")
                .Include("Member")
                .Where(i => i.MemberId == memberId)
                .ToList();
        }
    }
}
