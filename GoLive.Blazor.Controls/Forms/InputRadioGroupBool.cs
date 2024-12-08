using Microsoft.AspNetCore.Components.Forms;

namespace GoLive.Blazor.Controls.Forms;

public class InputRadioGroupBool<TValue> : InputRadioGroup<TValue>
{
    protected override bool TryParseValueFromString(string? value, out TValue result, out string? validationErrorMessage)
    {
        if (typeof(TValue) == typeof(bool))
        {
            if (bool.TryParse(value.ToLowerInvariant(), out bool val))
            {
                result = (TValue) (object) val;
                validationErrorMessage = null;
                return true;
            }
            else
            {
                result = default;
                validationErrorMessage = $"The selected value {value} is not a valid boolean.";
                return false;
            }
        }
        else
        {
            return base.TryParseValueFromString(value, out result, out validationErrorMessage);
        }
    }

}