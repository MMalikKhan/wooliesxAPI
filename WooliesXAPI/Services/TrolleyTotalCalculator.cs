using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using WooliesXAPI.DataContracts;

namespace WooliesXAPI.Services
{

    public interface ITrolleyTotalCalculator
    {
        decimal CalculateLowestTrolleyTotal(GetTrolleyTotalRequest request);

    }

    public class TrolleyTotalCalculator : ITrolleyTotalCalculator
    {
        private readonly ILogger<TrolleyTotalCalculator> _logger;

        public TrolleyTotalCalculator(ILogger<TrolleyTotalCalculator> logger)
        {
            _logger = logger;
        }

        public decimal CalculateLowestTrolleyTotal(GetTrolleyTotalRequest request)
        {
            var allProducts = request.Products;
            var allQuantities = request.Quantities;
            var trolleyTotalList = new List<decimal>();
            decimal total = 0;
            //calculate total without any promotions
            total = CalculatureTotalPrice(allQuantities, allProducts, total);
            trolleyTotalList.Add(total);

            // iterate specials and calculate new trolley total based on special pricing
            foreach (var specialItem in request.Specials)
            {
                //check if specific special applies to the list
                if (CheckIfThisSpecialApply(specialItem, allQuantities))
                {
                    var specialItemTotal = specialItem.Total;
                    var remainingItemsList = GetRemainingQuantityList(specialItem, allQuantities);
                    //check if special can be applied multiple times
                    while (CheckIfThisSpecialApply(specialItem, remainingItemsList))
                    {
                        remainingItemsList = GetRemainingQuantityList(specialItem, remainingItemsList);
                        specialItemTotal = specialItemTotal + specialItem.Total;
                    }

                    specialItemTotal = CalculatureTotalPrice(remainingItemsList, allProducts, specialItemTotal);
                    trolleyTotalList.Add(specialItemTotal);
                    _logger.LogTrace($"special total applied specialTotal {specialItem.Total} newTotal {specialItemTotal}");
                }
            }

            return trolleyTotalList.OrderBy(c => c).FirstOrDefault();
        }

        /// <summary>
        /// Calculate items total for given quantity an product list
        /// </summary>
        /// <param name="quantities"></param>
        /// <param name="products"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        private static decimal CalculatureTotalPrice(List<Quantity> quantities, List<ProductPrice> products,
            decimal total)
        {
            foreach (var itemsQuantity in quantities)
            {
                var purchasedItem = products.FirstOrDefault(p => p.Name == itemsQuantity.Name);
                if (purchasedItem == null)
                    continue;
                total = total + purchasedItem.Price * itemsQuantity.QuantityQuantity;
            }

            return total;
        }

        /// <summary>
        /// Check if a special apply to given purchase quantities
        /// </summary>
        /// <param name="special"></param>
        /// <param name="purchaseItemQuantityList"></param>
        /// <returns></returns>
        private bool CheckIfThisSpecialApply(Special special, List<Quantity> purchaseItemQuantityList)
        {
            foreach (var purchaseItemQuantity in purchaseItemQuantityList)
            {
                var isThisSpecialValid = special.Quantities.Any(c =>
                    c.Name == purchaseItemQuantity.Name && c.QuantityQuantity > purchaseItemQuantity.QuantityQuantity);
                if (isThisSpecialValid)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// This gives remaining quantities after applying special pricing
        /// </summary>
        /// <param name="special"></param>
        /// <param name="quantities"></param>
        /// <returns></returns>
        private List<Quantity> GetRemainingQuantityList(Special special, List<Quantity> quantities)
        {
            return quantities.Select(x => new Quantity
            {
                Name = x.Name,
                QuantityQuantity =
                    x.QuantityQuantity - special.Quantities.FirstOrDefault(q => q.Name == x.Name)?.QuantityQuantity ?? 0
            }).ToList();
        }
    }
}
