using IMMRequest.Domain;

namespace IMMRequest.BusinessLogic.Interfaces
{
    public interface IRequestValidatorHelper
    {
        void ValidateRequestObject(Request request);
        void ValidateAdd(Request request);
        void ValidateUpdate(Request request, Request Request);
        void ValidateType(Request request);
        void ValidateAFValues(Request request);
    }
}
