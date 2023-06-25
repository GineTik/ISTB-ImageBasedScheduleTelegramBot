namespace ISTB.Framework.Attributes.ValidateInputDataAttributes.UpdateDataNotNull
{
    public class UpdateTextNotNullAttribute : UpdateDataNotNullAttribute
    {
        public UpdateTextNotNullAttribute() : base(update => update.Message?.Text)
        {
            ErrorMessage = "Text is null";
        }
    }
}
