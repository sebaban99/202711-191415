using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace IMMRequest.BusinessLogic
{
    public abstract class BaseValidator<T>
    {
        public abstract void ValidateAdd(T entity);

        public abstract void ValidateDelete(T entity);

        public abstract void ValidateUpdate(T entity);

        public abstract bool AreEmptyFields(T entity);

        public abstract void ValidateEntityObject(T entity);

        public bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
