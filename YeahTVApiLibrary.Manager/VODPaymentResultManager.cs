using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApiLibrary.Infrastructure;
using YeahTVApi.Common;

namespace YeahTVApiLibrary.Manager
{

    public class VODPaymentResultManager : IVODPaymentResultManager
    {
        private IVODPaymentResultRepertory vODPaymentResultRepertory;
        public VODPaymentResultManager(IVODPaymentResultRepertory vODPaymentResultRepertory)
        {
            this.vODPaymentResultRepertory = vODPaymentResultRepertory;
        }
        public void Add(VODPaymentResult paymentResult)
        {
            vODPaymentResultRepertory.Insert(paymentResult);
        }
    }
}
