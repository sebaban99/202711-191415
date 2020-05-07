using IMMRequest.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace IMMRequest.BusinessLogic
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
