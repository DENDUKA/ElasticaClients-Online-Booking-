using ElasticaClients.DAL.Entities;
using ElasticaClients.Logic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ElasticaClients.Models
{
    public class FreezeSubscriptionModel : IValidatableObject
    {
        public int Id { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [DisplayName("От ")]
        public DateTime Start { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [DisplayName("До ")]
        public DateTime End { get; set; }
        [DisplayName("Описание ")]
        public string Description { get; set; }
        [Required]
        public int SubscriptionId { get; set; }

        public static explicit operator FreezeSubscriptionModel(FreezeSubscriptionItem s)
        {
            return new FreezeSubscriptionModel()
            {
                Id = s.Id,
                End = s.End,
                Start = s.Start,
                SubscriptionId = s.SubscriptionId,
                Description = s.Description,
            };
        }

        public static explicit operator FreezeSubscriptionItem(FreezeSubscriptionModel s)
        {
            return new FreezeSubscriptionItem()
            {
                Id = s.Id,
                End = s.End,
                Start = s.Start,
                SubscriptionId = s.SubscriptionId,
                Description = s.Description,
            };
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            var subscriptionB = System.Web.Mvc.DependencyResolver.Current.GetService(typeof(SubscriptionB)) as SubscriptionB;
            Subscription sub = subscriptionB?.Get(SubscriptionId);

            if (sub.BuyDate >= Start)
            {
                results.Add(new ValidationResult("День начала заморозки должен быть после даты покупки."));
            }
            //if (Start < DateTime.Today)
            //{
            //	results.Add(new ValidationResult("День начала заморозки не должен быть в прошлом. "));
            //}
            if (Start >= End)
            {
                results.Add(new ValidationResult("День окончания должен быть больше дня начала."));
            }
            else if ((End - Start).Days > 90)
            {
                results.Add(new ValidationResult("Заморозка не больше чем на 90 дней."));
            }

            return results;
        }
    }
}