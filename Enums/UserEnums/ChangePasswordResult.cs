using System;
using System.Collections.Generic;
using System.Text;

namespace Enums.UserEnums
{
    public enum ChangePasswordResult
    {
        AccountNotFound,
        WrongData,
        NewPasswordIsInTheIncorrectFormat,
        PasswordChagedSuccessfully
    }
}
