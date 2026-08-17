using System;
using System.Collections.Generic;
using System.Text;

namespace Enums
{
    public enum AuthPasswordResult
    {
        PasswordMustBeAtLeast_8CharacterLong,
        PassworMustHaveOne_UppercaseCharacter,
        PassworMustHaveOne_LowercaseCharacter,
        PassworMustHaveOne_NumericCharacter,
        PassworMustHaveOne_SpecialCharacter,
        PasswordIsInTheCorrectFormat
    }
}
