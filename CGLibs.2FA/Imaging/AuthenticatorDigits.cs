namespace CGLibs._2FA.Imaging
{
    public class AuthenticatorDigits
    {
        private AuthenticatorDigits(int digit)
        {
            _digits = digit;
        }

        int _digits = 6; //6 | 8 only allowed

        public static implicit operator int(AuthenticatorDigits authdigits)
        {
            return authdigits._digits;
        }

        public static implicit operator AuthenticatorDigits(int authdigits)
        {
            if (authdigits == 6 || authdigits == 8)
            {
                return new AuthenticatorDigits(authdigits);
            }
            else
            {
                return null;
            }
        }

        public static AuthenticatorDigits SIXDIGIT = new AuthenticatorDigits(6);

        public static AuthenticatorDigits EIGHTDIGIT = new AuthenticatorDigits(8);

    }
}
