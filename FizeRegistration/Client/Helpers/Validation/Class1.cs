using System.ComponentModel.DataAnnotations;

namespace FizeRegistration.Client.Helpers.Validation
{
    public class CustomAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value.GetType() == typeof(Foo))
            {
                Foo bar = (Foo)value;
                //compare the properties and return the result
            }
            throw new InvalidOperationException("This attribute is only valid for Foo objects");
        }
        [MetadataType(typeof(FooMD))]
        public partial class Foo
        {
     //... functions...
}
        [Custom]
        public class FooMD
        {
     //... other data annotations...
        }
    }

}
