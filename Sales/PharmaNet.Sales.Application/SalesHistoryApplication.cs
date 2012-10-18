using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PharmaNet.Sales.Domain;
using PharmaNet.Infrastructure.Repository;

namespace PharmaNet.Sales.Application
{
    public class SalesHistoryService
    {
        private IRepository<Member> _memberRepository;
        private IRepository<MeasurementPeriod> _measurementPeriodRepository;
        private IRepository<Product> _productRepository;
        private IRepository<SalesHistory> _salesHistoryRepository;

        public SalesHistoryService(
            IRepository<Member> memberRepository,
            IRepository<MeasurementPeriod> measurementPeriodRepository,
            IRepository<Product> productRepository,
            IRepository<SalesHistory> salesHistoryRepository)
        {
            _memberRepository = memberRepository;
            _measurementPeriodRepository = measurementPeriodRepository;
            _productRepository = productRepository;
            _salesHistoryRepository = salesHistoryRepository;
        }

        public void RecordSale(
            string customerName,
            DateTime saleDate,
            int productNumber,
            int quantity)
        {
            var member = _memberRepository.GetAll()
                .FirstOrDefault(m => m.Name == customerName);
            if (member == null)
                return;

            var product = _productRepository.GetAll()
                .Where(p => p.ProductNumber == productNumber)
                .Select(p => new { Product = p, p.Rebate })
                .FirstOrDefault();
            if (product == null || product.Rebate == null)
                return;
            var rebate = product.Rebate;

            DateTime startOfQuarter = new DateTime(
                saleDate.Year,
                ((saleDate.Month-1) / 3) * 3 + 1,
                1);
            var measurementPeriod = _measurementPeriodRepository.GetAll()
                .FirstOrDefault(mp => mp.StartDate == startOfQuarter);
            if (measurementPeriod == null)
            {
                measurementPeriod = _measurementPeriodRepository.Add(new MeasurementPeriod
                {
                    StartDate = startOfQuarter
                });
                _measurementPeriodRepository.SaveChanges();
            }

            var salesHistory = _salesHistoryRepository.GetAll()
                .FirstOrDefault(sh =>
                    sh.Member.Id == member.Id &&
                    sh.MeasurementPeriod.Id == measurementPeriod.Id &&
                    sh.Rebate.Id == rebate.Id);
            if (salesHistory == null)
            {
                salesHistory = _salesHistoryRepository.Add(new SalesHistory
                {
                    Member = member,
                    MeasurementPeriod = measurementPeriod,
                    Rebate = rebate
                });
                _salesHistoryRepository.SaveChanges();
            }
            if (salesHistory.Sales == null)
                salesHistory.Sales = new List<Sale>();
            salesHistory.Sales.Add(new Sale
            {
                Date = saleDate,
                Product = product.Product,
                Units = quantity
            });
            _salesHistoryRepository.SaveChanges();
        }
    }
}
