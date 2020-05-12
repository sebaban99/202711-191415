using Type = IMMRequest.Domain.Type;

namespace IMMRequest.BusinessLogic.Interfaces
{
    public interface ITypeValidatorHelper
    {
        void ValidateAdd(Type type);

        void ValidateDelete(Type type);
    }
}
