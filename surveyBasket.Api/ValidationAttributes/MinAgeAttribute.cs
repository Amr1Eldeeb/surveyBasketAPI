namespace surveyBasket.Api.ValidationAttributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]

    public class MinAgeAttribute(int MinAge) : ValidationAttribute
    {
        //public override bool IsValid(object? value)
        //{
        //    if (value is not null)
        //    {
        //        var date = (DateTime)value;
        //        if (DateTime.Today < date.AddYears(18)) 
        //         {
        //            return false;
        //        }
        //    }
        //        return true;
        //}
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is not null)
            {
                var date = (DateTime)value;

                // تحقق إذا كان المستخدم أصغر من السن المطلوب
                if (DateTime.Today < date.AddYears(MinAge))
                {
                    // نستخدم validationContext.DisplayName عشان نعرض اسم الحقل بشكل صحيح
                    return new ValidationResult($"Invalid {validationContext.DisplayName}. Minimum age is {MinAge}.");
                }
            }

            // لو القيمة صح أو فاضية، نرجع نجاح
            return ValidationResult.Success;
        }


    }
}
