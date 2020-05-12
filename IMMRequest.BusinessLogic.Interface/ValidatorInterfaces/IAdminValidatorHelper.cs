using IMMRequest.Domain;

namespace IMMRequest.BusinessLogic.Interfaces
{
    public interface IAdminValidatorHelper
    {
        void ValidateAdd(Admin admin);
        void ValidateDelete(Admin admin);
        void ValidateUpdate(Admin admin);
        void ValidateAdminObject(Admin admin);
    }
}
