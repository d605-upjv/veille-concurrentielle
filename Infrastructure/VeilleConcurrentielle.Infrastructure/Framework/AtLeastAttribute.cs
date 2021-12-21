using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace VeilleConcurrentielle.Infrastructure.Framework
{
    public class AtLeastAttribute : ValidationAttribute
    {
        private readonly int _minimum = 1;
        public AtLeastAttribute(int minimum)
        {
            _minimum = minimum;
        }
        public override bool IsValid(object? value)
        {
            var list = value as IList;
            if (list!=null && list.Count>= _minimum)
            {
                return true;
            }
            return false;
        }
    }
}
