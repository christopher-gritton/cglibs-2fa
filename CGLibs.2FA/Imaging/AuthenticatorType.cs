namespace CGLibs._2FA.Imaging
{
    public class AuthenticatorType
    {
        private AuthenticatorType(string OTPType)
        {
            _otptype = OTPType.ToLower();
        }

        private string _otptype = "totp";

        public override string ToString()
        {
            return _otptype;
        }

        public static implicit operator string( AuthenticatorType authtype)
        {
            return authtype._otptype;
        }

        public static implicit operator AuthenticatorType(string authtype)
        {
            if (authtype.Equals("totp", System.StringComparison.OrdinalIgnoreCase) || authtype.Equals("hotp", System.StringComparison.OrdinalIgnoreCase))
            {
                return new AuthenticatorType(authtype);
            }
            else
            {
                return null;
            }
        }

        public static AuthenticatorType HOTP = new AuthenticatorType("hotp");

        public static AuthenticatorType TOTP = new AuthenticatorType("totp");

    }
}
