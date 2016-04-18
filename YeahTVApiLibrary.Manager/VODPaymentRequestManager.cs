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

    public class VODPaymentRequestManager : IVODPaymentRequestManager
    {
        private IVODPaymentRequestRepertory vODPaymentRequestRepertory;
        public VODPaymentRequestManager(IVODPaymentRequestRepertory vODPaymentRequestRepertory)
        {
            this.vODPaymentRequestRepertory = vODPaymentRequestRepertory;
        }
        public void Add(VODPaymentRequest paymentRequest)
        {
            vODPaymentRequestRepertory.Insert(paymentRequest);
        }
    }
}
